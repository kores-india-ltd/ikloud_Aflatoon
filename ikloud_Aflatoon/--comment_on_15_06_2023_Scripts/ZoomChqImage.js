
$(document).ready(function () {

    $('#P2FIndicator').disabled = true;
    //document.getElementById('ChqDate').value = "";
    $('#ChqDate').val('');
    $("#Decision").focus();
    var actn = document.getElementById('IWDecision').value.toUpperCase();
    if (actn == "R") {
        return true;
    }
    else {
        $("#IWDecision").focus();
        $("#PayeeName").focus();
        $("#PayeeName").select();

        $('#frmLIC input').keydown(function (e) {
           
            if (e.keyCode == 13) {
                var cntv = document.getElementById('cnt').value;
                
                if (cntv != '0111') {
                    document.getElementById('cnt').value = cntv + 1;
                    $(':input:eq(' + ($(':input').index(this) + 1) + ')').focus().select();
                    return false;
                }
                else {
                    return true;
                }
            }
        });
    }

});

//------------------------------
$("#sortlink").click(function () {
    var $lat = $("#ChqDate").val();
    // var $lon = $("#SearchLongitude").val();
    //alert($lat);
    $(this).attr("href", $(this).attr("href") + "?chqdt=" + $lat);
});
//------------------------------
$("input").attr('autocomplete', 'off');

//-------------------IWQueryModule--------------------------
$("#p2fa").keydown(function (event) {

    if (event.keyCode == 82 || event.keyCode == 78 || event.keyCode == 8) {
    }
    else {
        event.preventDefault();
    }
});
function p2f() {
    if (document.getElementById("p2fa").value == "") {
        alert("P2F Recived field is empty!");
        return false;
    }
}
// -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -- -

$("input[name=Rdo]").change(function () {
    //alert("caleed");
    if ($("#Radio1").attr("checked")) {
        //alert("caleed");
        $('#Chqimg')
                   .wrap('<span style="display:inline-block"></span>')
                   .css('display', 'block')
                   .parent()
                   .zoom({ url: document.getElementById("Chqimg").value });
    }
    else {
        //alert("caleed");
        $('#Chqimgbk')
                  .wrap('<span style="display:inline-block"></span>')
                  .css('display', 'block')
                  .parent()
                  .zoom({ url: document.getElementById("Chqimgbk").value });
    }

});

///------------- jquery------------
//------------ IW Verfication --------------

$("#PayeeName").keydown(function (event) {
    if (event.shiftKey) {
        if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
        }
    }
    var vrpay = document.getElementById("PayeeName").value;
    // alert(vrpay.length);
    if (vrpay.length == 0) {
        if (event.keyCode == 32) {
            alert('Blank space are not allowed!');
            event.preventDefault();
            return false;
        }
    }

    if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.keyCode > 64 && event.keyCode < 91) || (event.keyCode > 96 && event.keyCode < 123) || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
    }
    else {
        //if (event.keyCode < 95) {
        if ((event.keyCode > 31 && event.keyCode > 65) || (event.keyCode > 90 && event.keyCode > 97)) {
            event.preventDefault();
        }
    }
});

//-----Prasad Test -------------------
$("#P2FIndicator").keydown(function (event) {
    if (event.shiftKey) {
        if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
        }
    }
   
    var ptf = document.getElementById('P2FIndicator').value.toUpperCase();
  
    if (event.keyCode == 78 || event.keyCode == 110 || event.keyCode == 89 || event.keyCode == 121 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        if (event.keyCode == 8) {
            if (ptf != "N" || ptf != "Y") {
                document.getElementById('P2FIndicator').value = "";
            }
        }
    }
    else {
        event.preventDefault();
    }
});

$("#Decision").keydown(function (event) {
    if (event.shiftKey) {
        if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
        }
    }
    var ptf;
    var inttype = document.getElementById('InstrumentTyep').value;
    if (inttype == "C") {
        document.getElementById('P2FIndicator').disabled = true;
        ptf = document.getElementById('P2FIndicator').value.toUpperCase();
    }

    //    if (event.shiftKey) {
    //        event.preventDefault();
    //    }
    if (document.getElementById("blockkey").value == "1") {
        if (event.keyCode == 65 || event.keyCode == 97) {
            event.preventDefault();
            alert('You can not accept this check!');
            return false;
        } // || event.keyCode == 99
        if (event.keyCode == 67 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

        }
        else {
            event.preventDefault();
        }
    }
    else {//|| event.keyCode == 99 || event.keyCode == 97
        if (event.keyCode == 67 || event.keyCode == 82 || event.keyCode == 65 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
            if (inttype == "C") {
                if (event.keyCode == 8) {
                    if (ptf != "N" || ptf != "Y") {
                        document.getElementById('P2FIndicator').value = "";
                    }
                }
            }

        }
        else {
            event.preventDefault();
        }
        //------------------
        if (inttype == "C") {
            if (document.getElementById('P2FIndicator').value == "") {
                //alert('Aila');
                if (document.getElementById('IQAFlag').value == 1) {

                    document.getElementById('P2FIndicator').value = "Y";
                }
                else {
                    document.getElementById('P2FIndicator').value = "N";
                }
            }
        }
    }

});

