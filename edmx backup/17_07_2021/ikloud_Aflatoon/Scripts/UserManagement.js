$(document).ready(function () {
    //--------Using jquery--- domain selection-----
    debugger;

    //$("#rc").prop('disabled', true);
    //$("#de").prop('disabled', true);
    //$("#qc").prop('disabled', true);
    //$("#ds").prop('disabled', true);
    //$("#vf").prop('disabled', true);
    //$("#rprt").prop('disabled', true);
    //$("#fildwnd").prop('disabled', true);
    //$("#qury").prop('disabled', true);
    //$("#quryMd").prop('disabled', true);
    //$("#chirjct").prop('disabled', true);
    //$("#rvf").prop('disabled', true);
    //$("#um").prop('disabled', true);
    //$("#sod").prop('disabled', true);
    //$("#master").prop('disabled', true);
    //$("#settg").prop('disabled', true);
    //$("#archv").prop('disabled', true);
    //$("#mesgbrd").prop('disabled', true);


    $('#Org').change(function () {
        // debugger;
        if ($("#Org").val() != null && $("#Org").val() != "") {
            //  $('#ajaxCust').empty();
            if ($("#SelectedAccesslevel").val() == "ORG") {
            }
        }
    });
    //---------------------
    $('#chnagebtn').click(function () {
        debugger;
        document.getElementById('login').style.display = 'block';
        document.getElementById('user').value = "";
        document.getElementById('passd').value = "";
        //document.getElementById('accesschange').value = "1";
        //$("#chnagebtn").prop("disabled", true);
        //$("#SelectedAccesslevel").prop("disabled", false);
        //$("#SelectedAccesslevel").val() == "Select"
    });

    //---------------------
    //$('#chkcustdomns').click(function () {
    //    debugger;
    //    $(document).ready(function () {

    //        $("#getcustdom").dialog({
    //            draggable: true,
    //            height: 500,
    //            width: 600,
    //            position: { my: "Center Top", at: "center right", of: window },

    //        });


    //    });

    //});

    $("#slogin").click(function () {


        if ($("#user").val() == "" || $("#user").val() == null) {
            alert('Please enter user name !');
            $("#user").focus();
            return false;
        }
        if ($("#passd").val() == "" || $("#passd").val() == null) {
            alert('Please enter password !');
            $("#passd").focus();
            return false;
        }
        if ($("#user").val().toUpperCase() != $("#lname").val().toUpperCase()) {
            alert('username not valid!');
            $("#user").focus();
            return false;
        }

        //-----------------Encrept the Data----------------
        valpass = $("#passd").val();       
        var xyz = "";
        var PQR = "";       
        //for (var i = 0; i < valpass.length; i++) {
        //    xyz = xyz + String.fromCharCode(valpass.charCodeAt(i) - 13);
        //}
        xyz = valpass;

        $.ajax({

            url: RootUrl + 'SOD/login',
            type: "POST",
            dataType: "html",
            data: { uname: $("#user").val(), upass: xyz, procdate: $("#ProcessingDate").val(), custid: $('#CustomerId').val(), loglevel: "User" },
            success: function (data) {
                // debugger;
                if (data == "true") {
                    document.getElementById('login').style.display = 'none';
                    document.getElementById('accesschange').value = "1";
                    $("#chnagebtn").prop("disabled", true);
                    $("#SelectedAccesslevel").prop("disabled", false);
                    $("#SelectedAccesslevel").val() == "Select"
                }
                else {
                    alert('Passowrd is wrong!!');
                    return false;
                }

            }
        });
    });
    //----------------------

    $('#SelectedAccesslevel').change(function () {
        // debugger;
        if ($("#SelectedAccesslevel").val() != null && $("#SelectedAccesslevel").val() != "") {
            //  $('#ajaxCust').empty();
            if ($("#SelectedAccesslevel").val() == "ORG") {
                document.getElementById("ajaxDom").style.display = "none";
                document.getElementById("ajaxCust").style.display = "none";
                $.ajax({
                    url: RootUrl + 'CreateUser/_SelectOrgnization',
                    type: 'GET',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'html',
                    //async: false,
                    success: function (data) {
                        // $('#ajaxCust').empty();
                        $('#ajaxorg').html(data);
                        document.getElementById("ajaxorg").style.display = "block";
                        ////alert(data);
                        //$('#dialogEditUser').html(data);
                        //$('#dialogEditUser').dialog('open');
                    }
                });
            }
            else if ($("#SelectedAccesslevel").val() == "CUST") {
                document.getElementById("ajaxorg").style.display = "none";
                document.getElementById("ajaxDom").style.display = "none";
                $.ajax({
                    url: RootUrl + 'CreateUser/_SelectCustomer',
                    type: 'GET',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'html',
                    //async: false,
                    success: function (data) {
                        // $('#ajaxCust').empty();
                        $('#ajaxCust').html(data);
                        document.getElementById("ajaxCust").style.display = "block";
                        ////alert(data);
                        //$('#dialogEditUser').html(data);
                        //$('#dialogEditUser').dialog('open');
                    }
                });
            }
            else if ($("#SelectedAccesslevel").val() == "DOM") {
                document.getElementById("ajaxorg").style.display = "none";
                document.getElementById("ajaxCust").style.display = "none";
                $.ajax({
                    url: RootUrl + 'CreateUser/_SelectDomains',
                    type: 'GET',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'html',
                    //async: false,
                    success: function (data) {

                        $('#ajaxDom').html(data);
                        document.getElementById("ajaxDom").style.display = "block";

                    }
                });
            }
        }
        //alert($("#CustBag").val());

    });
    //------------------------------

    $('#usertype').change(function () {

        if ($(this).val() == "BPO_User") {


            //$("#rc").prop('checked', true);
            $("#rc").prop("disabled", false);
            $("#de").prop('checked', true);
            $("#de").prop("disabled", false);
            $("#qc").prop('checked', true);
            $("#rc").prop("disabled", false);
            $("#ds").prop('checked', false);
            $("#vf").prop('checked', false);
            $("#vf").prop("disabled", true);
            $("#rprt").prop('checked', false);
            $("#fildwnd").prop('checked', false);
            $("#qury").prop('checked', false);
            $("#quryMd").prop('checked', false);
            //$("#chirjct").prop('checked', false);
            $("#chirjct").prop("disabled", true);
            $("#rvf").prop('checked', false);
            $("#rvf").prop("disabled", true);
            $("#um").prop("disabled", false);
            $("#sod").prop("disabled", false);
            $("#master").prop("disabled", false);
            $("#settg").prop("disabled", false);
            $("#archv").prop("disabled", false);
            $("#mesgbrd").prop("disabled", false);
            $("#qc").prop("disabled", false);
            $("#ds").prop('disabled', false);


            $("#um").prop('checked', false);
            $("#sod").prop('checked', false);
            $("#master").prop('checked', false);
            $("#settg").prop('checked', false);
            $("#archv").prop('checked', false);
            $("#mesgbrd").prop('checked', false);
            $("#l1").show();
            $("#l2").hide();
            $("#l3").hide();
            $("#l4").hide();
        }

        else if ($(this).val() == "Scanning_User") {
            $("#rc").prop('checked', false);
            $("#de").prop('checked', false);
            $("#qc").prop('checked', false);
            $("#ds").prop('checked', true);
            $("#vf").prop('checked', false);
            $("#rprt").prop('checked', true);
            $("#fildwnd").prop('checked', false);
            $("#qury").prop('checked', true);
            $("#quryMd").prop('checked', false);
            $("#chirjct").prop('checked', false);
            $("#rvf").prop('checked', false);
            $("#um").prop('checked', false);
            $("#sod").prop('checked', false);
            $("#master").prop('checked', false);
            $("#settg").prop('checked', false);
            $("#archv").prop('checked', false);
            $("#mesgbrd").prop('checked', false);
        }
        else if ($(this).val() == "Bank_User") {
            //
            $("#ds").prop('disabled', false);
            $("#vf").prop('disabled', false);
            $("#rprt").prop('disabled', false);
            $("#fildwnd").prop('disabled', false);
            $("#qury").prop('disabled', false);
            $("#quryMd").prop('disabled', false);
            $("#chirjct").prop('disabled', false);
            $("#rvf").prop('disabled', false);
            $("#um").prop('disabled', false);
            $("#sod").prop('disabled', false);
            $("#master").prop('disabled', false);
            $("#settg").prop('disabled', false);
            $("#archv").prop('disabled', false);
            $("#mesgbrd").prop('disabled', false);
            //$("#rc").prop('checked', false);
            $("#rc").prop("disabled", true);
            // $("#de").prop('checked', false);
            $("#de").prop("disabled", true);
            $("#qc").prop('checked', false);
            $("#qc").prop('disabled', true);
            $("#ds").prop('checked', true);
            $("#vf").prop('checked', true);
            $("#rprt").prop('checked', true);
            $("#fildwnd").prop('checked', false);
            $("#qury").prop('checked', true);
            $("#quryMd").prop('checked', false);
            $("#chirjct").prop('checked', false);
            $("#rvf").prop('checked', false);
            $("#um").prop('checked', false);
            $("#sod").prop('checked', false);
            $("#master").prop('checked', false);
            $("#settg").prop('checked', false);
            $("#archv").prop('checked', false);
            $("#mesgbrd").prop('checked', false);
            $("#l2").show();
            $("#l1").hide();
            $("#l3").hide();
            $("#l4").hide();
        }
        else if ($(this).val() == "Admin_User") {
            $("#rc").prop('checked', false);
            $("#de").prop('checked', false);
            $("#qc").prop('checked', false);
            $("#ds").prop('checked', true);
            $("#vf").prop('checked', false);
            $("#rprt").prop('checked', false);
            $("#fildwnd").prop('checked', false);
            $("#qury").prop('checked', true);
            $("#quryMd").prop('checked', false);
            $("#chirjct").prop('checked', false);
            $("#rvf").prop('checked', false);
            $("#um").prop('checked', true);
            $("#sod").prop('checked', true);
            $("#master").prop('checked', true);
            $("#settg").prop('checked', true);
            $("#archv").prop('checked', true);
            $("#mesgbrd").prop('checked', true);
        }
        else if ($(this).val() == "Developer_User") {
            $("#rc").prop('disabled', false);
            $("#de").prop('disabled', false);
            $("#qc").prop('disabled', false);
            $("#ds").prop('disabled', false);
            $("#vf").prop('disabled', false);
            $("#rprt").prop('disabled', false);
            $("#fildwnd").prop('disabled', false);
            $("#qury").prop('disabled', false);
            $("#quryMd").prop('disabled', false);
            $("#chirjct").prop('disabled', false);
            $("#rvf").prop('disabled', false);
            $("#um").prop('disabled', false);
            $("#sod").prop('disabled', false);
            $("#master").prop('disabled', false);
            $("#settg").prop('disabled', false);
            $("#archv").prop('disabled', false);
            $("#mesgbrd").prop('disabled', false);

            //-----------------------------------------
            $("#rc").prop('checked', true);
            $("#de").prop('checked', true);
            $("#qc").prop('checked', true);
            $("#ds").prop('checked', true);
            $("#vf").prop('checked', true);
            $("#rprt").prop('checked', true);
            $("#fildwnd").prop('checked', true);
            $("#qury").prop('checked', true);
            $("#quryMd").prop('checked', true);
            $("#chirjct").prop('checked', true);
            $("#rvf").prop('checked', true);
            $("#um").prop('checked', true);
            $("#sod").prop('checked', true);
            $("#master").prop('checked', true);
            $("#settg").prop('checked', true);
            $("#archv").prop('checked', true);
            $("#mesgbrd").prop('checked', true);
            $("#l1").show();
            $("#l2").show();
            $("#l3").show();
            $("#l4").show();
        }
    });


});
$(function () {
    $("#datepicker").datepicker({
        dateFormat: 'mm/dd/yy',
        changeMonth: true,
        changeYear: true,
        yearRange: '-100y:c+nn',
        maxDate: '-1d'
    });
});

