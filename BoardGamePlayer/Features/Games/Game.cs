using Microsoft.EntityFrameworkCore;

namespace BoardGamePlayer.Features.Games;

public class Game
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required Guid UserId { get; set; }
    public required GameStatus GameStatus { get; set; }
}

public class GameAppDbContext : DbContext
{
    public DbSet<Game> Games => Set<Game>();

    public GameAppDbContext(DbContextOptions<GameAppDbContext> options) : base(options)
    {
    }
}
