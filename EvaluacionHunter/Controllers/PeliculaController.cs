using AutoMapper;
using EvaluacionHunter.DTOs;
using EvaluacionHunter.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace EvaluacionHunter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeliculaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;


        public PeliculaController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Obtiene todas las peliculas sin stored procedure.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PeliculaDTO>> GetAll()
        {
            var peliculas = await _context.Peliculas.ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }

        /// <summary>
        /// Obtiene todas las peliculas por stored procedure.
        /// </summary>
        /// <returns></returns>
        [HttpGet("SP")]
        public async Task<List<PeliculaDTO>> GetAllSP()
        {
            var peliculas = await _context.Peliculas.FromSqlRaw("ObtenerTodasLasPeliculas").ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }



        /// <summary>
        /// Obtiene una pelicula por su id, y los actores que estan en ella.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ObtenerPelicula")]
        public async Task<ActionResult<PeliculaDTOConActores>> Get(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(x => x.ActoresPeliculas)
                .ThenInclude(x => x.Actor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return mapper.Map<PeliculaDTOConActores>(pelicula);
        }

        /// <summary>
        /// Crea una pelicula.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(PeliculaCreacionDTO obj)
        {

            var peliculaExiste = await _context.Peliculas.AnyAsync(x => x.Titulo == obj.Titulo);
            if (peliculaExiste)
            {
                return BadRequest($"La  pelicula  {obj.Titulo} ya existe.");
            }

            if (obj.ActoresIds == null)
            {
                return BadRequest("No se puede crear una pelicula sin actores");
            }

            var actoresIds = await _context.Actores.Where(x => obj.ActoresIds.Contains(x.Id))
                .Select(x => x.Id).ToListAsync();

            if (obj.ActoresIds.Count != actoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var pelicula = mapper.Map<Pelicula>(obj);

            _context.Peliculas.Add(pelicula);

            await _context.SaveChangesAsync();

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            return CreatedAtRoute("ObtenerPelicula", new { id = pelicula.Id }, peliculaDTO);
        }

        /// <summary>
        /// Edita una pelicula
        /// </summary>
        /// <param name="id"></param>
        /// <param name="peliculaCreacionDTO"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peliculaDb = await _context.Peliculas
                .Include(x => x.ActoresPeliculas)
                .FirstOrDefaultAsync(x => x.Id == id);

            var actoresIds = await _context.Actores.Where(x => peliculaCreacionDTO.ActoresIds.Contains(x.Id))
             .Select(x => x.Id).ToListAsync();

            if (peliculaCreacionDTO.ActoresIds.Count != actoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            if (peliculaDb == null)
            {
                return NotFound();
            }

            peliculaDb = mapper.Map(peliculaCreacionDTO, peliculaDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }
      
        /// <summary>
        /// Elimina una pelicula
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Peliculas.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();


            _context.Remove(new Pelicula { Id = id });
            await _context.SaveChangesAsync();
            return Ok();

        }




    }
}
