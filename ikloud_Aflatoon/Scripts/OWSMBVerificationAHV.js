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
var SlipUserNarration;
var Slipdecision;
var SlipID;
var SlipRawaDataID;
var strModified = "00000000";
var realmodified = false;
//var scondbck = false;
var finalAccount = "";
var acc1 = "";
var acc2 = "";
var acc3 = "";
//var acmaxlength = 18;
var accVal;
//var cnt = 0;

function getPayee() {
    //--------------------Payee Name-----------------------------------//
    debugger;
    if ($("#ChqAcno").val() != "") {
        $.ajax({
            url: RootUrl + 'OWSmbVerificationAHV/GetSMBCBSDetails',
            dataType: 'html',
            data: { ac: $("#ChqAcno").val() },
            success: function (data) {
                if (data != null || data != "") {
                    var dataresult = JSON.parse(data);
                    // alert(dataresult);
                    //data = data.replace(/{/g, "").replace(/}/g, "")
                    //alert(data[0].payeenameselected);
                    //var dataarray = [];
                    //dataarray = data.split(',');        
                    alert(dataresult.AccountStatus + ' And ' + dataresult.MOP);
                    // alert();

                    document.getElementById('ChqPayeeName').value = dataresult.payeenameselected;
                }
                else {
                    alert('Payee Name was not found..!!!');
                    document.getElementById('ChqPayeeName').focus();
                    return false
                }

            }
        });
    }
    else {
        alert('Please enter the Account numer first..!!');
        document.getElementById('ChqAcno').focus();
        return false;
    }

    //----------------------------------------------------------------//
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
}
function IWVef() {
    //rtncd
    document.getElementById('rtncd').style.display = "none";

    var bankCode = document.getElementById('BankCode').value;
    chr = document.getElementById('Decision').value.toUpperCase();
    var chr = document.getElementById('Decision').value.toUpperCase();
    document.getElementById('Decision').value = chr;
    var iwrk = document.getElementById('IWRemark').value;
    if (chr == "R") {
        if (iwrk == "") {
            if (bankCode === "641") {
                //document.getElementsByClassName("test").style.display = "";
                document.getElementById('rtncd').style.display = "";
                var module = $('.test');
                $('.test').css("display", "none");
            }
            else {
                document.getElementById('rtncd').style.display = "";
                document.getElementById('IWRemark').style.width = "10%";
                document.getElementById('IWRemark').focus();
            }

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
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function IWLICQC() {
    // alert('abid'); 
    //debugger;
    var bankCode = document.getElementById('BankCode').value;

    var IWdecn = document.getElementById('Decision').value.toUpperCase();

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



    if (document.getElementById('OWIsDataEntryAllowedForAccountNo').value === 'Y' &&
        document.getElementById('OWIsDataEntryAllowedForPayeeName').value === 'Y' &&
        document.getElementById('OWIsDataEntryAllowedForDate').value === 'Y' &&
        document.getElementById('OWIsDataEntryAllowedForAmount').value === 'Y') {
        //----------------------accountno-----------------------------------------
        acntno = document.getElementById('ChqAcno').value;
        acntlength = acntno.length;
        console.log(acntlength);
        console.log(acmaxlength);


        if (acntlength < 1) {
            alert('Account No field should not be empty !');
            document.getElementById('ChqAcno').focus();
            return false;
        }
        else if (acntlength < acminlength) {
            alert('Account No is not valid ! Minimum Account no length should be ' + (acminlength + 1));
            document.getElementById('ChqAcno').value = '';
            document.getElementById('ChqAcno').focus();
            return false;
        }
        else if (acntlength > (acmaxlength)) {
            alert('Account No is not valid ! Maximum Account no length should be ' + (acmaxlength));
            document.getElementById('ChqAcno').value = '';
            document.getElementById('ChqAcno').focus();
            return false;
        }

        if (!isNumber(acntno)) {
            alert('Account No is not valid !');
            document.getElementById('ChqAcno').value = "";
            document.getElementById('ChqAcno').focus();
            return false;
        }

        //----------------------accountno-----------------------------------------

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
        else if (!isNumber(dat)) {
            alert('Date is not valid !');
            document.getElementById('ChqDate').focus();
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
    }

    //----------------------payeename---------------------------------------
    payename = document.getElementById('ChqPayeeName').value;
    payenamelen = payename.length;

    if (payenamelen < 1) {
        alert('Payee Name field should not be empty !');
        document.getElementById('ChqPayeeName').focus();
        return false;
    }
    else if (payenamelen > payeemaxlength) {
        alert('Payee Name not valid !');
        document.getElementById('ChqPayeeName').focus();
        return false;
    }
    //----------------------payeename-----------------------------------------

    //----------------------------Amount---------------------//
    amt = document.getElementById('ChqAmt').value;
    // alert(amt);
    var intcont = 0;
    //for (var i = 0; i < amt.length; i++) {

    //    if (amt.charAt(i) == ".") {
    //        intcont++;
    //    }
    //    if (intcont > 1) {
    //        alert('Enter valid amount!');
    //        document.getElementById('Amt').focus();

    //        return false;
    //    }
    //}

    if (amt == "NaN") {
        alert('Enter valid amount!');
        document.getElementById('Amt').focus();

        return false;
    }

    amt1 = amt;
    amt = amt.replace(/^0+/, '')
    amt = amt.length;
    //if (amt1 == ".") {
    //    alert('Amount field should not be dot(.) !');
    //    document.getElementById('Amt').focus();
    //    return false;
    //}
    //else if (amt1 == ".0") {
    //    alert('Amount not valid !');

    //    return false;
    //}
    //else if (amt1 == ".00") {
    //    alert('Amount not valid !');

    //    return false;
    //}
    //else if (amt1 == "0.00") {
    //    alert('Amount field should not be zero(0) !');

    //    return false;
    //}
    //else if (amt1 == "0.10") {
    //    alert('You can not accept this amount checque) ! ' + amt1);

    //    return false;
    //}
    //else if (amt1 == "0.01") {
    //    alert('Amount field should not be zero(0) ! ' + amt1);

    //    return false;
    //}
    //else if (amt < 1) {
    //    alert('Amount field should not be empty !');
    //    document.getElementById('Amt').focus();
    //    return false;
    //}
    //else if (amt > 15) {
    //    alert('Amount not valid !');
    //    document.getElementById('Amt').focus();
    //    return false;
    //}


    ///------------------------------------Post Date and Stale Cheques ----///


    //var postdate = document.getElementById('postdt').value;
    //var staledate = document.getElementById('staledt').value;

    //var fnldate;
    //if (finldat.length < 10) {
    //    fnldate = '20' + yy + '/' + mm + '/' + dd;
    //}
    //else {
    //    fnldate = finldat;
    //}

    //var staldat = new Date(staledate);
    //var postdat = new Date(postdate);
    //var d3 = new Date(fnldate);

    //var timeDiff = Math.abs(staldat.getTime() - d3.getTime());
    //var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

    //if (IWdecn == "A") {
    //    if (postdat <= d3) {
    //        alert('Post date!!');
    //        document.getElementById('ChqDate').focus();
    //        document.getElementById('ChqDate').select();
    //        return false;
    //    }
    //    if (staldat >= d3) {
    //        alert('Stale Cheque!!');
    //        document.getElementById('ChqDate').focus();
    //        document.getElementById('ChqDate').select();
    //        return false;
    //    }
    //}
    ////------------------------Sort code---------------

    var ChqnoQC = document.getElementById('FinalChqNo').value;
    var SortQC = document.getElementById('FinalSortcode').value;
    var SANQC = document.getElementById('FinalSAN').value;
    var TransQC = document.getElementById('FinalTransCode').value;

    if (ChqnoQC.length <= 0 || ChqnoQC.length < 6) {
        alert('Please enter cheque No!');
        document.getElementById('FinalChqNo').focus();
        return false;
    }
    else if (ChqnoQC == "") {
        //alert('aila');
        alert("Cheque no should not be empty !");
        document.getElementById('FinalChqNo').focus();
        document.getElementById('FinalChqNo').select();
        return false;
    } else if (!isNumber(ChqnoQC)) {
        alert('Cheque no is not valid !');
        document.getElementById('FinalChqNo').focus();
        return false;
    }


    else if (SortQC.length <= 0 || SortQC.length < 9) {
        alert('Please enter sort code!');
        document.getElementById('FinalSortcode').focus();
        return false;
    }
    else if (SortQC == "") {
        //alert('aila');
        alert("Sort code should not be empty !");
        document.getElementById('FinalSortcode').focus();
        document.getElementById('FinalSortcode').select();
        return false;
    } else if (!isNumber(SortQC)) {
        alert('Sort code no is not valid !');
        document.getElementById('FinalSortcode').focus();
        return false;
    }


    else if (SANQC.length <= 0 || SANQC.length < 6) {
        alert('Please enter san No!');
        document.getElementById('FinalSAN').focus();
        return false;
    }
    else if (SANQC == "") {
        alert("SAN code should not be empty !");
        document.getElementById('FinalSAN').focus();
        document.getElementById('FinalSAN').select();
        return false;
    }
    else if (!isNumber(SANQC)) {
        alert('SAN code is not valid !');
        document.getElementById('FinalSAN').focus();
        return false;
    }
    else if (TransQC.length <= 0 || TransQC.length < 2) {
        alert('Please enter trans code!');
        document.getElementById('FinalTransCode').focus();
        return false;
    }
    else if (TransQC == "") {
        alert("Trans code should not be empty !");
        document.getElementById('FinalTransCode').focus();
        document.getElementById('FinalTransCode').select();
        return false;
    }
    else if (!isNumber(TransQC)) {
        alert('Trans code is not valid !');
        document.getElementById('FinalTransCode').focus();
        return false;
    }
    else if (ChqnoQC.length < 6 || ChqnoQC == "000000") {
        alert("Cheque no is not valid !");
        document.getElementById('FinalChqNo').focus();
        document.getElementById('FinalChqNo').select();
        return false;
    }
    else if (SortQC.length < 9 || SortQC == "000000000") {
        alert("Sort code no is not valid !");
        document.getElementById('FinalSortcode').focus();
        document.getElementById('FinalSortcode').select();
        return false;
    }
    else if (SANQC.length < 6) {
        alert("SAN code no is not valid !");
        document.getElementById('FinalSAN').focus();
        document.getElementById('FinalSAN').select();
        return false;
    }
    else if (ChqnoQC.length < 6 || ChqnoQC == "000000" || isNaN(ChqnoQC)) {
        alert("Cheque no is not valid !");
        document.getElementById('FinalChqNo').focus();
        document.getElementById('FinalChqNo').select();
        return false;
    }
    else if (TransQC.length < 2 || TransQC == "00" || TransQC.substring(0, 1) == "0") {
        alert("Trans code is not valid !");
        document.getElementById('FinalTransCode').focus();
        document.getElementById('FinalTransCode').select();
        return false;
    }
    //var rtnflg = validYrnscodes();
    //if (rtnflg == false) {
    //    alert("Trans code is not valid !");
    //    document.getElementById('FinalTransCode').focus();
    //    document.getElementById('FinalTransCode').select();
    //    return false;
    //}


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
        if (bankCode === "641") {
            if (document.getElementById('rejectreasondescrpsn').value == "") {
                alert('Please enter reject description!');
                document.getElementById('rejectreasondescrpsn').focus();
                return false;
            }
        }
        else {
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
}
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
        console.log($("#captureRawId").val());
        $.ajax({
            url: RootUrl + 'OWSmbVerificationAHV/getOWlogs',
            dataType: 'html',
            data: { id: $("#captureRawId").val() },
            success: function (data) {
                //alert(data);
                $('#activitylogs').html(data);
                $('#activitylogs').dialog('open');
            }
        });
    });
}
function passval(array) {

    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}


var value = 0;
callrotate = function () {
    value += 180;
    $("#myimg,#ftiffimg").rotate({ animateTo: value })
}


function ChangeImage(ImagePath) {
    //alert(ImagePath);
    //console.log(ImagePath);
    //debugger;
    if (ImagePath == "" || ImagePath == null) {
        alert("Image Not Available!!!");
        document.getElementById('divtiff').style.display = "none";
        document.getElementById('myimg').style.display = "";
        document.getElementById('myimg').src = document.getElementById('BackGrey').value;
    }
    else {
        $.ajax({
            url: RootUrl + 'OWSmbVerificationAHV/getTiffImage',
            dataType: 'html',
            data: { httpwebimgpath: ImagePath },
            success: function (Slipdata) {
                //debugger;
                $('#divtiff').html(Slipdata);
                document.getElementById('myimg').src = Slipdata;
                document.getElementById('myimg').style.display = "none";
                document.getElementById('divtiff').style.display = "block";

            }
        });
    }

}
function ChangeImageNew(ImagePath) {
    //alert(ImagePath);
    console.log(ImagePath);
    $.ajax({
        url: RootUrl + 'OWSmbVerificationAHV/getTiffImageNew',
        dataType: 'html',
        data: { httpwebimgpath: ImagePath },
        success: function (Slipdata) {
            //debugger;
            $('#divtiff').html(Slipdata);
            //document.getElementById('myimg').src = Slipdata;
            document.getElementById('myimg').style.display = "none";
            document.getElementById('divtiff').style.display = "block";

        }
    });
}
function fullImage() {
    //debugger;
    //alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}
$(document).ready(function () {

    var bankCode = document.getElementById('BankCode').value;
    accVal = $("#accValidation").val();
    console.log('Account Validation ' + accVal);
    var vfType = document.getElementById('hVfType').value;
    if (vfType === "RNormalL1") {
        $("#ChqDate").focus();
        document.getElementById('Decision').value = "A";
        document.getElementById('divDecision').style.display = 'none';
    }
    else {
        $("#Decision").focus();
        document.getElementById('divDecision').style.display = "";
    }

    if (bankCode === "641") {
        var chequeAmount = document.getElementById('ChequeAmount').value;
        if (vfType === "RNormal") {
            if (Number(chequeAmount) >= 200000) {

                var imgUrl = document.getElementById('FrontUV').value;
                $.ajax({
                    url: imgUrl,
                    type: 'HEAD',
                    error: function () {
                        //file not exists
                        alert('Image not loaded correctly!!!');
                        //document.getElementById('myimg').src = setArray[StartCount].BackGreyImage;
                    },
                    success: function () {
                        //file exists
                        console.log("UV image url success " + imgUrl);
                        document.getElementById('myimg').src = imgUrl;
                    }
                });
            }
        }

    }

    //=====================================================================================================
    if (document.getElementById('OWIsDataEntryAllowedForAccountNo').value === 'Y') {
        document.getElementById('divAccount').style.display = "";
    }
    else {
        document.getElementById('divAccount').style.display = 'none';
    }

    if (document.getElementById('OWIsDataEntryAllowedForPayeeName').value === 'Y') {
        document.getElementById('divPayeeName').style.display = "";
    }
    else {
        document.getElementById('divPayeeName').style.display = 'none';
    }

    if (document.getElementById('OWIsDataEntryAllowedForDate').value === 'Y') {
        document.getElementById('divDate').style.display = "";
    }
    else {
        document.getElementById('divDate').style.display = 'none';
    }

    if (document.getElementById('OWIsDataEntryAllowedForAmount').value === 'Y') {
        document.getElementById('divAmount').style.display = "";
    }
    else {
        document.getElementById('divAmount').style.display = 'none';
    }
    console.log(document.getElementById('OWIsDataEntryAllowedForAmount').value);
    console.log(document.getElementById('OWIsDataEntryAllowedForDate').value);
    console.log(document.getElementById('OWIsDataEntryAllowedForAccountNo').value);
    console.log("DisableAccNo");
    console.log(document.getElementById('disableVerAccNo').value);
    //==============================================================================================
    if (document.getElementById('disableVerAccNo').value === 'Y') {
        document.getElementById('ChqAcno').readOnly = true;
    }
    else {
        console.log("In else N");
        //document.getElementById('ChqAcno').readOnly = false;
        $("#ChqAcno").attr("readonly", false);
        $("#ChqAcno").prop("readonly", false);
        document.getElementById('ChqAcno').removeAttribute('readonly');
    }
    if (document.getElementById('disableCdkAccNo').value === 'Y') {
        document.getElementById('ChqAcno').readOnly = true;
    }
    else {
        document.getElementById('ChqAcno').readOnly = false;
    }
    if (document.getElementById('disableCdkPayeeName').value === 'Y') {
        document.getElementById('ChqPayeeName').readOnly = true;
    }

    var ScanningTypeId = document.getElementById('ScanningTypeId').value;



    //document.getElementById('btnSubmitCDK').style.display = 'none';
    //if (document.getElementById('scanningType').value === '11') {
    //    document.getElementById('btnSubmitCDK').style.display = 'block';
    //    document.getElementById('btnSubmit').style.display = 'none';
    //}


    var L1Status = document.getElementById('L1Status').value;
    var L2Status = document.getElementById('L2Status').value;
    var L1StatusNumber = Number(L1Status);
    var L2StatusNumber = Number(L2Status);
    console.log('L1Status = ' + L1Status);
    console.log('L2Status = ' + L2Status);
    var L1RejectDesc = document.getElementById('L1RejectDesc').value;
    var L2RejectDesc = document.getElementById('L2RejectDesc').value;
    console.log('L1RejectDesc = ' + L1RejectDesc);
    console.log('L2RejectDesc = ' + L2RejectDesc);
    document.getElementById('L1rejectDecrp').innerHTML = L1RejectDesc;
    document.getElementById('L2rejectDecrp').innerHTML = L2RejectDesc;

    var L1Modified = document.getElementById('L1Modified').value;
    console.log('L1Modified = ' + L1Modified);
    var L2Modified = document.getElementById('L2Modified').value;
    console.log('L2Modified = ' + L2Modified);
    var L2ModifiedNumber = Number(L2Modified);

    if (L1StatusNumber > 0) {
        document.getElementById('l1dec').style.display = "";
        if (L1StatusNumber === 2) {
            document.getElementById("l1decision").innerHTML = "A";
        }
        else if (L1StatusNumber === 3) {
            document.getElementById("l1decision").innerHTML = "R";
        }
        else {
            document.getElementById("l1decision").innerHTML = "M";
        }
    }
    else {
        document.getElementById('l1dec').style.display = "none";
        console.log('L1Status = none ' + L1Status);
    }

    if (L2StatusNumber > 0) {
        console.log('L2Status = block ' + L2Status);
        if (L2StatusNumber === 2) {
            document.getElementById("l2decision").innerHTML = "A";
        }
        else if (L2StatusNumber === 3) {
            document.getElementById("l2decision").innerHTML = "R";
        }
        else {
            if (L2StatusNumber === 8) {
                if (L2ModifiedNumber > 0) {
                    document.getElementById("l2decision").innerHTML = "M";
                }
                else {
                    document.getElementById("l2decision").innerHTML = "A";
                }
            }
            else if (L2StatusNumber === 9) {
                if (L2ModifiedNumber > 0) {
                    document.getElementById("l2decision").innerHTML = "M";
                }
                else {
                    document.getElementById("l2decision").innerHTML = "R";
                }
            }
            else {
                document.getElementById("l2decision").innerHTML = "M";
            }

        }
    }
    else {
        document.getElementById('l2dec').style.display = "none";
        console.log('L2Status = none ' + L2Status);
    }



    if (L2ModifiedNumber > 0) {
        //-------------Account---------------
        if (L2Modified.charAt(0) == "1") {
            document.getElementById("ChqAcno").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqAcno").style.backgroundColor = "white";
        }
        //-------------Amount---------------
        if (L2Modified.charAt(1) == "1") {
            document.getElementById("ChqAmt").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqAmt").style.backgroundColor = "white";
        }
        //-------------PayeeName---------------
        if (L2Modified.charAt(2) == "1") {
            document.getElementById("ChqPayeeName").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqPayeeName").style.backgroundColor = "white";
        }
        //-------------Date---------------
        if (L2Modified.charAt(3) == "1") {
            document.getElementById("ChqDate").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqDate").style.backgroundColor = "white";
        }
        //-------------FinalChqNo---------------
        if (L2Modified.charAt(4) == "1") {
            document.getElementById("FinalChqNo").style.backgroundColor = "red";
        }
        else {
            document.getElementById("FinalChqNo").style.backgroundColor = "white";
        }
        //-------------FinalSortcode---------------
        if (L2Modified.charAt(5) == "1") {
            document.getElementById("FinalSortcode").style.backgroundColor = "red";
        }
        else {
            document.getElementById("FinalSortcode").style.backgroundColor = "white";
        }
        //-------------FinalSAN---------------
        if (L2Modified.charAt(6) == "1") {
            document.getElementById("FinalSAN").style.backgroundColor = "red";
        }
        else {
            document.getElementById("FinalSAN").style.backgroundColor = "white";
        }
        //-------------FinalTransCode---------------
        if (L2Modified.charAt(7) == "1") {
            document.getElementById("FinalTransCode").style.backgroundColor = "red";
        }
        else {
            document.getElementById("FinalTransCode").style.backgroundColor = "white";
        }
    }
    else {
        //-------------Account---------------
        if (L1Modified.charAt(0) == "1") {
            document.getElementById("ChqAcno").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqAcno").style.backgroundColor = "white";
        }
        //-------------Amount---------------
        if (L1Modified.charAt(1) == "1") {
            document.getElementById("ChqAmt").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqAmt").style.backgroundColor = "white";
        }
        //-------------PayeeName---------------
        if (L1Modified.charAt(2) == "1") {
            document.getElementById("ChqPayeeName").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqPayeeName").style.backgroundColor = "white";
        }
        //-------------Date---------------
        if (L1Modified.charAt(3) == "1") {
            document.getElementById("ChqDate").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqDate").style.backgroundColor = "white";
        }
        //-------------FinalChqNo---------------
        if (L1Modified.charAt(4) == "1") {
            document.getElementById("FinalChqNo").style.backgroundColor = "red";
        }
        else {
            document.getElementById("FinalChqNo").style.backgroundColor = "white";
        }
        //-------------FinalSortcode---------------
        if (L1Modified.charAt(5) == "1") {
            document.getElementById("FinalSortcode").style.backgroundColor = "red";
        }
        else {
            document.getElementById("FinalSortcode").style.backgroundColor = "white";
        }
        //-------------FinalSAN---------------
        if (L1Modified.charAt(6) == "1") {
            document.getElementById("FinalSAN").style.backgroundColor = "red";
        }
        else {
            document.getElementById("FinalSAN").style.backgroundColor = "white";
        }
        //-------------FinalTransCode---------------
        if (L1Modified.charAt(7) == "1") {
            document.getElementById("FinalTransCode").style.backgroundColor = "red";
        }
        else {
            document.getElementById("FinalTransCode").style.backgroundColor = "white";
        }
    }

    if (vfType === "RNormal" && ScanningTypeId === "15") {
        console.log("In scanningType 15");
        //document.getElementById('ChqDate').style.borderColor = "blue";
        document.getElementById('ChqAmt').style.backgroundColor = "yellow";
        document.getElementById('ChqAcno').style.backgroundColor = "yellow";
        document.getElementById('ChqAmt').style.borderColor = "blue";
        document.getElementById('ChqAcno').style.borderColor = "blue";
        //document.getElementById('ChqPayeeName').style.borderColor = 'blue';

        document.getElementById('ChqAmt').readOnly = true;
        document.getElementById('ChqAcno').readOnly = true;
        //document.getElementById('ChqPayeeName').readOnly = true;
    }
    else {
        //$("#Decision").focus();
        //document.getElementById('divDecision').style.display = "";
    }


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

    //$('#ChqPayeeName').validate({
    //    rules: {
    //        field: {
    //            alphanumeric: true
    //        }
    //    }
    //});

    $('#FinalChqNo,#FinalSortcode,#FinalSAN,#FinalTransCode,#ChqDate,#ChqAmt,#ChqAcno,#ChqPayeeName').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    $('#FinalChqNo,#FinalSortcode,#FinalSAN,#FinalTransCode,#ChqDate,#ChqAmt').keypress(function (event) {
        return isNumber(event, this)
    });

    function isNumber(evt, element) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (
            (charCode != 45 || $(element).val().indexOf('-') != -1) && // “-” CHECK MINUS, AND ONLY ONE.  
            (charCode != 46 || $(element).val().indexOf('.') != -1) && // “.” CHECK DOT, AND ONLY ONE.  
            (charCode < 48 || charCode > 57))
            return false;
        return true;
    }




    $("#ChqDate,#FinalChqNo").on('keypress', function () {
        if ($(this).val().length > 5) {
            alert("Length cannot be more than 6 digits.");
            return false;
        }

        var charCode = (event.which) ? event.which : event.keyCode;

        if (String.fromCharCode(charCode).match(/[^0-9]/g))

            return false;
    });

    $("#ChqAcno").on('keypress', function () {

        //debugger;
        console.log("MaxAcLength " + acmaxlength);
        console.log($(this).val().length + " Ac Length");
        if ($(this).val().length > (acmaxlength - 1)) {
            alert("Length cannot be more than " + (acmaxlength) + " digits.");
            return false;
        }

        //var charCode = (event.which) ? event.which : event.keyCode;

        //if (String.fromCharCode(charCode).match(/[^0-9.]/g))

        //    return false;
    });

    $("#ChqAmt").on('keypress', function () {
        if ($(this).val().length > 12) {
            alert("Length cannot be more than 13 digits.");
            return false;
        }

        var charCode = (event.which) ? event.which : event.keyCode;

        if (String.fromCharCode(charCode).match(/[^0-9.]/g))

            return false;

        var amtval = $(this).val();

        //if (val.indexOf('.') !== -1) {
        //    console.log(val);
        //    var number = val.value.split('.');
        //    console.log(number);
        //    console.log(number[1]);
        //    //get the carat position
        //    var caratPos = getSelectionStart(el);
        //    var dotPos = el.value.indexOf(".");
        //    if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
        //        return false;
        //    }
        //}

        if (amtval.length > 0) {
            var intcont;
            for (var i = 0; i < amtval.length; i++) {

                if (amtval.charAt(i) == ".") {
                    intcont++;
                }
                if (intcont > 1) {
                    event.preventDefault();
                }
            }
            var splitstr = amtval.split('.');
            // debugger;
            if (splitstr[1] != null) {
                var strlength = splitstr[1].length;
                if (strlength > 1) {
                    // alert('Enter only 2 digit after decimal!');
                    event.preventDefault();
                }

            }
        }

    });

    //function getSelectionStart(o) {
    //    if (o.createTextRange) {
    //        var r = document.selection.createRange().duplicate()
    //        r.moveEnd('character', o.value.length)
    //        if (r.text == '') return o.value.length
    //        return o.value.lastIndexOf(r.text)
    //    } else return o.selectionStart
    //}

    $("#FinalSortcode").on('keypress', function () {
        if ($(this).val().length > 8) {
            alert("Length cannot be more than 9 digits.");
            return false;
        }
    });

    $("#FinalSAN").on('keypress', function () {
        if ($(this).val().length > 5) {
            alert("Length cannot be more than 6 digits.");
            return false;
        }
    });
    $("#FinalTransCode").on('keypress', function () {
        if ($(this).val().length > 1) {
            alert("Length cannot be more than 2 digits.");
            return false;
        }

    });
    //$("#Decision").on('keypress', function () {
    //    if ($(this).val().length > 1) {
    //        alert("Only 'A' and 'R' allowed.");
    //        return false;
    //    }

    //$('#Decision').keydown(function (e) {
    //    if (e.shiftKey || e.ctrlKey || e.altKey) {
    //        e.preventDefault();
    //    } else {
    //        var key = e.keyCode;
    //        alert(key);
    //        if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {
    //            e.preventDefault();
    //        }
    //    }
    //});

    $("#ChqPayeeName").on('keypress', function () {
        if ($(this).val().length > (payeemaxlength)) {
            alert("Length cannot be more than " + (payeemaxlength + 1) + " characters.");
            return false;
        }
    });
    $('#ChqPayeeName').keyup(function () {
        var $th = $(this);
        $th.val($th.val().replace(/[^a-zA-Z0-9\s]/g, function (str) { alert('You typed " ' + str + ' ".\n\nPlease use only letters and numbers.'); return ''; }));
    });




    $("#btnSubmit").click(function () {

        if (document.getElementById("btnSubmit").value == "Ok") {

            var IWdecn = document.getElementById('Decision').value.toUpperCase();

            //alert('IWdecn ' + IWdecn);

            //if ((IWdecn == "") || (IWdecn != 'A') || (IWdecn != 'R'))
            //{
            //    //alert(document.getElementById("btnSubmit").value);
            //    alert("Invalid Decision Character only 'A'(Accept) and 'R'(Reject) can be accepted.");
            //    document.getElementById('Decision').focus();
            //    return false;
            //}

            var result = IWLICQC();

            if (result == false) {

                return false;
            }

            var result1 = AllValidations();
            if (result1 == false) {

                return false;
            }
            //else {

            //    if (btnval == "R") {
            //        cnrslt = false;
            //        //-----valid Function for validation---------------
            //        var rslt = valid(document.getElementById("rejectreasondescrpsn").value, document.getElementById('IWRemark').value);
            //        if (rslt == false) {
            //            //alert('Please select reject reason!!');
            //            document.getElementById('rejectreasondescrpsn').focus();
            //            nextcall = false;
            //            return false;
            //        }
            //        else {
            //            nextcall = true;
            //        }
            //    }
            //}
        }
        //else if (document.getElementById("rejectreasondescrpsn").value == "Close")
        //    {}



    });

    //$("#btnSubmitCDK").click(function () {
    //    console.log('In submit cdk button');
    //    if (document.getElementById("btnSubmitCDK").value == "CDK Ok") {

    //        var IWdecn = document.getElementById('Decision').value.toUpperCase();
    //        var result = IWLICQC();
    //        console.log(result);
    //        if (result == false) {
    //            console.log('In false');
    //            return false;
    //        }
    //    }

    //});

    //================ Retrieve All Text values ======================
    var ChqAcno = document.getElementById('ChqAcno').value;
    var ChqAmt = document.getElementById('ChqAmt').value;
    var ChqPayeeName = document.getElementById('ChqPayeeName').value;
    var ChqDate = document.getElementById('ChqDate').value;
    var FinalChqNo = document.getElementById('FinalChqNo').value;
    var FinalSortcode = document.getElementById('FinalSortcode').value;
    var FinalSAN = document.getElementById('FinalSAN').value;
    var FinalTransCode = document.getElementById('FinalTransCode').value;
    //================ End ===========================================

    $("#ChqAcno").focus(function () {
        //Foutcnt = document.getElementById('cnt').value;
        console.log($("#ChqAcno").val().length);
        if ($("#ChqAcno").val().length === 0) {
            var path = document.getElementById('backGreyImage').src;
            console.log(path);
            document.getElementById('myimg').src = path;
            //ChangeImageNew(path);
        }

    });
    $("#ChqAcno").focusout(function () {
        //Foutcnt = document.getElementById('cnt').value;
        if (ChqAcno !== $("#ChqAcno").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 0, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 0, '0');
        }
        document.getElementById('realModified').value = realmodified;
        document.getElementById('modified').value = strModified;
        document.getElementById('myimg').src = document.getElementById('frontGreyImage').src;
        console.log(document.getElementById('myimg').src);
    });
    $("#ChqAmt").focusout(function () {
        //  debugger;
        //Foutcnt = document.getElementById('cnt').value;
        if (parseFloat(ChqAmt.replace(/,/g, '')) !== parseFloat($("#ChqAmt").val().replace(/,/g, ''))) {
            realmodified = true;
            strModified = setCharAt(strModified, 1, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 1, '0');
        }
        console.log(strModified);
        console.log(realmodified);
        document.getElementById('realModified').value = realmodified;
        document.getElementById('modified').value = strModified;
        console.log(document.getElementById('modified').value);
        //console.log(document.getElementById('realmodified').value);
    });
    $("#ChqPayeeName").focusout(function () {
        //Foutcnt = document.getElementById('cnt').value;
        if (ChqPayeeName !== $("#ChqPayeeName").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 2, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 2, '0');
        }
        document.getElementById('realModified').value = realmodified;
        document.getElementById('modified').value = strModified;
    });
    $("#ChqDate").focusout(function () {
        // debugger;
        //Foutcnt = document.getElementById('cnt').value;
        if (ChqDate != $("#ChqDate").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 3, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 3, '0');
        }
        document.getElementById('realModified').value = realmodified;
        document.getElementById('modified').value = strModified;
    });
    $("#FinalChqNo").focusout(function () {
        //Foutcnt = document.getElementById('cnt').value;
        if (FinalChqNo != $("#FinalChqNo").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 4, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 4, '0');
        }
        document.getElementById('realModified').value = realmodified;
        document.getElementById('modified').value = strModified;
    });
    $("#FinalSortcode").focusout(function () {
        //Foutcnt = document.getElementById('cnt').value;
        if (FinalSortcode != $("#FinalSortcode").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 5, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 5, '0');
        }
        document.getElementById('realModified').value = realmodified;
        document.getElementById('modified').value = strModified;
    });
    $("#FinalSAN").focusout(function () {
        //Foutcnt = document.getElementById('cnt').value;
        if (FinalSAN != $("#FinalSAN").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 6, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 6, '0');
        }
        document.getElementById('realModified').value = realmodified;
        document.getElementById('modified').value = strModified;
    });
    $("#FinalTransCode").focusout(function () {
        //Foutcnt = document.getElementById('cnt').value;
        if (FinalTransCode != $("#FinalTransCode").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 7, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 7, '0');
        }
        document.getElementById('realModified').value = realmodified;
        document.getElementById('modified').value = strModified;
    });

    //$("#ctsnocts").focusout(function () {
    //    Foutcnt = document.getElementById('cnt').value;
    //    if (tt[Foutcnt].ClearingType != $("#ctsnocts").val()) {
    //        realmodified = true;
    //        strModified = setCharAt(strModified, 8, '1');
    //    }
    //    else {
    //        realmodified = false;
    //        strModified = setCharAt(strModified, 8, '0');
    //    }
    //});

    //===================== New Validation ====================

    $("#ChqAcno").keyup(function (event) {
        console.log('In');
        if (accVal === 'Y') {
            //return isNumberWithDot(event, this);
            var result = isNumberWithDot(event, this);

            if (result === false) {
                return false;
            }
            else {
                var acc = $("#ChqAcno").val();
                console.log(acc);

                if (acc.indexOf('.') !== -1) {
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
                                    console.log('accStringNew1 - ' + accStringNew1);
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
                                        document.getElementById("ChqAcno").focus();
                                        return false;
                                    }

                                }
                            }
                            else {
                                var str = $("#ChqAcno").val();
                                var strNew = str.replace(/\./g, "");
                                $("#ChqAcno").val(strNew);
                                alert('Please Enter digit');
                                return false;
                            }


                        }
                        if (num === 2) {
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
                                    document.getElementById("ChqAcno").focus();
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
                                }
                                else {
                                    alert('Please enter non zero value.');
                                    document.getElementById("ChqAcno").focus();
                                    return false;
                                }
                                //finalAccount = acc1 + acc2 + acc3;
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

    $("#ChqAcno").focusout(function (event) {
        console.log('In ac foucusout');
        if (accVal === 'Y') {
            var len = document.getElementById("ChqAcno").value.length;
            if (len !== acmaxlength) {
                console.log('In if ac foucusout');
                if (finalAccount.length === acmaxlength) {
                    document.getElementById("ChqAcno").value = finalAccount;
                }
                else {

                    document.getElementById("ChqAcno").focus();

                }
            }
        }

    });

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

    function isDateValid(dt) {
        //console.log(dt);

        //console.log(yy);
        //var dd = dt.substring(0, 2);
        //console.log(dd);
        //console.log(dt);
        //var mm = dt.substring(4, 2);
        //console.log(mm);
        //console.log(dt);
        //var yy = dt.substring(10, 2);
        var newNum = dt.toString().match(/.{1,2}/g);

        var numberDD = Number(newNum[0]);
        var numberMM = Number(newNum[1]);
        var numberYY = Number(newNum[2]);
        console.log(numberDD + ' ' + numberMM + ' ' + numberYY);
        if (numberDD > 0 && numberDD < 32 && numberMM > 0 && numberMM < 13 && numberYY > 20) {
            return true;
        }
        else {
            return false;
        }
    };



});

