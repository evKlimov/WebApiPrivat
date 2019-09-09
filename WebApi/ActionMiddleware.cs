using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
{
    public class ActionMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ActionMiddleware> _logger;

        public ActionMiddleware(RequestDelegate next, ILogger<ActionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var obj = new
            {
                context.Connection.RemoteIpAddress,
                context.Connection.RemotePort,
                context.Connection.LocalIpAddress,
                context.Connection.LocalPort
            };
            _logger.LogInformation($"{context.Request.Path}: {obj.ToString()}");
            await _next.Invoke(context);
        }
    }
}
