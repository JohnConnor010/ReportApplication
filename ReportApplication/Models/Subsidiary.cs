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
    
    public partial class Subsidiary
    {
        public int SubsidiaryID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string Name { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
