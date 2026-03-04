using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{

    [Table("Inventario")]
    public class Inventario
    {
        [Key]
        public int InventarioId { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        public int Stock { get; set; }

        public int Minimo { get; set; }
    }
}
