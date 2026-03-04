using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{

    [Table("Pedido")]
    public class Pedido
    {
        [Key]
        public int PedidoId { get; set; }

        public DateTime FechaPedido { get; set; }

        public string EstadoPedido { get; set; }

        public decimal TotalPedido { get; set; }

        public ICollection<DetallePedido> Detalles { get; set; }
    }
}