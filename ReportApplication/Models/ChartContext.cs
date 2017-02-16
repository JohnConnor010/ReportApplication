using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReportApplication.Models
{
    public class ChartContext : DbContext
    {
        public ChartContext() : base("name=ChartConnectionString") { }
        public DbSet<Gather> Gathers { get; set; }
        public DbSet<GatherQuantity> GatherQuantities { get; set; }
        public DbSet<GatherTrend> GatherTrends { get; set; }
        public DbSet<Legend> Legends { get; set; }
        public DbSet<LegendData> LegendDatas { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gather>().HasMany(x => x.GatherTrends).WithOptional(x => x.Gather).WillCascadeOnDelete(true);
            base.OnModelCreating(modelBuilder);
        }

    }
}