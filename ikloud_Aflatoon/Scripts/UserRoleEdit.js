$(document).ready(function () {
    console.log('Hiii');
    //amtlimit();


});

function amtlimit() {
    console.log('In');
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

function valid() {


    if ($("#RoleName").val() == "" || $("#RoleName").val() == null) {
        alert('Please enter Role Name !');
        document.getElementById('RoleName').focus();
        return false;
    }
    if ($("#RoleName").val().length < 4) {
        alert('Please enter minimum 4 characters !');
        document.getElementById('RoleName').focus();
        return false;
    }


    //if ($('#qc').prop('checked')) {

    //    if (document.getElementById('l1frm').value == "") {
    //        alert('Please enter from amount!!');
    //        document.getElementById('l1frm').focus();
    //        return false;
    //    }
    //    else if (document.getElementById('l1to').value == "") {
    //        alert('Please enter To amount!!');
    //        document.getElementById('l1to').focus();
    //        return false;
    //    }
    //    else if ((parseInt(document.getElementById('l1frm').value) == 0) && (parseInt(document.getElementById('l1to').value) == 0)) {

    //        alert('Please enter correct amount!!');
    //        document.getElementById('l1frm').focus();
    //        return false;
    //    }
    //}
    //if ($('#vf').prop('checked')) {
    //    if (document.getElementById('l2frm').value == "") {
    //        alert('Please enter from amount!!');
    //        document.getElementById('l2frm').focus();
    //        return false;
    //    }
    //    else if (document.getElementById('l2to').value == "") {
    //        alert('Please enter To amount!!');
    //        document.getElementById('l2to').focus();
    //        return false;
    //    }
    //    else if ((parseInt(document.getElementById('l2frm').value) == 0) && (parseInt(document.getElementById('l2to').value) == 0)) {
    //        alert('Please enter correct amount!!');
    //        document.getElementById('l2frm').focus();
    //        return false;
    //    }
    //}
    //if ($('#rvf').prop('checked')) {
    //    if (document.getElementById('l3frm').value == "") {
    //        alert('Please enter from amount!!');
    //        document.getElementById('l3frm').focus();
    //        return false;
    //    }
    //    else if (document.getElementById('l3to').value == "") {
    //        alert('Please enter To amount!!');
    //        document.getElementById('l3to').focus();
    //        return false;
    //    }
    //    else if ((parseInt(document.getElementById('l3frm').value) == 0) && (parseInt(document.getElementById('l3to').value) == 0)) {
    //        alert('Please enter correct amount!!');
    //        document.getElementById('l3frm').focus();
    //        return false;
    //    }
    //}

    //if ($('#rvf4').prop('checked')) {
    //    if (document.getElementById('l4frm').value == "") {
    //        alert('Please enter from amount!!');
    //        document.getElementById('l4frm').focus();
    //        return false;
    //    }
    //    else if (document.getElementById('l4to').value == "") {
    //        alert('Please enter To amount!!');
    //        document.getElementById('l4to').focus();
    //        return false;
    //    }
    //    else if ((parseInt(document.getElementById('l4frm').value) == 0) && (parseInt(document.getElementById('l4to').value) == 0)) {
    //        alert('Please enter correct amount!!');
    //        document.getElementById('l4frm').focus();
    //        return false;
    //    }
    //}

    return true;
}