function AllValidations() {

    var bankCode = document.getElementById('BankCode').value;

    if (document.getElementById('OWIsDataEntryAllowedForAccountNo').value === 'Y' &&
        document.getElementById('OWIsDataEntryAllowedForPayeeName').value === 'Y' &&
        document.getElementById('OWIsDataEntryAllowedForDate').value === 'Y' &&
        document.getElementById('OWIsDataEntryAllowedForAmount').value === 'Y') {
        if (!$('#ChqDate').val()) {
            alert('Date cannot be blank.');
            $("#ChqDate").focus();
            return false;
        }

        else if (!$('#ChqAcno').val()) {
            alert('Account No cannot be blank.');
            $("#ChqAcno").focus();
            return false;
        }

        else if (!$('#ChqPayeeName').val()) {
            alert('Payee Name cannot be blank.');
            $("#ChqPayeeName").focus();
            return false;
        }

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
        //else if ((dat.toUpperCase().charAt(dat.length - 1) == "X") && (IWdecn.toUpperCase() == "A")) {
        //    alert("Date is not correct");
        //    document.getElementById('IWDecision').focus();
        //    return false;
        //}
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

            if ($("#ChqDate").val() != "000000") {
                var onlydate = dd + '/' + mm + '/' + '20' + yy;


                var rtn = validatedate(onlydate);
                if (rtn == false) {
                    document.getElementById('ChqDate').focus();
                    document.getElementById('ChqDate').select();
                    return false;
                }
            }

        }
    }
    else {
        if (!$('#ChqPayeeName').val()) {
            alert('Payee Name cannot be blank.');
            $("#ChqPayeeName").focus();
            return false;
        }
        else if (!$('#ChqAmt').val()) {
            alert('Amount cannot be blank.');
            $("#ChqAmt").focus();
            return false;
        }
    }

}

