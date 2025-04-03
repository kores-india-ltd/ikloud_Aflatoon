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
    
    public partial class CMSAdditionalTransactions
    {
        public long ID { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string ProductCode { get; set; }
        public Nullable<System.DateTime> ProdEffectiveDate { get; set; }
        public string ProdDvisionCode { get; set; }
        public string ProdHierarchyCode { get; set; }
        public string ProductDrawerName { get; set; }
        public string ProdSubCustomer { get; set; }
        public string ProdSubCustomerEntryLevel { get; set; }
        public string CustomerProdLcc { get; set; }
        public string SlipRefNo { get; set; }
        public string ClearZone { get; set; }
        public string InstrumentType { get; set; }
        public string ChequeNo { get; set; }
        public string SortCode { get; set; }
        public string TransCode { get; set; }
        public Nullable<long> RawDataId { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public string BranchCode { get; set; }
        public Nullable<int> ScanningNodeId { get; set; }
        public Nullable<int> BatchNo { get; set; }
        public Nullable<int> SlipNo { get; set; }
        public string Status { get; set; }
        public Nullable<int> RejectReason { get; set; }
        public Nullable<int> AdditionalInfoID1 { get; set; }
        public string AdditionalInfoValue1 { get; set; }
        public Nullable<int> AdditionalInfoID2 { get; set; }
        public string AdditionalInfoValue2 { get; set; }
        public Nullable<int> AdditionalInfoID3 { get; set; }
        public string AdditionalInfoValue3 { get; set; }
        public Nullable<int> AdditionalInfoID4 { get; set; }
        public string AdditionalInfoValue4 { get; set; }
        public Nullable<int> AdditionalInfoID5 { get; set; }
        public string AdditionalInfoValue5 { get; set; }
        public Nullable<bool> Matched { get; set; }
        public Nullable<int> EntryBY { get; set; }
        public Nullable<byte> EntryStatus { get; set; }
        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> CCPH_UserID { get; set; }
        public Nullable<int> SlipChequeCount { get; set; }
    }
}
