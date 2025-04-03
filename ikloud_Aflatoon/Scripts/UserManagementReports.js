
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

    $("#btnreport").click(function () {

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

        var repttype = document.getElementById('rpttype');
        var reptval = repttype.options[repttype.selectedIndex].text;
        if (reptval == "Select") {
            alert('Please select report type!!');
            $("#rpttype").focus();
            return false;
        }

        var filedwnldtype = document.getElementById('filedwnldtype');
        var filedwnldtypeval = filedwnldtype.options[filedwnldtype.selectedIndex].text;
        if (filedwnldtypeval == "Select") {
            alert('Please select download file type!!');
            $("#filedwnldtype").focus();
            return false;
        }

        //window.open(RootUrl + 'OWReports/OWActionReport?procdate=' + $("#procdate").val() + '&clrtypr=' + $("#clrtype").val() + '&reporttype=' + $("#rpttype").val() + '&vflevel=' + $("#verflevel").val() + '&filedwnldtype=' + $("#filedwnldtype").val() + '&domainid=' + $("#gridDomains").val());
        window.open(RootUrl + 'CreateUserNew/OWActionReport?fromdate=' + $("#fromdate").val() + '&todate=' + $("#todate").val() + '&reporttype=' + $("#rpttype").val() + '&filedwnldtype=' + $("#filedwnldtype").val());

    });
});