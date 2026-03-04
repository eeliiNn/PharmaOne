using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace farmaciaAPI.Models;

[Table("Categoria")]
public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }

    [Required]
    [StringLength(100)]
    public string NombreCategoria { get; set; }

    [JsonIgnore]
    public ICollection<Producto> Productos { get; set; }
}