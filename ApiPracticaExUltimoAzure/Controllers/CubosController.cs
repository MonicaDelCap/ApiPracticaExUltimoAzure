using System.Security.Claims;
using ApiPracticaExUltimoAzure.Models;
using ApiPracticaExUltimoAzure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiPracticaExUltimoAzure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private CubosRepository repository;

        public CubosController(CubosRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Cubo>>> ListaCubos()
        {
            return await repository.GetCubosAsync();
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Cubo>> FindCubo(int id)
        {
            return await repository.FindCuboByIdAsync(id);
        }


        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> CreateUsuario([FromBody] UsuariosCubo usuario)
        {
            await repository.CreateUsuarioAsync(usuario.Nombre,usuario.Email,usuario.Password,usuario.Imagen);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<UsuariosCubo>> PerfilUsuario()
        {
            Claim claim = HttpContext.User.FindFirst(z => z.Type == "UserData");
            string json = claim.Value;
            UsuariosCubo usuario = JsonConvert.DeserializeObject<UsuariosCubo>(json);
            return await repository.PerfilUsuario(usuario.IdUsuario);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<CompraCubos>>> ComprasUsuario()
        {
            Claim claim = HttpContext.User.FindFirst(z => z.Type == "UserData");
            string json = claim.Value;
            UsuariosCubo usuario = JsonConvert.DeserializeObject<UsuariosCubo>(json);
            return await repository.ComprasCubos(usuario.IdUsuario);
        }

        [Authorize]
        [HttpPost]
        [Route("[action]/{idcubo}")]
        public async Task<ActionResult> CreateCompra(int idcubo)
        {
            Claim claim = HttpContext.User.FindFirst(z => z.Type == "UserData");
            string json = claim.Value;
            UsuariosCubo usuario = JsonConvert.DeserializeObject<UsuariosCubo>(json);
            await repository.RealizarPedido(usuario.IdUsuario, idcubo);
            return Ok();
        }
    }
}
