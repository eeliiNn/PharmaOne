using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models;

[Table("Proveedor")]
public class Proveedor
{
    [Key]
    public int ProveedorId { get; set; }

    [Required]
    [StringLength(150)]
    public string Nombre { get; set; }

    [StringLength(20)]
    public string Telefono { get; set; }

    [StringLength(100)]
    public string Correo { get; set; }

    [StringLength(255)]
    public string Direccion { get; set; }
}