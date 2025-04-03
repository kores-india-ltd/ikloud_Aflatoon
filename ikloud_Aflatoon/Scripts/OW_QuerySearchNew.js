$(document).ready(function () {

    $(function () {
        $("#ProcessingDate,#ToProcessingDate").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            yearRange: '-100y:c+nn',
            // maxDate: '-1d'
        });
    });

    $("#btnsrch").click(function () {

        var flg = validEmpty();
        if (flg === true) {
            document.getElementById('loader').style.display = "";

            var p2f = "false";
            if ($("#p2f").is(":checked")) {
                // alert("Checkbox is checked.");
                p2f = "true";
            }

            $.ajax({
                url: RootUrl + 'OW_QuerySearchNew/QuerySearch',
                data: {
                    ProcessingDate: $("#ProcessingDate").val(), ToProcessingDate: $("#ToProcessingDate").val(), XMLSerialNo: $("#XMLSerialNo").val(), XMLAmount: $("#XMLAmount").val(),
                    AccountNo: $("#AccountNo").val(), XMLPayorBankRoutNo: $("#XMLPayorBankRoutNo").val(), XMLTrns: $("#XMLTrns").val(), BranchCode: $("#BranchCode").val(), P2f: p2f
                },
                //contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                type: 'POST',
                async: false,
                success: function (data) {
                    $('#chqsrch').html(data);
                    document.getElementById('loader').style.display = "none";
                },
                error: function (data) {
                    alert(data);
                }
            });
        }

    });

    $("#btncls").click(function () {
        window.location = RootUrl + 'Home/IWIndex';
    });

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

function selectedcheque(selectchq) {
    
    document.getElementById('loader').style.display = "";
    $.ajax({
        url: RootUrl + 'OW_QuerySearchNew/CheqDetls',
        data: { id: selectchq },
        dataType: 'html',
        type: 'POST',
        async: false,
        success: function (data) {
            if (data === false) {
                window.location = RootUrl + 'Home/IWIndex?id=1';
            }
            else {
                $('#chqdtls').html(data);
            }
            //  document.getElementById('chqdtlss').style.display = "block";
            document.getElementById('loader').style.display = "none";
        },
        error: function (data) {
            alert(data);
        }
    });
}

function SlipImage() {
    debugger;

    var lblval = $("#lblslpimg").text();// document.getElementById('lblslpimg').value;

    if (lblval == "Slip Front Image") {

        $.ajax({
            //url: RootUrl + 'OWSearch/slipImage',
            url: RootUrl + 'OW_QuerySearchNew/slipImage',
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
            url: RootUrl + 'OW_QuerySearchNew/getTiffImage',
            dataType: 'html',
            async: false,
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
            url: RootUrl + 'OW_QuerySearchNew/getTiffImage',
            dataType: 'html',
            async: false,
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


//13-01-25

function ActivityLog(rawdataid, chequeno) {
    debugger;

   

    document.getElementById("activityDiv").style.display = "";

    var showaclbl = document.getElementById("lblActivitylog").innerHTML;
    if (showaclbl == "Show Activity Log") {

        $.ajax({
            url: RootUrl + 'OW_QuerySearchNew/GetActivityLog',
            data: { rawdataid: rawdataid, chequeno: chequeno },
            dataType: 'html',
            type: 'POST',
            async: false,
            success: function (data) {
                debugger;
                $('#activityDiv').html(data);
                document.getElementById("lblActivitylog").innerHTML = "Hide Activity Log";
            },
            error: function (data) {
                alert(data);
            }
        });
    }
    else
    {
        document.getElementById("activityDiv").style.display = "none";
        document.getElementById("lblActivitylog").innerHTML = "Show Activity Log";
    }


  

}
