using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<IActionResult> CrearUsuario(UsuarioDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.UsuarioNombre == dto.UsuarioNombre))
                return BadRequest("El usuario ya existe");

            var usuario = new Usuario
            {
                UsuarioNombre = dto.UsuarioNombre,
                Password = dto.Password,
                RolId = dto.RolId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Select(u => new
                {
                    u.UsuarioId,
                    u.UsuarioNombre,
                    Rol = u.Rol.Nombre
                })
                .ToListAsync();

            return Ok(usuarios);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound("Usuario no encontrado");

            var tieneCliente = await _context.Clientes
                .AnyAsync(c => c.UsuarioId == id);

            if (tieneCliente)
                return BadRequest("No se puede eliminar: el usuario está vinculado a un cliente");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok("Usuario eliminado correctamente");
        }
    }
}