$("#Remark").keydown(function (event) {
    if (event.shiftKey) {
        if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
        }
    }
    var chr = document.getElementById('Decision').value.toUpperCase();
    if (chr == "C" && document.getElementById('InstrumentTyep').value.toUpperCase() == "C") {
        if (event.shiftKey) {
            event.preventDefault(); //event.keyCode == 68 || event.keyCode == 100 
        }
        if (event.keyCode == 67 || event.keyCode == 99 || event.keyCode == 77 || event.keyCode == 109 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            event.preventDefault();
        }
    }
    else if (chr == "C" && document.getElementById('InstrumentTyep').value.toUpperCase() == "S") {
        if (event.shiftKey) {
            event.preventDefault(); //|| event.keyCode == 97 || event.keyCode == 99 
        }
        if (event.keyCode == 67 || event.keyCode == 65 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            event.preventDefault();
        }
    }
});

//--------------------------Normal IW L1Verification ---------------------------//

/*$('#frmNrml input').keydown(function (e) {
//alert('aila');
if (e.keyCode == 13) {

//        if ($(':input:eq(' + ($(':input').index(this) + 1) + ')').attr('type') == 'submit') {
//            alert('Aila');
//            return true;
//        }
var cntv = document.getElementById('cntt').value;
// alert(cntv);
if (cntv != '01') {
//alert('abid');
document.getElementById('cntt').value = cntv + 1;
$(':input:eq(' + ($(':input').index(this) + 1) + ')').focus().select();
return false;
}
else {
//alert('aila');
//IWLICQC();
return true;
}
}

});*/
//----------------------- IW ---------------------SignatureVerification ----------------------
$("#IWDecision").keydown(function (event) {

    //alert(event.keyCode);

    if (event.shiftKey) {
        if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
        }
    }
    if (document.getElementById("blockkey").value == "1") {
        if (event.keyCode == 65 || event.keyCode == 97) {
            event.preventDefault();
            alert('You can not accept this check!'); //|| event.keyCode == 99 || event.keyCode == 114
            return false;
        }
        if (event.keyCode == 67 || event.keyCode == 82 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

        }
        else {
            event.preventDefault();
        }
    } //|| event.keyCode == 114 || event.keyCode == 97 || event.keyCode == 99 
    else {
        if (event.keyCode == 113 || event.keyCode == 67 || event.keyCode == 82 || event.keyCode == 65 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            event.preventDefault();
        }
    }
    if (event.keyCode == 113) {
        windowcall(); //Sign
        return false;
    }

});

$("#IWRemark").keydown(function (event) {
    var chr = document.getElementById('IWDecision').value.toUpperCase();
    if (event.shiftKey) {
        event.preventDefault();
    }

    if (chr == "R") {
        if ((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode > 95 && event.keyCode < 106) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            event.preventDefault(); // || event.keyCode == 68 || event.keyCode == 100 || event.keyCode == 112 
        }
    }
    else {
        //document.getElementById('IWRemark').maxlength = '2';
        //alert(event.keyCode);
        if (event.keyCode == 65 || event.keyCode == 80 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            event.preventDefault();
        }
    }

});

