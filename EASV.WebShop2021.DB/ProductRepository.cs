using System;
using System.Collections.Generic;
using System.IO.Pipes;
using EASV.Webshop2021.Core.Models;
using EASV.WebShop2021.DB.Entities;
using EASV.Webshop2021.Domain.IRepositories;

namespace EASV.WebShop2021.DB
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebShopDbContext _ctx;

        public ProductRepository(WebShopDbContext context)
        {
            _ctx = context;
        }
        
        public List<Product> ReadAll()
        {
            throw new System.NotImplementedException();
        }

        public Product CreateProduct(Product product)
        {
            var entity = _ctx.Products.Add(new ProductEntity
            {
                Id = product.Id,
                Name = product.Name
            }).Entity;

            return new Product
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public Product DeleteProduct(int id)
        {
            var entity = _ctx.Products.Remove(new ProductEntity {Id = id}).Entity;
            _ctx.SaveChanges();
            return new Product
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public Product UpdateProduct(int id, string Name)
        {
            var entity = _ctx.Products.Update(new ProductEntity {Id = id}).Entity;
            entity.Name = Name;
            return new Product
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}