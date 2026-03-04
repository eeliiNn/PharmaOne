using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using farmaciaAPI.Models;

namespace farmaciaAPI.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required]
        [Column("Usuario")]
        public string UsuarioNombre { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey("Rol")]
        public int RolId { get; set; }

        public Rol Rol { get; set; }

        public Cliente Cliente { get; set; }
    }
}