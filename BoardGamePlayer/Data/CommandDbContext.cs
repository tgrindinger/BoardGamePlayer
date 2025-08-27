using BoardGamePlayer.Domain;
using Microsoft.EntityFrameworkCore;

namespace BoardGamePlayer.Data;

public class CommandDbContext(DbContextOptions<CommandDbContext> options)
    : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<User> Users => Set<User>();
}
