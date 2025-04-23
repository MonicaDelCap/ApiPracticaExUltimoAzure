using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiPracticaExUltimoAzure.Helpers;
using ApiPracticaExUltimoAzure.Models;
using ApiPracticaExUltimoAzure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ApiPracticaExUltimoAzure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private CubosRepository repo;
        private HelperOAuth helper;

        public AuthController(CubosRepository repo, HelperOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            UsuariosCubo usuario = await this.repo.LoginUsuario(model.UserName, model.Password);
            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {
                SigningCredentials credentials = new SigningCredentials(this.helper.GetKeyToken(), SecurityAlgorithms.HmacSha256);
                string jsonEmpleado = JsonConvert.SerializeObject(usuario);

                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonEmpleado)

                };

                JwtSecurityToken token =
                    new JwtSecurityToken(
                        claims: informacion,
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(20),
                        notBefore: DateTime.UtcNow
                        );

                return Ok(new
                {
                    response = new JwtSecurityTokenHandler().WriteToken(token)

                });
            }
        }

    }
}
