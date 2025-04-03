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
//var scondbck = false;
function passval(array) {

    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}

$(document).ready(function () {

    $('#ChqAcno,#ChqPayeeName,#ChqAmt,#ChqDate').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    $('#ChqDate,#ChqAcno,#ChqAmt').keypress(function (event) {
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

    $("#ChqDate").focus();


    $("#ChqDate").on('keypress', function () {
        if ($(this).val().length > 6) {
            alert("Length cannot be more than 6 digits.");
            return false;
        }
    });

    $("#ChqAcno").on('keypress', function () {
        if ($(this).val().length > 16) {
            alert("Length cannot be more than 16 digits.");
            return false;
        }
    });

    $("#ChqAmt").on('keypress', function () {
        if ($(this).val().length > 13) {
            alert("Length cannot be more than 13 digits.");
            return false;
        }
    });

    $("#ChqPayeeName").on('keypress', function () {
        if ($(this).val().length > 30) {
            alert("Length cannot be more than 30 characters.");
            return false;
        }
    });

   

    $("#ok").click(function () {
        //  alert($("#Payee").val());

        if (!$('#ChqDate').val()) {
            alert('Date cannot be blank.');
        }

        if (!$('#ChqAcno').val()) {
            alert('Account No cannot be blank.');
        }

        if (!$('#ChqAmt').val()) {
            alert('Amount cannot be blank.');
        }

        if (!$('#ChqPayeeName').val()) {
            alert('Payee Name cannot be blank.');
        }

      });
});