var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var nextcall;
var L3;
var cnt = 0;
var tempcnt;
var chq;
var srtcd;
var sancd;
var trcd;
var next_idx = 0;
var tot_idx = 0;
var cbsbtn = false;
var btnvalacpt = false;
var idslst = [];
var ChequeAmountTotal = 0;
var tempAmtValue = 0;
var firstcall = false;
var randomPayeeName = "";
var realmodified = false;
var realAccModified = false;
var globalmodified = false;
var okcall = false;
var narrationReqirdflg = false;
var tempnarration;
var InstrumentType;
var SlipID;
var SlipRawaDataID;
var strModified = "0000000000";
var Foutcnt = 0;
//var scondbck = false;
function passval(array) {
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}

var getreason;
var getcbsdtls;
var clientdtls;
var getIwlogs;

var tempdat;
var fnldate;
var yr, mm, dd;

$(document).ready(function () {

    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        //alert('for ' + tt[z].Id);
        idslst.push(tt[z].Id)
    }
    // alert(idslst.length);
    //------------ idslist end----------------//
    $('#accnt,#ClientCd,#slpamount,#Amt,#ChqDate,#ChqnoQC,#SortQC,#SANQC,#TransQC,#IWRemark,#nartext').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    //-----------------ShortCut----for CBS---
    $("#accnt").keydown(function (event) {
        //   alert('aila');
        if (event.keyCode == 123) {
            getcbsdtls(); //CbsDetails
            return false;
        }
    });


    if (tt.length > 0) {

        debugger;

        document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

        document.getElementById('ChqCnt').innerHTML = tt[1].SlipChequeCount;
        document.getElementById('totamt').innerHTML = tt[1].ChequeAmountTotal;

        InstrumentType = tt[1].InstrumentType;

        //-------Remove save objects from browser---//
        window.localStorage.clear();
        //------------ idslist end----------------//

        if (tt[1].InstrumentType == "S") {

            if (tt[1].ScanningType == 3 || tt[1].ScanningType == 5) {

                $("#ClientCd").prop('disabled', true);

                document.getElementById('ClntsDtlsdiv').style.display = "";
                //document.getElementById('Chqacnt').style.display = "";
                document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;
                document.getElementById('ClientCd').value = tt[1].ClientCode;
                $.ajax({
                    url: RootUrl + 'OWL3/GetCBSDtls',
                    dataType: 'html',
                    data: { ac: tt[1].CreditAccountNo, callby: "ClientCode" },
                    success: function (data) {
                        $('#cbsdetails').html(data);
                        cbsbtn = true;
                    }
                });

                clientdtls();
                //if ($("#ClientCd").val() != "") {
                //    // alert('call');
                //    $.ajax({
                //        url: RootUrl + 'OWL2/GetClientDlts',
                //        dataType: 'html',
                //        data: { ac: $("#ClientCd").val() },
                //        success: function (data) {
                //            $('#clientdetails').html(data);
                //            cbsbtn = true;
                //        }
                //    });
                //    //clientdtls();
                //}
                document.getElementById('slpacnt').style.display = "";
                document.getElementById('accnt').value = tt[1].CreditAccountNo;
                document.getElementById('Decision').focus();
            }
            else {

                document.getElementById('slpacnt').style.display = "";
                document.getElementById('accnt').focus();
                document.getElementById('Chqacnt').style.display = "none";
                document.getElementById('accnt').value = tt[1].CreditAccountNo;
                document.getElementById('Decision').focus();

                //--------------------Added On 07-02-2017------------------
                document.getElementById('oldact').value = tt[1].CreditAccountNo;
                if ($("#NarrationID").val() == "Y") {
                    document.getElementById('nartext').value = tt[1].UserNarration;
                    $('#nartext').attr('readonly', false);
                }

                $.ajax({
                    url: RootUrl + 'OWL3/GetCBSDtls',
                    dataType: 'html',
                    data: { ac: $("#accnt").val(), strcbsdls: tt[1].CBSAccountInformation, strJoinHldrs: tt[1].CBSJointAccountInformation, callby: "Normal", payeename: tt[1].PayeeName },
                    success: function (data) {
                        cbsbtn = true;
                        $('#cbsdetails').html(data);

                    }
                });

            }
            //    debugger;

            ////---------------------Account---------Checking-----------------
            //narrationReqirdflg = false;
            //if (tt[1].CreditAccountNo.length == 14) {
            //    if (tt[1].CreditAccountNo == "06410125027255") {
            //        document.getElementById('narsndiv').style.display = "";
            //        document.getElementById('nartext').value = tt[1].UserNarration;
            //        tempnarration == tt[1].UserNarration;
            //        narrationReqirdflg = true;
            //    }
            //    else {
            //        tempnarration == tt[1].UserNarration;
            //        narrationAC(tt[1].CreditAccountNo);
            //    }

            //}

            //--------------------------------------------------
            document.getElementById('slpamt').style.display = "";
            document.getElementById('chqamt').style.display = "none";

            document.getElementById('MICR').style.display = "none";
            document.getElementById('slpamount').value = addCommas(Number(tt[1].SlipAmount).toFixed(2));
            //alert($("#PayeeName").prop("disabled", true));
            document.getElementById('sliplabl').style.display = "none";
            document.getElementById('divctsnoncts').style.display = "none";
            document.getElementById('divmarkp2f').style.display = "none";
            document.getElementById('bankname').style.display = "none";
            document.getElementById('lblslpimg').style.display = "none";

            //--------------------IF Account number and amount Modified --------------------------
            //-------------Account---------------
            if (tt[1].Modified2.charAt(0) == "1") {
                document.getElementById("accnt").style.backgroundColor = "red";
            }
            else {
                document.getElementById("accnt").style.backgroundColor = "white";
            }
            //-------------Amount---------------
            if (tt[1].Modified2.charAt(1) == "1") {
                document.getElementById("slpamount").style.backgroundColor = "red";
            }
            else {
                document.getElementById("slpamount").style.backgroundColor = "white";
            }

        }
        else {

            document.getElementById('slpamt').style.display = "none";
            document.getElementById('chqamt').style.display = "";
            document.getElementById('Chqacnt').style.display = "";
            document.getElementById('slpacnt').style.display = "none";
            document.getElementById('MICR').style.display = "";
            document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;

            document.getElementById('ChqnoQC').value = tt[1].ChequeNoFinal;
            document.getElementById('SortQC').value = tt[1].SortCodeFinal;
            document.getElementById('SANQC').value = tt[1].SANFinal;
            document.getElementById('TransQC').value = tt[1].TransCodeFinal;
            document.getElementById('Slipamt').innerHTML = tt[1].SlipAmount;
            document.getElementById('sliplabl').style.display = "";

            if ($("#NarrationID").val() == "Y") {
                document.getElementById('nartext').value = tt[1].UserNarration;
            }
        }

        document.getElementById("btnback").disabled = true
        document.getElementById('rejectreasondescrpsn').value = "";


        document.getElementById('mtrn').value = tt[1].RawDataId;
        //---------------Setting focus to textbox----------------//
        // debugger;
        //----------------------Set L1 and L2 Decision Color ----------------
        document.getElementById("l1decision").innerHTML = "";
        document.getElementById("l2decision").innerHTML = "";
        document.getElementById("L1rejectDecrp").innerHTML = "";
        document.getElementById("L2rejectDecrp").innerHTML = "";
        if (tt[1].L1VerificationStatus == 2) {

            document.getElementById("l1decision").innerHTML = "Y";
            //document.getElementById('l1decision').style.background = "Green";
            document.getElementById("l1decision").classList.add("w3-text-green");
            document.getElementById('L1rejectDecrp').style.display = "none";

        }
        else if (tt[1].L1VerificationStatus == 3) {
            document.getElementById("l1decision").innerHTML = "R";
            document.getElementById("l1decision").classList.add("w3-text-red");
            document.getElementById("L1rejectDecrp").innerHTML = "";
            // getL1Logs(tt[1].RawDataId);
            getReturnDecrp(tt[1].L1RejectReason, "L1");
        }
        if (tt[1].L2VerificationStatus == 2) {

            document.getElementById("l2decision").innerHTML = "Y";
            // document.getElementById('l2decision').style.background = "Green";
            document.getElementById("l2decision").classList.add("w3-text-green");
            document.getElementById('L2rejectDecrp').style.display = "none";
            document.getElementById('L2rejectDecrp').style.display = "none";
        }
        else if (tt[1].L2VerificationStatus == 3 || tt[1].L2VerificationStatus == 8) {
            if (tt[1].L2VerificationStatus == 3) {
                document.getElementById("l2decision").innerHTML = "R";
                document.getElementById("l2decision").classList.add("w3-text-red");
                getReturnDecrp(tt[1].L2RejectReason, "L2");
            }
            else {
                document.getElementById("l2decision").innerHTML = "M";
                document.getElementById("l2decision").classList.add("w3-text-orange");
                document.getElementById("L2rejectDecrp").innerHTML = "";
                document.getElementById('L2rejectDecrp').style.display = "none";
            }
        }
        //--------------------------------------------------------------
        document.getElementById('strbranchcd').innerHTML = tt[1].BranchCode;
        document.getElementById('ScanningID').innerHTML = tt[1].ScanningNodeId;
        document.getElementById('strBatchNo').innerHTML = tt[1].BatchNo;
        document.getElementById('strBatchSeqNO').innerHTML = tt[1].BatchSeqNo;
        //--------------------------------------------------------------
    }

    $("#ok").click(function () {
        // alert(randomPayeeName);
        nextcall = false;
        debugger;
        var result = IWLICQC()
        if (result == false) {

            return false;
        }
        else {
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;

            if (tt[cnt].callby != "Slip") {
                document.getElementById("btnback").disabled = true;//---Prev was false
            }



            var owL3 = "owL3";
            var btnval = document.getElementById('Decision').value.toUpperCase();
            if (btnval == "F" && tt[cnt].Status == 6) {
                alert('Already referred!!');
                nextcall = false;
            }
            else {

                if (backbtn == true) {
                    if (btnval == "R") {
                        cnrslt = false;
                        //-----valid Function for validation---------------
                        var rslt = valid(document.getElementById("rejectreasondescrpsn").value, document.getElementById('IWRemark').value);
                        if (rslt == false) {
                            //alert('Please select reject reason!!');
                            document.getElementById('rejectreasondescrpsn').focus();
                            nextcall = false;
                            return false;
                        }
                        else {
                            nextcall = true;
                        }
                    }
                    else {


                        cnrslt = true;
                        nextcall = true;
                    }

                    if (InstrumentType == "S") {
                        SlipID = tt[backcnt].Id;
                        SlipRawaDataID = tt[backcnt].RawDataId;
                    }

                    owL3 = owL3 + backcnt
                    L3 = {
                        "CreditAccountNo": $("#accnt").val(),
                        "SlipAmount": parseFloat($("#slpamount").val().replace(/,/g, '')),
                        "BatchNo": tt[backcnt].BatchNo,
                        "ClearingType": tt[backcnt].ClearingType,
                        "InstrumentType": tt[backcnt].InstrumentType,
                        "SlipNo": tt[backcnt].SlipNo,
                        "ProcessingDate": tt[backcnt].ProcessingDate,
                        "BranchCode": tt[backcnt].BranchCode,
                        "ScanningNodeId": tt[backcnt].ScanningNodeId,
                        "ClientCode": tt[backcnt].ClientCode,
                        "SlipRefNo": tt[backcnt].SlipRefNo,
                        "Id": tt[backcnt].Id,
                        "Action": btnval,
                        "Status": tt[backcnt].Status,
                        "RawDataId": tt[backcnt].RawDataId,
                        "RejectReason": $("#IWRemark").val(),
                        "DomainId": tt[backcnt].DomainId,
                        "CustomerId": tt[backcnt].CustomerId,
                        "CBSAccountInformation": $("#cbsdls").val(),
                        "CBSJointAccountInformation": $("#JoinHldrs").val(),
                        "FinalAmount": $("#Amt").val(),
                        "FinalDate": $("#ChqDate").val(),
                        "ChequeNoFinal": $("#ChqnoQC").val(),
                        "SortCodeFinal": $("#SortQC").val(),
                        "SANFinal": $("#SANQC").val(),
                        "TransCodeFinal": $("#TransQC").val(),
                        "ChequeAmountTotal": tt[backcnt].ChequeAmountTotal,
                        "PayeeName": randomPayeeName,//  $("#Payee").val(),
                        "L1RejectReason": tt[backcnt].L1RejectReason,
                        "L1VerificationStatus": tt[backcnt].L1VerificationStatus,
                        "modified": realmodified,
                        "AccModified": realAccModified,
                        "L2RejectReason": tt[backcnt].L2RejectReason,
                        "L2VerificationStatus": tt[backcnt].L2VerificationStatus,
                        "globalmodified": globalmodified,
                        "UserNarration": $("#nartext").val(),
                        "rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                        "ctsNonCtsMark": $("#ctsnocts").val(),
                        "P2fMark": $('#markp2f').is(":checked"),
                        "SlipID": SlipID,
                        "SlipRawaDataID": SlipRawaDataID,
                        "Modified3": strModified,
                    };
                }

                else {
                    if (btnval == "R") {
                        cnrslt = false;
                        //-----valid Function for validation---------------
                        var rslt = valid(document.getElementById("rejectreasondescrpsn").value, document.getElementById('IWRemark').value);
                        if (rslt == false) {
                            //alert('Please select reject reason!!');
                            document.getElementById('rejectreasondescrpsn').focus();
                            //  document.getElementById('RejectReason').style.display = 'block';
                            nextcall = false;
                            return false;
                        }
                        else {
                            nextcall = true;
                        }
                    }
                    else {
                        //alert('else');
                        //var tot1 = MatchWith(tempcnt);
                        //var totamt1 = MatchAmt(tempcnt);
                        //if (tot1 == 0) {
                        //    cnrslt = confirm("Entered MICR value is not maching with XML MICR\n are sure to accept with this value?");
                        //    if (cnrslt == false) {
                        //        document.getElementById('ChqnoQC').focus();
                        //        document.getElementById('ChqnoQC').select();
                        //    }
                        //    else {
                        //        nextcall = true;
                        //    }
                        //}
                        //else if (totamt1 == 0) {
                        //    cnrslt = confirm("Entered amount is not maching with XML amount\n are sure to accept with this value?");
                        //    if (cnrslt == false) {
                        //        document.getElementById('iwAmt').focus();
                        //        document.getElementById('iwAmt').select();
                        //    }
                        //    else {
                        //        nextcall = true;
                        //    }
                        //}
                        //    else {
                        //        cnrslt = true;
                        //        nextcall = true;
                        //}
                        cnrslt = true;
                        nextcall = true;
                    }

                    if (InstrumentType == "S") {
                        SlipID = tt[tempcnt].Id;
                        SlipRawaDataID = tt[tempcnt].RawDataId;
                    }

                    owL3 = owL3 + cnt;
                    L3 = {
                        "CreditAccountNo": $("#accnt").val(),
                        "SlipAmount": $("#slpamount").val(),
                        "BatchNo": tt[tempcnt].BatchNo,
                        "ClearingType": tt[tempcnt].ClearingType,
                        "InstrumentType": tt[tempcnt].InstrumentType,
                        "SlipNo": tt[tempcnt].SlipNo,
                        "ProcessingDate": tt[tempcnt].ProcessingDate,
                        "BranchCode": tt[tempcnt].BranchCode,
                        "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                        "ClientCode": tt[tempcnt].ClientCode,
                        "SlipRefNo": tt[tempcnt].SlipRefNo,
                        "Id": tt[tempcnt].Id,
                        "Action": btnval,
                        "Status": tt[tempcnt].Status,
                        "RawDataId": tt[tempcnt].RawDataId,
                        "RejectReason": $("#IWRemark").val(),
                        "DomainId": tt[tempcnt].DomainId,
                        "CustomerId": tt[tempcnt].CustomerId,
                        "CBSAccountInformation": $("#cbsdls").val(),
                        "CBSJointAccountInformation": $("#JoinHldrs").val(),
                        "FinalAmount": $("#Amt").val(),
                        "FinalDate": $("#ChqDate").val(),
                        "ChequeNoFinal": $("#ChqnoQC").val(),
                        "SortCodeFinal": $("#SortQC").val(),
                        "SANFinal": $("#SANQC").val(),
                        "TransCodeFinal": $("#TransQC").val(),
                        "ChequeAmountTotal": tt[tempcnt].ChequeAmountTotal,
                        "PayeeName": randomPayeeName,// $("#Payee").val() ,
                        "L1RejectReason": tt[tempcnt].L1RejectReason,
                        "L1VerificationStatus": tt[tempcnt].L1VerificationStatus,
                        "modified": realmodified,
                        "AccModified": realAccModified,
                        "L2RejectReason": tt[tempcnt].L2RejectReason,
                        "L2VerificationStatus": tt[tempcnt].L2VerificationStatus,
                        "globalmodified": globalmodified,
                        "UserNarration": $("#nartext").val(),
                        "rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                        "ctsNonCtsMark": $("#ctsnocts").val(),
                        "P2fMark": $('#markp2f').is(":checked"),
                        "SlipID": SlipID,
                        "SlipRawaDataID": SlipRawaDataID,
                        "Modified3": strModified,
                    };


                }
            }

            if (nextcall == true) {
                // alert('comn');
                common(owL3);
            }
            else {
                // alert('Okk');
                document.getElementById('accnt').focus();
                document.getElementById("btnback").disabled = true;
            }


        }
    });

    //-------------------------------------Reject--------------------------------//\


    $("#btnRejct").click(function () {


        document.getElementById("btnback").disabled = true;//Prev false
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var owL3 = "owL3";

        if (backbtn == true) {

            owL3 = owL3 + backcnt
            L3 = {
                "DbtAccountNo": $("#accnt").val(),
                "ActualAmount": $("#iwAmt").val(),
                "Date": $("#ChqDate").val(),
                "EntrySerialNo": $("#ChqnoQC").val(),
                "EntryPayorBankRoutNo": $("#SortQC").val(),
                "EntrySAN": $("#SANQC").val(),
                "EntryTransCode": $("#TransQC").val(),
                "ID": tt[backcnt].ID,
                "sttsdtqc": cnrslt,
                "XMLAmount": tt[backcnt].XMLAmount,
                "XMLSerialNo": tt[backcnt].XMLSerialNo,
                "XMLPayorBankRoutNo": tt[backcnt].XMLPayorBankRoutNo,
                "XMLSAN": tt[backcnt].XMLSAN,
                "XMLTransCode": tt[backcnt].XMLTransCode,
                "CBSAccountInformation": tt[backcnt].CBSAccountInformation,
                "CBSJointAccountInformation": tt[backcnt].CBSJointHoldersName,
            };
        }

        else {

            owL3 = owL3 + cnt;
            L3 = {
                "DbtAccountNo": $("#accnt").val(),
                "ActualAmount": $("#iwAmt").val(),
                "Date": $("#ChqDate").val(),
                "EntrySerialNo": $("#ChqnoQC").val(),
                "EntryPayorBankRoutNo": $("#SortQC").val(),
                "EntrySAN": $("#SANQC").val(),
                "EntryTransCode": $("#TransQC").val(),
                "ID": tt[tempcnt].ID,
                "sttsdtqc": cnrslt,
                "XMLAmount": tt[tempcnt].XMLAmount,
                "XMLSerialNo": tt[tempcnt].XMLSerialNo,
                "XMLPayorBankRoutNo": tt[tempcnt].XMLPayorBankRoutNo,
                "XMLSAN": tt[tempcnt].XMLSAN,
                "XMLTransCode": tt[tempcnt].XMLTransCode,
            };

        }
        common(owL3);
        backbtn = false;
    });
    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;

        if (Modernizr.localstorage) {

            backbtn = true;
            var owL3 = "owL3"
            cnt = document.getElementById('cnt').value;
            owL3 = owL3 + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(owL3));

            document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
            document.getElementById('slpamt').style.display = "none";
            document.getElementById('chqamt').style.display = "";
            document.getElementById('Chqacnt').style.display = "";
            document.getElementById('chequeAcct').innerHTML = "";
            document.getElementById('slpacnt').style.display = "none";
            document.getElementById('MICR').style.display = "";
            document.getElementById('ChqDate').value = "";
            document.getElementById('Amt').value = "";
            document.getElementById('ChqnoQC').value = "";
            document.getElementById('SortQC').value = "";
            document.getElementById('SANQC').value = "";
            document.getElementById('TransQC').value = "";
            //document.getElementById('accnt').value = orderData.DbtAccountNo
            //document.getElementById('ChqDate').value = orderData.Date
            //document.getElementById('iwAmt').value = orderData.ActualAmount
            //document.getElementById('ChqnoQC').value = orderData.EntrySerialNo;
            //document.getElementById('SortQC').value = orderData.EntryPayorBankRoutNo;
            //document.getElementById('SANQC').value = orderData.EntrySAN;
            //document.getElementById('TransQC').value = orderData.EntryTransCode;
            //document.getElementById('accnt').focus();
            document.getElementById('chequeAcct').innerHTML = orderData.CreditAccountNo;
            document.getElementById('Amt').value = orderData.FinalAmount;
            document.getElementById('ChqDate').value = orderData.FinalDate;
            document.getElementById('ChqnoQC').value = orderData.ChequeNoFinal;
            document.getElementById('SortQC').value = orderData.SortCodeFinal;
            document.getElementById('SANQC').value = orderData.SANFinal;
            document.getElementById('TransQC').value = orderData.TransCodeFinal;
            document.getElementById('Amt').focus();
            //--------------------------------------
            document.getElementById('ChqCnt').innerHTML = tt[1].SlipChequeCount;
            document.getElementById('totamt').innerHTML = ChequeAmountTotal;

            //if (tt[1].InstrumentType == "S") {
            //    document.getElementById('slpamt').style.display = "";
            //    document.getElementById('accnt').value = "";
            //    document.getElementById('slpamount').value = ""
            //    document.getElementById('chqamt').style.display = "none";
            //    document.getElementById('slpacnt').style.display = "";
            //    document.getElementById('Chqacnt').style.display = "none";
            //    document.getElementById('MICR').style.display = "none";
            //    document.getElementById('slpamount').value = tt[1].SlipAmount;
            //    document.getElementById('accnt').focus();

            //}
            //else {




            ////---------------------Filling cbsDetails------
            //$.ajax({
            //    url: '/owL3/GetCBSDtls',
            //    dataType: 'html',
            //    data: { strcbsdls: orderData.CBSAccountInformation, strJoinHldrs: orderData.CBSJointAccountInformation },
            //    success: function (data) {
            //        $('#cbsdetails').html(data);
            //        cbsbtn = true;
            //    }
            //});
            //   }
        }
    });
    //--------------Reject---------------------------------------
    $("#btnClose").click(function () {

        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;

            if (scond == true) {
                var i;
                if (tt[0].callby == "Cheq") {
                    i = 0;
                }
                else {
                    i = 1;
                }
                for (i; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL3" + i));

                    if (orderData.Id != null) {
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.CreditAccountNo);
                        arrlist.push(orderData.SlipAmount);
                        arrlist.push(orderData.BatchNo);
                        arrlist.push(orderData.ProcessingDate);
                        arrlist.push(orderData.InstrumentType);
                        arrlist.push(orderData.SlipNo);
                        arrlist.push(orderData.BranchCode);
                        arrlist.push(orderData.ClearingType);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.SlipRefNo);
                        arrlist.push(orderData.ClientCode);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.CBSAccountInformation);
                        arrlist.push(orderData.CBSJointAccountInformation);
                        arrlist.push(orderData.FinalAmount);
                        arrlist.push(orderData.FinalDate);
                        arrlist.push(orderData.ChequeNoFinal);
                        arrlist.push(orderData.SortCodeFinal);
                        arrlist.push(orderData.SANFinal);
                        arrlist.push(orderData.TransCodeFinal);
                        arrlist.push(orderData.ChequeAmountTotal);
                        arrlist.push(orderData.PayeeName);
                        arrlist.push(orderData.L1RejectReason);
                        arrlist.push(orderData.L1VerificationStatus);
                        arrlist.push(orderData.modified);
                        arrlist.push(orderData.AccModified);
                        arrlist.push(orderData.L2RejectReason);
                        arrlist.push(orderData.L2VerificationStatus);
                        arrlist.push(orderData.globalmodified);
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.rejectreasondescrpsn);
                        arrlist.push(orderData.ctsNonCtsMark);
                        arrlist.push(orderData.P2fMark);
                        arrlist.push(orderData.SlipID);
                        arrlist.push(orderData.SlipRawaDataID);
                        arrlist.push(orderData.Modified3);
                    }

                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL3" + i));
                    if (orderData.Id != null) {
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.CreditAccountNo);
                        arrlist.push(orderData.SlipAmount);
                        arrlist.push(orderData.BatchNo);
                        arrlist.push(orderData.ProcessingDate);
                        arrlist.push(orderData.InstrumentType);
                        arrlist.push(orderData.SlipNo);
                        arrlist.push(orderData.BranchCode);
                        arrlist.push(orderData.ClearingType);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.SlipRefNo);
                        arrlist.push(orderData.ClientCode);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.CBSAccountInformation);
                        arrlist.push(orderData.CBSJointAccountInformation);
                        arrlist.push(orderData.FinalAmount);
                        arrlist.push(orderData.FinalDate);
                        arrlist.push(orderData.ChequeNoFinal);
                        arrlist.push(orderData.SortCodeFinal);
                        arrlist.push(orderData.SANFinal);
                        arrlist.push(orderData.TransCodeFinal);
                        arrlist.push(orderData.ChequeAmountTotal);
                        arrlist.push(orderData.PayeeName);
                        arrlist.push(orderData.L1RejectReason);
                        arrlist.push(orderData.L1VerificationStatus);
                        arrlist.push(orderData.modified);
                        arrlist.push(orderData.AccModified);
                        arrlist.push(orderData.L2RejectReason);
                        arrlist.push(orderData.L2VerificationStatus);
                        arrlist.push(orderData.globalmodified);
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.rejectreasondescrpsn);
                        arrlist.push(orderData.ctsNonCtsMark);
                        arrlist.push(orderData.P2fMark);
                        arrlist.push(orderData.SlipID);
                        arrlist.push(orderData.SlipRawaDataID);
                        arrlist.push(orderData.Modified3);
                    }

                }
            }
            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(idslst);
            //if (orderData.InstrumentType == "S") {
            $.ajax({

                url: RootUrl + 'OWL3/OWL3',
                data: JSON.stringify({ lst: arrlist, snd: scond, btnClose: "Close", idlst: idslst, ChequeAmountTotal: parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) }),

                type: 'POST',
                contentType: 'application/json; charset=utf-8',

                dataType: 'json',
                success: function (result) {
                    if (result == false) {
                        window.location = RootUrl + 'Home/IWIndex';
                    }
                }

            });
            //}
            //else {
            //    $.ajax({

            //        url: RootUrl + 'OWL1Cheques/OWL1',
            //        data: JSON.stringify({ lst: arrlist, snd: scond, btnClose: "Close", idlst: idslst }),

            //        type: 'POST',
            //        contentType: 'application/json; charset=utf-8',

            //        dataType: 'json',
            //        success: function (result) {
            //            if (result == false) {
            //                window.location = RootUrl + 'Home/IWIndex';
            //            }
            //        }

            //    });
            //}

        }


    });
    //------------------------------------------------------------
    var value = 0;
    callrotate = function () {
        value += 180;
        $("#myimg").rotate({ animateTo: value })
    }
    //---------------- Data Entry -----------------------------------

    $("form input").keydown(function (e) {
        // alert('Aila');
        cnt = document.getElementById('cnt').value;
        next_idx = $('input[type=text]').index(this) + 1;
        tot_idx = $('body').find('input[type=text]').length;
        // alert(tot_idx);
        var tempcounter = document.getElementById('cnt').value;

        if (tt[tempcounter].InstrumentType == "S") {
            if (next_idx == 4) {//6
                next_idx = tot_idx;
            }

            if (e.keyCode == 13) {
                // alert(next_idx);
                if (tot_idx == next_idx) {
                    //go to the first text element if focused in the last text input element12.	
                    // alert(tot_idx + "-" + next_idx);
                    $("input[value='Ok']").click();
                }
                else if (next_idx == 1) {

                    var tempacntno = document.getElementById('oldact').value;
                    if ($("#accnt").val() == "") {
                        alert("Please enter account number!!");
                        $("#accnt").focus();
                        return false;
                    }
                    //&& cbsbtn == false
                    if (tempacntno != $("#accnt").val()) {

                        document.getElementById('oldact').value = $("#accnt").val();
                        getcbsdtls();
                    }
                    else {

                        next_idx = 3;//5
                        $('input[type=text]:eq(' + next_idx + ')').focus().select();
                    }
                }
                else {
                    next_idx = 3;//5
                    $('input[type=text]:eq(' + next_idx + ')').focus().select();
                }
            }


        }
        else if (tt[tempcounter].InstrumentType == "C") {

            if (next_idx == 4) {//6
                next_idx = tot_idx;
            }
            else {
                next_idx = 3;//5
            }

            if (e.keyCode == 13) {
                //  alert(next_idx);
                if (tot_idx == next_idx) {
                    $("input[value='Ok']").click();
                }
                else {
                    $('input[type=text]:eq(' + next_idx + ')').focus().select();
                }
            }

        }


    });
    //----------------narration--------------------
    $("#nartext").keypress(function (event) {

        // alert(event.keyCode);
        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 32 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 45 || event.keyCode == 47 || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            event.preventDefault();
        }
    });
    //---------------------------------------------
    //---------------------------------------------
    $("#ChqDate").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }
        //        if (event.keyCode == 113) {
        //            Fullimg(); //Sign
        //            return false;
        //        }
        //alert('abi!!');
        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.charCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            // alert('else');
            //if (event.keyCode < 95) {
            //if (event.keyCode == 32 || event.keyCode < 48 || (event.keyCode > 57)) {
            event.preventDefault();
            //  }
        }
    });
    //---------------------------------------------
    $("#accnt").keypress(function (event) {

        // alert(event.keyCode);
        // debugger;
        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if ($("#allowAlpha").val() == "1") {
            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 96 && event.charCode < 123)) {

            }
            else {
                event.preventDefault();
            }
        }
        else {
            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

            }
            else {
                event.preventDefault();
            }
        }

        //if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
        //    cbsbtn == false
        //}
        //else {
        //    event.preventDefault();
        //}
    });
    //------------------------------------------------

    $("#ChqnoQC,#SortQC,#SANQC,#TransQC,#IWRemark").keypress(function (event) {

        //alert(event.charCode);
        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            event.preventDefault();
        }
    });
    $("#Decision").keydown(function (event) {

        // alert(event.keyCode);

        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }
        if (document.getElementById("blockkey").value == "1") {
            if (event.keyCode == 65 || event.keyCode == 97) {
                event.preventDefault();
                alert('You can not accept this cheque!');
                return false;
            } // || event.keyCode == 99//for refer || event.keyCode == 70
            if (event.keyCode == 65 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

            }
            else {
                event.preventDefault();
            }
        }
        else {
            //|| event.keyCode == 70 refer
            if (event.keyCode == 65 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
            }
            else {
                event.preventDefault();
            }

        }
    });
    //----------Amount---------
    $("#slpamount,#Amt").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || event.charCode == 46 || (event.charCode > 47 && event.charCode < 58)) {
            var amtval = this.val();
            if (amtval.length > 0) {
                var splitstr = amtval.split('.');
                //debugger;
                if (splitstr[1] != null) {
                    var strlength = splitstr[1].length;
                    if (strlength > 1) {
                        // alert('Enter only 2 digit after decimal!');
                        event.preventDefault();
                    }

                }
            }
        }
        else {
            event.preventDefault();
        }
    });

    //$('input#slpamount,#Amt').change(function () {
    //    if ($(this).val() != "") {
    //        //alert('ok');
    //        var num = parseFloat($(this).val());
    //        var cleanNum = num.toFixed(2);
    //        $(this).val(cleanNum);
    //        if (num / num < 1) {
    //            //alert('Please enter only 2 decimal places, we have truncated extra points');
    //            //   $('#error').text('Please enter only 2 decimal places, we have truncated extra points');
    //        }
    //    }
    //});
    //------------rtndescrp---------------------------------

    $("#IWRemark").keyup(function (event) {
        var chkcode = false;
        var rejctrcd = $("#IWRemark").val();
        //  alert(rejctrcd);
        if (rejctrcd.length == 2) {
            var rjctresnlTemp = document.getElementById('rtnlist');
            var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
            for (var i = 0; i < rjctresnlTemp.length; i++) {
                if (rejctrcd == rjctresnlTemp[i].value) {
                    document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
                    chkcode = true;
                    break;
                }
            }
        }
        if (rejctrcd == "88") {

            document.getElementById("rejectreasondescrpsn").readOnly = false;

        }
        else if (rejctrcd != "88") {

            document.getElementById("rejectreasondescrpsn").readOnly = true;
        }
        if (rejctrcd.length == 2) {
            if (chkcode == false) {
                alert('Please enter correct return code!!');
                document.getElementById('IWRemark').value = "";
                document.getElementById('IWRemark').focus();
            }
        }
    });

    //--------------------------Do All input Changes----------------Validation
    $("#accnt").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].CreditAccountNo != $("#accnt").val()) {
            // alert('true');
            realAccModified = true;
            globalmodified = true;
            strModified = setCharAt(strModified, 0, '1');
        }
        else {
            realAccModified = false;
            globalmodified = true;
            strModified = setCharAt(strModified, 0, '0');
        }
    });
    $("#slpamount").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].SlipAmount != parseFloat($("#slpamount").val().replace(/,/g, ''))) {
            realmodified = true;
            globalmodified = true;
            strModified = setCharAt(strModified, 1, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 1, '0');
        }
    });
    $("#Amt").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].FinalAmount != parseFloat($("#Amt").val().replace(/,/g, ''))) {
            realmodified = true;
            globalmodified = true;
            strModified = setCharAt(strModified, 2, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 2, '0');
        }
    });
    $("#ChqDate").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (fnldate != $("#ChqDate").val()) {
            realmodified = true;
            globalmodified = true;
            strModified = setCharAt(strModified, 3, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 3, '0');
        }
    });
    $("#ChqnoQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].ChequeNoFinal != $("#ChqnoQC").val()) {
            modified = true;
            strModified = setCharAt(strModified, 4, '1');
        }
        else {
            modified = false;
            strModified = setCharAt(strModified, 4, '0');
        }

    });
    $("#SortQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].SortCodeFinal != $("#SortQC").val()) {
            modified = true;
            strModified = setCharAt(strModified, 5, '1');
        }
        else {
            strModified = setCharAt(strModified, 5, '0');
        }
    });
    $("#SANQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].SANFinal != $("#SANQC").val()) {
            modified = true;
            strModified = setCharAt(strModified, 6, '1');
        }
        else {
            strModified = setCharAt(strModified, 6, '0');
        }
    });
    $("#TransQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].TransCodeFinal != $("#TransQC").val()) {
            modified = true;
            strModified = setCharAt(strModified, 7, '1');
        }
        else {
            strModified = setCharAt(strModified, 7, '0');
        }
    });

    $("#ctsnocts").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].ClearingType != $("#ctsnocts").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 8, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 8, '0');
        }
    });
    //-------------------------------Common For Accept and Reject-----------------------------
    function common(val) {
        //alert(document.getElementById('cbsdls').value);
        //alert('comman');
        if (Modernizr.localstorage) {
            var localacct = window.localStorage;
            var chqiwmicr = JSON.stringify(L3);
            localacct.setItem(val, chqiwmicr);
        }

        if (backbtn == true) {
            document.getElementById('cnt').value = parseInt(backcnt) + 1;
        }
        else {
            document.getElementById('cnt').value = parseInt(cnt) + 1;
        }
        cnt = document.getElementById('cnt').value;

        if (cnt < tt.length) {
            document.getElementById('myimg').removeAttribute('src');

            document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;

            document.getElementById('Decision').value = "";
            document.getElementById('IWRemark').value = "";
            document.getElementById('rtncd').style.display = "none";
            document.getElementById('rejectreasondescrpsn').value = "";
            // document.getElementById("cbsdetails").innerHTML = "";
            //--------------------------------//
            document.getElementById('ChqCnt').innerHTML = tt[cnt].SlipChequeCount;
            document.getElementById('totamt').innerHTML = ChequeAmountTotal;

            if (tt[1].InstrumentType == "S") {
                document.getElementById('slpamt').style.display = "";
                document.getElementById('accnt').value = "";
                document.getElementById('slpamount').value = ""
                document.getElementById('chqamt').style.display = "none";
                document.getElementById('slpacnt').style.display = "";
                document.getElementById('Chqacnt').style.display = "none";
                document.getElementById('MICR').style.display = "none";
                document.getElementById('slpamount').value = tt[cnt].SlipAmount;
                document.getElementById('accnt').focus();
                document.getElementById('divctsnoncts').style.display = "none";
                document.getElementById('divmarkp2f').style.display = "none";
                document.getElementById('bankname').style.display = "none";
                document.getElementById('lblslpimg').style.display = "none";

                realmodified = false;
                realAccModified = false;
                globalmodified = false;

            }
            else {

                document.getElementById('slpamt').style.display = "none";
                document.getElementById('chqamt').style.display = "";
                document.getElementById('Chqacnt').style.display = "";
                document.getElementById('chequeAcct').innerHTML = "";
                document.getElementById('slpacnt').style.display = "none";
                document.getElementById('MICR').style.display = "";
                document.getElementById('ChqDate').value = "";
                document.getElementById('Amt').value = "";
                document.getElementById('ChqnoQC').value = "";
                document.getElementById('SortQC').value = "";
                document.getElementById('SANQC').value = "";
                document.getElementById('TransQC').value = "";

                document.getElementById('Slipamt').innerHTML = addCommas(Number(tt[cnt].SlipAmount).toFixed(2));
                document.getElementById('sliplabl').style.display = "";

                document.getElementById('divctsnoncts').style.display = "";
                document.getElementById('divmarkp2f').style.display = "";
                document.getElementById('lblslpimg').style.display = "";

                // if (tt[cnt].ClearingType == "01") {
                $('#ctsnocts').val(tt[cnt].ClearingType);

                if (tt[cnt].DocType.toUpperCase() == "C") {
                    document.getElementById("markp2f").checked = true;
                }
                else {
                    document.getElementById("markp2f").checked = false;
                }

                //--------------------------------------------------CheqDte---
                if (tt[1].FinalDate != "" || tt[1].FinalDate != null) {

                    if (tt[1].FinalDate.length > 6) {
                        tempdat = tt[1].FinalDate.split("-");
                        yr = tempdat[0];
                        yr = yr.substring(2, 4);
                        mm = tempdat[1];
                        dd = tempdat[2];
                        fnldate = dd + mm + yr;
                    }
                    else {
                        tempdat = tt[1].FinalDate;
                        yr = tempdat.substring(4, 6);
                        mm = tempdat.substring(2, 4);
                        dd = tempdat.substring(0, 2);
                        fnldate = dd + mm + yr;
                    }
                }
                else {
                    fnldate = "";
                }
                //------------------------------
                // document.getElementById('nartext').value = tempnarration;

                document.getElementById('chequeAcct').innerHTML = tt[cnt].CreditAccountNo;
                document.getElementById('Amt').value = addCommas(Number(tt[cnt].FinalAmount).toFixed(2));
                document.getElementById('ChqDate').value = fnldate;
                document.getElementById('ChqnoQC').value = tt[cnt].ChequeNoFinal;
                document.getElementById('SortQC').value = tt[cnt].SortCodeFinal;
                document.getElementById('SANQC').value = tt[cnt].SANFinal;
                document.getElementById('TransQC').value = tt[cnt].TransCodeFinal;
                document.getElementById('Decision').focus();
                tempAmtValue = tt[cnt].FinalAmount;

                document.getElementById('bankname').style.display = "";
                bankName(tt[cnt].SortCodeFinal);  //-------------For bank name

                document.getElementById('mtrn').value = tt[cnt].RawDataId;

                if ($("#NarrationID").val() == "Y") {
                    document.getElementById('nartext').value = tt[cnt].UserNarration;
                    $('#nartext').attr('readonly', false);
                }

                //-------------------------------------- Modification HI-------------------------------

                //-------------Amount------------------------------------------------------------------
                if (tt[cnt].Modified2.charAt(2) == "1") {
                    document.getElementById("Amt").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("Amt").style.backgroundColor = "white";
                }
                //-------------ChqDate-----------------------------------------------------------------
                if (tt[cnt].Modified2.charAt(3) == "1") {
                    document.getElementById("ChqDate").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("ChqDate").style.backgroundColor = "white";
                }
                //-------------ChqNo--------------------------------------------------------------------
                if (tt[cnt].Modified2.charAt(4) == "1") {
                    document.getElementById("ChqnoQC").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("ChqnoQC").style.backgroundColor = "white";
                }
                //-------------SortQC-------------------------------------------------------------------
                if (tt[cnt].Modified2.charAt(5) == "1") {
                    document.getElementById("SortQC").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("SortQC").style.backgroundColor = "white";
                }
                //-------------SortQC-------------------------------------------------------------------
                if (tt[cnt].Modified2.charAt(6) == "1") {
                    document.getElementById("SANQC").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("SANQC").style.backgroundColor = "white";
                }
                //-------------TransQC-------------------------------------------------------------------
                if (tt[cnt].Modified2.charAt(7) == "1") {
                    document.getElementById("TransQC").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("TransQC").style.backgroundColor = "white";
                }

                //---------------------CTS & Non CTS-------------------------------------------------------------------
                if (tt[cnt].Modified2.charAt(8) == "1") {
                    document.getElementById("ctsnocts").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("ctsnocts").style.backgroundColor = "white";
                }


                //----------------------Set L1 and L2 Decision Color ----------------
                document.getElementById("l1decision").innerHTML = "";
                document.getElementById("l2decision").innerHTML = "";
                document.getElementById("L1rejectDecrp").innerHTML = "";
                document.getElementById("L2rejectDecrp").innerHTML = "";

                if (tt[cnt].L1VerificationStatus == 2) {

                    document.getElementById("l1decision").innerHTML = "Y";
                    //document.getElementById('l1decision').style.background = "Green";
                    document.getElementById("l1decision").classList.add("w3-text-green");

                }
                else if (tt[cnt].L1VerificationStatus == 3) {
                    document.getElementById("l1decision").innerHTML = "R";
                    document.getElementById("l1decision").classList.add("w3-text-red");
                    document.getElementById("L1rejectDecrp").innerHTML = "";
                    // getL1Logs(tt[1].RawDataId);
                    getReturnDecrp(tt[cnt].L1RejectReason, "L1");
                }
                if (tt[cnt].L2VerificationStatus == 2) {

                    document.getElementById("l2decision").innerHTML = "Y";
                    // document.getElementById('l2decision').style.background = "Green";
                    document.getElementById("l2decision").classList.add("w3-text-green");

                }
                else if (tt[cnt].L2VerificationStatus == 3 || tt[cnt].L2VerificationStatus == 8) {
                    if (tt[cnt].L2VerificationStatus == 3) {
                        document.getElementById("l2decision").innerHTML = "R";
                        document.getElementById("l2decision").classList.add("w3-text-red");
                        getReturnDecrp(tt[cnt].L2RejectReason, "L2");
                    }
                    else {
                        document.getElementById("l2decision").innerHTML = "M";
                        document.getElementById("l2decision").classList.add("w3-text-orange");
                        document.getElementById("L2rejectDecrp").innerHTML = "";
                    }


                }

                ////---------------------Filling cbsDetails------
                if (tt[cnt].CreditAccountNo != "" || tt[cnt].CreditAccountNo != null) {

                    $.ajax({
                        url: RootUrl + 'OWL3/GetCBSDtls',
                        dataType: 'html',
                        data: { ac: tt[cnt].CreditAccountNo, strcbsdls: tt[cnt].CBSAccountInformation, strJoinHldrs: tt[cnt].CBSJointAccountInformation },
                        success: function (data) {
                            cbsbtn = true;
                            $('#cbsdetails').html(data);
                            $("#Payee option:first").attr('selected', 'selected');
                        }
                    });
                }
                if (narrationReqirdflg == true) {
                    document.getElementById('narsndiv').style.display = "";
                    document.getElementById('nartext').value = tt[cnt].UserNarration;
                }
                else {
                    document.getElementById('narsndiv').style.display = "none";
                }
            }

            //--------------------------------------------------------------
            document.getElementById('strbranchcd').innerHTML = tt[cnt].BranchCode;
            document.getElementById('ScanningID').innerHTML = tt[cnt].ScanningNodeId;
            document.getElementById('strBatchNo').innerHTML = tt[cnt].BatchNo;
            document.getElementById('strBatchSeqNO').innerHTML = tt[cnt].BatchSeqNo;
            //--------------------------------------------------------------

            // document.getElementById("h2amt").innerHTML = "{{iwAmt|number:2}}";
            if (backbtn == true) {
                document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
            }
            else {
                document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
            }

            backbtn = false;
        }
        else if (cnt > 0) {

            if (Modernizr.localstorage) {
                var listItems = [];
                var arrlist = [];
                var localData = window.localStorage;
                // alert('modi');
                if (scond == true) {
                    var i;
                    if (tt[0].callby == "Cheq") {
                        i = 0;
                    }
                    else {
                        i = 1;
                    }
                    for (i; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("owL3" + i));

                        if (orderData.Id != null) {
                            arrlist.push(orderData.Id);
                            arrlist.push(orderData.CreditAccountNo);
                            arrlist.push(orderData.SlipAmount);
                            arrlist.push(orderData.BatchNo);
                            arrlist.push(orderData.ProcessingDate);
                            arrlist.push(orderData.InstrumentType);
                            arrlist.push(orderData.SlipNo);
                            arrlist.push(orderData.BranchCode);
                            arrlist.push(orderData.ClearingType);
                            arrlist.push(orderData.ScanningNodeId);
                            arrlist.push(orderData.SlipRefNo);
                            arrlist.push(orderData.ClientCode);
                            arrlist.push(orderData.Action);
                            arrlist.push(orderData.Status);
                            arrlist.push(orderData.RawDataId);
                            arrlist.push(orderData.RejectReason);
                            arrlist.push(orderData.DomainId);
                            arrlist.push(orderData.CustomerId);
                            arrlist.push(orderData.CBSAccountInformation);
                            arrlist.push(orderData.CBSJointAccountInformation);
                            arrlist.push(orderData.FinalAmount);
                            arrlist.push(orderData.FinalDate);
                            arrlist.push(orderData.ChequeNoFinal);
                            arrlist.push(orderData.SortCodeFinal);
                            arrlist.push(orderData.SANFinal);
                            arrlist.push(orderData.TransCodeFinal);
                            arrlist.push(orderData.ChequeAmountTotal);
                            arrlist.push(orderData.PayeeName);
                            arrlist.push(orderData.L1RejectReason);
                            arrlist.push(orderData.L1VerificationStatus);
                            arrlist.push(orderData.modified);
                            arrlist.push(orderData.AccModified);
                            arrlist.push(orderData.L2RejectReason);
                            arrlist.push(orderData.L2VerificationStatus);
                            arrlist.push(orderData.globalmodified);
                            arrlist.push(orderData.UserNarration);
                            arrlist.push(orderData.rejectreasondescrpsn);
                            arrlist.push(orderData.ctsNonCtsMark);
                            arrlist.push(orderData.P2fMark);
                            arrlist.push(orderData.SlipID);
                            arrlist.push(orderData.SlipRawaDataID);
                            arrlist.push(orderData.Modified3);

                        }

                    }
                }
                else {


                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("owL3" + i));

                        if (orderData.Id != null) {
                            arrlist.push(orderData.Id);
                            arrlist.push(orderData.CreditAccountNo);
                            arrlist.push(orderData.SlipAmount);
                            arrlist.push(orderData.BatchNo);
                            arrlist.push(orderData.ProcessingDate);
                            arrlist.push(orderData.InstrumentType);
                            arrlist.push(orderData.SlipNo);
                            arrlist.push(orderData.BranchCode);
                            arrlist.push(orderData.ClearingType);
                            arrlist.push(orderData.ScanningNodeId);
                            arrlist.push(orderData.SlipRefNo);
                            arrlist.push(orderData.ClientCode);
                            arrlist.push(orderData.Action);
                            arrlist.push(orderData.Status);
                            arrlist.push(orderData.RawDataId);
                            arrlist.push(orderData.RejectReason);
                            arrlist.push(orderData.DomainId);
                            arrlist.push(orderData.CustomerId);
                            arrlist.push(orderData.CBSAccountInformation);
                            arrlist.push(orderData.CBSJointAccountInformation);
                            arrlist.push(orderData.FinalAmount);
                            arrlist.push(orderData.FinalDate);
                            arrlist.push(orderData.ChequeNoFinal);
                            arrlist.push(orderData.SortCodeFinal);
                            arrlist.push(orderData.SANFinal);
                            arrlist.push(orderData.TransCodeFinal);
                            arrlist.push(orderData.ChequeAmountTotal);
                            arrlist.push(orderData.PayeeName);
                            arrlist.push(orderData.L1RejectReason);
                            arrlist.push(orderData.L1VerificationStatus);
                            arrlist.push(orderData.modified);
                            arrlist.push(orderData.AccModified);
                            arrlist.push(orderData.L2RejectReason);
                            arrlist.push(orderData.L2VerificationStatus);
                            arrlist.push(orderData.globalmodified);
                            arrlist.push(orderData.UserNarration);
                            arrlist.push(orderData.rejectreasondescrpsn);
                            arrlist.push(orderData.ctsNonCtsMark);
                            arrlist.push(orderData.P2fMark);
                            arrlist.push(orderData.SlipID);
                            arrlist.push(orderData.SlipRawaDataID);
                            arrlist.push(orderData.Modified3);
                        }
                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------

                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;


                $.ajax({

                    url: RootUrl + 'OWL3/OWL3',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[pcnt - 1].FrontGreyImagePath, idlst: idslst, ChequeAmountTotal: parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) }),

                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',

                    dataType: 'json',
                    success: function (result) {

                        if (result == false) {
                            window.location = RootUrl + 'Home/IWIndex?id=1';
                        }
                        else {
                            tt = result;
                            document.getElementById('tempcnt').value = 1;
                            document.getElementById('cnt').value = 1;
                            cnt = 1;
                            tempcnt = 1;

                            if (tt != null && tt != "") {
                                scond = true;
                                idslst = [];

                                //-------------- idslist--------------------//
                                for (var z = 0; z < tt.length; z++) {
                                    idslst.push(tt[z].Id)
                                }

                                //-------Remove save objects from browser---//
                                window.localStorage.clear();
                                //------------ idslist end----------------//
                                //  alert(idslst.length);
                                //------------ idslist end----------------//
                                //  scondbck = true;
                                //-------------Saving Last data in storage---
                                var owL3 = "owL30"
                                InstrumentType = tt[1].InstrumentType;

                                if (tt[0].callby == "Cheq") {

                                    var L3 = {
                                        "CreditAccountNo": $("#accnt").val(),
                                        "SlipAmount": parseFloat($("#slpamount").val().replace(/,/g, '')),
                                        "BatchNo": tt[0].BatchNo,
                                        "ClearingType": tt[0].ClearingType,
                                        "InstrumentType": tt[0].InstrumentType,
                                        "SlipNo": tt[0].SlipNo,
                                        "ProcessingDate": tt[0].ProcessingDate,
                                        "BranchCode": tt[0].BranchCode,
                                        "ScanningNodeId": tt[0].ScanningNodeId,
                                        "ClientCode": tt[0].ClientCode,
                                        "SlipRefNo": tt[0].SlipRefNo,
                                        "Id": tt[0].Id,
                                        "Action": tt[0].Action,
                                        "Status": tt[0].Status,
                                        "RawDataId": tt[0].RawDataId,
                                        "RejectReason": tt[0].RejectReason,
                                        "DomainId": tt[0].DomainId,
                                        "CustomerId": tt[0].CustomerId,
                                        "CBSAccountInformation": tt[0].CBSAccountInformation,
                                        "CBSJointAccountInformation": tt[0].CBSJointAccountInformation,
                                        "FinalAmount": tt[0].FinalAmount,
                                        "FinalDate": tt[0].FinalDate,
                                        "ChequeNoFinal": tt[0].ChequeNoFinal,
                                        "SortCodeFinal": tt[0].SortCodeFinal,
                                        "SANFinal": tt[0].SANFinal,
                                        "TransCodeFinal": tt[0].TransCodeFinal,
                                        "ChequeAmountTotal": tt[0].ChequeAmountTotal,
                                        "PayeeName": tt[0].PayeeName,
                                        "L1RejectReason": tt[0].L1RejectReason,
                                        "L1VerificationStatus": tt[0].L1VerificationStatus,
                                        "modified": tt[0].modified,
                                        "AccModified": tt[0].AccModified,
                                        "L2RejectReason": tt[0].L2RejectReason,
                                        "L2VerificationStatus": tt[0].L2VerificationStatus,
                                        "globalmodified": tt[0].globalmodified,
                                        "rejectreasondescrpsn": tt[0].RejectReasonDescription,
                                        "ctsNonCtsMark": tt[0].ctsNonCtsMark,
                                        "SlipID": tt[0].SlipID,
                                        "SlipRawaDataID": tt[0].SlipRawaDataID,
                                        "Modified3": tt[0].Modified3,
                                    };
                                    if (Modernizr.localstorage) {

                                        var localacct = window.localStorage;
                                        var chqiwmicr = JSON.stringify(L3);
                                        localacct.setItem(owL3, chqiwmicr);

                                    }
                                }

                                // alert(tt[1].FrontGreyImagePath);


                                //----------------------------------------------------------------------//
                                document.getElementById('myimg').removeAttribute('src');

                                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

                                document.getElementById('Decision').value = "";
                                document.getElementById('IWRemark').value = "";
                                document.getElementById('rtncd').style.display = "none";
                                // document.getElementById("cbsdetails").innerHTML = "";
                                document.getElementById("L1rejectDecrp").innerHTML = "";
                                document.getElementById("L2rejectDecrp").innerHTML = "";
                                document.getElementById('rejectreasondescrpsn').value = "";

                                //--------------------------------//
                                document.getElementById('ChqCnt').innerHTML = tt[1].SlipChequeCount;
                                document.getElementById('totamt').innerHTML = tt[1].ChequeAmountTotal;
                                ChequeAmountTotal = tt[1].ChequeAmountTotal;
                                tempAmtValue = tt[cnt].FinalAmount;
                                // alert(tt[1].ChequeAmountTotal);

                                // debugger;
                                //----------------------Set L1 and L2 Decision Color ----------------
                                if (tt[1].L1VerificationStatus == 2) {

                                    document.getElementById("l1decision").innerHTML = "Y";
                                    //document.getElementById('l1decision').style.background = "Green";
                                    document.getElementById("l1decision").classList.add("w3-text-green");

                                }
                                else if (tt[1].L1VerificationStatus == 3) {
                                    document.getElementById("l1decision").innerHTML = "R";
                                    document.getElementById("l1decision").classList.add("w3-text-red");
                                    document.getElementById("L1rejectDecrp").innerHTML = "";
                                    // getL1Logs(tt[1].RawDataId);
                                    getReturnDecrp(tt[1].L1RejectReason, "L1");
                                }
                                if (tt[1].L2VerificationStatus == 2) {

                                    document.getElementById("l2decision").innerHTML = "Y";
                                    // document.getElementById('l2decision').style.background = "Green";
                                    document.getElementById("l2decision").classList.add("w3-text-green");

                                }
                                else if (tt[1].L2VerificationStatus == 3 || tt[1].L2VerificationStatus == 8) {
                                    if (tt[1].L2VerificationStatus == 3) {
                                        document.getElementById("l2decision").innerHTML = "R";
                                        document.getElementById("l2decision").classList.add("w3-text-red");
                                        getReturnDecrp(tt[1].L2RejectReason, "L2");
                                    }
                                    else {
                                        document.getElementById("l2decision").innerHTML = "M";
                                        document.getElementById("l2decision").classList.add("w3-text-orange");
                                        document.getElementById("L2rejectDecrp").innerHTML = "";
                                    }
                                    // 

                                }

                                //  debugger;
                                if (tt[1].InstrumentType == "S") {

                                    if (tt[1].ScanningType == 3 || tt[1].ScanningType == 5) {

                                        $("#ClientCd").prop('disabled', true);
                                        if ($("#ClientCd").val().toUpperCase() != "CMS") {
                                            document.getElementById('lblpayee').innerHTML = "";
                                        }
                                        document.getElementById('ClntsDtlsdiv').style.display = "";
                                        // document.getElementById('Chqacnt').style.display = "";

                                        document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;
                                        document.getElementById('ClientCd').value = tt[1].ClientCode;

                                        $.ajax({
                                            url: RootUrl + 'OWL3/GetCBSDtls',
                                            dataType: 'html',
                                            data: { ac: tt[1].CreditAccountNo, callby: "ClientCode" },
                                            success: function (data) {
                                                $('#cbsdetails').html(data);
                                                cbsbtn = true;
                                            }
                                        });

                                        clientdtls();
                                        //  document.getElementById('ClientCd').focus();


                                        document.getElementById('accnt').value = "";
                                        document.getElementById('slpacnt').style.display = "";
                                        document.getElementById('accnt').value = tt[1].CreditAccountNo;
                                        document.getElementById('Decision').focus();
                                        //document.getElementById("Payee").disabled = true;
                                        //   $("#Payee").prop('disabled', 'disabled');
                                        //$("#Payee").removeAttr("disabled");

                                    }
                                    else {
                                        document.getElementById('slpacnt').style.display = "";
                                        document.getElementById('accnt').focus();
                                        document.getElementById('Chqacnt').style.display = "none";
                                        document.getElementById('accnt').value = tt[1].CreditAccountNo;
                                        document.getElementById('Decision').focus();


                                        //--------------------Added On 07-02-2017------------------
                                        document.getElementById('oldact').value = tt[1].CreditAccountNo;

                                        $.ajax({
                                            url: RootUrl + 'OWL3/GetCBSDtls',
                                            dataType: 'html',
                                            data: { ac: $("#accnt").val(), strcbsdls: tt[1].CBSAccountInformation, strJoinHldrs: tt[1].CBSJointAccountInformation, callby: "Normal", payeename: tt[1].PayeeName },
                                            success: function (data) {
                                                $('#cbsdetails').html(data);
                                                cbsbtn = true;
                                            }
                                        });

                                        //---------------Select payee name as per L3 verification-------
                                        //var el = document.getElementById("Payee");
                                        //for (var i = 0; i < el.options.length; i++) {
                                        //    if (el.options[i].text == tt[1].PayeeName) {
                                        //        el.selectedIndex = i;
                                        //        break;
                                        //    }
                                        //}
                                    }
                                    //document.getElementById('nartext').value = "";
                                    //tempnarration = "";
                                    //narrationReqirdflg = false;
                                    //if (tt[1].CreditAccountNo.length == 14) {
                                    //    if (tt[1].CreditAccountNo == "06410125027255") {
                                    //        document.getElementById('narsndiv').style.display = "";
                                    //        document.getElementById('nartext').value = tt[1].UserNarration;
                                    //        tempnarration == tt[1].UserNarration;
                                    //        narrationReqirdflg = true;
                                    //    }
                                    //    else {
                                    //        tempnarration == tt[1].UserNarration;
                                    //        narrationAC(tt[1].CreditAccountNo);
                                    //    }

                                    //}
                                    if ($("#NarrationID").val() == "Y") {
                                        document.getElementById('nartext').value = tt[1].UserNarration;
                                        $('#nartext').attr('readonly', false);
                                    }

                                    document.getElementById('slpamt').style.display = "";
                                    document.getElementById('chqamt').style.display = "none";
                                    document.getElementById('slpamount').value = ""
                                    document.getElementById('MICR').style.display = "none";
                                    document.getElementById('slpamount').value = addCommas(Number(tt[1].SlipAmount).toFixed(2));
                                    document.getElementById('sliplabl').style.display = "none";
                                    document.getElementById('Slipamt').innerHTML = "";
                                    document.getElementById('divctsnoncts').style.display = "none";
                                    document.getElementById('divmarkp2f').style.display = "none";
                                    document.getElementById('bankname').style.display = "none";
                                    document.getElementById('lblslpimg').style.display = "none";

                                    //-------------Account-----------------------
                                    if (tt[1].Modified2.charAt(0) == "1") {
                                        document.getElementById("accnt").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("accnt").style.backgroundColor = "white";
                                    }
                                    //-------------Amount---------------
                                    if (tt[1].Modified2.charAt(1) == "1") {
                                        document.getElementById("slpamount").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("slpamount").style.backgroundColor = "white";
                                    }

                                    randomPayeeName = "";

                                    realmodified = false;
                                    realAccModified = false;
                                    globalmodified = false;

                                }
                                else {
                                    //alert('Else');
                                    document.getElementById("btnback").disabled = true;//prev false

                                    document.getElementById('slpamt').style.display = "none";
                                    document.getElementById('chqamt').style.display = "";
                                    document.getElementById('Chqacnt').style.display = "";
                                    document.getElementById('chequeAcct').innerHTML = "";
                                    document.getElementById('slpacnt').style.display = "none";
                                    document.getElementById('MICR').style.display = "";
                                    document.getElementById('ChqDate').value = "";
                                    document.getElementById('Amt').value = "";
                                    document.getElementById('ChqnoQC').value = "";
                                    document.getElementById('SortQC').value = "";
                                    document.getElementById('SANQC').value = "";
                                    document.getElementById('TransQC').value = "";
                                    randomPayeeName = "";

                                    document.getElementById('Slipamt').innerHTML = tt[1].SlipAmount;
                                    document.getElementById('sliplabl').style.display = "";

                                    document.getElementById('divctsnoncts').style.display = "";
                                    document.getElementById('divmarkp2f').style.display = "";
                                    document.getElementById('lblslpimg').style.display = "";

                                    // if (tt[cnt].ClearingType == "01") {
                                    $('#ctsnocts').val(tt[cnt].ClearingType);

                                    if (tt[cnt].DocType.toUpperCase() == "C") {
                                        document.getElementById("markp2f").checked = true;
                                    }
                                    else {
                                        document.getElementById("markp2f").checked = false;
                                    }

                                    //--------------------------------------------------CheqDte---
                                    if (tt[1].FinalDate != "" || tt[1].FinalDate != null) {

                                        if (tt[1].FinalDate.length > 6) {
                                            tempdat = tt[1].FinalDate.split("-");
                                            yr = tempdat[0];
                                            yr = yr.substring(2, 4);
                                            mm = tempdat[1];
                                            dd = tempdat[2];
                                            fnldate = dd + mm + yr;
                                        }
                                        else {
                                            tempdat = tt[1].FinalDate;
                                            yr = tempdat.substring(4, 6);
                                            mm = tempdat.substring(2, 4);
                                            dd = tempdat.substring(0, 2);
                                            fnldate = dd + mm + yr;
                                        }
                                    }
                                    else {
                                        fnldate = "";
                                    }
                                    //------------------------------
                                    // document.getElementById('nartext').value = tempnarration;
                                    document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;
                                    document.getElementById('Amt').value = addCommas(Number(tt[1].FinalAmount).toFixed(2));
                                    document.getElementById('ChqDate').value = fnldate;
                                    document.getElementById('ChqnoQC').value = tt[1].ChequeNoFinal;
                                    document.getElementById('SortQC').value = tt[1].SortCodeFinal;
                                    document.getElementById('SANQC').value = tt[1].SANFinal;
                                    document.getElementById('TransQC').value = tt[1].TransCodeFinal;

                                    randomPayeeName = tt[1].PayeeName;

                                    globalmodified = tt[1].globalmodified;
                                    realAccModified = tt[1].AccModified;
                                    document.getElementById('Decision').focus();
                                    document.getElementById('mtrn').value = tt[1].RawDataId;

                                    document.getElementById('bankname').style.display = "";
                                    bankName(tt[1].SortCodeFinal);  //-------------For bank name

                                    //-------------------------------------- Modification HI-------------------------------

                                    //-------------Amount----------------------------------------------------------------
                                    if (tt[1].Modified2.charAt(2) == "1") {
                                        document.getElementById("Amt").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("Amt").style.backgroundColor = "white";
                                    }
                                    //-------------ChqDate-----------------------------------------------------------------
                                    if (tt[1].Modified2.charAt(3) == "1") {
                                        document.getElementById("ChqDate").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("ChqDate").style.backgroundColor = "white";
                                    }
                                    //-------------ChqNo--------------------------------------------------------------------
                                    if (tt[1].Modified2.charAt(4) == "1") {
                                        document.getElementById("ChqnoQC").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("ChqnoQC").style.backgroundColor = "white";
                                    }
                                    //-------------SortQC-------------------------------------------------------------------
                                    if (tt[1].Modified2.charAt(5) == "1") {
                                        document.getElementById("SortQC").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("SortQC").style.backgroundColor = "white";
                                    }
                                    //-------------SortQC-------------------------------------------------------------------
                                    if (tt[1].Modified2.charAt(6) == "1") {
                                        document.getElementById("SANQC").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("SANQC").style.backgroundColor = "white";
                                    }
                                    //-------------TransQC-------------------------------------------------------------------
                                    if (tt[1].Modified2.charAt(7) == "1") {
                                        document.getElementById("TransQC").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("TransQC").style.backgroundColor = "white";
                                    }

                                    //-------------CTS & Non CTS-------------------------------------------------------------------
                                    if (tt[1].Modified2.charAt(8) == "1") {
                                        document.getElementById("ctsnocts").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("ctsnocts").style.backgroundColor = "white";
                                    }


                                    //---------------------Filling cbsDetails------
                                    var callbyValue;
                                    //   alert(tt[1].ScanningType);
                                    if (tt[1].ScanningType == 3 || tt[1].ScanningType == 5) {
                                        callbyValue = "ClientCode";
                                        $("#ClientCd").prop('disabled', true);
                                    }
                                    else {
                                        callbyValue = "Normal";
                                    }

                                    if ($("#NarrationID").val() == "Y") {
                                        document.getElementById('nartext').value = tt[1].UserNarration;
                                        $('#nartext').attr('readonly', false);
                                    }
                                    $.ajax({
                                        url: RootUrl + '/OWL3/GetCBSDtls',
                                        dataType: 'html',
                                        data: { ac: tt[1].CreditAccountNo, strcbsdls: tt[1].CBSAccountInformation, strJoinHldrs: tt[1].CBSJointAccountInformation, callby: "Normal", payeename: tt[1].PayeeName },
                                        success: function (data) {
                                            cbsbtn = true;
                                            $('#cbsdetails').html(data);
                                        }
                                    });

                                }
                                //--------------------------------------------------------------
                                document.getElementById('strbranchcd').innerHTML = tt[1].BranchCode;
                                document.getElementById('ScanningID').innerHTML = tt[1].ScanningNodeId;
                                document.getElementById('strBatchNo').innerHTML = tt[1].BatchNo;
                                document.getElementById('strBatchSeqNO').innerHTML = tt[1].BatchSeqNo;
                                //--------------------------------------------------------------
                            }
                        }

                    },
                    error: function () {
                        alert("error");
                    }
                });

            }
            else {
                alert('No data found!!');
            }
        }
    }



    getcbsdtls = function () {

        //alert('Smile!!');
        var Acct = document.getElementById('accnt').value;
        var acmin = document.getElementById('acmin').value;
        //Acct = Acct.replace(/^0+/, '')
        Acct = Acct.length


        if (Acct == "") {
            alert("Account no field should not be empty !");
            document.getElementById('accnt').focus();
            return false;
        }
        if (Acct < acmin) {

            alert("Minimum account no sould be " + acmin + " digits");
            document.getElementById('accnt').focus();
            return false;
        }
        // Acct = document.getElementById('accnt').value;
        if (Acct == "") {
            alert("Invalid Account Number!");
            document.getElementById('accnt').focus();
            return false;
        }
        else {
            $.ajax({
                url: RootUrl + 'OWL3/GetCBSDtls',
                dataType: 'html',
                data: { ac: $("#accnt").val() },
                success: function (data) {
                    cbsbtn = true;
                    $('#cbsdetails').html(data);

                }
            });
        }
    }

    //---------------------------OWActivity Logs-------------------
    getL1Logs = function (iwmntr) {
        $.ajax({
            url: RootUrl + 'OWL3/getL1logs',
            dataType: 'html',
            data: { id: iwmntr },
            success: function (data) {
                document.getElementById('L1rejectDecrp').value = data;
                //$('#cbsdetails').html(data);
                //$('#dialogEditUser').dialog('open');
            }
        });
    }


});


