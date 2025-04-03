$(document).ready(function () {
    debugger;
    //var selectedcheque;
    //var procdate = new Date();
    //var strprocdate = $("#procdate").val().split("/");;
    //procdate = strprocdate[1] + '/' + strprocdate[0] + '/' + strprocdate[2];

    //// alert(procdate);
    ////var date = $('#datepicker').datepicker({ dateFormat: 'dd-mm-yy' }).val();

    //('#ProcessingDate').datepicker().datepicker('setDate', procdate, { dateFormat: 'dd-mm-yy' });
    //$('#ToProcessingDate').datepicker().datepicker('setDate', procdate, { dateFormat: 'dd-mm-yy' });




    $(function () {
        $("#ProcessingDate,#ToProcessingDate").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            yearRange: '-100y:c+nn',
            // maxDate: '-1d'
        });
    });
    //RootUrl + 
    //$("#btnsrch").click(function () {

    //    var flg = validEmpty();
    //    if (flg === true) {
    //        document.getElementById('loader').style.display = "";

    //        var p2f = "false";
    //        var eod = "";
    //        if ($("#p2f").is(":checked")) {
    //            // alert("Checkbox is checked.");
    //            p2f = "true";
    //        }
    //        var extrvalarry = [];
    //        extrvalarry.push($("#ProcessingDate").val());
    //        extrvalarry.push($("#ToProcessingDate").val());
    //        extrvalarry.push($("#XMLSerialNo").val());
    //        extrvalarry.push($("#XMLAmount").val());
    //        extrvalarry.push($("#AccountNo").val());
    //        extrvalarry.push($("#XMLPayorBankRoutNo").val());
    //        extrvalarry.push($("#XMLTrns").val());
    //        extrvalarry.push($("#BranchCode").val());
    //        console.log(extrvalarry);

    //        window.open(RootUrl + 'Query_OW_Module/SearchQuery?ProcessingDate=' + extrvalarry[0] + '&ToProcessingDate=' + extrvalarry[1] + '&XMLSerialNo=' + extrvalarry[2] + '&XMLAmount=' + extrvalarry[3] + '&AccountNo=' + extrvalarry[4] + '&XMLPayorBankRoutNo=' + extrvalarry[5] + '&XMLTrns=' + extrvalarry[6] + '&BranchCode=' + extrvalarry[7] + '&P2f=' + p2f + '&EOD=' + eod);


    //        //$.ajax({
    //        //    //url: RootUrl + 'OWSearch/Query',
    //        //    //data: JSON.stringify({ extrafields: extrvalarry, P2f: p2f }),
    //        //    //type: 'POST',
    //        //    //url: RootUrl + 'OWSearch/SearchQuery',
    //        //    url: RootUrl + 'Query_OW_Module/SearchQuery',
    //        //    data: {
    //        //        ProcessingDate: extrvalarry[0], ToProcessingDate: extrvalarry[1], XMLSerialNo: extrvalarry[2], XMLAmount: extrvalarry[3],
    //        //        AccountNo: extrvalarry[4], XMLPayorBankRoutNo: extrvalarry[5], XMLTrns: extrvalarry[6], BranchCode: extrvalarry[7], P2f: p2f, EOD: eod
    //        //    },
    //        //    contentType: 'application/json; charset=utf-8',
    //        //    dataType: 'html',
    //        //    success: function (data) {
    //        //        $('#chqsrch').html(data);
    //        //        document.getElementById('loader').style.display = "none";
    //        //    },
    //        //    error: function(data){
    //        //        alert(data);
    //        //    }
    //        //});
    //    }
    //});
    $("#btncls").click(function () {
        window.location = RootUrl + 'Home/IWIndex';
    });

    //$("form input").keydown(function (e) {
    //    if (e.keyCode == 13) {
    //       // $("input[value='Search']").attr("disabled", true);
    //        $("input[value='Search']").focus().click();


    //    }
    //});

    $("#XMLAmount").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || event.charCode == 46 || (event.charCode > 47 && event.charCode < 58)) {

            var amtval = $("#XMLAmount").val();
            if (amtval.length > 0) {
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

        }
        else {
            event.preventDefault();
        }
    });
    $("#AccountNo,#XMLPayorBankRoutNo,#XMLTrns,#XMLSerialNo").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
            // alert('if');
        }
        else {
            event.preventDefault();
        }
    });

    $("#ProcessingDate,#ToProcessingDate").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

            //if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
            //    // alert('if');
            //}
        else {
            event.preventDefault();
        }
    });

});

