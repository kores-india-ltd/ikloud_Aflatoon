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

        public List<L1verificationModel> selectL1Cheques(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, string processingdate = null, string img = null, string callby = null, string tempclientcd = null, string CreditCardValidationReq = null, string CreditCardValidAcNo = null, string CreditCardInValidAcNo = null, int CustomerID = 0, int DomainID = 0, bool dirctslipcall = false, string CtsSessionType = null, string SrcWebIP = null, string DestWepIP = null, string SrcWebName = null, string DestWebName = null)
        {
            var objectlst = new List<L1verificationModel>();
            L1verificationModel def;
            DataSet ds = new DataSet();
            OWProcDataContext OWpro = new OWProcDataContext();
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

            if (dirctslipcall == true)
            {
                getslip = true;
                goto callslip;
            }

            ArrayList ids = new ArrayList();
            bool checkid = false;

            SqlDataAdapter adp = new SqlDataAdapter("OWSelectL1Cheques", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
            adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
            adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[3].ToString());
            adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[6].ToString());
            adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[9].ToString());
            adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[7].ToString();
            //-------------Added on 17-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
            //-------------Added on 18-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);

            //-------------Added on 12-09-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];


            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //--------------Checking Last record and list record are same------
                if (callby != "Slip")
                {
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
                            if (lst[11] != null && lst[11].ToString() != "")
                                tempclientcd = lst[11].ToString();

                            if (lst[28] != null)
                                userNarration = lst[28].ToString();

                            if (rejct == 88)
                            {
                                if (lst[31] != null)
                                    rejectreasondescrpsn = lst[31].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";

                            }

                            if (lst[32] != null)
                                Clearingtype = lst[32].ToString();
                            if (lst[27] != null)
                                payeename = lst[27].ToString();
                            ////---------------------added On 01-03-2017---------------
                            //if (lst[1].ToString().Length == 16)
                            //{
                            //    if (CreditCardValidationReq == "1")
                            //    {
                            //        if (lst[19].ToString().Split('|').ElementAt(1) == "S")
                            //            creditcardno = CreditCardValidAcNo;
                            //        else
                            //            creditcardno = CreditCardInValidAcNo;

                            //    }
                            //}
                            //else
                            //    creditcardno = lst[1].ToString();
                            //------------------------
                            //---------------Added On 26/09/2017---
                            if (lst[35] != null)
                                Modified = lst[35].ToString();


                            OWpro.UpdateOWL1(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(),
                                lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(), payeename, Convert.ToInt16(lst[13]), rejct, lst[12].ToString(), Session, processingdate,
                                Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, tempclientcd, userNarration, rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), Modified);

                            //-----------------Update Slip------------------
                            id = Convert.ToInt64(lst[0]);
                            if (lst[30] != null)
                            {
                                if (lst[30].ToString().ToUpper() == "R")
                                    L1dec = "L1R";
                                else
                                    L1dec = "L1";
                            }
                            //---------------Added On 25/05/2017------------------
                            if (lst[33] != null)
                                SlipID = Convert.ToInt64(lst[33]);
                            if (lst[34] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[34]);

                            if (lst[35] != null)
                                Modified = lst[35].ToString();


                            OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt16(lst[3].ToString()), Convert.ToInt16(lst[6].ToString()), Convert.ToInt16(lst[9].ToString()), lst[7].ToString(),
                                Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), payeename, lst[18].ToString(), lst[19].ToString(), L1dec, rejct, tempclientcd, userNarration, rejectreasondescrpsn,
                                Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session, SlipID, SlipRawaDataID, null, 0, Modified);

                            getslip = true;
                            goto callslip;
                        }

                    }
                }
                //else
                //{
                if (callby == "Slip")
                {
                    if (lst[15] != null && lst[15].ToString() != "")
                        rejct = Convert.ToByte(lst[15].ToString());

                    def = new L1verificationModel
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
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[18].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[19].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[20].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                        CBSAccountInformation = lst[18].ToString(),
                        CBSJointAccountInformation = lst[19].ToString(),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[23].ToString()),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[24].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[25]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[26]),
                        CreditAccountNo = lst[1].ToString(),
                        SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                        callby = "Slip",
                        ClientCode = lst[11].ToString(),
                        SlipUserNarration = lst[29].ToString(),
                        RejectReason = rejct,
                        Slipdecision = lst[30].ToString(),
                        SlipID = Convert.ToInt64(lst[33]),
                        SlipRawaDataID = Convert.ToInt64(lst[34]),

                    };
                    //------------------------------------------Added 18-01-2017------------------------//

                }
                else
                {
                    if (lst[15] != null && lst[15].ToString() != "")
                        rejct = Convert.ToByte(lst[15].ToString());
                    if (rejct == 88)
                    {
                        if (lst[31] != null)
                            rejectreasondescrpsn = lst[31].ToString();
                        else
                            rejectreasondescrpsn = "Other Reason";

                    }

                    def = new L1verificationModel
                   {
                       Id = Convert.ToInt64(lst[0]),
                       CustomerId = Convert.ToInt16(lst[17]),
                       DomainId = Convert.ToInt32(lst[16]),
                       ScanningNodeId = Convert.ToInt32(lst[9]),
                       BranchCode = lst[7].ToString(),
                       BatchNo = Convert.ToInt32(lst[3]),
                       InstrumentType = lst[5].ToString(),
                       ClearingType = lst[8].ToString(),
                       SlipNo = Convert.ToInt32(lst[6]),
                       FinalAmount = Convert.ToDecimal(lst[20]),
                       FinalDate = lst[21].ToString(),
                       ChequeNoFinal = lst[22].ToString(),
                       SortCodeFinal = lst[23].ToString(),
                       SANFinal = lst[24].ToString(),
                       TransCodeFinal = lst[25].ToString(),
                       Status = Convert.ToByte(lst[13]),
                       FrontTiffImagePath = img.Replace("jpg", "tif"),
                       FrontGreyImagePath = img,
                       //BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                       CBSAccountInformation = lst[18].ToString(),
                       CBSJointAccountInformation = lst[19].ToString(),
                       //SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[23].ToString()),
                       //ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[24].ToString()),
                       CreditAccountNo = lst[1].ToString(),
                       SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                       callby = "Cheq",
                       ClientCode = lst[11].ToString(),
                       ProcessingDate = Convert.ToDateTime(processingdate),
                       RawDataId = Convert.ToInt64(lst[14]),
                       PayeeName = lst[27].ToString(),
                       Action = lst[12].ToString(),
                       UserNarration = lst[28].ToString(),
                       SlipUserNarration = lst[29].ToString(),
                       RejectReason = rejct,
                       RejectReasonDescription = rejectreasondescrpsn,
                       Slipdecision = lst[30].ToString(),
                       ctsNonCtsMark = lst[32].ToString(),
                       SlipID = Convert.ToInt64(lst[33]),
                       SlipRawaDataID = Convert.ToInt64(lst[34]),
                       Modified = lst[35].ToString(),
                   };
                }
                ids.Add(def.Id);
                objectlst.Add(def);

                //------------------------END------------------------//
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
                        if (lst[15] != null && lst[15].ToString() != "")
                            rejct = Convert.ToByte(lst[15].ToString());

                        def = new L1verificationModel
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
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[18].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[19].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[20].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                            CBSAccountInformation = lst[18].ToString(),
                            CBSJointAccountInformation = lst[19].ToString(),
                            //CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            //CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[23].ToString()),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[24].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[25]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[26]),
                            CreditAccountNo = lst[1].ToString(),
                            SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                            ClientCode = lst[11].ToString(),
                            SlipUserNarration = lst[29].ToString(),
                            RejectReason = rejct,
                            Slipdecision = lst[30].ToString(),
                            SlipID = Convert.ToInt64(lst[33]),
                            SlipRawaDataID = Convert.ToInt64(lst[34]),

                        };
                        objectlst.Add(def);
                        ids.Add(def.Id);
                    }
                }
                getslip = false;
                //  }
            }
            else
            {
                //-----------------Update Slip------------------
                id = Convert.ToInt64(lst[0]);
                if (lst[29] != null)
                    userNarration = lst[29].ToString();

                if (lst[15] != null && lst[15].ToString() != "")
                    rejct = Convert.ToByte(lst[15].ToString());

                if (rejct == 88)
                {
                    if (lst[31] != null)
                        rejectreasondescrpsn = lst[31].ToString();
                    else
                        rejectreasondescrpsn = "Other Reason";

                }
                //-----------------Update Slip------------------
                id = Convert.ToInt64(lst[0]);
                if (lst[30] != null)
                {
                    if (lst[30].ToString().ToUpper() == "R")
                        L1dec = "L1R";
                    else
                        L1dec = "L1";
                }
                //---------------Added On 25/05/2017------------------
                if (lst[33] != null)
                    SlipID = Convert.ToInt64(lst[33]);
                if (lst[34] != null)
                    SlipRawaDataID = Convert.ToInt64(lst[34]);
                if (lst[35] != null)
                    Modified = lst[35].ToString();

                OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                    Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), payeename, lst[18].ToString(), lst[19].ToString(), L1dec, rejct, tempclientcd, userNarration, rejectreasondescrpsn,
                    Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session, SlipID, SlipRawaDataID, null, 0, Modified);

                getslip = true;
            }

        callslip:
            if (getslip == true)
            {
                adp = new SqlDataAdapter("OWSelectL1", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;

                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);
                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];



                ds = new DataSet();
                adp.Fill(ds);
                // var objectlst = new List<L1Verification>();
                // L1Verification def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new L1verificationModel
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
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[10].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[11].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[12].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
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
                        //CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        //CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[17].ToString(),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L1verificationModel
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
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[10].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[11].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[12].ToString().Replace(SrcWebIP, DestWepIP).Replace(SrcWebName, DestWebName),
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
                            //CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            //CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                }

            }
            return (objectlst);
        }
        //------------------L2 verification---------------------------------//
        public List<L2verificationModel> selectL2Cheques(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, string processingdate = null, string img = null, string callby = null, bool dirctslipcall = false, int CustomerID = 0, int DomainID = 0, string SlipOnlyAccept = null, double SlipOnlyAcceptAmtThreshold = 0, string StrVFType = null, string CtsSessionType = null)
        {

            var objectlst = new List<L2verificationModel>();
            L2verificationModel def;
            DataSet ds = new DataSet();
            OWProcDataContext OWpro = new OWProcDataContext();
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
            int ScanningType = 0;
            string finalmodified = "";
            Int64 SlipID = 0;
            Int64 SlipRawaDataID = 0;

            if (dirctslipcall == true)
            {
                getslip = true;
                goto callslip;
            }

            SqlDataAdapter adp = new SqlDataAdapter("OWSelectL2Cheques", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
            adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
            adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[3].ToString());
            adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[6].ToString());
            adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[9].ToString());
            adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[7].ToString();
            //-------------Added on 17-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
            //-------------Added on 18-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);

            //------------------------Changes on 13/07/2017 -------------------------------For Select Data for only slip updation-----------

            adp.SelectCommand.Parameters.Add("@ScanningType", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[38].ToString());
            adp.SelectCommand.Parameters.Add("@UpdateSlipOnly", SqlDbType.NVarChar).Value = SlipOnlyAccept;
            adp.SelectCommand.Parameters.Add("@AmountThreshold", SqlDbType.NVarChar).Value = SlipOnlyAcceptAmtThreshold;

            //-------------Added on 12-09-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];


            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //--------------Checking Last record and list record are same------
                if (callby != "Slip")
                {
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



                            OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                                lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, Session, processingdate,
                                Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, userNarration, rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalmodified);

                            //-----------------Update Slip------------------
                            byte acmodified = 0;
                            if (Convert.ToBoolean(lst[31]) == true)
                                acmodified = 1;
                            else
                                acmodified = 0;

                            //---------------Added On 21/06/2017------------------
                            if (lst[36] != null)
                                SlipID = Convert.ToInt64(lst[36]);
                            if (lst[37] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[37]);

                            //---------------Added on 14/07/2017----------------
                            if (lst[38] != null)
                                ScanningType = Convert.ToInt16(lst[38]);

                            id = Convert.ToInt64(lst[0]);
                            //-----------------------Update Slip As Rejected If any cheque get rejected-------
                            OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                                  Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L2", acmodified, tempclientcd, userNarration, null,
                                  Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session, SlipID, SlipRawaDataID, SlipOnlyAccept, ScanningType, finalmodified);

                            getslip = true;
                            goto callslip;
                        }

                    }
                }
                //else
                //{
                if (callby == "Slip")
                {
                    def = new L2verificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[1]),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[2].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[3].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[7].ToString()),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[8].ToString()),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[11].ToString()),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[12].ToString()),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[17].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[20].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[22].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[24].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[25].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[27].ToString()),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[28].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[31].ToString()),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[33].ToString(),
                        Modified1 = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                        CBSAccountInformation = lst[18].ToString(),
                        CBSJointAccountInformation = lst[19].ToString(),
                        CreditAccountNo = lst[1].ToString(),
                        SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                        callby = callby,
                        ClientCode = lst[11].ToString(),
                        AccModified = Convert.ToBoolean(lst[31].ToString()),
                        SlipID = Convert.ToInt64(lst[36]),
                        SlipRawaDataID = Convert.ToInt64(lst[37]),
                        Modified2 = lst[39].ToString(),

                        // SlipUserNarration = lst[33].ToString(),
                    };

                }
                else
                {
                    if (lst[15] != null && lst[15].ToString() != "")
                        rejct = Convert.ToByte(lst[15].ToString());
                    if (rejct == 88)
                    {
                        if (lst[33] != null)
                            rejectreasondescrpsn = lst[33].ToString();
                        else
                            rejectreasondescrpsn = "Other Reason";

                    }

                    def = new L2verificationModel
                    {
                        Id = Convert.ToInt64(lst[0]),
                        CustomerId = Convert.ToInt16(lst[17]),
                        DomainId = Convert.ToInt32(lst[16]),
                        ScanningNodeId = Convert.ToInt32(lst[9]),
                        BranchCode = lst[7].ToString(),
                        BatchNo = Convert.ToInt32(lst[3]),
                        InstrumentType = lst[5].ToString(),
                        ClearingType = lst[8].ToString(),
                        SlipNo = Convert.ToInt32(lst[6]),
                        FinalAmount = Convert.ToDecimal(lst[20]),
                        FinalDate = lst[21].ToString(),
                        ChequeNoFinal = lst[22].ToString(),
                        SortCodeFinal = lst[23].ToString(),
                        SANFinal = lst[24].ToString(),
                        TransCodeFinal = lst[25].ToString(),
                        Status = Convert.ToByte(lst[13]),
                        FrontGreyImagePath = img,
                        CBSAccountInformation = lst[18].ToString(),
                        CBSJointAccountInformation = lst[19].ToString(),
                        CreditAccountNo = lst[1].ToString(),
                        SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                        L1RejectReason = Convert.ToByte(lst[28].ToString()),
                        RejectReason = rejct,
                        L1VerificationStatus = Convert.ToByte(lst[29].ToString()),
                        ProcessingDate = Convert.ToDateTime(processingdate),
                        RawDataId = Convert.ToInt64(lst[14]),
                        PayeeName = lst[27].ToString(),
                        Action = lst[12].ToString(),
                        callby = callby,
                        ClientCode = lst[11].ToString(),
                        AccModified = Convert.ToBoolean(lst[31].ToString()),
                        UserNarration = lst[32].ToString(),
                        RejectReasonDescription = rejectreasondescrpsn,
                        ctsNonCtsMark = lst[34].ToString(),
                        P2fMark = Convert.ToBoolean(lst[35].ToString()),
                        SlipID = Convert.ToInt64(lst[36]),
                        SlipRawaDataID = Convert.ToInt64(lst[37]),
                        Modified2 = lst[39].ToString(),
                        // SlipUserNarration = lst[33].ToString(),
                    };
                }
                ids.Add(def.Id);
                objectlst.Add(def);

                //------------------------END------------------------//
                for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    if (callby == "Cheq")
                    {
                        if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            checkid = true;
                        else
                            checkid = false;
                    }

                    if (checkid == false)
                    {
                        def = new L2verificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[1]),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[2].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[3].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[4].ToString()),
                            ScanningNodeId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[5].ToString()),
                            BranchCode = ds.Tables[0].Rows[index].ItemArray[6].ToString(),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[7].ToString()),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[8].ToString()),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[11].ToString()),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[12].ToString()),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[18]),
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[27].ToString()),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[28].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[31].ToString()),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                            Modified1 = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            CBSAccountInformation = lst[18].ToString(),
                            CBSJointAccountInformation = lst[19].ToString(),
                            CreditAccountNo = lst[1].ToString(),
                            SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                            callby = callby,
                            ClientCode = lst[11].ToString(),
                            AccModified = Convert.ToBoolean(lst[31].ToString()),
                            SlipID = Convert.ToInt64(lst[36]),
                            SlipRawaDataID = Convert.ToInt64(lst[37]),
                            Modified2 = lst[39].ToString(),
                            //  SlipUserNarration = lst[33].ToString(),

                        };
                        objectlst.Add(def);
                        ids.Add(def.Id);
                    }
                }
                getslip = false;
                //  }
            }
            else
            {
                //-----------------Update Slip------------------
                byte acmodified = 0;
                if (Convert.ToBoolean(lst[31]) == true)
                    acmodified = 1;
                else
                    acmodified = 2;

                if (lst[32] != null)
                    userNarration = lst[32].ToString();
                //---------------Added On 21/06/2017------------------
                if (lst[36] != null)
                    SlipID = Convert.ToInt64(lst[36]);
                if (lst[37] != null)
                    SlipRawaDataID = Convert.ToInt64(lst[37]);
                //---------------Added on 14/07/2017----------------
                if (lst[38] != null)
                    ScanningType = Convert.ToInt16(lst[38]);

                //---------------Added on 14/07/2017----------------
                if (lst[39] != null)
                    finalmodified = lst[39].ToString();

                OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                   Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L2", acmodified, tempclientcd, userNarration, null,
                   Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session, SlipID, SlipRawaDataID, SlipOnlyAccept, ScanningType, finalmodified);

                //id = Convert.ToInt64(lst[0]);
                ////-----------------Update Slip------------------
                //byte acmodified = 0;
                //if (Convert.ToBoolean(lst[31]) == true)
                //    acmodified = 1;
                //else
                //    acmodified = 0;
                ////-----------------------Update Slip As Rejected If any cheque get rejected-------
                //SqlDataAdapter adp1 = new SqlDataAdapter("SelectOnlyIDForVF", con);
                //adp1.SelectCommand.CommandType = CommandType.StoredProcedure;
                //adp1.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                //adp1.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
                //adp1.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt16(lst[3].ToString());
                //adp1.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt16(lst[6].ToString());
                //adp1.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt16(lst[9].ToString());
                //adp1.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[7].ToString();
                //adp1.SelectCommand.Parameters.Add("@modeule", SqlDbType.NVarChar).Value = "L2R";
                //ds = new DataSet();

                //adp1.Fill(ds);

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt16(lst[3].ToString()), Convert.ToInt16(lst[6].ToString()), Convert.ToInt16(lst[9].ToString()), lst[7].ToString(),
                //        Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L2", 2, tempclientcd);
                //}
                //else
                //{
                //    OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt16(lst[3].ToString()), Convert.ToInt16(lst[6].ToString()), Convert.ToInt16(lst[9].ToString()), lst[7].ToString(),
                //        Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L2", acmodified, tempclientcd);
                //}
                getslip = true;

            }
        callslip:
            if (getslip == true)
            {
                //string StrVFType = "";
                //if (VFType == 1)
                //    StrVFType = "RNormal";
                //else if (id == 2)
                //    StrVFType = "RHold";
                //else if (id == 3)
                //    StrVFType = "BNormal";
                //else if (id == 4)
                //    StrVFType = "BHold";

                adp = new SqlDataAdapter("OWSelectL2", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = StrVFType;
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);

                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];

                ds = new DataSet();
                adp.Fill(ds);
                // var objectlst = new List<L1Verification>();
                // L1Verification def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new L2verificationModel
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
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[24].ToString()),
                        L1UserId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[25].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[26].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[28].ToString(),
                        CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[29].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        SlipUserNarration = ds.Tables[0].Rows[0].ItemArray[30].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[31].ToString(),
                        Modified1 = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        callby = "Slip",
                        //CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        //CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[17].ToString(),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L2verificationModel
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
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[24].ToString()),
                            L1UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[25].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            SlipUserNarration = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                            Modified1 = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            callby = "Slip",
                            //CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            //CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                }

            }
            return (objectlst);
        }

        //------------------L3 verification---------------------------------//
        public List<L3VerificationModel> selectL3Cheques(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, string processingdate = null, string img = null, string callby = null, bool dirctslipcall = false, int CustomerID = 0, int DomainID = 0, string CtsSessionType = null)
        {

            var objectlst = new List<L3VerificationModel>();
            L3VerificationModel def;
            DataSet ds = new DataSet();
            OWProcDataContext OWpro = new OWProcDataContext();
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
            bool mark2pf = false;
            bool ignoreIQA = false;
            string DocType = "B";
            string finalModified = "";
            Int64 SlipID = 0;
            Int64 SlipRawaDataID = 0;

            if (dirctslipcall == true)
            {
                getslip = true;
                goto callslip;
            }

            SqlDataAdapter adp = new SqlDataAdapter("OWSelectL3Cheques", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
            adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
            adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[3].ToString());
            adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[6].ToString());
            adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[9].ToString());
            adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[7].ToString();
            //-------------Added on 17-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
            //-------------Added on 18-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);

            //-------------Added on 12-09-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];


            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //--------------Checking Last record and list record are same------
                if (callby != "Slip")
                {
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
                                    if (lst[36] != null)
                                        rejectreasondescrpsn = lst[36].ToString();
                                    else
                                        rejectreasondescrpsn = "Other Reason";
                                }
                            }

                            if (lst[11] != null)
                                tempclientcd = lst[11].ToString();
                            if (lst[35] != null)
                                userNarration = lst[35].ToString();

                            if (lst[37] != null)
                                Clearingtype = lst[37].ToString();

                            //------------------marking P2F--------------------//
                            if (lst[38] != null)
                            {
                                mark2pf = Convert.ToBoolean(lst[38]);
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

                            if (lst[41] != null)
                                finalModified = lst[41].ToString();

                            OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                                lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, Session, processingdate,
                                Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, Convert.ToDouble(lst[2].ToString()), null, userNarration, rejectreasondescrpsn,
                                Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalModified);

                            //-----------------Update Slip------------------
                            byte acmodified = 0;
                            if (Convert.ToBoolean(lst[31]) == true)
                                acmodified = 1;
                            else if (Convert.ToBoolean(lst[34]) == true)
                                acmodified = 2;
                            else
                                acmodified = 0;
                            //---------------Added On 21/06/2017------------------
                            if (lst[39] != null)
                                SlipID = Convert.ToInt64(lst[39]);
                            if (lst[40] != null)
                                SlipRawaDataID = Convert.ToInt64(lst[40]);

                            if (lst[41] != null)
                                finalModified = lst[41].ToString();

                            id = Convert.ToInt64(lst[0]);
                            OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                                Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L3", acmodified, tempclientcd, userNarration, null,
                                Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session, SlipID, SlipRawaDataID, null, 0, finalModified);

                            getslip = true;
                            goto callslip;
                        }

                    }
                }
                //else
                //{
                if (callby == "Slip")
                {
                    def = new L3VerificationModel
                    {
                        Id = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[0]),
                        RawDataId = Convert.ToInt64(ds.Tables[0].Rows[0].ItemArray[1]),
                        ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[0].ItemArray[2].ToString()),
                        CustomerId = Convert.ToInt16(ds.Tables[0].Rows[0].ItemArray[3].ToString()),
                        DomainId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[4].ToString()),
                        ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[5].ToString()),
                        BranchCode = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                        BatchNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[7].ToString()),
                        BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[8].ToString()),
                        InstrumentType = ds.Tables[0].Rows[0].ItemArray[9].ToString(),
                        ClearingType = ds.Tables[0].Rows[0].ItemArray[10].ToString(),
                        SlipNo = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[11].ToString()),
                        FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[12].ToString()),
                        FinalDate = ds.Tables[0].Rows[0].ItemArray[13].ToString(),
                        ChequeNoFinal = ds.Tables[0].Rows[0].ItemArray[14].ToString(),
                        SortCodeFinal = ds.Tables[0].Rows[0].ItemArray[15].ToString(),
                        SANFinal = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        TransCodeFinal = ds.Tables[0].Rows[0].ItemArray[17].ToString(),
                        Status = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[18].ToString()),
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[19].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[20].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[22].ToString(),
                        DocType = ds.Tables[0].Rows[0].ItemArray[23].ToString(),
                        FrontTiffImagePath = ds.Tables[0].Rows[0].ItemArray[24].ToString(),
                        FrontGreyImagePath = ds.Tables[0].Rows[0].ItemArray[25].ToString(),
                        BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[26].ToString(),
                        SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[27].ToString()),
                        ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[28].ToString()),
                        L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[29].ToString()),
                        L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[30].ToString()),
                        ScanningType = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[33].ToString()),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[35].ToString(),
                        Modified2 = ds.Tables[0].Rows[0].ItemArray[36].ToString(),
                        CBSAccountInformation = lst[18].ToString(),
                        CBSJointAccountInformation = lst[19].ToString(),
                        CreditAccountNo = lst[1].ToString(),
                        SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                        AccModified = Convert.ToBoolean(lst[31].ToString()),
                        globalmodified = Convert.ToBoolean(lst[34].ToString()),
                        callby = callby,
                        SlipID = Convert.ToInt64(lst[39]),
                        SlipRawaDataID = Convert.ToInt64(lst[40]),
                        Modified3 = lst[41].ToString(),

                    };

                }
                else
                {
                    if (lst[15] != null && lst[15].ToString() != "")
                        rejct = Convert.ToByte(lst[15].ToString());
                    if (rejct == 88)
                    {
                        if (lst[36] != null && lst[36].ToString() != "")
                            rejectreasondescrpsn = lst[36].ToString();
                        else
                            rejectreasondescrpsn = "Other Reason";
                    }

                    def = new L3VerificationModel
                    {
                        Id = Convert.ToInt64(lst[0]),
                        CustomerId = Convert.ToInt16(lst[17]),
                        DomainId = Convert.ToInt32(lst[16]),
                        ScanningNodeId = Convert.ToInt32(lst[9]),
                        BranchCode = lst[7].ToString(),
                        BatchNo = Convert.ToInt32(lst[3]),
                        InstrumentType = lst[5].ToString(),
                        ClearingType = lst[8].ToString(),
                        SlipNo = Convert.ToInt32(lst[6]),
                        FinalAmount = Convert.ToDecimal(lst[20]),
                        FinalDate = lst[21].ToString(),
                        ChequeNoFinal = lst[22].ToString(),
                        SortCodeFinal = lst[23].ToString(),
                        SANFinal = lst[24].ToString(),
                        TransCodeFinal = lst[25].ToString(),
                        Status = Convert.ToByte(lst[13]),
                        FrontGreyImagePath = img,
                        CBSAccountInformation = lst[18].ToString(),
                        CBSJointAccountInformation = lst[19].ToString(),
                        CreditAccountNo = lst[1].ToString(),
                        SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                        L1RejectReason = Convert.ToByte(lst[28].ToString()),
                        L1VerificationStatus = Convert.ToByte(lst[29].ToString()),
                        L2VerificationStatus = Convert.ToByte(lst[33].ToString()),
                        L2RejectReason = Convert.ToByte(lst[32].ToString()),
                        RejectReason = rejct,
                        ProcessingDate = Convert.ToDateTime(processingdate),
                        RawDataId = Convert.ToInt64(lst[14]),
                        PayeeName = lst[27].ToString(),
                        Action = lst[12].ToString(),
                        modified = Convert.ToBoolean(lst[30].ToString()),
                        AccModified = Convert.ToBoolean(lst[31].ToString()),
                        globalmodified = Convert.ToBoolean(lst[34].ToString()),
                        callby = callby,
                        UserNarration = lst[35].ToString(),
                        RejectReasonDescription = rejectreasondescrpsn,
                        ctsNonCtsMark = lst[37].ToString(),
                        P2fMark = Convert.ToBoolean(lst[38].ToString()),
                        SlipID = Convert.ToInt64(lst[39]),
                        SlipRawaDataID = Convert.ToInt64(lst[40]),
                        Modified3 = lst[41].ToString(),
                    };
                }
                ids.Add(def.Id);
                objectlst.Add(def);

                //------------------------END------------------------//
                for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    if (callby == "Cheq")
                    {
                        if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                            checkid = true;
                        // break;
                        else
                            checkid = false;
                    }

                    if (checkid == false)
                    {
                        def = new L3VerificationModel
                        {
                            Id = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]),
                            RawDataId = Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[1]),
                            ProcessingDate = Convert.ToDateTime(ds.Tables[0].Rows[index].ItemArray[2].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[3].ToString()),
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[4].ToString()),
                            ScanningNodeId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[5].ToString()),
                            BranchCode = ds.Tables[0].Rows[0].ItemArray[6].ToString(),
                            BatchNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[7].ToString()),
                            BatchSeqNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[8].ToString()),
                            InstrumentType = ds.Tables[0].Rows[index].ItemArray[9].ToString(),
                            ClearingType = ds.Tables[0].Rows[index].ItemArray[10].ToString(),
                            SlipNo = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[11].ToString()),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[12].ToString()),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[13].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[14].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[17].ToString(),
                            Status = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[18]),
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            FrontTiffImagePath = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            FrontGreyImagePath = ds.Tables[0].Rows[index].ItemArray[25].ToString(),
                            BackTiffImagePath = ds.Tables[0].Rows[index].ItemArray[26].ToString(),
                            SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[27].ToString()),
                            ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[28].ToString()),
                            L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[29].ToString()),
                            L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[30].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[33].ToString()),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                            Modified2 = ds.Tables[0].Rows[index].ItemArray[36].ToString(),
                            CBSAccountInformation = lst[18].ToString(),
                            CBSJointAccountInformation = lst[19].ToString(),
                            CreditAccountNo = lst[1].ToString(),
                            SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                            AccModified = Convert.ToBoolean(lst[31].ToString()),
                            globalmodified = Convert.ToBoolean(lst[34].ToString()),
                            callby = callby,
                            SlipID = Convert.ToInt64(lst[39]),
                            SlipRawaDataID = Convert.ToInt64(lst[40]),
                            Modified3 = lst[41].ToString(),

                        };
                        objectlst.Add(def);
                        ids.Add(def.Id);
                    }
                }
                getslip = false;
                //  }
            }
            else
            {
                //-----------------Update Slip------------------
                id = Convert.ToInt64(lst[0]);
                //-----------------Update Slip------------------
                byte acmodified = 0;
                if (Convert.ToBoolean(lst[31]) == true)//---------Ac modified
                    acmodified = 1;
                else if (Convert.ToBoolean(lst[34]) == true)//Global modified
                    acmodified = 2;
                else
                    acmodified = 0;
                if (lst[35] != null)
                    userNarration = lst[35].ToString();

                //---------------Added On 21/06/2017------------------
                if (lst[39] != null)
                    SlipID = Convert.ToInt64(lst[39]);
                if (lst[40] != null)
                    SlipRawaDataID = Convert.ToInt64(lst[40]);

                if (lst[41] != null)
                    finalModified = lst[41].ToString();

                OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                    Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L3", acmodified, tempclientcd, userNarration, null,
                    Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session, SlipID, SlipRawaDataID, null, 0, finalModified);
                getslip = true;
            }
        callslip:
            if (getslip == true)
            {
                adp = new SqlDataAdapter("OWSelectL3", con);
                adp.SelectCommand.CommandType = CommandType.StoredProcedure;
                adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
                adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = "Normal";
                adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
                //-------------Added on 17-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
                //-------------Added on 18-05-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);

                //-------------Added on 12-09-2017-----------------------------
                adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];

                ds = new DataSet();
                adp.Fill(ds);
                // var objectlst = new List<L1Verification>();
                // L1Verification def;

                if (ds.Tables[0].Rows.Count > 0)
                {

                    def = new L3VerificationModel
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
                        L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[24].ToString()),
                        L1UserId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[25].ToString()),
                        L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[26].ToString()),
                        PayeeName = ds.Tables[0].Rows[0].ItemArray[27].ToString(),
                        L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[28].ToString()),
                        L2UserId = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[29].ToString()),
                        L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[0].ItemArray[30].ToString()),
                        CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[31].ToString(),
                        CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[32].ToString(),
                        UserNarration = ds.Tables[0].Rows[0].ItemArray[33].ToString(),
                        RejectReasonDescription = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                        Modified2 = ds.Tables[0].Rows[0].ItemArray[35].ToString(),
                        callby = "Slip",

                        //CBSAccountInformation = ds.Tables[0].Rows[0].ItemArray[16].ToString(),
                        //CBSJointAccountInformation = ds.Tables[0].Rows[0].ItemArray[17].ToString(),
                    };
                    objectlst.Add(def);
                    //------------------------END------------------------//
                    int index = 0;
                    int count = ds.Tables[0].Rows.Count;
                    while (count > 0)
                    {
                        def = new L3VerificationModel
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
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[24].ToString()),
                            L1UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[25].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[28].ToString()),
                            L2UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[29].ToString()),
                            L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[30].ToString()),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[31].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            Modified2 = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                            callby = "Slip",
                            //CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[15].ToString(),
                            //CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[16].ToString(),
                        };
                        //ViewBag.cnt = true;
                        objectlst.Add(def);
                        count = count - 1;
                        index = index + 1;
                    }
                }

            }
            return (objectlst);
        }

        //-----------------------OW L1 Chq------------------------
        public List<L1verificationModel> selectL1ChequesOnly(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, string processingdate = null, string img = null, string callby = null, string tempclientcd = null, string CreditCardValidationReq = null, string CreditCardValidAcNo = null, string CreditCardInValidAcNo = null, int CustomerID = 0, int DomainID = 0, bool dirctslipcall = false, string CtsSessionType = null)
        {
            var objectlst = new List<L1verificationModel>();
            L1verificationModel def = new L1verificationModel();
            DataSet ds = new DataSet();
            OWProcDataContext OWpro = new OWProcDataContext();
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

            //if (dirctslipcall == true)
            //{
            //    getslip = true;
            //    goto callslip;
            //}

            ArrayList ids = new ArrayList();
            bool checkid = false;

            SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL1", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
            adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;
            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;//Convert.ToInt16(Session["CustomerID"]);
            //-------------Added on 18-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);
            //-------------Added on 12-09-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];


            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //--------------Checking Last record and list record are same------
                //if (callby != "Slip")
                //{
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
                        if (lst[11] != null && lst[11].ToString() != "")
                            tempclientcd = lst[11].ToString();

                        if (lst[28] != null)
                            userNarration = lst[28].ToString();

                        if (rejct == 88)
                        {
                            if (lst[31] != null)
                                rejectreasondescrpsn = lst[31].ToString();
                            else
                                rejectreasondescrpsn = "Other Reason";

                        }

                        if (lst[32] != null)
                            Clearingtype = lst[32].ToString();
                        if (lst[27] != null)
                            payeename = lst[27].ToString();
                        //---------------Added On 26/09/2017---
                        if (lst[35] != null)
                            Modified = lst[35].ToString();

                        OWpro.UpdateOWL1(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(),
                            lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(), payeename, Convert.ToInt16(lst[13]), rejct, lst[12].ToString(), Session, processingdate,
                            Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, tempclientcd, userNarration, rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), Modified);

                        //    //-----------------Update Slip------------------
                        //    id = Convert.ToInt64(lst[0]);
                        //    if (lst[30] != null)
                        //    {
                        //        if (lst[30].ToString().ToUpper() == "R")
                        //            L1dec = "L1R";
                        //        else
                        //            L1dec = "L1";
                        //    }
                        //    //---------------Added On 25/05/2017------------------
                        //    if (lst[33] != null)
                        //        SlipID = Convert.ToInt64(lst[33]);
                        //    if (lst[34] != null)
                        //        SlipRawaDataID = Convert.ToInt64(lst[34]);
                        goto end;

                    }

                }

                if (lst[15] != null && lst[15].ToString() != "")
                    rejct = Convert.ToByte(lst[15].ToString());
                if (rejct == 88)
                {
                    if (lst[31] != null)
                        rejectreasondescrpsn = lst[31].ToString();
                    else
                        rejectreasondescrpsn = "Other Reason";

                }

                def = new L1verificationModel
                {
                    Id = Convert.ToInt64(lst[0]),
                    CustomerId = Convert.ToInt16(lst[17]),
                    DomainId = Convert.ToInt32(lst[16]),
                    ScanningNodeId = Convert.ToInt32(lst[9]),
                    BranchCode = lst[7].ToString(),
                    BatchNo = Convert.ToInt32(lst[3]),
                    InstrumentType = lst[5].ToString(),
                    ClearingType = lst[8].ToString(),
                    SlipNo = 0,
                    FinalAmount = Convert.ToDecimal(lst[20]),
                    FinalDate = lst[21].ToString(),
                    ChequeNoFinal = lst[22].ToString(),
                    SortCodeFinal = lst[23].ToString(),
                    SANFinal = lst[24].ToString(),
                    TransCodeFinal = lst[25].ToString(),
                    Status = Convert.ToByte(lst[13]),
                    FrontTiffImagePath = img.Replace("jpg", "tif"),
                    FrontGreyImagePath = img,
                    //BackTiffImagePath = ds.Tables[0].Rows[0].ItemArray[20].ToString(),
                    CBSAccountInformation = lst[18].ToString(),
                    CBSJointAccountInformation = lst[19].ToString(),
                    //SlipChequeCount = Convert.ToInt32(ds.Tables[0].Rows[0].ItemArray[23].ToString()),
                    //ChequeAmountTotal = Convert.ToDecimal(ds.Tables[0].Rows[0].ItemArray[24].ToString()),
                    CreditAccountNo = lst[1].ToString(),
                    //SlipAmount = 0,
                    callby = "Cheq",
                    ClientCode = lst[11].ToString(),
                    ProcessingDate = Convert.ToDateTime(processingdate),
                    RawDataId = Convert.ToInt64(lst[14]),
                    PayeeName = lst[27].ToString(),
                    Action = lst[12].ToString(),
                    UserNarration = lst[28].ToString(),
                    // SlipUserNarration = lst[29].ToString(),
                    RejectReason = rejct,
                    RejectReasonDescription = rejectreasondescrpsn,
                    // Slipdecision = lst[30].ToString(),
                    ctsNonCtsMark = lst[32].ToString(),
                    Modified = lst[35].ToString(),
                    SlipID = 0,
                    SlipRawaDataID = 0,
                };

                ids.Add(def.Id);
                objectlst.Add(def);

                for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    //if (callby == "Cheq")
                    //{
                    if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                    {
                        checkid = true;
                        // break;
                    }
                    else
                    {
                        checkid = false;
                    }
                    // }

                    if (checkid == false)
                    {
                        if (lst[15] != null && lst[15].ToString() != "")
                            rejct = Convert.ToByte(lst[15].ToString());

                        def = new L1verificationModel
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
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[20].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[21].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[22].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[24].ToString(),
                            callby = "Chq",
                            //Slipdecision = lst[30].ToString(),
                            //SlipID = Convert.ToInt64(lst[33]),
                            //SlipRawaDataID = Convert.ToInt64(lst[34]),

                        };
                        objectlst.Add(def);
                        ids.Add(def.Id);
                    }
                }

                // }
            }
        end:
            return (objectlst);
        }
        //------------------OW L2 Chq--------------
        public List<L2verificationModel> selectL2ChequesOnly(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, string processingdate = null, string img = null, string callby = null, bool dirctslipcall = false, int CustomerID = 0, int DomainID = 0, string SlipOnlyAccept = null, double SlipOnlyAcceptAmtThreshold = 0, string StrVFType = null, string CtsSessionType = null, string VFType = null)
        {

            var objectlst = new List<L2verificationModel>();
            L2verificationModel def;
            DataSet ds = new DataSet();
            OWProcDataContext OWpro = new OWProcDataContext();
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

                        OWpro.UpdateOWL2(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, Session, processingdate,
                            Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration, rejectreasondescrpsn, Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, finalmodified);

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

                def = new L2verificationModel
                {
                    Id = Convert.ToInt64(lst[0]),
                    CustomerId = Convert.ToInt16(lst[17]),
                    DomainId = Convert.ToInt32(lst[16]),
                    ScanningNodeId = Convert.ToInt32(lst[9]),
                    BranchCode = lst[7].ToString(),
                    BatchNo = Convert.ToInt32(lst[3]),
                    InstrumentType = lst[5].ToString(),
                    ClearingType = lst[8].ToString(),
                    SlipNo = 0,
                    FinalAmount = Convert.ToDecimal(lst[20]),
                    FinalDate = lst[21].ToString(),
                    ChequeNoFinal = lst[22].ToString(),
                    SortCodeFinal = lst[23].ToString(),
                    SANFinal = lst[24].ToString(),
                    TransCodeFinal = lst[25].ToString(),
                    Status = Convert.ToByte(lst[13]),
                    FrontGreyImagePath = img,
                    CBSAccountInformation = lst[18].ToString(),
                    CBSJointAccountInformation = lst[19].ToString(),
                    CreditAccountNo = lst[1].ToString(),
                    // SlipAmount = Convert.ToDecimal(lst[2].ToString()),
                    L1RejectReason = Convert.ToByte(lst[28].ToString()),
                    RejectReason = rejct,
                    L1VerificationStatus = Convert.ToByte(lst[29].ToString()),
                    ProcessingDate = Convert.ToDateTime(processingdate),
                    RawDataId = Convert.ToInt64(lst[14]),
                    PayeeName = lst[27].ToString(),
                    Action = lst[12].ToString(),
                    callby = callby,
                    ClientCode = lst[11].ToString(),
                    AccModified = Convert.ToBoolean(lst[31].ToString()),
                    UserNarration = lst[32].ToString(),
                    RejectReasonDescription = rejectreasondescrpsn,
                    ctsNonCtsMark = lst[34].ToString(),
                    P2fMark = Convert.ToBoolean(lst[35].ToString()),
                    Modified2 = lst[39].ToString(),
                    SlipID = 0,
                    SlipRawaDataID = 0,
                    // SlipUserNarration = lst[33].ToString(),
                };
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
                            DocType = ds.Tables[0].Rows[0].ItemArray[34].ToString(),
                            Modified1 = ds.Tables[0].Rows[0].ItemArray[35].ToString(),
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

        //------------------L3 verification---------------------------------//
        public List<L3VerificationModel> selectL3ChequesOnly(SqlConnection con, int uid = 0, string Session = null, List<string> lst = null, string processingdate = null, string img = null, string callby = null, bool dirctslipcall = false, int CustomerID = 0, int DomainID = 0, string CtsSessionType = null, string VFType = null)
        {

            var objectlst = new List<L3VerificationModel>();
            L3VerificationModel def;
            DataSet ds = new DataSet();
            OWProcDataContext OWpro = new OWProcDataContext();
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
            bool mark2pf = false;
            bool ignoreIQA = false;
            string DocType = "B";
            Int64 SlipID = 0;
            Int64 SlipRawaDataID = 0;

            if (dirctslipcall == true)
            {
                getslip = true;
                goto callslip;
            }

            SqlDataAdapter adp = new SqlDataAdapter("OWSelectCHQL3", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;
            adp.SelectCommand.Parameters.Add("@uid", SqlDbType.NVarChar).Value = uid;
            adp.SelectCommand.Parameters.Add("@VFtype", SqlDbType.NVarChar).Value = VFType;
            adp.SelectCommand.Parameters.Add("@ProcessingDate", SqlDbType.NVarChar).Value = processingdate;

            //adp.SelectCommand.Parameters.Add("@BatchNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[3].ToString());
            //adp.SelectCommand.Parameters.Add("@SlipNo", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[6].ToString());
            //adp.SelectCommand.Parameters.Add("@ScanningNodeId", SqlDbType.NVarChar).Value = Convert.ToInt32(lst[9].ToString());
            //adp.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.NVarChar).Value = lst[7].ToString();
            //-------------Added on 17-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CustomerID", SqlDbType.NVarChar).Value = CustomerID;
            //-------------Added on 18-05-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@DomainId", SqlDbType.NVarChar).Value = DomainID;//Convert.ToInt32(Session["DomainselectID"]);

            //-------------Added on 12-09-2017-----------------------------
            adp.SelectCommand.Parameters.Add("@CtsSessionType", SqlDbType.NVarChar).Value = CtsSessionType; //Session["CtsSessionType"];


            adp.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                //--------------Checking Last record and list record are same------
                //if (callby != "Slip")
                //{
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
                                if (lst[36] != null)
                                    rejectreasondescrpsn = lst[36].ToString();
                                else
                                    rejectreasondescrpsn = "Other Reason";
                            }
                        }

                        if (lst[11] != null)
                            tempclientcd = lst[11].ToString();
                        if (lst[35] != null)
                            userNarration = lst[35].ToString();

                        if (lst[37] != null)
                            Clearingtype = lst[37].ToString();

                        //------------------marking P2F--------------------//
                        if (lst[38] != null)
                        {
                            mark2pf = Convert.ToBoolean(lst[38]);
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


                        OWpro.UpdateOWL3(id, Convert.ToInt64(lst[14]), uid, lst[5].ToString(), Convert.ToDouble(lst[20].ToString()), finaldate, lst[22].ToString(), lst[23].ToString(), lst[24].ToString(), lst[25].ToString(), lst[1].ToString(),
                            lst[27].ToString(), Convert.ToInt16(lst[13]), rejct, modaction, Session, processingdate,
                            Convert.ToInt16(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Convert.ToInt32(lst[9].ToString()), null, 0, null, userNarration, rejectreasondescrpsn,
                            Clearingtype, lst[18].ToString(), lst[19].ToString(), ignoreIQA, DocType, null);

                        //-----------------Update Slip------------------
                        byte acmodified = 0;

                        if (Convert.ToBoolean(lst[31]) == true)
                            acmodified = 1;
                        else if (Convert.ToBoolean(lst[34]) == true)
                            acmodified = 2;
                        else
                            acmodified = 0;
                        //---------------Added On 21/06/2017------------------
                        if (lst[39] != null)
                            SlipID = Convert.ToInt64(lst[39]);
                        if (lst[40] != null)
                            SlipRawaDataID = Convert.ToInt64(lst[40]);

                        //id = Convert.ToInt64(lst[0]);
                        //OWpro.UpdateVerificationSlipOnly(Convert.ToInt64(lst[14]), uid, processingdate, Convert.ToInt32(lst[3].ToString()), Convert.ToInt32(lst[6].ToString()), Convert.ToInt32(lst[9].ToString()), lst[7].ToString(),
                        //    Convert.ToDouble(lst[2].ToString()), lst[1].ToString(), lst[27].ToString(), lst[18].ToString(), lst[19].ToString(), "L3", acmodified, tempclientcd, userNarration, null,
                        //    Convert.ToInt32(lst[17].ToString()), Convert.ToInt32(lst[16].ToString()), Session, SlipID, SlipRawaDataID, null, 0);

                        getslip = true;
                        goto callslip;
                    }

                }
                //  }
                //else
                //{


                if (lst[15] != null && lst[15].ToString() != "")
                    rejct = Convert.ToByte(lst[15].ToString());
                if (rejct == 88)
                {
                    if (lst[36] != null && lst[36].ToString() != "")
                        rejectreasondescrpsn = lst[36].ToString();
                    else
                        rejectreasondescrpsn = "Other Reason";
                }

                def = new L3VerificationModel
                {
                    Id = Convert.ToInt64(lst[0]),
                    CustomerId = Convert.ToInt16(lst[17]),
                    DomainId = Convert.ToInt32(lst[16]),
                    ScanningNodeId = Convert.ToInt32(lst[9]),
                    BranchCode = lst[7].ToString(),
                    BatchNo = Convert.ToInt32(lst[3]),
                    InstrumentType = lst[5].ToString(),
                    ClearingType = lst[8].ToString(),
                    SlipNo = 0,
                    FinalAmount = Convert.ToDecimal(lst[20]),
                    FinalDate = lst[21].ToString(),
                    ChequeNoFinal = lst[22].ToString(),
                    SortCodeFinal = lst[23].ToString(),
                    SANFinal = lst[24].ToString(),
                    TransCodeFinal = lst[25].ToString(),
                    Status = Convert.ToByte(lst[13]),
                    FrontGreyImagePath = img,
                    CBSAccountInformation = lst[18].ToString(),
                    CBSJointAccountInformation = lst[19].ToString(),
                    CreditAccountNo = lst[1].ToString(),
                    SlipAmount = 0,
                    L1RejectReason = Convert.ToByte(lst[28].ToString()),
                    L1VerificationStatus = Convert.ToByte(lst[29].ToString()),
                    L2VerificationStatus = Convert.ToByte(lst[33].ToString()),
                    L2RejectReason = Convert.ToByte(lst[32].ToString()),
                    RejectReason = rejct,
                    ProcessingDate = Convert.ToDateTime(processingdate),
                    RawDataId = Convert.ToInt64(lst[14]),
                    PayeeName = lst[27].ToString(),
                    Action = lst[12].ToString(),
                    modified = Convert.ToBoolean(lst[30].ToString()),
                    AccModified = Convert.ToBoolean(lst[31].ToString()),
                    globalmodified = Convert.ToBoolean(lst[34].ToString()),
                    callby = callby,
                    UserNarration = lst[35].ToString(),
                    RejectReasonDescription = rejectreasondescrpsn,
                    ctsNonCtsMark = lst[37].ToString(),
                    P2fMark = Convert.ToBoolean(lst[38].ToString()),
                    Modified3 = lst[41].ToString(),
                    SlipID = 0,
                    SlipRawaDataID = 0,
                };
                // }
                ids.Add(def.Id);
                objectlst.Add(def);

                //------------------------END------------------------//
                for (int index = 0; index < ds.Tables[0].Rows.Count; index++)
                {
                    //if (callby == "Cheq")
                    //{
                    if (Convert.ToInt64(ids[0]) == Convert.ToInt64(ds.Tables[0].Rows[index].ItemArray[0]))
                        checkid = true;
                    // break;
                    else
                        checkid = false;
                    //}

                    if (checkid == false)
                    {
                        def = new L3VerificationModel
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
                            DomainId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[17].ToString()),
                            CustomerId = Convert.ToInt16(ds.Tables[0].Rows[index].ItemArray[18].ToString()),
                            ScanningType = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[19].ToString()),
                            L1VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[20].ToString()),
                            L1UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[21].ToString()),
                            L1RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[22].ToString()),
                            PayeeName = ds.Tables[0].Rows[index].ItemArray[23].ToString(),
                            L2VerificationStatus = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[24].ToString()),
                            L2UserId = Convert.ToInt32(ds.Tables[0].Rows[index].ItemArray[25].ToString()),
                            L2RejectReason = Convert.ToByte(ds.Tables[0].Rows[index].ItemArray[26].ToString()),
                            CBSAccountInformation = ds.Tables[0].Rows[index].ItemArray[27].ToString(),
                            CBSJointAccountInformation = ds.Tables[0].Rows[index].ItemArray[28].ToString(),
                            UserNarration = ds.Tables[0].Rows[index].ItemArray[29].ToString(),
                            RejectReasonDescription = ds.Tables[0].Rows[index].ItemArray[30].ToString(),
                            FinalAmount = Convert.ToDecimal(ds.Tables[0].Rows[index].ItemArray[31]),
                            FinalDate = ds.Tables[0].Rows[index].ItemArray[32].ToString(),
                            ChequeNoFinal = ds.Tables[0].Rows[index].ItemArray[33].ToString(),
                            SortCodeFinal = ds.Tables[0].Rows[index].ItemArray[34].ToString(),
                            SANFinal = ds.Tables[0].Rows[index].ItemArray[35].ToString(),
                            TransCodeFinal = ds.Tables[0].Rows[index].ItemArray[36].ToString(),
                            DocType = ds.Tables[0].Rows[index].ItemArray[37].ToString(),
                            Modified2 = ds.Tables[0].Rows[index].ItemArray[38].ToString(),
                            callby = "Cheq",

                        };
                        objectlst.Add(def);
                        ids.Add(def.Id);
                    }
                }
                getslip = false;
                //  }
            }

        callslip:
            return (objectlst);
        }

    }
}