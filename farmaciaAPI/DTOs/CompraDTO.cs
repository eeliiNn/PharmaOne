namespace farmaciaAPI.DTOs
{
    public class CompraDTO
    {
        public int ProveedorId { get; set; }
        public List<DetalleCompraDTO> Detalles { get; set; }
    }

    public class DetalleCompraDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioCompra { get; set; }
        public string NumeroLote { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}