$("#ChqDate").keydown(function (event) {
    if (event.shiftKey) {
        event.preventDefault();
    }

    if (event.keyCode == 110 || event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 88 || event.keyCode == 120 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.keyCode > 95 && event.keyCode < 106) || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        //alert('ok');
    }
    else {
        //if (event.keyCode < 95) {
        if (event.keyCode == 32 || event.keyCode < 48 || (event.keyCode > 57)) {
            event.preventDefault();
        }

    }
});
//--------------------------------------
function SlpVerification(type) {
    if (type == 'OW') {
        var chr = document.getElementById('Decision').value.toUpperCase();
        //    document.getElementById('lblP2FIndicator').style.display = "none";
        //    document.getElementById('P2FIndicator').style.display = "none";
        document.getElementById('divhint').style.display = "none";
        document.getElementById('CMDdivhint').style.display = "";
        //document.getElementById('selt').style.display = "none";
        if (document.getElementById('InstrumentTyep').value.toUpperCase() == "C") {
            //        document.getElementById('lblP2FIndicator').style.display = "";
            //        document.getElementById('P2FIndicator').style.display = "";
        }
        //    else {
        ////        document.getElementById('lblP2FIndicator').style.display = "none";
        ////        document.getElementById('P2FIndicator').style.display = "none";
        //    }

        if (chr == "C" || chr == "R") {

            document.getElementById('Remark').style.display = "";
            document.getElementById('lblRemark').style.display = "";
            document.getElementById('Remark').style.width = "10%";
            document.getElementById('Remark').focus();

            if (document.getElementById('InstrumentTyep').value.toUpperCase() == "C") {
                if (document.getElementById('P2FIndicator').value != "N" || document.getElementById('P2FIndicator').value != "Y") {
                    document.getElementById('P2FIndicator').value = "";
                }
            }
            //        document.getElementById('lblP2FIndicator').style.display = "none";
            //        document.getElementById('P2FIndicator').style.display = "none";
            if (chr == "C") {
                document.getElementById('Comment').style.display = "";
                document.getElementById('lblComment').style.display = "";
                document.getElementById('divhint').style.display = "";
                document.getElementById('CMDdivhint').style.display = "none";
            }
            else {

                document.getElementById('Remark').style.width = "40%";
                document.getElementById('Remark').focus();
            }

        }
        else {
            //
            // alert('Saruche!');
            document.getElementById('lblComment').style.display = "none";
            document.getElementById('Remark').style.display = "none";
            document.getElementById('lblRemark').style.display = "none";
            document.getElementById('Comment').style.display = 'none';

            if (document.getElementById('InstrumentTyep').value.toUpperCase() == "C") {
                if (chr == "A" && document.getElementById('P2FIndicator').value == "Y") {//Change By Abid chr == "Y" to chr == "A"
                    document.getElementById('P2FIndicator').disabled = true;
                }
                else if (chr == "A") {//Change By Abid chr == "Y" to chr == "A"
                    document.getElementById('P2FIndicator').disabled = false;
                }
            }
        }
    }
    else {//-------------------IW----------------------
        // alert('hmmm!');

        document.getElementById('Remark').style.display = "none";
        document.getElementById('selt').style.display = "none";

        chr = document.getElementById('IWDecision').value.toUpperCase();
        document.getElementById('divhint').style.display = "none";
        document.getElementById('CMDdivhint').style.display = "";

        //            document.getElementById('lblRemark').style.display = "none";
        document.getElementById('selt').style.display = "none";
        if (chr == "C" || chr == "R") {

            document.getElementById('IWRemark').style.display = "";
            document.getElementById('IWRemark').style.width = "10%";
            document.getElementById('IWRemark').focus();
            document.getElementById('selt').style.display = "none";

            if (chr == "C") {
                document.getElementById('lblRemark').style.display = "";
                document.getElementById('IWRemark').value = "";
                document.getElementById('Comment').style.display = "";
                document.getElementById('lblComment').style.display = "";
                document.getElementById('CMDdivhint').style.display = "none";
                document.getElementById('divhint').style.display = "";
                //alert('hmm');

            }
            else {
                document.getElementById('IWRemark').style.width = "10%";
                document.getElementById('IWRemark').focus();
                document.getElementById('selt').style.display = "";
                document.getElementById('Remark').style.display = "";
            }

        }
        else {
            //
            // alert('Saruche!');
            document.getElementById('lblComment').style.display = "none";
            document.getElementById('IWRemark').style.display = "none";
            document.getElementById('lblRemark').style.display = "none";
            document.getElementById('Comment').style.display = 'none';
            var tempPayeeName = document.getElementById('PayeeName').value;
            // alert(tempPayeeName.substring(0, 5));
            if ((tempPayeeName.toUpperCase().substring(0, 5) == "NNNNN" || (tempPayeeName.toUpperCase().substring(0, 5) == "00000"))) {
                alert('Please Check Payee Name!!!');
                return false;
            }
        }
    }
}

