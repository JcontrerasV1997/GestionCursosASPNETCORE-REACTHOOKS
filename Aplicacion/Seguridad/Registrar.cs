using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>{
            public string Nombre{get;set;}
            public string Apellidos{get;set;}
            public string Email{get;set;}
            public string Password{get;set;}

            public string Username{get;set;}


        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
                public EjecutaValidador(){
                    RuleFor(validador => validador.Nombre).NotEmpty();
                    RuleFor(validador => validador.Apellidos).NotEmpty();
                    RuleFor(validador => validador.Email).NotEmpty();
                    RuleFor(validador => validador.Password).NotEmpty();
                    RuleFor(validador => validador.Username).NotEmpty();
                
                }
        }
        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext _context;
            private readonly UserManager<Usuario> _userManager;

            private readonly IJwtGenerador _jwtGenerador;
                
            public Manejador(CursosOnlineContext context, 
                            UserManager<Usuario> userManager, 
                            IJwtGenerador jwtGenerador ){
                _context=context;
                _userManager=userManager;
                _jwtGenerador=jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                        //Validacion del Email
                    var existe = await _context.Users.Where(registros => registros.Email == request.Email).AnyAsync();
                    if(existe){

                        throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje="El email ya tiene un usuario registrado"});
                    }

                    //Validacion del nombre de usuario
                    var existeUsername= await _context.Users.Where(evaluacion => evaluacion.UserName==request.Username).AnyAsync();
                    if(existeUsername){
                         throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new{mensaje="Actualmente existe un usuario con ese username"});   
                        
                        }

                            //Creacion del nuevo usuario
                        var usuarioNuevo=new Usuario{
                            NombreCompleto=request.Nombre + " " +request.Apellidos,
                            Email=request.Email,
                            UserName=request.Username
                        };

                       var resultado= await _userManager.CreateAsync(usuarioNuevo,request.Password);

                        if (resultado.Succeeded)
                        {
                            return new UsuarioData{
                                NombreCompleto=usuarioNuevo.NombreCompleto,
                                Token= _jwtGenerador.CrearToken(usuarioNuevo),
                                Username=usuarioNuevo.UserName,
                                Email=usuarioNuevo.Email
                            };
                        }

                        throw new Exception("No se pudo agregar al nuevo Usuarioo");
            }
        }
    }
}