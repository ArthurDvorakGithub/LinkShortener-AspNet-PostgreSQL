using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleLinkShortener.DataAccess;
using ExampleLinkShortener.DataAccess.Entities;
using ExampleLinkShortener.Models;
using Microsoft.EntityFrameworkCore;

namespace ExampleLinkShortener.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationContext _context;

        public ProjectService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectModel>> GetAll(string userId)
        {
            var projects = _context.Projects.Where(x => x.UserId == userId).ToList();
            var projectModels = new List<ProjectModel>();

            
            foreach (var project in projects)
            {
                var links =  _context.ProjectLinks
                   .Include(x => x.Link)
                   .Where(x => x.ProjectId == project.Id);
                
                var linkQty = await links.CountAsync();
                var linkTransitions = await links.Select(x => x.Link.RedirectCount).SumAsync();


                projectModels.Add(new ProjectModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    LinkQuantity = linkQty,
                    TransitionQuantity = linkTransitions
                });
            }

            return projectModels;
        }

        
        public Project GetProjectById(string id)
        {
            return _context.Projects.FirstOrDefault(x => x.Id == id);
        }

        public void SaveProject(Project entity)
        {
            
            if (entity.Id == default)
                _context.Entry(entity).State = EntityState.Added;

            
            else
                _context.Entry(entity).State = EntityState.Modified;
            
            _context.SaveChanges();
        }

        public async Task<ProjectWithLinksModel> GetById(string projectId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectId);

            var projectLinks = await _context.ProjectLinks
                .Include(x => x.Link)
                .Include(x => x.Project)
                .Where(x => x.ProjectId == projectId)
                .ToListAsync();

            var response = new ProjectWithLinksModel
            {
                Links = projectLinks.Select(x => x.Link).ToList(),
                ProjectName = project.Name,
                Id = project.Id
            }; 
            
            return response;
        }

        public async Task<string> Create(Project project)
        {
            var result = await _context.Projects.AddAsync(project);

            await _context.SaveChangesAsync();
           
            return result.Entity.Id;
            
        }


        public void DeleteProject(string id)
        {
            var links = _context.ProjectLinks.Where(x => x.ProjectId == id);
            var linkQty = links.CountAsync();
            var project = _context.Projects.AsNoTracking().FirstOrDefault(e => e.Id == id);

            if (linkQty.Result != 0)
            {
                
                var projectLinks = _context.ProjectLinks.Where(e => e.ProjectId == id);

                foreach(var projectLink in projectLinks)
                {
                    _context.ProjectLinks.Remove(projectLink);
                }
            }
            _context.Projects.Remove(project);
            
            _context.SaveChanges();

        }
    }
}