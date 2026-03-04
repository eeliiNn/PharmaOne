using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{
    [Table("DetalleVenta")]
    public class DetalleVenta
    {
        [Key]
        public int DetalleVentaId { get; set; }

        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public int LoteId { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal SubTotal { get; set; }

        [ForeignKey("VentaId")]
        public Venta Venta { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        [ForeignKey("LoteId")]
        public Lote Lote { get; set; }
    }
}
