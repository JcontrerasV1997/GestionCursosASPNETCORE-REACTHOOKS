using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
namespace Aplicacion
{
    public class Nuevo
    {
        // estas entidades seran las que se colocaran desde posman osea el mapeo
        public class Ejecuta : IRequest{
        
        // public int CursosId { get; set; }
        // Anotaciones tipo required para validar campos faciles 

        // [Required(ErrorMessage="Por Favor Ingrese el Titulo del Curso")]

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime? FechaPublicacion {get;set;}

        public List<Guid> ListaInstructor{get; set;}
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
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Guid _cursoId= Guid.NewGuid();
                var curso=new Cursos{
                    CursosId= _cursoId,
                    Titulo=request.Titulo,
                    Descripcion=request.Descripcion,
                    FechaPublicacion=request.FechaPublicacion
                };
                _context.Cursos.Add(curso);
                ////Validacion para tablas intermedias
                if (request.ListaInstructor!=null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                          var cursoInstructor= new CursoInstructor{
                                    CursosId= _cursoId,
                                    InstructorId= id
                            };
                            _context.CursoInstructor.Add(cursoInstructor);
                    }
                }

               var valor= await _context.SaveChangesAsync();
               
               if(valor>0){
                   return Unit.Value;
               }

               throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}