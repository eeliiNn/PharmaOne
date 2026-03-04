using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{
    [Table("Venta")]
    public class Venta
    {
        [Key]
        public int VentaId { get; set; }

        public DateTime FechaVenta { get; set; }

        public decimal Total { get; set; }

        public string TipoPago { get; set; }

        public int CajaId { get; set; }

        public decimal DescuentoAplicado { get; set; }

        [ForeignKey("CajaId")]
        public Caja Caja { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; }
    }
}
