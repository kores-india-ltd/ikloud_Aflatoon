
$(document).ready(function () {

    $(function () {
        $("#fromdate").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            yearRange: '-100y:c+nn',
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

   
    $("#rpttype").change(function () {

        //var clertype = document.getElementById('clrtype')
        var clerval = $("#rpttype").val();

        if (clerval == "Verification Report") {

            document.getElementById('vflevel').style.display = 'block';

        }
        else {
            document.getElementById('vflevel').style.display = 'none';
        }

        if (clerval == "Item Wise Presentation Details") {

            document.getElementById('scanningTypeLevel').style.display = 'block';

        }
        else {
            document.getElementById('scanningTypeLevel').style.display = 'none';
        }

    })

    $("#btnreport").click(function () {

        //if ($("#procdate").val() == "" || $("#procdate").val() == null) {
        //    alert('Please select processing date!!');
        //    $("#procdate").focus();
        //    return false;
        //}

        if ($("#fromdate").val() == "" || $("#fromdate").val() == null) {
            alert('Please select from date!!');
            $("#procdate").focus();
            return false;
        }

        if ($("#todate").val() == "" || $("#todate").val() == null) {
            alert('Please select to date!!');
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
        if (reptval == "Verification Report") {
            var verlevel = document.getElementById('verflevel')
            var verlevlval = verlevel.options[verlevel.selectedIndex].text;
            if (verlevlval == "Select") {
                alert('Please select verification level!!');
                $("#verflevel").focus();
                return false;
            }
        }
        if (reptval == "Item Wise Presentation Details") {
            var verlevel1 = document.getElementById('scanningTypeLevel');
            var verlevlval1 = verlevel.options[verlevel.selectedIndex].text;
            if (verlevlval == "Select") {
                alert('Please select verification level!!');
                $("#verflevel").focus();
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

        //window.open(RootUrl + 'OWReports/OWActionReport?procdate=' + $("#procdate").val() + '&clrtypr=' + $("#clrtype").val() + '&reporttype=' + $("#rpttype").val() + '&vflevel=' + $("#verflevel").val() + '&filedwnldtype=' + $("#filedwnldtype").val() + '&domainid=' + $("#gridDomains").val());
        window.open(RootUrl + 'OWReports/OWActionReport?fromdate=' + $("#fromdate").val() + '&todate=' + $("#todate").val() + '&clrtypr=' + $("#clrtype").val() + '&reporttype=' + $("#rpttype").val() + '&vflevel=' + $("#verflevel").val() + '&filedwnldtype=' + $("#filedwnldtype").val() + '&domainid=' + $("#gridDomains").val());

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