function validEmpty() {
    // debugger;
    var filedselected = true;
    if (document.getElementById('ProcessingDate').value == "" || document.getElementById('ProcessingDate').value == null) {
        alert('Enter processing date!!');
        document.getElementById('ProcessingDate').focus();
        return false;
    }
    if (document.getElementById('XMLSerialNo').value == "" || document.getElementById('XMLSerialNo').value == null) {
        filedselected = false;
    }
    else {
        return (true);
    }
    if (document.getElementById('XMLAmount').value == "" || document.getElementById('XMLAmount').value == null) {
        filedselected = false;
    }
    else {
        return (true);
    }
    if (document.getElementById('AccountNo').value == "" || document.getElementById('AccountNo').value == null) {
        filedselected = false;
    }
    else {
        return (true);
    }
    if (document.getElementById('XMLPayorBankRoutNo').value == "" || document.getElementById('XMLPayorBankRoutNo').value == null) {
        filedselected = false;
    }
    else {
        return (true);
    }
    if (document.getElementById('XMLTrns').value == "" || document.getElementById('XMLTrns').value == null) {
        filedselected = false;
    }
    else {
        return (true);
    }
    if (document.getElementById('BranchCode').value == "" || document.getElementById('BranchCode').value == null) {
        filedselected = false;
    }
    else {
        return (true);
    }
    if ($("#p2f").is(":checked")) {
        return (true);
    }
    else {
        filedselected = false;
    }
    if (filedselected == false) {
        alert('Enter at least one more field for search!!');
        document.getElementById('XMLSerialNo').focus();
        return false;
    }
    else {
        return (true);
    }
}
function selectrsn(ids, calfrm) {
    if (calfrm == "chqdtls") {
        document.getElementById('chqdtlss').style.display = 'none'
    }


    document.getElementById('rtnID').value = ids;
    document.getElementById('rjctrsn').style.display = 'block';
}
function mrkRtn(rtnval) {

    var rtnrjctdescrn = document.getElementById('rtndescrp').value;
    //-----valid Function for validation---------------
    var rslt = valid(document.getElementById('rtndescrp').value, rtnval);

    if (rslt == false) {
        //alert('Please select reject reason!!');
        document.getElementById('rtndescrp').focus();
        document.getElementById('rjctrsn').style.display = 'block';
        return false;
    }
    else {
        var ids = document.getElementById('rtnID').value;

        $.ajax({
            //url: RootUrl + 'IWSearch/MarkRtn',
            url: RootUrl + 'Query_IW_Module/MarkRtn',
            type: 'POST',
            dataType: 'html',
            data: { id: ids, actn: "M", npcirtncd: rtnval, rtnrjctdescrn: rtnrjctdescrn },
            success: function (data) {
                if (data == false) {
                    window.location = RootUrl + 'Home/IWIndex?id=1';
                }
                else {
                    alert('Return has been marked successfully!!');
                    $("input[value='Search']").focus().click();
                }
                //  document.getElementById('chqdtlss').style.display = "block";
                document.getElementById('loader').style.display = "none";
            }
        });
    }
    document.getElementById('rjctrsn').style.display = 'none';


}

//---------------iw Search mannual return validation----
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

function UnMark(id, calfrm) {



    $.ajax({
        //url: RootUrl + 'IWSearch/MarkRtn',
        url: RootUrl + 'Query_IW_Module/MarkRtn',
        type: 'POST',
        dataType: 'html',
        data: { id: id, actn: "U" },
        success: function (data) {
            if (data == false) {
                window.location = RootUrl + 'Home/IWIndex?id=1';
            }
            else {
                alert('Return has been Unmarked successfully!!');
                //-------------------Disabling Modal of cheq details---
                if (calfrm == "chqdtls") {
                    document.getElementById('chqdtlss').style.display = 'none'
                }

                $("input[value='Search']").focus().click();
            }
            //  document.getElementById('chqdtlss').style.display = "block";
            document.getElementById('loader').style.display = "none";
        }
    });
}

function selectedcheque(selectchq) {
    //document.getElementById('chqdtlss').style.display = "block";
    document.getElementById('loader').style.display = "";
    $.ajax({
        //url: RootUrl + 'OWSearch/CheqDetls',
        url: RootUrl + 'Query_OW_Module/CheqDetls',
        type: 'POST',
        dataType: 'html',
        data: { id: selectchq },
        success: function (data) {
            if (data == false) {
                window.location = RootUrl + 'Home/IWIndex?id=1';
            }
            else {
                $('#chqdtls').html(data);
            }
            //  document.getElementById('chqdtlss').style.display = "block";
            document.getElementById('loader').style.display = "none";
        }
    });
}

