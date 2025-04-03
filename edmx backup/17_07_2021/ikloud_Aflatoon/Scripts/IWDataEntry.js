var finalAccount = "";
var acc1 = "";
var acc2 = "";
var acc3 = "";
var acmaxlength = 18;
var accVal;
var cnt = 0;

$(document).ready(function () {
    console.log('Hiii');
    accVal = $("#accValidation").val();
    document.getElementById("currentCount").innerHTML = document.getElementById("deCount").value;
    document.getElementById("EntryAccountNo").focus();
    //function IsEmpty() {
    //    console.log('Hiii');
    //    if ($('#EntryAccountNo').val() === "" && $('#XMLPayeeName').val() === "" && $('#EntryChqDate').val() === "") {
    //        alert("Please fill all details.");
    //        return false;
    //    }
    //    return true;
    //};

    $("#EntryAccountNo").keyup(function (event) {
        console.log('In');
        if (accVal === 'Y') {
            //return isNumberWithDot(event, this);
            var result = isNumberWithDot(event, this);

            if (result === false) {
                return false;
            }
            else {
                var acc = $("#EntryAccountNo").val();
                console.log(acc);

                if (acc.indexOf('.') !== -1) {
                    var num = acc.match(/\./g).length;
                    if (num > 0) {
                        console.log('dot');
                        console.log(num);
                        if (num === 1) {
                            var dotIndex1 = acc.indexOf('.');
                            console.log('dotIndex1 - ' + dotIndex1);
                            console.log('currentCharacterIndex - ' + acc.length);
                            if (dotIndex1 !== 0) {
                                if ((acc.length - 1) <= dotIndex1) {
                                    var accStringNew1 = acc;
                                    console.log('accStringNew1 - ' + accStringNew1);
                                    var accString1 = accStringNew1.replace(/\./g, "");
                                    console.log('accString1 - ' + accString1);
                                    acc1 = padLeadingZeros(accString1, 4);
                                    console.log('acc1 - ' + acc1);
                                    var chkResult = checkStringWithAllZero(acc1);
                                    if (chkResult === true) {
                                        finalAccount = acc1;
                                        console.log('finalAccount - ' + finalAccount);
                                    }
                                    else {
                                        alert('Please enter non zero value.');
                                        document.getElementById("EntryAccountNo").focus();
                                        return false;
                                    }
                                    
                                }
                            }
                            else {
                                var str = $("#EntryAccountNo").val();
                                var strNew = str.replace(/\./g, "");
                                $("#EntryAccountNo").val(strNew);
                                alert('Please Enter digit');
                                return false;
                            }
                            

                        }
                        if (num === 2) {
                            var dotIndex2 = acc.indexOf('.');
                            var lastdotIndex2 = acc.lastIndexOf('.');
                            console.log('Last dotIndex - ' + lastdotIndex2);
                            if ((acc.length - 1) <= lastdotIndex2) {
                                var subAccString = acc.substring(dotIndex2);
                                console.log('subAccString - ' + subAccString);
                                var accStringNew2 = subAccString;
                                var accString2 = accStringNew2.replace(/\./g, "");
                                console.log('accString2 - ' + accString2);
                                acc2 = padLeadingZeros(accString2, 3);
                                console.log('acc2 - ' + acc2);
                                var chkResult1 = checkStringWithAllZero(acc2);
                                if (chkResult1 === true) {
                                    finalAccount = acc1 + acc2;
                                    console.log('finalAccount - ' + finalAccount);
                                }
                                else {
                                    alert('Please enter non zero value.');
                                    document.getElementById("EntryAccountNo").focus();
                                    return false;
                                }
                            }
                            else {
                                acc3 = acc.substring(acc.lastIndexOf('.') + 1);
                                console.log('acc3 - ' + acc3);
                                acc3 = padLeadingZeros(acc3, 9);
                                var chkResult2 = checkStringWithAllZero(acc3);
                                if (chkResult2 === true) {
                                    finalAccount = acc1 + acc2 + acc3;
                                    console.log('finalAccount - ' + finalAccount);
                                }
                                else {
                                    alert('Please enter non zero value.');
                                    document.getElementById("EntryAccountNo").focus();
                                    return false;
                                }
                                //finalAccount = acc1 + acc2 + acc3;
                                //console.log('finalAccount - ' + finalAccount);
                            }
                        }
                        if (num > 2) {
                            alert('You can not enter more than 2 dot.');
                            return false;
                        }
                    }
                }
            }

            
        }
        else {
            var result1 = isNumberWith(event, this);
            if (result1 === false) {
                return false;
            }
            
        }
        
        
    });

    function padLeadingZeros(num, size) {
        var s = num + "";
        while (s.length < size) s = "0" + s;
        return s;
    };

    function checkStringWithAllZero(str) {
        var number = Number(str);
        if (number > 0) {
            return true;
        }
        else {
            return false;
        }
    };

    $("#EntryAccountNo").focusout(function (event) {
        //var acc = $("#EntryAccountNo").val();
        //var acct = acc.substring(acc.lastIndexOf('.') + 1);
        //console.log('acct - ' + acct);
        //acct = padLeadingZeros(acct, 9);
        //var chkResult2 = checkStringWithAllZero(acct);
        //if (chkResult2 === true) {
        //    finalAccount = acc1 + acc2 + acct;
        //    console.log('finalAccount - ' + finalAccount);
        //}
        //else {
        //    alert('Please enter non zero value.');
        //    //document.getElementById("EntryAccountNo").focus();
        //    return false;
        //}
        var len = document.getElementById("EntryAccountNo").value.length;
        if (len !== 16) {
            if (finalAccount.length === 16) {
                document.getElementById("EntryAccountNo").value = finalAccount;
            }
            else {
                //var finAcc = padLeadingZeros(finalAccount, 16);
                //document.getElementById("EntryAccountNo").value = finAcc;
                document.getElementById("EntryAccountNo").focus();

            }
        }
        
        
        
    });

    //$('#XMLPayeeName').validate({
    //    rules: {
    //        field: {
    //            alphanumeric: true
    //        }
    //    }
    //});

    $('#EntryAccountNo,#XMLPayeeName,#EntryChqDate').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    $('#EntryChqDate').keypress(function (event) {
        //return isNumberWithDot(event, this);
        //var result = isNumber(event, this);
        //if (result === false) {
        //    console.log(result);
        //    event.preventDefault();
        //    //return false;
        //}
        //else {
        //    console.log(result);
        //    return true;
        //}

        var charCode = (event.which) ? event.which : event.keyCode;

        if (String.fromCharCode(charCode).match(/[^0-9]/g))

            return false;                        
    });

    $('#EntryAccountNo').keypress(function (event) {
        
        var charCode = (event.which) ? event.which : event.keyCode;

        if (String.fromCharCode(charCode).match(/[^0-9.]/g))

            return false;
    });

    //$('#EntryChqDate').focusout(function (event) {
    //    var dt = $("#EntryChqDate").val();
    //    var isValidResult = isDateValid(dt);

    //    if (isValidResult === false) {
    //        alert('Please enter valid date.');
    //        document.getElementById("EntryChqDate").focus();
    //    }
    //});

    function isNumberWithDot(evt, element) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        console.log(charCode);
        
        if (charCode >= 48 || charCode <= 57 || charCode == 46)
            return true;
        return false;
    }

    function isNumber(evt, element) {
        var charCode = (evt.which) ? evt.which : event.keyCode;
        console.log(charCode);
        
        if (charCode >= 48 && charCode <= 57)
            return true;
        return false;
    }

    function isDateValid(dt) {
        //console.log(dt);
        
        //console.log(yy);
        //var dd = dt.substring(0, 2);
        //console.log(dd);
        //console.log(dt);
        //var mm = dt.substring(4, 2);
        //console.log(mm);
        //console.log(dt);
        //var yy = dt.substring(10, 2);
        var newNum = dt.toString().match(/.{1,2}/g);
        
        var numberDD = Number(newNum[0]);
        var numberMM = Number(newNum[1]);
        var numberYY = Number(newNum[2]);
        console.log(numberDD + ' ' + numberMM + ' ' + numberYY);
        if (numberDD > 0 && numberDD < 32 && numberMM > 0 && numberMM < 13 && numberYY > 20) {
            return true;
        }
        else {
            return false;
        }
    };

    $("#EntryChqDate").on('keypress', function () {
        if ($(this).val().length > 5) {
            alert("Length cannot be more than 6 digits.");
            return false;
        }
    });

    $("#EntryAccountNo").on('keypress', function () {
        if ($(this).val().length > (acmaxlength)) {
            alert("Length cannot be more than " + (acmaxlength + 1) + " digits.");
            return false;
        }
    });

    $("#XMLPayeeName").on('keypress', function () {
        if ($(this).val().length > (payeemaxlength)) {
            alert("Length cannot be more than " + payeemaxlength + " characters.");
            return false;
        }
        $('#XMLPayeeName').keyup(function () {
            var $th = $(this);
            $th.val($th.val().replace(/[^a-zA-Z0-9\s]/g, function (str) { alert('You typed " ' + str + ' ".\n\nPlease use only letters and numbers.'); return ''; }));
        });
    });

    $("#btnSubmit").click(function () {
        //  alert($("#Payee").val());
        var result = AllValidations();

        if (result !== false) {
            cnt = Number(cnt) + 1;
            console.log(cnt);
            //document.getElementById("currentCount").innerHTML = cnt;
        }
        else {
            return false;
        }
        //var dt = $("#EntryChqDate").val();
        ////var isValidResult = isDateValid(dt);
        //var isValidResult = isDateValid(dt);
        //console.log(isValidResult);
        //if (isValidResult === false) {
        //    alert('Please enter valid date.');
        //    document.getElementById("EntryChqDate").focus();
        //    return false;
        //}
    });

});