//function validatedate(inputText) {
//    // debugger;

//    var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
//    // Match the date format through regular expression  
//    if (inputText.match(dateformat)) {
//        //document.form1.text1.focus();
//        //Test which seperator is used '/' or '-'  
//        var opera1 = inputText.split('/');
//        var opera2 = inputText.split('-');
//        lopera1 = opera1.length;
//        lopera2 = opera2.length;
//        // Extract the string into month, date and year  
//        if (lopera1 > 1) {
//            var pdate = inputText.split('/');
//        }
//        else if (lopera2 > 1) {
//            var pdate = inputText.split('-');
//        }
//        var dd = parseInt(pdate[0]);
//        var mm = parseInt(pdate[1]);
//        var yy = parseInt(pdate[2]);
//        // Create list of days of a month [assume there is no leap year by default]  
//        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
//        if (mm == 1 || mm > 2) {
//            if (dd > ListofDays[mm - 1]) {
//                alert('Invalid date!');
//                return false;
//            }
//        }
//        if (mm == 2) {
//            var lyear = false;
//            if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
//                lyear = true;
//            }
//            if ((lyear == false) && (dd >= 29)) {
//                alert('Invalid date!');
//                return false;
//            }
//            if ((lyear == true) && (dd > 29)) {
//                alert('Invalid date!');
//                return false;
//            }
//        }
//    }
//    else {
//        alert("Invalid date !");
//        //  document.form1.text1.focus();
//        return false;
//    }
//    //  return true;
//}

function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substr(0, index) + chr + str.substr(index + 1);
}








