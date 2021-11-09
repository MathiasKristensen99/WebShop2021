using System.Collections.Generic;
using EASV.Webshop2021.Core.IServices;
using EASV.Webshop2021.Core.Models;
using Moq;
using Xunit;

namespace EASV.Webshop2021.Core.Test.IServices
{
    public class IProductServiceTest
    {
        [Fact]
        public void IProductServiceExists()
        {
            var serviceMock = new Mock<IProductService>();
            Assert.NotNull(serviceMock);
        }

        [Fact]
        public void GetAll_WithNoParams_ReturnsListOfProducts()
        {
            var serviceMock = new Mock<IProductService>();
            serviceMock
                .Setup(service => service.GetAll()).Returns(new List<Product>());
            
            Assert.NotNull(serviceMock.Object.GetAll());
        }
    }
}