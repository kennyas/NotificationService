using Microsoft.Extensions.Primitives;
using MKopaMessageBox.AppCore.Interfaces;
using MKopaMessageBox.MKopaMessageBox.Domain.DTOs;
using System.Net;

namespace MKopaMessageBox.Extenstions.Middleware
{
    public class KeyAccessMiddleware
    {
        public readonly RequestDelegate _next;
        public KeyAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IAccessKeyRepository accessKey)
        {
            StringValues UserKeyValue;
            context.Request.Headers.TryGetValue(AppConstants.DefaultHeaderKeyName, out UserKeyValue);
            if (string.IsNullOrEmpty(UserKeyValue))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
            

        }
    }
}
