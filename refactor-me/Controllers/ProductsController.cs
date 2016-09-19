using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using refactor_me.Models;
using refactor_me.DataAccess;
using refactor_me.ErrorHandling;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : BaseApiController
    {
        [Route]
        [HttpGet]
        public HttpResponseMessage GetAll()
        {            
            return Request.CreateResponse(HttpStatusCode.OK, new Products());
        }

        [Route]
        [HttpGet]
        public HttpResponseMessage SearchByName(string name)
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Products(name));
        }

        [Route("{id}")]
        [HttpGet]
        public HttpResponseMessage GetProduct(Guid id)
        {
            var product = new Product(id);
            if (product.IsNew)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, 
                    ErrorProvider.ReturnError(ErrorCode.ProductNotFound).WrapInPayload());
            }                

            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        [Route]
        [HttpPost]
        public HttpResponseMessage CreateProduct(Product product)
        {
            var orig = new Product(product.Id);          
            orig.CopyFrom(product);
            orig.Save();
            return orig.IsNew
                        ? Request.CreateResponse(HttpStatusCode.Created, product)
                        : Request.CreateResponse(HttpStatusCode.OK, product);
        }

        [Route("{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateProduct(Guid id, Product product)
        {
            var orig = new Product(id);

            // Put verb is idempotent. 
            // Hence, if resource is not yet created, client should trigger a post request first
            if (orig.IsNew)
            {                
                Request.CreateResponse(HttpStatusCode.NotFound,
                            ErrorProvider.ReturnError(ErrorCode.ProductNotFound).WrapInPayload());
            }                

            orig.CopyFrom(product);
            orig.Save();
            return Request.CreateResponse(HttpStatusCode.OK, orig);
        }

        [Route("{id}")]
        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            var toDelete = new Product(id);
            if (toDelete.IsNew)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                            ErrorProvider.ReturnError(ErrorCode.ProductNotFound).WrapInPayload());
            }
            toDelete.Delete();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
