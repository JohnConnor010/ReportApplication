//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class ReferenceReport
    {
        public int ReferenceReportID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string ReportTitle { get; set; }
        public string FilePath { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
    }
}