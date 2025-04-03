
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var nextcall;
var L2;
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
var narrationReqirdflg = false;
var tempnarration;
var InstrumentType;
var SlipID;
var SlipRawaDataID;
var strModified = "0000000000";
var Foutcnt;
var finalAccount = "";
var acc1 = "";
var acc2 = "";
var acc3 = "";
var acmaxlength = 16;
var accVal;
var cnt = 0;
var idslst = [];
var backCnt = 0;
var AccNo = null;
var ChqAmount = 0;
var vImg = "FU";

//var scondbck = false;
function passval(array) {
    debugger;
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    AccNo = tt[0]["CreditAccountNo"];
    ChqAmount = tt[0]["FinalAmount"];
    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
    //GetAcctDetails();
}
function GetAcctDetails() {
    //debugger;
    //getcbsdtls(); code ====================

    if (document.getElementById('accnt').value != null && document.getElementById('accnt').value != "")
        var Acct = document.getElementById('accnt').value;
    else
        var Acct = AccNo;

    $("#accnt").val(Acct);


    var acmin = document.getElementById('acmin').value;
    //Acct = Acct.replace(/^0+/, '')
    Acct = Acct.length


    //For initial image display
    vImg = "FU";

    $("#cbsdetails").empty();
    //$("#SrcFnds").empty();
    var aNew = $("#accnt").val();
    //debugger;

    //==== Checking the api call setting added by amol on 21/09/2023 ===========
    debugger;
    var newAPICall = document.getElementById('NewApiCall').value;

    if (newAPICall == "Y" || newAPICall == "y") {
        $.ajax({
            url: RootUrl + 'OWL2/GetCBSDetailsWithAPI',
            dataType: 'html',
            data: { ac: $("#accnt").val() },
            async: false,
            cache: false,
            success: function (data) {
                debugger;
                cbsbtn = true;
                $('#cbsdetails').html(data);
                var vPayee = document.getElementById("Payee").value;
                $("#PayeeName").val(vPayee);
                vPayee = "";

            }
        });
    }
    else {

    }
    
    debugger;
    document.getElementById('accnt').focus();

}

var getreason;
var getcbsdtls;
var clientdtls;

var tempdat;
var fnldate;
var yr, mm, dd;


