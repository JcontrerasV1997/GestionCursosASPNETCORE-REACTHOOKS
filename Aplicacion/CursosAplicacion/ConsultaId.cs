using MediatR;
using Dominio;
using System.Threading;
using System.Threading.Tasks;
using Persistencia;
using Aplicacion.ManejadorError;
using System.Net;
namespace Aplicacion.CursosAplicacion

{
    //http://localhost:5000/api/Cursos/{id}
    public class ConsultaId
    {
        public class CursoUnico:IRequest<Cursos>
        {
            public int Id{get; set;}

            public CursoUnico(int Id){
                this.Id=Id;
            }
        }
        public class Manejador : IRequestHandler<CursoUnico, Cursos>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context=context;

            }
            public async Task<Cursos> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                   var curso= await _context.Cursos.FindAsync(request.Id);
                    if(curso==null){
                         throw new ManejadorExcepcion(HttpStatusCode.NotFound,new {mensaje="No se Encontro el Curso"});
                    }
                    return curso;
            }
        }
    }
}