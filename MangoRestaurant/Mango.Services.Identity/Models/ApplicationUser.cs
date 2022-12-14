using Microsoft.AspNetCore.Identity;

namespace Mango.Services.Identity.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DisplayName { get { return FirstName + " " + LastName; }}
    }
}
