using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIDemo.Filters
{
    public class LogExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            return File.AppendAllTextAsync("C:\\Users\\yangfan\\Desktop\\Log.txt",context.Exception.ToString());
        }
    }
}
