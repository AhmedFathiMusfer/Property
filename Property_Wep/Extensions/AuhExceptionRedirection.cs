using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Property_Wep.Services;

namespace Property_Wep.Extensions
{
    public class AuhExceptionRedirection : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if(context.Exception is AuthException ) {

                context.Result = new RedirectToActionResult("Login","Auth",null);
            }
        }
    }
}
