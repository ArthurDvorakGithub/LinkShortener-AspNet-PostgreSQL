using System;

namespace ExampleLinkShortener.DataAccess.Entities
{
    public class Project
    {
        public Project()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}