//-----
function amtlimit() {
    //------------QC----Checked-----
    if ($('#qc').prop('checked')) {
        $("#l1").show();
    } else {
        $("#l1").hide();
    }

    if ($('#vf').prop('checked')) {
        $("#l2").show();
    } else {
        $("#l2").hide();
    }

    if ($('#rvf').prop('checked')) {
        $("#l3").show();
    } else {
        $("#l3").hide();
    }
    if ($('#rvf4').prop('checked')) {
        $("#l4").show();
    } else {
        $("#l4").hide();
    }
}

function getBranches(radio_val) {
    var radio = document.getElementById(radio_val).value;

    if (radio == "All") {
        document.getElementById('chkdomain').style.display = 'none';
    }
    else {
        document.getElementById('chkdomain').style.display = 'block';
        //document.getElementById('chkdomain').style.width = '20%';
        //document.getElementById('chkdomain').style.height = '10%';
    }
}

function show() {

    if (document.getElementById("vf").checked == true) {
        document.getElementById('br').style.display = 'block';
    }
    else {
        document.getElementById('br').style.display = 'none';
    }
}
//-------------Domain Selection------------
function openadmin() {

    if (document.getElementById("chkadmin").checked == true) {
        document.getElementById('rowadmin').style.display = 'block';
    }
    else {
        document.getElementById('rowadmin').style.display = 'none';
    }
}

