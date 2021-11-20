using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ExampleLinkShortener.DataAccess.Entities
{
    public class UserLink
    {
        public UserLink()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsActive { get; set; } = true;
        public int RedirectCount { get; set; }
        public string Link { get; set; }
        public string LinkCode { get; set; }
    }
}