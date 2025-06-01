using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using System.Reflection;
using Xunit;

namespace BoardGamePlayer.Infrastructure;

public class SliceIsolationTests
{
    [Fact]
    public async Task GivenIHaveSlicesDefined_WhenIVerifyTheyAreIsolated_ThenIFindNoOverlaps()
    {
        // arrange
        MSBuildLocator.RegisterDefaults();
        using var workspace = MSBuildWorkspace.Create();
        var filePath = FindFile($"{nameof(BoardGamePlayer)}.sln");
        var solution = await workspace.OpenSolutionAsync(filePath);
        var featureTypes = await GetFeatureTypes(solution);

        // act
        var violations = await VerifyFeaturesAreIsolated(solution, featureTypes);

        // assert
        Assert.Empty(violations);
    }

    private static string FindFile(string filename)
    {
        var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var directory = new DirectoryInfo(currentPath!);
        while (directory != null && directory.GetFiles(filename).Length == 0)
        {
            directory = directory.Parent;
        }
        return directory?.GetFiles(filename).FirstOrDefault()?.FullName!;
    }

    private static async Task<List<INamedTypeSymbol>> GetFeatureTypes(Solution solution)
    {
        var featureTypes = new List<INamedTypeSymbol>();
        foreach (var project in solution.Projects)
        {
            var compilation = await project.GetCompilationAsync();
            var featureNamespace = compilation?.GlobalNamespace
                .GetNamespaceMembers()
                .FirstOrDefault(n => n.Name == nameof(BoardGamePlayer))?
                .GetNamespaceMembers()
                .FirstOrDefault(n => n.Name == nameof(Features));

            if (featureNamespace == null) continue;

            var handlerNamespaces = featureNamespace.GetNamespaceMembers()
                .SelectMany(n => n.GetNamespaceMembers())
                .Append(featureNamespace);

            foreach (var _namespace in handlerNamespaces)
            {
                foreach (var type in _namespace.GetTypeMembers()
                    .Where(n => !n.Name.Contains("Tests")))
                {
                    featureTypes.Add(type);
                }
            }
        }
        return featureTypes;
    }

    private static async Task<List<string>> VerifyFeaturesAreIsolated(Solution solution, List<INamedTypeSymbol> featureTypes)
    {
        var violations = new List<string>();
        foreach (var type in featureTypes)
        {
            var references = await SymbolFinder.FindReferencesAsync(type, solution);
            foreach (var reference in references)
            {
                foreach (var location in reference.Locations)
                {
                    await VerifyFeatureIsIsolated(solution, violations, type, location);
                }
            }
        }

        return violations;
    }

    private static async Task VerifyFeatureIsIsolated(Solution solution, List<string> violations, INamedTypeSymbol type, ReferenceLocation location)
    {
        // Get the document containing the reference
        var document = solution.GetDocument(location.Location.SourceTree);
        if (document == null || document.Name.Contains("Tests")) return;

        // Get SemanticModel for this document
        var semanticModel = await document.GetSemanticModelAsync();
        var root = await location.Location.SourceTree?.GetRootAsync()!;
        var node = root.FindNode(location.Location.SourceSpan);

        // Find the nearest containing type symbol
        var containingType = node.FirstAncestorOrSelf<TypeDeclarationSyntax>();
        if (containingType == null) return;

        var referencingType = semanticModel?.GetDeclaredSymbol(containingType);
        if (referencingType?.ContainingNamespace?.ToString()?.Contains(nameof(Features)) == true)
        {
            var currentSlice = GetSliceName(referencingType!);
            var referencedSlice = GetSliceName(type);

            if (currentSlice != referencedSlice)
            {
                violations.Add(
                    $"{referencingType} ({currentSlice}) -> {type} ({referencedSlice}) " +
                    $"at {location.Location.GetLineSpan().Path}:{location.Location.GetLineSpan().StartLinePosition.Line}");
            }
        }
    }

    private static string GetSliceName(ISymbol symbol)
        => symbol.ContainingNamespace?.ToString()?.Split('.')[2]!;
}
