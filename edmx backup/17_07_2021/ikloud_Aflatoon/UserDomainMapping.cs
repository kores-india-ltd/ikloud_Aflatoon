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
    
    public partial class UserDomainMapping
    {
        public int ID { get; set; }
        public Nullable<int> User_ID { get; set; }
        public Nullable<int> Domain_ID { get; set; }
    
        public virtual Domain Domain { get; set; }
    }
}
