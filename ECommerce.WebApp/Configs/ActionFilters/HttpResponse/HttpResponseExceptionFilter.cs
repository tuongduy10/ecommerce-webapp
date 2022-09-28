using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Configs.ActionFilters.HttpResponse
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException httpResponseException)
            {
                context.Result = new ObjectResult(httpResponseException.Value)
                {
                    StatusCode = httpResponseException.StatusCode
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
