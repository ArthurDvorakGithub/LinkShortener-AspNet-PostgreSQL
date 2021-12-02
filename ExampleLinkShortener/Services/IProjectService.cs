using System.Collections.Generic;
using System.Threading.Tasks;
using ExampleLinkShortener.DataAccess.Entities;
using ExampleLinkShortener.Models;

namespace ExampleLinkShortener.Services
{
    public interface IProjectService
    {
        Task<List<ProjectModel>> GetAll(string userId);
        Task<ProjectWithLinksModel> GetById(string projectId);
        Task<string> Create(Project entity);
        Project GetProjectById(string id); // выбрать ссылку по идентификатору
        void SaveProject(Project entity); //сохранить изменения в базу данных
        void DeleteProject(string id); 

    }
}