using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using refactor_me.DataAccess;

namespace refactor_me.Models
{
    public class Products
    {
        IProductDataMapper dal;

        public List<Product> Items { get; private set; }

        IProductDataMapper Dal
        {
            get
            {
                if (dal != null)
                    return dal;

                dal = DataMapperFactory<IProductDataMapper>.GetInstance();
                return dal;
            }
        }

        public Products()
        {
            Items = Dal.FindAll().ToList();
        }

        public Products(string name)
        {
            Items = Dal.FindByName(name).ToList();
        }
    }

    public class Product
    {
        IProductDataMapper dal;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        
        [JsonIgnore]
        public bool IsNew { get; }

        IProductDataMapper Dal
        {
            get
            {
                if (dal != null)
                    return dal;

                dal = DataMapperFactory<IProductDataMapper>.GetInstance();
                return dal;
            }
        }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(Guid id, bool isNew)
        {
            Id = id;
            IsNew = isNew;
        }

        public Product(Guid id)
        {
            IsNew = true;
            Id = id;
            
            var product = Dal.FindById(id);
            if (product == null)
                return;

            IsNew = false;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            DeliveryPrice = product.DeliveryPrice;
        }

        public void CopyFrom(Product source)
        {
            Name = source.Name;
            Price = source.Price;
            Description = source.Description;
            DeliveryPrice = source.DeliveryPrice;
        }

        public void Save()
        {
            Dal.Upsert(this);
        }

        public void Delete()
        {
            Dal.Delete(this);
        }
    }

    public class ProductOptions
    {
        IProductOptionDataMapper dal;

        IProductOptionDataMapper Dal
        {
            get
            {
                if (dal != null)
                    return dal;

                dal = DataMapperFactory<IProductOptionDataMapper>.GetInstance();
                return dal;
            }
        }

        public List<ProductOption> Items { get; private set; }

        public Guid ProductId { get; private set; }

        public ProductOptions()
        {
            Items = Dal.FindAll().ToList();
        }

        public ProductOptions(Guid productId)
        {
            ProductId = productId;
            Items = Dal.FindByProductId(productId).ToList();
        }
    }

    public class ProductOption
    {
        IProductOptionDataMapper dal;

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        IProductOptionDataMapper Dal
        {
            get
            {
                if (dal != null)
                    return dal;

                dal = DataMapperFactory<IProductOptionDataMapper>.GetInstance();
                return dal;
            }
        }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id, bool isNew)
        {
            IsNew = false;
            Id = id;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            Id = id;

            var productOption = Dal.FindById(id);
            if (productOption == null)
                return;

            IsNew = false;
            ProductId = productOption.ProductId;
            Name = productOption.Name;
            Description = productOption.Description;
        }

        public void Save()
        {
            Dal.Upsert(this);
        }

        public void Delete()
        {
            Dal.Delete(this);
        }

        public void CopyFrom(ProductOption source)
        {
            Name = source.Name;
            Description = source.Description;
        }
    }
}