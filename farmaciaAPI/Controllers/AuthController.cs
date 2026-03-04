using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u =>
                    u.UsuarioNombre == dto.UsuarioNombre &&
                    u.Password == dto.Password);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });

            return Ok(new
            {
                usuarioId = usuario.UsuarioId,
                usuarioNombre = usuario.UsuarioNombre,
                rol = usuario.Rol.Nombre
            });
        }
    }
}