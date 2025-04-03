
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var nextcall;
var L1;
var cnt;
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
var narrationReqirdflg = false;
var narrationMandate = false;
var InstrumentType;
var strModified = "0000000000";
var tiffimagecall = false;

var getreason;
var getcbsdtls;
var tempdat;
var fnldate;
var yr, mm, dd;
//-----------------------------------------Declaration END-------------------

function passval(array) {

    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}

$(document).ready(function () {

    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].Id)
    }
    //------------ idslist end----------------//
    $('#accnt,#ClientCd,#Amt,#ChqDate,#ChqnoQC,#SortQC,#SANQC,#TransQC,#IWRemark,#nartext').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    //-----------------ShortCut----for CBS-----------
    $("#accnt").keydown(function (event) {
        if (event.keyCode == 8 || event.keyCode == 32 || event.keyCode == 46) {
            cbsbtn = false;
        }
        if (event.keyCode == 123) {
            getcbsdtls(); //CbsDetails
            return false;
        }
    });
    if (tt.length > 0) {
        debugger
        //  document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
        convertTiffImage(tt[1].FrontGreyImagePath);
        document.getElementById('nartext').value = "";
        InstrumentType = tt[1].InstrumentType;
        //-------Remove save objects from browser---//
        window.localStorage.clear();

        document.getElementById('bankname').style.display = "";
        document.getElementById('chqamt').style.display = "";
        document.getElementById('MICR').style.display = "";
        document.getElementById('slpacnt').style.display = "";
        document.getElementById('accnt').value = tt[1].CreditAccountNo;

        if (tt[1].ScanningType == 11) {
            $("#accnt").prop('disabled', true);
        }
        //--------------------Added On 07-02-2017------------------
        document.getElementById('oldact').value = tt[1].CreditAccountNo;

        document.getElementById('ChqnoQC').value = tt[1].ChequeNoFinal;
        document.getElementById('SortQC').value = tt[1].SortCodeFinal;
        document.getElementById('SANQC').value = tt[1].SANFinal;
        document.getElementById('TransQC').value = tt[1].TransCodeFinal;

        bankName(tt[1].SortCodeFinal);  //-------------For bank name
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
        debugger;
        //------------------------------------------------------------
        //Calling CAR To LAR-------------amtwrd-------------------
        amtwrd = number2text(tt[1].FinalAmount)
        document.getElementById('amtwrd').innerHTML = amtwrd;
        //------------------------------------------------------------

        document.getElementById('Amt').value = addCommas(Number(tt[1].FinalAmount).toFixed(2));
        document.getElementById('ChqDate').value = fnldate;

        //-------------------Added ON 28-12-2020----By Abid------
        document.getElementById('brAccnt').value = tt[1].BranchAccount;
        document.getElementById('brAmt').value = tt[1].BranchAmount;

        document.getElementById('divctsnoncts').style.display = "";

        $('#ctsnocts').val(tt[1].ClearingType);
        if (tt[1].ScanningType == 11) {
            document.getElementById('ChqDate').focus();
        }
        else {
            if (document.getElementById("vftype").value == "CHQATVP") {
                document.getElementById('Decision').focus();
            }
            else {
                document.getElementById('accnt').focus();
            }

        }

        //----------------Modification --------Highlights----------------
        if (tt[1].RejectedField == "1") {
            document.getElementById("Amt").style.backgroundColor = "red";
        }
        else {
            document.getElementById("Amt").style.backgroundColor = "white";
        }
        if (tt[1].RejectedField == "2") {
            document.getElementById("ChqDate").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqDate").style.backgroundColor = "white";
        }
        if (tt[1].RejectedField == "3") {
            document.getElementById("accnt").style.backgroundColor = "red";
        }
        else {
            document.getElementById("accnt").style.backgroundColor = "white";
        }
        if (tt[1].RejectedField == "4") {
            document.getElementById("ChqnoQC").style.backgroundColor = "red";
            document.getElementById("SortQC").style.backgroundColor = "red";
            document.getElementById("SANQC").style.backgroundColor = "red";
            document.getElementById("TransQC").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqnoQC").style.backgroundColor = "white";
            document.getElementById("SortQC").style.backgroundColor = "white";
            document.getElementById("SANQC").style.backgroundColor = "white";
            document.getElementById("TransQC").style.backgroundColor = "white";
        }
        if (tt[1].RejectedField == "5") {
            document.getElementById("frmL1").classList.add("w3-highway-red");
        }
        else {
            document.body.style.backgroundColor = "white";
        }
        //-------------Account-----------------------
        if (document.getElementById("vftype").value == "CHQATVF") {
            //-------------Account-----------------------
            if (tt[1].ATVAccountPass.trim() == "N") {
                document.getElementById("accnt").style.backgroundColor = "red";
            }
            else {
                document.getElementById("accnt").style.backgroundColor = "white";
            }
            //-------------Amount------------------------------------------------------------------
            if (tt[1].ATVAmountPass.trim() == "N") {
                document.getElementById("Amt").style.backgroundColor = "red";
            }
            else {
                document.getElementById("Amt").style.backgroundColor = "white";
            }
            //-------------ChqDate-----------------------------------------------------------------
            if (tt[1].ATVDatePass.trim() == "N") {
                document.getElementById("ChqDate").style.backgroundColor = "red";
            }
            else {
                document.getElementById("ChqDate").style.backgroundColor = "white";
            }
            //-------------ChqNo--------------------------------------------------------------------
            if (tt[1].ATVMICRPasss.trim() == "N") {
                document.getElementById("ChqnoQC").style.backgroundColor = "red";
                document.getElementById("SortQC").style.backgroundColor = "red";
                document.getElementById("SANQC").style.backgroundColor = "red";
                document.getElementById("TransQC").style.backgroundColor = "red";
            }
            else {
                document.getElementById("ChqnoQC").style.backgroundColor = "white";
                document.getElementById("SortQC").style.backgroundColor = "white";
                document.getElementById("SANQC").style.backgroundColor = "white";
                document.getElementById("TransQC").style.backgroundColor = "white";
            }
        }
        //--------------------------END----------------------------------
        cbsbtn = false;
        if ($("#accnt").val() != "") {

            $.ajax({
                url: RootUrl + 'OWL1/GetCBSDtls_New',
                dataType: 'html',
                data: { ac: $("#accnt").val() },
                success: function (data) {
                    cbsbtn = true;
                    $('#cbsdetails').html(data);
                    $("#Payee option:first").attr('selected', 'selected');
                }
            });
        }

        //--------------------------------------------------------------
        document.getElementById('strbranchcd').innerHTML = tt[1].BranchCode;
        document.getElementById('ScanningID').innerHTML = tt[1].ScanningNodeId;
        document.getElementById('strBatchNo').innerHTML = tt[1].BatchNo;
        document.getElementById('strBatchSeqNO').innerHTML = tt[1].BatchSeqNo;
        //--------------------------------------------------------------

        document.getElementById("btnback").disabled = true
        document.getElementById('rejectreasondescrpsn').value = "";
    }

    $("#ok").click(function () {

        nextcall = false;
        debugger;

        var result = IWCQL1();

        if (result == false) {

            return false;
        }
        else {
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var owL1 = "owL1";
            var btnval = document.getElementById('Decision').value.toUpperCase();

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
                owL1 = owL1 + backcnt
                L1 = {
                    "CreditAccountNo": $("#accnt").val(),

                    "BatchNo": tt[backcnt].BatchNo,
                    "ClearingType": tt[backcnt].ClearingType,
                    "InstrumentType": tt[backcnt].InstrumentType,
                    "ProcessingDate": tt[backcnt].ProcessingDate,
                    "BranchCode": tt[backcnt].BranchCode,
                    "ScanningNodeId": tt[backcnt].ScanningNodeId,
                    "Id": tt[backcnt].Id,
                    "Action": btnval,
                    "Status": tt[backcnt].Status,
                    "RawDataId": tt[backcnt].RawDataId,
                    "RejectReason": $("#IWRemark").val(),
                    "DomainId": tt[backcnt].DomainId,
                    "CustomerId": tt[backcnt].CustomerId,
                    "CBSAccountInformation": $("#cbsdls").val(),
                    "CBSJointAccountInformation": $("#JoinHldrs").val(),
                    "FinalAmount": parseFloat($("#Amt").val().replace(/,/g, '')),
                    "FinalDate": $("#ChqDate").val(),
                    "ChequeNoFinal": $("#ChqnoQC").val(),
                    "SortCodeFinal": $("#SortQC").val(),
                    "SANFinal": $("#SANQC").val(),
                    "TransCodeFinal": $("#TransQC").val(),
                    "ChequeAmountTotal": tt[backcnt].ChequeAmountTotal,
                    "PayeeName": randomPayeeName,// $("#Payee").val()
                    "UserNarration": "0",
                    "rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                    "ctsNonCtsMark": $("#ctsnocts").val(),
                    "Modified": strModified,
                    "ScanningType": tt[backcnt].ScanningType,
                };
            }

            else {
                if (btnval == "R") {
                    cnrslt = false;
                    //-------------------------valid Function for validation---------------
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

                    cnrslt = true;
                    nextcall = true;
                }

                owL1 = owL1 + cnt;
                L1 = {
                    "CreditAccountNo": $("#accnt").val(),
                    "BatchNo": tt[tempcnt].BatchNo,
                    "ClearingType": tt[tempcnt].ClearingType,
                    "InstrumentType": tt[tempcnt].InstrumentType,
                    "ProcessingDate": tt[tempcnt].ProcessingDate,
                    "BranchCode": tt[tempcnt].BranchCode,
                    "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                    "Id": tt[tempcnt].Id,
                    "Action": btnval,
                    "Status": tt[tempcnt].Status,
                    "RawDataId": tt[tempcnt].RawDataId,
                    "RejectReason": $("#IWRemark").val(),
                    "DomainId": tt[tempcnt].DomainId,
                    "CustomerId": tt[tempcnt].CustomerId,
                    "CBSAccountInformation": $("#cbsdls").val(),
                    "CBSJointAccountInformation": $("#JoinHldrs").val(),
                    "FinalAmount": parseFloat($("#Amt").val().replace(/,/g, '')),
                    "FinalDate": $("#ChqDate").val(),
                    "ChequeNoFinal": $("#ChqnoQC").val(),
                    "SortCodeFinal": $("#SortQC").val(),
                    "SANFinal": $("#SANQC").val(),
                    "TransCodeFinal": $("#TransQC").val(),
                    "ChequeAmountTotal": tt[tempcnt].ChequeAmountTotal,
                    "PayeeName": randomPayeeName,// $("#Payee").val(),
                    "UserNarration": "0",
                    "rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                    "ctsNonCtsMark": $("#ctsnocts").val(),
                    "Modified": strModified,
                    "ScanningType": tt[tempcnt].ScanningType,

                };
            }

            if (nextcall == true) {

                common(owL1);
            }
            else {
                document.getElementById('accnt').focus();
                document.getElementById("btnback").disabled = true;
            }


        }
    });
    //---------------------------------------------------------------------------
    $("#accnt").focusout(function () {
        //  debugger;
        Foutcnt = document.getElementById('cnt').value;

        if (tt[Foutcnt].CreditAccountNo != $("#accnt").val()) {

            strModified = setCharAt(strModified, 0, '1');
        }
        else {
            strModified = setCharAt(strModified, 0, '0');
        }
    });


    $("#Amt").focusout(function () {
        //  debugger;
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].FinalAmount != parseFloat($("#Amt").val().replace(/,/g, ''))) {
            strModified = setCharAt(strModified, 2, '1');
        }
        else {
            strModified = setCharAt(strModified, 2, '0');
        }
    });
    $("#ChqDate").focusout(function () {
        // debugger;
        if (fnldate != $("#ChqDate").val()) {
            strModified = setCharAt(strModified, 3, '1');
        }
        else {
            strModified = setCharAt(strModified, 3, '0');
        }
    });
    $("#ChqnoQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].ChequeNoFinal != $("#ChqnoQC").val()) {
            strModified = setCharAt(strModified, 4, '1');
        }
        else {
            strModified = setCharAt(strModified, 4, '0');
        }
    });
    $("#SortQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].SortCodeFinal != $("#SortQC").val()) {
            strModified = setCharAt(strModified, 5, '1');
        }
        else {
            strModified = setCharAt(strModified, 5, '0');
        }
    });
    $("#SANQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].SANFinal != $("#SANQC").val()) {
            strModified = setCharAt(strModified, 6, '1');
        }
        else {
            strModified = setCharAt(strModified, 6, '0');
        }
    });
    $("#TransQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].TransCodeFinal != $("#TransQC").val()) {
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
    //---------------------------------------------------------------------------

    //-------------------------------------Reject--------------------------------//   
    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;

        if (Modernizr.localstorage) {

            backbtn = true;
            var owL1 = "owL1"
            cnt = document.getElementById('cnt').value;
            owL1 = owL1 + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(owL1));

            document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
            // document.getElementById('slpamt').style.display = "none";
            document.getElementById('chqamt').style.display = "";
            document.getElementById('Chqacnt').style.display = "";
            //document.getElementById('chequeAcct').innerHTML = "";
            document.getElementById('slpacnt').style.display = "none";
            document.getElementById('MICR').style.display = "";
            document.getElementById('ChqDate').value = "";
            document.getElementById('Amt').value = "";
            document.getElementById('ChqnoQC').value = "";
            document.getElementById('SortQC').value = "";
            document.getElementById('SANQC').value = "";
            document.getElementById('TransQC').value = "";
            // document.getElementById('chequeAcct').innerHTML = orderData.CreditAccountNo;
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
                for (i = i; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL1" + i));
                    //alert(orderData.Id);
                    if (orderData.Id != null) {
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.CreditAccountNo);
                        arrlist.push(orderData.BatchNo);
                        arrlist.push(orderData.ProcessingDate);
                        arrlist.push(orderData.InstrumentType);
                        arrlist.push(orderData.BranchCode);
                        arrlist.push(orderData.ClearingType);
                        arrlist.push(orderData.ScanningNodeId);
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
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.rejectreasondescrpsn);
                        arrlist.push(orderData.ctsNonCtsMark);
                        arrlist.push(orderData.Modified);
                        arrlist.push(orderData.ScanningType);
                    }

                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL1" + i));
                    if (orderData.Id != null) {
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.CreditAccountNo);
                        arrlist.push(orderData.BatchNo);
                        arrlist.push(orderData.ProcessingDate);
                        arrlist.push(orderData.InstrumentType);
                        arrlist.push(orderData.BranchCode);
                        arrlist.push(orderData.ClearingType);
                        arrlist.push(orderData.ScanningNodeId);
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
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.rejectreasondescrpsn);
                        arrlist.push(orderData.ctsNonCtsMark);
                        arrlist.push(orderData.Modified);
                        arrlist.push(orderData.ScanningType);
                    }

                }
            }
            //------------------------------- Calling Ajax for taking more data------------------          
            $.ajax({

                url: RootUrl + 'OWL1/OWChqL1',
                data: JSON.stringify({ lst: arrlist, snd: scond, btnClose: "Close", idlst: idslst, ChequeAmountTotal: 0 }),

                type: 'POST',
                contentType: 'application/json; charset=utf-8',

                dataType: 'json',
                success: function (result) {
                    if (result == false) {
                        window.location = RootUrl + 'Home/IWIndex';
                    }
                }

            });
        }
    });
    //------------------------------------------------------------
    var value = 0;
    callrotate = function () {
        value += 180;
        if (tiffimagecall == true) {
            $("#Tfmyimg").rotate({ animateTo: value })
        }
        else {
            $("#myimg").rotate({ animateTo: value })
        }

    }
    //---------------- Data Entry -----------------------------------
    //----------------narration--------------------
    $("#nartext").keypress(function (event) {

        if (event.shiftKey) {
            event.preventDefault();
        }
        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 32 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 45 || event.keyCode == 47 || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            event.preventDefault();
        }
    });
    //------------------
    $("#ChqDate").keypress(function (event) {

        if (event.shiftKey) {
            event.preventDefault();
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.charCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            event.preventDefault();
        }
    });
    //---------------------------------------------
    //--------------------------GET Image On Shorcut Key-------------------
    $(document).keydown(function (e) {
        if (e.keyCode == 113) {//---F2
            ChangeImage('FTiff');
        }
        if (e.keyCode == 115) {//------F4
            ChangeImage('FGray');
        }
        if (e.keyCode == 117) {//-----F6
            ChangeImage('BTiff');
        }
        if (e.keyCode == 118) {//-----F7
            callrotate();
        }
    });
    //------------------------------------END--------------------------------
    //---------------------------------------------

    $("form input").keydown(function (e) {
        next_idx = $('input[type=text]').index(this) + 1;
        tot_idx = $('body').find('input[type=text]').length;
        var tempcounter = document.getElementById('cnt').value;

        debugger;
        if (e.keyCode == 13) {
            //--------------------ATV---------
            if (document.getElementById("vftype").value == "CHQATVP") {
                if (next_idx == 1) {

                    var tempacntno = document.getElementById('oldact').value;
                    if ($("#accnt").val() == "") {
                        alert("Please enter account number!!");
                        $("#accnt").focus();
                        return false;
                    }
                    if (tempacntno != $("#accnt").val()) {

                        document.getElementById('oldact').value = $("#accnt").val();
                        getcbsdtls();
                    }
                    else {

                        next_idx = 9;
                        $('input[type=text]:eq(' + next_idx + ')').focus().select();
                    }
                }
                else if (next_idx == 2) {//4
                    next_idx = tot_idx;
                }
                else if (next_idx == 3) {//prev-5
                    if ($("#IWRemark").val() != 88) {
                        next_idx = 1;//prev-3
                    }
                }
                else {
                    next_idx = 1;//3
                }

                if (tot_idx == next_idx) {
                    $("input[value='Ok']").click();
                }
                else {
                    $('input[type=text]:eq(' + next_idx + ')').focus().select();
                }

            }
                //--------------------------------
            else {
                if (next_idx == 1) {

                    var tempacntno = document.getElementById('oldact').value;
                    if ($("#accnt").val() == "") {
                        alert("Please enter account number!!");
                        $("#accnt").focus();
                        return false;
                    }
                    if (tempacntno != $("#accnt").val()) {

                        document.getElementById('oldact').value = $("#accnt").val();
                        getcbsdtls();
                    }
                    else {

                        next_idx = 9;
                        $('input[type=text]:eq(' + next_idx + ')').focus().select();
                    }
                }
                else if (next_idx == 5) {//prev-9
                    next_idx = 1;
                }
                else if (next_idx == 4) {// prev-8
                    next_idx = 1;
                }
                else if (next_idx == 1) {//pre-5

                    next_idx = next_idx + 5;//pre-4
                }
                else if (next_idx == 11) {//13

                    next_idx = 1;//-prev- 7
                }
                else if (next_idx == 9) {

                    next_idx = 1;//-prev- 5
                }
                else if (next_idx == 2) {//--prev-6
                    next_idx = tot_idx;
                }
                else if (next_idx == 3) {//prev-7
                    if ($("#IWRemark").val() != 88) {
                        next_idx = 1;//prev-5
                    }
                }

                if (e.keyCode == 13) {

                    if (tot_idx == next_idx) {
                        $("input[value='Ok']").click();
                    }
                    else {
                        $('input[type=text]:eq(' + next_idx + ')').focus().select();
                    }
                }
            }

        }
        // }


    });
    //------------------END--------------------

    $("#accnt").keypress(function (event) {
        if (event.shiftKey) {
            event.preventDefault();
        }

        if ($("#allowAlpha").val() == "1") {
            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 96 && event.charCode < 123)) {
                cbsbtn = false;
            }
            else {
                event.preventDefault();
            }
        }
        else {
            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
                cbsbtn = false;
            }
            else {
                event.preventDefault();
            }
        }

    });

    $("#ChqnoQC,#SortQC,#SANQC,#TransQC,#IWRemark").keypress(function (event) {

        if (event.shiftKey) {
            event.preventDefault();
        }
        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            event.preventDefault();
        }
    });
    $("#Decision").keydown(function (event) {

        tempcnt = document.getElementById('tempcnt').value;

        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }
        if (document.getElementById("blockkey").value == "1") {
            if (event.keyCode == 65 || event.keyCode == 97) {
                event.preventDefault();
                alert('You can not accept the cheque!');
                return false;
            }
            if (event.keyCode == 65 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

            }
            else {
                event.preventDefault();
            }
        }
        else {

            if (event.keyCode == 65 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

            }
            else {
                event.preventDefault();
            }
        }
    });
    //----------Amount---------
    $("#Amt").keypress(function (event) {

        if (event.shiftKey) {
            event.preventDefault();
        }
        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || event.charCode == 46 || (event.charCode > 47 && event.charCode < 58)) {
            var amtval = this.value;
            if (amtval.length > 0) {
                var splitstr = amtval.split('.');
                if (splitstr[1] != null) {
                    var strlength = splitstr[1].length;
                    if (strlength > 1) {
                        event.preventDefault();
                    }
                }
            }
        }
        else {
            event.preventDefault();
        }
    });

    //------------rtndescrp---------------------------------

    $("#IWRemark").keyup(function (event) {
        var chkcode = false;
        var rejctrcd = $("#IWRemark").val();
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


    //-------------------------------Common For Accept and Reject-----------------------------
    function common(val) {
        if (Modernizr.localstorage) {
            var localacct = window.localStorage;
            var chqiwmicr = JSON.stringify(L1);
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

            if (tt[1].ScanningType == 11) {
                $("#accnt").prop('disabled', true);
            }

            //-------------added on 08-04-2019----------------
            var Tfimg = document.getElementById('Tfmyimg');
            if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                document.getElementById('tiffimg').style.display = "";
                // document.getElementById("myimg").style.display = "";
            }
            //-------------END----------------

            document.getElementById('myimg').removeAttribute('src');
            if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                document.getElementById('myimg').removeAttribute('Tfmyimg');
            }


            //document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
            convertTiffImage(tt[cnt].FrontGreyImagePath);

            document.getElementById('Decision').value = "";
            document.getElementById('IWRemark').value = "";
            document.getElementById('rtncd').style.display = "none";
            $("#cbsdetails").empty();

            strModified = "0000000000";
            document.getElementById('MICR').style.display = "";
            document.getElementById('ChqDate').value = "";
            document.getElementById('Amt').value = "";
            document.getElementById('ChqnoQC').value = "";
            document.getElementById('SortQC').value = "";
            document.getElementById('SANQC').value = "";
            document.getElementById('TransQC').value = "";
            InstrumentType = tt[cnt].InstrumentType;

            document.getElementById('divctsnoncts').style.display = "";
            $('#ctsnocts').val(tt[cnt].ClearingType);

            //--------------------------------------------------CheqDte---
            if (tt[cnt].FinalDate != "" || tt[cnt].FinalDate != null) {

                if (tt[cnt].FinalDate.length > 6) {
                    tempdat = tt[cnt].FinalDate.split("-");
                    yr = tempdat[0];
                    yr = yr.substring(2, 4);
                    mm = tempdat[1];
                    dd = tempdat[2];
                    fnldate = dd + mm + yr;
                }
                else {
                    tempdat = tt[cnt].FinalDate;
                    yr = tempdat.substring(4, 6);
                    mm = tempdat.substring(2, 4);
                    dd = tempdat.substring(0, 2);
                    fnldate = dd + mm + yr;
                }
            }
            else {
                fnldate = "";
            }

            //------------------------------------------------------------
            //Calling CAR To LAR-------------amtwrd-------------------
            amtwrd = number2text(tt[cnt].FinalAmount)
            document.getElementById('amtwrd').innerHTML = amtwrd;
            //------------------------------------------------------------

            document.getElementById('oldact').value = "";
            document.getElementById('accnt').value = "";
            document.getElementById('accnt').value = tt[cnt].CreditAccountNo;
            document.getElementById('Amt').value = addCommas(Number(tt[cnt].FinalAmount).toFixed(2));
            document.getElementById('ChqDate').value = fnldate;
            document.getElementById('ChqnoQC').value = tt[cnt].ChequeNoFinal;
            document.getElementById('SortQC').value = tt[cnt].SortCodeFinal;
            document.getElementById('SANQC').value = tt[cnt].SANFinal;
            document.getElementById('TransQC').value = tt[cnt].TransCodeFinal;
            //-------------------Added ON 28-12-2020----By Abid------
            document.getElementById('brAccnt').value = "";
            document.getElementById('brAmt').value = "";
            document.getElementById('brAccnt').value = tt[cnt].BranchAccount;
            document.getElementById('brAmt').value = tt[cnt].BranchAmount;
            //-------------------Added ON 28-12-2020----By Abid------END-

            if (tt[cnt].ScanningType == 11) {
                document.getElementById('ChqDate').focus();
            }
            else {//|| document.getElementById("vftype").value == "CHQATVF"
                if (document.getElementById("vftype").value == "CHQATVP") {
                    document.getElementById('Decision').focus();
                }
                else {
                    document.getElementById('accnt').focus();
                }
            }

            //----------------Modification --------Highlights----------------
            if (tt[cnt].RejectedField == "1") {
                document.getElementById("Amt").style.backgroundColor = "red";
            }
            else {
                document.getElementById("Amt").style.backgroundColor = "white";
            }
            if (tt[cnt].RejectedField == "2") {
                document.getElementById("ChqDate").style.backgroundColor = "red";
            }
            else {
                document.getElementById("ChqDate").style.backgroundColor = "white";
            }
            if (tt[cnt].RejectedField == "3") {
                document.getElementById("accnt").style.backgroundColor = "red";
            }
            else {
                document.getElementById("accnt").style.backgroundColor = "white";
            }
            if (tt[cnt].RejectedField == "4") {
                document.getElementById("ChqnoQC").style.backgroundColor = "red";
                document.getElementById("SortQC").style.backgroundColor = "red";
                document.getElementById("SANQC").style.backgroundColor = "red";
                document.getElementById("TransQC").style.backgroundColor = "red";
            }
            else {
                document.getElementById("ChqnoQC").style.backgroundColor = "white";
                document.getElementById("SortQC").style.backgroundColor = "white";
                document.getElementById("SANQC").style.backgroundColor = "white";
                document.getElementById("TransQC").style.backgroundColor = "white";
            }
            if (tt[cnt].RejectedField == "5") {
                document.getElementById("frmL1").classList.add("w3-highway-red");
            }
            else {
                document.body.style.backgroundColor = "white";
            }
            //-------------Account-----------------------
            if (document.getElementById("vftype").value == "CHQATVF") {
                //-------------Account-----------------------
                if (tt[cnt].ATVAccountPass.trim() == "N") {
                    document.getElementById("accnt").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("accnt").style.backgroundColor = "white";
                }
                //-------------Amount------------------------------------------------------------------
                if (tt[cnt].ATVAmountPass.trim() == "N") {
                    document.getElementById("Amt").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("Amt").style.backgroundColor = "white";
                }
                //-------------ChqDate-----------------------------------------------------------------
                if (tt[cnt].ATVDatePass.trim() == "N") {
                    document.getElementById("ChqDate").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("ChqDate").style.backgroundColor = "white";
                }
                //-------------ChqNo--------------------------------------------------------------------
                if (tt[cnt].ATVMICRPasss.trim() == "N") {
                    document.getElementById("ChqnoQC").style.backgroundColor = "red";
                    document.getElementById("SortQC").style.backgroundColor = "red";
                    document.getElementById("SANQC").style.backgroundColor = "red";
                    document.getElementById("TransQC").style.backgroundColor = "red";
                }
                else {
                    document.getElementById("ChqnoQC").style.backgroundColor = "white";
                    document.getElementById("SortQC").style.backgroundColor = "white";
                    document.getElementById("SANQC").style.backgroundColor = "white";
                    document.getElementById("TransQC").style.backgroundColor = "white";
                }
            }
            //----------------------END---------------------------------------------------//

            tempAmtValue = tt[cnt].FinalAmount;
            document.getElementById("btnback").disabled = true;//false
            document.getElementById('bankname').style.display = "";

            bankName(tt[cnt].SortCodeFinal);  //-------------For bank name

            cbsbtn = false;
            ////---------------------Filling cbsDetails------
            if (tt[cnt].CreditAccountNo != "") {

                $.ajax({
                    async: false,
                    url: RootUrl + 'OWL1/GetCBSDtls_New',
                    dataType: 'html',
                    data: { ac: $("#accnt").val() },
                    success: function (data) {
                        $('#cbsdetails').html(data);
                        cbsbtn = true;
                        $("#Payee option:first").attr('selected', 'selected');
                    }
                });
            }
            if (narrationReqirdflg == true) {
                document.getElementById('narsndiv').style.display = "";
            }
            else {
                document.getElementById('narsndiv').style.display = "none";
            }

            //--------------------------------------------------------------
            document.getElementById('strbranchcd').innerHTML = tt[cnt].BranchCode;
            document.getElementById('ScanningID').innerHTML = tt[cnt].ScanningNodeId;
            document.getElementById('strBatchNo').innerHTML = tt[cnt].BatchNo;
            document.getElementById('strBatchSeqNO').innerHTML = tt[cnt].BatchSeqNo;
            //-----------------------------------------------------------------------------------           
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

                if (scond == true) {
                    var i;
                    if (tt[0].callby == "Cheq") {
                        i = 0;
                    }
                    else {
                        i = 1;
                    }
                    for (i = i; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("owL1" + i));
                        if (orderData.Id != null) {
                            arrlist.push(orderData.Id);
                            arrlist.push(orderData.CreditAccountNo);
                            arrlist.push(orderData.BatchNo);
                            arrlist.push(orderData.ProcessingDate);
                            arrlist.push(orderData.InstrumentType);
                            arrlist.push(orderData.BranchCode);
                            arrlist.push(orderData.ClearingType);
                            arrlist.push(orderData.ScanningNodeId);
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
                            arrlist.push(orderData.UserNarration);
                            arrlist.push(orderData.rejectreasondescrpsn);
                            arrlist.push(orderData.ctsNonCtsMark);
                            arrlist.push(orderData.Modified);
                            arrlist.push(orderData.ScanningType);
                        }

                    }
                }
                else {

                    for (var i = 1; i < cnt; i++) {
                        var orderData = JSON.parse(localData.getItem("owL1" + i));

                        if (orderData.Id != null) {
                            arrlist.push(orderData.Id);
                            arrlist.push(orderData.CreditAccountNo);
                            arrlist.push(orderData.BatchNo);
                            arrlist.push(orderData.ProcessingDate);
                            arrlist.push(orderData.InstrumentType);
                            arrlist.push(orderData.BranchCode);
                            arrlist.push(orderData.ClearingType);
                            arrlist.push(orderData.ScanningNodeId);
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
                            arrlist.push(orderData.UserNarration);
                            arrlist.push(orderData.rejectreasondescrpsn);
                            arrlist.push(orderData.ctsNonCtsMark);
                            arrlist.push(orderData.Modified);
                            arrlist.push(orderData.ScanningType);
                        }

                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------                
                debugger;
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;

                $.ajax({

                    url: RootUrl + 'OWL1/OWChqL1',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[pcnt - 1].FrontGreyImagePath, idlst: idslst, ChequeAmountTotal: 0 }),

                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    async: false,
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

                                //-------------Saving Last data in storage---
                                var owL1 = "owL10"

                                InstrumentType = tt[1].InstrumentType;

                                var L1 = {
                                    "CreditAccountNo": $("#accnt").val(),

                                    "BatchNo": tt[0].BatchNo,
                                    "ClearingType": tt[0].ClearingType,
                                    "InstrumentType": tt[0].InstrumentType,
                                    "ProcessingDate": tt[0].ProcessingDate,
                                    "BranchCode": tt[0].BranchCode,
                                    "ScanningNodeId": tt[0].ScanningNodeId,
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
                                    "UserNarration": tt[0].UserNarration,
                                    "rejectreasondescrpsn": tt[0].RejectReasonDescription,
                                    "ctsNonCtsMark": tt[0].ctsNonCtsMark,
                                    "Modified": tt[0].Modified,
                                    "ScanningType": tt[0].ScanningType,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqiwmicr = JSON.stringify(L1);
                                    localacct.setItem(owL1, chqiwmicr);

                                }

                                //----------------------------------------------------------------------//

                                //-------------added on 08-04-2019--------------------------------
                                var Tfimg = document.getElementById('Tfmyimg');
                                if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                                    document.getElementById('tiffimg').style.display = "none";
                                    document.getElementById("myimg").style.display = "";
                                }
                                //-------------END----------------

                                document.getElementById('myimg').removeAttribute('src');
                                document.getElementById('Tfmyimg').removeAttribute('src');

                                //document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                convertTiffImage(tt[1].FrontGreyImagePath);

                                document.getElementById('Decision').value = "";
                                document.getElementById('IWRemark').value = "";
                                document.getElementById('rtncd').style.display = "none";
                                strModified = "0000000000";

                                $("#cbsdetails").empty();
                                //--------------------------------//
                                document.getElementById("btnback").disabled = true;//prev false

                                $("#ClientCd").prop('disabled', true);
                                // document.getElementById('slpamt').style.display = "none";
                                document.getElementById('chqamt').style.display = "";
                                document.getElementById('slpacnt').style.display = "";
                                document.getElementById('MICR').style.display = "";
                                document.getElementById('ChqDate').value = "";
                                document.getElementById('Amt').value = "";
                                document.getElementById('ChqnoQC').value = "";
                                document.getElementById('SortQC').value = "";
                                document.getElementById('SANQC').value = "";
                                document.getElementById('TransQC').value = "";
                                document.getElementById('divctsnoncts').style.display = "";

                                if (tt[1].ScanningType == 11) {
                                    $("#accnt").prop('disabled', true);
                                }
                                $('#ctsnocts').val(tt[cnt].ClearingType);

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
                                //-----------------------------------------
                                document.getElementById('accnt').value = tt[1].CreditAccountNo;
                                //--------------------Added On 07-02-2017------------------
                                document.getElementById('oldact').value = tt[1].CreditAccountNo;

                                cbsbtn = false;

                                if (tt[1].CreditAccountNo != "") {

                                    $.ajax({
                                        async: false,
                                        url: RootUrl + 'OWL1/GetCBSDtls_New',
                                        dataType: 'html',
                                        data: { ac: $("#accnt").val() },
                                        success: function (data) {
                                            $('#cbsdetails').html(data);
                                            cbsbtn = true;
                                            $("#Payee option:first").attr('selected', 'selected');
                                        }
                                    });

                                }
                                //----------------------------------------------------
                                //Calling CAR To LAR-------------amtwrd-------------------
                                amtwrd = number2text(tt[1].FinalAmount)
                                document.getElementById('amtwrd').innerHTML = amtwrd;
                                //------------------------------------------------------------
                                //----------------------------------------------------                                
                                document.getElementById('Amt').value = addCommas(Number(tt[1].FinalAmount).toFixed(2));
                                document.getElementById('ChqDate').value = fnldate;
                                document.getElementById('ChqnoQC').value = tt[1].ChequeNoFinal;
                                document.getElementById('SortQC').value = tt[1].SortCodeFinal;
                                document.getElementById('SANQC').value = tt[1].SANFinal;
                                document.getElementById('TransQC').value = tt[1].TransCodeFinal;

                                //-------------------Added ON 28-12-2020----By Abid------
                                document.getElementById('brAccnt').value = "";
                                document.getElementById('brAmt').value = "";
                                document.getElementById('brAccnt').value = tt[1].BranchAccount;
                                document.getElementById('brAmt').value = tt[1].BranchAmount;
                                //-------------------Added ON 28-12-2020----By Abid------END-

                                if (tt[1].ScanningType == 11) {
                                    document.getElementById('ChqDate').focus();
                                }
                                else {
                                    if (document.getElementById("vftype").value == "CHQATVP") {
                                        document.getElementById('Decision').focus();
                                    }
                                    else {
                                        document.getElementById('accnt').focus();
                                    }
                                }

                                //----------------Modification --------Highlights----------------
                                if (tt[1].RejectedField == "1") {
                                    document.getElementById("Amt").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("Amt").style.backgroundColor = "white";
                                }
                                if (tt[1].RejectedField == "2") {
                                    document.getElementById("ChqDate").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("ChqDate").style.backgroundColor = "white";
                                }
                                if (tt[1].RejectedField == "3") {
                                    document.getElementById("accnt").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("accnt").style.backgroundColor = "white";
                                }
                                if (tt[1].RejectedField == "4") {
                                    document.getElementById("ChqnoQC").style.backgroundColor = "red";
                                    document.getElementById("SortQC").style.backgroundColor = "red";
                                    document.getElementById("SANQC").style.backgroundColor = "red";
                                    document.getElementById("TransQC").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("ChqnoQC").style.backgroundColor = "white";
                                    document.getElementById("SortQC").style.backgroundColor = "white";
                                    document.getElementById("SANQC").style.backgroundColor = "white";
                                    document.getElementById("TransQC").style.backgroundColor = "white";
                                }
                                if (tt[1].RejectedField == "5") {
                                    document.getElementById("frmL1").classList.add("w3-highway-red");
                                }
                                else {
                                    document.body.style.backgroundColor = "white";
                                }
                                //---------------------------------------------------------------
                                if (document.getElementById("vftype").value == "CHQATVF") {
                                    //-------------Account-----------------------
                                    if (tt[1].ATVAccountPass.trim() == "N") {
                                        document.getElementById("accnt").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("accnt").style.backgroundColor = "white";
                                    }
                                    //-------------Amount------------------------------------------------------------------
                                    if (tt[1].ATVAmountPass.trim() == "N") {
                                        document.getElementById("Amt").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("Amt").style.backgroundColor = "white";
                                    }
                                    //-------------ChqDate-----------------------------------------------------------------
                                    if (tt[1].ATVDatePass.trim() == "N") {
                                        document.getElementById("ChqDate").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("ChqDate").style.backgroundColor = "white";
                                    }
                                    //-------------ChqNo--------------------------------------------------------------------
                                    if (tt[1].ATVMICRPasss.trim() == "N") {
                                        document.getElementById("ChqnoQC").style.backgroundColor = "red";
                                        document.getElementById("SortQC").style.backgroundColor = "red";
                                        document.getElementById("SANQC").style.backgroundColor = "red";
                                        document.getElementById("TransQC").style.backgroundColor = "red";
                                    }
                                    else {
                                        document.getElementById("ChqnoQC").style.backgroundColor = "white";
                                        document.getElementById("SortQC").style.backgroundColor = "white";
                                        document.getElementById("SANQC").style.backgroundColor = "white";
                                        document.getElementById("TransQC").style.backgroundColor = "white";
                                    }
                                }
                                //--------------------END-------------------------------------------------

                                bankName(tt[1].SortCodeFinal);  //-------------For bank name
                                document.getElementById('bankname').style.display = "";

                                //---------------------Filling cbsDetails------
                                var callbyValue;
                                callbyValue = "Normal";

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

    //---------------------------------------
    getcbsdtls = function () {
        //debugger;
        cbsbtn = false;
        var Acct = document.getElementById('accnt').value;
        var actualtAC = document.getElementById('accnt').value;
        var acmin = document.getElementById('acmin').value;
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
        // Acct = document.getElementById('accnt').value
        if (Acct == "") {
            alert("Invalid Account Number!");
            document.getElementById('accnt').focus();
            return false;
        }
        else {
            $.ajax({
                async: false,
                url: RootUrl + 'OWL1/GetCBSDtls_New',
                dataType: 'html',
                data: { ac: $("#accnt").val() },
                success: function (data) {
                    cbsbtn = true;
                    $('#cbsdetails').html(data);
                    $("#Payee option:first").attr('selected', 'selected');

                }
            });
        }
    }
});

