using System.Collections.Generic;
using ExampleLinkShortener.DataAccess.Entities;

namespace ExampleLinkShortener.Models
{
    public class ProjectWithLinksModel
    {
        public string ProjectName { get; set; }
        public List<UserLink> Links { get; set; }
        // сделать UserLinkModel в котором будет фул ссылка string FullLink
        public string Id { get; set; }
    }
}