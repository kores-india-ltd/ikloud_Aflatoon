using ikloud_Aflatoon.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace ikloud_Aflatoon.Models
{
    public class ObjectClass
    {

         //------------------OW L2 Chq--------------
        public List<L2verificationModel> selectL2ChequesOnly(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, string processingdate = null, string img = null, string callby = null, bool dirctslipcall = false, int CustomerID = 0, int DomainID = 0, string SlipOnlyAccept = null, double SlipOnlyAcceptAmtThreshold = 0, string StrVFType = null, string CtsSessionType = null, string VFType = null)
        {
                var objectlst = new List<L2verificationModel>();
            logerror("in selectL2ChequesOnly class===>.", "");
            try
            {
                L2verificationModel def;
                DataSet ds = new DataSet();
                //OWProcDataContext OWpro = new OWProcDataContext();
                Int64 id = 0;
                byte rejct = 0;
                bool getslip = false;
                string finaldate = "";
                ArrayList ids = new ArrayList();
                bool checkid = false;
                string modaction = "";
                string tempclientcd = "";
                string userNarration = "";
                string rejectreasondescrpsn = "";
                string Clearingtype = "";
                byte L3Status = 0;
                bool mark2pf = false;
                bool ignoreIQA = false;
                string DocType = "B";
                string finalmodified = "";
                int ScanningType = 0;
                Int64 SlipID = 0;
                Int64 SlipRawaDataID = 0;

                string api_data = "";
                string isOpenedDateOld = "";

                if (dirctslipcall == true)
                {
                    getslip = true;
                    goto callslip;
                }

                logerror("in selectL2ChequesOnly class===>Uid==>", uid.ToString());
                logerror("in selectL2ChequesOnly class===>VFType==>", VFType.ToString());
                logerror("in selectL2ChequesOnly class===>processingdate==>", processingdate.ToString());
                logerror("in selectL2ChequesOnly class===>CustomerID==>", CustomerID.ToString());
                logerror("in selectL2ChequesOnly class===>DomainID==>", DomainID.ToString());
                logerror("in selectL2ChequesOnly class===>CtsSessionType==>", CtsSessionType.ToString());

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL2", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                //adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = "RNormal";
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = VFType;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
                //adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[3].ToString());
                //adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[6].ToString());
                //adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[9].ToString());
                //adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[7].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
                                                                                                       //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);

                //------------------------Changes on 13/07/2017 --------------For Select Data for only slip updation-----------

                //adp.SelectCommand.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[38].ToString());
                //adp.SelectCommand.Parameters.Add("@UpdateSlipOnly", SqlDbType.NVarChar).Value = SlipOnlyAccept;
                //adp.SelectCommand.Parameters.Add("@AmountThreshold", SqlDbType.NVarChar).Value = SlipOnlyAcceptAmtThreshold;

                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];


                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------

                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            id = Convert.ToInt64(lst[0]);

                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());

                            if (lst[21] != null && lst[21].ToString() != "")
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            if (lst[12].ToString() == "A")
                            {
                                if (Convert.ToBoolean(lst[30]) == true)
                                    modaction = "M";
                                else
                                    modaction = "A";
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";

                                if (rejct == 88)
                                {
                                    if (lst[33] != null)
                                        rejectreasondescrpsn = lst[33].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";

                                }
                            }

                            if (lst[11] != null)
                                tempclientcd = lst[11].ToString();
                            if (lst[32] != null)
                                userNarration = lst[32].ToString();

                            if (lst[34] != null)
                                Clearingtype = lst[34].ToString();
                            //-----------------Marking P2F----------------------//

                            if (lst[35] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[35]);
                                if (mark2pf == true)
                                {
                                    ignoreIQA = true;
                                    DocType = "C";
                                }
                                else
                                {
                                    ignoreIQA = false;
                                    DocType = "B";
                                }

                            }
                            else
                            {
                                ignoreIQA = false;
                                DocType = "B";
                            }


                            //---------------Added on 14/07/2017----------------
                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();

                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            //    lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, Session, processingdate,
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration, rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalmodified, null, lst[40].ToString(), Convert.ToInt32(lst[41]), Convert.ToInt32(lst[42]));

                            api_data = lst[43] ?? "";

                            isOpenedDateOld = lst[44] ?? "";

                            SqlCommand cmd = new SqlCommand("UpdateOWL2", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@RawDataId", Convert.ToInt64(lst[14]));
                            cmd.Parameters.AddWithValue("@Uid", uid);
                            cmd.Parameters.AddWithValue("@InstrumentType", lst[5].ToString());
                            cmd.Parameters.AddWithValue("@FinalAmount", Convert.ToDouble(lst[20].ToString()));
                            cmd.Parameters.AddWithValue("@FinalDate", finaldate);
                            cmd.Parameters.AddWithValue("@FinalChqNo", lst[22].ToString());
                            cmd.Parameters.AddWithValue("@FinalSortcode", lst[23].ToString());
                            cmd.Parameters.AddWithValue("@FinalSAN", lst[24].ToString());
                            cmd.Parameters.AddWithValue("@FinalTransCode", lst[25].ToString());
                            cmd.Parameters.AddWithValue("@CreditAccountNo", lst[1].ToString());
                            cmd.Parameters.AddWithValue("@PayeName", lst[27].ToString());
                            cmd.Parameters.AddWithValue("@status", Convert.ToInt16(lst[13]));
                            cmd.Parameters.AddWithValue("@RejectReason", rejct);
                            cmd.Parameters.AddWithValue("@ActionTaken", modaction);
                            cmd.Parameters.AddWithValue("@LName", Session);
                            cmd.Parameters.AddWithValue("@ProcessingDate", processingdate);
                            cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(lst[17].ToString()));
                            cmd.Parameters.AddWithValue("@DomainId", Convert.ToInt32(lst[16].ToString()));
                            cmd.Parameters.AddWithValue("@ScanningNodeId", Convert.ToInt32(lst[9].ToString()));
                            cmd.Parameters.AddWithValue("@ChequeAmtotal", null);
                            cmd.Parameters.AddWithValue("@SlipAmount", 0);
                            cmd.Parameters.AddWithValue("@ChequeTotal", null);
                            cmd.Parameters.AddWithValue("@UserNarration", userNarration);
                            cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);

                           // cmd.Parameters.AddWithValue("@CTSNONCTS", Clearingtype);
                            cmd.Parameters.AddWithValue("@CTSNONCTS", CtsSessionType);

                            cmd.Parameters.AddWithValue("@CBSAccountInformation", lst[18].ToString());
                            cmd.Parameters.AddWithValue("@CBSJointAccountInformation", lst[19].ToString());
                            cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreIQA);
                            cmd.Parameters.AddWithValue("@DocType", DocType);
                            cmd.Parameters.AddWithValue("@Modified", finalmodified);
                            cmd.Parameters.AddWithValue("@strHoldReason", "");
                            cmd.Parameters.AddWithValue("@DraweeName", lst[40].ToString());
                            cmd.Parameters.AddWithValue("@NRESourceOfFundId", Convert.ToInt32(lst[41]));
                            cmd.Parameters.AddWithValue("@NROSourceOfFundId", Convert.ToInt32(lst[42]));

                            //============= Added by Amol on 29/02/2024 for handling HighValue L3 cheques start ===========
                            cmd.Parameters.AddWithValue("@VFTYPE", VFType);
                            //============= Added by Amol on 29/02/2024 for handling HighValue L3 cheques end ===========

                            //============= Added by Amol on 01/03/2024 for handling API details start ===========
                            cmd.Parameters.AddWithValue("@API_Data", api_data);
                            //============= Added by Amol on 29/02/2024 for handling API details end ===========

                            //============= Added by Amol on 21/03/2024 for handling API details start ===========
                            cmd.Parameters.AddWithValue("@IsOpenedDateOld", isOpenedDateOld);
                            //============= Added by Amol on 21/03/2024 for handling API details end ===========

                            cmd.Parameters.AddWithValue("@SrcFndsDescription", lst[45].ToString());
                            cmd.Parameters.AddWithValue("@NROSrcFndsDescription", lst[46].ToString());

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            ////-----------------Update Slip------------------
                            //byte acmodified = 0;
                            //if (Convert.ToBoolean(lst[31]) == true)
                            //    acmodified = 1;
                            //else
                            //    acmodified = 0;

                            //---------------Added On 21/06/2017------------------
                            //if (lst[36] != null)
                            //    SlipID = Convert.ToInt64(lst[36]);
                            //if (lst[37] != null)
                            //    SlipRawaDataID = Convert.ToInt64(lst[37]);

                            ////---------------Added on 14/07/2017----------------
                            //if (lst[38] != null)
                            //    ScanningType = Convert.ToInt16(lst[38]);

                            //id = Convert.ToInt64(lst[0]);
                            ////-----------------------Update Slip As Rejected If any cheque get rejected-------
                            //OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                            //      Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L2", acmodified, tempclientcd, userNarration, null,
                            //      Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session, SlipID, SlipRawaDataID, SlipOnlyAccept, ScanningType);

                            getslip = true;
                            goto callslip;
                        }

                    }


                    //else
                    //{
                    if (lst[15] != null && lst[15].ToString() != "")
                        rejct = Convert.ToByte(lst[15].ToString());
                    if (rejct == 88)
                    {
                        if (lst[33] != null)
                            rejectreasondescrpsn = lst[33].ToString();
                        else
                            rejectreasondescrpsn = "Other Reason";

                    }

                    api_data = lst[43] ?? "";
                    isOpenedDateOld = lst[44] ?? "";

                    def = new L2verificationModel();
                    
                    def.Id = Convert.ToInt64(lst[0]);
                    def.CustomerId = Convert.ToInt16(lst[17]);
                    def.DomainId = Convert.ToInt32(lst[16]);
                    def.ScanningNodeId = Convert.ToInt32(lst[9]);
                    def.BranchCode = lst[7].ToString();
                    def.BatchNo = Convert.ToInt32(lst[3]);
                    def.InstrumentType = lst[5].ToString();
                    def.ClearingType = lst[8].ToString();
                    def.SlipNo = 0;
                    def.FinalAmount = Convert.ToDecimal(lst[20]);
                    def.FinalDate = lst[21].ToString();
                    def.ChequeNoFinal = lst[22].ToString();
                    def.SortCodeFinal = lst[23].ToString();
                    def.SANFinal = lst[24].ToString();
                    def.TransCodeFinal = lst[25].ToString();
                    def.Status = Convert.ToByte(lst[13]);
                    def.FrontGreyImagePath = img;
                    def.CBSAccountInformation = lst[18].ToString();
                    def.CBSJointAccountInformation = lst[19].ToString();
                    def.CreditAccountNo = lst[1].ToString();
                    def.L1RejectReason = Convert.ToByte(lst[28].ToString());
                    def.RejectReason = rejct;
                    def.L1VerificationStatus = Convert.ToByte(lst[29].ToString());
                    def.ProcessingDate = Convert.ToDateTime(processingdate);
                    def.RawDataId = Convert.ToInt64(lst[14]);
                    def.PayeeName = lst[27].ToString();
                    def.Action = lst[12].ToString();
                    def.callby = callby;
                    def.ClientCode = lst[11].ToString();
                    def.AccModified = Convert.ToBoolean(lst[31].ToString());
                    def.DraweeName = lst[40].ToString();
                    def.NRESourceOfFundId = Convert.ToInt32(lst[41]);
                    def.NROSourceOfFundId = Convert.ToInt32(lst[42]);

                    def.SrcFndsDescription= lst[45].ToString();
                    def.NROSrcFndsDescription = lst[46].ToString();

                    def.API_Data = api_data;
                    def.IsOpenedDateOld = isOpenedDateOld;

                    if (lst[32] != null)
                        def.UserNarration = lst[32].ToString();
                    else
                        def.UserNarration = "";

                    def.RejectReasonDescription = rejectreasondescrpsn;
                    def.ctsNonCtsMark = lst[34].ToString();
                    def.P2fMark = Convert.ToBoolean(lst[35].ToString());
                    def.Modified2 = lst[39].ToString();
                    def.SlipID = 0;
                    def.SlipRawaDataID = 0;
                    

                    //def = new L2verificationModel
                    //{
                    //    Id = Convert.ToInt64(lst[0]),
                    //    CustomerId = Convert.ToInt16(lst[17]),
                    //    DomainId = Convert.ToInt32(lst[16]),
                    //    ScanningNodeId = Convert.ToInt32(lst[9]),
                    //    BranchCode = lst[7].ToString(),
                    //    BatchNo = Convert.ToInt32(lst[3]),
                    //    InstrumentType = lst[5].ToString(),
                    //    ClearingType = lst[8].ToString(),
                    //    SlipNo = 0,
                    //    FinalAmount = Convert.ToDecimal(lst[20]),
                    //    FinalDate = lst[21].ToString(),
                    //    ChequeNoFinal = lst[22].ToString(),
                    //    SortCodeFinal = lst[23].ToString(),
                    //    SANFinal = lst[24].ToString(),
                    //    TransCodeFinal = lst[25].ToString(),
                    //    Status = Convert.ToByte(lst[13]),
                    //    FrontGreyImagePath = img,
                    //    CBSAccountInformation = lst[18].ToString(),
                    //    CBSJointAccountInformation = lst[19].ToString(),
                    //    CreditAccountNo = lst[1].ToString(),
                    //    // SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                    //    L1RejectReason = Convert.ToByte(lst[28].ToString()),
                    //    RejectReason = rejct,
                    //    L1VerificationStatus = Convert.ToByte(lst[29].ToString()),
                    //    ProcessingDate = Convert.ToDateTime(processingdate),
                    //    RawDataId = Convert.ToInt64(lst[14]),
                    //    PayeeName = lst[27].ToString(),
                    //    Action = lst[12].ToString(),
                    //    callby = callby,
                    //    ClientCode = lst[11].ToString(),
                    //    AccModified = Convert.ToBoolean(lst[31].ToString()),
                    //    UserNarration = lst[32].ToString(),
                    //    RejectReasonDescription = rejectreasondescrpsn,
                    //    ctsNonCtsMark = lst[34].ToString(),
                    //    P2fMark = Convert.ToBoolean(lst[35].ToString()),
                    //    Modified2 = lst[39].ToString(),
                    //    SlipID = 0,
                    //    SlipRawaDataID = 0,
                    //    // SlipUserNarration = lst[33].ToString(),
                    //};
                    //  }
                    ids.Add(def.Id);
                    objectlst.Add(def);

                    //------------------------END------------------------//
                    for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                    {
                        //if (callby == "Cheq")
                        //{
                        if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            checkid = true;
                        else
                            checkid = false;
                        //}

                        if (checkid == false)
                        {
                            def = new L2verificationModel
                            {
                                Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                                BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                                BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                                InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                                //SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                                //SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                                //SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                                //ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                                Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                                FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                                BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                                ClientCode = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                                SlipRefNo = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                                CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                                BranchCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                                ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[13].ToString()),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[14].ToString()),
                                RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[15].ToString()),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                                CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                                ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                                L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                                L1UserId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                                L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                                PayeeName = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                                CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                                CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                                UserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                                SlipUserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                                RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                                FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[28]),
                                FinalDate = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                                ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                                SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                                SANFinal = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                                TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                                DocType = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                                Modified1 = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                                FrontUVImagePath = ds.Tables[0].Rows[index]["FrontUVImage"].ToString(),
                                DraweeName = ds.Tables[0].Rows[index]["DraweeName"].ToString(),
                                NRESourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[index]["NRESourceOfFundId"]),
                                NROSourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[index]["NROSourceOfFundId"]),

                               // SrcFndsDescription = ds.Tables[0].Rows[index]["SrcFndsDescription"].ToString(),
                               // NROSrcFndsDescription = ds.Tables[0].Rows[index]["NROSrcFndsDescription"].ToString(),
                                callby = "Cheq",
                                // ClientCode = lst[11].ToString(),
                                //AccModified = Convert.ToBoolean(lst[31].ToString()),
                                //SlipID = Convert.ToInt64(lst[36]),
                                //SlipRawaDataID = Convert.ToInt64(lst[37]),
                                //  SlipUserNarration = lst[33].ToString(),

                            };
                            objectlst.Add(def);
                            ids.Add(def.Id);
                        }
                    }
                    //  }
                }

            callslip:

                return (objectlst);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return (objectlst);
            }
            finally
            {
                con.Close();
            }
        }

        //------------------ CMS L1/L2 verification---------------------------------//
        
        public List<CMS_L1verificationModel> selectCMSL1Cheques(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, 
            string processingdate = null, string img = null, string callby = null, string tempclientcd = null, 
            int CustomerID = 0, int DomainID = 0, bool dirctslipcall = false, string CtsSessionType = null, string StrVFType = null, string StrBranchCode = null)
        {
            var objectlst = new List<CMS_L1verificationModel>();
            CMS_L1verificationModel def;
            DataSet ds = new DataSet();
            //OWProcDataContext OWpro = new OWProcDataContext();
            Int64 id = 0;
            byte rejct = 0;
            bool getslip = false;
            string finaldate = "";
            string userNarration = "";
            string rejectreasondescrpsn = "";
            //string tempclientcd = "";
            string Clearingtype = "";
            string creditcardno = "";
            string payeename = "Not Found";
            string Modified = "";
            Byte l1slipststus = 0;
            string L1dec = "";
            Int64 SlipID = 0;
            Int64 SlipRawaDataID = 0;
            string LICNO = "";
            decimal branchAmt = 0;
            byte AiFinalResult = 0;
            string Hirarchylink = "";
            string divisionlink = "";
            byte ScanningType = 0;

            if (dirctslipcall == true)
            {
                getslip = true;
                goto callslip;
            }

            ArrayList ids = new ArrayList();
            bool checkid = false;

            SqlDataAdapter adp = new SqlDataAdapter("OWSelectCMSL1Cheques", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
            adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
            adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[5].ToString());
            adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[9].ToString());
            adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[4].ToString());
            adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[3].ToString();
            //-------------Added on 17-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
            //-------------Added on 18-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[2].ToString());//Convert.ToInt32(Session["DomainselectID"]);
            //-------------Added on 12-09-2017-----------------------------
            //adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];
            adp.SelectCommand.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[8].ToString());


            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                decimal slipAmt = 0;
                decimal finalAmt = 0;

                if (callby != "Slip")
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            string finDate = "";
                            string slipRefNo = "";
                            string draweename = "";

                            if (lst[11] != null && lst[11].ToString() != "")
                                tempclientcd = lst[11].ToString();

                            if (lst[18] != null && lst[18].ToString() != "")
                                finalAmt = Convert.ToDecimal(lst[18].ToString());

                            if (lst[14] != null)
                                slipRefNo = lst[14].ToString();

                            if (lst[19] != null && lst[19].ToString() != "")
                            {
                                if (lst[19].ToString().Length != 10)
                                    finDate = "20" + lst[19].ToString().Substring(4, 2) + "-" + lst[19].ToString().Substring(2, 2) + "-" + lst[19].ToString().Substring(0, 2);
                                else
                                    finDate = lst[19].ToString();
                            }

                            if (lst[25] != null)
                                payeename = lst[25].ToString();

                            if (lst[26] != null)
                                draweename = lst[26].ToString();

                            if (lst[27] != null)
                                userNarration = lst[27].ToString();

                            //====== Cheque Update Record start =========
                            SqlCommand com = new SqlCommand("CMS_Update_ChequeData", con);

                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[0]);
                            com.Parameters.Add("@RawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[10]);
                            com.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
                            com.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = CustomerID;
                            com.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;
                            com.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[3].ToString();
                            com.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = lst[4].ToString();
                            com.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = lst[8].ToString();
                            com.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = lst[5].ToString();
                            com.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = lst[9].ToString();
                            com.Parameters.Add("@SlipRefNo", SqlDbType.NVarChar).Value = slipRefNo.ToString();
                            com.Parameters.Add("@FinalDate", SqlDbType.NVarChar).Value = finDate.ToString();
                            com.Parameters.Add("@FinalAmount", SqlDbType.Decimal).Value = finalAmt;
                            com.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = tempclientcd.ToString();
                            com.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = payeename.ToString();
                            com.Parameters.Add("@DraweeName", SqlDbType.NVarChar).Value = draweename.ToString();
                            com.Parameters.Add("@UserNarration", SqlDbType.NVarChar).Value = userNarration.ToString();
                            //com.Parameters.Add("@NoOfInstrument", SqlDbType.Int).Value = lst[24].ToString();
                            com.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;
                            //com.Parameters.Add("@AccountNo", SqlDbType.NVarChar).Value = lst[15].ToString();
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
                            //con.Dispose();
                            //====== Cheque Update Record end =========

                            //====== Slip Update Record start =========
                            SqlCommand com1 = new SqlCommand("CMS_Update_SlipStatusOnly", con);

                            com1.CommandType = CommandType.StoredProcedure;
                            com1.Parameters.Add("@SlipID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[28]);
                            com1.Parameters.Add("@SlipRawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[29]);
                            com1.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;
                            
                            con.Open();
                            com1.ExecuteNonQuery();
                            con.Close();
                            //con.Dispose();
                            //====== Slip Update Record end =========

                            getslip = true;
                            goto callslip;
                        }
                    }
                }


                if (callby == "Slip")
                {
                    def = new CMS_L1verificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[1].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[2].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[3].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[5].ToString(),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[7].ToString()),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[10].ToString()),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[11].ToString()),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[17]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[18].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[19].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[23]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[24]),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[25].ToString(),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[27]),
                        callby = "Slip",
                        ClientCode = ds.Tables[0].Rows[0].ItemArray[28].ToString(),
                        SlipID = Convert.ToInt64(lst[28]),
                        SlipRawaDataID = Convert.ToInt64(lst[29]),

                    };
                }
                else
                {
                    string draweename = "";
                    string finDate1 = "";
                    if (lst[19] != null && lst[19].ToString() != "")
                    {
                        if (lst[19].ToString().Length != 10)
                            finDate1 = "20" + lst[19].ToString().Substring(4, 2) + "-" + lst[19].ToString().Substring(2, 2) + "-" + lst[19].ToString().Substring(0, 2);
                        else
                            finDate1 = lst[19].ToString();
                    }

                    string slpDate1 = "";
                    if (lst[17] != null && lst[17].ToString() != "")
                    {
                        if (lst[17].ToString().Length != 10)
                            slpDate1 = "20" + lst[17].ToString().Substring(4, 2) + "-" + lst[17].ToString().Substring(2, 2) + "-" + lst[17].ToString().Substring(0, 2);
                        else
                            slpDate1 = lst[17].ToString();
                    }

                    if (lst[16] != null && lst[16].ToString() != "")
                        slipAmt = Convert.ToDecimal(lst[16].ToString());

                    if (lst[18] != null && lst[18].ToString() != "")
                        finalAmt = Convert.ToDecimal(lst[18].ToString());

                    if (lst[25] != null)
                        payeename = lst[25].ToString();

                    if (lst[26] != null)
                        draweename = lst[26].ToString();

                    if (lst[27] != null)
                        userNarration = lst[27].ToString();

                    if (lst[11] != null && lst[11].ToString() != "")
                        tempclientcd = lst[11].ToString();

                    if (lst[8] != null)
                        ScanningType = Convert.ToByte(lst[8]);

                    def = new CMS_L1verificationModel
                    {
                        Id = Convert.ToInt64(lst[0]),
                        CustomerId = Convert.ToInt16(lst[1]),
                        DomainId = Convert.ToInt32(lst[2]),
                        ScanningNodeId = Convert.ToInt32(lst[4]),
                        BranchCode = lst[3].ToString(),
                        BatchNo = Convert.ToInt32(lst[5]),
                        InstrumentType = lst[7].ToString(),
                        SlipNo = Convert.ToInt32(lst[9]),
                        FinalAmount = finalAmt,        //Convert.ToDecimal(lst[18]),
                        FinalDate = finDate1.ToString(),
                        ChequeNoFinal = lst[20].ToString(),
                        SortCodeFinal = lst[21].ToString(),
                        SANFinal = lst[22].ToString(),
                        TransCodeFinal = lst[23].ToString(),
                        
                        FrontTiffImagePath = img.Replace("jpg", "tif"),
                        FrontGreyImagePath = img,
                        
                        CreditAccountNo = lst[15].ToString(),
                        SlipAmount = slipAmt,
                        SlipDate = slpDate1.ToString(),
                        callby = "Cheq",
                        ClientCode = tempclientcd.ToString(),
                        ProcessingDate = Convert.ToDateTime(processingdate),
                        RawDataId = Convert.ToInt64(lst[10]),
                        PayeeName = payeename.ToString(),
                        DraweeName = draweename.ToString(),
                        UserNarration = userNarration.ToString(),
                        
                        SlipID = Convert.ToInt64(lst[28]),
                        SlipRawaDataID = Convert.ToInt64(lst[29]),
                        ScanningType = Convert.ToByte(ScanningType),
                    };
                }
                ids.Add(def.Id);
                objectlst.Add(def);

                for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    if (callby == "Cheq")
                    {
                        if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                        {
                            checkid = true;
                            // break;
                        }
                        else
                        {
                            checkid = false;
                        }
                    }

                    if (checkid == false)
                    {
                        def = new CMS_L1verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[1].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[2].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[3].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[4].ToString()),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[6].ToString()),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[7].ToString()),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[10].ToString()),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[11].ToString()),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[17]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[18].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            
                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[23]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[24]),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[27]),
                            ClientCode = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            //SlipUserNarration = lst[29].ToString(),
                            //RejectReason = rejct,
                            //Slipdecision = lst[30].ToString(),
                            SlipID = Convert.ToInt64(lst[28]),
                            SlipRawaDataID = Convert.ToInt64(lst[29]),
                            
                        };
                        objectlst.Add(def);
                        ids.Add(def.Id);
                    }
                }

                getslip = false;
            }
            else
            {
                //====== Slip Update Record start =========
                SqlCommand com1 = new SqlCommand("CMS_Update_SlipStatusOnly", con);

                com1.CommandType = CommandType.StoredProcedure;
                com1.Parameters.Add("@SlipID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[28]);
                com1.Parameters.Add("@SlipRawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[29]);
                com1.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;

                con.Open();
                com1.ExecuteNonQuery();
                con.Close();
                //con.Dispose();
                //====== Slip Update Record end =========
                getslip = true;
            }

        callslip:
            if (getslip == true)
            {
                adp = new SqlDataAdapter("OWSelectCMSL1", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;

                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];

                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = StrVFType;

                adp.SelectCommand.Parameters.Add("@BranchCodeId", SqlDbType.NVarChar).Value = StrBranchCode;


                ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new CMS_L1verificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                        SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[9]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ClientCode = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        SlipRefNo = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[17].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[23].ToString()),

                        callby = "Slip",
                    };
                    objectlst.Add(def);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new CMS_L1verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[5].ToString()),
                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[6].ToString()),
                            SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[7]),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[8]),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[9]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ClientCode = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            SlipRefNo = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[23].ToString()),
                            callby = "Slip",

                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                }
            }
        finalexit:
            return (objectlst);

        }


        public List<CMS_L2verificationModel> selectCMSL2Cheques(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null,
            string processingdate = null, string img = null, string callby = null, string tempclientcd = null,
            int CustomerID = 0, int DomainID = 0, bool dirctslipcall = false, string CtsSessionType = null, string StrVFType = null, string StrBranchCode = null)
        {
            var objectlst = new List<CMS_L2verificationModel>();
            CMS_L2verificationModel def;
            DataSet ds = new DataSet();
            //OWProcDataContext OWpro = new OWProcDataContext();
            Int64 id = 0;
            byte rejct = 0;
            bool getslip = false;
            string finaldate = "";
            string userNarration = "";
            string rejectreasondescrpsn = "";
            //string tempclientcd = "";
            string Clearingtype = "";
            string creditcardno = "";
            string payeename = "Not Found";
            string Modified = "";
            Byte l1slipststus = 0;
            string L2Slipdec = "";
            Int64 SlipID = 0;
            Int64 SlipRawaDataID = 0;
            string LICNO = "";
            decimal branchAmt = 0;
            byte AiFinalResult = 0;
            string Hirarchylink = "";
            string divisionlink = "";
            byte ScanningType = 0;
            string actionTaken = "";

            if (dirctslipcall == true)
            {
                getslip = true;
                goto callslip;
            }

            ArrayList ids = new ArrayList();
            bool checkid = false;

            SqlDataAdapter adp = new SqlDataAdapter("OWSelectCMSL2Cheques", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
            adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
            adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[5].ToString());
            adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[9].ToString());
            adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[4].ToString());
            adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[3].ToString();
            //-------------Added on 17-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
            //-------------Added on 18-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[2].ToString());//Convert.ToInt32(Session["DomainselectID"]);
            //-------------Added on 12-09-2017-----------------------------
            //adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];
            adp.SelectCommand.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[8].ToString());


            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                decimal slipAmt = 0;
                decimal finalAmt = 0;

                if (callby != "Slip")
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            string finDate = "";
                            string slipRefNo = "";
                            string draweename = "";

                            if (lst[11] != null && lst[11].ToString() != "")
                                tempclientcd = lst[11].ToString();

                            if (lst[18] != null && lst[18].ToString() != "")
                                finalAmt = Convert.ToDecimal(lst[18].ToString());

                            if (lst[14] != null)
                                slipRefNo = lst[14].ToString();

                            if (lst[19] != null && lst[19].ToString() != "")
                            {
                                if (lst[19].ToString().Length != 10)
                                    finDate = "20" + lst[19].ToString().Substring(4, 2) + "-" + lst[19].ToString().Substring(2, 2) + "-" + lst[19].ToString().Substring(0, 2);
                                else
                                    finDate = lst[19].ToString();
                            }

                            if (lst[25] != null)
                                payeename = lst[25].ToString();

                            if (lst[26] != null)
                                draweename = lst[26].ToString();

                            if (lst[27] != null)
                                userNarration = lst[27].ToString();

                            if (lst[31] != null && lst[31].ToString() != "")
                                actionTaken = lst[31].ToString();

                            if (lst[32] != null && lst[32].ToString() != "")
                                rejct = Convert.ToByte(lst[32].ToString());

                            if (rejct == 88)
                            {
                                if (lst[33] != null)
                                    rejectreasondescrpsn = lst[33].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";

                            }

                            //====== Cheque Update Record start =========
                            SqlCommand com = new SqlCommand("CMS_Update_ChequeDataL2", con);

                            com.CommandType = CommandType.StoredProcedure;
                            com.Parameters.Add("@ID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[0]);
                            com.Parameters.Add("@RawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[10]);
                            com.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
                            com.Parameters.Add("@CustomerId", SqlDbType.NVarChar).Value = CustomerID;
                            com.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;
                            com.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[3].ToString();
                            com.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = lst[4].ToString();
                            com.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = lst[8].ToString();
                            com.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = lst[5].ToString();
                            com.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = lst[9].ToString();
                            com.Parameters.Add("@SlipRefNo", SqlDbType.NVarChar).Value = slipRefNo.ToString();
                            com.Parameters.Add("@FinalDate", SqlDbType.NVarChar).Value = finDate.ToString();
                            com.Parameters.Add("@FinalAmount", SqlDbType.Decimal).Value = finalAmt;
                            com.Parameters.Add("@ClientCode", SqlDbType.NVarChar).Value = tempclientcd.ToString();
                            com.Parameters.Add("@PayeeName", SqlDbType.NVarChar).Value = payeename.ToString();
                            com.Parameters.Add("@DraweeName", SqlDbType.NVarChar).Value = draweename.ToString();
                            com.Parameters.Add("@UserNarration", SqlDbType.NVarChar).Value = userNarration.ToString();
                            //com.Parameters.Add("@NoOfInstrument", SqlDbType.Int).Value = lst[24].ToString();
                            com.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;
                            com.Parameters.Add("@ActionTaken", SqlDbType.NVarChar).Value = actionTaken;
                            com.Parameters.Add("@RejectReason", SqlDbType.TinyInt).Value = rejct;
                            com.Parameters.Add("@RejectReasonDescription", SqlDbType.NVarChar).Value = rejectreasondescrpsn;
                            con.Open();
                            com.ExecuteNonQuery();
                            con.Close();
                            //con.Dispose();
                            //====== Cheque Update Record end =========

                            //====== Slip Update Record start =========

                            if (lst[30] != null)
                            {
                                if (lst[30].ToString().ToUpper() == "R")
                                    L2Slipdec = "L2R";
                                else
                                    L2Slipdec = "L2";
                            }

                            SqlCommand com1 = new SqlCommand("CMS_Update_SlipStatusOnlyL2", con);

                            com1.CommandType = CommandType.StoredProcedure;
                            com1.Parameters.Add("@SlipID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[28]);
                            com1.Parameters.Add("@SlipRawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[29]);
                            com1.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;
                            com1.Parameters.Add("@SlipDecision", SqlDbType.NVarChar).Value = L2Slipdec;

                            con.Open();
                            com1.ExecuteNonQuery();
                            con.Close();
                            //con.Dispose();
                            //====== Slip Update Record end =========

                            getslip = true;
                            goto callslip;
                        }
                    }
                }


                if (callby == "Slip")
                {
                    if (lst[31] != null && lst[31].ToString() != "")
                        actionTaken = lst[31].ToString();

                    if (lst[32] != null && lst[32].ToString() != "")
                        rejct = Convert.ToByte(lst[32].ToString());

                    if (rejct == 88)
                    {
                        if (lst[33] != null)
                            rejectreasondescrpsn = lst[33].ToString();
                        else
                            rejectreasondescrpsn = "Other Reason";

                    }

                    def = new CMS_L2verificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[1].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[2].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[3].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[5].ToString(),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[7].ToString()),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[8].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[10].ToString()),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[11].ToString()),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[17]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[18].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[19].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[23]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[24]),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[25].ToString(),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[27]),
                        callby = "Slip",
                        ClientCode = ds.Tables[0].Rows[0].ItemArray[28].ToString(),
                        DraweeName = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        SlipID = Convert.ToInt64(lst[28]),
                        SlipRawaDataID = Convert.ToInt64(lst[29]),
                        RejectReason = rejct,
                        Slipdecision = lst[30].ToString(),

                    };
                }
                else
                {
                    if (lst[31] != null && lst[31].ToString() != "")
                        actionTaken = lst[31].ToString();

                    if (lst[32] != null && lst[32].ToString() != "")
                        rejct = Convert.ToByte(lst[32].ToString());

                    if (rejct == 88)
                    {
                        if (lst[33] != null)
                            rejectreasondescrpsn = lst[33].ToString();
                        else
                            rejectreasondescrpsn = "Other Reason";

                    }

                    string draweename = "";
                    string finDate1 = "";
                    if (lst[19] != null && lst[19].ToString() != "")
                    {
                        if (lst[19].ToString().Length != 10)
                            finDate1 = "20" + lst[19].ToString().Substring(4, 2) + "-" + lst[19].ToString().Substring(2, 2) + "-" + lst[19].ToString().Substring(0, 2);
                        else
                            finDate1 = lst[19].ToString();
                    }

                    string slpDate1 = "";
                    if (lst[17] != null && lst[17].ToString() != "")
                    {
                        if (lst[17].ToString().Length != 10)
                            slpDate1 = "20" + lst[17].ToString().Substring(4, 2) + "-" + lst[17].ToString().Substring(2, 2) + "-" + lst[17].ToString().Substring(0, 2);
                        else
                            slpDate1 = lst[17].ToString();
                    }

                    if (lst[16] != null && lst[16].ToString() != "")
                        slipAmt = Convert.ToDecimal(lst[16].ToString());

                    if (lst[18] != null && lst[18].ToString() != "")
                        finalAmt = Convert.ToDecimal(lst[18].ToString());

                    if (lst[25] != null)
                        payeename = lst[25].ToString();

                    if (lst[26] != null)
                        draweename = lst[26].ToString();

                    if (lst[27] != null)
                        userNarration = lst[27].ToString();

                    if (lst[11] != null && lst[11].ToString() != "")
                        tempclientcd = lst[11].ToString();

                    if (lst[8] != null)
                        ScanningType = Convert.ToByte(lst[8]);

                    def = new CMS_L2verificationModel
                    {
                        Id = Convert.ToInt64(lst[0]),
                        CustomerId = Convert.ToInt16(lst[1]),
                        DomainId = Convert.ToInt32(lst[2]),
                        ScanningNodeId = Convert.ToInt32(lst[4]),
                        BranchCode = lst[3].ToString(),
                        BatchNo = Convert.ToInt32(lst[5]),
                        InstrumentType = lst[7].ToString(),
                        SlipNo = Convert.ToInt32(lst[9]),
                        FinalAmount = finalAmt,        //Convert.ToDecimal(lst[18]),
                        FinalDate = finDate1.ToString(),
                        ChequeNoFinal = lst[20].ToString(),
                        SortCodeFinal = lst[21].ToString(),
                        SANFinal = lst[22].ToString(),
                        TransCodeFinal = lst[23].ToString(),

                        FrontTiffImagePath = img.Replace("jpg", "tif"),
                        FrontGreyImagePath = img,

                        CreditAccountNo = lst[15].ToString(),
                        SlipAmount = slipAmt,
                        SlipDate = slpDate1.ToString(),
                        callby = "Cheq",
                        ClientCode = tempclientcd.ToString(),
                        ProcessingDate = Convert.ToDateTime(processingdate),
                        RawDataId = Convert.ToInt64(lst[10]),
                        PayeeName = payeename.ToString(),
                        DraweeName = draweename.ToString(),
                        UserNarration = userNarration.ToString(),

                        SlipID = Convert.ToInt64(lst[28]),
                        SlipRawaDataID = Convert.ToInt64(lst[29]),
                        ScanningType = Convert.ToByte(ScanningType),
                        Action = actionTaken,
                        Slipdecision = lst[30].ToString(),
                        RejectReason = rejct,
                        RejectReasonDescription = rejectreasondescrpsn,
                    };
                }
                ids.Add(def.Id);
                objectlst.Add(def);

                for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    if (callby == "Cheq")
                    {
                        if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                        {
                            checkid = true;
                            // break;
                        }
                        else
                        {
                            checkid = false;
                        }
                    }

                    if (checkid == false)
                    {
                        if (lst[32] != null && lst[32].ToString() != "")
                            rejct = Convert.ToByte(lst[32].ToString());

                        def = new CMS_L2verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[1].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[2].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[3].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[4].ToString()),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[5].ToString(),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[6].ToString()),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[7].ToString()),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[10].ToString()),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[11].ToString()),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[17]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[18].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[19].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[20].ToString(),

                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[23]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[24]),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[27]),
                            ClientCode = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            DraweeName = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            //SlipUserNarration = lst[29].ToString(),
                            RejectReason = rejct,
                            Slipdecision = lst[30].ToString(),
                            SlipID = Convert.ToInt64(lst[28]),
                            SlipRawaDataID = Convert.ToInt64(lst[29]),

                        };
                        objectlst.Add(def);
                        ids.Add(def.Id);
                    }
                }

                getslip = false;
            }
            else
            {
                if (lst[30] != null)
                {
                    if (lst[30].ToString().ToUpper() == "R")
                        L2Slipdec = "L2R";
                    else
                        L2Slipdec = "L2";
                }

                //====== Slip Update Record start =========
                SqlCommand com1 = new SqlCommand("CMS_Update_SlipStatusOnlyL2", con);

                com1.CommandType = CommandType.StoredProcedure;
                com1.Parameters.Add("@SlipID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[28]);
                com1.Parameters.Add("@SlipRawDataID", SqlDbType.BigInt).Value = Convert.ToInt64(lst[29]);
                com1.Parameters.Add("@UserId", SqlDbType.Int).Value = uid;
                com1.Parameters.Add("@SlipDecision", SqlDbType.NVarChar).Value = L2Slipdec;

                con.Open();
                com1.ExecuteNonQuery();
                con.Close();
                //con.Dispose();
                //====== Slip Update Record end =========
                getslip = true;
            }

        callslip:
            if (getslip == true)
            {
                adp = new SqlDataAdapter("OWSelectCMSL2", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;

                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];

                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = StrVFType;

                adp.SelectCommand.Parameters.Add("@BranchCodeId", SqlDbType.NVarChar).Value = StrBranchCode;


                ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    def = new CMS_L2verificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[1]),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[2]),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[3].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[4].ToString(),
                        SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                        SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[9]),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[11].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[12].ToString(),
                        ClientCode = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        SlipRefNo = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        CreditAccountNo = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[17].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[21].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[22].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[23].ToString()),

                        PayeeName = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        SlipDate = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        PickupLocationId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[28].ToString()),
                        PickupLocationCode = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        NoOfInstrument = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[30].ToString()),

                        callby = "Slip",
                    };
                    objectlst.Add(def);

                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new CMS_L2verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[5].ToString()),
                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[6].ToString()),
                            SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[7]),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[8]),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[9]),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                            ClientCode = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            SlipRefNo = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[23].ToString()),
                            callby = "Slip",

                            PayeeName = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            SlipDate = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            PickupLocationId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[28].ToString()),
                            PickupLocationCode = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            NoOfInstrument = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[30].ToString()),

                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                }
            }
        finalexit:
            return (objectlst);

        }


        //------------------OW L2 Chq--------------
        public List<L2verificationModel> selectL2ChequesOnlyHV(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, string processingdate = null, string img = null, string callby = null, bool dirctslipcall = false, int CustomerID = 0, int DomainID = 0, string SlipOnlyAccept = null, double SlipOnlyAcceptAmtThreshold = 0, string StrVFType = null, string CtsSessionType = null, string VFType = null)
        {
            var objectlst = new List<L2verificationModel>();

            try
            {
                L2verificationModel def;
                DataSet ds = new DataSet();
                //OWProcDataContext OWpro = new OWProcDataContext();
                Int64 id = 0;
                byte rejct = 0;
                bool getslip = false;
                string finaldate = "";
                ArrayList ids = new ArrayList();
                bool checkid = false;
                string modaction = "";
                string tempclientcd = "";
                string userNarration = "";
                string rejectreasondescrpsn = "";
                string Clearingtype = "";
                byte L3Status = 0;
                bool mark2pf = false;
                bool ignoreIQA = false;
                string DocType = "B";
                string finalmodified = "";
                int ScanningType = 0;
                Int64 SlipID = 0;
                Int64 SlipRawaDataID = 0;

                if (dirctslipcall == true)
                {
                    getslip = true;
                    goto callslip;
                }

                SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL2", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = "RNormalHV";
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
                //adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[3].ToString());
                //adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[6].ToString());
                //adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[9].ToString());
                //adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[7].ToString();
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
                                                                                                       //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);

                //------------------------Changes on 13/07/2017 --------------For Select Data for only slip updation-----------

                //adp.SelectCommand.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[38].ToString());
                //adp.SelectCommand.Parameters.Add("@UpdateSlipOnly", SqlDbType.NVarChar).Value = SlipOnlyAccept;
                //adp.SelectCommand.Parameters.Add("@AmountThreshold", SqlDbType.NVarChar).Value = SlipOnlyAcceptAmtThreshold;

                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];


                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //--------------Checking Last record and list record are same------

                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        if (Convert.ToInt64(lst[0]) == Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]))
                        {
                            id = Convert.ToInt64(lst[0]);

                            if (lst[15] != null && lst[15].ToString() != "")
                                rejct = Convert.ToByte(lst[15].ToString());

                            if (lst[21] != null && lst[21].ToString() != "")
                            {
                                if (lst[21].ToString().Length != 10)
                                    finaldate = "20" + lst[21].ToString().Substring(4, 2) + "-" + lst[21].ToString().Substring(2, 2) + "-" + lst[21].ToString().Substring(0, 2);
                                else
                                    finaldate = lst[21].ToString();
                            }
                            if (lst[12].ToString() == "A")
                            {
                                if (Convert.ToBoolean(lst[30]) == true)
                                    modaction = "M";
                                else
                                    modaction = "A";
                            }
                            else if (lst[12].ToString() == "R")
                            {
                                modaction = "R";

                                if (rejct == 88)
                                {
                                    if (lst[33] != null)
                                        rejectreasondescrpsn = lst[33].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";

                                }
                            }

                            if (lst[11] != null)
                                tempclientcd = lst[11].ToString();
                            if (lst[32] != null)
                                userNarration = lst[32].ToString();

                            if (lst[34] != null)
                                Clearingtype = lst[34].ToString();
                            //-----------------Marking P2F----------------------//

                            if (lst[35] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[35]);
                                if (mark2pf == true)
                                {
                                    ignoreIQA = true;
                                    DocType = "C";
                                }
                                else
                                {
                                    ignoreIQA = false;
                                    DocType = "B";
                                }

                            }
                            else
                            {
                                ignoreIQA = false;
                                DocType = "B";
                            }


                            //---------------Added on 14/07/2017----------------
                            if (lst[39] != null)
                                finalmodified = lst[39].ToString();

                            //OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            //    lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, Session, processingdate,
                            //    Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration, rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalmodified, null, lst[40].ToString(), Convert.ToInt32(lst[41]), Convert.ToInt32(lst[42]));

                            SqlCommand cmd = new SqlCommand("UpdateOWL2", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@RawDataId", Convert.ToInt64(lst[14]));
                            cmd.Parameters.AddWithValue("@Uid", uid);
                            cmd.Parameters.AddWithValue("@InstrumentType", lst[5].ToString());
                            cmd.Parameters.AddWithValue("@FinalAmount", Convert.ToDouble(lst[20].ToString()));
                            cmd.Parameters.AddWithValue("@FinalDate", finaldate);
                            cmd.Parameters.AddWithValue("@FinalChqNo", lst[22].ToString());
                            cmd.Parameters.AddWithValue("@FinalSortcode", lst[23].ToString());
                            cmd.Parameters.AddWithValue("@FinalSAN", lst[24].ToString());
                            cmd.Parameters.AddWithValue("@FinalTransCode", lst[25].ToString());
                            cmd.Parameters.AddWithValue("@CreditAccountNo", lst[1].ToString());
                            cmd.Parameters.AddWithValue("@PayeName", lst[27].ToString());
                            cmd.Parameters.AddWithValue("@status", Convert.ToInt16(lst[13]));
                            cmd.Parameters.AddWithValue("@RejectReason", rejct);
                            cmd.Parameters.AddWithValue("@ActionTaken", modaction);
                            cmd.Parameters.AddWithValue("@LName", Session);
                            cmd.Parameters.AddWithValue("@ProcessingDate", processingdate);
                            cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt16(lst[17].ToString()));
                            cmd.Parameters.AddWithValue("@DomainId", Convert.ToInt32(lst[16].ToString()));
                            cmd.Parameters.AddWithValue("@ScanningNodeId", Convert.ToInt32(lst[9].ToString()));
                            cmd.Parameters.AddWithValue("@ChequeAmtotal", null);
                            cmd.Parameters.AddWithValue("@SlipAmount", 0);
                            cmd.Parameters.AddWithValue("@ChequeTotal", null);
                            cmd.Parameters.AddWithValue("@UserNarration", userNarration);
                            cmd.Parameters.AddWithValue("@RejectReasonDescription", rejectreasondescrpsn);
                            cmd.Parameters.AddWithValue("@CTSNONCTS", Clearingtype);
                            cmd.Parameters.AddWithValue("@CBSAccountInformation", lst[18].ToString());
                            cmd.Parameters.AddWithValue("@CBSJointAccountInformation", lst[19].ToString());
                            cmd.Parameters.AddWithValue("@IgnoreIQA", ignoreIQA);
                            cmd.Parameters.AddWithValue("@DocType", DocType);
                            cmd.Parameters.AddWithValue("@Modified", finalmodified);
                            cmd.Parameters.AddWithValue("@strHoldReason", "");
                            cmd.Parameters.AddWithValue("@DraweeName", lst[40].ToString());
                            cmd.Parameters.AddWithValue("@NRESourceOfFundId", Convert.ToInt32(lst[41]));
                            cmd.Parameters.AddWithValue("@NROSourceOfFundId", Convert.ToInt32(lst[42]));
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();

                            
                            getslip = true;
                            goto callslip;
                        }

                    }


                    //else
                    //{
                    if (lst[15] != null && lst[15].ToString() != "")
                        rejct = Convert.ToByte(lst[15].ToString());
                    if (rejct == 88)
                    {
                        if (lst[33] != null)
                            rejectreasondescrpsn = lst[33].ToString();
                        else
                            rejectreasondescrpsn = "Other Reason";

                    }
                    def = new L2verificationModel();

                    def.Id = Convert.ToInt64(lst[0]);
                    def.CustomerId = Convert.ToInt16(lst[17]);
                    def.DomainId = Convert.ToInt32(lst[16]);
                    def.ScanningNodeId = Convert.ToInt32(lst[9]);
                    def.BranchCode = lst[7].ToString();
                    def.BatchNo = Convert.ToInt32(lst[3]);
                    def.InstrumentType = lst[5].ToString();
                    def.ClearingType = lst[8].ToString();
                    def.SlipNo = 0;
                    def.FinalAmount = Convert.ToDecimal(lst[20]);
                    def.FinalDate = lst[21].ToString();
                    def.ChequeNoFinal = lst[22].ToString();
                    def.SortCodeFinal = lst[23].ToString();
                    def.SANFinal = lst[24].ToString();
                    def.TransCodeFinal = lst[25].ToString();
                    def.Status = Convert.ToByte(lst[13]);
                    def.FrontGreyImagePath = img;
                    def.CBSAccountInformation = lst[18].ToString();
                    def.CBSJointAccountInformation = lst[19].ToString();
                    def.CreditAccountNo = lst[1].ToString();
                    def.L1RejectReason = Convert.ToByte(lst[28].ToString());
                    def.RejectReason = rejct;
                    def.L1VerificationStatus = Convert.ToByte(lst[29].ToString());
                    def.ProcessingDate = Convert.ToDateTime(processingdate);
                    def.RawDataId = Convert.ToInt64(lst[14]);
                    def.PayeeName = lst[27].ToString();
                    def.Action = lst[12].ToString();
                    def.callby = callby;
                    def.ClientCode = lst[11].ToString();
                    def.AccModified = Convert.ToBoolean(lst[31].ToString());
                    def.DraweeName = lst[40].ToString();
                    def.NRESourceOfFundId = Convert.ToInt32(lst[41]);
                    def.NROSourceOfFundId = Convert.ToInt32(lst[42]);

                    if (lst[32] != null)
                        def.UserNarration = lst[32].ToString();
                    else
                        def.UserNarration = "";

                    def.RejectReasonDescription = rejectreasondescrpsn;
                    def.ctsNonCtsMark = lst[34].ToString();
                    def.P2fMark = Convert.ToBoolean(lst[35].ToString());
                    def.Modified2 = lst[39].ToString();
                    def.SlipID = 0;
                    def.SlipRawaDataID = 0;


                    
                    ids.Add(def.Id);
                    objectlst.Add(def);

                    //------------------------END------------------------//
                    for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                    {
                        //if (callby == "Cheq")
                        //{
                        if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            checkid = true;
                        else
                            checkid = false;
                        //}

                        if (checkid == false)
                        {
                            def = new L2verificationModel
                            {
                                Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                                BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[1]),
                                BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[2]),
                                InstrumentType = ds.Tables[0].Rows[index].ItemArray[3].ToString(),
                                ClearingType = ds.Tables[0].Rows[index].ItemArray[4].ToString(),
                                //SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                                //SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[6].ToString()),
                                //SlipAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[7]),
                                //ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[8]),
                                Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[5]),
                                FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                                FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[7].ToString(),
                                BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[8].ToString(),
                                ClientCode = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                                SlipRefNo = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                                CreditAccountNo = ds.Tables[0].Rows[index].ItemArray[11].ToString(),
                                BranchCode = ds.Tables[0].Rows[index].ItemArray[12].ToString(),
                                ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[13].ToString()),
                                ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[14].ToString()),
                                RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[15].ToString()),
                                DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[16].ToString()),
                                CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                                ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                                L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                                L1UserId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                                L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                                PayeeName = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                                CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                                CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                                UserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                                SlipUserNarration = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                                RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                                FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[28]),
                                FinalDate = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                                ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                                SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                                SANFinal = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                                TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                                DocType = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                                Modified1 = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                                FrontUVImagePath = ds.Tables[0].Rows[index]["FrontUVImage"].ToString(),
                                DraweeName = ds.Tables[0].Rows[index]["DraweeName"].ToString(),
                                NRESourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[index]["NRESourceOfFundId"]),
                                NROSourceOfFundId = Convert.ToInt32(ds.Tables[0].Rows[index]["NROSourceOfFundId"]),
                                callby = "Cheq",
                                // ClientCode = lst[11].ToString(),
                                //AccModified = Convert.ToBoolean(lst[31].ToString()),
                                //SlipID = Convert.ToInt64(lst[36]),
                                //SlipRawaDataID = Convert.ToInt64(lst[37]),
                                //  SlipUserNarration = lst[33].ToString(),

                            };
                            objectlst.Add(def);
                            ids.Add(def.Id);
                        }
                    }
                    //  }
                }

            callslip:

                return (objectlst);
            }
            catch (Exception e)
            {
                string message = "";
                string innerExcp = "";
                if (e.Message != null)
                    message = e.Message.ToString();
                if (e.InnerException != null)
                    innerExcp = e.InnerException.Message;

                return (objectlst);
            }
            finally
            {
                con.Close();
            }
        }

        private void logerror(string errormsg, string errordesc)
        {
           // var writeLog = ConfigurationManager.AppSettings["WriteLog"].ToString().ToUpper();

           
                ErrorDisplay er = new ErrorDisplay();
                string ServerPath = "";
                string filename = "";
                string fileNameWithPath = "";
                //FormsAuthentication.SignOut();


                //ViewBag.Error = e.InnerException;

                //-------------------------------- ServerPath = Server.MapPath("~/Logs/");----
                ServerPath =HttpContext.Current.Server.MapPath("~/Logs/");
                if (System.IO.Directory.Exists(ServerPath) == false)
                {
                    System.IO.Directory.CreateDirectory(ServerPath);
                }
                filename = DateTime.Now.ToString("ddMMyyyy") + "Logs.txt";
                fileNameWithPath = ServerPath + filename;
                System.IO.StreamWriter str = new System.IO.StreamWriter(fileNameWithPath, true, System.Text.Encoding.Default);
                //  str.WriteLine("hello");
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": Exception: " + errormsg);
                str.WriteLine(DateTime.Now.ToShortTimeString() + ": StackTrace: " + errordesc);
                str.Close();
           


        }
    }
}