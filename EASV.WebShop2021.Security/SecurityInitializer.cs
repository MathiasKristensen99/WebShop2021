using System;
using EASV.WebShop2021.Security.Models;
using EASV.WebShop2021.Security.Services;

namespace EASV.WebShop2021.Security
{
    public class SecurityInitializer : ISecurityInitializer
    {
        public void Initialize(AuthDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            context.SaveChanges();

            var authenticationService = new AuthService(Array.Empty<byte>());
            var password = "password123";
            
            authenticationService.CreatePasswordHash(password, out var pass, out var salt);

            LoginUser user = new LoginUser
            {
                UserName = "Admin",
                PasswordHash = pass,
                PasswordSalt = salt,
            };

            context.LoginUsers.Add(user);

            Permission permission = new Permission
            {
                Name = "CanWriteProductsHandler"
            };

            context.Permissions.Add(permission);

            UserPermission userPermission = new UserPermission
            {
                Permission = permission,
                User = user
            };

            context.UserPermissions.Add(userPermission);

            context.SaveChanges();
        }
    }
}