namespace EASV.WebShop2021.Security.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int DbUserId { get; set; }
    }
}