using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EASV.WebShop2021.Security;
using EASV.WebShop2021.Security.Models;
using EASV.Webshop2021.WebApi.Dtos;
using EASV.Webshop2021.WebApi.PolicyHandlers;
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
        private readonly IAuthService _authorizationService;

        public LoginController(IUserAuthenticatorService service, IAuthService authorizationService)
        {
            _service = service;
            _authorizationService = authorizationService;
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
        
        [Authorize(Policy=nameof(CanReadProductsHandler))]
        [HttpGet(nameof(GetProfile))]
        public ActionResult<ProfileDto> GetProfile()
        {
            var user = HttpContext.Items["LoginUser"] as LoginUser;
            if (user != null)
            {
                List<Permission> permissions = _authorizationService.GetPermissions(user.Id);
                return Ok(new ProfileDto
                {
                    Permissions = permissions.Select(p => p.Name).ToList(),
                    Name = user.UserName
                });
            }

            return Unauthorized();
        }
    }
}