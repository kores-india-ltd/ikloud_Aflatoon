using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace ikloud_Aflatoon.Infrastructure
{
    public class DatabaseQuery
    {
        public SqlDataReader GetSQLReader(string strQuery, SqlConnection SQlcConn)
        {
            System.Data.SqlClient.SqlCommand oCmd = new System.Data.SqlClient.SqlCommand();

            oCmd.CommandType = CommandType.Text;
            oCmd.CommandText = strQuery;
            if (SQlcConn.State == ConnectionState.Broken || SQlcConn.State == ConnectionState.Closed)
                SQlcConn.Open();
            oCmd.Connection = SQlcConn;
            return oCmd.ExecuteReader();
        }
    }
    public class ClearingTypes
    {
        public List<string> ClrType { get; set; }
        public List<string> ProType { get; set; }


        
        public ClearingTypes()
        {
            ClrType = new List<string>();
            ClrType.Add("Select");
            ClrType.Add("OW");
            ClrType.Add("IW");

            ProType = new List<string>();
            ProType.Add("Select");
            ProType.Add("CTS");
            ProType.Add("Non-CTS");
        }
        //public List<string> ProcessTypes()
        //{
        //    ProType = new List<string>();
        //    ProType.Add("Select");
        //    ProType.Add("CTS");
        //    ProType.Add("Non-CTS");
        //    return ProType;
        //}
    }
    public class Download
    {
        public string filename { get; set; }
        public int TotalRecord { get; set; }
        public int readyTodwnld { get; set; }
        public int AlreadyDwnld { get; set; }
        public int PendingVF { get; set; }
        public bool dwn { get; set; }
        public bool Redwn { get; set; }
        public int status { get; set; }


    }
    public class selectcustprocDate
    {
        public int Orgdrop { get; set; }
        public string Accesslevel { get; set; }
        public IEnumerable<SelectListItem> OrgnizationLst { get; set; }
        public IEnumerable<SelectListItem> CustomerLst { get; set; }
        public IEnumerable<SelectListItem> DomainLst { get; set; }
        public IEnumerable<SelectListItem> BranchLst { get; set; }
        public int custid { get; set; }
        public bool userlogin { get; set; }
    }
    public class unlockRecords
    {
        public Int64 Id { get; set; }
        public Int64 RawCaptureID { get; set; }
        public int Status { get; set; }
        public int Userid { get; set; }
        public string Username { get; set; }
        public int Batchno { get; set; }
        public string BranchCode { get; set; }
        public int ScannerNodeId { get; set; }
        public int BatchSeqNo { get; set; }
        public int SlipNo { get; set; }
        public string InstrumentType { get; set; }
        public decimal CheqAmount { get; set; }
        public decimal Slipamount { get; set; }
    }
    public class CustomHelpers
    {
        public static int pageno;

        public int NextPage()
        {
            pageno = pageno + 1;
            return pageno;
        }

        public int PrevPage()
        {
            pageno = pageno - 1;
            return pageno;
        }

        public int ResetPage()
        {
            pageno = 0;
            return pageno;
        }
    }
    public class WriteErrorLog
    {
        public WriteErrorLog(string fname, string errstring)
        {

            //ServerPath = Server.MapPath("~/FileDownloads/");
            //if (System.IO.Directory.Exists(ServerPath) == false)
            //{
            //    System.IO.Directory.CreateDirectory(ServerPath);
            //}

            //fileNameWithPath = ServerPath + fname;

            StreamWriter str = new StreamWriter(fname, false, System.Text.Encoding.Default);
            str.WriteLine(errstring);
            str.Close();

            //return 0;
        }
    }
    public class CheckRole
    {
        private ikloud_Aflatoon.Models.UserAflatoonDbContext db = new ikloud_Aflatoon.Models.UserAflatoonDbContext();
        public bool UserManagment { get; set; }
        public bool RejectRepair { get; set; }
        public bool DE { get; set; }
        public bool QC { get; set; }
        public bool VF { get; set; }
        public bool Report { get; set; }
        public bool fildwnd { get; set; }
        public bool Ds { get; set; } //Dashboard
        public bool QueryMod { get; set; }
        public bool Query { get; set; }
        public bool SOD { get; set; }
        public bool Master { get; set; }
        public bool Settg { get; set; }
        public bool Archv { get; set; }
        public bool Mesgbrd { get; set; }
        public bool chirjct { get; set; }
        public bool RVF { get; set; }
        public bool CCPH { get; set; }


        public CheckRole(int uid)
        {
            UserManagment = false;
            RejectRepair = false;
            DE = false;
            QC = false;
            VF = false;
            Report = false;
            fildwnd = false;
            Ds = false;
            QueryMod = false;
            Query = false;
            SOD = false;
            Master = false;
            Settg = false;
            Archv = false;
            Mesgbrd = false;
            chirjct = false;
            RVF = false;
            CCPH = false;

            var rolesassigned = from r in db.RoleMappings
                                where r.UserID == uid && r.Active == true
                                select r.Process;
            int count = rolesassigned.Count();
            foreach (var item in rolesassigned)
            {
                switch (item)
                {
                    case "UserManagment":
                        {
                            UserManagment = true;
                            break;
                        }
                    case "RejectRepair":
                        {
                            RejectRepair = true;
                            break;
                        }
                    case "DE":
                        {
                            DE = true;
                            break;
                        }
                    case "QC":
                        {
                            QC = true;
                            break;
                        }
                    case "VF":
                        {
                            VF = true;
                            break;
                        }
                    case "Report":
                        {
                            Report = true;
                            break;
                        }
                    case "fildwnd":
                        {
                            fildwnd = true;
                            break;
                        }
                    case "Ds":
                        {
                            Ds = true;
                            break;
                        }
                    case "QueryMod":
                        {
                            QueryMod = true;
                            break;
                        }
                    case "Query":
                        {
                            Query = true;
                            break;
                        }
                    case "sod":
                        {
                            SOD = true;
                            break;
                        }
                    case "master":
                        {
                            Master = true;
                            break;
                        }
                    case "settg":
                        {
                            Settg = true;
                            break;
                        }
                    case "archv":
                        {
                            Archv = true;
                            break;
                        }
                    case "mesgbrd":
                        {
                            Mesgbrd = true;
                            break;
                        }
                    case "chirjct":
                        {
                            chirjct = true;
                            break;
                        }
                    case "RVF":
                        {
                            RVF = true;
                            break;
                        }
                    case "CCPH":
                        {
                            CCPH = true;
                            break;
                        }
                    default:
                        break;
                }
            }


        }
    }

    public class CdkList
    {
        public int CustomerID { get; set; }
        public int DomainId { get; set; }
        public string BranchCode { get; set; }
        public string DomainName { get; set; }
        public string BranchName { get; set; }
        public string CDKName { get; set; }
        public int Count { get; set; }
        public int indexid { get; set; }
    }
    public class customAccount
    {
        public Int64 Id { get; set; }
        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
        public byte Status { get; set; }
        public Int64 RawDataId { get; set; }
        public int CustomerId { get; set; }
        public int DomainId { get; set; }
        public int ScanningNodeId { get; set; }
        public byte SlipAccountNoSettings { get; set; }
        public string CbsClinDtls { get; set; }
        public string CbsjointHlds { get; set; }
        public string Action { get; set; }


    }
    public class RejectReason
    {
        public string ReasonCodeS { get; set; }
        public int ReasonCode { get; set; }
        public int mtrid { get; set; }
        public string Description { get; set; }
        public bool IsChecked { get; set; }
        public string CBSCode { get; set; }

    }

    public class ExceptionFilter
    {
        public string ExReturnReasonCode { get; set; }
        public string ExReturnExceptionDiscription { get; set; }
    }
    public class cbstetails
    {
        public string cbsdls { get; set; }
        public string JoinHldrs { get; set; }
        public string MOP { get; set; }
        public string AccountStatus { get; set; }
        public string AccountOwnership { get; set; }
        public List<string> PayeeName { get; set; }
        public string callby { get; set; }
        public string payeenameselected { get; set; }
        public string Account { get; set; }
        public string CheckAc { get; set; }
        public string Allow { get; set; }
        public string FreezAllow { get; set; }
        //public string MyProperty { get; set; }
        public string status { get; set; }

        public string AI_MOP { get; set; }
        public string MOPDescrp { get; set; }
        public string AccountStatusDescrp { get; set; }
        public string AccountOwnershipdescrp { get; set; }
        public string Message { get; set; }
        public string AllowToBlock { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string popRqd { get; set; }
        public string Color { get; set; }
        public string InstrmntType { get; set; }
        public string Outward { get; set; }
        public string Inward { get; set; }
        public string Active { get; set; }
        public string SchemeCode { get; set; }
        //public string MyProperty { get; set; }
        //public string status { get; set; }
        public string AccountClose { get; set; }
        public string SchemeType { get; set; }
        public string FreezeCode { get; set; }
        public string Currency { get; set; }
        public string NREFlag { get; set; }
        public string AmountLimit { get; set; }
        public string ErrorMessage { get; set; }
        public string newAccount { get; set; }
        public string FCRAFlag { get; set; }
        public string SolId { get; set; }
        // public string Currency { get; set; }
        public string acOpenDate { get; set; }
        public string ACBAL { get; set; }
        public string ACBALAmount { get; set; }
        public List<string> PayeeNameList { get; set; }

        //vikram
        public string sSchmCode { get; set; }
        public string sFreezeStatusCode { get; set; }
        public string sModeOfOper { get; set; }
        public string sacct_status { get; set; }
        public string sSchmType { get; set; }
        public string sName { get; set; }
        public string scustomerisMinor { get; set; }
        public string sInvalid { get; set; }
        //for verification 
        public string sInvalidAcFlag { get; set; }
        public string sSIBFEE { get; set; }

        //CLEARING ADJUSTMENT PAYABLE ACCOUNT
        public string sCAPA { get; set; }
        //CLEARING ADJUSTMENT RECEIVABLE ACCOUNT
        public string sCARA { get; set; }
        //OUTWARD CLG SUSP ACCOUNT
        public string sOCSA { get; set; }
        //REALISATION SUSPENSE ACCOUNT
        public string sRSA { get; set; }
        //O/W CLEARING RETURN SUSPENSE ACCOUNT
        public string sOCRSA { get; set; }
        //RTGS-NEFT PARKING ACCOUNT
        public string sRNPA { get; set; }
        //RTGS BANK INWARD STP PARKING ACCOUNT
        public string sRBISPA { get; set; }

        public string sSCSA { get; set; }
        public string sNCA { get; set; }
        public string sNSCA { get; set; }
        public string sTDPA { get; set; }
        public string sFCRA { get; set; }
        public string sTAPC { get; set; }
        public string sCDP { get; set; }
        public string sIPC { get; set; }
        public string sRPMC { get; set; }
        public string sCDRF { get; set; }
        public string sCDRFC { get; set; }
        public string sMC { get; set; }

        public string sPC { get; set; }
        public string sKRF { get; set; }
        public string sKMS { get; set; }
        public string sKP { get; set; }

        public string sELPC { get; set; }
        public string sGDD { get; set; }
        public string sNF { get; set; }
        public string sOT { get; set; }

        public string sOP { get; set; }
        public string sOPR { get; set; }

        public string sClosedAccount { get; set; }

        //vikram

        //====== Added by Amol on 20/03/2024 ====================
        public string SourceCustomerId { get; set; }
        public string IsOpenedDateOld { get; set; }
        public Int64 OpenedDate { get; set; }


        //sambhaji
        public string StaffAcc {  get; set; }   
        public string P2F {  get; set; }    
        public string cpps {  get; set; }

        public string productCode { get; set; }
        public string productType { get; set; }
        public string accountCurrencyCode { get; set; }

        public string ApiDataString { get; set; }

        public string Invalidtbl { get; set; }

        //sambhaji
        public string PayeeNewAccFlag { get; set; }

        public string OffAcc { get; set; }

        //BOFD BOFDRoutNo
        public string BOFDRoutNo { get; set; }  

    }
    public class IWSearch
    {
        public Int64 ID { get; set; }
        public string XMLSerialNo { get; set; }
        public string ProcessingDate { get; set; }
        public string ToProcessingDate { get; set; }
        public decimal? XMLAmount { get; set; }
        public string XMLPayorBankRoutNo { get; set; }
        public string XMLSAN { get; set; }
        public string XMLTrns { get; set; }
        public string XMLPayeeName { get; set; }
        public string DocType { get; set; }
        public string PresentmentDate { get; set; }
        public string BOFDRoutNo { get; set; }
        public string MICRRepairFlags { get; set; }
        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public string chiStatus { get; set; }
        public string AccountNo { get; set; }
        public string P2F { get; set; }
        public string L1VerificationName { get; set; }
        public string L2VerificationName { get; set; }
        public string L3VerificationName { get; set; }

        public string L1VerificationAction { get; set; }
        public string L2VerificationAction { get; set; }
        public string L3VerificationAction { get; set; }
        public string chequedate { get; set; }
        public string L1RejectReason { get; set; }
        public string L2RejectReason { get; set; }
        public string L3RejectReason { get; set; }
        public int MiscStatus { get; set; }
        public string CreditAccountNo { get; set; }
        public int CustomerID { get; set; }
        public int ScanningID { get; set; }
        public string BranchCode { get; set; }
        public int BatchNo { get; set; }
        public int SlipNo { get; set; }
        public string RejectReasonDescription { get; set; }
        public string ItemSeqNo { get; set; }
        public string BranchName { get; set; }

        public int ReturnReason { get; set; }
        public string ReturnReasonDescription { get; set; }
        public string ReturnMarkedByName { get; set; }

        public Int64 RawDataId { get; set; }
        public int Status { get; set; }

        //21-12-24
       public string SQMakerId { get; set; }
       public string SQMakerDecision { get; set; }
       public string SQMakerReturnReason { get; set; }
       public string SQMakerReturnReasonDiscription { get; set; }
       public string SQCheckerId { get; set; }
       public string SQCheckerDecision { get; set; }

        public string SQCheckerReturnReason { get; set; }
        public string SQCheckerReturnReasonDiscription { get; set; }


        public string ExceptionRejectReason { get; set; }
       public string ExceptionRejectDescription { get; set; }

        public string IsByPassedFromL0 { get; set; }    


    }
    public class L2Helper
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
        public string L2Opsts { get; set; }
        public string L2Rejectreason { get; set; }
        public string L2ReturnReasonDescription { get; set; }
        public string L3Opsts { get; set; }
        public int L3Rejectreason { get; set; }
        public string L3ReturnReasonDescription { get; set; }
        public string DbtAccountNoOld { get; set; }
        public string DateOld { get; set; }
        public int L1Status { get; set; }
        public int L2Status { get; set; }
        public string PresentingBankRoutNo { get; set; }
        public string DocType { get; set; }
        public string XMLMICRRepairFlags { get; set; }
        public string Clrtype { get; set; }
        public string Vftype { get; set; }
        public string AIStatus { get; set; }

    }
    public class IWTempL1VerificationModel
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
        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public string CBSClientAccountDtls { get; set; }
        public string CBSJointHoldersName { get; set; }
        public int File_ID { get; set; }
        public int FileSeqNo { get; set; }
      //  public Nullable<bool> sttsdtqc { get; set; }

        public string sttsdtqc { get; set; }
        public string XMLPayeeName { get; set; }
        public string ClearingType { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public string PresentingBankRoutNo { get; set; }
        public string DocType { get; set; }
        public string XMLMICRRepairFlags { get; set; }
        public Nullable<int> CustomerId { get; set; }


        //sambhaji 16-10-24
        public string ExRejectReason { get; set; }
        public string ExRejectDescription { get; set; }

        public string StrModified { get;set; }
             
    }
    public class ChiReject
    {
        public Int64 Id { get; set; }
        public int ScanningNodeId { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime? FinalDate { get; set; }
        public string ChequeNoFinal { get; set; }
        public string SortCodeFinal { get; set; }
        public string SANFinal { get; set; }
        public string TransCodeFinal { get; set; }
        public string CHIStatus { get; set; }
        public int CHIRejectReason { get; set; }
        public string DocType { get; set; }
        public bool IgnoreIQA { get; set; }
        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public string ChiRejectdescription { get; set; }
        public int Customerid { get; set; }
        public int Customer_ID { get; set; }
        public int DomainID { get; set; }
        public long RawDataID { get; set; }
    }

    public class L1verificationModel
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
        public Nullable<decimal> ChequeAmountTotal { get; set; }
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
        public string ClientCode { get; set; }
        public string SlipRefNo { get; set; }
        public string PayeeName { get; set; }
        public Nullable<bool> DEBySnippet { get; set; }
        public Nullable<byte> AccountNoStatus { get; set; }
        public Nullable<byte> L2Status { get; set; }
        public Nullable<byte> L3Status { get; set; }
        public Nullable<byte> L3RequiredForRejected { get; set; }
        public Nullable<byte> CBSSettings { get; set; }
        public string CBSAccountInformation { get; set; }
        public string CBSJointAccountInformation { get; set; }
        public Nullable<byte> ChequeAmountSettings { get; set; }
        public Nullable<byte> ChequeDateSettings { get; set; }
        public Nullable<decimal> Amount1 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string FinalDate { get; set; }
        public string ChequeNoFinal { get; set; }
        public string SortCodeFinal { get; set; }
        public string SANFinal { get; set; }
        public string TransCodeFinal { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<byte> RejectReason { get; set; }
        public Nullable<bool> IsHighValue { get; set; }
        public Nullable<int> LockUser { get; set; }
        public Nullable<byte> TempStatus { get; set; }
        public Nullable<byte> ChequeDateStatus { get; set; }
        public Nullable<byte> ChequeAmountStatus { get; set; }
        public string callby { get; set; }
        public string Action { get; set; }
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
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
        public string FinalAccountNo { get; set; }
        public string AccounNoCBSResult1 { get; set; }
        public string AccounNoCBSResult2 { get; set; }
        public string AccounNoCBSResultFinal { get; set; }
        public Nullable<byte> SlipAccountNoSettings { get; set; }
        public Nullable<byte> SlipAccountNoStatus { get; set; }
        public Nullable<bool> IsDateByPassed { get; set; }
        public Nullable<bool> IsAccountNoByPassed { get; set; }
        public Nullable<bool> IsL1ByPassed { get; set; }
        public Nullable<bool> IsL2ByPassed { get; set; }
        public Nullable<bool> IsL3ByPassed { get; set; }
        public string RejectReasonDescription { get; set; }
        public string UserNarration { get; set; }
        public string SlipUserNarration { get; set; }
        public string Slipdecision { get; set; }
        public string ctsNonCtsMark { get; set; }
        public long SlipID { get; set; }
        public long SlipRawaDataID { get; set; }
        public string Modified { get; set; }
    }
    public class L2verificationModel
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
        public Nullable<decimal> ChequeAmountTotal { get; set; }
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
        public string FrontUVImagePath { get; set; }
        public string CreditAccountNo { get; set; }
        public Nullable<System.DateTime> InsertTimeStamp { get; set; }
        public Nullable<System.DateTime> CaptureTimeStamp { get; set; }
        public string DocType { get; set; }
        public string ClientCode { get; set; }
        public string SlipRefNo { get; set; }
        public string PayeeName { get; set; }
        public string DraweeName { get; set; }
        public Nullable<bool> DEBySnippet { get; set; }
        public Nullable<byte> AccountNoStatus { get; set; }
        public Nullable<byte> L2Status { get; set; }
        public Nullable<byte> L3Status { get; set; }
        public Nullable<byte> L3RequiredForRejected { get; set; }
        public Nullable<byte> CBSSettings { get; set; }
        public string CBSAccountInformation { get; set; }
        public string CBSJointAccountInformation { get; set; }
        public Nullable<byte> ChequeAmountSettings { get; set; }
        public Nullable<byte> ChequeDateSettings { get; set; }
        public Nullable<decimal> Amount1 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string FinalDate { get; set; }
        public string ChequeNoFinal { get; set; }
        public string SortCodeFinal { get; set; }
        public string SANFinal { get; set; }
        public string TransCodeFinal { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<byte> RejectReason { get; set; }
        public Nullable<int> L1UserId { get; set; }
        public Nullable<byte> L1VerificationStatus { get; set; }
        public Nullable<byte> L1RejectReason { get; set; }
        public Nullable<bool> IsHighValue { get; set; }
        public Nullable<byte> TempStatus { get; set; }
        public Nullable<int> LockUser { get; set; }
        public Nullable<byte> ChequeDateStatus { get; set; }
        public Nullable<byte> ChequeAmountStatus { get; set; }
        public string L2ModifiedFields { get; set; }
        public string callby { get; set; }
        public string Action { get; set; }
        public Nullable<bool> modified { get; set; }
        public Nullable<bool> AccModified { get; set; }
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
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
        public string FinalAccountNo { get; set; }
        public string AccounNoCBSResult1 { get; set; }
        public string AccounNoCBSResult2 { get; set; }
        public string AccounNoCBSResultFinal { get; set; }
        public Nullable<byte> SlipAccountNoSettings { get; set; }
        public Nullable<byte> SlipAccountNoStatus { get; set; }
        public Nullable<bool> IsDateByPassed { get; set; }
        public Nullable<bool> IsAccountNoByPassed { get; set; }
        public Nullable<bool> IsL1ByPassed { get; set; }
        public Nullable<bool> IsL2ByPassed { get; set; }
        public Nullable<bool> IsL3ByPassed { get; set; }
        public string RejectReasonDescription { get; set; }
        public string UserNarration { get; set; }
        public string SlipUserNarration { get; set; }
        public string ctsNonCtsMark { get; set; }
        public long SlipID { get; set; }
        public long SlipRawaDataID { get; set; }
        public bool P2fMark { get; set; }
        public string Modified1 { get; set; }
        public string Modified2 { get; set; }
        public Nullable<int> NRESourceOfFundId { get; set; }
        public Nullable<int> NROSourceOfFundId { get; set; }

        public string API_Data { get; set; }

        public string IsOpenedDateOld { get; set; }


        //sambhaji 30-1-25
        public string SrcFndsDescription { get; set; }
        public string NROSrcFndsDescription { get; set; }
    }
    public partial class L3VerificationModel
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
        public Nullable<decimal> ChequeAmountTotal { get; set; }
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
        public string ClientCode { get; set; }
        public string SlipRefNo { get; set; }
        public string PayeeName { get; set; }
        public Nullable<bool> DEBySnippet { get; set; }
        public Nullable<byte> AccountNoStatus { get; set; }
        public Nullable<byte> L2Status { get; set; }
        public Nullable<byte> L3Status { get; set; }
        public Nullable<byte> L3RequiredForRejected { get; set; }
        public Nullable<byte> CBSSettings { get; set; }
        public string CBSAccountInformation { get; set; }
        public string CBSJointAccountInformation { get; set; }
        public Nullable<byte> ChequeAmountSettings { get; set; }
        public Nullable<byte> ChequeDateSettings { get; set; }
        public Nullable<decimal> Amount1 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string FinalDate { get; set; }
        public string ChequeNoFinal { get; set; }
        public string SortCodeFinal { get; set; }
        public string SANFinal { get; set; }
        public string TransCodeFinal { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<byte> RejectReason { get; set; }
        public Nullable<int> L1UserId { get; set; }
        public Nullable<byte> L1VerificationStatus { get; set; }
        public Nullable<byte> L1RejectReason { get; set; }
        public Nullable<int> L2UserId { get; set; }
        public Nullable<byte> L2VerificationStatus { get; set; }
        public Nullable<byte> L2RejectReason { get; set; }
        public Nullable<bool> IsHighValue { get; set; }
        public Nullable<byte> TempStatus { get; set; }
        public Nullable<int> LockUser { get; set; }
        public Nullable<byte> ChequeDateStatus { get; set; }
        public Nullable<byte> ChequeAmountStatus { get; set; }
        public string L2ModifiedFields { get; set; }
        public string callby { get; set; }
        public string Action { get; set; }
        public Nullable<bool> modified { get; set; }
        public Nullable<bool> AccModified { get; set; }
        public Nullable<byte> ChequeAccountNoSettings { get; set; }
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
        public string FinalAccountNo { get; set; }
        public string AccounNoCBSResult1 { get; set; }
        public string AccounNoCBSResult2 { get; set; }
        public string AccounNoCBSResultFinal { get; set; }
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
        public Nullable<bool> globalmodified { get; set; }
        public Nullable<bool> IsDateByPassed { get; set; }
        public Nullable<bool> IsAccountNoByPassed { get; set; }
        public Nullable<bool> IsL1ByPassed { get; set; }
        public Nullable<bool> IsL2ByPassed { get; set; }
        public Nullable<bool> IsL3ByPassed { get; set; }
        public string RejectReasonDescription { get; set; }
        public string UserNarration { get; set; }
        public string ctsNonCtsMark { get; set; }
        public long SlipID { get; set; }
        public long SlipRawaDataID { get; set; }
        public bool P2fMark { get; set; }
        public string Modified2 { get; set; }
        public string Modified3 { get; set; }
    }

    public partial class getcustdomname
    {
        public string LabelName { get; set; }
        public string Name { get; set; }
    }

    public class ProductivityLogs
    {
        public long Id { get; set; }

        public Nullable<long> RawDataId { get; set; }

        public string ProcessingDate { get; set; }

        public Nullable<int> CustomerId { get; set; }

        public Nullable<int> DomainId { get; set; }

        public string LogLevel { get; set; }

        public string ActionTaken { get; set; }

        public string LoginId { get; set; }

        public Nullable<System.DateTime> TimeStamp { get; set; }

        public Nullable<long> Count { get; set; }
    }

    public class SelectionForBranchCode
    {
        public IEnumerable<SelectListItem> BranchCodeList { get; set; }
        //public IEnumerable<SelectListItem> BatchNoList { get; set; }
    }

    public class SelectionForBranchDataEntry
    {
        public IEnumerable<SelectListItem> BranchCodeList { get; set; }
        public IEnumerable<SelectListItem> BatchNoList { get; set; }
    }

    public class UserRoles
    {
        public Int64 Id { get; set; }
        public string RoleName { get; set; }
        public bool IsRejectRepaire { get; set; }
        public bool IsDataEntry { get; set; }
        public bool IsQC { get; set; }
        public bool IsVerification { get; set; }
        public bool IsReport { get; set; }
        public bool IsFileDownload { get; set; }
        public bool IsDashboard { get; set; } //Dashboard
        public bool IsQuery { get; set; }
        public bool IsQueryWithModification { get; set; }
        public bool IsReVerification { get; set; }
        public bool IsL4Verification { get; set; }
        public bool IsCCPH_Verification { get; set; }
        public bool IsUserManagement { get; set; }
        public bool IsSOD { get; set; }
        public bool IsMasters { get; set; }
        public bool IsSettings { get; set; }
        public bool IsArchieve { get; set; }
        public bool IsMessageBroadcasting { get; set; }
        public bool IsChiReject { get; set; }
        public bool IsRoleMaster { get; set; }
        public bool IsUserManagementChecker { get; set; }
        public bool IsRoleMasterChecker { get; set; }

        public decimal L1StartLimit { get; set; }
        public decimal L1StopLimit { get; set; }
        public decimal L2StartLimit { get; set; }
        public decimal L2StopLimit { get; set; }
        public decimal L3StartLimit { get; set; }
        public decimal L3StopLimit { get; set; }
        public decimal L4StartLimit { get; set; }
        public decimal L4StopLimit { get; set; }
    }

    public class Exe_Status
    {
        public Int64 FileId { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
        public string ClearingType { get; set; }
        public int ItemCount { get; set; }
        public int AcceptCount { get; set; }
        public int RejectCount { get; set; }
        public string CreationTimeStamp { get; set; }
        public string UploadTimeStamp { get; set; }
        public string RESTimeStamp { get; set; }
        public string OACKTimeStamp { get; set; }
    }

    public class Exe_Count_CXF
    {
        public string ClearingType { get; set; }
        public Int64 TotalCount { get; set; }
        public Int64 Available { get; set; }
        public Int64 Duplicate { get; set; }
        public Int64 HomeClearing { get; set; }

    }

    public class Exe_Count_Vendor
    {
        public Int64 LoadedCount { get; set; }
        public Int64 ProcessedCount { get; set; }
        public Int64 AcceptedCount { get; set; }
        public Int64 RejecedCount { get; set; }
        public Int64 PresentToDEM { get; set; }
    }

    public class Exe_Count_BRF
    {
        public string ClearingType { get; set; }
        public Int64 TotalCount { get; set; }
        public Int64 Presented { get; set; }
        public Int64 Returned { get; set; }
        public Int64 Extention { get; set; }
    }

    public class Exe_Count_PPS
    {
        public Int64 TotalCount { get; set; }
        public Int64 Available { get; set; }
        public Int64 Presented { get; set; }
        public Int64 Duplicate { get; set; }
    }

    public class Exe_Count_RRF
    {
        public string ClearingType { get; set; }
        public Int64 TotalCount { get; set; }
        public Int64 Available { get; set; }
        public Int64 Presented { get; set; }
    }

    public class Exe_Count_PXF
    {
        public string ClearingType { get; set; }
        public string DocType { get; set; }
        public Int64 TotalCount { get; set; }
    }

    public class CMS_L1verificationModel
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
        public Nullable<decimal> ChequeAmountTotal { get; set; }
        public string ChequeNoMICR { get; set; }
        public string SortCodeMICR { get; set; }
        public string SANMICR { get; set; }
        public string TransCodeMICR { get; set; }
        public string ChequeNoNI { get; set; }
        public string SortCodeNI { get; set; }
        public string SANNI { get; set; }
        public string TransCodeNI { get; set; }

        public Nullable<long> RawDataId { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string FrontGreyImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public string BackGreyImagePath { get; set; }
        public string CreditAccountNo { get; set; }
        public Nullable<System.DateTime> InsertTimeStamp { get; set; }
        public Nullable<System.DateTime> CaptureTimeStamp { get; set; }
        public string DocType { get; set; }
        public string ClientCode { get; set; }
        public string SlipRefNo { get; set; }
        public string PayeeName { get; set; }
        public string CBSAccountInformation { get; set; }
        public string CBSJointAccountInformation { get; set; }
        public Nullable<byte> ChequeAmountSettings { get; set; }
        public Nullable<byte> ChequeDateSettings { get; set; }
        public Nullable<decimal> Amount1 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string FinalDate { get; set; }
        public string ChequeNoFinal { get; set; }
        public string SortCodeFinal { get; set; }
        public string SANFinal { get; set; }
        public string TransCodeFinal { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<byte> RejectReason { get; set; }
        public Nullable<bool> IsHighValue { get; set; }
        public Nullable<int> LockUser { get; set; }
        public Nullable<byte> TempStatus { get; set; }
        public Nullable<byte> ChequeDateStatus { get; set; }
        public Nullable<byte> ChequeAmountStatus { get; set; }
        public string callby { get; set; }
        public string Action { get; set; }
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
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
        public string FinalAccountNo { get; set; }
        public string AccounNoCBSResult1 { get; set; }
        public string AccounNoCBSResult2 { get; set; }
        public string AccounNoCBSResultFinal { get; set; }
        public Nullable<byte> SlipAccountNoSettings { get; set; }
        public Nullable<byte> SlipAccountNoStatus { get; set; }
        public Nullable<bool> IsDateByPassed { get; set; }
        public Nullable<bool> IsAccountNoByPassed { get; set; }
        public Nullable<bool> IsL1ByPassed { get; set; }
        public Nullable<bool> IsL2ByPassed { get; set; }
        public Nullable<bool> IsL3ByPassed { get; set; }
        public string RejectReasonDescription { get; set; }
        public string UserNarration { get; set; }
        public string SlipUserNarration { get; set; }
        public string Slipdecision { get; set; }
        public string ctsNonCtsMark { get; set; }
        public long SlipID { get; set; }
        public long SlipRawaDataID { get; set; }
        public string Modified { get; set; }

        public string SlipDate { get; set; }

        public Nullable<int> NoOfInstrument { get; set; }

        public string DraweeName { get; set; }
        public Nullable<long> PickupLocationCode { get; set; }
        public Nullable<long> PickupLocationId { get; set; }


    }

    public class CMS_L2verificationModel
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
        public Nullable<decimal> ChequeAmountTotal { get; set; }
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
        public string FrontUVImagePath { get; set; }
        public string CreditAccountNo { get; set; }
        public Nullable<System.DateTime> InsertTimeStamp { get; set; }
        public Nullable<System.DateTime> CaptureTimeStamp { get; set; }
        public string DocType { get; set; }
        public string ClientCode { get; set; }
        public string SlipRefNo { get; set; }
        public string PayeeName { get; set; }
        public string DraweeName { get; set; }
        public Nullable<bool> DEBySnippet { get; set; }
        public Nullable<byte> AccountNoStatus { get; set; }
        public Nullable<byte> L2Status { get; set; }
        public Nullable<byte> L3Status { get; set; }
        public Nullable<byte> L3RequiredForRejected { get; set; }
        public Nullable<byte> CBSSettings { get; set; }
        public string CBSAccountInformation { get; set; }
        public string CBSJointAccountInformation { get; set; }
        public Nullable<byte> ChequeAmountSettings { get; set; }
        public Nullable<byte> ChequeDateSettings { get; set; }
        public Nullable<decimal> Amount1 { get; set; }
        public Nullable<decimal> Amount2 { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string FinalDate { get; set; }
        public string ChequeNoFinal { get; set; }
        public string SortCodeFinal { get; set; }
        public string SANFinal { get; set; }
        public string TransCodeFinal { get; set; }
        public Nullable<byte> Status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<byte> RejectReason { get; set; }
        public Nullable<int> L1UserId { get; set; }
        public Nullable<byte> L1VerificationStatus { get; set; }
        public Nullable<byte> L1RejectReason { get; set; }
        public Nullable<bool> IsHighValue { get; set; }
        public Nullable<byte> TempStatus { get; set; }
        public Nullable<int> LockUser { get; set; }
        public Nullable<byte> ChequeDateStatus { get; set; }
        public Nullable<byte> ChequeAmountStatus { get; set; }
        public string L2ModifiedFields { get; set; }
        public string callby { get; set; }
        public string Action { get; set; }
        public Nullable<bool> modified { get; set; }
        public Nullable<bool> AccModified { get; set; }
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
        public string AccountNo1 { get; set; }
        public string AccountNo2 { get; set; }
        public string FinalAccountNo { get; set; }
        public string AccounNoCBSResult1 { get; set; }
        public string AccounNoCBSResult2 { get; set; }
        public string AccounNoCBSResultFinal { get; set; }
        public Nullable<byte> SlipAccountNoSettings { get; set; }
        public Nullable<byte> SlipAccountNoStatus { get; set; }
        public Nullable<bool> IsDateByPassed { get; set; }
        public Nullable<bool> IsAccountNoByPassed { get; set; }
        public Nullable<bool> IsL1ByPassed { get; set; }
        public Nullable<bool> IsL2ByPassed { get; set; }
        public Nullable<bool> IsL3ByPassed { get; set; }
        public string RejectReasonDescription { get; set; }
        public string UserNarration { get; set; }
        public string SlipUserNarration { get; set; }
        public string ctsNonCtsMark { get; set; }
        public long SlipID { get; set; }
        public long SlipRawaDataID { get; set; }
        public bool P2fMark { get; set; }
        public string Modified1 { get; set; }
        public string Modified2 { get; set; }
        public Nullable<int> NRESourceOfFundId { get; set; }
        public Nullable<int> NROSourceOfFundId { get; set; }

        public string SlipDate { get; set; }
        public Nullable<int> NoOfInstrument { get; set; }
        public string PickupLocationCode { get; set; }
        public Nullable<long> PickupLocationId { get; set; }
        public string Slipdecision { get; set; }
    }

    public class OutwardDomainDashBoard_V2_Model
    {
        public System.DateTime ProcessDate { get; set; }
        public int CustomerId { get; set; }
        public int DomainId { get; set; }
        public string scanNodeDesc { get; set; }
        public int cntSlips { get; set; }
        public int cntchqs { get; set; }
        public int cntslipamtde { get; set; }
        public int cntchqamtde { get; set; }
        public int cntchqmicr { get; set; }
        public int cntchqdatede { get; set; }
        public int cntslipaccountde { get; set; }
        public int cntchqaccountde { get; set; }
        public int cntL1VerSC { get; set; }
        public int cntL1VerC { get; set; }
        public int cntL2VerSC { get; set; }
        public int cntL2VerC { get; set; }
        public int cntL3VerSC { get; set; }
        public int cntL3VerC { get; set; }
        public int cntDiscrepant { get; set; }
        public int cntChireject { get; set; }
        public int cntP2f { get; set; }
        public int cntReadyforBundling { get; set; }
        public int cntFileCreated { get; set; }
        public int cntFileUploaded { get; set; }

        public int cntResrecvd { get; set; }
        public int cntOackrecvd { get; set; }
        public string CustomerName { get; set; }
        public string ClearingType { get; set; }
        public Nullable<int> cntTrfChqs { get; set; }
        public Nullable<int> cntTrfChqsL1 { get; set; }
        public Nullable<int> cntTrfChqsL2 { get; set; }
        public Nullable<int> cntTrfChqsL3 { get; set; }
        public Nullable<int> HoldSlipsL2 { get; set; }
        public Nullable<int> HoldSlipsL3 { get; set; }
        public Nullable<int> cntTrfDiscrepant { get; set; }
        public Nullable<int> cntChiReturn { get; set; }
    }


    //sambhaji
    public class Show_AllCount_Reset
    {
        public string ClearingType { set; get; }
        public string TotalCount { get; set; }
    }

    public class Show_AllDetails_Reset
    {
        public string ClearingType { set; get; }
        public string Processingdate { set; get; }
        public string CustomerId { set; get; }
        public string ChequeNo { set; get; }
        public string SortCode { set; get; }
        public string San { set; get; }
        public string Tc { set; get; }
        public string Amount { set; get; }
    }

    public class GetShowCountAccept
    {
        public string TotalCheques { get; set; }
        public string TotalAmount { get; set; }

    }

    public class GetShowRecordAccept
    {
        public Int64 id { get; set; }
        public string ChqNo { get; set; }
        public string SortCode { get; set; }
        public string Transcode { get; set; }
        public string SANCode { get; set; }
        public string Amount { get; set; }
        public string Payeename { get; set; }

    }

    public class GetTimeSettingsFlag
    {
        public bool flag { get; set; }
    }

    //sambhaji l2 17-10-24
    public class IWTempL2VerificationModel
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
        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public string CBSClientAccountDtls { get; set; }
        public string CBSJointHoldersName { get; set; }
        public int File_ID { get; set; }
        public int FileSeqNo { get; set; }
       // public Nullable<bool> sttsdtqc { get; set; }

        public string sttsdtqc { get; set; }
        public string XMLPayeeName { get; set; }
        public string ClearingType { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public string PresentingBankRoutNo { get; set; }
        public string DocType { get; set; }
        public string XMLMICRRepairFlags { get; set; }
        public Nullable<int> CustomerId { get; set; }


        //sambhaji 16-10-24
        public string ExRejectReason { get; set; }
        public string ExRejectDescription { get; set; }

        public string StrModified { get; set; }
    }


    public class IWTempL3VerificationModel
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
        public string FrontGreyImagePath { get; set; }
        public string FrontTiffImagePath { get; set; }
        public string BackTiffImagePath { get; set; }
        public string CBSClientAccountDtls { get; set; }
        public string CBSJointHoldersName { get; set; }
        public int File_ID { get; set; }
        public int FileSeqNo { get; set; }
       // public Nullable<bool> sttsdtqc { get; set; }

        public string sttsdtqc { get; set; }

        public string XMLPayeeName { get; set; }
        public string ClearingType { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public string PresentingBankRoutNo { get; set; }
        public string DocType { get; set; }
        public string XMLMICRRepairFlags { get; set; }
        public Nullable<int> CustomerId { get; set; }


        //sambhaji 16-10-24
        public string ExRejectReason { get; set; }
        public string ExRejectDescription { get; set; }

        public string StrModified { get; set; }
    }


    //for session iwdashboard  29-10-24
    public class IwSessionDashBoardData
    {
      //  public string SettlementPeriod { get; set; }
      //  public int TotalChequesReceived { get; set; }
      //  public int PendingForSubmission { get; set; }
      //  public int ResponseSubmitted { get; set; }
      // // public int SuccessPosted { get; set; }
      ////  public int ReturnPosted { get; set; }
      //  public int FailedPostings { get; set; }
      //  public int PendingForPosting { get; set; }
      //  public int L1Pending { get; set; }
      //  public int L2Pending { get; set; }
      //  public int L3Pending { get; set; }
      //  public int Locked { get; set; }
      //  public int KoresSQPending { get; set; }



        //new

        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ExpiryTime { get; set; }
        public int TotalInstruments { get; set; }
        public int PendingforNPCI { get; set; }
        public int SubmittedToNPCI { get; set; }
        public int SuccessPosted { get; set; }
        public int ReturnPosted { get; set; }
        public int PendingforCBSPosting { get; set; }
        public int FailedCBSPosted { get; set; }
        public int SucessCBSPosted { get; set; }
        public int PendingforL1 { get; set; }
        public int PendingforL2 { get; set; }
        public int PendingforL3 { get; set; }
        public int PendingSQ { get; set; }
        public int L1LockCount { get; set; }
        public int L2LockCount { get; set; }
        public int L3LockCount { get; set; }

        public string HeaderExpiryTime { get;set; }




    }

    public class suspenseQcbsdata
    {
        public string CurrentAccBal { get; set; }
        public List<string> PayeeName { get; set; }
        public string PreviousAccBal { get; set; }

        public string SOLID { get; set; }

        public string AccStatus { get; set; }

        public string API_Data { get; set; }


        public string acOpenDateOld { get; set; }
        public string MOP { get; set; }
        public string StaffAcc { get; set; }
        public string sFreezeStatusCode { get; set; }
        public string productCode { get; set; }
        public string productType { get; set; }
        public string accountCurrencyCode { get; set; }
        public string ACBALAmount { get; set; }
        public string SourceCustomerId { get; set; }
        public string OffAcc { get; set; }


    }

    public class CPPS_NewAccFlag
    {
        public string CPPS_Flag { get; set; }
        public string PayeeNewAccFlag { get; set; }
        public string P2F { get; set; }
    }


    //13-01-24

    public class ActivityLogViewModal
    {
        public IEnumerable<Get_OWL1ActivityLog> Get_OWL1ActivityLog { get; set; }
        public IEnumerable<Get_OWL2ActivityLog> Get_OWL2ActivityLog { get; set; }
         public IEnumerable<Get_OWL3ActivityLog> Get_OWL3ActivityLog { get; set; }
    }

    public class IWActivityLogViewModal
    {
        public IEnumerable<Get_OWL1ActivityLog> Get_OWL1ActivityLog { get; set; }
        public IEnumerable<Get_OWL2ActivityLog> Get_OWL2ActivityLog { get; set; }
        public IEnumerable<Get_OWL3ActivityLog> Get_OWL3ActivityLog { get; set; }
        public IEnumerable<Get_IWSQActivityLog> Get_IWSQActivityLog { get; set; }
    }

    public class Get_OWL1ActivityLog
    {
        public string modified { get; set; }
        public string LogLevel { get; set; }
        public string L1_ChequeNo { get; set; }
       public string L1_AL_ChequeNo { get;set; }

        public string L1_ChequeNo_Comparison { get; set; }

        public string L1_SortCode { get; set; }
        public string L1_AL_SortCode { get; set; }
        public string L1_SortCode_Comparison { get; set; }
        public string L1_SAN { get; set; }
        public string L1_AL_SAN { get; set; }
        public string L1_SAN_Comparison { get; set; }
        public string L1_TC { get; set; }
        public string L1_AL_TC { get; set; }
        public string L1_TC_Comparison { get; set; }
        public string L1_CreditAccNo { get; set; }
        public string L1_AL_CreditAccNo { get; set; }
        public string L1_CreditAccNo_Comparison { get; set; }
        public string L1RawDataId { get; set; }
        public string AlL1RawDataId { get; set; }
        public string L1_RawDataId_Comparison { get; set; }
        public string L1_status { get; set; }
        public string AL_L1_LoginId { get; set; }
        public DateTime L1timestamp { get; set; }


    }

    public class Get_OWL2ActivityLog
    {
        public string modified { get; set; }
        public string LogLevel { get; set; }
        public string L2_ChequeNo { get; set; }
        public string L2_AL_ChequeNo { get; set; }

        public string L2_ChequeNo_Comparison { get; set; }

        public string L2_SortCode { get; set; }
        public string L2_AL_SortCode { get; set; }
        public string L2_SortCode_Comparison { get; set; }
        public string L2_SAN { get; set; }
        public string L2_AL_SAN { get; set; }
        public string L2_SAN_Comparison { get; set; }
        public string L2_TC { get; set; }
        public string L2_AL_TC { get; set; }
        public string L2_TC_Comparison { get; set; }
        public string L2_CreditAccNo { get; set; }
        public string L2_AL_CreditAccNo { get; set; }
        public string L2_CreditAccNo_Comparison { get; set; }
        public string L2RawDataId { get; set; }
        public string AlL2RawDataId { get; set; }
        public string L2_RawDataId_Comparison { get; set; }
        public string L2_status { get; set; }
        public string AL_L2_LoginId { get; set; }
        public DateTime L2timestamp { get; set; }


    }

    public class Get_OWL3ActivityLog
    {
        public string modified { get; set; }
        public string LogLevel { get; set; }
        public string L3_ChequeNo { get; set; }
        public string L3_AL_ChequeNo { get; set; }

        public string L3_ChequeNo_Comparison { get; set; }

        public string L3_SortCode { get; set; }
        public string L3_AL_SortCode { get; set; }
        public string L3_SortCode_Comparison { get; set; }
        public string L3_SAN { get; set; }
        public string L3_AL_SAN { get; set; }
        public string L3_SAN_Comparison { get; set; }
        public string L3_TC { get; set; }
        public string L3_AL_TC { get; set; }
        public string L3_TC_Comparison { get; set; }
        public string L3_CreditAccNo { get; set; }
        public string L3_AL_CreditAccNo { get; set; }
        public string L3_CreditAccNo_Comparison { get; set; }
        public string L3RawDataId { get; set; }
        public string AlL3RawDataId { get; set; }
        public string L3_RawDataId_Comparison { get; set; }
        public string L3_status { get; set; }
        public string AL_L3_LoginId { get; set; }
        public DateTime L3timestamp { get; set; }


    }


    public class Get_IWSQActivityLog
    {
        public string modified { get; set; }
        public string LogLevel { get; set; }
        public string SQ_ChequeNo { get; set; }
        public string SQ_AL_ChequeNo { get; set; }

        public string SQ_ChequeNo_Comparison { get; set; }

        public string SQ_SortCode { get; set; }
        public string SQ_AL_SortCode { get; set; }
        public string SQ_SortCode_Comparison { get; set; }
        public string SQ_SAN { get; set; }
        public string SQ_AL_SAN { get; set; }
        public string SQ_SAN_Comparison { get; set; }
        public string SQ_TC { get; set; }
        public string SQ_AL_TC { get; set; }
        public string SQ_TC_Comparison { get; set; }
        public string SQ_CreditAccNo { get; set; }
        public string SQ_AL_CreditAccNo { get; set; }
        public string SQ_CreditAccNo_Comparison { get; set; }
        public string SQRawDataId { get; set; }
        public string AlSQRawDataId { get; set; }
        public string SQ_RawDataId_Comparison { get; set; }
        public string SQ_status { get; set; }
        public string AL_SQ_LoginId { get; set; }
        public DateTime SQtimestamp { get; set; }

        public string SQ_Maker_status { get; set; } 
        public string SQ_Checker_status { get; set; } 
        public string SQ_MakerID { get; set; } 
        public string SQ_CheckerID { get; set; } 

        public string SQ_MakerTime { get; set; }
        public string SQ_CheckerTime { get; set; }




    }


}