function AllValidations() {
    if (!$('#EntryChqDate').val()) {
        alert('Date cannot be blank.');
        $("#EntryChqDate").focus();
        return false;
    }

    else if (!$('#EntryAccountNo').val()) {
        alert('Account No cannot be blank.');
        $("#EntryAccountNo").focus();
        return false;
    }

    else if (!$('#XMLPayeeName').val()) {
        alert('Payee Name cannot be blank.');
        $("#XMLPayeeName").focus();
        return false;
    }

    var dd, mm, yy;
    dat = document.getElementById('EntryChqDate').value;
    var chqdt = document.getElementById('EntryChqDate').value;
    if (chqdt.length <= 0 || chqdt.length < 2) {
        alert('Please enter correct Date!');
        document.getElementById('EntryChqDate').focus();
        return false;
    }
    if (dat == "") {
        //alert('aila');
        alert("Date field should not be empty !");
        document.getElementById('EntryChqDate').focus();
        document.getElementById('EntryChqDate').select();
        return false;
    }
    //else if ((dat.toUpperCase().charAt(dat.length - 1) == "X") && (IWdecn.toUpperCase() == "A")) {
    //    alert("Date is not correct");
    //    document.getElementById('IWDecision').focus();
    //    return false;
    //}
    else if (dat.length < 6) {
        alert("Date not valid !");
        document.getElementById('EntryChqDate').focus();
        document.getElementById('EntryChqDate').select();
        return false;
    }
    else {

        finldat = new String(dat);
        //alert(dat.length);
        if (dat.length == 2) {

            dd = finldat.substring(0, 2)
        }
        else if (dat.length == 4) {

            dd = finldat.substring(0, 2)
            mm = finldat.substring(2, 4)
        }
        else if (dat.length == 6) {

            dd = finldat.substring(0, 2)
            mm = finldat.substring(2, 4)
            yy = finldat.substring(4, 6)
        }

        if ($("#EntryChqDate").val() != "000000") {
            var onlydate = dd + '/' + mm + '/' + '20' + yy;


            var rtn = validatedate(onlydate);
            if (rtn == false) {
                document.getElementById('EntryChqDate').focus();
                document.getElementById('EntryChqDate').select();
                return false;
            }
        }

    }
}

