using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/pedido")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PedidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(PedidoDTO dto)
        {
            decimal descuentoPorcentaje = 0;

            if (dto.ClienteId.HasValue)
            {
                var membresia = await _context.Membresias
                    .Where(m => m.ClienteId == dto.ClienteId && m.Estado == true)
                    .OrderByDescending(m => m.FechaFin)
                    .FirstOrDefaultAsync();

                if (membresia != null)
                    descuentoPorcentaje = membresia.PorcentajeDescuento;
            }

            var pedido = new Pedido
            {
                FechaPedido = DateTime.Now,
                EstadoPedido = "Pendiente",
                TotalPedido = 0
            };

            _context.Pedidos.Add(pedido);

            decimal total = 0;

            foreach (var item in dto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);

                if (producto == null)
                    return BadRequest("Producto no existe");

                var detalle = new DetallePedido
                {
                    Pedido = pedido,
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.PrecioVenta
                };

                _context.DetallePedidos.Add(detalle);

                total += item.Cantidad * producto.PrecioVenta;
            }

            if (descuentoPorcentaje > 0)
            {
                var montoDescuento = total * (descuentoPorcentaje / 100);
                total -= montoDescuento;
            }

            pedido.TotalPedido = total;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Pedido creado correctamente",
                pedidoId = pedido.PedidoId,
                totalFinal = total
            });
        }

        [HttpPut("confirmar/{pedidoId}")]
        public async Task<IActionResult> Confirmar(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);

            if (pedido == null)
                return NotFound("Pedido no existe");

            if (pedido.EstadoPedido != "Pendiente")
                return BadRequest("Solo pedidos pendientes pueden confirmarse");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var item in pedido.Detalles)
                {
                    int cantidadRestante = item.Cantidad;

                    var lotes = await _context.Lotes
                        .Where(l => l.ProductoId == item.ProductoId
                                    && l.CantidadDisponible > 0
                                    && l.FechaVencimiento >= DateTime.Today)
                        .OrderBy(l => l.FechaVencimiento)
                        .ToListAsync();

                    foreach (var lote in lotes)
                    {
                        if (cantidadRestante <= 0)
                            break;

                        int cantidadTomada = Math.Min(cantidadRestante, lote.CantidadDisponible);

                        lote.CantidadDisponible -= cantidadTomada;
                        cantidadRestante -= cantidadTomada;
                    }

                    if (cantidadRestante > 0)
                        throw new Exception("Stock insuficiente");

                    var inventario = await _context.Inventarios
                        .FirstOrDefaultAsync(i => i.ProductoId == item.ProductoId);

                    if (inventario == null || inventario.Stock < item.Cantidad)
                        throw new Exception("Inventario insuficiente");

                    inventario.Stock -= item.Cantidad;
                }

                pedido.EstadoPedido = "Confirmado";

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Pedido confirmado y stock actualizado");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("enviar/{pedidoId}")]
        public async Task<IActionResult> Enviar(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            if (pedido == null)
                return NotFound("Pedido no existe");

            if (pedido.EstadoPedido != "Confirmado")
                return BadRequest("Solo pedidos confirmados pueden enviarse");

            pedido.EstadoPedido = "Enviado";

            await _context.SaveChangesAsync();

            return Ok("Pedido enviado");
        }

        [HttpPut("entregar/{pedidoId}")]
        public async Task<IActionResult> Entregar(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            if (pedido == null)
                return NotFound("Pedido no existe");

            if (pedido.EstadoPedido != "Enviado")
                return BadRequest("Solo pedidos enviados pueden entregarse");

            pedido.EstadoPedido = "Entregado";

            await _context.SaveChangesAsync();

            return Ok("Pedido entregado correctamente");
        }

        [HttpPut("cancelar/{pedidoId}")]
        public async Task<IActionResult> Cancelar(int pedidoId)
        {
            var pedido = await _context.Pedidos.FindAsync(pedidoId);

            if (pedido == null)
                return NotFound("Pedido no existe");

            if (pedido.EstadoPedido == "Confirmado"
                || pedido.EstadoPedido == "Enviado"
                || pedido.EstadoPedido == "Entregado")
            {
                return BadRequest("No se puede cancelar este pedido");
            }

            pedido.EstadoPedido = "Cancelado";

            await _context.SaveChangesAsync();

            return Ok("Pedido cancelado correctamente");
        }
    }
}