using System.ComponentModel.DataAnnotations;
namespace tp8.ViewModels;

public class PresupuestoViewModel
{
    public int IdPresupuesto { get; set; }

    [Display(Name = "Email del Destinatario")]
    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido.")]

    public string NombreDestinatario { get; set; }

    [Display(Name = "Fecha de Creación")]
    [Required(ErrorMessage = "La fecha es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; } 

    //LA validacion de la fecha futura se hace en el controlador

}