using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservasapp.datos;
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
    public async Task<IActionResult> GetServicios()
    {
        var servicios = await _context.Servicio.ToListAsync();
        return Ok(servicios);
    }
}
