using System.Linq;
using System.Security.Claims;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Seguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor= httpContextAccessor;

        }
        public string ObtenerUsuarioSesion()
        {

            var userName=_httpContextAccessor.HttpContext.User?.Claims?.
                        FirstOrDefault(validacion => validacion.Type==ClaimTypes.NameIdentifier)?.Value;
                        return userName;

        }
    }
}