using System.ComponentModel.DataAnnotations;

namespace EvaluacionHunter.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
    
        public string Titulo { get; set; }
     
        public string Genero { get; set; }
    
        public string Idioma { get; set; }
    
        public int Duracion { get; set; }
    }
}
