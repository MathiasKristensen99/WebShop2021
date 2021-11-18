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
            try
            {
                var products = _productService.GetAll()
                    .Select(product => new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name
                    }).ToList();
            
                return Ok(new ProductsDto
                {
                    List = products
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<ProductDto> GetProduct(int id)
        {
            var product = _productService.GetProduct(id);
            return Ok(new ProductDto
            {
                Id = product.Id,
                Name = product.Name
            });
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

        [HttpPut("{id:int}")]
        public ActionResult<ProductDto> UpdateProduct(int id, ProductDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("Ids dont match");
            }
            var product = _productService.UpdateProduct(new Product
            {
                Id = dto.Id,
                Name = dto.Name
            });
            return Ok(dto);
        }
    }
}