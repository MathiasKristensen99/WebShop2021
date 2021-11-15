namespace EASV.WebShop2021.Security
{
    public interface IUserAuthenticatorService
    {
        bool Login(string username, string password, out string token);

        bool CreateUser(string username, string password);
    }
}