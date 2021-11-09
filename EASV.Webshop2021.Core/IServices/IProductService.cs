using System.Collections.Generic;
using EASV.Webshop2021.Core.Models;

namespace EASV.Webshop2021.Core.IServices
{
    public interface IProductService
    {
        List<Product> GetAll();
    }
}