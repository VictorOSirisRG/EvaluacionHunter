using EvaluacionHunter.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EvaluacionHunter
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);  

            builder.Entity<ActorPelicula>().HasKey(x => new { x.PeliculaId, x.ActorId });

        }

        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; } 

        public DbSet<ActorPelicula> ActoresPeliculas { get; set; }
    }
}
