using System.ComponentModel.DataAnnotations;

namespace reservasapp.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public TimeSpan Duracion { get; set; }

        public ICollection<Reserva> Reservas {  get; set; }= new List<Reserva>();
    }
}
