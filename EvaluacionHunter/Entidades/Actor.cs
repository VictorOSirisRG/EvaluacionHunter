using System.ComponentModel.DataAnnotations;

namespace EvaluacionHunter.Entidades
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }
        [Required]
      
        public DateTime FechaNacimiento { get; set; }
        [Required]
        [StringLength(100)]
        public string Nacionalidad { get; set; }

     
        public List<ActorPelicula> ActoresPeliculas { get; set; }
    }
}
