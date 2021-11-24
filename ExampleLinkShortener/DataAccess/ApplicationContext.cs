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

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLink>()
                .HasIndex(u => u.LinkCode)
                .IsUnique();

        }
    }
}
