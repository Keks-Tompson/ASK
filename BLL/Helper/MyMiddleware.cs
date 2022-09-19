using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ASK.BLL.Helper
{
    public class MyMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next.Invoke(context);
            await context.Response.WriteAsync($"<br>Status code - {context.Response.StatusCode}");
        }
    }
}
