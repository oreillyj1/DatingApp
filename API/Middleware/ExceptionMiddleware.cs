using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        public ILogger<ExceptionMiddleware> _logger { get; }
        public RequestDelegate _next { get; }
 
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate request, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = request;
        }

        public async Task InvokeAsync(HttpContext context) {
            try {
                await _next(context);
            }catch(Exception ex) {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType="application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment() 
                ? new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode,"internal server errror");

                var options = new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};

                var json= JsonSerializer.Serialize(response,options);

                await context.Response.WriteAsync(json);
            }
        }
 
    }
}