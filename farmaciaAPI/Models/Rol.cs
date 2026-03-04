using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farmaciaAPI.Models
{
    [Table("Rol")]
    public class Rol
    {
        [Key]
        public int RolId { get; set; }
        [Required]
        public string Nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
