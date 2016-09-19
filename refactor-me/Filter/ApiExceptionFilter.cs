using refactor_me.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace refactor_me.Filter
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var error = ErrorProvider.ReturnGenericFromException(actionExecutedContext.Exception);
            actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                                    { Content = new ObjectContent(typeof(Error),error, new JsonMediaTypeFormatter()) };
        }
    }
}