using System.Collections.Generic;
using System.Linq;
using EASV.WebShop2021.Security.Models;
using EASV.WebShop2021.Security.Repositories;

namespace EASV.WebShop2021.Security.Services
{
    public class UserAuthenticatorService : IUserAuthenticatorService
    {
        private readonly LoginUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserAuthenticatorService(LoginUserRepository repository, IAuthService service)
        {
            _authService = service;
            _userRepository = repository;
        }
        
        public bool Login(string username, string password, out string token)
        {
            LoginUser user = _userRepository.GetAll().FirstOrDefault(loginUser => loginUser.UserName.Equals(username));

            if (user == null)
            {
                token = null;
                return false;
            }

            if (!_authService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                token = null;
                return false;
            }

            token = _authService.GenerateJwtToken(user);
            return true;
        }

        public bool CreateUser(string username, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}