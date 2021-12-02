using ExampleLinkShortener.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleLinkShortener.DataAccess
{
    public sealed class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<UserLink> UserLinks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectLink> ProjectLinks { get; set; }
        public object ProjectModel { get; internal set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLink>()
                .HasIndex(u => u.LinkCode)
                .IsUnique();



            //modelBuilder
            //    .Entity<ProjectLink>()
            //    .HasOne(e => e.Project)
            //    .WithMany(e => e.Pr)
            //    .OnDelete(DeleteBehavior.ClientCascade);

            //modelBuilder
            //    .Entity<Project>()
            //    .HasOne(e => e.Id)
            //    .WithOne(e => e.)
            //    .OnDelete(DeleteBehavior.ClientCascade);
        }


    }
}
