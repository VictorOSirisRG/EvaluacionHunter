using System.ComponentModel.DataAnnotations;

namespace EvaluacionHunter.Entidades
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(100)]
        public string Genero { get; set; }
        [Required]
        [StringLength(100)]
        public string Idioma { get; set; }
        [Required]
        public int Duracion { get; set; }

        public List<ActorPelicula> ActoresPeliculas { get; set; }
    }
}
