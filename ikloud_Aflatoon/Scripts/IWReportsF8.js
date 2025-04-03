
$(document).ready(function () {

    $(function () {
        //$("#procdate").datepicker({
        //    dateFormat: 'mm/dd/yy',
        //    changeMonth: true,
        //    changeYear: true,
        //    yearRange: '-100y:c+nn',
        //    // maxDate: '-1d'
        //});

        $("#procdate").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            yearRange: '-100y:c+nn',
            //defaultDate: '09/09/21',
            // maxDate: '-1d'
        });
    });
    var processingDate = document.getElementById('ProcessingDate').value;
    console.log(processingDate);

    $("#procdate").val(processingDate);

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
                
                document.getElementById("userdiv").style.display = "";
            }
            
        }
    });

    $("#btnreport").click(function () {

        if ($("#procdate").val() == "" || $("#procdate").val() == null) {
            alert('Please select processing date!!');
            $("#procdate").focus();
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

        window.open(RootUrl + 'IWReportF8/IWActionReport?procdate=' + $("#procdate").val() + '&clrtypr=' + $("#clrtype").val() + '&reporttype=' + $("#rpttype").val() + '&vflevel=' + $("#verflevel").val() + '&UserName=' + $("#listuser").val() + '&filedwnldtype=' + $("#filedwnldtype").val() + '&domainid=' + $("#gridDomains").val());


    });
});