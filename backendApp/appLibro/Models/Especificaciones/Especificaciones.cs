using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CambioPrecioParametros
{
    public class CambioPrecioParametros
    {
        public int CategoriaId { get; set; }
        public decimal Porcentaje { get; set; }
        public string Texto { get; set; }
    }
}
