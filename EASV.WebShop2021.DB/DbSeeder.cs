namespace EASV.WebShop2021.DB
{
    public class DbSeeder
    {
        private readonly WebShopDbContext _ctx;

        public DbSeeder(WebShopDbContext context)
        {
            _ctx = context;
        }

        public void SeedDevelopment()
        {
            
        }

        public void SeedProduction()
        {
            _ctx.Database.EnsureCreated();
        }
    }
}