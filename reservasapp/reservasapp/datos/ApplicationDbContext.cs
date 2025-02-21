using Microsoft.EntityFrameworkCore;
using reservasapp.Models;

namespace reservasapp.datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : base(opciones)
        {

        }


        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Reserva> Reserva { get; set; }

        
    }
}
