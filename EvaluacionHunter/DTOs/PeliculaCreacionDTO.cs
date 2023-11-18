using System.ComponentModel.DataAnnotations;

namespace EvaluacionHunter.DTOs
{
    public class PeliculaCreacionDTO
    {
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

        public List<int> ActoresIds { get; set; }
    }
}