function getselect() {
    randomPayeeName = document.getElementById('Payee').value;
}
function reasonselected(rtnval) {

    document.getElementById('IWRemark').value = rtnval;
    document.getElementById('RejectReason').style.display = 'none';
    var rejctrcd = $("#IWRemark").val();
    if (rejctrcd.length == 2) {
        var rjctresnlTemp = document.getElementById('rtnlist');
        var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
        for (var i = 0; i < rjctresnlTemp.length; i++) {
            if (rejctrcd == rjctresnlTemp[i].value) {
                document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
                break;
            }
        }
    }
}
//---------------
function valid(RejectDescription, Reasoncode) {
    if (Reasoncode == "") {
        alert('Reason code not selected !');
        return false;
    }
    if (Reasoncode == "88" && RejectDescription == "") {
        alert('Please enter reject description!');
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
            return false;
        }
        if (str.indexOf("OTHER REASON") >= 0 || str.indexOf("OTHERREASON") >= 0) {
            alert('You can not mention "other reason"!!');
            return false;
        }
        else if (str.indexOf("OTHER") >= 0 && str.indexOf("REASON") >= 0) {
            alert('You can not mention "other reason"!!');
            return false;
        }
        var re = /[^0-9]/g;
        if (re.test(str.charAt(0)) == false || /^[a-zA-Z0-9- ]*$/.test(str.charAt(0)) == false) {
            alert('Please start with alphabets!!');
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
            return false;
        }
    }
}

