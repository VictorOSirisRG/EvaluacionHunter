namespace EvaluacionHunter.Entidades
{
    public class ActorPelicula
    {
        public int ActorId { get; set; }
        public int PeliculaId { get; set; }

        public Actor Actor { get; set; }

        public Pelicula Pelicula { get;set; }

    }
}
