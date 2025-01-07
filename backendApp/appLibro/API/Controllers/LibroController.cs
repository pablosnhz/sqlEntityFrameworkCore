using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entidades;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LibroController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> Get()
        {
         /* traemos datos de libros por defecto y le sumamos Categoria con el include */
            var lista = await _context.Libros.Include(c => c.Categoria).ToListAsync();
            return Ok(lista);
        }
    }
}
