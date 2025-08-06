using Microsoft.AspNetCore.Mvc;
using cinema_tickets.DTOs;
using cinema_tickets.Services;

namespace cinema_tickets.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
    {
        var tickets = await _ticketService.GetAllTicketsAsync();
        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDto>> GetTicket(int id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket == null)
            return NotFound();

        return Ok(ticket);
    }

    [HttpGet("showtime/{showtimeId}")]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTicketsByShowtime(int showtimeId)
    {
        var tickets = await _ticketService.GetTicketsByShowtimeAsync(showtimeId);
        return Ok(tickets);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto createTicketDto)
    {
        try
        {
            var ticket = await _ticketService.CreateTicketAsync(createTicketDto);
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TicketDto>> UpdateTicket(int id, UpdateTicketDto updateTicketDto)
    {
        try
        {
            var ticket = await _ticketService.UpdateTicketAsync(id, updateTicketDto);
            return Ok(ticket);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTicket(int id)
    {
        var result = await _ticketService.DeleteTicketAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
} 