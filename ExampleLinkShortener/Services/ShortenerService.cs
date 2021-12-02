using System;
using System.Collections.Generic;
using System.Linq;
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

        // сокращаем ссылку string projectId,

        public async Task<string> Shortify(string projectId, string linkName , string url, string userId)
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
                LinkName = linkName,
                UserId = userId,
                Link = url
            };

            var link = await _context.UserLinks.AddAsync(userLink);

            await _context.ProjectLinks.AddAsync(new ProjectLink
            {
                LinkId = link.Entity.Id,
                ProjectId = projectId
            });

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
        public List<UserLink> GetLinksByUserId(string id)
        {
            var userLinks = _context.Users.AsNoTracking()
                .Include(x => x.UserLinks)
                .FirstOrDefault(x => x.Id == id)?
                .UserLinks;
            
            return userLinks;
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
            var links = _context.ProjectLinks.Where(x => x.LinkId == id);
            var linkQty = links.CountAsync();
            var userLink = _context.UserLinks.AsNoTracking().FirstOrDefault(e => e.Id == id);

            if (linkQty.Result != 0)
            {
                var projectLinks = _context.ProjectLinks.Where(e => e.LinkId == id);

                foreach (var projectLink in projectLinks)
                {
                    _context.ProjectLinks.Remove(projectLink);
                }
            }

            _context.UserLinks.Remove(userLink);
           
            _context.SaveChanges();
        }
    }
}