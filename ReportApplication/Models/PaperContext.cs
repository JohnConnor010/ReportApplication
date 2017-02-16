using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReportApplication.Models
{
    public class PaperContext : DbContext
    {
        public PaperContext()
            : base("name=PaperConnectionString")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaperCategory>().HasMany(p => p.Papers).WithRequired(x => x.PaperCategory).WillCascadeOnDelete(true);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Paper> Papers { get; set; }

        public DbSet<PaperCategory> PaperCategories { get; set; }

        public DbSet<PPTTemplate> PPTTemplates { get; set; }
    }
}