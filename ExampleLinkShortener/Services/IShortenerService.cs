using System.Collections.Generic;
using ExampleLinkShortener.DataAccess.Entities;
using System.Threading.Tasks;

namespace ExampleLinkShortener.Services
{
    public interface IShortenerService
    {
        Task<string> Shortify(string projectId, string linkName, string url, string userId); // сокращаем ссылку 
        Task<string> GetLink(string encodedUrl); // получаем сокращенную ссылку ссылку
        List<UserLink> GetLinksByUserId(string id); //сделать выборку всех ссылок
        UserLink GetUserLinkById(string id); // выбрать ссылку по идентификатору
        void SaveUserLink(UserLink entity); //сохранить изменения в базу данных
        void DeleteUserLink(string id); // удалить текстовое поле
    }
}