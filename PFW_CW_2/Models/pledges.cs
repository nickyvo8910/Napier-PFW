//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PFW_CW_2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class pledges
    {
        public int causeId { get; set; }
        public string memberId { get; set; }
        public Nullable<System.DateTime> date { get; set; }
    
        public virtual causes causes { get; set; }
        public virtual members members { get; set; }
    }
}
