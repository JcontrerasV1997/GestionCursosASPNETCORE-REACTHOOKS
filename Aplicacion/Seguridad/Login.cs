using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta: IRequest<UsuarioData>/// Si coloco Usuario me devuelve todo lo del usuario pero no lo esencial
        {
            public string Email {get; set;}
            public string Password {get;set;}
        }

        public class EjecutaValidacion: AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion(){

                RuleFor(Validar => Validar.Email).NotEmpty();
                RuleFor(Validar=> Validar.Password).NotEmpty();
            }
        }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _singInManager;
            
            private readonly IJwtGenerador _jwtGenerador;
            public Manejador(UserManager<Usuario> userManager,SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador)
            {
                _userManager=userManager;
                _singInManager= signInManager;
                _jwtGenerador=jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario= await _userManager.FindByEmailAsync(request.Email);
                    
                    if (usuario == null)
                    {
                        throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);  
                    }

                   var resultado= await _singInManager.CheckPasswordSignInAsync(usuario,request.Password,false);
                        if (resultado.Succeeded)
                        {
                            return new UsuarioData{

                                       NombreCompleto=usuario.NombreCompleto,
                                        Token = _jwtGenerador.CrearToken(usuario),
                                        Username=usuario.UserName,
                                        Email=usuario.Email,
                                        Imagen=null 

                            };
                        }
                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);  
            }
        }
    }
}