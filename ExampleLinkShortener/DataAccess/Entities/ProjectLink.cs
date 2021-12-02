using System;

namespace ExampleLinkShortener.DataAccess.Entities
{
    public class ProjectLink
    {
        public ProjectLink()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string Id { get; set; }
        
        public string ProjectId { get; set; }
        public Project Project { get; set; }
        
        public string LinkId { get; set; }
        public UserLink Link { get; set; }
    }
}