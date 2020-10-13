using System;
using System.Net;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAppi.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErrorMiddleware> _logger;
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger)
        {
            _next=next;
            _logger=logger;

        }
        public async Task Invoke(HttpContext context)
        {
            try{
                 await _next(context);

            }
            catch(Exception ex){

                    await ManejadorExcepcionAsincrono(context,ex, _logger);
            }

        }

        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger)
        {
            object errores =null;
            switch(ex){
                case ManejadorExcepcion manejador :
                    logger.LogError(ex,"manejador error");
                    errores= manejador.Errores;
                    context.Response.StatusCode=(int) manejador.Codigo;
                    break;
                case Exception e:
                    logger.LogError(ex, "error de servidor");
                    errores=string.IsNullOrWhiteSpace(e.Message) ? "Error":e.Message;
                    context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType="application/json";
            
            if(errores != null){
                var resultado=JsonConvert.SerializeObject(new {errores});
                await context.Response.WriteAsync(resultado);
            }
        }
    }
}