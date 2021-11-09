using EASV.WebShop2021.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace EASV.WebShop2021.DB
{
    public class WebShopDbContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        
        public WebShopDbContext(DbContextOptions<WebShopDbContext> options) : base(options) { }
    }
}