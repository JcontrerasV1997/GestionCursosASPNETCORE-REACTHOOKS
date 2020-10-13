using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebAppi.Controllers
{
    [AllowAnonymous]
    public class UsuarioController: ControllerRecipiente
    {
        // http://localhost:5000/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login(Login.Ejecuta datos)
        {
                return await Mediator.Send(datos);
        }

        // http://localhost:5000/api/Usuario/login
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioData>> Registrar(Registrar.Ejecuta valores){
                return await Mediator.Send(valores);

        }

        // http://localhost:5000/api/Usuario
        [HttpGet]
        public async Task<ActionResult<UsuarioData>> DevolverUsuario(){

                    return await Mediator.Send(new UsuarioActual.Ejecutar());

        }
    }
}