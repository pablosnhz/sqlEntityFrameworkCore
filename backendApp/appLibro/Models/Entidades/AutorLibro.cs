using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    [Table("AutorLibro")]
    public class AutorLibro
    {
        [Column("AutorLibro")]
        public int Id { get; set; }

        public int AutorId { get; set; }
        
        [ForeignKey("AutorId")]
        public Autor Autor { get; set;}

        public int LibroId { get; set; }

        [ForeignKey("LibroId")]
        public Libro Libro { get; set;}

        public int Orden { get; set; }
    }
}
