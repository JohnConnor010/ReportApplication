﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ReportApplication.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DataSettingContext : DbContext
    {
        public DataSettingContext()
            : base("name=DataSettingContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DataServiceCategory> DataServiceCategories { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<MainCategory> MainCategories { get; set; }
        public virtual DbSet<MediumCategory> MediumCategories { get; set; }
        public virtual DbSet<SmallCategory> SmallCategories { get; set; }
        public virtual DbSet<Judge> Judges { get; set; }
        public virtual DbSet<Suggestion> Suggestions { get; set; }
    }
}
