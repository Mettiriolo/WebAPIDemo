using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Transactions;

namespace WebAPIDemo.Filters
{
    public class TransactionScopeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor actionDesc)
            {
                bool hasNotTransactionScopeAttribute = actionDesc.MethodInfo
                    .GetCustomAttributes(typeof(NotTransactionalAttribute), false).Any();//检查 NotTransactionalAttribute特性

                if (hasNotTransactionScopeAttribute)
                {
                    await next();//Action方法
                    return;
                }
                using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var result = await next();
                if (result.Exception == null)
                {
                    transactionScope.Complete();
                }
            }
        }
    }
}
