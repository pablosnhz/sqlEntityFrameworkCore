using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinqController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LinqController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetLibrosQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosQuery()
        {
            var lista = await (from l in _context.Libros.Include(l => l.Categoria)
                               select l).ToListAsync();
            return Ok(lista);
        }
    }
}
