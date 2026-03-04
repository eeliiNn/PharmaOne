using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{
    [Table("Membresia")]
    public class Membresia
    {
        [Key]
        public int MembresiaId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; }

        [Required]
        public string TipoMembresia { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public decimal PorcentajeDescuento { get; set; }

        [Required]
        public bool Estado { get; set; }
    }
}