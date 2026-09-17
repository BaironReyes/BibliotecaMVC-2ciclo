using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaMVC.Models
{
    public class Libro
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }

    }
}
