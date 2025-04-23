using ApiPracticaExUltimoAzure.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiPracticaExUltimoAzure.Helpers
{
    public class HelperSecurityUser
    {
        private IHttpContextAccessor contextAccessor;

        public HelperSecurityUser(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public UsuariosCubo GetUser()
        {
            Claim claim = contextAccessor.HttpContext.User.FindFirst(x => x.Type == "UserData");
            string jsonUser = claim.Value;
            UsuariosCubo user = JsonConvert.DeserializeObject<UsuariosCubo>(jsonUser);
            return user;
        }
    }
}
