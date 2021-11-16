using System;
using System.Text;
using System.Threading.Tasks;
using ExampleLinkShortener.DataAccess;
using ExampleLinkShortener.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleLinkShortener.Services
{
    public class ShortenerService : IShortenerService
    {
        private readonly ApplicationContext _context;

        public ShortenerService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<string> Shortify(string url, string userId)
        {
            var bytes = Encoding.UTF8.GetBytes(url);
            var encodedUrl = Convert.ToBase64String(bytes);

            var userLink = new UserLink
            {
                Link = encodedUrl,
                UserId = userId
            };

            await _context.AddAsync(userLink);
            await _context.SaveChangesAsync();
            
            return encodedUrl;
        }

        public async Task<string> GetLink(string encodedUrl, string userId)
        {
            var userLink = await _context.UserLinks.FirstOrDefaultAsync(x => x.Link == encodedUrl);
            var decodedUrl = Convert.FromBase64String(encodedUrl);
            var decodedString = Encoding.UTF8.GetString(decodedUrl);
            
            userLink.RedirectCount++;

            await _context.SaveChangesAsync();

            return decodedString;
        }
    }
}