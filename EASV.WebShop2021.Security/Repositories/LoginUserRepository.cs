using System.Collections.Generic;
using System.Linq;
using EASV.WebShop2021.Security.Models;
using Microsoft.EntityFrameworkCore;

namespace EASV.WebShop2021.Security.Repositories
{
    public class LoginUserRepository
    {
        private readonly AuthDbContext _ctx;

        public LoginUserRepository(AuthDbContext context)
        {
            _ctx = context;
        }

        public IEnumerable<LoginUser> GetAll()
        {
            return _ctx.LoginUsers.ToList();
        }

        public LoginUser Get(int id)
        {
            return _ctx.LoginUsers.FirstOrDefault(user => user.Id == id);
        }

        public void Add(LoginUser user)
        {
            _ctx.LoginUsers.Add(user);
            _ctx.SaveChanges();
        }

        public void Update(LoginUser user)
        {
            _ctx.Entry(user).State = EntityState.Modified;
            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = Get(id);
            _ctx.LoginUsers.Remove(item);
            _ctx.SaveChanges();
        }
    }
}