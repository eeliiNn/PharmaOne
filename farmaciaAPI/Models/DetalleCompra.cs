using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{

    [Table("DetalleCompra")]
    public class DetalleCompra
    {
        [Key]
        public int DetalleCompraId { get; set; }

        public int CompraId { get; set; }

        [ForeignKey("CompraId")]
        public Compra Compra { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioCompra { get; set; }

        public string NumeroLote { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public Lote Lote { get; set; }
    }
}