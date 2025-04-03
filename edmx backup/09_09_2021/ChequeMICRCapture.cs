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
    
    public partial class ChequeMICRCapture
    {
        public long Id { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> DomainId { get; set; }
        public Nullable<int> ScanningNodeId { get; set; }
        public string TruncatingRouteNo { get; set; }
        public string BranchCode { get; set; }
        public string BOFD { get; set; }
        public string IFSCode { get; set; }
        public Nullable<byte> ScanningType { get; set; }
        public Nullable<byte> CycleNo { get; set; }
        public Nullable<int> BatchNo { get; set; }
        public Nullable<int> BatchSeqNo { get; set; }
        public string InstrumentType { get; set; }
        public string ClearingType { get; set; }
        public Nullable<int> SlipNo { get; set; }
        public Nullable<int> SlipChequeCount { get; set; }
        public Nullable<decimal> SlipAmount { get; set; }
        public string ChequeNoMICR { get; set; }
        public string SortCodeMICR { get; set; }
        public string SANMICR { get; set; }
        public string TransCodeMICR { get; set; }
        public string ChequeNoNI { get; set; }
        public string SortCodeNI { get; set; }
        public string SANNI { get; set; }
        public string TransCodeNI { get; set; }
        public string ChequeNoPara { get; set; }
        public string SortCodePara { get; set; }
        public string SANPara { get; set; }
        public string TransCodePara { get; set; }
        public string DatePara { get; set; }
        public string MICRRepairStatus { get; set; }
        public Nullable<byte> MICRRepairRequired { get; set; }
        public Nullable<byte> IQAFlag { get; set; }
        public string IQAString { get; set; }
        public Nullable<bool> IgnoreIQA { get; set; }
        public Nullable<long> RawDataId { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string FrontGreyImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public string BackGreyImagePath { get; set; }
        public string CreditAccountNo { get; set; }
        public Nullable<System.DateTime> InsertTimeStamp { get; set; }
        public Nullable<System.DateTime> CaptureTimeStamp { get; set; }
        public string DocType { get; set; }
        public Nullable<bool> DEBySnippet { get; set; }
        public Nullable<byte> AccountNoStatus { get; set; }
        public Nullable<byte> L2Status { get; set; }
        public Nullable<byte> L3Status { get; set; }
        public Nullable<byte> L3RequiredForRejected { get; set; }
        public Nullable<byte> CBSSettings { get; set; }
        public Nullable<byte> ChequeAmountSettings { get; set; }
        public Nullable<byte> ChequeDateSettings { get; set; }
        public Nullable<byte> ChequeDateStatus { get; set; }
        public Nullable<byte> ChequeAmountStatus { get; set; }
        public Nullable<decimal> Amount1 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public string ChequeNoFinal { get; set; }
        public string SortCodeFinal { get; set; }
        public string SANFinal { get; set; }
        public string TransCodeFinal { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<byte> ChequeAccountNoSettings { get; set; }
        public Nullable<System.DateTime> PostDate { get; set; }
        public Nullable<System.DateTime> StaleDate { get; set; }
        public Nullable<System.DateTime> ScanCutOffDateTime { get; set; }
        public Nullable<byte> ScannedAfterCutOff { get; set; }
        public Nullable<byte> L1Status { get; set; }
        public Nullable<decimal> L1AmountThreshold { get; set; }
        public Nullable<decimal> L2AmountThreshold { get; set; }
        public Nullable<decimal> L3AmountThreshold { get; set; }
        public Nullable<bool> ProcessBypassFlag { get; set; }
        public Nullable<int> ProcessBypassLevel { get; set; }
        public Nullable<bool> ProcessBypassReversed { get; set; }
        public Nullable<byte> SlipAccountNoSettings { get; set; }
        public Nullable<byte> SlipAccountNoStatus { get; set; }
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
        public string FinalAccountNo { get; set; }
        public string AccounNoCBSResult1 { get; set; }
        public string AccounNoCBSResult2 { get; set; }
        public string AccounNoCBSResultFinal { get; set; }
        public Nullable<bool> IsDateByPassed { get; set; }
        public Nullable<bool> IsAccountNoByPassed { get; set; }
        public Nullable<bool> IsL1ByPassed { get; set; }
        public Nullable<bool> IsL2ByPassed { get; set; }
        public Nullable<bool> IsL3ByPassed { get; set; }
        public Nullable<bool> IsHighValue { get; set; }
        public string FrontUVImagePath { get; set; }
        public string FrontUVImagePath1 { get; set; }
    }
}
