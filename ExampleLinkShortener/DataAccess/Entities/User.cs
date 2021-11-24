using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleLinkShortener.DataAccess.Entities
{
    public class User : IdentityUser
    {
        public int Year { get; set; }

        //public List<UserLink> UserLinks { get; set; } = new List<UserLink>();

    }   
}