function convertTiffImage(varImage) {

    $.ajax({
        url: RootUrl + 'OWL1/getTiffImage',
        data: { httpwebimgpath: varImage },
        type: 'GET',
        datatype: 'html',
        success: function (rtiff) {
            debugger;
            document.getElementById("myimg").style.display = "none";
            $('#tiffimg').html(rtiff);
            //-------------------Added on 02-03-2019---------
            var Tfimg = document.getElementById('Tfmyimg');
            if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                document.getElementById('tiffimg').style.display = "";
            }
            //-----------------------------------------------
        },
        error: function () {
            alert("Error occured while converting image!!!");
        }
    });
}

function IWCQL1() {

    var IWdecn = document.getElementById('Decision').value.toUpperCase();
    var valcnt = document.getElementById('cnt').value;
    //----------------DBTAC-----------------------
    randomPayeeName = document.getElementById('Payee').value;
    var acmin = document.getElementById('acmin').value;
    var TempAcct = document.getElementById('accnt').value;
    var lastChar = TempAcct[TempAcct.length - 1];
    if (isNaN(lastChar) == true) {
        alert('Account number not valid!!');
        document.getElementById('accnt').focus();
        return false;
    }
    var Acct = document.getElementById('accnt').value;
    Acct = Acct.length
    var Accval = document.getElementById('accnt').value;

    if (Acct.length > acmin) {
        var prevval = "";
        var nextval;
        var index = 0;
        for (var i = 0; i < acmin; i++) {
            nextval = "";
            nextval = Accval.charAt(i);
            if (prevval != "") {
                if (prevval == nextval) {
                    prevval = nextval;
                    index = index + 1;
                }
                else {
                    index = 0;
                }
            }
            else {
                prevval = nextval
            }
        }
        if (index > 6 && Accval.substring(0, 7) != "9999999") {
            alert("Invalid Account Number!");
            document.getElementById('accnt').focus();
            return false;
        }

    }
    if (Acct.length > 15) {
        $.ajax({
            async: false,
            url: RootUrl + 'OWL1/GetCBSDtls_New',
            dataType: 'html',
            data: { ac: $("#accnt").val() },
            success: function (data) {
                cbsbtn = true;
                $('#cbsdetails').html(data);
                $("#Payee option:first").attr('selected', 'selected');
            }
        });
    }
    if (tt[valcnt].CreditAccountNo != $("#accnt").val() && cbsbtn == false) {
        alert("Click on 'GetDetails' button/press F12 !");
        document.getElementById('accnt').focus();
        return false;
    }
    if (document.getElementById("blockkey").value == "1") {
        if (IWdecn == "A") {
            alert('You can not accept the cheque!');
            return false;
        }
    }

    //-----------------Account-----------------
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
    if (cbsbtn == false) {
        alert("Click on 'GetDetails' button/press F12 !");
        document.getElementById('accnt').focus();
        return false;
    }
    //----------------------------Amount---------------------//
    amt = document.getElementById('Amt').value;
    // alert(amt);
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
    amt = amt.replace(/^0+/, '')
    amt = amt.replace(/,/g, "");
    amt = amt.length;
    if (amt1 == ".") {
        alert('Amount field should not be dot(.) !');
        document.getElementById('Amt').focus();
        return false;
    }
    else if (amt1 == ".0") {
        alert('Amount not valid !');

        return false;
    }
    else if (amt1 == ".00") {
        alert('Amount not valid !');

        return false;
    }
    else if (amt1 == "0.00") {
        alert('Amount field should not be zero(0) !');

        return false;
    }
    else if (amt1 == "0.10") {
        alert('You can not accept this amount checque) ! ' + amt1);

        return false;
    }
    else if (amt1 == "0.01") {
        alert('Amount field should not be zero(0) ! ' + amt1);

        return false;
    }
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
    else if (dat.length < 6) {
        alert("Date not valid !");
        document.getElementById('ChqDate').focus();
        document.getElementById('ChqDate').select();
        return false;
    }
    else {

        finldat = new String(dat);
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
                document.getElementById('rejectreasondescrpsn').focus();
                return false;
            }
        }
        var flg = false;
        var iwrked = document.getElementById('IWRemark').value;
        // debugger;
        if (iwrked.length == 1) {
            //-------------------------
            var pad = "0"
            iwrked = pad.substring(0, iwrked.length) + iwrked;
            //-------------------------
        }

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

