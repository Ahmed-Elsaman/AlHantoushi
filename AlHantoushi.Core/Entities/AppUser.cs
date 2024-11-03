using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AlHantoushi.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
