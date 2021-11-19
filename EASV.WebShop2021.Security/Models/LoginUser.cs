namespace EASV.WebShop2021.Security.Models
{
    public class LoginUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public int DbUserId { get; set; }
    }
}