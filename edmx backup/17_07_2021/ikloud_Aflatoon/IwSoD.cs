//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ikloud_Aflatoon
{
    using System;
    using System.Collections.Generic;
    
    public partial class IwSoD
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public System.DateTime ProcessingDate { get; set; }
        public byte SoDStatus { get; set; }
        public Nullable<System.DateTime> SoDOn { get; set; }
        public Nullable<int> SoDBy { get; set; }
        public byte EoDStatus { get; set; }
        public Nullable<System.DateTime> EoDOn { get; set; }
        public Nullable<int> EoDBy { get; set; }
        public System.DateTime PostDated { get; set; }
        public System.DateTime StaleDated { get; set; }
    }
}
