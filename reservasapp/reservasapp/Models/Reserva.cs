using System.ComponentModel.DataAnnotations;

namespace reservasapp.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; } = null!;
        public DateTime Fecha { get; set; }
        public int Turno { get; set; }
    }


}
