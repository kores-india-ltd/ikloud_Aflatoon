

$(document).ready(function () {

    // alert('aila');
    $("#ChequeNoFinal,#SortCodeFinal,#SANFinal,#TransCodeFinal").keypress(function (event) {


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

        //alert(event.keyCode);

        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }

        else {

            if (event.keyCode == 80 || event.keyCode == 83 || event.keyCode == 78 || event.keyCode == 66 || event.keyCode == 71 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

            }
            else {
                event.preventDefault();
            }
        }
    });

    var value = 0;
    callrotate = function () {
        value += 180;
        $("#myimg").rotate({ animateTo: value })
    }
    //--------------------------------btnokk
    $("#btnok").click(function () {

        $.ajax({
            url: RootUrl + 'ChiRejectHandler/ChiReject',
            data: { customerid: $("#Customer").val() },
            type: 'GET',
            dataType: 'html',
            asyn: false,
            success: function (result) {
                if (result == "false") {
                    alert('No Data was found !!!');
                }
                else {
                    $('#chidetls').html(result);
                    $("#Decision").focus();

                }
            }

        });
    });
    //-------------------------
    $("#btnClose").click(function () {
        debugger;
        var allfieldsval = [];
        allfieldsval.push($("#Id").val());
        $.ajax({
            url: RootUrl + 'ChiRejectHandler/ChiReject',
            data: JSON.stringify({ allfieldsval: allfieldsval, btnvalue: "Closed" }),
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            success: function (result) {
                if (result == "false") {
                    window.location = RootUrl + 'Home/IWIndex';
                }
            }
        });

    });
    //-----------------------
    $("#btnacept").click(function () {

        // alert('aila');
        var flg = validMICR();
        if (flg != false) {
            var allfieldsval = [];
            allfieldsval.push($("#Customer").val());
            allfieldsval.push($("#ChequeNoFinal").val());
            allfieldsval.push($("#SortCodeFinal").val());
            allfieldsval.push($("#SANFinal").val());
            allfieldsval.push($("#TransCodeFinal").val());
            allfieldsval.push($("#Customer").val());
            allfieldsval.push($("#Id").val());
            allfieldsval.push($("#DomainID").val());
            allfieldsval.push($("#ScanningNodeId").val());
            allfieldsval.push($("#RawDataID").val());
            allfieldsval.push($("#Customer_ID").val());
            allfieldsval.push($("#Decision").val());

            var IQAIgnore = false;
            if ($("#ignriqa").is(":checked")) {
                // alert("Checkbox is checked.");
                IQAIgnore = true;
            }

            $.ajax({
                url: RootUrl + 'ChiRejectHandler/ChiReject',
                data: JSON.stringify({ allfieldsval: allfieldsval, IQAIgnore: IQAIgnore }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                success: function (tempresult) {
                    if (tempresult == "true") {
                        $("#chidetls").empty();
                        $.ajax({
                            url: RootUrl + 'ChiRejectHandler/ChiReject',
                            data: { customerid: $("#Customer").val() },
                            type: 'GET',
                            dataType: 'html',
                            success: function (lastresult) {
                                if (lastresult == "false") {
                                    $("#chidetls").empty();
                                    alert('No Data was found !!!');
                                    $("#Customer").prop("disabled", false);
                                }
                                else {
                                    $('#chidetls').html(lastresult);
                                    $("#Decision").focus();
                                    $("input[value='OK']").attr("disabled", false);

                                }
                            }

                        });
                    }
                }
            });
        }
    });
    $("form input").keydown(function (e) {
        if (e.keyCode == 13) {
            $("input[value='OK']").attr("disabled", true);
            $("input[value='OK']").focus().click();
        }
    });

});



function validMICR() {
    //alert('aila');
    //  debugger;
    var ChqnoQC = document.getElementById('ChequeNoFinal').value;
    var SortQC = document.getElementById('SortCodeFinal').value;
    var SANQC = document.getElementById('SANFinal').value;
    var TransQC = document.getElementById('TransCodeFinal').value;

    if (ChqnoQC == "") {
        //alert('aila');
        alert("Cheque no should not be empty !");
        document.getElementById('ChequeNoFinal').focus();
        return false;
    }
    else if (SortQC == "") {
        //alert('aila');
        alert("Sort code should not be empty !");
        document.getElementById('SortCodeFinal').focus();
        return false;
    }
    else if (SANQC == "") {
        alert("SAN code should not be empty !");
        document.getElementById('SANFinal').focus();
        return false;
    }
    else if (TransQC == "") {
        alert("Trans code should not be empty !");
        document.getElementById('TransCodeFinal').focus();
        return false;
    }
    else if (SortQC.length < 9 || SortQC == "000000000") {
        alert("Sort code is not valid !");
        document.getElementById('SortCodeFinal').focus();
        return false;
    }
    else if (ChqnoQC.length < 6 || ChqnoQC == "000000") {
        alert("Cheque number is not valid !");
        document.getElementById('ChequeNoFinal').focus();
        return false;
    }
    else if (SANQC.length < 6) {
        alert("SAN code is not valid !");
        document.getElementById('SANFinal').focus();
        return false;
    }
    else if (TransQC.length < 2 || TransQC == "00" || TransQC.substring(0, 1) == "0") {
        alert("Trans code is not valid !");
        document.getElementById('TransCodeFinal').focus();
        return false;
    }
    var rtnflg = validYrnscodes();
    if (rtnflg == false) {
        alert("Trans code is not valid !");
        document.getElementById('TransCodeFinal').focus();

        return false;
    }
    if ($("#Decision").val() == "") {
        alert("Please enter decision!!");
        $("#Decision").focus();
        return false;
    }
}
function validYrnscodes() {
    var trnresult = $("#TransCodeFinal").val();// document.getElementById('TransCodeFinal').value;
    var flg;
    $.ajax({

        url: RootUrl + "ChiRejectHandler/ValidTrans",
        data: { transcode: trnresult },
        dataType: "html",
        async: false,
        success: function (trdata) {
            if (trdata == "false") {
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