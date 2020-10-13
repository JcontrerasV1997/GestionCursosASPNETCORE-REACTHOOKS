using System;

namespace Dominio
{
    public class CursoInstructor
    {
        public Guid InstructorId { get; set; }

        public Guid CursosId { get; set; }

        public Instructor Instructor { get; set; }

        public Cursos Curso { get; set; }
        
    }
}