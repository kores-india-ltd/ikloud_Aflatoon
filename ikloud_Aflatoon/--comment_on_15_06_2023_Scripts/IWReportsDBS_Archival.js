﻿
$(document).ready(function () {

    //$(function () {
    //    $("#procdate").datepicker({
    //        dateFormat: 'mm/dd/yy',
    //        changeMonth: true,
    //        changeYear: true,
    //        yearRange: '-100y:c+nn',
    //        // maxDate: '-1d'
    //    });
    //});
    //var processingDate = document.getElementById('ProcessingDate').value;
    //console.log(processingDate);

    //$("#procdate").val(processingDate);
    $("#fromdate").prop('disabled', true);
    $("#todate").prop('disabled', true);
    var processingDate = document.getElementById('ProcessingDate').value;

    //$(function () {
    //    var year = (new Date).getFullYear();
    //    $("#fromdate").datepicker({
    //        dateFormat: 'dd/mm/yy',
    //        changeMonth: true,
    //        changeYear: false,
    //        //yearRange: '-100y:c+nn',
    //        //defaultDate: '09/09/21',
    //        //maxDate: '1y',
    //        //minDate: '-1y',
    //        minDate: new Date(year, 0, 1),
    //        maxDate: new Date(year, 11, 31),
    //        onSelect: function (selectedDate) {
    //            //if (d !== i.lastVal) {
    //            //    $(this).change();
    //            //}
    //            console.log("selectedDate - " + selectedDate);
    //            //var dateFormat = $("#fromdate").datepicker("option", "dateFormat");
    //            //$("#fromdate").datepicker("option", "dateFormat", 'mm/dd/yy');
    //            //var orginalDate = new Date(selectedDate);
    //            //console.log("orginalDate - " + orginalDate);
    //            //console.log("AddedDay " + (orginalDate.getMonth() + 1).getDate() - 1);
    //            //var monthsAddedDate = new Date(new Date(orginalDate).setMonth(orginalDate.getMonth() + 1));
    //            //var AddedDate = orginalDate.getMonth();
    //            //var AddedMonth = selectedDate.getDate() + 1;
    //            //var AddedYear = orginalDate.getFullYear();
    //            //console.log(monthsAddedDate);
    //            //console.log("AddedDate - " + AddedDate);
    //            //console.log("AddedMonth - " + AddedMonth);
    //            //console.log("AddedYear - " + AddedYear);
    //            //var setNextMonthDate = AddedDate + "/" + AddedMonth + "/" + AddedYear;
    //            //console.log("setNextMonthDate - " + setNextMonthDate);

    //            var dateObject = $("#fromdate").datepicker("getDate"); // get the date object
    //            console.log("dateObject - " + dateObject);
    //            //var dateString = dateObject.getFullYear() + '-' + (dateObject.getMonth() + 1) + '-' + dateObject.getDate();
    //            var dateString = (dateObject.getDate() - 1) + '/' + (dateObject.getMonth() + 1) + '/' + dateObject.getFullYear();
    //            console.log("dateString - " + dateString);
    //            $("#todate").datepicker("option", 'minDate', selectedDate);
    //            $("#todate").datepicker("option", 'maxDate', new Date(dateObject.getFullYear(), dateObject.getMonth() + 1, dateObject.getDate() - 1));
    //            $("#todate").val(selectedDate);
    //            //$("#todate").datepicker("option", 'maxDate', (orginalDate.getMonth() + 1).getDate() - 1);
    //        }
    //    });
    //});

    $(function () {
        $("#todate").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: false,
            changeYear: false,
            //yearRange: '-100y:c+nn',
            maxDate: '30d',
            minDate: processingDate
        });
    });
    
    console.log(processingDate);

    //$("#fromdate").val(processingDate);
    //$("#todate").val(processingDate);

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
        $("#fromdate").prop('disabled', true);
        $("#todate").prop('disabled', true);
        $("#fromdate").val('');
        $("#todate").val('');
        var dbYear = document.getElementById('dbYear');

        var dbYearString = dbYear.options[dbYear.selectedIndex].text;

        console.log("dbYearString - " + dbYearString);
        var currentYear = new Date(dbYear, 01, 01);
        console.log("currentYear - " + currentYear);
        var year = currentYear.getFullYear();
        console.log("year - " + year);

        if (dbYearString === "Select") {
            $("#fromdate").prop('disabled', true);
            $("#todate").prop('disabled', true);
        }
        else {
            $("#fromdate").prop('disabled', false);
            //$("#todate").prop('disabled', false);
            $("#fromdate").datepicker('setDate', new Date(dbYearString, 0, 1));
            //$("#fromdate").val(processingDate);
            var newDate = new Date(dbYearString, 0, 1);
            var dtString = newDate.getDate() + '/' + (newDate.getMonth() + 1) + '/' + newDate.getFullYear();
            //$("#fromdate").datepicker("option", 'defaultDate', new Date(newDate.getFullYear(), newDate.getMonth() + 1, newDate.getDate() - 1));
            $("#fromdate").datepicker("option", 'defaultDate', dtString);
            $("#fromdate").val(dtString);

            $("#fromdate").datepicker({
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
                    $("#todate").prop('disabled', false);
                    $("#todate").datepicker({
                        dateFormat: 'dd/mm/yy',
                        changeMonth: false,
                        changeYear: false,
                        //yearRange: '-100y:c+nn',
                        maxDate: '30d',
                        minDate: processingDate
                    });

                    console.log("selectedDate - " + selectedDate);

                    var dateObject = $("#fromdate").datepicker("getDate"); // get the date object
                    console.log("dateObject - " + dateObject);
                    //var dateString = dateObject.getFullYear() + '-' + (dateObject.getMonth() + 1) + '-' + dateObject.getDate();
                    var dateString = (dateObject.getDate() - 1) + '/' + (dateObject.getMonth() + 1) + '/' + dateObject.getFullYear();
                    console.log("dateString - " + dateString);
                    $("#todate").datepicker("option", 'minDate', selectedDate);
                    $("#todate").datepicker("option", 'maxDate', new Date(dateObject.getFullYear(), dateObject.getMonth() + 1, dateObject.getDate() - 1));
                    $("#todate").val(selectedDate);
                    //$("#todate").datepicker("option", 'maxDate', (orginalDate.getMonth() + 1).getDate() - 1);
                }
            });
        }
    });

    $("#rpttype").change(function () {

        //var clertype = document.getElementById('clrtype')
        var clerval = $("#rpttype").val();

        if (clerval == "Productivity Report" || clerval == "Verification Report" || clerval == "Audit Report") {
            document.getElementById('vflevel').style.display = 'block';

        }
        else {
            document.getElementById('vflevel').style.display = 'none';
            document.getElementById("userdiv").style.display = "none";
        }


    });
    $("#verflevel").change(function () {
        // debugger;
        var clervalval = $("#rpttype").val();
        var vftypeval = $("#verflevel").val();
        if (clervalval == "Audit Report") {
            if (clervalval == "Audit Report" && vftypeval != "L1 Verification" && vftypeval != "Select") {
                //$.ajax({
                //    url: RootUrl + 'IWReport/getUsers',
                //    type: 'post',
                //    data: {
                //        VFlevel: vftypeval
                //    }
                //}).done(function (response) {
                //    $("listuser").html(response);

                //});
                document.getElementById("userdiv").style.display = "";
            }
            //else {
            //    alert('Only L2 And L3 can be generated!!');
            //    document.getElementById("userdiv").style.display = "none";

            //    document.getElementById('verflevel').value = "Select";
            //    $("#verflevel").focus();
            //    return false;
            //}
        }



    });

    $("#btnreport").click(function () {

        //if ($("#procdate").val() == "" || $("#procdate").val() == null) {
        //    alert('Please select processing date!!');
        //    $("#procdate").focus();
        //    return false;
        //}

        var dbYear = document.getElementById('dbYear');
        var dbYearString = dbYear.options[dbYear.selectedIndex].text;
        if (dbYearString === "Select") {
            alert('Please select DB Year!!');
            $("#dbYear").focus();
            return false;
        }

        if ($("#fromdate").val() == "" || $("#fromdate").val() == null) {
            alert('Please select from date!!');
            $("#fromdate").focus();
            return false;
        }

        if ($("#todate").val() == "" || $("#todate").val() == null) {
            alert('Please select to date!!');
            $("#todate").focus();
            return false;
        }

        var clertype = document.getElementById('clrtype')
        var clerval = clertype.options[clertype.selectedIndex].text;
        if (clerval == "Select") {
            alert('Please select clearing type!!');
            $("#clrtype").focus();
            return false;
        }
        var repttype = document.getElementById('rpttype')
        var reptval = repttype.options[repttype.selectedIndex].text;
        if (reptval == "Select") {
            alert('Please select report type!!');
            $("#rpttype").focus();
            return false;
        }
        // debugger;
        // var vftype = $("#verflevel").val();
        if (reptval == "Productivity Report" || reptval == "Verification Report" || reptval == "Audit Report") {
            var verlevel = document.getElementById('verflevel');
            var verlevlval = verlevel.options[verlevel.selectedIndex].text;
            // alert(verlevlval);
            if (verlevlval == "Select") {
                alert('Please select verification level!!');
                $("#verflevel").focus();
                return false;
            }
            // && verlevlval == "L1 Verification"
            //if (reptval == "Audit Report") {
            //    alert('Only L2 And L3 can be generated!!');
            //    $("#verflevel").focus();
            //    return false;
            //}
        }
        var filedwnldtype = document.getElementById('filedwnldtype')
        var filedwnldtypeval = filedwnldtype.options[filedwnldtype.selectedIndex].text;
        if (filedwnldtypeval == "Select") {
            alert('Please select download file type!!');
            $("#filedwnldtype").focus();
            return false;
        }

        window.open(RootUrl + 'IWReportDBS_Archival/IWActionReport?fromdate=' + $("#fromdate").val() + '&todate=' + $("#todate").val() + '&clrtypr=' + $("#clrtype").val() + '&reporttype=' + $("#rpttype").val() + '&vflevel=' + $("#verflevel").val() + '&UserName=' + $("#listuser").val() + '&filedwnldtype=' + $("#filedwnldtype").val() + '&dbyear=' + $("#dbYear").val());


    });
});