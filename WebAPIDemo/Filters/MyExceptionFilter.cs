using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIDemo.Filters
{
    public class MyExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _environment;

        public MyExceptionFilter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            string message;
            if (_environment.IsDevelopment())
            {
                message = context.Exception.ToString();
            }
            else
            {
                message = "服务端发生未知异常";
            }
            ObjectResult objectResult = new ObjectResult(new { code=500,message});
            context.Result = objectResult;
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
