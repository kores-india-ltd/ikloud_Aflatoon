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
    
    public partial class IWFileHDR
    {
        public IWFileHDR()
        {
            this.IWImageDtls = new HashSet<IWImageDtl>();
        }
    
        public int ID { get; set; }
        public System.DateTime ProcessingDate { get; set; }
        public string FileName { get; set; }
        public string ProcessStatus { get; set; }
        public Nullable<int> FileCount { get; set; }
        public Nullable<decimal> FileAmount { get; set; }
        public string VersionNumber { get; set; }
        public string TestFileIndicator { get; set; }
        public string CreationDate { get; set; }
        public string CreationTime { get; set; }
        public string FileID { get; set; }
        public string SessionNumber { get; set; }
        public string SessionDate { get; set; }
        public string SettlementDate { get; set; }
        public string SessionExtensionHrs { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<bool> AIActive { get; set; }
    
        public virtual ICollection<IWImageDtl> IWImageDtls { get; set; }
    }
}
