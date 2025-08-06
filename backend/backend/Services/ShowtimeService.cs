using cinema_tickets.Data;
using cinema_tickets.DTOs;
using Microsoft.EntityFrameworkCore;

namespace cinema_tickets.Services;

public class ShowtimeService : IShowtimeService
{
    private readonly CinemaDbContext _context;

    public ShowtimeService(CinemaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShowtimeDto>> GetAllShowtimesAsync()
    {
        var showtimes = await _context.Showtimes
            .Include(s => s.Movie)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        return showtimes.Select(s => new ShowtimeDto
        {
            Id = s.Id,
            MovieId = s.MovieId,
            MovieTitle = s.Movie.Title,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Hall = s.Hall,
            Price = s.Price,
            TotalSeats = s.TotalSeats,
            AvailableSeats = s.AvailableSeats
        });
    }

    public async Task<ShowtimeDto?> GetShowtimeByIdAsync(int id)
    {
        var showtime = await _context.Showtimes
            .Include(s => s.Movie)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (showtime == null) return null;

        return new ShowtimeDto
        {
            Id = showtime.Id,
            MovieId = showtime.MovieId,
            MovieTitle = showtime.Movie.Title,
            StartTime = showtime.StartTime,
            EndTime = showtime.EndTime,
            Hall = showtime.Hall,
            Price = showtime.Price,
            TotalSeats = showtime.TotalSeats,
            AvailableSeats = showtime.AvailableSeats
        };
    }

    public async Task<IEnumerable<ShowtimeDto>> GetAvailableShowtimesAsync()
    {
        var showtimes = await _context.Showtimes
            .Include(s => s.Movie)
            .Where(s => s.StartTime > DateTime.Now && s.AvailableSeats > 0)
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        return showtimes.Select(s => new ShowtimeDto
        {
            Id = s.Id,
            MovieId = s.MovieId,
            MovieTitle = s.Movie.Title,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Hall = s.Hall,
            Price = s.Price,
            TotalSeats = s.TotalSeats,
            AvailableSeats = s.AvailableSeats
        });
    }
} 