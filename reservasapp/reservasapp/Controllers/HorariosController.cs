using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using reservasapp.datos;

[ApiController]
[Route("api/[controller]")]
public class HorariosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HorariosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("disponibles")]
    public async Task<IActionResult> GetHorariosDisponibles([FromQuery] DateTime fecha)
    {
        var horarioTrabajo = await _context.HorarioTrabajo
            .FirstOrDefaultAsync(h => h.Dia == fecha.DayOfWeek);

        if (horarioTrabajo == null)
            return NotFound("No hay horarios de trabajo definidos para este día.");

        // Obtener los turnos reservados para esa fecha
        var turnosReservados = await _context.Reserva
            .Where(r => r.Fecha.Date == fecha.Date)
            .Select(r => r.Turno)
            .ToListAsync();

        List<int> turnosDisponibles = new List<int>();
        for (int turno = horarioTrabajo.HoraInicio; turno <= horarioTrabajo.HoraFin; turno++)
        {
            if (!turnosReservados.Contains(turno))
                turnosDisponibles.Add(turno);
        }

        return Ok(turnosDisponibles);
    }

}
