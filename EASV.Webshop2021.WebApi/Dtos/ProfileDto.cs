using System.Collections.Generic;

namespace EASV.Webshop2021.WebApi.Dtos
{
    public class ProfileDto
    {
        public List<string> Permissions { get; set; }
        public string Name { get; set; }
    }
}