function narrationAC(curntAc) {
    // debugger;
    narrationReqirdflg = false;
    var narsnac = document.getElementById('narration');
    var actualAC = curntAc.slice(-10)

    for (var i = 0; i < narsnac.length; i++) {
        //alert(rtncode);
        if (actualAC == narsnac[i].value && $("#ofc").val() == "1") {
            //  alert(rtnlistDescrp[i].value);
            document.getElementById('narsndiv').style.display = "none";
            // document.getElementById('ofc').value = "1";
            narrationReqirdflg = false;
            break;
        }
        else if (curntAc == narsnac[i].value && $("#ofc").val() == "1") {
            //  alert(rtnlistDescrp[i].value);
            document.getElementById('narsndiv').style.display = "none";
            // document.getElementById('ofc').value = "1";
            narrationReqirdflg = false;
            break;
        }
        else if ($("#ofc").val() == "1") {
            document.getElementById('narsndiv').style.display = "";
            // document.getElementById('ofc').value = "1";
            narrationReqirdflg = true;
        }
    }
}
function IWVef() {
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
            document.getElementById('rtncd').style.display = "";
            document.getElementById('IWDecision').focus();
        }
    }
    else {
        document.getElementById('rtncd').style.display = "none";
    }
}

function fullImage() {
    if (tiffimagecall == true) {
        document.getElementById('fiwimg').style.display = 'block'
        document.getElementById('Tfulmyimg').src = document.getElementById('Tfmyimg').src;
    }
    else {

        document.getElementById('iwimg').style.display = 'block'
        document.getElementById('myfulimg').src = document.getElementById('myimg').src;
    }
}
//---------------------------------
function ChangeImage(imagetype) {
    var indexcnt;

    if (backbtn == true) {
        indexcnt = backcnt;
    }
    else {
        indexcnt = document.getElementById('cnt').value;
    }

    if (imagetype == "FTiff") {
        tiffimagecall = true;

        $.ajax({
            url: RootUrl + 'OWL1/getTiffImage',
            data: { httpwebimgpath: tt[indexcnt].FrontTiffImagePath },
            type: 'GET',
            datatype: 'html',
            success: function (rtiff) {
                debugger;
                document.getElementById("myimg").style.display = "none";
                $('#tiffimg').html(rtiff);
                //-------------------Added on 02-03-2019---------
                var Tfimg = document.getElementById('Tfmyimg');
                if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                    document.getElementById('tiffimg').style.display = "";
                }
                //-----------------------------------------------
            },
            error: function () {
                alert("Error occured while converting image!!!");
            }
        });
    }
    else if (imagetype == "BTiff") {
        tiffimagecall = true;

        $.ajax({
            url: RootUrl + 'OWL1/getTiffImage',
            data: { httpwebimgpath: tt[indexcnt].BackTiffImagePath },
            type: 'GET',
            datatype: 'html',
            success: function (rtiff) {
                debugger;
                document.getElementById("myimg").style.display = "none";
                $('#tiffimg').html(rtiff);
                //-------------------Added on 02-03-2019---------
                var Tfimg = document.getElementById('Tfmyimg');
                if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                    document.getElementById('tiffimg').style.display = "";
                }
                //-----------------------------------------------
            },
            error: function () {
                alert("Error occured while converting image!!!");
            }
        });
    }

    else if (imagetype == "FGray") {
        tiffimagecall = true;

        //document.getElementById("tiffimg").style.display = "none";
        //document.getElementById("myimg").style.display = "";
        //document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath;
        $.ajax({
            url: RootUrl + 'OWL1/getTiffImage',
            data: { httpwebimgpath: tt[indexcnt].FrontGreyImagePath },
            type: 'GET',
            datatype: 'html',
            success: function (rtiff) {
                debugger;
                document.getElementById("myimg").style.display = "none";
                $('#tiffimg').html(rtiff);
                //-------------------Added on 02-03-2019---------
                var Tfimg = document.getElementById('Tfmyimg');
                if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                    document.getElementById('tiffimg').style.display = "";
                }
                //-----------------------------------------------
            },
            error: function () {
                alert("Error occured while converting image!!!");
            }
        });


    }
    else if (imagetype == "BGray") {
        tiffimagecall = true;//false
        //var tempfullimg = tt[indexcnt].FrontTiffImagePath.replace(/Front/g, "Back").replace(/tif/g, "jpg");
        //document.getElementById("tiffimg").style.display = "none";
        //document.getElementById("myimg").style.display = "";
        //document.getElementById('myimg').src = tempfullimg;
        $.ajax({
            url: RootUrl + 'OWL1/getTiffImage',
            data: { httpwebimgpath: tt[indexcnt].FrontTiffImagePath.replace(/Front/g, "Back") },
            type: 'GET',
            datatype: 'html',
            success: function (rtiff) {
                debugger;
                document.getElementById("myimg").style.display = "none";
                $('#tiffimg').html(rtiff);
                //-------------------Added on 02-03-2019---------
                var Tfimg = document.getElementById('Tfmyimg');
                if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                    document.getElementById('tiffimg').style.display = "";
                }
                //-----------------------------------------------
            },
            error: function () {
                alert("Error occured while converting image!!!");
            }
        });
    }

    //else if (imagetype == "FGray") {
    //    tiffimagecall = false;
    //    document.getElementById("tiffimg").style.display = "none";
    //    document.getElementById("myimg").style.display = "";
    //    document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath;
    //}
    //else if (imagetype == "BGray") {
    //    tiffimagecall = false;
    //    var tempfullimg = tt[indexcnt].FrontTiffImagePath.replace(/Front/g, "Back").replace(/tif/g, "jpg");
    //    document.getElementById("tiffimg").style.display = "none";
    //    document.getElementById("myimg").style.display = "";
    //    document.getElementById('myimg').src = tempfullimg;
    //}

}
//----------------------------------
function CalAmt() {
    //  debugger;
    if ($("#Amt").val() != null || $("#Amt").val() != "") {
        ChequeAmountTotal = parseFloat(parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) - parseFloat(String(tempAmtValue).replace(/,/g, '')));
        ChequeAmountTotal = (parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) + parseFloat($("#Amt").val().replace(/,/g, '')));
        //ChequeAmountTotal.toFixed(1));

        //document.getElementById('totamt').innerHTML = ChequeAmountTotal.toFixed(2);
        tempAmtValue = parseFloat($("#Amt").val().replace(/,/g, ''));
        //-----------For---Axis bank----------------

        //if ($("#Scheme").val() == "SBTRS") {
        //    var temppamt = $("#Amt").val();
        //    // var tempchqamt = $("#Amt").val();

        //    if (temppamt.replace(/,/g, "") >= 50000) {
        //        alert('Alert! Exceeds Rs. 50,5000, check the beneficiary\n if third party, to comply with RBI guidelines!!');
        //    }
        //}
        //else if ($("#Scheme").val() == "SUKAN") {
        //    var temppamt = $("#Amt").val();
        //    //var tempamt = $("#Amt").val();

        //    if (temppamt.replace(/,/g, "") % 100 != 0 && temppamt.replace(/,/g, "") > 150000) {
        //        document.getElementById("blockkey").value = "1";
        //        alert('Alert! Sukanya Samrudhi, Amount not valid!!');
        //    }
        //    else {
        //        document.getElementById("blockkey").value = "0";
        //    }
        //}
        //-----------For---Axis bank----------------END
    }

}

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
//------------------------Comma Sepration----------------------------------//
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
    for (var i = 0; i <= amount.length - 1; i++) {
        finaloutpt = amount[i] + finaloutpt;
        if (flg == true) {
            if (count == 2) {
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
                finaloutpt = "," + finaloutpt;
                count = 0;
            }
        }
        count = count + 1;
    }

    return (finaloutpt + x2);
}

function bankName(bankcode) {
    $.ajax({
        url: RootUrl + 'OWL1/GetBankName',
        dataType: 'html',
        data: { bankcode: bankcode },
        success: function (databank) {
            $('#bankname').html(databank);
        }
    });
}
function bankNameONFocus() {
    var valcntt = document.getElementById('cnt').value;
    if ($("#SortQC").val() != "" && $("#SortQC").val().length == 9 && $("#SortQC").val() != tt[valcntt].SortCodeFinal) {
        $.ajax({
            url: RootUrl + 'OWL1/GetBankName',
            dataType: 'html',
            data: { bankcode: $("#SortQC").val() },
            success: function (databank) {
                $('#bankname').html(databank);
            }
        });
    }
}
function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substr(0, index) + chr + str.substr(index + 1);
}



