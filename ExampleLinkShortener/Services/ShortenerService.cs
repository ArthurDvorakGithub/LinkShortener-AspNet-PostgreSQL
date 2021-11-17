using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExampleLinkShortener.DataAccess;
using ExampleLinkShortener.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
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
            
            string linkCode;

            do
            {
                linkCode = Guid.NewGuid().ToString("n")[..6];
            }
            while (_context.UserLinks.Any(x => x.LinkCode == linkCode));

            var userLink = new UserLink
            {
                LinkCode = linkCode,
                UserId = userId,
                Link = url
            };



            await _context.UserLinks.AddAsync(userLink);
            await _context.SaveChangesAsync();

            return linkCode;
        }


        public async Task<string> GetLink(string encodedUrl)
        {
            var userLink = await _context.UserLinks.FirstOrDefaultAsync(x => x.LinkCode == encodedUrl);

            userLink.RedirectCount++;

            await _context.SaveChangesAsync();

            return userLink.Link;
        }
    }
}