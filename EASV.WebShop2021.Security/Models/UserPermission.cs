namespace EASV.WebShop2021.Security.Models
{
    public class UserPermission
    {
        public int UserId { get; set; }
        public LoginUser User { get; set; }
        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}