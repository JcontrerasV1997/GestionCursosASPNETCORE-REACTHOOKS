using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using FluentValidation;
using MediatR;
using Persistencia;
namespace Aplicacion.CursosAplicacion
{
    public class Editar
    {
        public class Ejecuta: IRequest{
        public int CursosId { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime?  FechaPublicacion { get; set; }
        }

        public class EjecutaValidacion: AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {

             RuleFor(validacion => validacion.Titulo).NotEmpty();
             RuleFor(validacion => validacion.Descripcion).NotEmpty();
             RuleFor(validacion => validacion.FechaPublicacion).NotEmpty();

            }
            
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context){
                    _context=context;

            }

            // Metodo asincrono para actualizar, dentro de el referencio todo.
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var curso= await _context.Cursos.FindAsync(request.CursosId);
                if(curso==null){
                    // throw new Exception("el curso no existe");
                     throw new ManejadorExcepcion(HttpStatusCode.NotFound,new {mensaje="No se Encontro el Curso"});
                }
                curso.Titulo= request.Titulo ?? request.Titulo;
                curso.Descripcion= request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion=request.FechaPublicacion ?? curso.FechaPublicacion;
                var resultado = await _context.SaveChangesAsync();
                    if (resultado>0)
                        return Unit.Value;
                        throw new Exception("No se guardaron los cambios en el curso");  
            }
        }
    }
}