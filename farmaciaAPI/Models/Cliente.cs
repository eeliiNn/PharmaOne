using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using farmaciaAPI.Models;

namespace farmaciaAPI.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public ICollection<Membresia> Membresias { get; set; }
    }
}