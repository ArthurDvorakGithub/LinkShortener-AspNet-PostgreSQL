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

        // сокращаем ссылку

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

        //Выбираем все ссылки из базы данных
        public IQueryable<UserLink> GetAllUserLinks()
        {
            return _context.UserLinks;
        }

        //Берем одну запись из списка
        public UserLink GetUserLinkById(string id)
        {
            return _context.UserLinks.FirstOrDefault(x => x.Id == id);
        }

        public void SaveUserLink(UserLink entity)
        {
            //Если идентификатор отсутствует , то помечаем его как новый обьект - .Added
            if (entity.Id == default)
                _context.Entry(entity).State = EntityState.Added;

            //Если идентификатор уже есть в БД , то помечаем его как измененный - .Modified
            else
                _context.Entry(entity).State = EntityState.Modified;
            //Сохраняем
            _context.SaveChanges();
        }

        // Удаляем текстовое поле из БД
        public void DeleteUserLink(string id)
        {
            //создаем новый пустой обьект и назначаем ему идентификатор
            _context.UserLinks.Remove(new UserLink() { Id = id });
            _context.SaveChanges();
        }



    }
}