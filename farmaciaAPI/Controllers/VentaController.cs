using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/venta")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(VentaDTO dto)
        {
            var caja = await _context.Cajas
                .FirstOrDefaultAsync(c => c.CajaId == dto.CajaId && c.Estado == "Abierta");

            if (caja == null)
                return BadRequest("La caja no está abierta");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
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

                var venta = new Venta
                {
                    FechaVenta = DateTime.Now,
                    TipoPago = dto.TipoPago,
                    CajaId = dto.CajaId,
                    Total = 0,
                    DescuentoAplicado = 0
                };

                _context.Ventas.Add(venta);

                decimal total = 0;

                foreach (var item in dto.Detalles)
                {
                    int cantidadRestante = item.Cantidad;

                    var lotes = await _context.Lotes
                        .Where(l => l.ProductoId == item.ProductoId
                                    && l.CantidadDisponible > 0
                                    && l.FechaVencimiento >= DateTime.Today)
                        .OrderBy(l => l.FechaVencimiento)
                        .ToListAsync();

                    var inventario = await _context.Inventarios
                        .FirstOrDefaultAsync(i => i.ProductoId == item.ProductoId);

                    if (inventario == null)
                        throw new Exception("El producto no existe en inventario");

                    if (inventario.Stock <= inventario.Minimo)
                    {
                        throw new Exception("No se puede vender este producto porque alcanzó el stock mínimo");
                    }

                    if (!lotes.Any())
                        throw new Exception("No hay stock disponible o el producto está vencido");

                    var producto = await _context.Productos.FindAsync(item.ProductoId);

                    foreach (var lote in lotes)
                    {
                        if (cantidadRestante <= 0)
                            break;

                        int cantidadTomada = Math.Min(cantidadRestante, lote.CantidadDisponible);

                        lote.CantidadDisponible -= cantidadTomada;

                        var detalle = new DetalleVenta
                        {
                            Venta = venta,
                            ProductoId = item.ProductoId,
                            LoteId = lote.LoteId,
                            Cantidad = cantidadTomada,
                            PrecioUnitario = producto.PrecioVenta,
                            SubTotal = cantidadTomada * producto.PrecioVenta
                        };

                        _context.DetalleVentas.Add(detalle);

                        total += detalle.SubTotal;
                        cantidadRestante -= cantidadTomada;
                    }

                    if (cantidadRestante > 0)
                        throw new Exception("Stock insuficiente");

                    if (inventario == null || inventario.Stock < item.Cantidad)
                        throw new Exception("Inventario insuficiente");

                    if (dto.Detalles == null || !dto.Detalles.Any())
                        return BadRequest("La venta debe contener al menos un producto");

                    if (item.Cantidad <= 0)
                        throw new Exception("La cantidad debe ser mayor que cero");

                    if (producto == null)
                        throw new Exception("El producto no existe");

                    if (!lotes.Any())
                        throw new Exception("No hay stock disponible o el medicamento está vencido");



                    inventario.Stock -= item.Cantidad;
                }

                decimal totalBruto = total;
                decimal montoDescuento = 0;

                if (descuentoPorcentaje > 0)
                {
                    montoDescuento = totalBruto * (descuentoPorcentaje / 100);
                    total = totalBruto - montoDescuento;
                }

                venta.DescuentoAplicado = montoDescuento;
                venta.Total = total;

                caja.TotalVentas += total;
                caja.MontoFinal += caja.MontoInicial + caja.TotalVentas;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    mensaje = "Venta registrada correctamente",
                    totalBruto,
                    descuentoAplicado = montoDescuento,
                    totalFinal = total
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("cerrar/{cajaId}")]
        public async Task<IActionResult> CerrarCaja(int cajaId)
        {
            var caja = await _context.Cajas.FindAsync(cajaId);

            if (caja == null || caja.Estado != "Abierta")
                return BadRequest("Caja no válida");

            caja.Estado = "Cerrada";
            caja.FechaCierre = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Caja cerrada correctamente",
                totalVentas = caja.TotalVentas,
                montoFinal = caja.MontoFinal
            });
        }

        [HttpGet("historial")]
        public async Task<IActionResult> Historial()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Detalles)
                .Select(v => new
                {
                    v.VentaId,
                    v.FechaVenta,
                    v.TipoPago,
                    v.Total,
                    v.DescuentoAplicado
                })
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();

            return Ok(ventas);
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var ventas = await _context.Ventas
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();

            return Ok(ventas);
        }

        [HttpGet("detalle/{ventaId}")]
        public async Task<IActionResult> Detalle(int ventaId)
        {
            var detalles = await _context.DetalleVentas
                .Include(d => d.Producto)
                .Where(d => d.VentaId == ventaId)
                .Select(d => new
                {
                    d.Producto.Nombre,
                    d.Cantidad,
                    d.PrecioUnitario,
                    d.SubTotal
                })
                .ToListAsync();

            return Ok(detalles);
        }

        [HttpGet("disponibles")]
        public async Task<IActionResult> Disponibles()
        {
            var productos = await _context.Productos
                .Join(_context.Inventarios,
                      p => p.ProductoId,
                      i => i.ProductoId,
                      (p, i) => new
                      {
                          p.ProductoId,
                          p.Nombre,
                          p.PrecioVenta,
                          stock = i.Stock,
                          foto = p.Foto
                      })
                .Where(p => p.stock > 0)
                .ToListAsync();

            return Ok(productos);
        }
    }
}