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
    
    public partial class ModificationLog
    {
        public long Id { get; set; }
        public Nullable<long> RawDataId { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> DomainId { get; set; }
        public Nullable<int> ScanningNodeId { get; set; }
        public Nullable<int> LogLevel { get; set; }
        public Nullable<int> LogField { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public string ChequeNoOld { get; set; }
        public string SortCodeOld { get; set; }
        public string SANOld { get; set; }
        public string TransCodeOld { get; set; }
        public Nullable<decimal> AmountOld { get; set; }
        public string CreditAccountNoOld { get; set; }
        public string ClientCodeOld { get; set; }
        public string SlipRefNoOld { get; set; }
        public string DebitAccountNoOld { get; set; }
        public string ChequeNoNew { get; set; }
        public string SortCodeNew { get; set; }
        public string SANNew { get; set; }
        public string TransCodeNew { get; set; }
        public Nullable<decimal> AmountNew { get; set; }
        public string CreditAccountNoNew { get; set; }
        public string ClientCodeNew { get; set; }
        public string SlipRefNoNew { get; set; }
        public string DebitAccountNoNew { get; set; }
    }
}
