using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/clientes/register
        [HttpPost("register")]
        public async Task<IActionResult> RegistrarCliente(RegisterDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.UsuarioNombre == dto.UsuarioNombre))
                return BadRequest("El usuario ya existe");

            // Crear usuario primero
            var usuario = new Usuario
            {
                UsuarioNombre = dto.UsuarioNombre,
                Password = dto.Password,
                RolId = dto.RolId // debe ser el Rol Cliente
            };

            var cliente = new Cliente
            {
                Usuario = usuario,
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                clienteId = cliente.ClienteId
            });
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<IActionResult> ListarClientes()
        {
            var clientes = await _context.Clientes
                .Include(c => c.Usuario)
                .Select(c => new
                {
                    c.ClienteId,
                    Usuario = c.Usuario.UsuarioNombre,
                    c.Nombre,
                    c.Apellidos,
                    c.Telefono,
                    c.Direccion
                })
                .ToListAsync();

            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCliente(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .Where(c => c.ClienteId == id)
                .Select(c => new
                {
                    c.ClienteId,
                    c.Nombre,
                    c.Apellidos,
                    c.Telefono,
                    c.Direccion
                })
                .FirstOrDefaultAsync();

            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }
    }
}