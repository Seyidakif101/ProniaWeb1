using Microsoft.AspNetCore.Identity;

namespace ProniaWebSeyid.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
