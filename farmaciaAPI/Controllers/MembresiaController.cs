using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/membresias")]
    [ApiController]
    public class MembresiasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MembresiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Crear membresía
        [HttpPost]
        public async Task<IActionResult> CrearMembresia(MembresiaDTO dto)
        {
            var cliente = await _context.Clientes.FindAsync(dto.ClienteId);

            if (cliente == null)
                return NotFound("Cliente no encontrado");

            decimal descuento = 0;

            switch (dto.TipoMembresia.ToLower())
            {
                case "black":
                    descuento = 30;
                    break;

                case "dorada":
                    descuento = 15;
                    break;

                case "azul":
                    descuento = 5;
                    break;

                default:
                    return BadRequest("Tipo de membresía inválido");
            }

            var activa = await _context.Membresias
                .FirstOrDefaultAsync(m => m.ClienteId == dto.ClienteId && m.Estado);

            if (activa != null)
                return BadRequest("El cliente ya tiene una membresía activa");

            var membresia = new Membresia
            {
                ClienteId = dto.ClienteId,
                TipoMembresia = dto.TipoMembresia,
                FechaInicio = DateTime.Now,
                FechaFin = dto.FechaFin,
                PorcentajeDescuento = descuento,
                Estado = true
            };

            _context.Membresias.Add(membresia);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Membresía creada correctamente",
                descuento = descuento
            });
        }

        // Listar membresías
        [HttpGet]
        public async Task<IActionResult> ListarMembresias()
        {
            var hoy = DateTime.Now;

            var membresias = await _context.Membresias
                .Include(m => m.Cliente)
                .ToListAsync();

            foreach (var m in membresias)
            {
                if (m.FechaFin < hoy && m.Estado)
                    m.Estado = false;
            }

            await _context.SaveChangesAsync();

            return Ok(membresias.Select(m => new
            {
                m.MembresiaId,
                Cliente = m.Cliente.Nombre + " " + m.Cliente.Apellidos,
                m.TipoMembresia,
                m.FechaInicio,
                m.FechaFin,
                m.PorcentajeDescuento,
                Estado = m.Estado ? "Activa" : "Inactiva"
            }));
        }

        // Obtener membresía activa de un cliente
        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> ObtenerMembresiaCliente(int clienteId)
        {
            var hoy = DateTime.Now;

            var membresia = await _context.Membresias
                .Where(m => m.ClienteId == clienteId && m.Estado)
                .FirstOrDefaultAsync();

            if (membresia == null)
                return Ok(new
                {
                    tieneMembresia = false,
                    descuento = 0
                });

            if (membresia.FechaFin < hoy)
            {
                membresia.Estado = false;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    tieneMembresia = false,
                    descuento = 0
                });
            }

            return Ok(new
            {
                tieneMembresia = true,
                descuento = membresia.PorcentajeDescuento,
                tipo = membresia.TipoMembresia,
                fechaFin = membresia.FechaFin
            });
        }
    }
}