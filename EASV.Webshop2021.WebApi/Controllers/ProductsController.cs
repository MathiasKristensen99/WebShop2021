using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EASV.Webshop2021.Core.IServices;
using EASV.Webshop2021.Core.Models;
using EASV.Webshop2021.WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EASV.Webshop2021.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService service)
        {
            _productService = service;
        }
        
        [HttpGet]
        public ActionResult<ProductsDto> GetAllProducts()
        {
            var dto = new ProductsDto();
            dto.List = new List<ProductDto>
            {
                new ProductDto{Id = 1, Name = "Fuck"},
                new ProductDto{Id = 2, Name = "Lort"},
                new ProductDto{Id = 3, Name = "Pis"},
                new ProductDto{Id = 4, Name = "ForbandedeBøsselort"}
            };
            
            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct([FromBody] ProductDto dto)
        {
            var productFromDto = new Product
            {
                Name = dto.Name
            };

            try
            {
                var newProduct = _productService.CreateProduct(productFromDto);
                return Created($"https://localhost:5001/api/products/{newProduct.Id}", newProduct);
            }
            catch (ArgumentException ae)
            {
                return BadRequest(ae.Message);
            }
        }

        [HttpDelete("{id}")]
        public Product Delete(int id)
        {
            return _productService.DeleteProduct(id);
        }
    }
}