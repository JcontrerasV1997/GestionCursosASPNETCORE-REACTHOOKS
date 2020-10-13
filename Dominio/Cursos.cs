using System;
using System.Collections;
using System.Collections.Generic;

namespace Dominio
{
    public class Cursos
    {
        public Guid CursosId { get; set; }

        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        // public byte[] FotoPortada{get; set;}

        
        public Precio PrecioPromocion { get; set; } /// curso tiene a precio

        public ICollection<Comentario> ComentarioLista { get; set; }//un curso posee varios comentarios por eso lo meto en un en una coleccion generica
        
        public ICollection<CursoInstructor> InstructorLink { get; set; }//Representacion de relacion muchos a muchos, curso tiene varios registros de CursoInstructor   
    }
}