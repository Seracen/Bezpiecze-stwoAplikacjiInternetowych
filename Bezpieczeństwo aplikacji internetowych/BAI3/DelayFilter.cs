using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using BAI3.Context;
using BAI3.Models;

namespace BAI3
{
    public class DelayFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        public DelayFilter()
        {
        }
        async Task IAsyncActionFilter.OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var c = context.HttpContext.RequestServices.GetService(typeof(BaiContext)) as BaiContext;
            LoginAttepmts newLoginAttempt;
            if (c.LoginAttempts.ToList().Count == 0)
            {
                newLoginAttempt = new LoginAttepmts(0);
            }
            else
            {
                newLoginAttempt = c.LoginAttempts.First();
            }
            await Task.Delay(500 * newLoginAttempt.attempt);
            await next();
        }
    }
}
