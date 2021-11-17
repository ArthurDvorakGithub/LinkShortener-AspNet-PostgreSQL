using System.Threading.Tasks;

namespace ExampleLinkShortener.Services
{
    public interface IShortenerService
    {
        Task<string> Shortify(string url, string userId);
        Task<string> GetLink(string encodedUrl);
    }
}