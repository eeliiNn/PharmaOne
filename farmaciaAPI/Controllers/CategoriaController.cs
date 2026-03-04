using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace farmaciaAPI.Controllers
{
    [Route("api/categoria")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar()
        {
            var categorias = await _context.Categorias
                .Select(c => new
                {
                    c.CategoriaId,
                    c.NombreCategoria
                })
                .ToListAsync();

            return Ok(categorias);
        }
    }
}
