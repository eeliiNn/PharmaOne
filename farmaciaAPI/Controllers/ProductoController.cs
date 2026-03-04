using farmaciaAPI.DTOs;
using farmaciaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/producto")]
[ApiController]
public class ProductoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("crear")]
    public async Task<IActionResult> Crear(ProductoDTO dto)
    {
        if (dto == null)
            return BadRequest("Datos inválidos");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest("El nombre es obligatorio");

        if (dto.Nombre.Length > 100)
            return BadRequest("El nombre no puede superar 100 caracteres");

        if (string.IsNullOrWhiteSpace(dto.Descripcion))
            return BadRequest("La descripción es obligatoria");

        if (dto.Descripcion.Length > 300)
            return BadRequest("La descripción no puede superar 300 caracteres");

        if (dto.PrecioVenta <= 0)
            return BadRequest("El precio debe ser mayor que 0");

        if (dto.CategoriaId <= 0)
            return BadRequest("Debe seleccionar una categoría válida");

        var existeNombre = await _context.Productos
    .AnyAsync(p => p.Nombre.ToLower() == dto.Nombre.ToLower());

        if (existeNombre)
            return BadRequest("Ya existe un producto con ese nombre");

        var categoriaExiste = await _context.Categorias
            .AnyAsync(c => c.CategoriaId == dto.CategoriaId);

        if (!categoriaExiste)
            return BadRequest("La categoría no existe");

        if (!string.IsNullOrWhiteSpace(dto.Foto))
        {
            if (!Uri.IsWellFormedUriString(dto.Foto, UriKind.Absolute))
                return BadRequest("La URL de la foto no es válida");
        }

        var producto = new Producto
        {
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion.Trim(),
            PrecioVenta = dto.PrecioVenta,
            RequiereReceta = dto.RequiereReceta,
            CategoriaId = dto.CategoriaId,
            Estado = true,
            Foto = dto.Foto
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return Ok("Producto creado correctamente");
    }

    [HttpGet("listar")]
    public async Task<IActionResult> Listar()
    {
        var productos = await _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.Estado == true)
            .ToListAsync();

        return Ok(productos);
    }

    [HttpGet("inventario-admin")]
    public async Task<IActionResult> InventarioAdmin()
    {
        var data = await _context.Productos
            .Include(p => p.Categoria)
            .Select(p => new
            {
                p.ProductoId,
                p.Nombre,
                p.Descripcion,
                p.PrecioVenta,
                p.RequiereReceta,
                p.Estado,
                Categoria = p.Categoria.NombreCategoria,
                CategoriaId = p.CategoriaId,
                p.Foto,
                Stock = _context.Inventarios
                    .Where(i => i.ProductoId == p.ProductoId)
                    .Select(i => i.Stock)
                    .FirstOrDefault(),
                Minimo = _context.Inventarios
                    .Where(i => i.ProductoId == p.ProductoId)
                    .Select(i => i.Minimo)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(data);
    }

    [HttpPut("activar/{id}")]
    public IActionResult Activar(int id)
    {
        var producto = _context.Productos.Find(id);

        if (producto == null)
            return NotFound();

        producto.Estado = true;

        _context.SaveChanges();

        return NoContent();
    }

    [HttpGet("disponibles")]
    public async Task<IActionResult> Disponibles()
    {
        var productos = await _context.Inventarios
            .Include(i => i.Producto)
            .ThenInclude(p => p.Categoria)
            .Where(i => i.Stock > 0 && i.Producto.Estado == true)
            .Select(i => new
            {
                i.Producto.ProductoId,
                i.Producto.Nombre,
                i.Producto.PrecioVenta,
                i.Producto.Foto,
                i.Stock
            })
            .ToListAsync();

        return Ok(productos);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string nombre)
    {
        var productos = await _context.Productos
            .Where(p => p.Nombre.Contains(nombre) && p.Estado == true)
            .ToListAsync();

        return Ok(productos);
    }

    [HttpGet("categoria/{categoriaId}")]
    public async Task<IActionResult> PorCategoria(int categoriaId)
    {
        var productos = await _context.Productos
            .Where(p => p.CategoriaId == categoriaId && p.Estado == true)
            .ToListAsync();

        return Ok(productos);
    }

    [HttpPut("editar/{id}")]
    public async Task<IActionResult> Editar(int id, ProductoDTO dto)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
            return NotFound("Producto no encontrado");

        if (string.IsNullOrWhiteSpace(dto.Nombre))
            return BadRequest("El nombre es obligatorio");

        if (string.IsNullOrWhiteSpace(dto.Descripcion))
            return BadRequest("La descripción es obligatoria");

        if (dto.PrecioVenta <= 0)
            return BadRequest("El precio debe ser mayor que 0");

        var categoriaExiste = await _context.Categorias
            .AnyAsync(c => c.CategoriaId == dto.CategoriaId);

        if (!categoriaExiste)
            return BadRequest("La categoría no existe");

        producto.Nombre = dto.Nombre.Trim();
        producto.Descripcion = dto.Descripcion.Trim();
        producto.PrecioVenta = dto.PrecioVenta;
        producto.RequiereReceta = dto.RequiereReceta;
        producto.CategoriaId = dto.CategoriaId;
        producto.Foto = dto.Foto;

        await _context.SaveChangesAsync();

        return Ok("Producto actualizado correctamente");
    }

    [HttpPut("desactivar/{id}")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
            return NotFound("Producto no encontrado");

        producto.Estado = false;
        await _context.SaveChangesAsync();

        return Ok("Producto desactivado correctamente");
    }
}