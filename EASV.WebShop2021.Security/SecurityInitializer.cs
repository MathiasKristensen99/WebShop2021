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
            
            var password1 = "password321";
            
            authenticationService.CreatePasswordHash(password, out var pass1, out var salt1);

            LoginUser user1 = new LoginUser
            {
                UserName = "User",
                PasswordHash = pass1,
                PasswordSalt = salt1
            };

            context.LoginUsers.Add(user);

            Permission permission = new Permission
            {
                Id = 1,
                Name = "CanWriteProducts"
            };
            
            context.Permissions.Add(permission);

            Permission permission1 = new Permission
            {
                Id = 2,
                Name = "CanReadProducts"
            };

            context.Permissions.Add(permission1);

            UserPermission userPermission = new UserPermission
            {
                UserId = 1,
                Permission = permission,
                User = user,
                PermissionId = 1
                
            };

            context.UserPermissions.Add(userPermission);

            UserPermission userPermission1 = new UserPermission
            {
                UserId = 2,
                Permission = permission1,
                User = user1,
                PermissionId = 2
            };

            context.UserPermissions.Add(userPermission1);

            context.SaveChanges();
        }
    }
}