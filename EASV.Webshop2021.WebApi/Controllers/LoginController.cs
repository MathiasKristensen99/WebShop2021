using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EASV.WebShop2021.Security;
using EASV.WebShop2021.Security.Models;
using EASV.Webshop2021.WebApi.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EASV.Webshop2021.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserAuthenticatorService _service;

        public LoginController(IUserAuthenticatorService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            string userToken;
            if (_service.Login(dto.Username, dto.Password, out userToken))
            {
                return Ok(new
                {
                    username = dto.Username,
                    token = userToken
                });
            }

            return Unauthorized("No access, bitch.");
        }
    }
}