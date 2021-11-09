using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EASV.Webshop2021.WebApi.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EASV.Webshop2021.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
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
    }
}