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
    
    public partial class Branch
    {
        public int ID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public string SortCode_BankBranchCode { get; set; }
        public Nullable<int> Domain_ID { get; set; }
    
        public virtual Domain Domain { get; set; }
        public virtual BankBranches BankBranches { get; set; }
    }
}
