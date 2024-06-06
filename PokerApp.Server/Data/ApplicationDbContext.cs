namespace PokerApp.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PokerApp.Server.Models;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GamePlayer> GamePlayers { get; set; }
    public DbSet<Hand> Hands { get; set; }
    public DbSet<CommunityCards> CommunityCards { get; set; }
    public DbSet<Round> Rounds { get; set; }
    public DbSet<Bet> Bets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships or additional model properties here
        // Example:
        // modelBuilder.Entity<User>()
        //     .HasMany(u => u.Games)
        //     .WithOne(g => g.User)
        //     .HasForeignKey(g => g.UserID);
    }
}

