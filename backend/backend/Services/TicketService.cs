using cinema_tickets.Data;
using cinema_tickets.DTOs;
using cinema_tickets.Models;
using Microsoft.EntityFrameworkCore;

namespace cinema_tickets.Services;

public class TicketService : ITicketService
{
    private readonly CinemaDbContext _context;

    public TicketService(CinemaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
    {
        var tickets = await _context.Tickets
            .Include(t => t.Showtime)
            .ThenInclude(s => s.Movie)
            .OrderByDescending(t => t.PurchaseDate)
            .ToListAsync();

        return tickets.Select(t => new TicketDto
        {
            Id = t.Id,
            ShowtimeId = t.ShowtimeId,
            CustomerName = t.CustomerName,
            CustomerEmail = t.CustomerEmail,
            PhoneNumber = t.PhoneNumber,
            SeatNumber = t.SeatNumber,
            Price = t.Price,
            PurchaseDate = t.PurchaseDate,
            Status = t.Status,
            MovieTitle = t.Showtime.Movie.Title,
            ShowtimeStartTime = t.Showtime.StartTime,
            Hall = t.Showtime.Hall
        });
    }

    public async Task<TicketDto?> GetTicketByIdAsync(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Showtime)
            .ThenInclude(s => s.Movie)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) return null;

        return new TicketDto
        {
            Id = ticket.Id,
            ShowtimeId = ticket.ShowtimeId,
            CustomerName = ticket.CustomerName,
            CustomerEmail = ticket.CustomerEmail,
            PhoneNumber = ticket.PhoneNumber,
            SeatNumber = ticket.SeatNumber,
            Price = ticket.Price,
            PurchaseDate = ticket.PurchaseDate,
            Status = ticket.Status,
            MovieTitle = ticket.Showtime.Movie.Title,
            ShowtimeStartTime = ticket.Showtime.StartTime,
            Hall = ticket.Showtime.Hall
        };
    }

    public async Task<TicketDto> CreateTicketAsync(CreateTicketDto createTicketDto)
    {
        // Validate showtime exists
        var showtime = await _context.Showtimes
            .Include(s => s.Movie)
            .FirstOrDefaultAsync(s => s.Id == createTicketDto.ShowtimeId);

        if (showtime == null)
            throw new ArgumentException("Showtime not found");

        // Check if seat is available
        var seatTaken = await _context.Tickets
            .AnyAsync(t => t.ShowtimeId == createTicketDto.ShowtimeId && 
                          t.SeatNumber == createTicketDto.SeatNumber &&
                          t.Status == "Active");

        if (seatTaken)
            throw new InvalidOperationException("Seat is already taken");

        // Check if showtime has available seats
        if (showtime.AvailableSeats <= 0)
            throw new InvalidOperationException("No available seats for this showtime");

        var ticket = new Ticket
        {
            ShowtimeId = createTicketDto.ShowtimeId,
            CustomerName = createTicketDto.CustomerName,
            CustomerEmail = createTicketDto.CustomerEmail,
            PhoneNumber = createTicketDto.PhoneNumber,
            SeatNumber = createTicketDto.SeatNumber,
            Price = showtime.Price,
            PurchaseDate = DateTime.UtcNow,
            Status = "Active"
        };

        _context.Tickets.Add(ticket);
        
        // Update available seats
        showtime.AvailableSeats--;
        
        await _context.SaveChangesAsync();

        return new TicketDto
        {
            Id = ticket.Id,
            ShowtimeId = ticket.ShowtimeId,
            CustomerName = ticket.CustomerName,
            CustomerEmail = ticket.CustomerEmail,
            PhoneNumber = ticket.PhoneNumber,
            SeatNumber = ticket.SeatNumber,
            Price = ticket.Price,
            PurchaseDate = ticket.PurchaseDate,
            Status = ticket.Status,
            MovieTitle = showtime.Movie.Title,
            ShowtimeStartTime = showtime.StartTime,
            Hall = showtime.Hall
        };
    }

    public async Task<TicketDto> UpdateTicketAsync(int id, UpdateTicketDto updateTicketDto)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Showtime)
            .ThenInclude(s => s.Movie)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new ArgumentException("Ticket not found");

        // Check if new seat is available (if seat number changed)
        if (ticket.SeatNumber != updateTicketDto.SeatNumber)
        {
            var seatTaken = await _context.Tickets
                .AnyAsync(t => t.ShowtimeId == ticket.ShowtimeId && 
                              t.SeatNumber == updateTicketDto.SeatNumber &&
                              t.Status == "Active" &&
                              t.Id != id);

            if (seatTaken)
                throw new InvalidOperationException("Seat is already taken");
        }

        ticket.CustomerName = updateTicketDto.CustomerName;
        ticket.CustomerEmail = updateTicketDto.CustomerEmail;
        ticket.PhoneNumber = updateTicketDto.PhoneNumber;
        ticket.SeatNumber = updateTicketDto.SeatNumber;
        ticket.Status = updateTicketDto.Status;

        await _context.SaveChangesAsync();

        return new TicketDto
        {
            Id = ticket.Id,
            ShowtimeId = ticket.ShowtimeId,
            CustomerName = ticket.CustomerName,
            CustomerEmail = ticket.CustomerEmail,
            PhoneNumber = ticket.PhoneNumber,
            SeatNumber = ticket.SeatNumber,
            Price = ticket.Price,
            PurchaseDate = ticket.PurchaseDate,
            Status = ticket.Status,
            MovieTitle = ticket.Showtime.Movie.Title,
            ShowtimeStartTime = ticket.Showtime.StartTime,
            Hall = ticket.Showtime.Hall
        };
    }

    public async Task<bool> DeleteTicketAsync(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Showtime)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) return false;

        // If ticket is active, increase available seats
        if (ticket.Status == "Active")
        {
            ticket.Showtime.AvailableSeats++;
        }

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TicketDto>> GetTicketsByShowtimeAsync(int showtimeId)
    {
        var tickets = await _context.Tickets
            .Include(t => t.Showtime)
            .ThenInclude(s => s.Movie)
            .Where(t => t.ShowtimeId == showtimeId)
            .OrderBy(t => t.SeatNumber)
            .ToListAsync();

        return tickets.Select(t => new TicketDto
        {
            Id = t.Id,
            ShowtimeId = t.ShowtimeId,
            CustomerName = t.CustomerName,
            CustomerEmail = t.CustomerEmail,
            PhoneNumber = t.PhoneNumber,
            SeatNumber = t.SeatNumber,
            Price = t.Price,
            PurchaseDate = t.PurchaseDate,
            Status = t.Status,
            MovieTitle = t.Showtime.Movie.Title,
            ShowtimeStartTime = t.Showtime.StartTime,
            Hall = t.Showtime.Hall
        });
    }
} 