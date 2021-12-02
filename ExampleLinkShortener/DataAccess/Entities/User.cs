using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ExampleLinkShortener.DataAccess.Entities
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public List<UserLink> UserLinks { get; set; }
    }   
}
