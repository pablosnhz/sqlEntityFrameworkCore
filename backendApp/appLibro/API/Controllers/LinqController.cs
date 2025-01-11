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


        // procedemos a ver el UNION y el UNION ALL
        [HttpGet("GetLibrosUnionQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosUnionQuery()
        {
            var lista1 = await(from l in _context.Libros.Include(l => l.Categoria)
                               orderby l.Titulo
                               where l.Precio >= 40
                               select new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.Categoria.Nombre,
                                    Precio = l.Precio,
                                    Publicado = l.Publicado
                                }).ToListAsync();

            var lista2 = await (from l in _context.Libros.Include(l => l.Categoria)
                                orderby l.Titulo
                                where l.Publicado.Year >= 2013
                                select new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.Categoria.Nombre,
                                    Precio = l.Precio,
                                    Publicado = l.Publicado
                                }).ToListAsync();

            var listaUnion = lista1.Union(lista2);
            
            return Ok(listaUnion);
        }

        [HttpGet("GetLibrosUnionMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosUnionMethod()
        {
            var lista1 = await _context.Libros.Include(l => l.Categoria)
                                   .Where(l => l.Precio >= 40)
                                   .Select(l => new
                                   {
                                       Titulo = l.Titulo,
                                       Categoria = l.Categoria.Nombre,
                                       Precio = l.Precio,
                                       Publicado = l.Publicado
                                   })
                                   .ToListAsync();

            var lista2 = await _context.Libros.Include(l => l.Categoria)
                                   .Where(l => l.Publicado.Year >= 2013)
                                   .Select(l => new
                                   {
                                       Titulo = l.Titulo,
                                       Categoria = l.Categoria.Nombre,
                                       Precio = l.Precio,
                                       Publicado = l.Publicado
                                   })
                                   .ToListAsync();

            var listaUnion = lista1.Union(lista2);
            return Ok(listaUnion);
        }

        // ALL UNION
        [HttpGet("GetLibrosConcatQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosConcatQuery()
        {
            var lista1 = await (from l in _context.Libros.Include(l => l.Categoria)
                                orderby l.Titulo
                                where l.Precio >= 40
                                select new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.Categoria.Nombre,
                                    Precio = l.Precio,
                                    Publicado = l.Publicado
                                }).ToListAsync();

            var lista2 = await (from l in _context.Libros.Include(l => l.Categoria)
                                orderby l.Titulo
                                where l.Publicado.Year >= 2013
                                select new
                                {
                                    Titulo = l.Titulo,
                                    Categoria = l.Categoria.Nombre,
                                    Precio = l.Precio,
                                    Publicado = l.Publicado
                                }).ToListAsync();

            var listaUnion = lista1.Concat(lista2); // UNION ALL

            return Ok(listaUnion);
        }

        [HttpGet("GetLibrosConcatMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosConcatMethod()
        {
            var lista1 = await _context.Libros.Include(l => l.Categoria)
                                   .Where(l => l.Precio >= 40)
                                   .Select(l => new
                                   {
                                       Titulo = l.Titulo,
                                       Categoria = l.Categoria.Nombre,
                                       Precio = l.Precio,
                                       Publicado = l.Publicado
                                   })
                                   .ToListAsync();

            var lista2 = await _context.Libros.Include(l => l.Categoria)
                                   .Where(l => l.Publicado.Year >= 2013)
                                   .Select(l => new
                                   {
                                       Titulo = l.Titulo,
                                       Categoria = l.Categoria.Nombre,
                                       Precio = l.Precio,
                                       Publicado = l.Publicado
                                   })
                                   .ToListAsync();

            var listaUnion = lista1.Concat(lista2); //union all
            return Ok(listaUnion);
        }

        [HttpGet("GetLibrosGroupByCountQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByCountQuery()
        {
            var lista = await (from l in _context.Libros.Include(l => l.Categoria)
                                orderby l.Titulo
                                // para poner varios group l by new {l.categoria.nombre, l.titulo}
                                group l by l.Categoria.Nombre into grp
                                select new
                                {
                                    Categoria = grp.Key,
                                    Cantidad = grp.Count()
                                }).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("GetLibrosGroupByCountMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByCountMethod()
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                                   .GroupBy(l => l.Categoria.Nombre)
                                   .Select(grp => new
                                   {
                                       Categoria = grp.Key,
                                       Cantidad = grp.Count()
                                   })
                                   .ToListAsync();
            return Ok(lista);
        }

        [HttpGet("GetLibrosGroupByMaxQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByMaxQuery()
        {
            var lista = await (from l in _context.Libros.Include(l => l.Categoria)
                               orderby l.Titulo
                               group l by l.Categoria.Nombre into grp
                               select new
                               {
                                   Categoria = grp.Key,
                                   PrecioMaximo = ( from l2 in grp select l2.Precio ).Max()
                               }).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("GetLibrosGroupByMaxMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByMaxMethod()
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                                   .GroupBy(l => l.Categoria.Nombre)
                                   .Select(grp => new
                                   {
                                       Categoria = grp.Key,
                                       PrecioMaximo = grp.Select(l2 => l2.Precio).Max()
                                   })
                                   .ToListAsync();
            return Ok(lista);
        }
        // promedio con avg
        [HttpGet("GetLibrosGroupByAvgQuery")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByAvgQuery()
        {
            var lista = await (from l in _context.Libros.Include(l => l.Categoria)
                               orderby l.Titulo
                               group l by l.Categoria.Nombre into grp
                               select new
                               {
                                   Categoria = grp.Key,
                                   PrecioPromedio = (from l2 in grp select l2.Precio).Average()
                               }).ToListAsync();

            return Ok(lista);
        }

        [HttpGet("GetLibrosGroupByAvgMethod")]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibrosGroupByAvgMethod()
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                                   .GroupBy(l => l.Categoria.Nombre)
                                   .Select(grp => new
                                   {
                                       Categoria = grp.Key,
                                       PrecioPromedio = grp.Select(l2 => l2.Precio).Average()
                                   })
                                   .ToListAsync();
            return Ok(lista);
        }

        // valores agregados que hicimos para las tablas como por ejemplo un resenia
        [HttpGet("GetLibrosDetalle")]
        public async Task<ActionResult<IEnumerable<LibroDetalleDto>>> GetLibrosDetalle()
        {
            var lista = await _context.Libros.Include(l => l.Categoria)
                                      .Include(l => l.PrecioOferta)
                                      .Include(l => l.LibrosAutores).ThenInclude(l => l.Autor)
                                      .Include(l => l.Reviews)
                                      .ToListAsync();

            var listaDetalle = lista.Select(l => new LibroDetalleDto
            {
                Titulo = l.Titulo,
                Categoria = l.Categoria.Nombre,
                Precio = l.Precio,
                Publicado = l.Publicado,
                PrecioActual = l.PrecioOferta == null 
                               ? l.Precio
                               : l.PrecioOferta.NuevoPrecio,
                TextoPromocional = l.PrecioOferta == null
                                    ? null
                                    : l.PrecioOferta.TextoPromocional,
                Autores = string.Join(", ", l.LibrosAutores
                                 .OrderBy(l => l.Orden)
                                 .Select(l => l.Autor.Nombre)),
                ReviewsCantidad = l.Reviews.Count(),
                ReviewsPromedio = l.Reviews.Select(l => (double?) l.NumeroEstrellas).Average(),
            });

            return Ok(listaDetalle);
        }

        //explicando stored procedure en linq
        [HttpGet("GetRangoSP")]
        public async Task<ActionResult<IEnumerable<LibroRango>>> GetRangoSP(int rangoInicio, int rangoFin)
        {
            var lista = await _context.LibroRango
                              .FromSqlRaw("LibrosPublicadosRango {0}, {1}", rangoInicio, rangoFin)
                              .ToListAsync();
            return Ok(lista);
        }
    }
}
