using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dominio;
using MediatR;
using Persistencia;
namespace Aplicacion.CursosAplicacion
{
    // en Aplicacion se desarrolla toda la logica de negocio, el retorno de entidades en este caso las consultas
    public class Consulta
    {
        public class ListaCursos: IRequest<List<Cursos>>{}

        public class Manejador : IRequestHandler<ListaCursos, List<Cursos>>
        {
            private readonly CursosOnlineContext _context; //OBJETO DE TIPO CURSOSCONTEX referenciado para poder colocarlo dentro del constructor
                                                            // de manejador
            public Manejador(CursosOnlineContext context){
                    _context= context;
            }
            public async Task<List<Cursos>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                    var cursos = await _context.Cursos
                    .Include(valor => valor.InstructorLink)
                    .ThenInclude(valor => valor.Instructor).ToListAsync();
                    return cursos;
            
            }
        }

    }
}