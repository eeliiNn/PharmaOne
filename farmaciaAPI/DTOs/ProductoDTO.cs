namespace farmaciaAPI.DTOs
{
    public class ProductoDTO
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioVenta { get; set; }
        public bool RequiereReceta { get; set; }
        public int CategoriaId { get; set; }
        public string Foto { get; set; }
    }
}
