using System.Collections.Generic;
using System.Linq;
using EASV.Webshop2021.Core.Models;
using EASV.WebShop2021.DB.Entities;
using EASV.Webshop2021.Domain.IRepositories;

namespace EASV.WebShop2021.DB.Repositories
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
            return _ctx.Products.Select(entity => new Product
            {
                Id = entity.Id,
                Name = entity.Name
            }).ToList();
        }

        public Product CreateProduct(Product product)
        {
            var entity = _ctx.Products.Add(new ProductEntity
            {
                Id = product.Id,
                Name = product.Name
            }).Entity;

            _ctx.SaveChanges();

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

        public Product UpdateProduct(Product product)
        {
            var entity = _ctx.Products.Update(new ProductEntity
            {
                Id = product.Id,
                Name = product.Name
            }).Entity;
            
            _ctx.SaveChanges();
            
            return new Product
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}