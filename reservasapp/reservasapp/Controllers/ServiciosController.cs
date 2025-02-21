using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservasapp.datos;
using reservasapp.Models;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ServiciosController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ServiciosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Servicio>>> GetServicios()
    {
        try
        {
            var servicios = await _context.Servicio.ToListAsync();
            Console.WriteLine("Servicios obtenidos correctamente:", servicios);
            return servicios;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error al obtener servicios:", ex);
            return StatusCode(500, "Error interno del servidor");
        }
    }
}
