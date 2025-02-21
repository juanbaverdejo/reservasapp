using System.ComponentModel.DataAnnotations;

namespace reservasapp.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public int ServicioId { get; set; }
        public DateTime Fecha { get; set; }
        public int Horario { get; set; }
    }


}
