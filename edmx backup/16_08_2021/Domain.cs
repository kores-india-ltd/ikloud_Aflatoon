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
    
    public partial class Domain
    {
        public Domain()
        {
            this.AppSettings = new HashSet<AppSetting>();
            this.Branches = new HashSet<Branch>();
            this.UserDomainMappings = new HashSet<UserDomainMapping>();
        }
    
        public int ID { get; set; }
        public string DomainName { get; set; }
        public int Customer_ID { get; set; }
        public string CityCode_CityCode { get; set; }
    
        public virtual ICollection<AppSetting> AppSettings { get; set; }
        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<UserDomainMapping> UserDomainMappings { get; set; }
    }
}
