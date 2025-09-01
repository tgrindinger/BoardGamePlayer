using BoardGamePlayer.Infrastructure;
using BoardGamePlayer.Features.Games;
using BoardGamePlayer.Features.Users;
using BoardGamePlayer.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterDependencies();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<CommandDbContext>();
    await context!.Database.EnsureCreatedAsync();
    context.DumpSchema();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BoardGamePlayer v1");
    });
}

app.UseHttpsRedirection();

app.MapGameEndpoints();
app.MapUserEndpoints();

await app.RunAsync();
