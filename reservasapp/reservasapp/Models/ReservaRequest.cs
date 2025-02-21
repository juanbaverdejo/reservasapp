using System.ComponentModel.DataAnnotations;

public class ReservaRequest
{
    [Required]
    public string Cliente { get; set; }
    [Required]
    public int ServicioId { get; set; }
    public DateTime Fecha { get; set; }
    public int Turno { get; set; }
}
