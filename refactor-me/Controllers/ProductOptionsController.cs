using refactor_me.ErrorHandling;
using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace refactor_me.Controllers
{
    [RoutePrefix("products/{productId}/options")]
    public class ProductOptionsController : BaseApiController
    {
        [Route]
        [HttpGet]
        public HttpResponseMessage GetOptions(Guid productId)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new ProductOptions(productId));
        }

        [Route("{id}")]
        [HttpGet]
        public HttpResponseMessage GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    ErrorProvider.ReturnError(ErrorCode.ProductNotFound).WrapInPayload());
                
            return Request.CreateResponse(HttpStatusCode.OK, option);
        }

        [Route]
        [HttpPost]
        public HttpResponseMessage CreateOption(Guid productId, ProductOption option)
        {
            var orig = new ProductOption(option.Id);
            orig.CopyFrom(option);
            orig.ProductId = productId;
            orig.Save();

            return orig.IsNew
                ? Request.CreateResponse(HttpStatusCode.Created, option)
                : Request.CreateResponse(HttpStatusCode.OK, option);
        }

        [Route("{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id);

            // Put verb is idempotent. 
            // Hence, if resource is not yet created, client should trigger a post request first
            if (orig.IsNew)
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    ErrorProvider.ReturnError(ErrorCode.ProductNotFound).WrapInPayload());

            orig.CopyFrom(option);
            orig.Save();

            return Request.CreateResponse(HttpStatusCode.OK, orig);
        }

        [Route("{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteOption(Guid productId, Guid id)
        {
            var opt = new ProductOption(id);
            if (opt.IsNew)
                return Request.CreateResponse(HttpStatusCode.NotFound,
                        ErrorProvider.ReturnError(ErrorCode.ProductNotFound).WrapInPayload());

            if (opt.ProductId != productId)
                return Request.CreateResponse(HttpStatusCode.NotFound,
                        ErrorProvider.ReturnError(ErrorCode.ProductNotFound).WrapInPayload());

            opt.Delete();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}