using System.ComponentModel.DataAnnotations;
namespace tp8.ViewModels;
public class ProductoViewModel
{
    
    [StringLength(250)]
    public string Descripcion { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
    public int Precio { get; set; }
    public int IdProducto { get; set; }
}