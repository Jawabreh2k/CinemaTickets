using Microsoft.AspNetCore.Mvc;
using cinema_tickets.DTOs;
using cinema_tickets.Services;

namespace cinema_tickets.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowtimesController : ControllerBase
{
    private readonly IShowtimeService _showtimeService;

    public ShowtimesController(IShowtimeService showtimeService)
    {
        _showtimeService = showtimeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShowtimeDto>>> GetShowtimes()
    {
        var showtimes = await _showtimeService.GetAllShowtimesAsync();
        return Ok(showtimes);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<ShowtimeDto>>> GetAvailableShowtimes()
    {
        var showtimes = await _showtimeService.GetAvailableShowtimesAsync();
        return Ok(showtimes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShowtimeDto>> GetShowtime(int id)
    {
        var showtime = await _showtimeService.GetShowtimeByIdAsync(id);
        if (showtime == null)
            return NotFound();

        return Ok(showtime);
    }
} 