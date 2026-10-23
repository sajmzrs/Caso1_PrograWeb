using System.ComponentModel.DataAnnotations;

namespace Caso1.Models;

public class Reserva
{
    public long Consecutivo { get; set; }

    [Required(ErrorMessage = "Indique el número de habitación.")]
    [StringLength(10, ErrorMessage = "El número de habitación admite hasta 10 caracteres.")]
    [Display(Name = "Número de habitación")]
    public string NumeroHabitacion { get; set; } = "";

    public DateTime FechaReserva { get; set; }

    [Required(ErrorMessage = "Indique el monto total.")]
    [Range(typeof(decimal), "0.01", "99999999.99", ErrorMessage = "Ingrese un monto entre 0.01 y 99999999.99.")]
    [Display(Name = "Monto total")]
    public decimal? MontoTotal { get; set; }

    [Required(ErrorMessage = "Seleccione un tipo de habitación.")]
    [Display(Name = "Tipo de habitación")]
    public int? TipoHabitacion { get; set; }

    public string DescripcionTipo { get; set; } = "";
}

public class TipoHabitacion
{
    public int Tipo { get; set; }
    public string DescripcionTipo { get; set; } = "";
}
