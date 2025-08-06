using Microsoft.EntityFrameworkCore;
using cinema_tickets.Models;

namespace cinema_tickets.Data;

public class CinemaDbContext : DbContext
{
    public CinemaDbContext(DbContextOptions<CinemaDbContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Showtime> Showtimes { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Showtime>()
            .HasOne(s => s.Movie)
            .WithMany(m => m.Showtimes)
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Showtime)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.ShowtimeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Movies
        modelBuilder.Entity<Movie>().HasData(
            new Movie
            {
                Id = 1,
                Title = "The Matrix",
                Description = "A computer programmer discovers a mysterious world.",
                Duration = 136,
                Genre = "Sci-Fi",
                Rating = "R",
                ReleaseDate = new DateTime(1999, 3, 31)
            },
            new Movie
            {
                Id = 2,
                Title = "Inception",
                Description = "A thief who steals corporate secrets through dream-sharing technology.",
                Duration = 148,
                Genre = "Sci-Fi",
                Rating = "PG-13",
                ReleaseDate = new DateTime(2010, 7, 16)
            },
            new Movie
            {
                Id = 3,
                Title = "The Dark Knight",
                Description = "Batman faces his greatest challenge yet.",
                Duration = 152,
                Genre = "Action",
                Rating = "PG-13",
                ReleaseDate = new DateTime(2008, 7, 18)
            }
        );

        // Seed Showtimes
        modelBuilder.Entity<Showtime>().HasData(
            new Showtime
            {
                Id = 1,
                MovieId = 1,
                StartTime = new DateTime(2025, 8, 5, 19, 0, 0),
                EndTime = new DateTime(2025, 8, 5, 21, 16, 0),
                Hall = "Hall A",
                Price = 12.99m,
                TotalSeats = 100,
                AvailableSeats = 100
            },
            new Showtime
            {
                Id = 2,
                MovieId = 2,
                StartTime = new DateTime(2025, 8, 5, 21, 0, 0),
                EndTime = new DateTime(2025, 8, 5, 23, 28, 0),
                Hall = "Hall B",
                Price = 14.99m,
                TotalSeats = 80,
                AvailableSeats = 80
            },
            new Showtime
            {
                Id = 3,
                MovieId = 3,
                StartTime = new DateTime(2025, 8, 6, 20, 0, 0),
                EndTime = new DateTime(2025, 8, 6, 22, 32, 0),
                Hall = "Hall A",
                Price = 13.99m,
                TotalSeats = 100,
                AvailableSeats = 100
            }
        );
    }
} 