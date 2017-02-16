using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReportApplication.Models
{
    public class MemberContext : DbContext
    {
        public MemberContext()
            : base("name=DefaultConnection")
        {

        }
        public DbSet<RoleFunction> RoleFunctions { get; set; }
    }
    [Table("RoleFunction")]
    public partial class RoleFunction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(128)]
        public string RoleId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ParentID { get; set; }

        [StringLength(50)]
        public string IconClass { get; set; }

        [StringLength(100)]
        public string Url { get; set; }
    }
}