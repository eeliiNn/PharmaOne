namespace farmaciaAPI.DTOs
{
    public class PedidoDTO
    {
        public int? ClienteId { get; set; } 
        public List<DetallePedidoDTO> Detalles { get; set; }
    }

    public class DetallePedidoDTO
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
