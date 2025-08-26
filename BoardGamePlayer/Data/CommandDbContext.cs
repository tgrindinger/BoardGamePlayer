using BoardGamePlayer.Domain;
using Microsoft.EntityFrameworkCore;

namespace BoardGamePlayer.Data;

public class CommandDbContext : DbContext
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<User> Users => Set<User>();

    public CommandDbContext(DbContextOptions<CommandDbContext> options) : base(options) { }
}
