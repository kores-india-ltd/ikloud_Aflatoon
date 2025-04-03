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
    
    public partial class IWTempL2Verification
    {
        public long ID { get; set; }
        public Nullable<int> OpsStatus { get; set; }
        public decimal XMLAmount { get; set; }
        public string XMLSerialNo { get; set; }
        public string XMLPayorBankRoutNo { get; set; }
        public string XMLSAN { get; set; }
        public string XMLTransCode { get; set; }
        public string EntrySerialNo { get; set; }
        public string EntryPayorBankRoutNo { get; set; }
        public string EntrySAN { get; set; }
        public string EntryTransCode { get; set; }
        public Nullable<decimal> ActualAmount { get; set; }
        public string DbtAccountNo { get; set; }
        public string Date { get; set; }
        public string RejectReason { get; set; }
        public string RejectDescription { get; set; }
        public Nullable<int> L1By { get; set; }
        public Nullable<int> L2By { get; set; }
        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public string CBSClientAccountDtls { get; set; }
        public string CBSJointHoldersName { get; set; }
        public string ReturnReasonDescription { get; set; }
        public int File_ID { get; set; }
        public int FileSeqNo { get; set; }
        public string XMLPayeeName { get; set; }
        public string ClearingType { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public Nullable<int> L1Status { get; set; }
        public string PresentingBankRoutNo { get; set; }
        public string DocType { get; set; }
        public string XMLMICRRepairFlags { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string L2RejectReason { get; set; }
        public string L1RejectReason { get; set; }
        public string EntryPayeeName { get; set; }
    }
}
