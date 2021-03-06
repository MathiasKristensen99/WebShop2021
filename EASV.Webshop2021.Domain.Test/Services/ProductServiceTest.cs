using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EASV.Webshop2021.Core.IServices;
using EASV.Webshop2021.Core.Models;
using EASV.Webshop2021.Domain.IRepositories;
using EASV.Webshop2021.Domain.Services;
using Moq;
using Xunit;

namespace EASV.Webshop2021.Domain.Test.Services
{
    public class ProductServiceTest
    {
        #region ProductService Init Tests
        
        [Fact]
        public void ProductService_IsIProductService()
        {
            var repoMock = new Mock<IProductRepository>();
            var productService = new ProductService(repoMock.Object);
            Assert.IsAssignableFrom<IProductService>(productService);
        }

        [Fact]
        public void ProductService_WithNullRepository_ThrowsInvalidDataException()
        {
            Assert.Throws<InvalidDataException>(() => new ProductService(null));
        }
        
        [Fact]
        public void ProductService_WithNullRepository_ThrowsExceptionWithMessage()
        {
            var ex = Assert.Throws<InvalidDataException>(() => new ProductService(null));
            Assert.Equal("Product repository cannot be null", ex.Message);
        }
        #endregion

        #region ProductService GetAll

        [Fact]
        public void GetAll_NoParams_CallsProductRepositoryOnce()
        {
            var repoMock = new Mock<IProductRepository>();
            var productService = new ProductService(repoMock.Object);

            productService.GetAll();
            
            repoMock.Verify(repository => repository.ReadAll(), Times.Once);
        }
        
        [Fact]
        public void GetAll_NoParams_ReturnsAllProductsAsList()
        {
            var expected = new List<Product> {new Product {Id = 1, Name = "test"}};
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(repository => repository.ReadAll())
                .Returns(expected);
            var productService = new ProductService(repoMock.Object);

            productService.GetAll();

            Assert.Equal(expected, productService.GetAll(), new ProductComparer());
        }

        #endregion

        [Fact]
        public void GetProductByID()
        {
            var expected = new Product
            {
                Id = 1,
                Name = "test"
            };
            
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(repository => repository.GetProduct(expected.Id)).Returns(expected);
            var productService = new ProductService(repoMock.Object);
            int id = 1;

            Assert.Equal(expected, productService.GetProduct(1), new ProductComparer());
        }

        #region ProductService Create
        
        [Fact]
        public void CreateProduct()
        {
            var expected = new Product
            {
                Id = 1,
                Name = "test"
            };
            
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(repository => repository.CreateProduct(expected)).Returns(expected);

            ProductService productService = new ProductService(repoMock.Object);

            productService.CreateProduct(expected);

            Assert.Equal(expected, productService.CreateProduct(expected), new ProductComparer());
        }
        
        #endregion

        [Fact]
        public void DeleteProduct()
        {
            var product = new Product
            {
                Id = 1,
                Name = "test"
            };

            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(repository => repository.DeleteProduct(product.Id));

            ProductService productService = new ProductService(repoMock.Object);

            productService.DeleteProduct(product.Id);
            
            repoMock.Verify(repository => repository.DeleteProduct(product.Id));
        }
    }

    public class ProductComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(Product obj)
        {
            return HashCode.Combine(obj.Id, obj.Name);
        }
    }
}