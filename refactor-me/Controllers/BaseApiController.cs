using refactor_me.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace refactor_me.Controllers
{
    [ApiExceptionFilter]
    public class BaseApiController : ApiController
    {
    }
}
