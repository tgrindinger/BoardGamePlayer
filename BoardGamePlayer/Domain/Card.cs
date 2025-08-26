namespace BoardGamePlayer.Domain;

public record Card(
    string Name,
    CardType CardType,
    bool IsMilitary,
    bool IsRebel,
    int EyeCount,
    int GeneCount,
    int MilitaryPower,
    int Cost);

public static class DeckBuilder
{
    public static IEnumerable<Card> Build()
    {
        var deck = new List<Card>();
        deck.AddRange(BuildDevelopments());
        deck.AddRange(BuildPlanets());
        // todo: shuffle
        return deck;
    }

    public static IEnumerable<Card> BuildDevelopments()
    {
        var deck = new List<Card>();
        deck.AddRange(Enumerable.Repeat(tradePact, 5));
        deck.AddRange(Enumerable.Repeat(colonyConvoy, 4));
        deck.AddRange(Enumerable.Repeat(contactSpecialist, 3));
        deck.AddRange(Enumerable.Repeat(investmentCredits, 3));
        deck.AddRange(Enumerable.Repeat(geneticsLab, 2));
        deck.AddRange(Enumerable.Repeat(militaryConvoy, 2));
        deck.AddRange(Enumerable.Repeat(miningRobots, 2));
        deck.AddRange(Enumerable.Repeat(spaceMarines, 4));
        deck.AddRange(Enumerable.Repeat(consumerMarkets, 3));
        deck.AddRange(Enumerable.Repeat(warPropaganda, 2));
        deck.AddRange(Enumerable.Repeat(galacticAdvertisers, 3));
        deck.AddRange(Enumerable.Repeat(miningConglomerate, 2));
        deck.AddRange(Enumerable.Repeat(replicantRobots, 2));
        deck.AddRange(Enumerable.Repeat(upliftResearchers, 3));
        deck.AddRange(Enumerable.Repeat(dropShips, 2));
        deck.AddRange(Enumerable.Repeat(galacticSalon, 2));
        deck.Add(alienTechnologyInstitute);
        deck.Add(freeTradeAssociation);
        deck.Add(newGalacticOrder);
        deck.Add(miningLeague);
        deck.Add(galacticImperium);
        deck.Add(upliftCode);
        deck.Add(galacticFederation);
        deck.Add(galacticSurveySeti);
        return deck;
    }

    public static IEnumerable<Card> BuildPlanets()
    {
        var deck = new List<Card>();
        // todo: define planets
        return deck;
    }

    // todo: turn these into objects
    private readonly static Card tradePact = new(
        Name: "Trade Pact",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 1
    );

    private readonly static Card colonyConvoy = new(
        Name: "Colony Convoy",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 1
    );

    private readonly static Card contactSpecialist = new(
        Name: "Contact Specialist",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 1
    );

    private readonly static Card investmentCredits = new(
        Name: "Investment Credits",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 1
    );

    private readonly static Card geneticsLab = new(
        Name: "Genetics Lab",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 1
    );

    private readonly static Card militaryConvoy = new(
        Name: "Military Convoy",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 1,
        Cost: 1
    );

    private readonly static Card miningRobots = new(
        Name: "Mining Robots",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 1
    );

    private readonly static Card spaceMarines = new(
        Name: "Space Marines",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 2,
        Cost: 2
    );

    private readonly static Card consumerMarkets = new(
        Name: "Consumer Markets",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 2
    );

    private readonly static Card warPropaganda = new(
        Name: "War Propaganda",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 1,
        Cost: 2
    );

    private readonly static Card galacticAdvertisers = new(
        Name: "Galactic Advertisers",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 3
    );

    private readonly static Card miningConglomerate = new(
        Name: "Mining Conglomerate",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 1,
        Cost: 3
    );

    private readonly static Card replicantRobots = new(
        Name: "Replicant Robots",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 3
    );

    private readonly static Card upliftResearchers = new(
        Name: "Uplift Researchers",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 1,
        MilitaryPower: 0,
        Cost: 4
    );

    private readonly static Card dropShips = new(
        Name: "Drop Ships",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 3,
        Cost: 4
    );

    private readonly static Card galacticSalon = new(
        Name: "Galactic Salon",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 2,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 6
    );

    private readonly static Card alienTechnologyInstitute = new(
        Name: "Alien Technology Institute",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 7
    );

    private readonly static Card freeTradeAssociation = new(
        Name: "Free Trade Association",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 7
    );

    private readonly static Card newGalacticOrder = new(
        Name: "New Galactic Order",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 2,
        Cost: 8
    );

    private readonly static Card miningLeague = new(
        Name: "Mining League",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 8
    );

    private readonly static Card galacticImperium = new(
        Name: "Galactic Imperium",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 1,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 8
    );

    private readonly static Card upliftCode = new(
        Name: "Uplift Code",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 2,
        MilitaryPower: 0,
        Cost: 9
    );

    private readonly static Card galacticFederation = new(
        Name: "Galactic Federation",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 0,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 9
    );

    private readonly static Card galacticSurveySeti = new(
        Name: "Galactic Survey: Seti",
        CardType: CardType.Development,
        IsMilitary: false,
        IsRebel: false,
        EyeCount: 3,
        GeneCount: 0,
        MilitaryPower: 0,
        Cost: 9
    );
}
