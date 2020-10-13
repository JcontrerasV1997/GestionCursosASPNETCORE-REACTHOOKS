using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion;
using Aplicacion.CursosAplicacion;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebAppi.Controllers
{

    //En esta carpeta estan todos los controladores del proyecto, un controlador es una clase que hereda de controller base
    // esta intermedia entre la base de datos y el proyecto, ademas contiene las instrucciones y peticiones http aplicadas por anotaciones
    // ENDPOINT DECLARADO http://localhost:5000/api/Cursos
    [Route ("api/[controller]")] //endpoint referenciando la direccion de la api/nombre del controlador
    [ApiController]
    public class CursosController: ControllerRecipiente
    {
       

        [HttpGet]
        public async Task<ActionResult<List<Cursos>>>Get(){

            return await Mediator.Send(new Consulta.ListaCursos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cursos>> Detalle(int id){
                return await Mediator.Send(new ConsultaId.CursoUnico(Id:id));
        }

        [HttpPost]
        public async Task<ActionResult<Unit>>Crear(Nuevo.Ejecuta data)
        {
           return await Mediator.Send(data);

        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(int id, Editar.Ejecuta data)
        {
            data.CursosId =id;
            return await Mediator.Send(data);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(int id){

            return await Mediator.Send(new Eliminar.Ejecuta(Id:id));
        }

    }
}