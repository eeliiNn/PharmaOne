using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models;

[Table("Producto")]
public class Producto
{
    [Key]
    public int ProductoId { get; set; }

    [Required]
    [StringLength(150)]
    public string Nombre { get; set; }

    [StringLength(255)]
    public string Descripcion { get; set; }

    [Required]
    public decimal PrecioVenta { get; set; }

    [Required]
    public bool RequiereReceta { get; set; }

    public int CategoriaId { get; set; }

    [ForeignKey("CategoriaId")]
    public Categoria Categoria { get; set; }

    [Required]
    public bool Estado { get; set; }

    public string Foto { get; set; }
}