$(document).ready(function () {

    // To set focus on first control
    $(function () {
        $("input[type=text]").first().focus();
    });

    debugger;

    //account no. with dot (.)
    $("#accnt").keyup(function (event) {
        //debugger;
        console.log('In');
        if (accVal === 'Y') {
            //return isNumberWithDot(event, this);
            var result = isNumberWithDot(event, this);

            if (result === false) {
                debugger;
                //finalAccount = $("#accnt").val();
                return false;
            }
            else {
                finalAccount = $("#accnt").val();
                var acc = $("#accnt").val();
                console.log(acc);

                if (acc.indexOf('.') !== -1) {
                    //debugger

                    var num = acc.match(/\./g).length;
                    if (num > 0) {
                        console.log('dot');
                        console.log(num);
                        if (num === 1) {
                            var dotIndex1 = acc.indexOf('.');
                            console.log('dotIndex1 - ' + dotIndex1);
                            console.log('currentCharacterIndex - ' + acc.length);
                            if (dotIndex1 !== 0) {
                                if ((acc.length - 1) <= dotIndex1) {
                                    var accStringNew1 = acc;
                                    //console.log('accStringNew1 - ' + accStringNew1);
                                    var accString1 = accStringNew1.replace(/\./g, "");
                                    console.log('accString1 - ' + accString1);
                                    acc1 = padLeadingZeros(accString1, 4);
                                    console.log('acc1 - ' + acc1);
                                    var chkResult = checkStringWithAllZero(acc1);
                                    if (chkResult === true) {
                                        finalAccount = acc1;
                                        console.log('finalAccount - ' + finalAccount);
                                    }
                                    else {
                                        alert('Please enter non zero value.');
                                        document.getElementById("accnt").focus();
                                        return false;
                                    }

                                }
                            }
                            else {
                                var str = $("#accnt").val();
                                var strNew = str.replace(/\./g, "");
                                $("#accnt").val(strNew);
                                alert('Please Enter digit');
                                return false;
                            }


                        }
                        if (num === 2) {
                            //debugger
                            var dotIndex2 = acc.indexOf('.');
                            var lastdotIndex2 = acc.lastIndexOf('.');
                            console.log('Last dotIndex - ' + lastdotIndex2);
                            if ((acc.length - 1) <= lastdotIndex2) {
                                var subAccString = acc.substring(dotIndex2);
                                console.log('subAccString - ' + subAccString);
                                var accStringNew2 = subAccString;
                                var accString2 = accStringNew2.replace(/\./g, "");
                                console.log('accString2 - ' + accString2);
                                acc2 = padLeadingZeros(accString2, 3);
                                console.log('acc2 - ' + acc2);
                                var chkResult1 = checkStringWithAllZero(acc2);
                                if (chkResult1 === true) {
                                    finalAccount = acc1 + acc2;
                                    console.log('finalAccount - ' + finalAccount);
                                }
                                else {
                                    alert('Please enter non zero value.');
                                    document.getElementById("accnt").focus();
                                    return false;
                                }
                            }
                            else {
                                acc3 = acc.substring(acc.lastIndexOf('.') + 1);
                                console.log('acc3 - ' + acc3);
                                acc3 = padLeadingZeros(acc3, 9);
                                var chkResult2 = checkStringWithAllZero(acc3);
                                if (chkResult2 === true) {
                                    finalAccount = acc1 + acc2 + acc3;
                                    console.log('finalAccount - ' + finalAccount);
                                    //$("#ChqAcno").val(finalAccount);
                                    //document.getElementById("getdtls").focus();
                                }
                                else {
                                    alert('Please enter non zero value.');
                                    document.getElementById("accnt").focus();
                                    return false;
                                }
                                finalAccount = acc1 + acc2 + acc3;
                                //console.log('finalAccount - ' + finalAccount);
                            }
                        }
                        if (num > 2) {
                            alert('You can not enter more than 2 dot.');
                            return false;
                        }
                    }
                }
            }


        }
        else {
            var result1 = isNumberWith(event, this);
            if (result1 === false) {
                return false;
            }
            else {
                document.getElementById("getdtls").focus();
            }


        }


    });

    function padLeadingZeros(num, size) {
        var s = num + "";
        while (s.length < size) s = "0" + s;
        return s;
    };

    function checkStringWithAllZero(str) {
        var number = Number(str);
        if (number > 0) {
            return true;
        }
        else {
            return false;
        }
    };

    $("#Decision").focusout(function () {

        document.getElementById("idReason").focus();
    });

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

        //debugger;
        var Acct = document.getElementById('accnt').value;

        if (Acct != null)
            accVal = "Y";

        if (event.keyCode == 123) {
            getcbsdtls(); //CbsDetails
            return false;
        }
    });

    //debugger;
    if (tt.length > 0) {

        if (document.getElementById('hVfType').value == "RHold" || document.getElementById('hVfType').value == "BHold") {
            document.getElementById('hldslip').style.display = "";
        }

        var Tfimg = document.getElementById('myimg1');
        if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
            document.getElementById('myimg1').style.display = "none";
        }

        document.getElementById('myimg').removeAttribute('src');
        document.getElementById('divtiff').style.display = "none";
        document.getElementById('myimg').style.display = "block";
        document.getElementById('myimg').src = RootUrl + 'Icons/Loading.jpg';

        var frontGreyimgUrl = tt[1].FrontGreyImagePath;
        debugger;
        if (Number(tt[1].FinalAmount) >= 200000) {

            var imgUrl = tt[1].FrontUVImagePath;
            $.ajax({
                url: imgUrl,
                type: 'HEAD',
                error: function () {
                    debugger;
                    //file not exists
                    alert('Image not loaded correctly!!!');
                    //document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

                    $.ajax({
                        url: frontGreyimgUrl,
                        type: 'HEAD',
                        error: function () {
                            debugger;
                            //file not exists
                            //alert('Image not loaded correctly!!!');
                            document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                        },
                        success: function () {
                            debugger;
                            //file exists
                            console.log("Front image url success " + frontGreyimgUrl);
                            document.getElementById('myimg').src = frontGreyimgUrl;
                        }
                    });
                },
                success: function () {
                    debugger;
                    //file exists
                    console.log("UV image url success " + imgUrl);
                    document.getElementById('myimg').src = imgUrl;
                }
            });

            //document.getElementById('myimg').src = tt[1].FrontUVImagePath;
            //ChangeImage('FUV');
        }
        else {
            //document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
            //ChangeImage('FGray');
            //var frontGreyimgUrl = tt[1].FrontGreyImagePath;

            $.ajax({
                url: frontGreyimgUrl,
                type: 'HEAD',
                error: function () {
                    debugger;
                    //file not exists
                    alert('Image not loaded correctly!!!');
                    document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                },
                success: function () {
                    debugger;
                    //file exists
                    console.log("Front image url success " + frontGreyimgUrl);
                    document.getElementById('myimg').src = frontGreyimgUrl;
                }
            });
        }

        InstrumentType = tt[1].InstrumentType;

        //-------Remove save objects from browser---//
        window.localStorage.clear();
        //------------ idslist end----------------//
        if (tt[1].InstrumentType == "S") {

            if (tt[1].ScanningType == 3 || tt[1].ScanningType == 5) {

                $("#ClientCd").prop('disabled', true);

                document.getElementById('ClntsDtlsdiv').style.display = "";
                document.getElementById('Chqacnt').style.display = "";
                document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;
                document.getElementById('ClientCd').value = tt[1].ClientCode;
                debugger;
                $.ajax({
                    url: RootUrl + 'OWL2/GetCBSDetailsWithAPI',
                    dataType: 'html',
                    data: { ac: tt[1].CreditAccountNo, callby: "ClientCode" },
                    success: function (data) {
                        cbsbtn = true;
                        $('#cbsdetails').html(data);

                        var vPayee = document.getElementById("Payee").value;
                        $("#PayeeName").val(vPayee);
                        vPayee = "";



                        //if (tt[1].FinalAmount >= 200000)
                        //    ChangeImage('FUV');
                        //else
                        //    ChangeImage('FGray');
                    }
                });
                // alert($("#ClientCd").val());

                if ($("#ClientCd").val() != "") {
                    // alert('call');
                    $.ajax({
                        url: RootUrl + 'OWL2/GetClientDlts',
                        dataType: 'html',
                        data: { ac: $("#ClientCd").val() },
                        success: function (data) {
                            cbsbtn = true;
                            $('#clientdetails').html(data);

                        }
                    });
                    //clientdtls();
                }
                document.getElementById('accnt').value = tt[1].CreditAccountNo;
                document.getElementById('Decision').focus();

            }
            else {
                document.getElementById('slpacnt').style.display = "";
                document.getElementById('Decision').focus();
                document.getElementById('Chqacnt').style.display = "none";
                document.getElementById('accnt').value = tt[1].CreditAccountNo;
                document.getElementById('Decision').focus();

                //--------------------Added On 07-02-2017------------------
                document.getElementById('oldact').value = tt[1].CreditAccountNo;

                if ($("#NarrationID").val() == "Y") {
                    //document.getElementById('nartext').value = tt[1].UserNarration;
                    //$('#nartext').attr('readonly', false);
                }
                var aNew = $("#accnt").val();
                debugger;

                $.ajax({
                    url: RootUrl + 'OWL2/GetCBSDetailsWithAPI',
                    dataType: 'html',
                    //data: { ac: $("#accnt").val(), strcbsdls: tt[1].CBSAccountInformation, strJoinHldrs: tt////[1].CBSJointAccountInformation, callby: "Normal", payeename: tt[1].PayeeName },
                    data: { ac: $("#accnt").val() },
                    success: function (data) {
                        cbsbtn = true;
                        $('#cbsdetails').html(data);
                        var vPayee = document.getElementById("Payee").value;
                        $("#PayeeName").val(vPayee);
                        vPayee = "";



                        //if (tt[1].FinalAmount >= 200000)
                        //    ChangeImage('FUV');
                        //else
                        //    ChangeImage('FGray');
                    }
                });



            }

            //--------------------------------------------------
            document.getElementById('slpamt').style.display = "";
            document.getElementById('chqamt').style.display = "none";

            document.getElementById('MICR').style.display = "none";
            document.getElementById('slpamount').value = addCommas(Number(tt[1].SlipAmount).toFixed(2));
            document.getElementById('sliplabl').style.display = "none";

            document.getElementById('divctsnoncts').style.display = "none";
            document.getElementById('divmarkp2f').style.display = "none";
            document.getElementById('bankname').style.display = "none";


        }
        else {

            document.getElementById('slpamt').style.display = "none";
            document.getElementById('chqamt').style.display = "";
            document.getElementById('MICR').style.display = "";
            //-----------------------------
            document.getElementById('slpacnt').style.display = "";
            // document.getElementById('Decision').focus();
            document.getElementById('Chqacnt').style.display = "none";
            document.getElementById('accnt').value = tt[1].CreditAccountNo;
            document.getElementById('Decision').focus();

            //--------------------Added On 07-02-2017------------------
            document.getElementById('oldact').value = tt[1].CreditAccountNo;

            var aNew = $("#accnt").val();

            debugger;
            //--------------------------------
            document.getElementById('ChqDate').value = "";
            document.getElementById('Amt').value = "";
            document.getElementById('ChqnoQC').value = "";
            document.getElementById('SortQC').value = "";
            document.getElementById('SANQC').value = "";
            document.getElementById('TransQC').value = "";

            document.getElementById('PayeeName').value = "";
            document.getElementById('DraweeName').value = "";

            document.getElementById('divctsnoncts').style.display = "";
            document.getElementById('divmarkp2f').style.display = "";

            document.getElementById('mtrn').value = tt[1].RawDataId;
            document.getElementById('bankname').style.display = "";

            realmodified = false;

            document.getElementById('Amt').value = addCommas(Number(tt[1].FinalAmount).toFixed(2));

            document.getElementById('ChqnoQC').value = tt[1].ChequeNoFinal;
            document.getElementById('SortQC').value = tt[1].SortCodeFinal;
            document.getElementById('SANQC').value = tt[1].SANFinal;
            document.getElementById('TransQC').value = tt[1].TransCodeFinal;

            document.getElementById('PayeeName').value = tt[1].PayeeName;
            document.getElementById('DraweeName').value = tt[1].DraweeName;

            //============ amol changes for setting the sourceOfFunds values on 02/11/2022 start ============
            debugger;
            $("#cbsdetails").empty();
            //$("#SrcFnds").empty();

            GetAcctDetails();

            if (tt[1].NRESourceOfFundId == 0 && tt[1].NROSourceOfFundId == 0) {
                document.getElementById('SrcFnds').style.display = "none";
            }
            else {
                document.getElementById('SrcFnds').style.display = "block";
                if (tt[1].NRESourceOfFundId > 0) {
                    //document.getElementById('ddSrcFndsNRE').selectedIndex = tt[1].NRESourceOfFundId;
                    $.ajax({
                        url: RootUrl + 'OWL2/GetSourceofFundsFromNRE_NRO',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'html',
                        data: { ID: tt[1].NRESourceOfFundId, RecordID: tt[1].Id, RawDataId: tt[1].RawDataId, schemeType: 'NRE' },
                        async: false,
                        success: function (data) {
                            debugger;
                            document.getElementById('txtSrcFnds').value = '';
                            $("#txtSrcFnds").val(data);
                        }
                    });

                }
                else if (tt[1].NROSourceOfFundId > 0) {
                    //document.getElementById('ddSrcFndsNRO').selectedIndex = tt[1].NROSourceOfFundId;
                    $.ajax({
                        url: RootUrl + 'OWL2/GetSourceofFundsFromNRE_NRO',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'html',
                        data: { ID: tt[1].NROSourceOfFundId, RecordID: tt[1].Id, RawDataId: tt[1].RawDataId, schemeType: 'NRO' },
                        async: false,
                        success: function (data) {
                            debugger;
                            document.getElementById('txtSrcFnds').value = '';
                            $("#txtSrcFnds").val(data);
                        }
                    });
                }
            }
            //============ amol changes for setting the sourceOfFunds values on 02/11/2022 end ============

            tempAmtValue = tt[1].FinalAmount;
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
            //debugger;
            document.getElementById('ChqDate').value = fnldate;

            $('#ctsnocts').val(tt[1].ClearingType);
            // }
            if (tt[1].DocType.toUpperCase() == "C") {
                document.getElementById("markp2f").checked = true;
            }
            else {
                document.getElementById("markp2f").checked = false;
            }
            bankName(tt[1].SortCodeFinal);  //-------------For bank name

        }

        //--------------------------------------------------------------
        document.getElementById('strbranchcd').innerHTML = tt[1].BranchCode;
        document.getElementById('ScanningID').innerHTML = tt[1].ScanningNodeId;
        document.getElementById('strBatchNo').innerHTML = tt[1].BatchNo;
        document.getElementById('strBatchSeqNO').innerHTML = tt[1].BatchSeqNo;
        //--------------------------------------------------------------

        //-------------------------------------- Modification HI-------------------------------
        //-------------Account-----------------------
        if (tt[1].Modified1.charAt(0) == "1") {
            document.getElementById("accnt").style.backgroundColor = "red";
        }
        else {
            document.getElementById("accnt").style.backgroundColor = "white";
        }
        //-------------Amount------------------------------------------------------------------
        if (tt[1].Modified1.charAt(2) == "1") {
            document.getElementById("Amt").style.backgroundColor = "red";
        }
        else {
            document.getElementById("Amt").style.backgroundColor = "white";
        }
        //-------------ChqDate-----------------------------------------------------------------
        if (tt[1].Modified1.charAt(3) == "1") {
            document.getElementById("ChqDate").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqDate").style.backgroundColor = "white";
        }
        //-------------ChqNo--------------------------------------------------------------------
        if (tt[1].Modified1.charAt(4) == "1") {
            document.getElementById("ChqnoQC").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqnoQC").style.backgroundColor = "white";
        }
        //-------------SortQC-------------------------------------------------------------------
        if (tt[1].Modified1.charAt(5) == "1") {
            document.getElementById("SortQC").style.backgroundColor = "red";
        }
        else {
            document.getElementById("SortQC").style.backgroundColor = "white";
        }
        //-------------SortQC-------------------------------------------------------------------
        if (tt[1].Modified1.charAt(6) == "1") {
            document.getElementById("SANQC").style.backgroundColor = "red";
        }
        else {
            document.getElementById("SANQC").style.backgroundColor = "white";
        }
        //-------------TransQC-------------------------------------------------------------------
        if (tt[1].Modified1.charAt(7) == "1") {
            document.getElementById("TransQC").style.backgroundColor = "red";
        }
        else {
            document.getElementById("TransQC").style.backgroundColor = "white";
        }

        document.getElementById("btnback").disabled = true

        document.getElementById('mtrn').value = tt[1].RawDataId;

        document.getElementById('rejectreasondescrpsn').value = "";

        // alert(tt[1].L1VerificationStatus);
        document.getElementById("l1decision").innerHTML = "";
        //----------------------Set L1 and L2 Decision Color ----------------
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
            getReturnDecrp(tt[1].L1RejectReason);
        }
    }


    $("#ok").click(function () {
        debugger;
        nextcall = false;

        var NREtxtSrcFnds = '';
        var NROtxtSrcFnds = '';
        var result = IWLICQC();
        if (result == false) {

            return false;
        }
        else {
            cnt = document.getElementById('cnt').value;
            //tempcnt = document.getElementById('tempcnt').value;

            if (tt[cnt].callby != "Slip") {
                document.getElementById("btnback").disabled = true;//prev false
            }

            var owL2 = "owL2";
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

                   // let apiData = $("#sourceCustomerId").val() + "|" + $("#currencyVal").val();

                   let apiData=$("#AllAPIData").val();


                    if (tt[backcnt].NRESourceOfFundId > 0) {
                        NREtxtSrcFnds = $("#txtSrcFnds").val();
                        NROtxtSrcFnds = '';
                    }
                    else if (tt[backcnt].NROSourceOfFundId > 0) {
                        NROtxtSrcFnds = $("#txtSrcFnds").val();
                        NREtxtSrcFnds = '';
                    }

                    owL2 = owL2 + backcnt
                    L2 = {
                        "CreditAccountNo": $("#accnt").val(),
                        "SlipAmount": parseFloat($("#slpamount").val().replace(/,/g, '')),
                        "BatchNo": tt[backcnt].BatchNo,
                        "ClearingType": tt[backcnt].ClearingType,
                        "InstrumentType": tt[backcnt].InstrumentType,
                        "SlipNo": tt[backcnt].SlipNo,
                        "ProcessingDate": tt[backcnt].ProcessingDate,
                        "BranchCode": tt[backcnt].BranchCode,
                        "ScanningNodeId": tt[backcnt].ScanningNodeId,
                        "ClientCode": $("#ClientCd").val(),
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
                        "FinalAmount": parseFloat($("#Amt").val().replace(/,/g, '')),
                        "FinalDate": $("#ChqDate").val(),
                        "ChequeNoFinal": $("#ChqnoQC").val(),
                        "SortCodeFinal": $("#SortQC").val(),
                        "SANFinal": $("#SANQC").val(),
                        "TransCodeFinal": $("#TransQC").val(),
                        "ChequeAmountTotal": tt[backcnt].ChequeAmountTotal,
                        "PayeeName": $("#PayeeName").val(),//randomPayeeName,//$("#Payee").val(),
                        "L1RejectReason": tt[backcnt].L1RejectReason,
                        "L1VerificationStatus": tt[backcnt].L1VerificationStatus,
                        "modified": realmodified,
                        "AccModified": realAccModified,
                        "UserNarration": $("#nartext").val(),
                        "rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                        "ctsNonCtsMark": $("#ctsnocts").val(),
                        "P2fMark": $('#markp2f').is(":checked"),
                        "SlipID": SlipID,
                        "SlipRawaDataID": SlipRawaDataID,
                        "ScanningType": tt[backcnt].ScanningType,
                        "Modified2": strModified,
                        "DraweeName": $("#DraweeName").val(),
                        //"NRESourceOfFundId": $("#ddSrcFndsNRE").val(),
                        //"NROSourceOfFundId": $("#ddSrcFndsNRO").val(),
                        "NRESourceOfFundId": tt[backcnt].NRESourceOfFundId,
                        "NROSourceOfFundId": tt[backcnt].NROSourceOfFundId,

                        //========= Added by Amol on 01/02/2024 start ===========
                        "API_Data": apiData,
                        //========= Added by Amol on 21/03/2024 start ===========
                        "IsOpenedDateOld": $("#IsOpenedDateOld").val(),
                        SrcFndsDescription: NREtxtSrcFnds,
                        NROSrcFndsDescription: NROtxtSrcFnds,
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

                        cnrslt = true;
                        nextcall = true;
                    }
                    if (InstrumentType == "S") {
                        SlipID = tt[tempcnt].Id;
                        SlipRawaDataID = tt[tempcnt].RawDataId;
                    }

                    debugger;
                    var tempcnt_Arr = cnt;

                    owL2 = owL2 + cnt;

                  //  let apiData = $("#sourceCustomerId").val() + "|" + $("#currencyVal").val();
                    let apiData = $("#AllAPIData").val();

                    if (tt[tempcnt_Arr].NRESourceOfFundId > 0) {
                        NREtxtSrcFnds = $("#txtSrcFnds").val();
                        NROtxtSrcFnds = '';
                    }
                    else if (tt[tempcnt_Arr].NROSourceOfFundId > 0) {
                        NROtxtSrcFnds = $("#txtSrcFnds").val();
                        NREtxtSrcFnds = '';
                    }

                    L2 = {
                        "CreditAccountNo": $("#accnt").val(),
                        "SlipAmount": parseFloat($("#slpamount").val().replace(/,/g, '')),
                        "BatchNo": tt[tempcnt_Arr].BatchNo,
                        "ClearingType": tt[tempcnt_Arr].ClearingType,
                        "InstrumentType": tt[tempcnt_Arr].InstrumentType,
                        "SlipNo": tt[tempcnt_Arr].SlipNo,
                        "ProcessingDate": tt[tempcnt_Arr].ProcessingDate,
                        "BranchCode": tt[tempcnt_Arr].BranchCode,
                        "ScanningNodeId": tt[tempcnt_Arr].ScanningNodeId,
                        "ClientCode": $("#ClientCd").val(),
                        "SlipRefNo": tt[tempcnt_Arr].SlipRefNo,
                        "Id": tt[tempcnt_Arr].Id,
                        "Action": btnval,
                        "Status": tt[tempcnt_Arr].Status,
                        "RawDataId": tt[tempcnt_Arr].RawDataId,
                        "RejectReason": $("#IWRemark").val(),
                        "DomainId": tt[tempcnt_Arr].DomainId,
                        "CustomerId": tt[tempcnt_Arr].CustomerId,
                        "CBSAccountInformation": $("#cbsdls").val(),
                        "CBSJointAccountInformation": $("#JoinHldrs").val(),
                        "FinalAmount": parseFloat($("#Amt").val().replace(/,/g, '')),
                        "FinalDate": $("#ChqDate").val(),
                        "ChequeNoFinal": $("#ChqnoQC").val(),
                        "SortCodeFinal": $("#SortQC").val(),
                        "SANFinal": $("#SANQC").val(),
                        "TransCodeFinal": $("#TransQC").val(),
                        "ChequeAmountTotal": tt[tempcnt_Arr].ChequeAmountTotal,
                        "PayeeName": $("#PayeeName").val(), //randomPayeeName,//$("#Payee").val() ,
                        "L1RejectReason": tt[tempcnt_Arr].L1RejectReason,
                        "L1VerificationStatus": tt[tempcnt_Arr].L1VerificationStatus,
                        "modified": realmodified,
                        "AccModified": realAccModified,
                        "UserNarration": $("#nartext").val(),
                        "rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                        "ctsNonCtsMark": $("#ctsnocts").val(),
                        "P2fMark": $('#markp2f').is(":checked"),
                        "SlipID": SlipID,
                        "SlipRawaDataID": SlipRawaDataID,
                        "ScanningType": tt[tempcnt_Arr].ScanningType,
                        "Modified2": strModified,
                        "DraweeName": $("#DraweeName").val(),
                        //"NRESourceOfFundId": $("#ddSrcFndsNRE").val(),
                        //"NROSourceOfFundId": $("#ddSrcFndsNRO").val(),
                        "NRESourceOfFundId": tt[tempcnt_Arr].NRESourceOfFundId,
                        "NROSourceOfFundId": tt[tempcnt_Arr].NROSourceOfFundId,

                        //========= Added by Amol on 01/02/2024 start ===========
                        "API_Data": apiData,
                        //========= Added by Amol on 21/03/2024 start ===========
                        "IsOpenedDateOld": $("#IsOpenedDateOld").val(),

                        SrcFndsDescription: NREtxtSrcFnds,
                        NROSrcFndsDescription: NROtxtSrcFnds,
                    };

                }
            }

            if (nextcall == true) {
                document.getElementById('IsOpenedDateOld').value = "";
                common(owL2);

            }
            else {
                // alert('Okk');
                document.getElementById('Decision').focus();
                document.getElementById("btnback").disabled = true;
            }

        }
    });

    //-------------------------------------Reject--------------------------------//\


    $("#btnRejct").click(function () {

        document.getElementById("btnback").disabled = true;//prev false
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var owL2 = "owL2";

        if (backbtn == true) {

            owL2 = owL2 + backcnt
            L2 = {
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

            owL2 = owL2 + cnt;
            L2 = {
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
        common(owL2);
        backbtn = false;
    });
    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;

        if (Modernizr.localstorage) {

            backbtn = true;
            var owL2 = "owL2"
            cnt = document.getElementById('cnt').value;
            owL2 = owL2 + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(owL2));

            debugger;
            if (Number(tt[cnt - 1].FinalAmount) >= 200000)
                document.getElementById('myimg').src = tt[cnt - 1].FrontUVImagePath;
            else
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
            document.getElementById('PayeeName').value = "";
            document.getElementById('DraweeName').value = "";

            document.getElementById('chequeAcct').innerHTML = orderData.CreditAccountNo;
            document.getElementById('Amt').value = orderData.FinalAmount;
            document.getElementById('ChqDate').value = orderData.FinalDate;
            document.getElementById('ChqnoQC').value = orderData.ChequeNoFinal;
            document.getElementById('SortQC').value = orderData.SortCodeFinal;
            document.getElementById('SANQC').value = orderData.SANFinal;
            document.getElementById('TransQC').value = orderData.TransCodeFinal;
            document.getElementById('Amt').focus();
            document.getElementById('PayeeName').value = orderData.PayeeName;
            document.getElementById('DraweeName').value = orderData.DraweeName;
            //--------------------------------------
            document.getElementById('ChqCnt').innerHTML = tt[1].SlipChequeCount;
            document.getElementById('totamt').innerHTML = ChequeAmountTotal;

            let apiData = orderData.API_Data.split("|");

            document.getElementById('currencyVal').value = apiData[1];
            document.getElementById('sourceCustomerId').value = apiData[0];

        }
    });
    //--------------Reject---------------------------------------
    $("#btnClose").click(function () {
        //alert('call');

        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;
            // alert(scond);
            if (scond == true) {
                var i;
                if (tt[0].callby == "Cheq") {
                    i = 0;
                }
                else {
                    i = 1;
                }
                for (i; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL2" + i));

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
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.rejectreasondescrpsn);
                        arrlist.push(orderData.ctsNonCtsMark);
                        arrlist.push(orderData.P2fMark);
                        arrlist.push(orderData.SlipID);
                        arrlist.push(orderData.SlipRawaDataID);
                        arrlist.push(orderData.ScanningType);
                        arrlist.push(orderData.Modified2);
                        arrlist.push(orderData.DraweeName);
                        arrlist.push(orderData.NRESourceOfFundId);
                        arrlist.push(orderData.NROSourceOfFundId);

                        //=================== Added by Amol on 01/03/2024 =============
                        arrlist.push(orderData.API_Data);
                        //=================== Added by Amol on 21/03/2024 =============
                        arrlist.push(orderData.IsOpenedDateOld);

                        arrlist.push(orderData.SrcFndsDescription);
                        arrlist.push(orderData.NROSrcFndsDescription);
                    }

                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL2" + i));
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
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.rejectreasondescrpsn);
                        arrlist.push(orderData.ctsNonCtsMark);
                        arrlist.push(orderData.P2fMark);
                        arrlist.push(orderData.SlipID);
                        arrlist.push(orderData.SlipRawaDataID);
                        arrlist.push(orderData.ScanningType);
                        arrlist.push(orderData.Modified2);
                        arrlist.push(orderData.DraweeName);
                        arrlist.push(orderData.NRESourceOfFundId);
                        arrlist.push(orderData.NROSourceOfFundId);

                        //=================== Added by Amol on 01/03/2024 =============
                        arrlist.push(orderData.API_Data);
                        //=================== Added by Amol on 21/03/2024 =============
                        arrlist.push(orderData.IsOpenedDateOld);

                        arrlist.push(orderData.SrcFndsDescription);
                        arrlist.push(orderData.NROSrcFndsDescription);
                    }
                    
                }
            }
            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(idslst);
            //if (orderData.InstrumentType == "S") {
            $.ajax({

                url: RootUrl + 'OWL2/OWL2Chq',
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

    //---------------- Data Entry -----------------------------------

    $("form input").keydown(function (e) {
        // alert('Aila');
        next_idx = $('input[type=text]').index(this) + 1;
        tot_idx = $('body').find('input[type=text]').length;
        // alert(tot_idx);
        // alert(tot_idx);
        var tempcounter = document.getElementById('cnt').value;
        // debugger;
        if (tt[tempcounter].InstrumentType == "S") {
            //alert(next_idx);

            if (next_idx == 4) {//---6
                next_idx = tot_idx;
            }
            //else {
            //    next_idx = 5;
            //}

            if (e.keyCode == 13) {
                //  alert(next_idx);
                if (tot_idx == next_idx) {
                    $("input[value='Ok']").click();
                }
                else if (next_idx == 1) {

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
            //debugger;
            if (e.keyCode == 13) {
                //  alert(next_idx);
                //if (tot_idx == next_idx) {
                //    $("input[value='Ok']").click();
                //}
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

                        next_idx = 3;//5
                        $('input[type=text]:eq(' + next_idx + ')').focus().select();
                    }
                }
                //else {
                //    next_idx = 3;//5
                //    $('input[type=text]:eq(' + next_idx + ')').focus().select();
                //}
                else if (next_idx == 4) {//6
                    next_idx = tot_idx;
                }
                else {
                    next_idx = 3;//5
                }

                if (tot_idx == next_idx) {
                    $("input[value='Ok']").click();
                }
                else {
                    $('input[type=text]:eq(' + next_idx + ')').focus().select();
                }
            }


            //if (e.keyCode == 13) {
            //    //  alert(next_idx);
            //    if (tot_idx == next_idx) {
            //        $("input[value='Ok']").click();
            //    }
            //    else {
            //        $('input[type=text]:eq(' + next_idx + ')').focus().select();
            //    }
            //}

        }
    });
    //----------------narration--------------------
    $("#nartext").keypress(function (event) {
        //debugger;
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

    //--------------------------------------------
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

        //debugger;
        tempcnt = document.getElementById('tempcnt').value;
        var c = document.getElementById("blockkey").value;

        //allowing inactive and debit freze acc 20-12-24 
        var accStatus=document.getElementById("accountStatus").value;
        var freezeStatusCode=document.getElementById('freezeStatusCode').value;

        if(accStatus=="Inactive" || freezeStatusCode=="D")
        {
            document.getElementById("blockkey").value="0";
        }
        //allowing inactive and debit freze acc 20-12-24 

        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }
        if (document.getElementById("blockkey").value == "1") {
            if (event.keyCode == 65 || event.keyCode == 97) {
                event.preventDefault();
                alert('You can not accept the cheque! - ' + document.getElementById("sCAPA").value);
                return false;
            } // event.keyCode == 65 || event.keyCode == 72|| event.keyCode == 104 ||


            if (event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

            }
            else {
                event.preventDefault();
            }
        }
        else {

            if (tt[tempcnt].InstrumentType == 'S') {
                if (event.keyCode == 65 || event.keyCode == 72 || event.keyCode == 82 || event.keyCode == 104 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

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
            var amtval = this.value;;
            if (amtval.length > 0) {
                var splitstr = amtval.split('.');

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

    $('#DraweeName').keyup(function () {
        var $th = $(this);
        $th.val($th.val().replace(/[^a-zA-Z0-9\s]/g, function (str) { alert('You typed " ' + str + ' ".\n\nPlease use only letters and numbers.'); return ''; }));
    });

    //--------------------------Do All inpute Changes----------------Validation

    $("#Amt").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        //  debugger;
        if (tt[Foutcnt].FinalAmount != parseFloat($("#Amt").val().replace(/,/g, ''))) {
            realmodified = true;
            strModified = setCharAt(strModified, 2, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 2, '0');
        }
    });
    $("#ChqDate").focusout(function () {
        // debugger;
        if (fnldate != $("#ChqDate").val()) {
            realmodified = true;
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
            realmodified = true;
            strModified = setCharAt(strModified, 4, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 4, '0');
        }
    });
    $("#SortQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].SortCodeFinal != $("#SortQC").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 5, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 5, '0');
        }
    });
    $("#SANQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].SANFinal != $("#SANQC").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 6, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 6, '0');
        }
    });
    $("#TransQC").focusout(function () {
        Foutcnt = document.getElementById('cnt').value;
        if (tt[Foutcnt].TransCodeFinal != $("#TransQC").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 7, '1');
        }
        else {
            realmodified = false;
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

    //-------------------------------Common For Accept and Reject--- --------------------------
    function common(val) {
        //alert(document.getElementById('cbsdls').value);
        //alert('comman');
        debugger;
        if (Modernizr.localstorage) {
            var localacct = window.localStorage;
            var chqiwmicr = JSON.stringify(L2);
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

            var Tfimg = document.getElementById('myimg1');
            if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                document.getElementById('myimg1').style.display = "none";
            }

            document.getElementById('myimg').removeAttribute('src');
            document.getElementById('divtiff').style.display = "none";
            document.getElementById('myimg').style.display = "block";
            document.getElementById('myimg').src = RootUrl + 'Icons/Loading.jpg';

            var frontGreyimgUrl = tt[cnt].FrontGreyImagePath;
            debugger;
            if (Number(tt[cnt].FinalAmount) >= 200000) {

                var imgUrl = tt[cnt].FrontUVImagePath;
                $.ajax({
                    url: imgUrl,
                    type: 'HEAD',
                    error: function () {
                        //file not exists
                        alert('Image not loaded correctly!!!');
                        //document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;

                        $.ajax({
                            url: frontGreyimgUrl,
                            type: 'HEAD',
                            error: function () {
                                //file not exists
                                //alert('Image not loaded correctly!!!');
                                document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                            },
                            success: function () {
                                //file exists
                                console.log("Front image url success " + frontGreyimgUrl);
                                document.getElementById('myimg').src = frontGreyimgUrl;
                            }
                        });
                    },
                    success: function () {
                        //file exists
                        console.log("UV image url success " + imgUrl);
                        document.getElementById('myimg').src = imgUrl;
                    }
                });

                //document.getElementById('myimg').src = tt[cnt].FrontUVImagePath;
                //ChangeImage('FUV');
            }
            else {
                //ChangeImage('FGray');
                //document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;

                $.ajax({
                    url: frontGreyimgUrl,
                    type: 'HEAD',
                    error: function () {
                        //file not exists
                        alert('Image not loaded correctly!!!');
                        document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                    },
                    success: function () {
                        //file exists
                        console.log("Front image url success " + frontGreyimgUrl);
                        document.getElementById('myimg').src = frontGreyimgUrl;
                    }
                });
            }


            document.getElementById('Decision').value = "";
            document.getElementById('IWRemark').value = "";
            document.getElementById('rtncd').style.display = "none";
            document.getElementById('rejectreasondescrpsn').value = "";

            // document.getElementById("cbsdetails").innerHTML = "";
            //--------------------------------//
            //document.getElementById('ChqCnt').innerHTML = tt[cnt].SlipChequeCount;
            //document.getElementById('totamt').innerHTML = ChequeAmountTotal;

            if (tt[cnt].InstrumentType == "S") {
                document.getElementById('slpamt').style.display = "";
                document.getElementById('accnt').value = "";
                document.getElementById('slpamount').value = ""
                document.getElementById('chqamt').style.display = "none";
                document.getElementById('slpacnt').style.display = "";
                document.getElementById('Chqacnt').style.display = "none";
                document.getElementById('MICR').style.display = "none";
                document.getElementById('slpamount').value = addCommas(Number(tt[cnt].SlipAmount).toFixed(2));
                document.getElementById('Decision').focus();
                document.getElementById('divctsnoncts').style.display = "none";
                document.getElementById('divmarkp2f').style.display = "none";
                realAccModified = false;

            }
            else {
                realAccModified = false;

                document.getElementById('slpamt').style.display = "none";
                document.getElementById('chqamt').style.display = "";
                // document.getElementById('Chqacnt').style.display = "";
                //document.getElementById('chequeAcct').innerHTML = "";
                document.getElementById('slpacnt').style.display = "";
                document.getElementById('MICR').style.display = "";
                document.getElementById('ChqDate').value = "";
                document.getElementById('Amt').value = "";
                document.getElementById('ChqnoQC').value = "";
                document.getElementById('SortQC').value = "";
                document.getElementById('SANQC').value = "";
                document.getElementById('TransQC').value = "";
                document.getElementById('oldact').value = "";
                // document.getElementById('Slipamt').innerHTML = tt[cnt].SlipAmount;
                // document.getElementById('sliplabl').style.display = "";
                document.getElementById('divctsnoncts').style.display = "";
                document.getElementById('divmarkp2f').style.display = "";
                // document.getElementById('lblslpimg').style.display = "none";
                document.getElementById('DraweeName').value = "";
                realmodified = false;

                // if (tt[cnt].ClearingType == "01") {
                $('#ctsnocts').val(tt[cnt].ClearingType);
                // }
                if (tt[cnt].DocType.toUpperCase() == "C") {
                    document.getElementById("markp2f").checked = true;
                }
                else {
                    document.getElementById("markp2f").checked = false;
                }
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
                //------------------------------
                // document.getElementById('nartext').value = tempnarration;
                // document.getElementById('chequeAcct').innerHTML = tt[cnt].CreditAccountNo;
                document.getElementById('accnt').value = tt[cnt].CreditAccountNo;
                document.getElementById('oldact').value = tt[cnt].CreditAccountNo;
                document.getElementById('Decision').focus();

                //--------------------Added On 07-02-2017------------------
                debugger;

                document.getElementById('Amt').value = addCommas(Number(tt[cnt].FinalAmount).toFixed(2));
                ChqAmount = tt[cnt].FinalAmount;
                document.getElementById('ChqDate').value = fnldate;
                document.getElementById('ChqnoQC').value = tt[cnt].ChequeNoFinal;
                document.getElementById('SortQC').value = tt[cnt].SortCodeFinal;
                document.getElementById('SANQC').value = tt[cnt].SANFinal;
                document.getElementById('TransQC').value = tt[cnt].TransCodeFinal;

                document.getElementById('PayeeName').value = tt[cnt].PayeeName;
                document.getElementById('DraweeName').value = tt[cnt].DraweeName;

                //============ amol changes for setting the sourceOfFunds values on 02/11/2022 start ============
                debugger;
                $("#cbsdetails").empty();
                //$("#SrcFnds").empty();

                GetAcctDetails();

                var NRE_Id = 0;
                var NRO_Id = 0;
                $.ajax({
                    url: RootUrl + 'OWL2/GetNRE_NRO_Id_From_RawDataId',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'html',
                    data: { ID: tt[cnt].Id, RawDataId: tt[cnt].RawDataId },
                    async: false,
                    success: function (data) {
                        debugger;
                        var dataResult = JSON.parse(data);
                        NRE_Id = dataResult.NRESourceOfFundId;
                        NRO_Id = dataResult.NROSourceOfFundId;
                    }
                });

                tt[cnt].NRESourceOfFundId = NRE_Id;
                tt[cnt].NROSourceOfFundId = NRO_Id;

                if (tt[cnt].NRESourceOfFundId == 0 && tt[cnt].NROSourceOfFundId == 0) {
                    document.getElementById('SrcFnds').style.display = "none";
                }
                else {
                    document.getElementById('SrcFnds').style.display = "block";
                    //console.log("tt NRESourceOfFundId - " + tt[cnt].NRESourceOfFundId);
                    //console.log("ddl NRESourceOfFundId - " + document.getElementById('ddSrcFndsNRE').value);
                    if (tt[cnt].NRESourceOfFundId > 0) {
                        $.ajax({
                            url: RootUrl + 'OWL2/GetSourceofFundsFromNRE_NRO',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'html',
                            data: { ID: tt[cnt].NRESourceOfFundId, RecordID: tt[cnt].Id, RawDataId: tt[cnt].RawDataId, schemeType: 'NRE' },
                            async: false,
                            success: function (data) {
                                debugger;
                                document.getElementById('txtSrcFnds').value = '';
                                $("#txtSrcFnds").val(data);
                            }
                        });
                        //document.getElementById('ddSrcFndsNRE').selectedIndex = tt[cnt].NRESourceOfFundId;
                        //$("#txtSrcFnds").val($("#ddSrcFndsNRE option:selected").text());
                    }
                    else if (tt[cnt].NROSourceOfFundId > 0) {
                        $.ajax({
                            url: RootUrl + 'OWL2/GetSourceofFundsFromNRE_NRO',
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'html',
                            data: { ID: tt[cnt].NROSourceOfFundId, RecordID: tt[cnt].Id, RawDataId: tt[cnt].RawDataId, schemeType: 'NRO' },
                            async: false,
                            success: function (data) {
                                debugger;
                                document.getElementById('txtSrcFnds').value = '';
                                $("#txtSrcFnds").val(data);
                            }
                        });
                        //document.getElementById('ddSrcFndsNRO').selectedIndex = tt[cnt].NROSourceOfFundId;
                        //$("#txtSrcFnds").val($("#ddSrcFndsNRO option:selected").text());
                    }
                }
                //============ amol changes for setting the sourceOfFunds values on 02/11/2022 end ============

                document.getElementById('Decision').focus();
                tempAmtValue = tt[cnt].FinalAmount;
                document.getElementById('mtrn').value = tt[cnt].RawDataId;
                document.getElementById('bankname').style.display = "";
                //document.getElementById('lblslpimg').style.display = "";

                bankName(tt[cnt].SortCodeFinal);  //-------------For bank name

                if ($("#NarrationID").val() == "Y") {

                }
                //----------------------Set L1 and L2 Decision Color ----------------
                document.getElementById("L1rejectDecrp").innerHTML = "";

                if (tt[cnt].L1VerificationStatus == 2) {

                    document.getElementById("l1decision").innerHTML = "Y";
                    //document.getElementById('l1decision').style.background = "Green";
                    document.getElementById("l1decision").classList.add("w3-text-green");
                    document.getElementById('L1rejectDecrp').style.display = "none";
                }
                else if (tt[cnt].L1VerificationStatus == 3) {
                    document.getElementById("l1decision").innerHTML = "R";
                    document.getElementById("l1decision").classList.add("w3-text-red");
                    document.getElementById("L1rejectDecrp").innerHTML = "";
                    // document.getElementById('L1rejectDecrp').style.display = "";
                    // getL1Logs(tt[1].RawDataId);
                    getReturnDecrp(tt[cnt].L1RejectReason);
                    //  alert('alare');
                    //  alert($("#cbsdls"));
                }

                if (narrationReqirdflg == true) {
                    document.getElementById('narsndiv').style.display = "";
                    //document.getElementById('nartext').value = tt[cnt].UserNarration;
                }
                else {
                    document.getElementById('narsndiv').style.display = "none";
                }
            }

            //-------------------------------------- Modification HI-------------------------------
            //--------------------------------------------------------------
            document.getElementById('strbranchcd').innerHTML = tt[cnt].BranchCode;
            document.getElementById('ScanningID').innerHTML = tt[cnt].ScanningNodeId;
            document.getElementById('strBatchNo').innerHTML = tt[cnt].BatchNo;
            document.getElementById('strBatchSeqNO').innerHTML = tt[cnt].BatchSeqNo;
            //-----------------------------------------------------------------------------------

            //-------------Account-----------------------
            if (tt[cnt].Modified1.charAt(0) == "1") {
                document.getElementById("accnt").style.backgroundColor = "red";
            }
            else {
                document.getElementById("accnt").style.backgroundColor = "white";
            }
            //-------------Amount------------------------------------------------------------------
            if (tt[cnt].Modified1.charAt(2) == "1") {
                document.getElementById("Amt").style.backgroundColor = "red";
            }
            else {
                document.getElementById("Amt").style.backgroundColor = "white";
            }
            //-------------ChqDate-----------------------------------------------------------------
            if (tt[cnt].Modified1.charAt(3) == "1") {
                document.getElementById("ChqDate").style.backgroundColor = "red";
            }
            else {
                document.getElementById("ChqDate").style.backgroundColor = "white";
            }
            //-------------ChqNo--------------------------------------------------------------------
            if (tt[cnt].Modified1.charAt(4) == "1") {
                document.getElementById("ChqnoQC").style.backgroundColor = "red";
            }
            else {
                document.getElementById("ChqnoQC").style.backgroundColor = "white";
            }
            //-------------SortQC-------------------------------------------------------------------
            if (tt[cnt].Modified1.charAt(5) == "1") {
                document.getElementById("SortQC").style.backgroundColor = "red";
            }
            else {
                document.getElementById("SortQC").style.backgroundColor = "white";
            }
            //-------------SortQC-------------------------------------------------------------------
            if (tt[cnt].Modified1.charAt(6) == "1") {
                document.getElementById("SANQC").style.backgroundColor = "red";
            }
            else {
                document.getElementById("SANQC").style.backgroundColor = "white";
            }
            //-------------TransQC-------------------------------------------------------------------
            if (tt[cnt].Modified1.charAt(7) == "1") {
                document.getElementById("TransQC").style.backgroundColor = "red";
            }
            else {
                document.getElementById("TransQC").style.backgroundColor = "white";
            }

            //  cbsbtn = false;

            // document.getElementById("h2amt").innerHTML = "{{iwAmt|number:2}}";
            if (backbtn == true) {
                document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
            }
            else {
                document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
            }

            backbtn = false;
            document.getElementById("btnback").disabled = true
        } //if cnt < tt
        else if (cnt > 0) {
            debugger;
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
                        debugger;
                        var orderData = JSON.parse(localData.getItem("owL2" + i));

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
                            arrlist.push(orderData.UserNarration);
                            arrlist.push(orderData.rejectreasondescrpsn);
                            arrlist.push(orderData.ctsNonCtsMark);
                            arrlist.push(orderData.P2fMark);
                            arrlist.push(orderData.SlipID);
                            arrlist.push(orderData.SlipRawaDataID);
                            arrlist.push(orderData.ScanningType);
                            arrlist.push(orderData.Modified2);
                            arrlist.push(orderData.DraweeName);
                            arrlist.push(orderData.NRESourceOfFundId);
                            arrlist.push(orderData.NROSourceOfFundId);

                            //=================== Added by Amol on 01/03/2024 =============
                            arrlist.push(orderData.API_Data);
                            //=================== Added by Amol on 21/03/2024 =============
                            arrlist.push(orderData.IsOpenedDateOld);

                            arrlist.push(orderData.SrcFndsDescription);
                            arrlist.push(orderData.NROSrcFndsDescription);
                        }

                    }
                }
                else {


                    for (var i = 1; i < cnt; i++) {
                        var orderData = JSON.parse(localData.getItem("owL2" + i));

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
                            arrlist.push(orderData.UserNarration);
                            arrlist.push(orderData.rejectreasondescrpsn);
                            arrlist.push(orderData.ctsNonCtsMark);
                            arrlist.push(orderData.P2fMark);
                            arrlist.push(orderData.SlipID);
                            arrlist.push(orderData.SlipRawaDataID);
                            arrlist.push(orderData.ScanningType);
                            arrlist.push(orderData.Modified2);
                            arrlist.push(orderData.DraweeName);
                            arrlist.push(orderData.NRESourceOfFundId);
                            arrlist.push(orderData.NROSourceOfFundId);

                            //=================== Added by Amol on 01/03/2024 =============
                            arrlist.push(orderData.API_Data);
                            //=================== Added by Amol on 21/03/2024 =============
                            arrlist.push(orderData.IsOpenedDateOld);

                            arrlist.push(orderData.SrcFndsDescription);
                            arrlist.push(orderData.NROSrcFndsDescription);
                        }
                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------

                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;


                $.ajax({

                    url: RootUrl + 'OWL2/OWL2Chq',
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
                                var owL2 = "owL20"

                                InstrumentType = tt[1].InstrumentType;

                                if (tt[0].callby == "Cheq") {

                                    var L2 = {
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
                                        "DraweeName": tt[0].DraweeName,
                                        "L1RejectReason": tt[0].L1RejectReason,
                                        "L1VerificationStatus": tt[0].L1VerificationStatus,
                                        "modified": tt[0].modified,
                                        "UserNarration": tt[0].UserNarration,
                                        "rejectreasondescrpsn": tt[0].RejectReasonDescription,
                                        "ctsNonCtsMark": tt[0].ctsNonCtsMark,
                                        "P2fMark": tt[0].P2fMark,
                                        "SlipID": tt[0].SlipID,
                                        "SlipRawaDataID": tt[0].SlipRawaDataID,
                                        "ScanningType": tt[0].ScanningType,
                                        "Modified2": tt[0].Modified2,
                                        "DraweeName": tt[0].DraweeName,
                                        "NRESourceOfFundId": tt[0].NRESourceOfFundId,
                                        "NROSourceOfFundId": tt[0].NROSourceOfFundId,

                                        "API_Data": tt[0].API_Data,
                                        "IsOpenedDateOld": tt[0].IsOpenedDateOld,

                                        "SrcFndsDescription": tt[0].SrcFndsDescription,
                                        "NROSrcFndsDescription": tt[0].NROSrcFndsDescription,
                                    };
                                    if (Modernizr.localstorage) {

                                        var localacct = window.localStorage;
                                        var chqiwmicr = JSON.stringify(L2);
                                        localacct.setItem(owL2, chqiwmicr);

                                    }
                                }

                                // alert(tt[1].FrontGreyImagePath);

                                // debugger;
                                //----------------------------------------------------------------------//~/Icons/noimagefound.jpg
                                var Tfimg = document.getElementById('myimg1');
                                if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                                    document.getElementById('myimg1').style.display = "none";
                                }

                                document.getElementById('myimg').removeAttribute('src');
                                document.getElementById('divtiff').style.display = "none";
                                document.getElementById('myimg').style.display = "block";
                                document.getElementById('myimg').src = RootUrl + 'Icons/Loading.jpg';

                                var frontGreyimgUrl = tt[1].FrontGreyImagePath;
                                debugger;
                                if (Number(tt[1].FinalAmount) >= 200000) {

                                    var imgUrl = tt[1].FrontUVImagePath;
                                    $.ajax({
                                        url: imgUrl,
                                        type: 'HEAD',
                                        error: function () {
                                            //file not exists
                                            alert('Image not loaded correctly!!!');
                                            //document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

                                            $.ajax({
                                                url: frontGreyimgUrl,
                                                type: 'HEAD',
                                                error: function () {
                                                    //file not exists
                                                    //alert('Image not loaded correctly!!!');
                                                    document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                                                },
                                                success: function () {
                                                    //file exists
                                                    console.log("Front image url success " + frontGreyimgUrl);
                                                    document.getElementById('myimg').src = frontGreyimgUrl;
                                                }
                                            });

                                        },
                                        success: function () {
                                            //file exists
                                            console.log("UV image url success " + imgUrl);
                                            document.getElementById('myimg').src = imgUrl;
                                        }
                                    });

                                    //document.getElementById('myimg').src = tt[1].FrontUVImagePath;
                                }
                                else {
                                    //document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

                                    $.ajax({
                                        url: frontGreyimgUrl,
                                        type: 'HEAD',
                                        error: function () {
                                            //file not exists
                                            alert('Image not loaded correctly!!!');
                                            document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                                        },
                                        success: function () {
                                            //file exists
                                            console.log("Front image url success " + frontGreyimgUrl);
                                            document.getElementById('myimg').src = frontGreyimgUrl;
                                        }
                                    });
                                }

                                document.getElementById('Decision').value = "";
                                document.getElementById('IWRemark').value = "";
                                document.getElementById('rtncd').style.display = "none";
                                document.getElementById('rejectreasondescrpsn').value = "";

                                cbsbtn = false;
                                // debugger;
                                //----------------------Set L1 and L2 Decision Color ----------------
                                // alert(tt[1].L1VerificationStatus);
                                document.getElementById("l1decision").innerHTML = "";
                                document.getElementById("L1rejectDecrp").innerHTML = "";
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
                                    getReturnDecrp(tt[1].L1RejectReason);
                                }

                                if (tt[1].InstrumentType == "S") {

                                    if (tt[1].ScanningType == 3 || tt[1].ScanningType == 5) {

                                        $("#ClientCd").prop('disabled', true);
                                        document.getElementById('lblpayee').innerHTML = "";
                                        document.getElementById('ClntsDtlsdiv').style.display = "";
                                        document.getElementById('Chqacnt').style.display = "";
                                        document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;
                                        document.getElementById('ClientCd').value = tt[1].ClientCode;
                                        debugger;
                                        $.ajax({
                                            url: RootUrl + 'OWL2/GetCBSDetailsWithAPI',
                                            dataType: 'html',
                                            //data: { ac: tt[1].CreditAccountNo, callby: "ClientCode" },
                                            data: { ac: tt[1].CreditAccountNo },
                                            success: function (data) {
                                                cbsbtn = true;
                                                $('#cbsdetails').html(data);
                                                var vPayee = document.getElementById("Payee").value;
                                                $("#PayeeName").val(vPayee);
                                                vPayee = "";

                                                //if (tt[1].FinalAmount >= 200000)
                                                //    ChangeImage('FUV');
                                                //else
                                                //    ChangeImage('FGray');
                                            }
                                        });

                                        if ($("#ClientCd").val() != "") {
                                            // alert('call');
                                            $.ajax({
                                                url: RootUrl + 'OWL2/GetClientDlts',
                                                dataType: 'html',
                                                data: { ac: $("#ClientCd").val() },
                                                success: function (data) {
                                                    cbsbtn = true;
                                                    $('#clientdetails').html(data);

                                                }
                                            });
                                            //clientdtls();
                                        }
                                        document.getElementById('Decision').focus();
                                        //document.getElementById("Payee").disabled = true;
                                        //   $("#Payee").prop('disabled', 'disabled');
                                        //$("#Payee").removeAttr("disabled");

                                    }
                                    else {
                                        document.getElementById('slpacnt').style.display = "";
                                        document.getElementById('Decision').focus();
                                        document.getElementById('Chqacnt').style.display = "none";
                                        document.getElementById('accnt').value = tt[1].CreditAccountNo;
                                        document.getElementById('Decision').focus();
                                        //--------------------Added On 07-02-2017------------------
                                        document.getElementById('oldact').value = tt[1].CreditAccountNo;
                                        var aNew = $("#accnt").val();
                                        debugger;
                                        $.ajax({
                                            url: RootUrl + 'OWL2/GetCBSDetailsWithAPI',
                                            dataType: 'html',
                                            //data: { ac: $("#accnt").val(), strcbsdls: tt////[1].CBSAccountInformation, strJoinHldrs: tt[1].CBSJointAccountInformation, callby: "Normal", payeename: tt
                                            //[1].PayeeName },
                                            data: { ac: $("#accnt").val() },

                                            success: function (data) {
                                                cbsbtn = true;
                                                $('#cbsdetails').html(data);
                                                var vPayee = document.getElementById("Payee").value;
                                                $("#PayeeName").val(vPayee);
                                                vPayee = "";

                                                //if (tt[1].FinalAmount >= 200000)
                                                //    ChangeImage('FUV');
                                                //else
                                                //    ChangeImage('FGray');
                                            }
                                        });

                                        //---------------Select payee name as per L2 verification-------
                                        //var el = document.getElementById("Payee");
                                        //for (var i = 0; i < el.options.length; i++) {
                                        //    if (el.options[i].text == tt[1].PayeeName) {
                                        //        el.selectedIndex = i;
                                        //        break;
                                        //    }
                                        //}
                                    }
                                    //---------------------Account---------Checking-----------------

                                    document.getElementById('accnt').focus();
                                    if ($("#NarrationID").val() == "Y") {
                                        //document.getElementById('nartext').value = tt[1].UserNarration;
                                        //$('#nartext').attr('readonly', false);
                                    }

                                    //--------------------------------------------------
                                    document.getElementById('slpamt').style.display = "";
                                    document.getElementById('chqamt').style.display = "none";
                                    document.getElementById('slpamount').value = ""
                                    document.getElementById('MICR').style.display = "none";
                                    document.getElementById('slpamount').value = addCommas(Number(tt[1].SlipAmount).toFixed(2));
                                    document.getElementById('sliplabl').style.display = "none";
                                    document.getElementById('Slipamt').innerHTML = "";
                                    document.getElementById('divctsnoncts').style.display = "none";
                                    document.getElementById('divmarkp2f').style.display = "none";
                                    realAccModified = false;
                                    document.getElementById('mtrn').value = tt[1].RawDataId;
                                    document.getElementById('bankname').style.display = "none";
                                    //document.getElementById('lblslpimg').style.display = "none";

                                }
                                else {
                                    //alert('Else');
                                    realAccModified = false;
                                    document.getElementById("btnback").disabled = true;//prev false

                                    //   document.getElementById('slpamt').style.display = "none";
                                    document.getElementById('chqamt').style.display = "";
                                    document.getElementById('slpacnt').style.display = "";
                                    document.getElementById('MICR').style.display = "";
                                    document.getElementById('ChqDate').value = "";
                                    document.getElementById('Amt').value = "";
                                    document.getElementById('ChqnoQC').value = "";
                                    document.getElementById('SortQC').value = "";
                                    document.getElementById('SANQC').value = "";
                                    document.getElementById('TransQC').value = "";
                                    document.getElementById('oldact').value = "";
                                    document.getElementById('PayeeName').value = "";
                                    document.getElementById('DraweeName').value = "";
                                    document.getElementById('divctsnoncts').style.display = "";
                                    document.getElementById('divmarkp2f').style.display = "";

                                    document.getElementById('accnt').value = tt[1].CreditAccountNo;
                                    document.getElementById('oldact').value = tt[1].CreditAccountNo;
                                    document.getElementById('Decision').focus();

                                    realmodified = false;
                                    //   debugger;
                                    $('#ctsnocts').val(tt[1].ClearingType);

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
                                    debugger;
                                    //------------------------------
                                    //tempnarration = tt[1].UserNarration;
                                    //document.getElementById('nartext').value = tempnarration;
                                    document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;
                                    document.getElementById('Amt').value = addCommas(Number(tt[1].FinalAmount).toFixed(2));
                                    document.getElementById('ChqDate').value = fnldate;
                                    document.getElementById('ChqnoQC').value = tt[1].ChequeNoFinal;
                                    document.getElementById('SortQC').value = tt[1].SortCodeFinal;
                                    document.getElementById('SANQC').value = tt[1].SANFinal;
                                    document.getElementById('TransQC').value = tt[1].TransCodeFinal;

                                    document.getElementById('PayeeName').value = tt[1].PayeeName;

                                    randomPayeeName = tt[1].PayeeName;

                                    document.getElementById('DraweeName').value = tt[1].DraweeName;

                                    //============ amol changes for setting the sourceOfFunds values on 02/11/2022 start ============
                                    debugger;
                                    $("#cbsdetails").empty();
                                    //$("#SrcFnds").empty();

                                    GetAcctDetails();

                                    if (tt[1].NRESourceOfFundId == 0 && tt[1].NROSourceOfFundId == 0) {
                                        document.getElementById('SrcFnds').style.display = "none";
                                    }
                                    else {
                                        document.getElementById('SrcFnds').style.display = "block";
                                        if (tt[1].NRESourceOfFundId > 0) {
                                            $.ajax({
                                                url: RootUrl + 'OWL2/GetSourceofFundsFromNRE_NRO',
                                                contentType: 'application/json; charset=utf-8',
                                                dataType: 'html',
                                                data: { ID: tt[1].NRESourceOfFundId,RecordID: tt[1].Id, RawDataId: tt[1].RawDataId, schemeType: 'NRE' },
                                                async: false,
                                                success: function (data) {
                                                    debugger;
                                                    document.getElementById('txtSrcFnds').value = '';
                                                    $("#txtSrcFnds").val(data);
                                                }
                                            });
                                            //document.getElementById('ddSrcFndsNRE').selectedIndex = tt[1].NRESourceOfFundId;
                                            //$("#txtSrcFnds").val($("#ddSrcFndsNRE option:selected").text());
                                        }
                                        else if (tt[1].NROSourceOfFundId > 0) {
                                            $.ajax({
                                                url: RootUrl + 'OWL2/GetSourceofFundsFromNRE_NRO',
                                                contentType: 'application/json; charset=utf-8',
                                                dataType: 'html',
                                                data: { ID: tt[1].NROSourceOfFundId,RecordID: tt[1].Id, RawDataId: tt[1].RawDataId, schemeType: 'NRO' },
                                                async: false,
                                                success: function (data) {
                                                    debugger;
                                                    document.getElementById('txtSrcFnds').value = '';
                                                    $("#txtSrcFnds").val(data);
                                                }
                                            });
                                            //document.getElementById('ddSrcFndsNRO').selectedIndex = tt[1].NROSourceOfFundId;
                                            //$("#txtSrcFnds").val($("#ddSrcFndsNRO option:selected").text());
                                        }
                                    }

                                    //============ amol changes for setting the sourceOfFunds values on 02/11/2022 end ============

                                    document.getElementById('Decision').focus();

                                    document.getElementById('mtrn').value = tt[1].RawDataId;

                                    document.getElementById('bankname').style.display = "";
                                    bankName(tt[1].SortCodeFinal);  //-------------For bank name


                                    document.getElementById('accnt').focus();

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
                                    //-----------------Commented on 03/12/2016-----
                                    if ($("#NarrationID").val() == "Y") {
                                        //document.getElementById('nartext').value = tt[1].UserNarration;
                                        //$('#nartext').attr('readonly', false);
                                    }

                                    debugger;

                                }

                                document.getElementById('accnt').focus();
                                //-------------------------------------- Modification HI-------------------------------
                                //-------------Account-----------------------
                                if (tt[1].Modified1.charAt(0) == "1") {
                                    document.getElementById("accnt").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("accnt").style.backgroundColor = "white";
                                }

                                //-------------Amount------------------------------------------------------------------
                                if (tt[1].Modified1.charAt(2) == "1") {
                                    document.getElementById("Amt").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("Amt").style.backgroundColor = "white";
                                }
                                //-------------ChqDate-----------------------------------------------------------------
                                if (tt[1].Modified1.charAt(3) == "1") {
                                    document.getElementById("ChqDate").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("ChqDate").style.backgroundColor = "white";
                                }
                                //-------------ChqNo--------------------------------------------------------------------
                                if (tt[1].Modified1.charAt(4) == "1") {
                                    document.getElementById("ChqnoQC").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("ChqnoQC").style.backgroundColor = "white";
                                }
                                //-------------SortQC-------------------------------------------------------------------
                                if (tt[1].Modified1.charAt(5) == "1") {
                                    document.getElementById("SortQC").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("SortQC").style.backgroundColor = "white";
                                }
                                //-------------SortQC-------------------------------------------------------------------
                                if (tt[1].Modified1.charAt(6) == "1") {
                                    document.getElementById("SANQC").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("SANQC").style.backgroundColor = "white";
                                }
                                //-------------TransQC-------------------------------------------------------------------
                                if (tt[1].Modified1.charAt(7) == "1") {
                                    document.getElementById("TransQC").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("TransQC").style.backgroundColor = "white";
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

    clientdtls = function () {
        //alert('aya');
        $.ajax({
            url: RootUrl + 'OWL2/GetClientDlts',
            dataType: 'html',
            data: { ac: $("#ClientCd").val() },
            success: function (data) {
                $('#clientdetails').html(data);
                cbsbtn = true;
            }
        });
    }

    getcbsdtls = function () {
        debugger;
        //alert('Smile!!');
        var Acct = document.getElementById('accnt').value;
        if (Acct.length != 16) {
            alert("Account no should be of 16 digits!");
            document.getElementById('accnt').focus();
            return false;
        }
        if (isNaN(Acct)) {
            alert("Account no can only accept numeric values!");
            document.getElementById('accnt').focus();
            return false;
        }
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
            var aNew = $("#accnt").val();
            //debugger;
            $.ajax({
                //url: RootUrl + 'OWL2/GetCBSDtls',
                url: RootUrl + 'OWL2/GetCBSDetailsWithAPI',
                dataType: 'html',
                data: { ac: $("#accnt").val() },
                success: function (data) {
                    cbsbtn = true;
                    $('#cbsdetails').html(data);
                    document.getElementById('Decision').focus();
                    var vPayee = document.getElementById("Payee").value;
                    $("#PayeeName").val(vPayee);
                    vPayee = "";
                }
            });
        }
    }

    var value = 0;
    callrotate = function () {
        //value += 180;
        //$("#myimg,#myimg1").rotate({ animateTo: value })
        if (value == 0) {
            value += 180;
            $("#myimg,#myimg1").rotate({ animateTo: value })
        }
        else {
            value += 180;
            $("#myimg,#myimg1").rotate({ animateTo: value })
            value = 0;
        }
    }


});

