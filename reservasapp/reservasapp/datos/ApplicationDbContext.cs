using Microsoft.EntityFrameworkCore;
using reservasapp.Models;

namespace reservasapp.datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones) : base(opciones)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación: Un servicio tiene muchas reservas
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Servicio)
                .WithMany(s => s.Reservas)
                .HasForeignKey(r => r.ServicioId);

            // Índice único para evitar dos reservas en la misma fecha y turno
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => new { r.Fecha, r.Turno })
                .IsUnique();

            // Índice único para evitar que el mismo cliente tenga dos reservas en el mismo día
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => new { r.Cliente, r.Fecha })
                .IsUnique();
        }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<HorarioTrabajo> HorarioTrabajo { get; set; }

        
    }
}
