using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using refactor_me.Controllers;
using refactor_me.Models;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Net;
using refactor_me.DataAccess;

namespace refactor_me.Tests
{
    [TestClass]
    public class ProductsControllerTest
    {
        ProductsController testController;

        [TestInitialize]
        public void TestInitialize()
        {
            testController = new ProductsController();
            testController.Request = new HttpRequestMessage();
            testController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());            
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllProducts()
        {
            var result = (testController.GetAll().Content as ObjectContent).Value as Products;            
            Assert.AreEqual(2, result.Items.Count);
        }

        [TestMethod]
        public void GetSearchByName_ShouldReturnResult()
        {
            var result = (testController.SearchByName("Apple").Content as ObjectContent).Value as Products;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(1, result.Items.Count);            
        }

        [TestMethod]
        public void GetSearchByName_ShouldReturnNoResult()
        {
            var result = (testController.SearchByName("Product non-existent").Content as ObjectContent).Value as Products;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public void GetProduct_ShouldReturnExistingProduct()
        {
            var existingId = new Guid("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var response = testController.GetProduct(existingId);
            var result = (response.Content as ObjectContent).Value as Product;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(existingId, result.Id);
            Assert.AreEqual(false, result.IsNew);
            Assert.AreEqual("Samsung Galaxy S7", result.Name);
        }

        public void GetProduct_ReturnNotFound()
        {
            var existingId = new Guid("8f2e9176-35ee-4f0a-ae55-83023d2db1a3");
            var response = testController.GetProduct(existingId);
            var result = (response.Content as ObjectContent).Value as Product;
            Assert.IsNotNull(response);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            Assert.IsNull(result);
        }

        [Ignore]
        [TestMethod]
        public void CreateProduct_NewProductCreated()
        {
            var guid = Guid.NewGuid();
            var expected = new Product(guid, true)
            {
                Name = "Raspberry Mobile",
                Description = "Modular Mobile Phone",
                Price = 1000,
                DeliveryPrice = 10
            };
            testController.CreateProduct(expected);
            
            var response = testController.GetProduct(guid);
            var result = (response.Content as ObjectContent).Value as Product;
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.Description, result.Description);
            Assert.AreEqual(expected.Price, result.Price);
            Assert.AreEqual(expected.DeliveryPrice, result.DeliveryPrice);
        }
    }
}
