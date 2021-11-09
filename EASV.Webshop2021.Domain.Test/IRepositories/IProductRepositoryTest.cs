using System.Collections.Generic;
using EASV.Webshop2021.Core.Models;
using EASV.Webshop2021.Domain.IRepositories;
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
    }
}