function SlipImage() {
    debugger;

    var lblval = $("#lblslpimg").text();// document.getElementById('lblslpimg').value;

    if (lblval == "Slip Front Image") {

        $.ajax({
            //url: RootUrl + 'OWSearch/slipImage',
            url: RootUrl + 'Query_OW_Module/slipImage',
            dataType: 'html',
            data: {
                processingdate: $("#ProcessingDate").val(), CustomerID: $("#CustomerID").val(), scanningNodeID: $("#ScanningID").val(), BranchCode: $("#BranchCode").val(),
                batchNo: $("#BatchNo").val(), SlipNo: $("#SlipNo").val()
            },
            success: function (Slipdata) {
                // alert(Slipdata);

                var dataarray = [];
                var frontslip;
                var backslip;
                var frontsliparr = [];
                var backSliparr = [];
                dataarray = Slipdata.split(",");
                frontsliparr = dataarray[0];
                backSliparr = dataarray[1];
                frontsliparr = frontsliparr.split("FrontGreyImagePath");
                backSliparr = backSliparr.split("BackGreyImagePath");
                debugger;
                frontslip = frontsliparr[1];
                backslip = backSliparr[1];
                frontslip = frontslip.substring(3, frontslip.length - 1);
                backslip = backslip.substring(3, backslip.length - 2);

                // document.getElementById('mytest').removeAttribute('src');
               // $("#myfulimg").attr("src", frontslip.replace('"', "").replace('"', ""));
                document.getElementById('myimg').src = frontslip;

                //document.getElementById('chqdtlss').style.display = 'none';
               // document.getElementById('iwimg').style.display = 'block';
                // document.getElementById('myfulimg').src = frontslip.replace('"',"");
                document.getElementById('lblslpimg').innerHTML = "Slip Back Image";
                document.getElementById('slpback').value = backslip.replace('"', "");
                // alert(frontslip);
                // alert(backslip);

                // debugger;
                //document.getElementById('myimg').src = Slipdata.replace('"', "").replace('"', "");
            }
        });

    }
    else {
       // $("#myfulimg").attr("src", document.getElementById('slpback').value.replace('"', "").replace('"', ""));
        document.getElementById('myimg').src = document.getElementById('slpback').value;
       // document.getElementById('chqdtlss').style.display = 'none';
        //document.getElementById('iwimg').style.display = 'block';
        //document.getElementById('myfulimg').src = document.getElementById('slpback').value;
        document.getElementById('lblslpimg').innerHTML = "Slip Front Image";
    }


}

function ChangeImage(imagetype) {
    debugger
    //alert(imagetype);
    //  var indexcnt = document.getElementById('cnt').value; 
    if (imagetype == "frntTiffimg") {
       // alert($("#FrontTiffImagePath").val());
        $.ajax({
            //url: RootUrl + 'OWSearch/getTiffImage',
            url: RootUrl + 'Query_OW_Module/getTiffImage',
            dataType: 'html',
            data: { httpwebimgpath: $("#frntTiffimg").val() },
            success: function (Slipdata) {
                debugger;
                $('#divtiff').html(Slipdata);
                //document.getElementById('myimg').src = Slipdata;
                document.getElementById('myimg').style.display = "none";
                document.getElementById('divtiff').style.display = "block";

            }
        });

        //document.getElementById('myimg').src = document.getElementById('FrontTiffImagePath').value;
    }
    else if (imagetype == "backTiffimg") {
        //alert('Browser not supporting!!!'); 

        $.ajax({
            //url: RootUrl + 'OWSearch/getTiffImage',
            url: RootUrl + 'Query_OW_Module/getTiffImage',
            dataType: 'html',
            data: { httpwebimgpath: $("#backTiffimg").val() },
            success: function (Slipdata) {
                debugger;
                $('#divtiff').html(Slipdata);
                //document.getElementById('myimg').src = Slipdata;
                document.getElementById('myimg').style.display = "none";
                document.getElementById('divtiff').style.display = "block";

            }
        });
        //document.getElementById('myimg').src = document.getElementById('BackTiffImagePath').value;
    }
    else if (imagetype == "frntGryimg") {

        document.getElementById('myimg').src = document.getElementById('frntGryimg').value;
        document.getElementById('myimg').style.display = "block";
        document.getElementById('divtiff').style.display = "none";
    }

}





