using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{
    [Table("Caja")]
    public class Caja
    {
        [Key]
        public int CajaId { get; set; }

        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }

        public decimal MontoInicial { get; set; }
        public decimal TotalVentas { get; set; }
        public decimal MontoFinal { get; set; }

        public string Estado { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Venta> Ventas { get; set; }
    }
}
