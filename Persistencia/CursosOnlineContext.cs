using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Persistencia
{
    public class CursosOnlineContext: IdentityDbContext<Usuario>//DbContext
    {
        public CursosOnlineContext(DbContextOptions options): base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CursoInstructor>().HasKey(LlaveCompuesta => new {
            
            LlaveCompuesta.InstructorId, 
            LlaveCompuesta.CursosId
            
                }); 
        }
                        
        public DbSet<Cursos> Cursos { get; set; }
        
        public DbSet<Precio> Precio { get; set; }

        public DbSet<Comentario> Comentario { get; set; }
       
        public DbSet<Instructor> Instructor { get; set; }

        public DbSet<CursoInstructor> CursoInstructor { get; set; }


 } 
}