function valid() {
    // alert($('#Dom').val()); 
    debugger;

    var e1 = document.getElementById('usertype')
    var strUser1 = e1.options[e1.selectedIndex].text;
    if (strUser1 == "Select") {
        alert('Please select user type !');
        document.getElementById('usertype').focus();
        return false;
    }
    debugger;
    if ($("#accesschange").val() == "1" || ($("#accesschange").val() == "0" && $("#SelectedAccesslevel").val() == "")) {
        if ($("#SelectedAccesslevel").val() == "Select" || $("#SelectedAccesslevel").val() == "") {
            alert('Please select user accesslevel !');
            document.getElementById('SelectedAccesslevel').focus();
            return false;
        }
    }

    if ($("#LoginID").val() == "" || $("#LoginID").val() == null) {
        alert('Please enter LoginId !');
        document.getElementById('LoginID').focus();
        return false;
    }


    if (document.getElementById("title").value == "select" || document.getElementById("title").value == "") {
        alert('Please select title');
        document.getElementById("title").focus();
        return false;
    }
    if ($("#FirstName").val() == "" || $("#FirstName").val() == null) {
        alert('Please enter First Name !');
        document.getElementById('FirstName').focus();
        return false;
    }
    if ($("#FirstName").val().length < 4) {
        alert('Please enter minimum 4 characters !');
        document.getElementById('FirstName').focus();
        return false;
    }
    if ($("#LastName").val() == "" || $("#LastName").val() == null) {
        alert('Please enter Last Name !');
        document.getElementById('LastName').focus();
        return false;
    }
    if ($("#LastName").val().length < 4) {
        alert('Please enter minimum 4 characters !');
        document.getElementById('LastName').focus();
        return false;
    }
    //
    //if (document.getElementById('datepicker').value == "") {
    //    alert('Please select date of birth');
    //    document.getElementById("datepicker").focus();
    //    return false;
    //}
    var c = document.getElementById('city')
    var strcity = c.options[c.selectedIndex].text;
    if (strcity == "Select") {
        alert('Please select Location !');
        document.getElementById('city').focus();

        return false;
    }

    var e = document.getElementById('policynm')
    var strUser = e.options[e.selectedIndex].text;
    if (strUser == "Select") {
        alert('Please select policy type !');
        document.getElementById('policynm').focus();

        return false;
    }
    if ($('#qc').prop('checked')) {
        // alert(parseInt(document.getElementById('l1frm').value));
        // alert(parseInt(document.getElementById('l1to').value));
        if (document.getElementById('l1frm').value == "") {
            alert('Please enter from amount!!');
            document.getElementById('l1frm').focus();
            return false;
        }
        else if (document.getElementById('l1to').value == "") {
            alert('Please enter To amount!!');
            document.getElementById('l1to').focus();
            return false;
        }
        else if ((parseInt(document.getElementById('l1frm').value) == 0) && (parseInt(document.getElementById('l1to').value) == 0)) {

            alert('Please enter correct amount!!');
            document.getElementById('l1frm').focus();
            return false;
        }
    }
    if ($('#vf').prop('checked')) {
        if (document.getElementById('l2frm').value == "") {
            alert('Please enter from amount!!');
            document.getElementById('l2frm').focus();
            return false;
        }
        else if (document.getElementById('l2to').value == "") {
            alert('Please enter To amount!!');
            document.getElementById('l2to').focus();
            return false;
        }
        else if ((parseInt(document.getElementById('l2frm').value) == 0) && (parseInt(document.getElementById('l2to').value) == 0)) {
            alert('Please enter correct amount!!');
            document.getElementById('l2frm').focus();
            return false;
        }
    }
    if ($('#rvf').prop('checked')) {
        if (document.getElementById('l3frm').value == "") {
            alert('Please enter from amount!!');
            document.getElementById('l3frm').focus();
            return false;
        }
        else if (document.getElementById('l3to').value == "") {
            alert('Please enter To amount!!');
            document.getElementById('l3to').focus();
            return false;
        }
        else if ((parseInt(document.getElementById('l3frm').value) == 0) && (parseInt(document.getElementById('l3to').value) == 0)) {
            alert('Please enter correct amount!!');
            document.getElementById('l3frm').focus();
            return false;
        }
    }

    if ($('#rvf4').prop('checked')) {
        if (document.getElementById('l4frm').value == "") {
            alert('Please enter from amount!!');
            document.getElementById('l4frm').focus();
            return false;
        }
        else if (document.getElementById('l4to').value == "") {
            alert('Please enter To amount!!');
            document.getElementById('l4to').focus();
            return false;
        }
        else if ((parseInt(document.getElementById('l4frm').value) == 0) && (parseInt(document.getElementById('l4to').value) == 0)) {
            alert('Please enter correct amount!!');
            document.getElementById('l4frm').focus();
            return false;
        }
    }
    //  debugger;
    if ($("#accesschange").val() == "1" || ($("#accesschange").val() == "0" && $("#SelectedAccesslevel").val() == "")) {
        //-----------------------------------------Orgnization user-----------------------
        if ($('#SelectedAccesslevel').val() == "ORG") {
            if ($('#Org').val() == null || $('#Org').val() == "Select Orgnization") {
                alert('Please select orgnization!!');
                $('#Org').focus();
                return false;
            }


        }
            //-----------------------------------------Customer user-----------------------
        else if ($('#SelectedAccesslevel').val() == "CUST") {
            if ($('#CustBag').val() == null || $('#CustBag').val() == "Select Customer") {
                alert('Please select customer!!');
                $('#CustBag').focus();
                return false;
            }

        }
            //-----------------------------------------Customer user-----------------------
        else if ($('#SelectedAccesslevel').val() == "DOM") {
            if ($('#Dom').val() == null || $('#Dom').val() == "Select Domains") {
                alert('Please select Domains!!');
                $('#Dom').focus();
                return false;
            }
        }
    }
    return true;
}
//-----------------Dialog box----------------
function policydetails() {
    var e = document.getElementById('policynm')

    var strUser = e.options[e.selectedIndex].text;

    if (strUser == "Select") {
        alert('Please select policy type !');
        document.getElementById('policynm').focus();
        return false;
    }
    $(document).ready(function () {
        $("#dialogEditUser").dialog({
            modal: true,
            draggable: true,
            resizable: true,
            position: ['center', 'top'],
            show: 'blind',
            hide: 'blind',
            width: 400,
            dialogClass: 'ui-dialog-osx',
            buttons: {
                "Click here for close": function () {
                    $(this).dialog("close");
                }
            }
        });

        $.ajax({
            url: '/CreateUser/PolicyDetails',
            dataType: 'html',
            data: { name: $("#policynm").val() },
            success: function (data) {
                //alert(data);
                $('#dialogEditUser').html(data);
                $('#dialogEditUser').dialog('open');
            }
        });
    });
}
//------------------For getting customers and domains-----List---
function getcustdomainslist() {
    $(document).ready(function () {

        //$("#getcustdom").dialog({
        //    modal: true,
        //    draggable: true,
        //    resizable: true,
        //    position: ['center', 'top'],
        //    show: 'blind',
        //    hide: 'blind',
        //    width: 400,
        //    dialogClass: 'ui-dialog-osx',
        //    buttons: {
        //        "Click here for close": function () {
        //            $(this).dialog("close");
        //        }
        //    }
        //});
        $("#getcustdom").dialog({
            draggable: true,
            height: 500,
            width: 600,
            position: ['center', 'top'],

        });

        $.ajax({
            url: RootUrl + 'CreateUser/_getcustdomains',
            type: "POST",
            // contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: { userlevel: $("#SelectedAccesslevel").val(), uid: $("#ID").val() },
            success: function (retndata) {
                debugger;
                $('#getcustdom').html(retndata);
                $('#getcustdom').dialog('open');
            }

        });
    });


}