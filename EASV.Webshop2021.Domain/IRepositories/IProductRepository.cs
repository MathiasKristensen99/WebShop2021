using System.Collections.Generic;
using EASV.Webshop2021.Core.Models;

namespace EASV.Webshop2021.Domain.IRepositories
{
    public interface IProductRepository
    {
        List<Product> ReadAll();

        Product CreateProduct(Product product);
    }
}