function clientdtls() {
    debugger;
    var clntcnt = document.getElementById('cnt').value;
    // alert(tt[clntcnt].BranchCode);
    if ($("#ClientCd").val() != "" && $("#ClientCd").val().toUpperCase() == "CMS") {
        return true;
    }
    else if ($("#ClientCd").val() != "") {

        $.ajax({
            url: RootUrl + 'OWL3/GetClientDlts',
            dataType: 'html',
            data: { ac: $("#ClientCd").val(), branchcode: tt[clntcnt].BranchCode },
            success: function (data) {
                $('#clientdetails').html(data);
                cbsbtn = true;
            },
            error: function () {
                alert("An error occurred while procesing your request. Service Unavailable  !!!...\\n Please Login Again");
            }
        });
    }
    else {
        alert('Please enter Client Code !!');
        $("#ClientCd").focus();
        return false;
    }

}

//-----------------------Activity Logs----------------
function getowlogs() {

    $(document).ready(function () {
        $("#activitylogs").dialog({
            draggable: true,
            height: 500,
            width: 600,
            position: { my: "Center Top", at: "center center", of: window },
            //buttons: [
            //{
            //    text: "minimize",
            //    click: function () {
            //        $(this).parents('.ui-dialog').animate({
            //            height: '40px',
            //            top: $(window).height() - 40
            //        }, 400); 
            //    }
            //}]
        });

        $.ajax({
            url: RootUrl + 'OWL3/getOWlogs',
            dataType: 'html',
            data: { id: $("#mtrn").val() },
            success: function (data) {
                //alert(data);
                $('#activitylogs').html(data);
                $('#activitylogs').dialog('open');
            }
        });
    });
}
function getselect() {
    var valcntget = document.getElementById('cnt').value;
    //----------------DBTAC-----------------------
    if (tt[valcntget].ScanningType == 3 || tt[valcntget].ScanningType == 5) {
        randomPayeeName = document.getElementById('txtpayee').value;
    }
    else {
        randomPayeeName = document.getElementById('Payee').value;
    }
    //randomPayeeName = $("#Payee").val();
}
function getReturnDecrp(rtncode, rjctlevel) {
    //alert('ok');
    var valTempcnt = document.getElementById('cnt').value;
    var rjctresnl = document.getElementById('rtnlist');
    var rtnlistDescrp = document.getElementById('rtnlistDescrp');
    for (var i = 0; i < rjctresnl.length; i++) {

        if (rtncode == rjctresnl[i].value) {
            //  alert(rtnlistDescrp[i].value);
            if (rjctlevel == "L2") {
                if (rtncode == "88") {

                    document.getElementById("L2rejectDecrp").innerHTML = tt[valTempcnt].RejectReasonDescription;
                }
                else {
                    document.getElementById("L2rejectDecrp").innerHTML = rtnlistDescrp[i].value;
                }

                document.getElementById('L2rejectDecrp').style.display = "";
            }
            else {
                if (rtncode == "88") {

                    document.getElementById("L1rejectDecrp").innerHTML = tt[valTempcnt].RejectReasonDescription;
                }
                else {
                    document.getElementById("L1rejectDecrp").innerHTML = rtnlistDescrp[i].value;
                }

                document.getElementById('L1rejectDecrp').style.display = "";
            }

            break;
        }
    }
}
function reasonselected(rtnval) {
    //var rtnrjctdescrn = document.getElementById('rtndescrp').value;
    ////-----valid Function for validation---------------
    //var rslt = valid(document.getElementById('rtndescrp').value, rtnval);

    //if (rslt == false) {
    //    //alert('Please select reject reason!!');
    //    document.getElementById('rtndescrp').focus();
    //    document.getElementById('RejectReason').style.display = 'block';
    //    return false;
    //}
    // else {
    // alert(rtnval);
    document.getElementById('IWRemark').value = rtnval;
    document.getElementById('RejectReason').style.display = 'none';
    //var rejctrcd = $("#IWRemark").val();
    if (rtnval.length == 2) {
        var rjctresnlTemp = document.getElementById('rtnlist');
        var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
        for (var i = 0; i < rjctresnlTemp.length; i++) {
            if (rtnval == rjctresnlTemp[i].value) {
                document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
                // alert("fun " + rtnval);
                break;
            }
        }
    }
    // }
}
//---------------
function valid(RejectDescription, Reasoncode) {
    //  alert(Reasoncode + " Valid");
    if (Reasoncode == "") {
        alert('Reason code not selected !');
        return false;
    }
    if (Reasoncode == "88" && RejectDescription == "") {
        alert('Please enter reject description!');
        //document.getElementById('RejectDescription').style.display = "";
        return false;
    }
    else if (Reasoncode == "88") {
        var str1 = RejectDescription.toUpperCase();
        var str = str1.replace(/\s+/g, " ");

        for (var j = 0; j < str.length; j++) {
            if (str.charAt(j) == "&" || str.charAt(j) == "<" || str.charAt(j) == ">" || str.charAt(j) == "'" || str.charAt(j) == '"') {
                alert("some special characters are not allowed\n & < > ' ");
                return false;
            }
        }
        if (str.length < 6) {
            alert('Please enter minimum 6 character for reject description!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('RejectDescription').focus();
            return false;
        }
        // alert(str);
        if (str.indexOf("OTHER REASON") >= 0 || str.indexOf("OTHERREASON") >= 0) {
            alert('You can not mention "other reason"!!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('RejectDescription').focus();
            return false;
        }
        else if (str.indexOf("OTHER") >= 0 && str.indexOf("REASON") >= 0) {
            alert('You can not mention "other reason"!!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('RejectDescription').focus();
            return false;
        }
        var re = /[^0-9]/g;
        // alert(str.charAt(0));
        // alert(re.test(str.charAt(0)));
        if (re.test(str.charAt(0)) == false || /^[a-zA-Z0-9- ]*$/.test(str.charAt(0)) == false) {
            alert('Please start with alphabets!!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('RejectDescription').focus();
            return false;
        }

        var prv = false;
        next = false
        for (var i = 0; i < str.length; i++) {
            if (/^[a-zA-Z0-9- ]*$/.test(str.charAt(i)) == false) {
                if (prv == true) {
                    next = true;
                    break;
                }
                prv = true;
            }
            else {
                prv = false;
            }
        }

        if (next == true) {
            alert('Repetitive special characters are not allowed!!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('lblrjctds').style.display = "";
            //document.getElementById('RejectDescription')
            // document.getElementById('RejectDescription').focus();
            return false;
        }
    }
}


function IWLICQC() {
    // alert('abid');
    var cmsac = false;
    var IWdecn = document.getElementById('Decision').value.toUpperCase();
    var valcnt = document.getElementById('cnt').value;
    //----------------DBTAC-----------------------

    if (tt[valcnt].ScanningType == 3 || tt[valcnt].ScanningType == 5) {
        //-------------------Check CMSMaster Table------------
        $.ajax({
            url: RootUrl + 'OWL2/GetCMSAccount',
            dataType: 'html',
            data: { ac: $("#accnt").val() },
            success: function (cmsflg) {
                debugger;
                if (cmsflg == "false" || cmsflg == false) {
                    cmsac = true;
                    //alert('Account was not found in master !!');
                    //document.getElementById('accnt').focus();
                    //return false;
                }
            },
            async: false
        });

        if (cmsac == true) {
            alert('Account was not found in master !!');
            document.getElementById('accnt').focus();
            return false;
        }

        if (IWdecn != "F" && IWdecn != "R" && $("#ClientCd").val().toUpperCase() != "CMS") {
            randomPayeeName = document.getElementById('txtpayee').value;
        }
        else {//if ($("#ClientCd").val().toUpperCase() == "CMS")
            randomPayeeName = document.getElementById('Payee').value;
        }
    }
    else {
        randomPayeeName = document.getElementById('Payee').value;
    }

    var acmin = document.getElementById('acmin').value;
    //Acct = Acct.replace(/^0+/, '')

    //--------------- //-------------------------------------Validate Narration----On 07-02-2017-----------
    // narrationReqirdflg = false;
    var TempAcct = document.getElementById('accnt').value;
    if (TempAcct.length == 14) {
        if (TempAcct == "06410125027255") {
            document.getElementById('narsndiv').style.display = "";
            narrationReqirdflg = true;
        }
        else {
            narrationAC(TempAcct);
        }
    }

    if (narrationReqirdflg == true) {
        if ($("#nartext").val() == "") {
            alert('Please enter narration!');
            document.getElementById('nartext').focus();
            return false;
        }
        else if ($("#nartext").val().length < 4) {
            alert('Please enter minimum 3 characters in narration!');
            document.getElementById('nartext').focus();
            return false;
        }
    }

    if (tt[valcnt].InstrumentType == "S") {
        var Acct = document.getElementById('accnt').value;
        Acct = Acct.length
        if (Acct == "") {
            alert("Account no field should not be empty !");
            document.getElementById('accnt').focus();
            return false;
        }
        if (Acct < acmin) {

            alert("Minimum account no sould be " + acmin + " digits");
            document.getElementById('accnt').focus();
            return false;
        }
        Acct = document.getElementById('accnt').value.replace(/^0+/, '')
        if (Acct == "") {
            alert("Invalid Account Number!");
            document.getElementById('accnt').focus();
            return false;
        }
        //if (cbsbtn == false) {
        //    alert("Click on 'GetDetails' button/press F12 !");
        //    document.getElementById('accnt').focus();
        //    return false;
        //}

        //----------------------------Amount---------------------//
        amt = document.getElementById('slpamount').value;
        //alert(amt);   
        var intcont = 0;
        for (var i = 0; i < amt.length; i++) {

            if (amt.charAt(i) == ".") {
                intcont++;
            }
            if (intcont > 1) {
                alert('Enter valid amount!');
                document.getElementById('slpamount').focus();

                return false;
            }
        }

        if (amt == "NaN") {
            alert('Enter valid amount!');
            document.getElementById('slpamount').focus();

            return false;
        }

        amt1 = amt;
        var amtval = amt;
        amtval = amtval.replace(/^0+/, '')
        amt = amt.replace(/^0+/, '')
        amt = amt.replace(/,/g, '')
        amt = amt.length;
        if (amtval == ".") {
            alert('Amount field should not be dot(.) !');
            document.getElementById('slpamount').focus();
            return false;
        }
        else if (amtval == ".0") {
            alert('Amount not valid !');

            return false;
        }
        else if (amtval == ".00") {
            alert('Amount not valid !');

            return false;
        }
        else if (amtval == "00.00") {
            alert('Amount not valid !');

            return false;
        }
        else if (amtval == "0.00") {
            alert('Amount field should not be zero(0) !');

            return false;
        }
        else if (amt < 1) {
            alert('Amount field should not be empty !');
            document.getElementById('slpamount').focus();
            return false;
        }
        else if (amt > 15) {
            alert('Amount not valid !');
            document.getElementById('slpamount').focus();
            return false;
        }
    }
    if (tt[valcnt].InstrumentType == "C") {
        //----------------------------Amount---------------------//
        amt = document.getElementById('Amt').value;
        //alert(amt);   
        var intcont = 0;
        for (var i = 0; i < amt.length; i++) {

            if (amt.charAt(i) == ".") {
                intcont++;
            }
            if (intcont > 1) {
                alert('Enter valid amount!');
                document.getElementById('Amt').focus();

                return false;
            }
        }

        if (amt == "NaN") {
            alert('Enter valid amount!');
            document.getElementById('Amt').focus();

            return false;
        }

        amt1 = amt;
        var amtval = amt;
        amtval = amtval.replace(/^0+/, '')
        amt = amt.replace(/^0+/, '')
        amt = amt.replace(/,/g, '')
        amt = amt.length;

        if (amtval == ".") {
            alert('Amount field should not be dot(.) !');
            document.getElementById('Amt').focus();
            return false;
        }
        else if (amtval == ".0") {
            alert('Amount not valid !');

            return false;
        }
        else if (amtval == ".00") {
            alert('Amount not valid !');

            return false;
        }
        else if (amtval == "00.00") {
            alert('Amount not valid !');

            return false;
        }
        else if (amtval == "0.00") {
            alert('Amount field should not be zero(0) !');

            return false;
        }
        else if (amtval == "00.00") {
            alert('You can not accept this amount checque) ! ' + amt1);

            return false;
        }
        //else if (amt1 == "0.01") {
        //    alert('Amount field should not be zero(0) ! ' + amt1);

        //    return false;
        //}
        else if (amt < 1) {
            alert('Amount field should not be empty !');
            document.getElementById('Amt').focus();
            return false;
        }
        else if (amt > 15) {
            alert('Amount not valid !');
            document.getElementById('Amt').focus();
            return false;
        }
        //-------------------------ChqDate-------------------------------------------------//

        var dd, mm, yy;
        dat = document.getElementById('ChqDate').value;
        var chqdt = document.getElementById('ChqDate').value;
        if (chqdt.length <= 0 || chqdt.length < 2) {
            alert('Please enter correct Date!');
            document.getElementById('ChqDate').focus();
            return false;
        }
        if (dat == "") {
            //alert('aila');
            alert("Date field should not be empty !");
            document.getElementById('ChqDate').focus();
            document.getElementById('ChqDate').select();
            return false;
        }
        else if ((dat.toUpperCase().charAt(dat.length - 1) == "X") && (IWdecn.toUpperCase() == "A")) {
            alert("Date is not correct, Please reject this cheque!!");
            document.getElementById('IWDecision').focus();
            return false;
        }
            //else if (dat == "000000") {
            //    alert("Date not valid !");
            //    document.getElementById('ChqDate').focus();
            //    document.getElementById('ChqDate').select();
            //    return false;
            //}
            // else if (dat.length < 2 || dat.length == 3 || dat.length == 5) {
        else if (dat.length < 6) {
            alert("Date not valid !");
            document.getElementById('ChqDate').focus();
            document.getElementById('ChqDate').select();
            return false;
        }
        else {


            finldat = new String(dat);
            //alert(dat.length);
            if (dat.length == 2) {

                dd = finldat.substring(0, 2)
            }
            else if (dat.length == 4) {

                dd = finldat.substring(0, 2)
                mm = finldat.substring(2, 4)
            }
            else if (dat.length == 6) {

                dd = finldat.substring(0, 2)
                mm = finldat.substring(2, 4)
                yy = finldat.substring(4, 6)
            }

            if (IWdecn == "A" || (IWdecn == "R" && $("#ChqDate").val() != "000000")) {
                var onlydate = dd + '/' + mm + '/' + '20' + yy;


                var rtn = validatedate(onlydate);
                if (rtn == false) {
                    document.getElementById('ChqDate').focus();
                    document.getElementById('ChqDate').select();
                    return false;
                }
            }

        }

        ///------------------------------------Post Date and Stale Cheques ----///


        var postdate = document.getElementById('postdt').value;
        var staledate = document.getElementById('staledt').value;

        var fnldate;
        if (finldat.length < 10) {
            fnldate = '20' + yy + '/' + mm + '/' + dd;
        }
        else {
            fnldate = finldat;
        }


        var staldat = new Date(staledate);
        var postdat = new Date(postdate);
        var d3 = new Date(fnldate);

        //var timeDiff = Math.abs(staldat.getTime() - d3.getTime());
        //var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

        if (IWdecn == "A") {
            if (postdat <= d3) {
                alert('Post date!!');
                document.getElementById('ChqDate').focus();
                document.getElementById('ChqDate').select();
                return false;
            }
            if (staldat >= d3) {
                alert('Stale Cheque!!');
                document.getElementById('ChqDate').focus();
                document.getElementById('ChqDate').select();
                return false;
            }
        }
        ////------------------------Sort code---------------

        var ChqnoQC = document.getElementById('ChqnoQC').value;
        var SortQC = document.getElementById('SortQC').value;
        var SANQC = document.getElementById('SANQC').value;
        var TransQC = document.getElementById('TransQC').value;

        if (ChqnoQC.length <= 0 || ChqnoQC.length < 6) {
            alert('Please enter cheque No!');
            document.getElementById('ChqnoQC').focus();
            return false;
        }
        else if (ChqnoQC == "") {
            //alert('aila');
            alert("Cheque no should not be empty !");
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();
            return false;
        }
        else if (SortQC.length <= 0 || SortQC.length < 9) {
            alert('Please enter sort code!');
            document.getElementById('SortQC').focus();
            return false;
        }
        else if (SortQC == "") {
            //alert('aila');
            alert("Sort code should not be empty !");
            document.getElementById('SortQC').focus();
            document.getElementById('SortQC').select();
            return false;
        }
        else if (SANQC.length <= 0 || SANQC.length < 6) {
            alert('Please enter san No!');
            document.getElementById('SANQC').focus();
            return false;
        }
        else if (SANQC == "") {
            alert("SAN code should not be empty !");
            document.getElementById('SANQC').focus();
            document.getElementById('SANQC').select();
            return false;
        }
        else if (TransQC.length <= 0 || TransQC.length < 2) {
            alert('Please enter trans code!');
            document.getElementById('TransQC').focus();
            return false;
        }
        else if (TransQC == "") {
            alert("Trans code should not be empty !");
            document.getElementById('TransQC').focus();
            document.getElementById('TransQC').select();
            return false;
        }
        else if (ChqnoQC.length < 6) {
            alert("Cheque no is not valid !");
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();
            return false;
        }
        else if (SortQC.length < 9) {
            alert("Sort code no is not valid !");
            document.getElementById('SortQC').focus();
            document.getElementById('SortQC').select();
            return false;
        }
        else if (SANQC.length < 6) {
            alert("SAN code no is not valid !");
            document.getElementById('SANQC').focus();
            document.getElementById('SANQC').select();
            return false;
        }
        else if (TransQC.length < 2) {
            alert("Trans code no is not valid !");
            document.getElementById('TransQC').focus();
            document.getElementById('TransQC').select();
            return false;
        }
        else if (ChqnoQC.length < 6 || ChqnoQC == "000000") {
            alert("Cheque no is not valid !");
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();
            return false;
        }
        else if (SortQC.length < 9 || SortQC == "000000000") {
            alert("Sort code no is not valid !");
            document.getElementById('SortQC').focus();
            document.getElementById('SortQC').select();
            return false;
        }
        else if (SANQC.length < 6) {
            alert("SAN code no is not valid !");
            document.getElementById('SANQC').focus();
            document.getElementById('SANQC').select();
            return false;
        }
        else if (ChqnoQC.length < 6 || ChqnoQC == "000000" || isNaN(ChqnoQC)) {
            alert("Cheque no is not valid !");
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();
            return false;
        }
        else if (TransQC.length < 2 || TransQC == "00" || TransQC.substring(0, 1) == "0") {
            alert("Trans code is not valid !");
            document.getElementById('TransQC').focus();
            document.getElementById('TransQC').select();
            return false;
        }
        var rtnflg = validYrnscodes();
        if (rtnflg == false) {
            alert("Trans code is not valid !");
            document.getElementById('TransQC').focus();
            document.getElementById('TransQC').select();
            return false;
        }
    }
    //if (IWdecn == "A" && document.getElementById('blockkey') == "1") {
    //    event.preventDefault();
    //    alert('You can not accept this check!');
    //    return false;
    //}
    //alert(document.getElementById('IWRemark').value + " IWLICQC");
    //var temprtn = document.getElementById('IWRemark').value;
    // alert(temprtn);
    if (IWdecn == "") {
        alert('Please enter decision!');
        document.getElementById('Decision').focus();
        return false;
    }

    else if (IWdecn != "A" && IWdecn != "R" && IWdecn != "F") {
        // alert(IWdecn);
        alert('Decision not correct!');
        document.getElementById('Decision').focus();
        return false;
    }
    else if (IWdecn == "R") {
        if (document.getElementById('IWRemark').value == "") {
            alert('Please enter reject reason!');
            document.getElementById('IWRemark').focus();
            return false;
        }

        else if (document.getElementById('IWRemark').value == "88") {

            var dscflg = valid(document.getElementById('rejectreasondescrpsn').value, document.getElementById('IWRemark').value);
            if (dscflg == false) {
                // alert('In valid reject reason!');
                document.getElementById('rejectreasondescrpsn').focus();
                return false;
            }
        }
        var flg = false;
        var iwrked = document.getElementById('IWRemark').value;
        ///-----------------------------------------------
        if (iwrked.length == 1) {
            //-------------------------
            var pad = "0"
            iwrked = pad.substring(0, iwrked.length) + iwrked;
            //-------------------------
        }
        ///--------------------------------------------------

        var rjctresnl = document.getElementById('rtnlist');
        for (var i = 0; i < rjctresnl.length; i++) {

            if (iwrked == rjctresnl[i].value) {
                flg = true;
                // alert(rjctresnl[i].value);
                break;
            }
        }
        if (flg == false) {
            alert('In valid reject reason!');
            document.getElementById('IWRemark').focus();
            return false;
        }
    }
}
//---------------------------------------------
function validatedate(inputText) {
    // debugger;

    var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    // Match the date format through regular expression  
    if (inputText.match(dateformat)) {
        //document.form1.text1.focus();
        //Test which seperator is used '/' or '-'  
        var opera1 = inputText.split('/');
        var opera2 = inputText.split('-');
        lopera1 = opera1.length;
        lopera2 = opera2.length;
        // Extract the string into month, date and year  
        if (lopera1 > 1) {
            var pdate = inputText.split('/');
        }
        else if (lopera2 > 1) {
            var pdate = inputText.split('-');
        }
        var dd = parseInt(pdate[0]);
        var mm = parseInt(pdate[1]);
        var yy = parseInt(pdate[2]);
        // Create list of days of a month [assume there is no leap year by default]  
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (mm == 1 || mm > 2) {
            if (dd > ListofDays[mm - 1]) {
                alert('Invalid date!');
                return false;
            }
        }
        if (mm == 2) {
            var lyear = false;
            if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                lyear = true;
            }
            if ((lyear == false) && (dd >= 29)) {
                alert('Invalid date!');
                return false;
            }
            if ((lyear == true) && (dd > 29)) {
                alert('Invalid date!');
                return false;
            }
        }
    }
    else {
        alert("Invalid date !");
        //  document.form1.text1.focus();
        return false;
    }
    //  return true;
}
//--------------------------

function IWVef() {
    //rtncd
    document.getElementById('rtncd').style.display = "none";


    chr = document.getElementById('Decision').value.toUpperCase();
    var chr = document.getElementById('Decision').value.toUpperCase();
    var iwrk = document.getElementById('IWRemark').value;
    if (chr == "R") {
        if (iwrk == "") {
            document.getElementById('rtncd').style.display = "";
            document.getElementById('IWRemark').style.width = "10%";
            document.getElementById('IWRemark').focus();
        }
        else {
            // alert('aila');
            document.getElementById('rtncd').style.display = "";
            document.getElementById('IWDecision').focus();
        }
    }
    else {
        document.getElementById('rtncd').style.display = "none";
    }
}


function fullImage() {
    //  alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}
function ChangeImage(imagetype) {
    // alert(imagetype);
    var indexcnt = document.getElementById('cnt').value;
    if (imagetype == "FTiff") {

        document.getElementById('myimg').src = tt[indexcnt].FrontTiffImagePath;
    }
    else if (imagetype == "BTiff") {
        //alert('Browser not supporting!!!');
        document.getElementById('myimg').src = tt[indexcnt].BackTiffImagePath;
    }
    else if (imagetype == "FGray") {

        document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath;
    }

}


function CalAmt() {
    if ($("#Amt").val() != null || $("#Amt").val() != "") {
        ChequeAmountTotal = parseFloat(parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) - parseFloat(String(tempAmtValue).replace(/,/g, '')));
        ChequeAmountTotal = (parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) + parseFloat($("#Amt").val().replace(/,/g, '')));
        //ChequeAmountTotal.toFixed(1));
        document.getElementById('totamt').innerHTML = ChequeAmountTotal;
        tempAmtValue = parseFloat($("#Amt").val().replace(/,/g, ''));
    }
}

function narrationAC(curntAc) {
    narrationReqirdflg = false;
    var narsnac = document.getElementById('narration');
    var actualAC = curntAc.slice(-10)

    for (var i = 0; i < narsnac.length; i++) {
        //alert(rtncode);
        if (actualAC != narsnac[i].value && $("#ofc").val() == "1") {
            //  alert(rtnlistDescrp[i].value);
            document.getElementById('narsndiv').style.display = "";
            // document.getElementById('nartext').value = tt[1].UserNarration;
            narrationReqirdflg = true;
            break;
        }
    }
}

///----------------

function validYrnscodes() {
    var temptrnscd = document.getElementById('TransQC').value;
    var flg;
    $.ajax({

        url: RootUrl + "IWDataEntry/ValidTrans",
        data: { transcode: temptrnscd },
        dataType: "html",
        async: false,
        success: function (trnsresult) {
            if (trnsresult == "false") {
                alert('Trans code not valid!!');

                flg = false;
            }
            else {
                flg = true;
            }
        }

    });
    return flg;
}

function addCommas(amtval) {
    var flg = true;
    var count = 0;
    var finaloutpt = "";
    //        var nStr = document.getElementById("amt").value;
    var nStr = amtval;
    // alert(nStr);
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';

    var amount = new String(x1);
    amount = amount.split("").reverse();

    //        for (var i = x1.length-1; i>0; i--) {
    for (var i = 0; i <= amount.length - 1; i++) {
        // alert;
        //alert(amount[i]);
        finaloutpt = amount[i] + finaloutpt;
        // alert(finaloutpt);
        if (flg == true) {
            if (count == 2) {
                // alert('cool');
                flg = false;
                if (amount.length <= 3) {
                }
                else {
                    finaloutpt = "," + finaloutpt;
                }
                count = 0;
            }
        }
        else {
            if (count == 2 && i <= amount.length - 2) {
                // flg = false;
                //alert('aila');
                finaloutpt = "," + finaloutpt;
                count = 0;
            }
        }
        count = count + 1;
    }

    return (finaloutpt + x2);
}

function CalSLPAmt() {
    // debugger;

    if ($("#Scheme").val() == "SBTRS") {
        var tempslpamt = $("#slpamount").val();
        // var tempchqamt = $("#Amt").val();

        if (tempslpamt.replace(/,/g, "") >= 50000) {
            alert('Alert! Exceeds Rs. 50,5000, check the beneficiary\n if third party, to comply with RBI guidelines!!');
        }
    }
    else if ($("#Scheme").val() == "SUKAN") {
        var tempSamt = $("#slpamount").val();
        //var tempamt = $("#Amt").val();

        if (tempSamt.replace(/,/g, "") % 100 != 0 && tempSamt.replace(/,/g, "") > 150000) {
            document.getElementById("blockkey").value = "1";
            alert('Alert! Sukanya Samrudhi, Amount not valid!!');
        }
        else {
            document.getElementById("blockkey").value = "0";
        }
    }
}

function bankName(bankcode) {
    // alert('hello');
    $.ajax({
        url: RootUrl + 'OWL3/GetBankName',
        dataType: 'html',
        data: { bankcode: bankcode },
        success: function (databank) {
            //alert(data);
            $('#bankname').html(databank);
            //$('#dialogEditUser').dialog('open');
        }
    });
}

function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substr(0, index) + chr + str.substr(index + 1);
}

//-----------------------Slip Image Call----------------------24/07/2017-----------------//
function callslipimage() {
    var recordId = document.getElementById('tempcnt').value;
    //  debugger;
    $.ajax({
        url: RootUrl + 'OWL3/slipImage',
        dataType: 'html',
        data: { SlipId: tt[recordId].SlipID },
        success: function (Slipdata) {
            //  alert(Slipdata);
            // debugger;
            document.getElementById('myimg').src = Slipdata.replace('"', "").replace('"', "");
        },
        async: false
    });
}


