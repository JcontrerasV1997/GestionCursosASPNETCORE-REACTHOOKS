using Dominio;

namespace Aplicacion
{
    public interface IJwtGenerador
    {
         string CrearToken(Usuario usuario);
    }
}