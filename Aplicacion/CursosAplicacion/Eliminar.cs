using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion
{
    public class Eliminar
    {
        public class Ejecuta:IRequest
        {
                public int Id {get;set;}
                public Ejecuta(int Id){
                     this.Id=Id;
                }            
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {

             private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context){
                    _context=context;

            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {

                var curso= await _context.Cursos.FindAsync(request.Id);

                    if (curso==null)
                    {
                        // throw new Exception("no se puede eliminar el curso");
                        throw new ManejadorExcepcion(HttpStatusCode.NotFound,new {mensaje="No se Encontro el Curso"});
                    }
                    _context.Remove(curso);

                    var resultado=await _context.SaveChangesAsync();

                    if (resultado>0)
                    {
                        return Unit.Value;
                    }
                    
                    throw new Exception("No se pudieron eliminar");
            }
        }
    }
}