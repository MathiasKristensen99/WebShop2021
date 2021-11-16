namespace EASV.WebShop2021.Security
{
    public interface ISecurityInitializer
    {
        void Initialize(AuthDbContext context);
    }
}