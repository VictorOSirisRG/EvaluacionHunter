using AutoMapper;
using EvaluacionHunter.DTOs;
using EvaluacionHunter.Entidades;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EvaluacionHunter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class ActorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper mapper;


        public ActorController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los actores creados sin stored Procedured
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ActorDTO>> GetAll()
        {
            var actores = await _context.Actores.ToListAsync();

            return mapper.Map<List<ActorDTO>>(actores);
        }

        /// <summary>
        /// Obtiene todos los actores creados por stored procedured
        /// </summary>
        /// <returns></returns>
        [HttpGet("SP")]
        public async Task<List<ActorDTO>> GetAllSP()
        {
            var actores = await _context.Actores.FromSqlRaw("ObtenerTodoslosAutores").ToListAsync();

            return mapper.Map<List<ActorDTO>>(actores);
        }
        /// <summary>
        /// Obtiene un actor en especifico por su id, ademas trae las peliculas en la cual el actor participa.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "ObtenerActor")]
        public async Task<ActionResult<ActorDTOConPeliculas>> Get(int id)
        {
            var actor = await _context.Actores
                .Include(x => x.ActoresPeliculas)
                .ThenInclude(x => x.Pelicula)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }

            return mapper.Map<ActorDTOConPeliculas>(actor);
        }

        /// <summary>
        /// Crea un Actor
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Post(ActorCreacionDTO obj)
        {

            var actorExiste = await _context.Actores.AnyAsync(x => x.Nombre == obj.Nombre && x.Apellido == obj.Apellido);
            if (actorExiste)
            {
                return BadRequest($"El actor  {obj.Nombre} {obj.Apellido} ya existe.");
            }

            var actor = mapper.Map<Actor>(obj);

            _context.Actores.Add(actor);
            await _context.SaveChangesAsync();

            var actorDTO = mapper.Map<ActorDTO>(actor);

            return CreatedAtRoute("ObtenerActor", new { id = actor.Id }, actorDTO);
        }

        /// <summary>
        /// Editar un actor
        /// </summary>
        /// <param name="actorDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ActorCreacionDTO actorDTO, int id)
        {

            var existe = await _context.Actores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();

            
            var actor = mapper.Map<Actor>(actorDTO);

            actor.Id= id;


            _context.Actores.Update(actor);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Elimina un actor.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Actores.AnyAsync(x => x.Id == id);

            if (!existe)
                return NotFound();


            _context.Remove(new Actor { Id = id });
            await _context.SaveChangesAsync();
            return Ok();




        }




    }
}
