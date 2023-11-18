using EvaluacionHunter.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EvaluacionHunter.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Crea un usuario para poder utilizar los endpoints.
        /// </summary>
        /// <param name="CredencialesUsuario"></param>
        /// <returns></returns>
        [HttpPost("registrar")] //api/cuentas/registrar
        public async Task<ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario CredencialesUsuario)
        {
            var usuario = new IdentityUser { UserName = CredencialesUsuario.Email, Email = CredencialesUsuario.Email };
            var resultado = await userManager.CreateAsync(usuario, CredencialesUsuario.Password);

            if (resultado.Succeeded)
            {
                return ConstruirToken(CredencialesUsuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        /// <summary>
        /// Te Crea el json necesario para poder autenticarte y poder utilizar los endpoints.
        /// </summary>
        /// <param name="CredencialesUsuario"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario CredencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(CredencialesUsuario.Email, CredencialesUsuario.Password
                , isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return ConstruirToken(CredencialesUsuario);
            }
            else
            {
                return BadRequest("Login Incorrecto");
            }


        }

        private RespuestaAutenticacion ConstruirToken(CredencialesUsuario CredencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email",CredencialesUsuario.Email)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwT"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddHours(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                FechaExpiracion = expiracion,
            };
        }

    }
}
