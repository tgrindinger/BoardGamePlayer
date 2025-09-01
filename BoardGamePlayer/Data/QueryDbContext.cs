using BoardGamePlayer.Domain;
using BoardGamePlayer.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BoardGamePlayer.Data;

public class QueryDbContext(DbContextOptions<QueryDbContext> options)
    : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Player> Players => Set<Player>();

    public override int SaveChanges()
    {
        throw new QueryWriteException("Cannot save changes to a query context");
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        throw new QueryWriteException("Cannot save changes to a query context");
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        throw new QueryWriteException("Cannot save changes to a query context");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new QueryWriteException("Cannot save changes to a query context");
    }
}
