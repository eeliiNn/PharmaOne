using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{

    [Table("DetallePedido")]
    public class DetallePedido
    {
        [Key]
        public int DetallePedidoId { get; set; }

        public int PedidoId { get; set; }

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
    }
}