//-----------------------Activity Logs----------------
function getowlogs() {

    // debugger;
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
            url: RootUrl + 'OWL2/getOWlogs',
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

function fullImageTiff() {
    //debugger;
    //alert('ok');
    console.log("zoom tiff");
    document.getElementById('iwimg1').style.display = 'block';
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg1').src = document.getElementById('myimg1').src;
};
function isNumberWithDot(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    console.log(charCode);

    if (charCode >= 48 || charCode <= 57 || charCode == 46)
        return true;
    return false;
}

function isNumberWith(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    console.log(charCode);

    if (charCode >= 48 && charCode <= 57)
        return true;
    return false;
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
}

function getSrcFndsNRE() {
    $("#txtSrcFnds").val($("#ddSrcFndsNRE option:selected").text());
}

function getSrcFndsNRO() {
    $("#txtSrcFnds").val($("#ddSrcFndsNRO option:selected").text());
}

function getReturnDecrp(rtncode) {
    //alert('ok');
    var valTempcnt = document.getElementById('cnt').value;
    var rjctresnl = document.getElementById('rtnlist');
    var rtnlistDescrp = document.getElementById('rtnlistDescrp');
    for (var i = 0; i < rjctresnl.length; i++) {
        //alert(rtncode);
        if (rtncode == rjctresnl[i].value) {
            if (rtncode == "88") {

                document.getElementById("L1rejectDecrp").innerHTML = tt[valTempcnt].RejectReasonDescription;
            }
            else {
                document.getElementById("L1rejectDecrp").innerHTML = rtnlistDescrp[i].value;
            }

            document.getElementById('L1rejectDecrp').style.display = "";
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
    document.getElementById('IWRemark').value = rtnval;
    document.getElementById('RejectReason').style.display = 'none';
    var rejctrcd = $("#IWRemark").val();
    if (rejctrcd.length == 2) {
        var rjctresnlTemp = document.getElementById('rtnlist');
        var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
        for (var i = 0; i < rjctresnlTemp.length; i++) {
            if (rejctrcd == rjctresnlTemp[i].value) {
                //if (rejctrcd == "88") {
                //    document.getElementById("rejectreasondescrpsn").value = rtnrjctdescrn;
                //}
                //else {
                document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
                //}
                break;
            }
        }
    }
    // }
    document.getElementById('ok').focus();
}
//---------------
function valid(RejectDescription, Reasoncode) {
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
    debugger;
    //if (document.getElementById('sCAPA').value == "NRESA" || document.getElementById('sCAPA').value == "NROSA" || document.getElementById('sCAPA').value == "NRESP" || document.getElementById('sCAPA').value == "NROSP" || document.getElementById('sCAPA').value == "NRETR" || document.getElementById('sCAPA').value == "NROTR" || document.getElementById('sCAPA').value == "NRET1" || document.getElementById('sCAPA').value == "NROT1" || document.getElementById('sCAPA').value == "NRET3" || document.getElementById('sCAPA').value == "NEPIS" || document.getElementById('sCAPA').value == "NOPIS" || document.getElementById('sCAPA').value == "NREWL" || document.getElementById('sCAPA').value == "NROWL" || document.getElementById('sCAPA').value == "NRSAV" || document.getElementById('sCAPA').value == "SFNRE" || document.getElementById('sCAPA').value == "SFNRO") {
    //    alert("NR Account!");
    //}
    var IWdecn = document.getElementById('Decision').value.toUpperCase();
    var valcnt = document.getElementById('cnt').value;
    var acmin = document.getElementById('acmin').value;

    //----------------------------Amount---------------------//
    amt = document.getElementById('Amt').value;

    if (amt == "NaN" || parseFloat(amt) == 0 || amt == "") {
        alert('Enter valid amount!');
        document.getElementById('Amt').focus();

        return false;
    }

    var ChqnoQC = document.getElementById('ChqnoQC').value;
    var SortQC = document.getElementById('SortQC').value;
    var SANQC = document.getElementById('SANQC').value;
    var TransQC = document.getElementById('TransQC').value;

    var numbers = /^[0-9]+$/;

    if (!ChqnoQC.match(numbers)) {
        alert('Please enter correct cheque No!');
        document.getElementById('ChqnoQC').focus();
        return false;
    }

    if (!SortQC.match(numbers)) {
        alert('Please enter correct sort code No!');
        document.getElementById('SortQC').focus();
        return false;
    }

    if (!SANQC.match(numbers)) {
        alert('Please enter correct SAN No!');
        document.getElementById('SANQC').focus();
        return false;
    }

    if (!TransQC.match(numbers)) {
        alert('Please enter correct Trans Code!');
        document.getElementById('TransQC').focus();
        return false;
    }

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
    else if (SANQC.length > 6 && TransQC.length < 3) {
        alert("Please enter valid SAN and TransCode !");
        document.getElementById('SANQC').focus();
        return false;
    }
    else if (TransQC.length > 2 && SANQC.length < 7) {
        alert("Please enter valid SAN and TransCode !");
        document.getElementById('SANQC').focus();
        return false;
    }

    var rtnflg = validYrnscodes();
    if (rtnflg == false) {
        alert("Trans code is not valid !");
        document.getElementById('TransQC').focus();
        document.getElementById('TransQC').select();
        return false;
    }

    //debugger;
    //Acct = Acct.replace(/^0+/, '')
    //--------------- //-------------------------------------Validate Narration----On 07-02-2017-----------
    var TempAcct = document.getElementById('accnt').value;
    // narrationReqirdflg = false;
    // alert($("#nartext").val());
    if (TempAcct.length == 14) {
        if (TempAcct == "06410125027255") {
            document.getElementById('narsndiv').style.display = "";
            narrationReqirdflg = true;
        }
        else {
            narrationAC(TempAcct);
        }
    }

    //=========== High value cheque validation changes by Amol on 15/02/2024 start ============
    let userConfirmationHighAmount = false;
    let highValue = document.getElementById('HighValueChqAmt').value;
    let chequeAmt = document.getElementById('Amt').value.replace(/,/g, '');

    if (Number(chequeAmt) >= Number(highValue)) {
        //alert('High value amount of cheque !!!');

        userConfirmationHighAmount = confirm("High value amount of cheque !!!");
        if (userConfirmationHighAmount == false) {
            return false;
        }
    }
    //=========== High value cheque validation changes by Amol on 15/02/2024 end ============

    //=========== opened account date older than 6 months validation changes by Amol on 23/05/2024 start ============
    let userConfirmationOld = false;
    let isOpenedDateOld = document.getElementById('IsOpenedDateOld').value;

    if (isOpenedDateOld == "N") {
        //alert('Less than 6 months old account. Kindly check KYC and EDD.!!!');

        userConfirmationOld = confirm("Less than 6 months old account. Kindly check KYC and EDD.!!!");
        if (userConfirmationOld == false) {
            return false;
        }
    }
    //=========== opened account date older than 6 months validation changes by Amol on 23/05/2024 end ============

    if (tt[valcnt].InstrumentType == "S") {
        var Acct = document.getElementById('accnt').value;
        Acct = Acct.length

        // var tempacnt = $("#cnt").val();//documemt.getElementById('').value;

        if (tt[valcnt].DbtAccountNo != $("#accnt").val() && cbsbtn == false) {
            alert("Click on 'GetDetails' button/press F12 !");
            document.getElementById('accnt').focus();
            return false;
        }

        if (Acct == "") {
            alert("Account no field should not be empty !");
            document.getElementById('accnt').focus();
            return false;
        }
        //debugger;
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
            alert("Click on 'GetDetails'!");
            document.getElementById('accnt').focus();
            return false;
        }

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
        amt = amt.replace(/^0+/, '')
        amt = amt.length;
        if (amt1 == ".") {
            alert('Amount field should not be dot(.) !');
            document.getElementById('slpamount').focus();
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

        //-------------------------ChqDate-------------------------------------------------//

        if (IWdecn == "A") {

            //====================Account start ===============================
            debugger;
            if (document.getElementById('accnt').value.length < acmin || document.getElementById('accnt').value == "") {

                alert("Minimum account no sould be " + acmin + " digits");
                document.getElementById('accnt').focus();
                return false;
            }

            //====================Account End ===============================

            //============ Added on 03/10/2023 by amol for payeename validation start ===========
            debugger;
            var pName = document.getElementById('PayeeName').value.trim();
            if (pName == "") {
                alert("Please enter payee name");
                return false;
            }

            //var regexPattern = /^[a-zA-Z0-9 ]*$/;
            //if (regexPattern.test(pName)) {
            //    // If the input is valid, do nothing or perform desired actions
            //} else {
            //    // If the input contains special characters, remove them from the input
            //    //textBox.value = inputValue.replace(/[^a-zA-Z0-9 ]/g, '');
            //    alert("Please do enter special characters");
            //    return false;
            //}
            //============ Added on 03/10/2023 by amol for payeename validation end ===========

            //====================Date start ===============================
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

                ///------------------------------------Post Date and Stale Cheques ----///
                var postdate = document.getElementById('postdt').value;
                var staledate = document.getElementById('staledt').value;

                var fnldate;
                if (finldat.length < 10) {
                    fnldate = '20' + yy + '-' + mm + '-' + dd;
                }
                else {
                    fnldate = finldat;
                }

                userConfirmationPD = false;
                if ((postdate <= fnldate) || (staledate >= fnldate)) {
                    userConfirmationPD = confirm("Do you want to keep post or stale date like this");
                    //if (userConfirmationPD == true && document.getElementById("blockkey").value == "0" && 		(document.getElementById('Decision').value == "A" || document.getElementById('Decision').value == "a"))
                    if (userConfirmationPD == true && document.getElementById("blockkey").value == "0") {
                        //return true;
                    }
                    else if (userConfirmationPD == true && document.getElementById("blockkey").value == "1"
                        && (document.getElementById('Decision').value == "R" || document.getElementById('Decision').value == "r")) {
                        //return true;
                    }
                    else {
                        document.getElementById('ChqDate').focus();
                        document.getElementById('ChqDate').select();
                        return false;
                    }
                }

            }

            //====================Date end ===============================
        }

    }


    


    //if (document.getElementById('partialSrcFnds').value == "N") {
    //    var nreOrnro = document.getElementById('NreOrNro').value;
    //    document.getElementById('SrcFnds').style.display = "block";
    //    if (nreOrnro == "NRE") {
    //        if (document.getElementById('ddSrcFndsNRE').value == "" || document.getElementById('ddSrcFndsNRE').value == null || document.getElementById('ddSrcFndsNRE').value == 0) {
    //            alert("Select NRE Source of Funds!");
    //            return false;
    //        }
    //    }
    //    else if (nreOrnro == "NRO") {
    //        if (document.getElementById('sddSrcFndsNRO').value == "" || document.getElementById('ddSrcFndsNRO').value == null || document.getElementById('ddSrcFndsNRO').value == 0) {
    //            alert("Select NRO Source of Funds!");
    //            return false;
    //        }
    //    }

    //}


    //----------------DBTAC-----------------------

    if (IWdecn == "R") {
        randomPayeeName = "No Name";
    }
    else {
        if (tt[valcnt].ScanningType == 3 || tt[valcnt].ScanningType == 5) {
            randomPayeeName = document.getElementById('txtpayee').value;
        }
        else {
            //randomPayeeName = document.getElementById('Payee').value;
        }
    }

    
    if (IWdecn == "") {
        alert('Please enter decision!');
        document.getElementById('Decision').focus();
        return false;
    }

    else if (IWdecn != "A" && IWdecn != "R" && IWdecn != "F" && IWdecn != "H") {
        // alert(IWdecn);
        alert('Decision not correct!');
        document.getElementById('Decision').focus();
        return false;
    }
    else if (IWdecn == "A") {

        //================== Added currency validation on 02/03/2024 by amol start ======================
        debugger;
        var newAPICall = document.getElementById('NewApiCall').value;

        if (newAPICall == "Y" || newAPICall == "y") {
            if (document.getElementById("currencyVal").value !== "INR") {
                alert('Non INR Currency Account!!');
                return false;
            }

            if (document.getElementById("productCode").value == "INCOM") {
                alert('INCOME accounts cannot be used for Outward Clearing!!');
                return false;
            }

            let productType = document.getElementById("productType").value;
            if (productType !== "OAB") {
                let userConfirmationAccountNegativeBalance = false;
                let balAmt = Number(document.getElementById("accountBalances").value);
                if (balAmt < 0) {
                    //alert('Effective available Balance is Negative!!');
                    //return false;

                    userConfirmationAccountNegativeBalance = confirm("Effective available Balance is Negative.");

                    if (userConfirmationAccountNegativeBalance == false) {
                        return false;
                    }
                }
                
            }

            //=========== account status validation changes by Amol on 15/10/2024 start ============
            let userConfirmationAccountStatusDormant = false;
            let isAccountStatusDormant = document.getElementById('accountStatus').value;

            if (isAccountStatusDormant == "Dormant") {

                userConfirmationAccountStatusDormant = confirm("Dormant Account.Do you want to proceed?");

                if (userConfirmationAccountStatusDormant == false) {
                    return false;
                }
            }

            let userConfirmationAccountStatusInactive = false;
            let isAccountStatusInactive = document.getElementById('accountStatus').value;

            if (isAccountStatusInactive == "Inactive") {

                userConfirmationAccountStatusInactive = confirm("Inactive Account.Do you want to proceed?");

                if (userConfirmationAccountStatusInactive == false) {
                    return false;
                }
		        else
                {
                    document.getElementById("blockkey").value = '0'
                }
            }
            //=========== account status validation changes by Amol on 15/10/2024 end ============

            //=========== account freeze status code validation changes by Amol on 15/10/2024 start ============
            let userConfirmationAccountFreezeStatusCode = false;
            let isAccountFreezeStatusCode = document.getElementById('freezeStatusCode').value;

            if (isAccountFreezeStatusCode == "D") {

                userConfirmationAccountFreezeStatusCode = confirm("Debit Freeze Account.Do you want to proceed?");

                if (userConfirmationAccountFreezeStatusCode == false) {
                    return false;
                }
		        else
                {
                    document.getElementById("blockkey").value = '0'
                }
            }
            //=========== account freeze status code validation changes by Amol on 15/10/2024 end ============
        }
        
        //================== Added currency validation on 02/03/2024 by amol end ======================

        //================== Added marp2f validation on 21/03/2024 by amol start ======================
        var chkMarkP2F = document.getElementById("markp2f");

        // Check if the checkbox is checked
        if (chkMarkP2F.checked) {
            //console.log("Checkbox is checked");
            alert('You can not accept, please uncheck the MarkP2F checkbox!!');
            return false;
        }
        //================== Added marp2f validation on 21/03/2024 by amol end ======================

        
        
        //========== Sorcode validation added by amol on 01/06/2024 start =======
        let isSortCodeValidation = document.getElementById('isSortCodeValidation').value;

        if (isSortCodeValidation == "Y") {
            var resultFlg = sortCodeValidation();
            if (resultFlg == false) {
                //alert("Sort Code is not valid !");

                let userConfirmationSortCode = false;
                userConfirmationSortCode = confirm("Sort Code is not valid.!!!");

                if (userConfirmationSortCode == false) {
                    document.getElementById('SortQC').focus();
                    document.getElementById('SortQC').select();
                    return false;
                }
            }
        }
        //========== Sorcode validation added by amol on 01/06/2024 end =======
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

    var c = document.getElementById("blockkey").value;
    //debugger;

    chr = document.getElementById('Decision').value.toUpperCase();
    var chr = document.getElementById('Decision').value.toUpperCase();
    var iwrk = document.getElementById('IWRemark').value;
    if (chr == "R" || chr == "r") {
        if (iwrk == "") {
            document.getElementById('rtncd').style.display = "";
            document.getElementById('IWRemark').style.width = "10%";
            //document.getElementById('IWRemark').focus();
            document.getElementById('idReason').focus();
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

    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}
function ChangeImage(imagetype) {
    debugger;
    //alert('ok');$('#checkbox_ID').is(":checked")
    var bankcode = document.getElementById('BankCode').value;
    var indexcnt = document.getElementById('cnt').value;
    if (imagetype == "FTiff") {

        if (bankcode != "048") {
            var i = tt[indexcnt].FrontTiffImagePath;
            $.ajax({
                url: i,
                type: 'HEAD',
                error: function () {
                    //file not exists
                    alert('Image not loaded correctly!!!');
                    document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                },
                success: function () {
                    //file exists
                    //console.log("Front image url success " + frontGreyimgUrl);
                    $.ajax({
                        url: RootUrl + 'OWL2/getTiffImage',
                        dataType: 'html',
                        data: { httpwebimgpath: i },
                        success: function (Slipdata) {
                            //debugger;
                            $('#divtiff').html(Slipdata);
                            //document.getElementById('myimg').src = Slipdata;
                            document.getElementById('myimg').style.display = "none";
                            document.getElementById('divtiff').style.display = "block";

                        }
                    });
                }
            });

            
        }
        else {
            document.getElementById('myimg').src = tt[indexcnt].FrontTiffImagePath;
        }

        //document.getElementById('myimg').src = tt[indexcnt].FrontTiffImagePath;
    }
    else if (imagetype == "BTiff") {

        if (bankcode != "048") {
            var i = tt[indexcnt].BackTiffImagePath;
            $.ajax({
                url: i,
                type: 'HEAD',
                error: function () {
                    //file not exists
                    alert('Image not loaded correctly!!!');
                    document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                },
                success: function () {
                    //file exists
                    //console.log("Front image url success " + frontGreyimgUrl);
                    $.ajax({
                        url: RootUrl + 'OWL2/getTiffImage',
                        dataType: 'html',
                        data: { httpwebimgpath: i },
                        success: function (Slipdata) {
                            //debugger;
                            $('#divtiff').html(Slipdata);
                            //document.getElementById('myimg').src = Slipdata;
                            document.getElementById('myimg').style.display = "none";
                            document.getElementById('divtiff').style.display = "block";
                        }
                    });
                }
            });

            
        }
        else {
            document.getElementById('myimg').src = tt[indexcnt].BackTiffImagePath;
        }

        //document.getElementById('myimg').src = tt[indexcnt].BackTiffImagePath;
    }
    else if (imagetype == "FGray") {
        //debugger;

        var i = tt[indexcnt].FrontGreyImagePath;
        $.ajax({
            url: i,
            type: 'HEAD',
            error: function () {
                //file not exists
                alert('Image not loaded correctly!!!');
                document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
            },
            success: function () {
                //file exists
                //console.log("Front image url success " + frontGreyimgUrl);
                $.ajax({
                    url: RootUrl + 'OWL2/getTiffImage',
                    dataType: 'html',
                    data: { httpwebimgpath: i },
                    success: function (Slipdata) {
                        //debugger;
                        $('#divtiff').html(Slipdata);
                        //document.getElementById('myimg').src = Slipdata;
                        document.getElementById('myimg').style.display = "none";
                        document.getElementById('divtiff').style.display = "block";
                    }
                });
            }
        });

        
        //document.getElementById('divtiff').style.display = "none";
        //document.getElementById('myimg').style.display = "block";
        //document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath;
    }
    else if (imagetype == "FUV") {
        //debugger;
        if (tt[indexcnt].FrontUVImagePath != null) {
            var i = tt[indexcnt].FrontUVImagePath;
            $.ajax({
                url: i,
                type: 'HEAD',
                error: function () {
                    //file not exists
                    alert('Image not loaded correctly!!!');
                    document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                },
                success: function () {
                    //file exists
                    //console.log("Front image url success " + frontGreyimgUrl);
                    $.ajax({
                        url: RootUrl + 'OWL2/getTiffImage',
                        dataType: 'html',
                        data: { httpwebimgpath: i },
                        success: function (Slipdata) {
                            //debugger;
                            $('#divtiff').html(Slipdata);
                            //document.getElementById('myimg').src = Slipdata;
                            document.getElementById('myimg').style.display = "none";
                            document.getElementById('divtiff').style.display = "block";
                        }
                    });
                }
            });

            
        }
        //else
        //alert("UV image not available");
    }

}

function fnGetvalue(inpt) {
    var vlu = document.getElementById(inpt).value();
    alert(vlu);

}
function CalAmt() {
    //debugger;
    if ($("#Amt").val() != null || $("#Amt").val() != "") {
        ChequeAmountTotal = parseFloat(parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) - parseFloat(String(tempAmtValue).replace(/,/g, '')));
        ChequeAmountTotal = (parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) + parseFloat($("#Amt").val().replace(/,/g, '')));
        //ChequeAmountTotal.toFixed(1));
        document.getElementById('totamt').innerHTML = ChequeAmountTotal.toFixed(2);
        tempAmtValue = parseFloat($("#Amt").val().replace(/,/g, ''));
    }

}
function narrationAC(curntAc) {
    // debugger;
    narrationReqirdflg = false;
    var narsnac = document.getElementById('narration');
    var actualAC = curntAc.slice(-10)

    for (var i = 0; i < narsnac.length; i++) {
        //alert(rtncode);
        if (actualAC != narsnac[i].value && $("#ofc").val() == "1") {
            //  alert(rtnlistDescrp[i].value);
            document.getElementById('narsndiv').style.display = "";
            // document.getElementById('ofc').value = "1";
            narrationReqirdflg = true;
            break;
        }
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
        url: RootUrl + 'OWL2/GetBankName',
        dataType: 'html',
        data: { bankcode: bankcode },
        success: function (databank) {
            //alert(data);
            $('#bankname').html(databank);
            //$('#dialogEditUser').dialog('open');
        }
    });
}
function bankNameONFocus() {
    //debugger;
    var valcntt = document.getElementById('cnt').value;
    if ($("#SortQC").val() != "" && $("#SortQC").val().length == 9 && $("#SortQC").val() != tt[valcntt].SortCodeFinal) {

        $.ajax({
            url: RootUrl + 'OWL2/GetBankName',
            dataType: 'html',
            data: { bankcode: $("#SortQC").val() },
            success: function (databank) {
                //alert(data);
                $('#bankname').html(databank);
                //$('#dialogEditUser').dialog('open');
            }
        });
    }

}

function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substr(0, index) + chr + str.substr(index + 1);
}

function sortCodeValidation() {
    debugger;
    var sortcode = document.getElementById('SortQC').value;
    var flg;
    $.ajax({

        url: RootUrl + "OWL2/IsSortCodeExistInBankBranches",
        data: { sortcode: sortcode },
        dataType: "html",
        type: 'POST',
        async: false,
        success: function (data) {
            if (data == "false" || data == false) {
                //alert('Sort code not valid!!');

                flg = false;
            }
            else {
                flg = true;
            }
        }

    });
    return flg;
}





