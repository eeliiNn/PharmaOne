using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models;

[Table("Compra")]
public class Compra
{
    [Key]
    public int CompraId { get; set; }

    public DateTime FechaCompra { get; set; }

    public int ProveedorId { get; set; }

    [ForeignKey("ProveedorId")]
    public Proveedor Proveedor { get; set; }

    public decimal Total { get; set; }

    public ICollection<DetalleCompra> Detalles { get; set; }
}