using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/compra")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompraController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear(CompraDTO dto)
        {
            var proveedorExiste = await _context.Proveedores
                .AnyAsync(p => p.ProveedorId == dto.ProveedorId);

            if (!proveedorExiste)
                return BadRequest("Proveedor no existe");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var compra = new Compra
                {
                    FechaCompra = DateTime.Now,
                    ProveedorId = dto.ProveedorId,
                    Total = 0
                };

                _context.Compras.Add(compra);

                decimal total = 0;

                foreach (var item in dto.Detalles)
                {
                    var detalle = new DetalleCompra
                    {
                        Compra = compra, 
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioCompra = item.PrecioCompra,
                        NumeroLote = item.NumeroLote,
                        FechaVencimiento = item.FechaVencimiento
                    };

                    _context.DetalleCompras.Add(detalle);

                    var lote = new Lote
                    {
                        ProductoId = item.ProductoId,
                        NumeroLote = item.NumeroLote,
                        FechaVencimiento = item.FechaVencimiento,
                        CantidadDisponible = item.Cantidad,
                        DetalleCompra = detalle 
                    };

                    _context.Lotes.Add(lote);

                    var inventario = await _context.Inventarios
                        .FirstOrDefaultAsync(i => i.ProductoId == item.ProductoId);

                    if (inventario == null)
                    {
                        inventario = new Inventario
                        {
                            ProductoId = item.ProductoId,
                            Stock = item.Cantidad,
                            Minimo = 10
                        };

                        _context.Inventarios.Add(inventario);
                    }
                    else
                    {
                        inventario.Stock += item.Cantidad;
                    }

                    total += item.Cantidad * item.PrecioCompra;
                }

                compra.Total = total;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok("Compra registrada correctamente");
            }
            catch
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Error al registrar la compra");
            }
        }
    }
}
