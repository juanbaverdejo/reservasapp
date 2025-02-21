using System.ComponentModel.DataAnnotations;

namespace reservasapp.Models
{
    public class Servicio
    {
        
            public int Id { get; set; }
            public string Nombre { get; set; }
            public decimal Precio { get; set; }
            public int Duracion { get; set; }
       
    }
}