//------------------------
function chkpt2f(type) {

    //------------------
    if (type == "OW") {
        var re = /[^0-9]/g;
        var decn = document.getElementById('Decision').value.toUpperCase();
        // alert(decn);
        var inttype1 = document.getElementById('InstrumentTyep').value;
        if (inttype1 == "C") {
            if (document.getElementById('P2FIndicator').value == "") {

                if (document.getElementById('IQAFlag').value == 1) {

                    document.getElementById('P2FIndicator').value = "Y";
                }
                else {
                    document.getElementById('P2FIndicator').value = "N";
                }
            }
        }
        if (decn == "") {
            alert('Please enter decision!');
            document.getElementById('Decision').focus();
            return false;
        }
        if (decn != "A" && decn != "R" && decn != "C") {
            alert('Decision not correct!');
            document.getElementById('Decision').focus();
            return false;
        }
        if (decn == "R") {
            if (document.getElementById('Remark').value == "") {
                alert('Please enter reject reason!');
                document.getElementById('Remark').focus();
                return false;
            }
        }
        else if (decn == "C") {
            //alert(document.getElementById('Remark').value.length);
            var tempremrk = document.getElementById('Remark').value;
            tempremrk = tempremrk.replace(/^s+/g, '').replace(/s+$/g, '');
            if (tempremrk == "" || tempremrk.length > 1) {
                alert('Please enter remark!');
                document.getElementById('Remark').value = "";
                document.getElementById('Remark').focus();
                return false;
            }
        }
        if (decn == "A") {
            if (document.getElementById('InstrumentTyep').value == "C") {

                var ChqNoVal = document.getElementById('ChqNo').value;
                var SortCodeVal = document.getElementById('SortCode').value;
                var SAN = document.getElementById('SAN').value;
                var TrCodeVal = document.getElementById('TrCode').value;

                if (ChqNoVal.length < 6 || SortCodeVal.length < 9 || SAN.length < 6 || TrCodeVal.length < 2 || ChqNoVal == "000000" || SortCodeVal == "000000000" || TrCodeVal == "00" || TrCodeVal == "000") {
                    alert('MICR code is not valid!');
                    return false;
                }
                if (re.test(ChqNoVal) == true || re.test(SortCodeVal) == true || re.test(SAN) == true || re.test(TrCodeVal) == true) {
                    alert('MICR code is not valid!');
                    return false;
                }
            }
        }

    }
    else {

        var IWdecn = document.getElementById('IWDecision').value.toUpperCase();
        if (IWdecn == "") {
            alert('Please enter decision!');
            document.getElementById('IWDecision').focus();
            return false;
        }

        if (IWdecn == "A") {

            var PAyee = document.getElementById('PayeeName').value;
            if (PAyee == "") {

                alert("Payee field should not be empty !");
                document.getElementById('PayeeName').focus();
                document.getElementById('PayeeName').select();
                return false;
            }
            if (PAyee.length < 5 && PAyee != "") {
                //alert('aila');
                alert("Enter minimum 5 character for payee name !");
                document.getElementById('PayeeName').focus();
                document.getElementById('PayeeName').select();
                return false;
            }
            if ((PAyee.toUpperCase().substring(0, 5) == "NNNNN" || (PAyee.toUpperCase().substring(0, 5) == "00000"))) {
                alert('Please Check Payee Name!!!');
                document.getElementById('PayeeName').focus();
                document.getElementById('PayeeName').select();
                return false;
            }


        }
        if (IWdecn != "A" && IWdecn != "R" && IWdecn != "C") {
            alert('Decision not correct!');
            document.getElementById('IWDecision').focus();
            return false;
        }
        if (IWdecn == "R") {
            if (document.getElementById('IWRemark').value == "") {
                alert('Please enter reject reason!');
                document.getElementById('IWRemark').focus();
                return false;
            }
        }
        else if (IWdecn == "C") {
            if (document.getElementById('IWRemark').value == "") {
                alert('Please enter remark!');
                document.getElementById('IWRemark').focus();
                return false;
            }
        }
        //document.getElementById('Submit1').disabled = true;
    }
    return true;
}
//--------------IW----QCLICVerification----------------------------------------//
function IWLICQC() {


    //-------------------------ChqDate-------------------------------------------------//
    var IWdecn = document.getElementById('IWDecision').value.toUpperCase();
    dat = document.getElementById('ChqDate').value;
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
    else if (dat == "000000") {
        alert("Date not valid !");
        document.getElementById('ChqDate').focus();
        document.getElementById('ChqDate').select();
        return false;
    }
    // else if (dat.length < 2 || dat.length == 3 || dat.length == 5) {
    else if (dat.length < 6) {
        alert("Date not valid !");
        document.getElementById('ChqDate').focus();
        document.getElementById('ChqDate').select();
        return false;
    }
    else {
        var dd, mm, yy;
        finldat = new String(dat);
        // alert(finldat);
        if (dat.length = 2) {
            dd = finldat.substring(0, 2)
        }
        else if (dat.length = 4) {
            dd = finldat.substring(0, 2)
            mm = finldat.substring(2, 4)
        }
        else if (dat.length = 6) {

            dd = finldat.substring(0, 2)
            mm = finldat.substring(2, 4)
            yy = finldat.substring(4, 6)
        }
        //alert(dd+'-'+ mm +'-'+'20'+yy);
        var d = new Date();
        var n = d.getFullYear().toString().substring(2);
        if (yy > n || yy > n - 1) {
            alert('Please enter correct date!');
            document.getElementById('ChqDate').focus();
            document.getElementById('ChqDate').select();
            return false;
        }
        if (dd > 31) {
            alert('Please enter correct date!');
            document.getElementById('ChqDate').focus();
            document.getElementById('ChqDate').select();
            return false;
        }
        if (mm > 12) {
            alert('Please enter correct date!');
            document.getElementById('ChqDate').focus();
            document.getElementById('ChqDate').select();
            return false;
        }

    }


    ///----------------------------------------------------------------------------------------//
    if (IWdecn != 'R') {
        var cntv = document.getElementById('cnt').value;
        if (cntv != '0111') {
            alert('Please check complete fields (Payee Name,Amt and Date)!');
            document.getElementById('PayeeName').focus();
            return false;
        }
    }


    var chqdt = document.getElementById('ChqDate').value;
    if (chqdt.length <= 0 || chqdt.length < 2) {
        alert('Please enter correct Date!');
        document.getElementById('ChqDate').focus();
        return false;
    }



    // alert('Aila');
    if (IWdecn == "") {
        alert('Please enter decision!');
        document.getElementById('IWDecision').focus();
        return false;
    }
    if (IWdecn == "A") {

        var PAyee = document.getElementById('PayeeName').value;
        if (PAyee == "") {

            alert("Payee field should not be empty !");
            document.getElementById('PayeeName').focus();
            document.getElementById('PayeeName').select();
            return false;
        }
        if (PAyee.length < 5 && PAyee != "") {
            //alert('aila');
            alert("Enter minimum 5 character for payee name !");
            document.getElementById('PayeeName').focus();
            document.getElementById('PayeeName').select();
            return false;
        }
        if ((PAyee.toUpperCase().substring(0, 5) == "NNNNN" || (PAyee.toUpperCase().substring(0, 5) == "00000"))) {
            alert('Please Check Payee Name!!!');
            document.getElementById('PayeeName').focus();
            document.getElementById('PayeeName').select();
            return false;
        }

    }
    if (IWdecn != "A" && IWdecn != "R" && IWdecn != "C") {
        alert('Decision not correct!');
        document.getElementById('IWDecision').focus();
        return false;
    }
    if (IWdecn == "R") {
        if (document.getElementById('IWRemark').value == "") {
            alert('Please enter reject reason!');
            document.getElementById('IWRemark').focus();
            return false;
        }
    }
    else if (IWdecn == "C") {
        if (document.getElementById('IWRemark').value == "") {
            alert('Please enter remark!');
            document.getElementById('IWRemark').focus();
            return false;
        }
    }
}

function windowcall() {
    //    javascript:window.history.forward();
    //  alert('Ohh!');
    var url = document.getElementById('sign').value;
    url = url + '=' + document.getElementById('DbtAccNo').value;
    // document.getElementById('signchk').value = "Y";
    window.open(url, 'Signature', 'width=500,height=500,left=900,scrollbars=yes,titlebar=yes,resizable=no,location=yes,toolbar=0,status=1').focus();
}

function addCommas(amount) {
    //alert('Suche');
    var flg = true;
    var count = 0;
    var finaloutpt = "";
    //        var nStr = document.getElementById("amt").value;
    var nStr = amount;
    alert(nStr);
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
                finaloutpt = "," + finaloutpt;
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
    //                var amount = new String(nStr);
    //                amount = amount.split("").reverse();

    //                var output = "";
    //                for (var i = 0; i <= amount.length - 1; i++) {
    //                    output = amount[i] + output;
    //                    if ((i + 1) % 3 == 0 && (amount.length - 1) !== i) output = ',' + output;
    //                }
    //                alert(output);
    //                return output;

    //alert(finaloutpt+x2);
    return (finaloutpt + x2);
}
