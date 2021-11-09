using EASV.Webshop2021.Core.Models;
using Xunit;

namespace EASV.Webshop2021.Core.Test.Models
{
    public class ProductTest
    {
        [Fact]
        public void ProductExists()
        {
            var product = new Product();
            Assert.NotNull(product);
        }

        [Fact]
        public void ProductHasStringPropertyId()
        {
            var product = new Product();
            product.Name = (string) "Test";
            Assert.Equal("Test", product.Name);
        }

        [Fact]
        public void ProductHasIntProperty()
        {
            var product = new Product();
            product.Id = (int) 1;
            Assert.Equal(1, product.Id);
        }
    }
}