var value = 0;
callrotate = function () {
    value += 180;
    $("#myimg,#ftiffimg").rotate({ animateTo: value });
}

function ChangeImage(ImagePath) {
    //alert(ImagePath);
    console.log('ImagePath');
    console.log(ImagePath);
    $.ajax({
        url: RootUrl + 'IWDataEntry/getTiffImage',
        dataType: 'html',
        data: { httpwebimgpath: ImagePath },
        success: function (Slipdata) {
            //debugger;
            $('#divtiff').html(Slipdata);
            //document.getElementById('myimg').src = Slipdata;
            document.getElementById('myimg').style.display = "none";
            document.getElementById('divtiff').style.display = "block";

        }
    });

}
function fullImage() {
    debugger;
    //alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}
function RemoveWhiteSpace() {
    var act = $("#EntryAccountNo").val().trim();
    $("#EntryAccountNo").val(act);
}
//---------------------------------------------
function validatedate(inputText) {
    // debugger;

    var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
    // Match the date format through regular expression  
    if (inputText.match(dateformat)) {
        //document.form1.text1.focus();
        //Test which seperator is used '/' or '-'  
        var opera1 = inputText.split('/');
        var opera2 = inputText.split('-');
        lopera1 = opera1.length;
        lopera2 = opera2.length;
        // Extract the string into month, date and year  
        if (lopera1 > 1) {
            var pdate = inputText.split('/');
        }
        else if (lopera2 > 1) {
            var pdate = inputText.split('-');
        }
        var dd = parseInt(pdate[0]);
        var mm = parseInt(pdate[1]);
        var yy = parseInt(pdate[2]);
        // Create list of days of a month [assume there is no leap year by default]  
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (mm == 1 || mm > 2) {
            if (dd > ListofDays[mm - 1]) {
                alert('Invalid date!');
                return false;
            }
        }
        if (mm == 2) {
            var lyear = false;
            if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                lyear = true;
            }
            if ((lyear == false) && (dd >= 29)) {
                alert('Invalid date!');
                return false;
            }
            if ((lyear == true) && (dd > 29)) {
                alert('Invalid date!');
                return false;
            }
        }
    }
    else {
        alert("Invalid date !");
        //  document.form1.text1.focus();
        return false;
    }
    //  return true;
}
//--------------------------