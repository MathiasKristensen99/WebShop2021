using System.Collections.Generic;
using EASV.Webshop2021.Core.Models;
using EASV.Webshop2021.Domain.IRepositories;
using EASV.Webshop2021.Domain.Services;
using Moq;
using Xunit;

namespace EASV.Webshop2021.Domain.Test.IRepositories
{
    public class IProductRepositoryTest
    {
        [Fact]
        public void IProductRepositoryExists()
        {
            var repoMock = new Mock<IProductRepository>();
            Assert.NotNull(repoMock.Object);
        }

        [Fact]
        public void ReadAll_WithNoParams_ReturnsListOfProducts()
        {
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(repository => repository.ReadAll())
                .Returns(new List<Product>());
            
            Assert.NotNull(repoMock.Object.ReadAll());
        }

        [Fact]
        public void CreateProduct()
        {
            var repoMock = new Mock<IProductRepository>();
            Product product = new Product
            {
                Id = 1,
                Name = "test"
            };

            repoMock.Setup(repository => repository.CreateProduct(product)).Returns(product);

            ProductService service = new ProductService(repoMock.Object);

            Assert.Equal(service.CreateProduct(product), product);
        }
    }
}