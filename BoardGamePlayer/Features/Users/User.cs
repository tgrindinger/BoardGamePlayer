using Microsoft.EntityFrameworkCore;

namespace BoardGamePlayer.Features.Users;

public class User
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required List<Guid> GameIds { get; set; }
}

public class UserAppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    public UserAppDbContext(DbContextOptions<UserAppDbContext> options) : base(options)
    {
    }
}
