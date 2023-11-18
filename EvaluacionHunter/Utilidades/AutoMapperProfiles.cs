using AutoMapper;
using EvaluacionHunter.DTOs;
using EvaluacionHunter.Entidades;
using static System.Reflection.Metadata.BlobBuilder;

namespace EvaluacionHunter.Utilidades
{
    public class AutoMapperProfiles : Profile
    {

        public AutoMapperProfiles() {

            CreateMap<ActorCreacionDTO, Actor>();
            CreateMap<Actor, ActorDTO>();
            CreateMap<Actor, ActorDTOConPeliculas>()
         .ForMember(x => x.Peliculas, opciones => opciones.MapFrom(MapActorDTOConPeliculas));
            
            
            CreateMap<Pelicula, PeliculaDTO>();
            CreateMap<Pelicula, PeliculaDTOConActores>()
            .ForMember(x => x.Actores, opciones => opciones.MapFrom(MapPeliculaDTOActores));
            CreateMap<PeliculaCreacionDTO, Pelicula>()
          .ForMember(pelicula => pelicula.ActoresPeliculas, opciones => opciones.MapFrom(MapActoresLibros));


        }



        private List<PeliculaDTO> MapActorDTOConPeliculas(Actor actor, ActorDTO actorDTO)
        {
            var resultado = new List<PeliculaDTO>();

            if (actor.ActoresPeliculas == null) { return resultado; }

            foreach (var actorPelicula in actor.ActoresPeliculas)
            {
                resultado.Add(new PeliculaDTO()
                {
                    Id = actorPelicula.PeliculaId,
                    Titulo = actorPelicula.Pelicula.Titulo,
                    Genero = actorPelicula.Pelicula.Genero,
                    Idioma=actorPelicula.Pelicula.Idioma,
                    Duracion = actorPelicula.Pelicula.Duracion
                });
            }

            return resultado;

        }

        private List<ActorDTO> MapPeliculaDTOActores(Pelicula pelicula, PeliculaDTO peliculaDTO)
        {
            var resultado = new List<ActorDTO>();
            if (pelicula.ActoresPeliculas == null) { return resultado; }

            foreach (var actorDePelicula in pelicula.ActoresPeliculas)
            {
                resultado.Add(new ActorDTO()
                {
                    Id = actorDePelicula.ActorId,
                    Nombre = actorDePelicula.Actor.Nombre,
                    Apellido =actorDePelicula.Actor.Apellido,
                    FechaNacimiento=actorDePelicula.Actor.FechaNacimiento,
                    Nacionalidad=actorDePelicula.Actor.Nacionalidad
                });
            }
            return resultado;

        }

        private List<ActorPelicula> MapActoresLibros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<ActorPelicula>();

            if (peliculaCreacionDTO.ActoresIds == null) { return resultado; }

            foreach (var actorId in peliculaCreacionDTO.ActoresIds)
            {
                resultado.Add(new ActorPelicula() { ActorId = actorId });
            }

            return resultado;
        }

    }
}
