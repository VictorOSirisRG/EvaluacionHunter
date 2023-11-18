using System.ComponentModel.DataAnnotations;

namespace EvaluacionHunter.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
   
        public string Nombre { get; set; }
    
        public string Apellido { get; set; }
 

        public DateTime FechaNacimiento { get; set; }
     
        public string Nacionalidad { get; set; }
    }
}
