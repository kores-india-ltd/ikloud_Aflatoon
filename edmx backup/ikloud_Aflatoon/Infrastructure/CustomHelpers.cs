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
        public Nullable<bool> sttsdtqc { get; set; }
        public string XMLPayeeName { get; set; }
        public string ClearingType { get; set; }
        public Nullable<System.DateTime> ProcessingDate { get; set; }
        public string PresentingBankRoutNo { get; set; }
        public string DocType { get; set; }
        public string XMLMICRRepairFlags { get; set; }
        public Nullable<int> CustomerId { get; set; }
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
}