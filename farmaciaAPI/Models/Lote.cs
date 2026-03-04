using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{

    [Table("Lote")]
    public class Lote
    {
        [Key]
        public int LoteId { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        public string NumeroLote { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public int CantidadDisponible { get; set; }

        public int DetalleCompraId { get; set; }

        [ForeignKey("DetalleCompraId")]
        public DetalleCompra DetalleCompra { get; set; }
    }
}