using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReportApplication.Models
{
    public class EmailContext : DbContext
    {
        public EmailContext()
            : base("name=PaperConnectionString")
        {

        }
        public DbSet<EmailGroup> EmailGroups { get; set; }

        public DbSet<EmailUser> EmailUsers { get; set; }

        public DbSet<SendLog> SendLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}