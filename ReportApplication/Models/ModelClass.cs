using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity.Spatial;

namespace ReportApplication.Models
{
    [Table("Gather")]
    public partial class Gather
    {
        public Gather()
        {
            GatherQuantities = new HashSet<GatherQuantity>();
            GatherTrends = new HashSet<GatherTrend>();
            Legends = new HashSet<Legend>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CustomerID { get; set; }

        [StringLength(50)]
        public string TimeRange { get; set; }

        [StringLength(150)]
        public string ChartTitle { get; set; }

        [StringLength(200)]
        public string ExcelFilePath { get; set; }

        [StringLength(200)]
        public string PngFilePath { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? AddDate { get; set; }

        [StringLength(50)]
        public string Flag { get; set; }

        public virtual ICollection<GatherQuantity> GatherQuantities { get; set; }

        public virtual ICollection<GatherTrend> GatherTrends { get; set; }

        public virtual ICollection<Legend> Legends { get; set; }
    }

    [Table("GatherQuantity")]
    public partial class GatherQuantity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int? GatherID { get; set; }

        [StringLength(50)]
        public string ChannelName { get; set; }

        public int? SensitiveNumber { get; set; }

        public int? TotalNumber { get; set; }

        [ForeignKey("GatherID")]
        public virtual Gather Gather { get; set; }

    }

    [Table("GatherTrend")]
    public partial class GatherTrend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? GatherID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? Number { get; set; }

        [ForeignKey("GatherID")]
        public virtual Gather Gather { get; set; }
    }
    [Table("Legend")]
    public partial class Legend
    {
        public Legend()
        {
            LegendDatas = new HashSet<LegendData>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string LegendName { get; set; }

        public int GatherID { get; set; }

        [ForeignKey("GatherID")]
        public virtual Gather Gather { get; set; }

        public ICollection<LegendData> LegendDatas { get; set; }
    }
    [Table("LegendData")]
    public partial class LegendData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int LegendID { get; set; }

        [StringLength(50)]
        public string DateString { get; set; }
        
        [StringLength(50)]
        public string LegendValue { get; set; }

        [ForeignKey("LegendID")]
        public virtual Legend Legend { get; set; }
    }

    [Table("PaperCategory")]
    public partial class PaperCategory
    {
        public PaperCategory()
        {
            Papers = new HashSet<Paper>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [StringLength(50)]
        public string CategoryName { get; set; }

        public string CategorySummary { get; set; }

        public int? CustomerCategoryID { get; set; }

        public ICollection<Paper> Papers { get; set; }
    }

    [Table("Paper")]
    public partial class Paper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaperID { get; set; }

        public int CustomerID { get; set; }

        public int CategoryID { get; set; }

        public int? CustomerCategoryID { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(50)]
        public string PaperPublishedDate { get; set; }

        public int ReprintCount { get; set; }

        [StringLength(50)]
        public string FirstSite { get; set; }

        [StringLength(100)]
        public string ReprintSite { get; set; }

        
        public string Url { get; set; }

        [Column(TypeName = "text")]
        public string Summary { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AddDate { get; set; }
        
        [ForeignKey("CategoryID")]
        public virtual PaperCategory PaperCategory { get; set; }
    }
    [Table("PPTTemplate")]
    public partial class PPTTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Path { get; set; }

        [Column(TypeName = "text")]
        public string Summary { get; set; }

        public DateTime? AddDate { get; set; }
    }
    [Table("EmailGroup")]
    public partial class EmailGroup
    {
        public EmailGroup()
        {
            EmailUsers = new HashSet<EmailUser>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public int? ParentID { get; set; }

        [StringLength(200)]
        public string GroupDescription { get; set; }

        public ICollection<EmailUser> EmailUsers { get; set; }
    }
    [Table("EmailUser")]
    public partial class EmailUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        public int? GroupID { get; set; }

        public int? SubGroupID { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        public DateTime AddDate { get; set; }

        [ForeignKey("GroupID")]
        public virtual EmailGroup EmailGroup { get; set; }
    }
    [Table("SendLog")]
    public partial class SendLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string Sender { get; set; }

        [StringLength(200)]
        public string Receiver { get; set; }

        public DateTime? SendTime { get; set; }

        [StringLength(50)]
        public string SendType { get; set; }
    }
}