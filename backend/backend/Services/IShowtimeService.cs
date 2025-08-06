using cinema_tickets.DTOs;

namespace cinema_tickets.Services;

public interface IShowtimeService
{
    Task<IEnumerable<ShowtimeDto>> GetAllShowtimesAsync();
    Task<ShowtimeDto?> GetShowtimeByIdAsync(int id);
    Task<IEnumerable<ShowtimeDto>> GetAvailableShowtimesAsync();
} 