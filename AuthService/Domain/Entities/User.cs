using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        public int Age { get; set; }
        public string Country { get; set; }
    }
}
