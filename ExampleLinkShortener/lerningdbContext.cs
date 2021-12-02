using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ExampleLinkShortener
{
    public partial class lerningdbContext : DbContext
    {
        public lerningdbContext()
        {
        }

        public lerningdbContext(DbContextOptions<lerningdbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseNpgsql("Host=lerning-db.hiteka.net;Port=5432;Database=lerning-db;Username=levkalex98;Password=HtxS8t6a9DaEaDzv2P");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.utf8");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
