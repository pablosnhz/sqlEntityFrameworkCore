using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
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
                               select new LibroDto
                               {
                                   Titulo = l.Titulo,
                                   Categoria = l.Categoria.Nombre,
                                   Precio = l.Precio
                               }).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosMethod()
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                                            .Select(l => new LibroDto
                                            {
                                                Titulo = l.Titulo,
                                                Categoria = l.Categoria.Nombre,
                                                Precio = l.Precio
                                            })
                                            .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosPorNombreQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorNombreQuery(string titulo)
        {
            var lista = await (from l in _context.Libros.Include(l => l.Categoria)
                               where l.Titulo == titulo
                               select l).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosPorNombreMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosPorNombreMethod(string titulo)
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                .Where(l => l.Titulo == titulo)
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosConstainsQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosConstainsQuery(string filtro)
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                .Where(l => l.Titulo.Contains(filtro))
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosContainsMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosContainsMethod(string filtro)
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                .Where(l => l.Titulo.Contains(filtro))
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosLikeQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosLikeQuery(string filtro)
        {
            var filtroLike = $"{filtro}%";
            var lista = await (from l in _context.Libros.Include(l => l.Categoria)
                               where EF.Functions.Like(l.Titulo, filtroLike)
                               select l).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosLikeMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosLikeMethod(string filtro)
        {
            var filtroLike = $"{filtro}%";

            var lista = await _context.Libros.Include(l => l.Categoria)
                .Where(l => EF.Functions.Like(l.Titulo, filtroLike))
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosOrderByQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosOrderByQuery()
        {
            var lista = await (from l in _context.Libros.Include(l => l.Categoria)
                               orderby l.Titulo
                               select l).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosOrderByMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosOrderByMethod(string filtro)
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                .OrderBy(l => l.Titulo)
                .OrderBy(l => l.Descripcion)
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosProyectarQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosProyectarQuery()
        {
            var lista = await (from l in _context.Libros.Include(l => l.Categoria)
                               orderby l.Titulo, l.Descripcion
                               select new LibroDto{
                                   Titulo = l.Titulo, 
                                   Categoria = l.Categoria.Nombre, 
                                   Precio = l.Precio
                               }).ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosProyectarMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosProyectarMethod()
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                .OrderBy(l => l.Titulo)
                .OrderBy(l => l.Descripcion)
                .Select(l => new LibroDto{
                    Titulo = l.Titulo, 
                    Categoria = l.Categoria.Nombre, 
                    Precio = l.Precio})
                .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetCategoriasQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetCategoriasQuery()
        {
            var lista = await (from c in _context.Categorias
                               select new 
                               {
                                Id = c.Id,
                                Categoria = c.Nombre,
                                Libros = c.ListaLibros.Select(l => new {
                                    Titulo = l.Titulo,
                                    Descripcion = l.Descripcion,
                                })
                               }).ToArrayAsync();
            return Ok(lista);
        }

        [HttpGet("GetCategoriasMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetCategoriasMethod()
        {
            var lista = await _context.Categorias
                                .Select(c => new
                                {
                                    Id = c.Id,
                                    Categoria = c.Nombre,
                                    Libros = c.ListaLibros
                                }).ToArrayAsync();
            return Ok(lista);
        }
    }
}
