using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/proveedor")]
[ApiController]
public class ProveedorController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProveedorController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Listar()
    {
        var proveedores = await _context.Proveedores.ToListAsync();
        return Ok(proveedores);
    }

    [HttpPost("crear")]
    public async Task<IActionResult> Crear(ProveedorDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest("El nombre es obligatorio");

        if (!string.IsNullOrWhiteSpace(dto.Correo))
        {
            bool correoExiste = await _context.Proveedores
                .AnyAsync(p => p.Correo == dto.Correo);

            if (correoExiste)
                return BadRequest("El correo ya está registrado");
        }

        var proveedor = new Proveedor
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono,
            Correo = dto.Correo,
            Direccion = dto.Direccion
        };

        _context.Proveedores.Add(proveedor);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensaje = "Proveedor creado correctamente",
            proveedor.ProveedorId
        });
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string nombre)
    {
        var proveedores = await _context.Proveedores
            .Where(p => p.Nombre.Contains(nombre))
            .ToListAsync();

        if (!proveedores.Any())
            return NotFound("No se encontraron proveedores");

        return Ok(proveedores);
    }

    [HttpPut("editar/{id}")]
    public async Task<IActionResult> Editar(int id, ProveedorDTO dto)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);

        if (proveedor == null)
            return NotFound("Proveedor no encontrado");

        if (!string.IsNullOrWhiteSpace(dto.Correo))
        {
            bool correoExiste = await _context.Proveedores
                .AnyAsync(p => p.Correo == dto.Correo && p.ProveedorId != id);

            if (correoExiste)
                return BadRequest("El correo ya está registrado");
        }

        proveedor.Nombre = dto.Nombre;
        proveedor.Telefono = dto.Telefono;
        proveedor.Correo = dto.Correo;
        proveedor.Direccion = dto.Direccion;

        await _context.SaveChangesAsync();

        return Ok("Proveedor actualizado correctamente");
    }
}