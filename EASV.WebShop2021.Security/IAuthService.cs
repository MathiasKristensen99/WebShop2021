using System.Collections.Generic;
using EASV.WebShop2021.Security.Models;

namespace EASV.WebShop2021.Security
{
    public interface IAuthService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);

        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);

        string GenerateJwtToken(LoginUser user);
        
        List<Permission> GetPermissions(int userId);

        public LoginUser IsValidUserInformation(LoginUser user);
    }
}