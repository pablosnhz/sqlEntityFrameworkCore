using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    [Table("Review")]
    public class Review
    {
        [Key]
        [Column("ReviewId")]
        public int Id { get; set; }
        public string NombreVotante { get; set; }
        public int NumeroEstrellas { get; set; }
        public string Comentario { get; set; }
        public int LibroId { get; set; }

        [ForeignKey("LibroId")]
        public Libro Libro { get; set; }
    }
}
