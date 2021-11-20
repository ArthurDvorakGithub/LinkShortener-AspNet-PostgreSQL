using ExampleLinkShortener.DataAccess.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleLinkShortener.Services
{
    public interface IShortenerService
    {
        Task<string> Shortify(string url, string userId); // сокращаем ссылку
        Task<string> GetLink(string encodedUrl); // получаем сокращенную ссылку ссылку
        IQueryable<UserLink> GetAllUserLinks(); //сделать выборку всех ссылок
        UserLink GetUserLinkById(string id); // выбрать ссылку по идентификатору
        void SaveUserLink(UserLink entity); //сохранить изменения в базу данных
        void DeleteUserLink(string id); // удалить текстовое поле


    }
}