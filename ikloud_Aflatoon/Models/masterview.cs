using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

namespace ikloud_Aflatoon
{

    public class applicationSettingsView
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string SettingLevel { get; set; }
        public string CustomerName { get; set; }
        
        public int CustomerId { get; set; }
        public int SettingNameId { get; set; }
        public int SettingLevelId { get; set; }

        public IEnumerable<SelectListItem> CustomerLst { get; set; }
        public IEnumerable<SelectListItem> SettingNameLst { get; set; }
        public IEnumerable<SelectListItem> SettingLevelLst { get; set; }
    }

    public class customerView 
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "OrganizationName required.")]
        [StringLength(30,MinimumLength=5,ErrorMessage="Minimum of 5 characters required")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "GridName required.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Minimum of 5 characters required")]
        public string GridName { get; set; }

        [Required(ErrorMessage = "CustomerName required.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Minimum of 5 characters required")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "CustomerCode required.")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Minimum of 4 characters required")]
        public string CustomerCode { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "PresentingBankRouteNo must be numeric")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "PresentingBankRouteNo length must be 9 digits")]
        [Required(ErrorMessage = "PresentingBankRouteNo required.")]
        public string PresentingBankRouteNo { get; set; }

        public int GridId { get; set; }
        public int OrganizationId { get; set; }

        public IEnumerable<SelectListItem> Grid { get; set; }
        public IEnumerable<SelectListItem> Organization { get; set; }

    }

    public class branchView
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "BranchCode required.")]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "Minimum of 3 characters required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "BranchCode must be numeric")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "BranchName required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Minimum of 3 characters required")]
        public string BranchName { get; set; }


        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [Required(ErrorMessage = "IFSCode required.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "11 digits code required")]
        public string IFSCode { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "MICRCode must be numeric")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "MICRCode length must be 9 digits")]
        [Required(ErrorMessage = "MICRCode required.")]
        public string MICRCode { get; set; }


        public string OutwardDomain { get; set; }
        public string InwardDomain { get; set; }

        public int OwDomainId { get; set; }
        public int ?IwDomainId { get; set; }

        public IEnumerable<SelectListItem> OwDomainLst { get; set; }
        public IEnumerable<SelectListItem> IwDomainLst { get; set; }



    }
    public class domainView
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "DomainName required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "min 3 & maximum 20 characters required.")]
        public string DomainName { get; set; }

        [Required(ErrorMessage = "DomainCode required.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Alphanumeric Max 10 and Min 3 digits required.")]
        //[RegularExpression("[a-zA-Z0-9]*$", ErrorMessage = "Domain must be Alphanumeric")]
        public string DomainCode { get; set; }

        [Required(ErrorMessage = "CityCode required.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Numeric 3 digits required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CityCode must be numeric")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "BranchCode required.")]
        [StringLength(5, MinimumLength = 3, ErrorMessage = "Numeric 3 digits required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "BranchCode must be numeric")]
        public string BranchCode { get; set; }


        public int custID { get; set; }

        public List<domainLevelSettingView> lstDomainLevelSettings = new List<domainLevelSettingView>();


    }
    public class domainLevelSettingView
    {
        public int id { get; set; }
        public int appSettingID { get; set; }
        public string SettingName { get; set; }
        //[RegularExpression("^[0-9]*$", ErrorMessage = "SettingValue must not contain special character.")]
        [Required(ErrorMessage = "SettingValue required.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Minimum of 5 characters required")]
        public string SettingValue { get; set; }

    }
    public class instrumentView
    {
        public int Id { get; set; }
        public long captureRawId { get; set; }
        public int batchId { get; set; }

        public int? domainId { get; set; }
        public int? scanningNodeId { get; set; }
        public string branchNo { get; set; }
        public int? batchNo { get; set; }
        public int? batchSeqNo { get; set; }
        public int? slipSeqNo { get; set; }
        // [MaxLength(1)]
        public string InstrumentType { get; set; }

        public string FrontGreyImage { get; set; }
        public string FrontTiffImage { get; set; }

        public string BackGreyImage { get; set; }
        public string BackTiffImage { get; set; }

    }


    public class SMBVerificationView
    {
        public int L2Id { get; set; }
        public long captureRawId { get; set; }
        public int BatchNo { get; set; }
        public int BatchSeqNo { get; set; }
        public string InstrumentType { get; set; }
        public int CustomerId { get; set; }
        public int DomainId { get; set; }
        public int ScanningNodeId { get; set; }
        public string BranchCode { get; set; }

        public string FrontGreyImage { get; set; }
        public string FrontTiffImage { get; set; }
        public string BackGreyImage { get; set; }
        public string BackTiffImage { get; set; }

        public string ChqDate { get; set; }

        public string ChqAmt { get; set; }
        public string ChqAcno { get; set; }
        public string ChqPayeeName { get; set; }

        public string FinalChqNo { get; set; }
        
        public string FinalSortcode { get; set; }
        
        public string FinalSAN { get; set; }
        
        public string FinalTransCode { get; set; }

        public string CBSAccountInformation { get; set; }
        public string CBSJointAccountInformation { get; set; }

        public int ScanningType { get; set; }
        public string FrontUVImage { get; set; }

        public string DraweeName { get; set; }

        public string txtSrcFnds { get; set; }
    }
    public class SMBDataEntryView
    {
        public int Id { get; set; }
        public long captureRawId { get; set; }

        public string InstrumentType { get; set; }
        public int CustomerId { get; set; }
        public int DomainId { get; set; }
        public int ScanningNodeId { get; set; }


        public string FrontGreyImage { get; set; }
        public string FrontTiffImage { get; set; }

        public string BackTiffImage { get; set; }

        public string ChqDate { get; set; }
        public string ChqAmt { get; set; }
        public string ChqAcno { get; set; }
        public string ChqPayeeName { get; set; }



    }

    public class IWDataEntryView
    {
        public Int64 Id { get; set; }

        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackGreyImagePath { get; set; }
        public string BackTiffImagePath { get; set; }

        public string XMLPayeeName { get; set; }
        public string EntryAccountNo { get; set; }
        public string EntryChqDate { get; set; }
        public string EntryPayeeName { get; set; }
    }

    public class IWL2Verification
    {
        public Int64 Id { get; set; }

        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackGreyImagePath { get; set; }
        public string BackTiffImagePath { get; set; }

        public string XMLPayeeName { get; set; }
        
        public string EntryPayeeName { get; set; }
        public string EntryAccountNo { get; set; }
        public string EntryChqDate { get; set; }
    }

    public class HolidayMaster
    {
        public int ID { get; set; }
        public int Customerid { get; set; }
        public string Holiday_Date { get; set; }
        public string Holiday_Descripton { get; set; }
        public int Created_by { get; set; }
        public string Created_on { get; set; }
        public int Modified_by { get; set; }
        public string Modified_on { get; set; }
    }

    public class SANMICRMaster
    {
        public int ID { get; set; }
        public string MICR_Code { get; set; }
        public string SAN { get; set; }

        public int Account_No { get; set; }

    }


    public class IwSuspenseQueueView
    {
        public long Id { get; set; }
        public Nullable<System.DateTime> InsertTimeStamp { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public int RunningSequenceNo { get; set; }

        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public Nullable<decimal> AvailabeAmount { get; set; }
        public Nullable<decimal> ShadowBalance { get; set; }
        public string ChequeNumber { get; set; }
        public Nullable<decimal> Amount { get; set; }

        public string LienFlag { get; set; }
        public string Remarks { get; set; }
        public string PPSFlag { get; set; }
        public string PONumber { get; set; }
        public string POStatus { get; set; }
        public string Decision { get; set; }

        public int RejectReason { get; set; }
        public string WaiveCharges { get; set; }
        public Nullable<byte> Status { get; set; }

        public string UDK { get; set; }
    }


    public class IwSuspenseQueueViewCM
    {
        public long Id { get; set; }
        public Nullable<System.DateTime> InsertTimeStamp { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public int RunningSequenceNo { get; set; }

        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public Nullable<decimal> AvailabeAmount { get; set; }
        public Nullable<decimal> ShadowBalance { get; set; }
        public string ChequeNumber { get; set; }
        public Nullable<decimal> Amount { get; set; }

        public string LienFlag { get; set; }
        public string Remarks { get; set; }
        public string PPSFlag { get; set; }
        public string PONumber { get; set; }
        public string POStatus { get; set; }
        public string Decision { get; set; }

        public int RejectReason { get; set; }
        public string WaiveCharges { get; set; }
        public Nullable<byte> Status { get; set; }

        public string UDK { get; set; }

        public string SOLID { get; set; }
        public string InitialAvailableBalance { get; set; }
        public string CurrentAvailableBalance { get; set; }
        public string RBISequenceNo { get; set; }
        public string CPPS_Flag { get; set; }
        public string ExceptionReson { get; set; }
        public string ExceptionID { get; set; }
        public string MakerDecision { get; set; }
        public string MakerId { get; set; }
        public string CheckerDecision { get; set; }
        public string CheckerId { get; set; }
        // public string ExpiryTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime ExpiryTime { get; set; }
        public string ViewImg { get; set; }

        public string FrontGrayImg { get; set; }
        public string FrontTiffImg { get; set; }
        public string BackGrayImg { get; set; }
        public string BackTiffImg { get; set; }

        public string XmlSerialNo { get; set; }
        public string XmlSortCode { get; set; }
        public string XmlTc { get; set; }
        public string XmlSan { get; set; }


        public string EntrySerialNo { get; set; }
        public string EntrySortCode { get; set; }
        public string EntryTc { get; set; }
        public string EntrySan { get; set; }



        public string MakerReturnReason { get; set; }
        public string MakerReturnReasonDiscription {  get; set; }    

        public string CheckerSendBackToMakerDiscription { get; set; }


        public string SQAccountName { get; set; }

        public List<string> AccName { get; set; } = null;

        public string SelectedAccountName { get; set; } = "";

        public string SQMakerId { get; set; }

        public string SQCheckerId { get; set; }



        public string SQEntrySerialNo {  get; set; }
        public string SQEntryPayorBankRoutNo { get; set; }
        public string SQEntryTransCode { get; set; }
        public string SQEntrySAN { get; set; }

        public string API_Data { get; set; }

        public string SQAccountNumber { get; set; }


    }



    public class CMS_CustomerMaster_DBS_View
    {
        public long ID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }

        public string CustomerAccountNo { get; set; }

        public bool IsActive { get; set; }

    }

    public class CMS_LocationMaster_DBS_View
    {
        public long ID { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }

        //public int CustomerAccountNo { get; set; }

        public bool IsActive { get; set; }

    }
}
