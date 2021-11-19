using System.Collections.Generic;
using EASV.WebShop2021.Security.Models;

namespace EASV.WebShop2021.Security
{
    public interface IAuthService
    {
        string Hash(string password);

        string GenerateJwtToken(LoginUser user);

        public List<Permission> GetPermissions(int userId);

        public LoginUser IsValidUserInformation(LoginUser user);
    }
}