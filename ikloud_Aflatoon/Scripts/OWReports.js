﻿
$(document).ready(function () {
    
    $(function () {
        $("#fromdate").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            yearRange: '-100y:c+nn',
            //defaultDate: '09/09/21',
            // maxDate: '-1d'
        });
    });

     $(function () {
        $("#todate").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            yearRange: '-100y:c+nn',
            // maxDate: '-1d'
        });
    });

    var processingDate = document.getElementById('ProcessingDate').value;
    console.log(processingDate);
    
    $("#fromdate").val(processingDate);
    //$("#fromdate").datepicker('setDate', new Date(currentValue));
    $("#todate").val(processingDate);
   
    $("#rpttype").change(function () {

        //var clertype = document.getElementById('clrtype')
        var clerval = $("#rpttype").val();

        if (clerval == "Verification Report") {

            document.getElementById('vflevel').style.display = 'block';

        }
        else {
            document.getElementById('vflevel').style.display = 'none';
        }

        if (clerval == "Item Wise Presentation Details" || clerval == "Batch Wise Summary Report" || clerval == "Presentment Details BranchWise Report"
            || clerval == "Return PullOut Report" || clerval == "Verification / CHI Reject Report" || clerval == "P2F PullOut Report"
            || clerval == "Return Memo Report" || clerval == "Presentment BranchWise Summary Report" || clerval == "Settlement Details BranchWise Report"
            || clerval == "Settlement BranchWise Summary Report"
            || clerval == "Return Memo With BranchName Report" || clerval == "Return Memo With Image Report" || clerval == "Return Details Report With BranchWise"
            || clerval == "Return Details Report With BranchWise Summary"
            || clerval == "P2F Details BranchWise Report" || clerval == "P2F BranchWise Summary Report" || clerval == "PPS Report"
            || clerval == "Outward MICR Repair Report") {

            document.getElementById('scanningTypeLevelDiv').style.display = 'block';

        }
        else {
            document.getElementById('scanningTypeLevelDiv').style.display = 'none';
        }

    });

    $("#btnreport").click(function () {

        //if ($("#procdate").val() == "" || $("#procdate").val() == null) {
        //    alert('Please select processing date!!');
        //    $("#procdate").focus();
        //    return false;
        //}



        if ($("#gridDomains").val() != "") {

            if ($("#Branch").val() == "") {
                alert("Please Select Branch");
                $("#Branch").focus();
                return false;
            }
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
        if (reptval == "Verification Report") {
            var verlevel = document.getElementById('verflevel')
            var verlevlval = verlevel.options[verlevel.selectedIndex].text;
            if (verlevlval == "Select") {
                alert('Please select verification level!!');
                $("#verflevel").focus();
                return false;
            }
        }
        if (reptval == "Item Wise Presentation Details" || reptval == "Batch Wise Summary Report" || reptval == "Presentment Details BranchWise Report"
            || reptval == "Return PullOut Report" || reptval == "Verification / CHI Reject Report" || reptval == "P2F PullOut Report"
            || reptval == "Return Memo Report" || reptval == "Presentment BranchWise Summary Report" || reptval == "Settlement Details BranchWise Report"
            || reptval == "Settlement BranchWise Summary Report"
            || reptval == "Return Memo With BranchName Report" || reptval == "Return Memo With Image Report" || reptval == "Return Details Report With BranchWise"
            || reptval == "Return Details Report With BranchWise Summary"
            || reptval == "P2F Details BranchWise Report" || reptval == "P2F BranchWise Summary Report" || reptval == "PPS Report"
            || reptval == "Outward MICR Repair Report") {
            var verlevel1 = document.getElementById('scanningTypeLevel');
            console.log(verlevel1);
            var verlevlval1 = verlevel1.options[verlevel1.selectedIndex].text;
            console.log(verlevlval1);
            if (verlevlval1 == "Select") {
                alert('Please select Scanning Type!!');
                $("#scanningTypeLevel").focus();
                return false;
            }
        }

        var filedwnldtype = document.getElementById('filedwnldtype')
        var filedwnldtypeval = filedwnldtype.options[filedwnldtype.selectedIndex].text;
        if (filedwnldtypeval == "Select") {
            alert('Please select download file type!!');
            $("#filedwnldtype").focus();
            return false;
        }
        //alert($("#gridDomains").val());

        var ScanningType = $("#scanningTypeLevel").val();
        if (ScanningType === "No AddList File") {
            ScanningType = 2;
        }
        else if (ScanningType === "Scanning With AddList File") {
            ScanningType = 15;
        }
        else if (ScanningType === "CDK") {
            ScanningType = 11;
        }
	else if (ScanningType === "CMS") {
            ScanningType = 3;
        }
        else {
            ScanningType = 0;
        }

        //window.open(RootUrl + 'OWReports/OWActionReport?procdate=' + $("#procdate").val() + '&clrtypr=' + $("#clrtype").val() + '&reporttype=' + $("#rpttype").val() + '&vflevel=' + $("#verflevel").val() + '&filedwnldtype=' + $("#filedwnldtype").val() + '&domainid=' + $("#gridDomains").val());
        window.open(RootUrl + 'OWReports/OWActionReport?fromdate=' + $("#fromdate").val() + '&todate=' + $("#todate").val() + '&clrtypr=' + $("#clrtype").val() + '&reporttype=' + $("#rpttype").val() + '&vflevel=' + $("#verflevel").val() + '&filedwnldtype=' + $("#filedwnldtype").val() + '&domainid=' + $("#gridDomains").val() + '&scanningtype=' + ScanningType + '&expiryTime=' + $("#SessionExpiryTime").val() + '&branchCode=' + $("#Branch").val());

        //$.ajax({
        //    url: RootUrl + 'IWReport/Report',
        //    data: JSON.stringify({ procdate: $("#procdate").val(), clrtypr: $("#clrtype").val(), reporttype: $("#rpttype").val(), vflevel: $("#verflevel").val() }),
        //    type: 'Post',
        //    contentType: 'application/json; charset=utf-8',
        //    dataType: 'json',
        //    success: function (result) {
        //        //alert('ok');
        //        //window.open(result);
        //    }
        //});

    });


  
  

});

function FillBranchDropdown(inputElement) {

    debugger;
    var inputValue = inputElement.value;

    if (inputValue == "") {
        var dropdown = $('#Branch');
        dropdown.empty();
        dropdown.append('<option value="">Select a Branch</option>');

        return false;
    }

    if (inputValue) {

        $.ajax({
            
            url: RootUrl + 'OWReports/getbranchname',
            type: 'GET',
            data: { Did: inputValue },  
            success: function (data) {
                // Handle success
                debugger
                console.log(data);  
                var dropdown = $('#Branch'); 
                dropdown.empty();
                dropdown.append('<option value="">Select a Branch</option>');
                $.each(data, function (index, branch) {
                    dropdown.append('<option value="' + branch.branchCode + '">' + branch.branchName + '</option>');
                });
            },
            error: function (xhr, status, error) {
                console.error('Error:', error);
            }
        });



    }

  

}