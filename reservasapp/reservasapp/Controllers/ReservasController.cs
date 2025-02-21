using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using reservasapp.Models;
using System;
using reservasapp.datos;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReservasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
    {
        return await _context.Reserva.ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CreateReserva([FromBody] Reserva reserva)
    {
        if (string.IsNullOrWhiteSpace(reserva.Cliente))
            return BadRequest("El nombre del cliente es requerido.");

        if (reserva.Fecha == default || reserva.Horario == default)
            return BadRequest("La fecha y el horario son requeridos.");

        var reservaExistente = await _context.Reserva
            .FirstOrDefaultAsync(r => r.Fecha.Date == reserva.Fecha.Date && r.Horario == reserva.Horario);
        if (reservaExistente != null)
            return BadRequest("Ya existe una reserva para esa fecha y horario.");

        var reservaMismaFecha = await _context.Reserva
            .FirstOrDefaultAsync(r => r.Cliente.ToLower() == reserva.Cliente.ToLower() && r.Fecha.Date == reserva.Fecha.Date);
        if (reservaMismaFecha != null)
            return BadRequest("El cliente ya tiene una reserva para este día.");

        _context.Reserva.Add(reserva);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Reserva creada exitosamente", reservaId = reserva });
    }
}
