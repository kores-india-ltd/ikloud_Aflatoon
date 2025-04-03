function changePassword() {
    debugger;
    //alert('ok');
    //alert(document.getElementById("Aplhanumericmadate").value);
    //alert(document.getElementById("Specialcharmandate").value);
    var newpwd = document.getElementById('NewPassword').value;
    var valname = newpwd;
    var valpass = document.getElementById('OldPassword').value;
    var valConf = document.getElementById('ConfrmPassword').value;

    if (document.getElementById('OldPassword').value == "") {
        alert('Please enter correct old password!');
        document.getElementById('OldPassword').focus();
        document.getElementById('OldPassword').select();
        return false;
    }
    if (newpwd == "") {
        alert('Please enter correct new password!');
        document.getElementById('NewPassword').focus();
        return false;
    }
    // alert('Ok'); 
    if (document.getElementById('ConfrmPassword').value == "") {
        alert('Please enter correct confirm password!');
        document.getElementById('ConfrmPassword').focus();
        document.getElementById('ConfrmPassword').select();
        return false;
    }
   /* if (newpwd.length < document.getElementById("minpwdlength").value) {
        alert('Minimum Password length should be ' + document.getElementById("minpwdlength").value + ' character !');
        document.getElementById('NewPassword').focus();
        document.getElementById('NewPassword').select();
        return false;
    }
    if (newpwd.length > document.getElementById("maxpwdlength").value) {
        alert('Maximum Password length should no be greater than ' + document.getElementById("maxpwdlength").value + ' character !');
        document.getElementById('NewPassword').focus();
        document.getElementById('NewPassword').select();
        return false;
    }

    if ((document.getElementById("Aplhanumericmadate").value == "True") && (document.getElementById("Specialcharmandate").value == "True")) {
        
        var ret = /^.*(?=.{4,10})(?=.*\d)(?=.*[a-zA-Z]).*$/;
        
        if ((!ret.test(newpwd)) || (!tt.test(newpwd))) {
            //alert('fhjkd');
            alert('Password should contain alphanumeric and one special charater and one capital alphabet!');
            document.getElementById('NewPassword').focus();
            document.getElementById('NewPassword').select();
            return false;
        }
    }
    else if (document.getElementById("Aplhanumericmadate").value == "True") {
        
        var re = /^.*(?=.{4,10})(?=.*\d)(?=.*[a-zA-Z]).*$/;
        if (!re.test(newpwd)) {
            alert('Password should contain alphanumeric value!');
            document.getElementById('NewPassword').focus();
            document.getElementById('NewPassword').select();
            return false;
        }
    }
    if (document.getElementById('ConfrmPassword').value != document.getElementById('NewPassword').value) {
        alert('New password and confirm password is not matching!');
        document.getElementById('ConfrmPassword').focus();
        document.getElementById('ConfrmPassword').select();
        return false;
    }*/

    //-------------------------For Password Encryption-------------------
    /*var valpass = $("#OldPassword").val();
    var valname = $("#NewPassword").val();
    var valConf = $("#ConfrmPassword").val();*/
    var xyz = "";
    var PQR = "";
    var ABC = "";

    ////-------------------------Axis--------------
    ////----------------------P-------------------
    for (var i = 0; i < valpass.length; i++) {
        xyz = xyz + String.fromCharCode(valpass.charCodeAt(i) - 13);
    }
    ////----------------------N--------------
    for (var i = 0; i < valname.length; i++) {
        PQR = PQR + String.fromCharCode(valname.charCodeAt(i) - 13);
    }
    ////----------------------N--------------
    for (var i = 0; i < valConf.length; i++) {
        ABC = ABC + String.fromCharCode(valConf.charCodeAt(i) - 13);
    }

    document.getElementById('OldPassword').value = xyz;
    document.getElementById('NewPassword').value = PQR;
    document.getElementById('ConfrmPassword').value = ABC;
    //--------------End---

    return true;
}