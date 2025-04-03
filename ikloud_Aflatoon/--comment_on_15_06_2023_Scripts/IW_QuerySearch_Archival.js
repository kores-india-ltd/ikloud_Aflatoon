$(document).ready(function () {

    var selectedcheque;

    //$('#ProcessingDate').datepicker().datepicker('setDate', 'today');
    //$('#ToProcessingDate').datepicker().datepicker('setDate', 'today');
    //$("#ProcessingDate").focus();
    //$(function () {
    //    $("#ProcessingDate,#ToProcessingDate").datepicker({
    //        //dateFormat: 'mm/dd/yy',
    //        dateFormat: 'dd/mm/yy',
    //        changeMonth: true,
    //        changeYear: true,
    //        yearRange: '-100y:c+nn',
    //        // maxDate: '-1d'
    //    });
    //});
    //RootUrl + 

    $("#ProcessingDate").prop('disabled', true);
    $("#ToProcessingDate").prop('disabled', true);

    var processingDate = document.getElementById('procdate').value;
    console.log(processingDate);

    //============ For filling DB Year dropdown start ===========================
    $("#dbYear").empty();
    var listDate = processingDate.split('/');
    console.log("first - " + listDate[0]);
    console.log("second - " + listDate[1]);
    console.log("third - " + listDate[2]);
    var processingYear = listDate[2].substring(4, 2);
    //var processingYear1 = listDate[2].substring(4, 2);
    //var processingYear2 = listDate[2].substring(3, 0);
    console.log("ProcessingYear - " + processingYear);
    //console.log("ProcessingYear - " + processingYear1);
    //console.log("ProcessingYear - " + processingYear2);
    $("#dbYear").append(
        $('<option></option>').val(0).html("Select"));
    for (var i = 21; i <= processingYear; i++) {
        console.log("i = " + i);
        var j = "20" + i;
        $("#dbYear").append(
            $('<option></option>').val(j).html(j));
        //if (i <= processingYear) {
        //    console.log("In i - " + i);


        //    if (i === processingYear) {
        //        break;
        //    }
        //}
    }

    //============ For filling DB Year dropdown finished ===========================

    $("#dbYear").change(function () {
        $("#ProcessingDate").prop('disabled', true);
        $("#ToProcessingDate").prop('disabled', true);
        $("#ProcessingDate").val('');
        $("#ToProcessingDate").val('');
        var dbYear = document.getElementById('dbYear');

        var dbYearString = dbYear.options[dbYear.selectedIndex].text;

        console.log("dbYearString - " + dbYearString);
        var currentYear = new Date(dbYear, 01, 01);
        console.log("currentYear - " + currentYear);
        var year = currentYear.getFullYear();
        console.log("year - " + year);

        if (dbYearString === "Select") {
            $("#ProcessingDate").prop('disabled', true);
            $("#ToProcessingDate").prop('disabled', true);
        }
        else {
            $("#ProcessingDate").prop('disabled', false);
            //$("#ToProcessingDate").prop('disabled', false);
            $("#ProcessingDate").datepicker('setDate', new Date(dbYearString, 0, 1));
            //$("#fromdate").val(processingDate);
            var newDate = new Date(dbYearString, 0, 1);
            var dtString = newDate.getDate() + '/' + (newDate.getMonth() + 1) + '/' + newDate.getFullYear();
            //$("#fromdate").datepicker("option", 'defaultDate', new Date(newDate.getFullYear(), newDate.getMonth() + 1, newDate.getDate() - 1));
            $("#ProcessingDate").datepicker("option", 'defaultDate', dtString);
            $("#ProcessingDate").val(dtString);

            $("#ProcessingDate").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: false,
                //yearRange: '2021:2021',
                //defaultDate: new Date(dbYearString, 0, 1),
                //maxDate: '1y',
                //minDate: '-1y',
                minDate: new Date(dbYearString, 0, 1),
                maxDate: new Date(dbYearString, 11, 31),
                onSelect: function (selectedDate) {
                    $("#ToProcessingDate").prop('disabled', false);
                    $("#ToProcessingDate").datepicker({
                        dateFormat: 'dd/mm/yy',
                        changeMonth: false,
                        changeYear: false,
                        //yearRange: '-100y:c+nn',
                        maxDate: '30d',
                        minDate: processingDate
                    });

                    console.log("selectedDate - " + selectedDate);

                    var dateObject = $("#ProcessingDate").datepicker("getDate"); // get the date object
                    console.log("dateObject - " + dateObject);
                    //var dateString = dateObject.getFullYear() + '-' + (dateObject.getMonth() + 1) + '-' + dateObject.getDate();
                    var dateString = (dateObject.getDate() - 1) + '/' + (dateObject.getMonth() + 1) + '/' + dateObject.getFullYear();
                    console.log("dateString - " + dateString);
                    $("#ToProcessingDate").datepicker("option", 'minDate', selectedDate);
                    $("#ToProcessingDate").datepicker("option", 'maxDate', new Date(dateObject.getFullYear(), dateObject.getMonth() + 1, dateObject.getDate() - 1));
                    $("#ToProcessingDate").val(selectedDate);
                    //$("#todate").datepicker("option", 'maxDate', (orginalDate.getMonth() + 1).getDate() - 1);
                }
            });
        }
    });

    $("#btnsrch").click(function () {

        var flg = validEmpty();
        if (flg == true) {
            document.getElementById('loader').style.display = "";
            //   alert($("#clrtype").val());
            var dbyear = $("#dbYear").val();

            var p2f = "false";
            var eod = "false";
            if ($("#p2f").is(":checked")) {
                // alert("Checkbox is checked.");
                p2f = "true";
            }
            //debugger;
            //var extrvalarry = [];
            //extrvalarry.push($("#ProcessingDate").val());
            //extrvalarry.push($("#ToProcessingDate").val());
            //extrvalarry.push($("#XMLSerialNo").val());
            //extrvalarry.push($("#XMLAmount").val());
            //extrvalarry.push($("#AccountNo").val());
            //extrvalarry.push($("#XMLPayorBankRoutNo").val());
            //extrvalarry.push($("#XMLTrns").val());
            //extrvalarry.push($("#clrtype").val());
            //console.log(extrvalarry);
            //console.log("Url:- " + RootUrl + "Query_IW_Module/SearchIndex");

            $.ajax({
                url: RootUrl + 'IW_QuerySearch_Archival/QuerySearch',
                data: {
                    ProcessingDate: $("#ProcessingDate").val(), ToProcessingDate: $("#ToProcessingDate").val(), XMLSerialNo: $("#XMLSerialNo").val(), XMLAmount: $("#XMLAmount").val(),
                    AccountNo: $("#AccountNo").val(), XMLPayorBankRoutNo: $("#XMLPayorBankRoutNo").val(), XMLTrns: $("#XMLTrns").val(), clrtype: $("#clrtype").val(), P2f: p2f, EOD: eod, DbYear: dbyear
                },
                //data: {},
                //data: {
                //    ProcessingDate: $("#ProcessingDate").val(), ToProcessingDate: $("#ToProcessingDate").val(), XMLSerialNo: $("#XMLSerialNo").val()
                //},
                //type: 'POST',
                //contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                type: 'POST',
                async: false,
                success: function (data) {
                    //alert(data);
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
    var dbYear = document.getElementById('dbYear');
    var dbYearString = dbYear.options[dbYear.selectedIndex].text;
    if (dbYearString === "Select") {
        alert('Please select DB Year!!');
        $("#dbYear").focus();
        return false;
    }

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
            url: RootUrl + 'IW_QuerySearch_Archival/MarkRtn',
            data: { id: ids, actn: "M", npcirtncd: rtnval, rtnrjctdescrn: rtnrjctdescrn },
            dataType: 'html',
            type: 'POST',
            async: false,
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
            },
            error: function (data) {
                alert(data);
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
        url: RootUrl + 'IW_QuerySearch_Archival/MarkRtn',
        data: { id: id, actn: "U" },
        dataType: 'html',
        type: 'POST',
        async: false,
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
    //debugger;
    var dbyear = $("#dbYear").val();
    document.getElementById('loader').style.display = "";
    var eod = "";
    var EOD = "";
    $.ajax({
        //url: RootUrl + 'IWSearch/CheqDetls',
        //url: RootUrl + 'IW_QuerySearch/ShowData',
        url: RootUrl + 'IW_QuerySearch_Archival/CheqDetls',
        data: { id: selectchq, EOD: EOD, DbYear: dbyear },
        //data: {},
        dataType: 'html',
        type: 'POST',
        async: false,

        success: function (data) {
            //alert(data);
            if (data == false) {
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

function fullImageClose() {
    //alert('ok');
    document.getElementById('iwimg').style.display = 'none';
    document.getElementById('chqdtlss').style.display = "";
}

function fullImage() {
    //alert('ok');
    document.getElementById('iwimg').style.display = "block";
    document.getElementById('chqdtlss').style.display = 'none';
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}

function ChangeImage(imagetype, status) {

    //alert(imagetype);
    //  var indexcnt = document.getElementById('cnt').value; 
    //if (imagetype == "FTiff") {

    //    document.getElementById('myimg').src = document.getElementById('FrontTiffImagePath').value;
    //}
    //else if (imagetype == "BTiff") {
    //    //alert('Browser not supporting!!!'); 
    //    document.getElementById('myimg').src = document.getElementById('BackTiffImagePath').value;
    //}
    //else if (imagetype == "FGray") {

    //    document.getElementById('myimg').src = document.getElementById('FrontGreyImagePath').value;
    //}

    console.log("Status : " + status);

    if (status == "2" || status == "4" || status == "12" || status == "14" || status == "22" || status == "24") {
        console.log("In If " + status);
        if (imagetype == "frntTiffimg") {
            // alert($("#FrontTiffImagePath").val());
            console.log("In If frontTiffImg " + status);
            tiffimagecall = true;

            $.ajax({
                //url: RootUrl + 'OWSearch/getTiffImage',
                url: RootUrl + 'IW_QuerySearch_Archival/getTiffImageNew',
                dataType: 'html',
                data: {},
                success: function (Slipdata) {
                    //debugger;
                    document.getElementById("myimg").style.display = "none";
                    $('#divtiff').html(Slipdata);
                    console.log("frntTiffimg value : " + document.getElementById('frntTiffimg').value);
                    document.getElementById('myimg1').src = document.getElementById('frntTiffimg').value;
                    console.log("tiffimg html : " + $('#divtiff').html());

                    //document.getElementById('myimg').src = Slipdata;
                    document.getElementById('myimg').style.display = "none";
                    document.getElementById('divtiff').style.display = "block";
                    //-------------------Added on 02-03-2019---------
                    var Tfimg = document.getElementById('myimg1');
                    if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                        document.getElementById('divtiff').style.display = "";
                    }
                }
            });

            //document.getElementById('myimg').src = document.getElementById('FrontTiffImagePath').value;
        }
        else if (imagetype == "backTiffimg") {
            //alert('Browser not supporting!!!'); 
            console.log("In If backTiffImg " + status);
            tiffimagecall = true;

            $.ajax({
                //url: RootUrl + 'OWSearch/getTiffImage',
                url: RootUrl + 'IW_QuerySearch_Archival/getTiffImageNew',
                dataType: 'html',
                data: {},
                success: function (Slipdata) {
                    //debugger;
                    document.getElementById("myimg").style.display = "none";
                    $('#divtiff').html(Slipdata);
                    document.getElementById("divtiff").style.display = "";
                    document.getElementById('myimg1').src = document.getElementById('backTiffimg').value;
                    //document.getElementById('myimg').src = Slipdata;
                    document.getElementById('myimg').style.display = "none";
                    document.getElementById('divtiff').style.display = "block";
                    //-------------------Added on 02-03-2019---------
                    var Tfimg = document.getElementById('myimg1');
                    if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                        document.getElementById('divtiff').style.display = "";
                    }
                }
            });
            //document.getElementById('myimg').src = document.getElementById('BackTiffImagePath').value;
        }
        else if (imagetype == "frntGryimg") {
            console.log("In If frontGrayImg " + status);
            tiffimagecall = false;
            document.getElementById('myimg').src = document.getElementById('frntGryimg').value;
            document.getElementById('myimg').style.display = "block";
            document.getElementById('divtiff').style.display = "none";
        }

    }
    else {
        if (imagetype == "frntTiffimg") {
            // alert($("#FrontTiffImagePath").val());
            $.ajax({
                //url: RootUrl + 'OWSearch/getTiffImage',
                url: RootUrl + 'IW_QuerySearch_Archival/getTiffImage',
                dataType: 'html',
                data: { httpwebimgpath: $("#frntTiffimg").val() },
                success: function (Slipdata) {
                    //debugger;
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
                url: RootUrl + 'IW_QuerySearch_Archival/getTiffImage',
                dataType: 'html',
                data: { httpwebimgpath: $("#backTiffimg").val() },
                success: function (Slipdata) {
                    //debugger;
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
    

}

function fullImageTiff(imageData) {
    console.log("In fullImageTiff image - ", + imageData);
    //document.getElementById('myimg1').style.display = "none";
    document.getElementById('myfulimg').src = imageData;
    document.getElementById('iwimg1').style.display = "block";
    console.log("In fullImageTiff - ", + document.getElementById('myfulimg').src);
}
