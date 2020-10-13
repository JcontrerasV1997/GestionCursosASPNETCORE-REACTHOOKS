using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MediatR;
using Persistencia;
using Aplicacion.CursosAplicacion;
using FluentValidation.AspNetCore;
using Aplicacion;
using WebAppi.Middleware;
using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Aplicacion.Interfaces;

namespace WebAppi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // con el parametro services y referenciado al metodo para agregar el contexto de tipo generico llamado cursosOnlineContext
            // y establezco la conexion desde el context en el webApi
            services.AddDbContext<CursosOnlineContext>(conexion => {
                conexion.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));});

            ///Incluciones para librerias 
            services.AddMediatR(typeof(Consulta.Manejador).Assembly);
            services.AddControllers(valor =>{
                var policy=new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                valor.Filters.Add(new AuthorizeFilter(policy));
            }).AddFluentValidation(validacion => validacion.RegisterValidatorsFromAssemblyContaining<Nuevo>());
            var builder= services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType,builder.Services);
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();

            //Servicio de Autenticacion
            var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("esta es mi palabra clave"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(valido=>
            {
                    valido.TokenValidationParameters= new TokenValidationParameters{
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=key,
                    ValidateAudience=false,
                    ValidateIssuer=false //el issuer es el envio del token, desde que ip quieres enviar estas indicando que esta validacion este en false
                    };
            });

            services.AddScoped<IJwtGenerador, JwtGenerador>();
            services.AddScoped<IUsuarioSesion, UsuarioSesion>();
    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();
            
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }
            
            // app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
