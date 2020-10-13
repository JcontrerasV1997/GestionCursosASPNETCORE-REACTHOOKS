using System.Linq;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace Persistencia
{
    public class DataPrueba
    {
        public  static async Task InsertarDatos(CursosOnlineContext context, UserManager<Usuario> usuarioManager)
        {
           if(!usuarioManager.Users.Any())//valida si existe un usuario dentro de la base de datos      
           {
                var usuario=new Usuario
                {
                    NombreCompleto="juan manuel", 
                    UserName="juanqui", 
                    Email="juanqui@gmail.com"
                };
                await usuarioManager.CreateAsync(usuario, "Peo123456$");
           } 
        }
    }
}