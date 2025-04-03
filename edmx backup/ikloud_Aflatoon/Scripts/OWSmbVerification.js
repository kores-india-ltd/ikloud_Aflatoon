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

function getPayee() {
    //--------------------Payee Name-----------------------------------//
    debugger;
    if ($("#ChqAcno").val() != "") {
        $.ajax({
            url: RootUrl + 'OWSmbVerification/GetSMBCBSDetails',
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


    chr = document.getElementById('Decision').value.toUpperCase();
    var chr = document.getElementById('Decision').value.toUpperCase();
    document.getElementById('Decision').value = chr;
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
    else if (!isNumber(SortQC)) {
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
            url: RootUrl + 'OWSmbVerification/getOWlogs',
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

    $.ajax({
        url: RootUrl + 'OWSmbVerification/getTiffImage',
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

    
    $("#Decision").focus();
    //document.getElementById('btnSubmitCDK').style.display = 'none';
    //if (document.getElementById('scanningType').value === '11') {
    //    document.getElementById('btnSubmitCDK').style.display = 'block';
    //    document.getElementById('btnSubmit').style.display = 'none';
    //}
    if (document.getElementById('disableVerAccNo').value === 'Y') {
        document.getElementById('ChqAcno').readOnly = true;
    }
    if (document.getElementById('disableCdkAccNo').value === 'Y') {
        document.getElementById('ChqAcno').readOnly = true;
    }
    if (document.getElementById('disableCdkPayeeName').value === 'Y') {
        document.getElementById('ChqPayeeName').readOnly = true;
    }

    var L1Modified = document.getElementById('L1Modified').value;
    var L2Modified = document.getElementById('L2Modified').value;
    var L2ModifiedNumber = Number(L2Modified);

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

    $('#FinalChqNo,#FinalSortcode,#FinalSAN,#FinalTransCode,#ChqDate,#ChqAmt,#ChqAcno').keypress(function (event) {
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
    });

    $("#ChqAcno").on('keypress', function () {

        //debugger;

        if ($(this).val().length > (acmaxlength)) {
            alert("Length cannot be more than " + (acmaxlength) + " digits.");
            return false;
        }
    });

    $("#ChqAmt").on('keypress', function () {
        if ($(this).val().length > 12) {
            alert("Length cannot be more than 13 digits.");
            return false;
        }
    });
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
        console.log(document.getElementById('realmodified').value);
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

    
});

function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substr(0, index) + chr + str.substr(index + 1);
}








