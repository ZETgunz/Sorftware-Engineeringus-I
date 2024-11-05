using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Enums;
using backend.DTOs.Account;

namespace backend.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AccountDTO> AccountDTOs { get; set; }

    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<AccountDTO>().HasKey(a => a.Username);

        modelBuilder.Entity<Game>().HasKey(g => g.Id);

        modelBuilder.Entity<Game>().ToTable("Games");



        modelBuilder.Entity<AccountDTO>().HasData(
            new AccountDTO { Username = "admin", Password = "admin", role = Role.User, score = 10 },
            new AccountDTO { Username = "user", Password = "user", role = Role.Admin, score = 20 }
        );

        modelBuilder.Entity<Game>().HasData(
            new Game
            {
                Id = 1,
                Name = "Melon Ninja",
                Description = "Slice as many Melons as possible",
                Route = "/melonNinja"
            },
            new Game
            {
                Id = 2,
                Name = "Sequence",
                Description = "Remember the sequence and repeat it",
                Route = "/sequence"
            }
        );
    }
}