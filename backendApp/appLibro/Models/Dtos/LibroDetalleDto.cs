using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class LibroDetalleDto
    {
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public DateTime Publicado { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioActual { get; set; }
        public string TextoPromocional { get; set; }
        public string Autores { get; set; }
        public int ReviewsCantidad { get; set; }
        public double? ReviewsPromedio { get; set; }
    }
}
