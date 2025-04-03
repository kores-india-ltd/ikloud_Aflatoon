
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
//var scondbck = false;
function passval(array) {
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}


$(document).ready(function () {

    if (tt.length > 0) {
        document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
        document.getElementById('iwpa').focus();
        document.getElementById("btnback").disabled = true;
    }

    $("#btnok").click(function () {
       // alert('ok');
       // var AmResult = AmValid();
        //if (AmResult == false) {
        //    document.getElementById('PayeeName').focus();
        //    document.getElementById('PayeeName').select();
        //}
        //else {

            document.getElementById("btnback").disabled = false;
            var cnt = document.getElementById('cnt').value;
            var tempcnt = document.getElementById('tempcnt').value;
            var iwpa = "iwpa";
            if (backbtn == true) {

                iwpa = iwpa + backcnt
                var pn = {
                    "IWPayee": $("#iwpa").val(),
                    "ID": tt[backcnt].ID,
                };
            }

            else {
                iwpa = iwpa + cnt;
                var pn = {
                    "IWPayee": $("#iwpa").val(),
                    "ID": tt[tempcnt].ID,
                };
            }

            if (Modernizr.localstorage) {

                var localacct = window.localStorage;
                var chqamt = JSON.stringify(pn);
                localacct.setItem(iwpa, chqamt);

            }

            if (backbtn == true) {
                document.getElementById('cnt').value = parseInt(backcnt) + 1;
            }
            else {
                document.getElementById('cnt').value = parseInt(cnt) + 1;
            }

            var cnt = document.getElementById('cnt').value;
            if (cnt < tt.length) {

                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                document.getElementById('iwpa').value = "";
                document.getElementById('iwpa').focus();
                if (backbtn == true) {

                    document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
                }
                else {
                    document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
                    //lesscnt = parseInt(lesscnt) - 1;
                }
                backbtn = false;
            }
            else if (cnt > 0) {
              //  alert('Abid');
                if (Modernizr.localstorage) {
                    var listItems = [];
                    var arrlist = [];
                    var localData = window.localStorage;

                    if (scond == true) {
                        for (var i = 0; i < cnt; i++) {

                            var orderData = JSON.parse(localData.getItem("iwpa" + i));
                            arrlist.push(orderData.ID);
                            arrlist.push(orderData.IWPayee);
                        }
                    }
                    else {
                        for (var i = 1; i < cnt; i++) {

                            var orderData = JSON.parse(localData.getItem("iwpa" + i));
                            arrlist.push(orderData.ID);
                            arrlist.push(orderData.IWPayee);
                        }
                    }

                  //  ------------------------------- Calling Ajax for taking more data------------------

                    $.ajax({

                        url: '/IWPayeeDE/Index',
                        data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[cnt - 1].FrontGreyImagePath }),

                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',

                        dataType: 'json',
                        success: function (result) {
                            tt = result;
                            document.getElementById('tempcnt').value = 1;
                            document.getElementById('cnt').value = 1;


                            if (tt != null && tt != "") {
                                scond = true;
                                //-------------Saving Last data in storage---
                                var iwpa = "iwpa0"
                                var pn = {
                                    "IWPayee": tt[0].EntryAmount,
                                    "ID": tt[0].ID,
                                    "FrontGreyImagePath": tt[0].FrontGreyImagePath,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqacnt = JSON.stringify(pn);
                                    localacct.setItem(iwpa, chqacnt);

                                }
                                //----------------------------------------------------------------------//
                                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                document.getElementById('iwpa').value = "";
                                document.getElementById('iwpa').focus();
                            }
                            else {
                                alert('No Data Found!!');
                            }
                        },
                        error: function () {
                            alert("error");
                        }
                    });
                }
            }

            else {
                alert('No data found!!');
            }
            backbtn = false;
       // }

    });

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;
        //if (scondbck == true) {

        //    if (Modernizr.localstorage) {

        //        backbtn = true;
        //        var iwamnt = "iwamnt0"
        //        var localData = window.localStorage;
        //        var orderData = JSON.parse(localData.getItem(iwamnt));

        //        document.getElementById('myimg').src = tt[0].FrontGreyImagePath;

        //        document.getElementById('iwAmt').value = tt[0].EntryAmount;
        //        document.getElementById('iwAmt').focus();
        //    }
        //}
        //else {
        if (Modernizr.localstorage) {

            backbtn = true;
            var PayeeName = "PayeeName"
            var cnt = document.getElementById('cnt').value;
            PayeeName = PayeeName + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(iwamnt));

            document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;

            document.getElementById('IWPayee').value = orderData.Amount;
            document.getElementById('IWPayee').focus();
        }
        // }

    });

    $("form input").keydown(function (e) {
        //alert('Aila');
        var next_idx = $('input[type=text]').index(this) + 1;
        var tot_idx = $('body').find('input[type=text]').length;
        if (event.keyCode == 13) {
            if (tot_idx == next_idx)
                //go to the first text element if focused in the last text input element12.	
                $("input[value='Accept']").focus().click();
            //else
            //    //go to the next text input element.
                $('input[type=text]:eq(' + next_idx + ')').focus().select();
           
        }
    });
    //----------------------Validation--------------
    $("#iwpa").keydown(function (event) {

        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }

        if (event.keyCode == 110 || event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.keyCode > 95 && event.keyCode < 106) || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
            //alert('ok');
        }
        else {
            //if (event.keyCode < 95) {
            if (event.keyCode == 32 || event.keyCode < 48 || (event.keyCode > 57)) {
                event.preventDefault();
            }
        }
    });
//    $('input#iwAmt').change(function () {
//        if ($(this).val() != "") {
//            //alert('ok');
//            var num = parseFloat($(this).val());
//            var cleanNum = num.toFixed(2);
//            $(this).val(cleanNum);
//            if (num / num < 1) {
//                //alert('Please enter only 2 decimal places, we have truncated extra points');
//                //   $('#error').text('Please enter only 2 decimal places, we have truncated extra points');
//            }
//        }
//    });
});

//function AmValid() {
//    amt = document.getElementById('iwAmt').value;
//    //alert(amt);   
//    var intcont = 0;
//    for (var i = 0; i < amt.length; i++) {

//        if (amt.charAt(i) == ".") {
//            intcont++;
//        }
//        if (intcont > 1) {
//            alert('Enter valid amount!');

//            return false;
//        }
//    }

//    if (amt == "NaN") {
//        alert('Enter valid amount!');

//        return false;
//    }

//    amt1 = amt;
//    amt = amt.replace(/^0+/, '')
//    amt = amt.length;
//    if (amt1 == ".") {
//        alert('Amount field should not be dot(.) !');

//        return false;
//    }
//    else if (amt1 == "0.00") {
//        alert('Amount field should not be zero(0) !');

//        return false;
//    }
//    else if (amt < 1) {
//        alert('Amount field should not be empty !');

//        return false;
//    }
//    else if (amt > 15) {
//        alert('Amount not valid !');

//        return false;
//    }
//}
