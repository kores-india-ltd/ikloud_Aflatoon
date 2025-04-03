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


var value = 0;
callrotate = function () {
    value += 180;
    $("#myimg,#ftiffimg").rotate({ animateTo: value })
}

function ChangeImage(ImagePath) {
    //alert(ImagePath);
        
        $.ajax({
            url: RootUrl + 'OWSMBDataEntry/getTiffImage',
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

        //document.getElementById('myimg').src = openFile(tt[indexcnt].FrontGreyImagePath);
        //alert('after image bind');

    

    //debugger;
    //if (imagetype == "FTiff") {
    //    $.ajax({
    //        url: RootUrl + 'OWSMBDataEntry/getTiffImage',
    //        dataType: 'html',
    //        data: { httpwebimgpath: tt[indexcnt].FrontTiffImagePath },
    //        success: function (Slipdata) {
    //            //debugger;
    //            $('#divtiff').html(Slipdata);
    //            //document.getElementById('myimg').src = Slipdata;
    //            document.getElementById('myimg').style.display = "none";
    //            document.getElementById('divtiff').style.display = "block";

    //        }
    //    });

    //    //document.getElementById('myimg').src = openFile(tt[indexcnt].FrontGreyImagePath);
    //    //alert('after image bind');

    //}
    //else if (imagetype == "BTiff") {
    //    //alert('Browser not supporting!!!');
    //    //document.getElementById('myimg').src = tt[indexcnt].BackTiffImagePath;
    //    //document.getElementById('myimg').style.display = "block";
    //    //document.getElementById('divtiff').style.display = "none";

    //    $.ajax({
    //        url: RootUrl + 'OWSMBDataEntry/getTiffImage',
    //        dataType: 'html',
    //        data: { httpwebimgpath: tt[indexcnt].BackTiffImagePath },
    //        success: function (Slipdata) {
    //            //debugger;
    //            $('#divtiff').html(Slipdata);
    //            //document.getElementById('myimg').src = Slipdata;
    //            document.getElementById('myimg').style.display = "none";
    //            document.getElementById('divtiff').style.display = "block";

    //        }
    //    });

    //}
    //else if (imagetype == "FGray") {
    //    document.getElementById('divtiff').style.display = "none";
    //    document.getElementById('myimg').style.display = "block";
    //    document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath;
    //}

}
function fullImage() {
    debugger;
    //alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}
function RemoveWhiteSpace() {
    var act = $("#ChqAcno").val().trim();
    $("#ChqAcno").val(act);
}
$(document).ready(function () {

    $('#ChqPayeeName').validate({
        rules: {
            field: {
                alphanumeric: true
            }
        }
    });

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
        if ($(this).val().length > 5) {
            alert("Length cannot be more than 6 digits.");
            return false;
        }
    });

    $("#ChqAcno").on('keypress', function () {
        if ($(this).val().length > (acmaxlength)) {
            alert("Length cannot be more than " + (acmaxlength+1) + " digits.");
            return false;
        }
    });

    $("#ChqAmt").on('keypress', function () {
        if ($(this).val().length > 12) {
            alert("Length cannot be more than 13 digits.");
            return false;
        }
    });

    $("#ChqPayeeName").on('keypress', function () {
        if ($(this).val().length > (payeemaxlength)) {
            alert("Length cannot be more than " + (payeemaxlength+1) + " characters.");
            return false;
        }
        $('#ChqPayeeName').keyup(function () {
            var $th = $(this);
            $th.val($th.val().replace(/[^a-zA-Z0-9\s]/g, function (str) { alert('You typed " ' + str + ' ".\n\nPlease use only letters and numbers.'); return ''; }));
        });
    });

    function IsEmpty() {
        if ($('#ChqDate').val() === "" && $('#ChqAcno').val() === "" && $('#ChqAmt').val() === "" && $('#ChqPayeeName').val() === "") {
            alert("Please fill all details.");
            return false;
        }
        return true;
    };

    //$("#btnSubmit").click(function () {
    //    //  alert($("#Payee").val());

    //    if (!$('#ChqDate').val()) {
    //        alert('Date cannot be blank.');
    //        $("#ChqDate").focus();
    //        return false;
    //    }

    //    else if (!$('#ChqAcno').val()) {
    //        alert('Account No cannot be blank.');
    //        $("#ChqAcno").focus();
    //        return false;
    //    }

    //    else if (!$('#ChqAmt').val()) {
    //        alert('Amount cannot be blank.');
    //        $("#ChqAmt").focus();
    //        return false;
    //    }

    //    else if (!$('#ChqPayeeName').val()) {
    //        alert('Payee Name cannot be blank.');
    //        $("#ChqPayeeName").focus();
    //        return false;
    //    }
       

    //});

   
});