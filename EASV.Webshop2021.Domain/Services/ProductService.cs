using System.Collections.Generic;
using System.IO;
using System.Linq;
using EASV.Webshop2021.Core.IServices;
using EASV.Webshop2021.Core.Models;
using EASV.Webshop2021.Domain.IRepositories;

namespace EASV.Webshop2021.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            if (productRepository == null)
            {
                throw new InvalidDataException("Product repository cannot be null");
            }

            _productRepository = productRepository;
        }

        public List<Product> GetAll()
        {
            return _productRepository.ReadAll();
        }

        public Product GetProduct(int id)
        {
            return _productRepository.GetProduct(id);
        }

        public Product CreateProduct(Product product)
        {
            return _productRepository.CreateProduct(product);
        }

        public Product DeleteProduct(int id)
        {
            return _productRepository.DeleteProduct(id);
        }

        public Product UpdateProduct(Product product)
        {
            return _productRepository.UpdateProduct(product);
        }
    }
}