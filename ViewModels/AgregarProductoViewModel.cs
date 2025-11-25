using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace tp8.ViewModels;

public class AgregarProductoViewModel
{
    // ID del presupuesto al que se va a agregar (viene de la URL o campo oculto)
    public int IdPresupuesto { get; set; }
    [Display(Name = "Producto a agregar")]

    // ID del producto seleccionado en el dropdown
    public int IdProducto { get; set; }

    // Validación: Requerido y debe ser positivo
    [Display(Name = "Cantidad")]
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; }

    //Propiedad adicional para el Dropdown (no se valida, solo se usa en la Vista)
    //public SelectList ListaProductos { get; set; }
    [ValidateNever]
    public SelectList ListaProductos { get; set; }
}