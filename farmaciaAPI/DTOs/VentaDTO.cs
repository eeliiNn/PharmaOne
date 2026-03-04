namespace farmaciaAPI.DTOs
{
    public class VentaDTO
    {
        public int CajaId { get; set; }
        public string TipoPago { get; set; }
        public int? ClienteId { get; set; } 
        public List<DetalleVentaDTO> Detalles { get; set; }
    }

    public class DetalleVentaDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
