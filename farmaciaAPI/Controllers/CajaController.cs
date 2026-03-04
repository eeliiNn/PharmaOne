using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/caja")]
    [ApiController]
    public class CajaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CajaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("abrir")]
        public async Task<IActionResult> AbrirCaja(AbrirCajaDTO dto)
        {
            // Verificar si el usuario existe
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioId == dto.UsuarioId);

            if (usuario == null)
            {
                return BadRequest(new
                {
                    mensaje = "El usuario no existe"
                });
            }

            // Verificar si ya tiene caja abierta
            var cajaExistente = await _context.Cajas
                .FirstOrDefaultAsync(c => c.UsuarioId == dto.UsuarioId && c.Estado == "Abierta");

            if (cajaExistente != null)
            {
                return Ok(new
                {
                    mensaje = "Caja ya estaba abierta",
                    cajaId = cajaExistente.CajaId
                });
            }

            var caja = new Caja
            {
                FechaApertura = DateTime.Now,
                MontoInicial = dto.MontoInicial,
                MontoFinal = dto.MontoInicial,
                TotalVentas = 0,
                Estado = "Abierta",
                UsuarioId = dto.UsuarioId
            };

            _context.Cajas.Add(caja);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Caja abierta correctamente",
                cajaId = caja.CajaId
            });
        }
        [HttpGet("abierta/{usuarioId}")]
        public async Task<IActionResult> ObtenerCajaAbierta(int usuarioId)
        {
            var caja = await _context.Cajas
                .Where(c => c.UsuarioId == usuarioId && c.Estado == "Abierta")
                .Select(c => new { c.CajaId })
                .FirstOrDefaultAsync();

            if (caja == null)
                return NotFound();

            return Ok(caja);
        }

        [HttpPost("cerrar/{cajaId}")]
        public async Task<IActionResult> CerrarCaja(int cajaId)
        {
            var caja = await _context.Cajas
                .FirstOrDefaultAsync(c => c.CajaId == cajaId && c.Estado == "Abierta");

            if (caja == null)
                return BadRequest("La caja no está abierta");

            caja.FechaCierre = DateTime.Now;
            caja.Estado = "Cerrada";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Caja cerrada correctamente",
                caja.MontoInicial,
                caja.MontoFinal,
                totalVentas = caja.MontoFinal - caja.MontoInicial
            });
        }
    }
}