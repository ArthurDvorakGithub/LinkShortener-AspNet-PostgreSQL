namespace ExampleLinkShortener.DataAccess.Entities
{
    public class UserLink
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsActive { get; set; } = true;
        public int RedirectCount { get; set; }
        public string Link { get; set; }
    }
}