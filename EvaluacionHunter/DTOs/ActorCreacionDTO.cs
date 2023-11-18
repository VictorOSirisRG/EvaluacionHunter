using System.ComponentModel.DataAnnotations;

namespace EvaluacionHunter.DTOs
{
    public class ActorCreacionDTO
    {
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

    }
}
