using cinema_tickets.DTOs;

namespace cinema_tickets.Services;

public interface ITicketService
{
    Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
    Task<TicketDto?> GetTicketByIdAsync(int id);
    Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto);
    Task<TicketDto> UpdateTicketAsync(int id, UpdateTicketDto updateTicketDto);
    Task<bool> DeleteTicketAsync(int id);
    Task<IEnumerable<TicketDto>> GetTicketsByShowtimeAsync(int showtimeId);
} 