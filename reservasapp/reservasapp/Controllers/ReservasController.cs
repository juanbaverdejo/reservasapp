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
    [HttpPost]
    public async Task<IActionResult> CreateReserva([FromBody] ReservaRequest reservaRequest)
    {
        if (string.IsNullOrWhiteSpace(reservaRequest.Cliente))
            return BadRequest("El nombre del cliente es requerido.");

        var servicio = await _context.Servicio.FindAsync(reservaRequest.ServicioId);
        if (servicio == null)
            return BadRequest("El servicio seleccionado no existe.");

        var horarioTrabajo = await _context.HorarioTrabajo
            .FirstOrDefaultAsync(h => h.Dia == reservaRequest.Fecha.DayOfWeek);
        if (horarioTrabajo == null)
            return BadRequest("No hay horarios de trabajo definidos para el día seleccionado.");

        // Validación: que el turno esté dentro del rango permitido
        if (reservaRequest.Turno < horarioTrabajo.HoraInicio || reservaRequest.Turno > horarioTrabajo.HoraFin)
            return BadRequest($"El turno debe estar entre {horarioTrabajo.HoraInicio} y {horarioTrabajo.HoraFin}.");

        // Validación: no puede existir una reserva para la misma fecha y turno
        var reservaExistente = await _context.Reserva
            .FirstOrDefaultAsync(r => r.Fecha.Date == reservaRequest.Fecha.Date &&
                                      r.Turno == reservaRequest.Turno);
        if (reservaExistente != null)
            return BadRequest("Ya existe una reserva para esa fecha y turno.");

        // Validación: el mismo cliente no puede tener 2 reservas en el mismo día
        var reservaMismaFecha = await _context.Reserva
            .FirstOrDefaultAsync(r => r.Cliente.ToLower() == reservaRequest.Cliente.ToLower() &&
                                      r.Fecha.Date == reservaRequest.Fecha.Date);
        if (reservaMismaFecha != null)
            return BadRequest("El cliente ya tiene una reserva para este día.");

        var nuevaReserva = new Reserva
        {
            Cliente = reservaRequest.Cliente,
            ServicioId = reservaRequest.ServicioId,
            Fecha = reservaRequest.Fecha.Date,
            Turno = reservaRequest.Turno
        };

        _context.Reserva.Add(nuevaReserva);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Reserva creada exitosamente", reservaId = nuevaReserva.Id });
    }

}
