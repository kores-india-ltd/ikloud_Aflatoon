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
    
    public partial class ActivityLog
    {
        public long Id { get; set; }
        public Nullable<long> RawDataId { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> DomainId { get; set; }
        public Nullable<int> ScanningNodeId { get; set; }
        public string LogLevel { get; set; }
        public string ActionTaken { get; set; }
        public string LoginId { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public string ChequeNo { get; set; }
        public string SortCode { get; set; }
        public string SAN { get; set; }
        public string TransCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string CreditAccountNo { get; set; }
        public string ClientCode { get; set; }
        public string SlipRefNo { get; set; }
        public string DebitAccountNo { get; set; }
        public string RejectDesc { get; set; }
    }
}
