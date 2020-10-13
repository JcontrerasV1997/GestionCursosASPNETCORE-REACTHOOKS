
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Precio
    {
        public Guid PrecioId { get; set; }
        //Longitud del decimal
        [Column(TypeName = "decimal(18,4)")]

        public decimal PrecioActual { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Promocion { get; set; }

        public Guid CursosId { get; set; }
    }

}