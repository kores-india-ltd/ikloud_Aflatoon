
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var cnt;
var tempcnt;
var acnt;
var tmpflg = true;

function passval(array) {
    // alert('next');
    //alert(array);
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}

function imageconvert() {
    //debugger;
    valpass = $("#pass").val();
    valname = $("#name").val();
    var xyz = "";
    var PQR = "";

    //----------------------P-------------------
    for (var i = 0; i < valpass.length; i++) {
        xyz = xyz + String.fromCharCode(valpass.charCodeAt(i) - 13);
    }
    ////----------------------N--------------
    //for (var i = 0; i < valname.length; i++) {
    //    PQR = PQR + String.fromCharCode(valname.charCodeAt(i) - 13);
    //}

    document.getElementById('pass').value = xyz;
    //document.getElementById('name').value = PQR;
}

$(document).ready(function () {
    if (document.getElementById('nodata').value != false) {

        if (tt.length > 0) {
            document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
            document.getElementById('accnt').focus();
            document.getElementById("btnback").disabled = true;
            // alert('ok');
            // alert(tt[1].ID);
        }
    }

    $("#ok").click(function () {
  
        var dbResult = DbValid();
        if (dbResult == false) {
            document.getElementById('accnt').focus();
            document.getElementById('accnt').select();
        }
        else {
            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var accnt = "accnt";
            if (backbtn == true) {
                var tot = MatchWith(backcnt);
                if (tot == 2) {
                  //  alert('1');
                    cnrslt = confirm("Entered value is not maching with D1 and D2\n are sure to accept with this value?");
                    if (cnrslt == false) {
                        document.getElementById('accnt').focus();
                        document.getElementById('accnt').select();
                    }
                }
                else {
                    cnrslt = true;
                }
                accnt = accnt + backcnt
                acnt = {
                    "Account": $("#accnt").val(),
                    "ID": tt[backcnt].ID,
                    "stts": cnrslt,
                    "OCRDebtAccountNo": tt[backcnt].OCRDebtAccountNo,
                    "CBSDebtAccountNo": tt[backcnt].CBSDebtAccountNo,
                };
            }

            else {
                var tot1 = MatchWith(tempcnt);
                if (tot1 == 2) {
                   // alert('11');
                    cnrslt = confirm("Entered value is not maching with D1 and D2\n are sure to accept with this value?");
                    if (cnrslt == false) {
                        document.getElementById('accnt').focus();
                        document.getElementById('accnt').select();
                    }
                }
                else {
                    cnrslt = true;
                }
                accnt = accnt + cnt;
                acnt = {
                    "Account": $("#accnt").val(),
                    "ID": tt[tempcnt].ID,
                    "stts": cnrslt,
                    "OCRDebtAccountNo": tt[tempcnt].OCRDebtAccountNo,
                    "CBSDebtAccountNo": tt[tempcnt].CBSDebtAccountNo,
                };
            }
            if (cnrslt == true) {
                commondbt(accnt);
            }
            else {
                // alert('Okk');
                document.getElementById('accnt').focus();
                document.getElementById("btnback").disabled = true;
            }

            backbtn = false;
        }

    });
    //-------------------------------------Reject--------------------------------//
    $("#btnRejct").click(function () {


        document.getElementById("btnback").disabled = false;
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var accnt = "accnt";

        if (backbtn == true) {

            accnt = accnt + backcnt
            acnt = {
                "Account": "9999999999",
                "ID": tt[backcnt].ID,
                "stts": false,
                "OCRDebtAccountNo": tt[backcnt].OCRDebtAccountNo,
                "CBSDebtAccountNo": tt[backcnt].CBSDebtAccountNo,
            };
        }

        else {

            accnt = accnt + cnt;
            acnt = {
                "Account": "9999999999",
                "ID": tt[tempcnt].ID,
                "stts": false,
                "OCRDebtAccountNo": tt[tempcnt].OCRDebtAccountNo,
                "CBSDebtAccountNo": tt[tempcnt].CBSDebtAccountNo,
            };
        }
        commondbt(accnt);
        backbtn = false;
    });
    //----------------------------------------back--------------------
    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;
       
        //if (scond == true) {

        //    if (Modernizr.localstorage) {

        //        backbtn = true;
        //        var accnt = "accnt0"
        //        //var cnt = document.getElementById('cnt').value;
        //        //accnt = accnt + (parseInt(cnt) - 1)
        //        //backcnt = parseInt(cnt) - 1;
        //        var localData = window.localStorage;
        //        var orderData = JSON.parse(localData.getItem(accnt));

        //        document.getElementById('myimg').src = tt[0].FrontGreyImagePath;

        //        document.getElementById('accnt').value = tt[0].EntryDebtAccountNo;
        //        document.getElementById('accnt').focus();
        //    }
        //}
        //else {
        if (Modernizr.localstorage) {

            backbtn = true;
            var accnt = "accnt"
            var cnt = document.getElementById('cnt').value;
            //  alert(cnt);
            accnt = accnt + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
           // alert(tt[backcnt].ID);
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(accnt));

            document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
         //   alert(orderData.Account);
            document.getElementById('accnt').value = orderData.Account;
            document.getElementById('accnt').focus();
        }
        //        }

    });
    $("form input").keydown(function (e) {
        // alert('Aila');
        var next_idx = $('input[type=text]').index(this) + 1;
        var tot_idx = $('body').find('input[type=text]').length;
       // alert(tot_idx);
        if (event.keyCode == 13) {
            //alert(tot_idx+'next '+next_idx);
            //if (next_idx==1) {
            //    next_idx = next_idx + 1;
            //}
            if (tot_idx == next_idx) {
                //go to the first text element if focused in the last text input element12.	

                $("input[value='Ok']").click();
            }
            else {

                $('input[type=text]:eq(' + next_idx + ')').focus().select();
            }
            /// $("input[value='Accept']").focus().click();
            //                $('button[type=submit] .NavButton').click();
            // return true;
        }
    });
    $("#accnt").keydown(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }
        //        if (event.keyCode == 113) {
        //            Fullimg(); //Sign
        //            return false;
        //        }
        //alert('abi!!');
        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.keyCode > 47 && event.keyCode < 58)) {
            // alert('if');
        }
        else {
            // alert('else');
            //if (event.keyCode < 95) {
            //if (event.keyCode == 32 || event.keyCode < 48 || (event.keyCode > 57)) {
            event.preventDefault();
            //  }
        }
    });
});


function commondbt(val) {
    if (Modernizr.localstorage) {

        var localacct = window.localStorage;
        var chqacnt = JSON.stringify(acnt);
        localacct.setItem(val, chqacnt);

    }

    if (backbtn == true) {
        document.getElementById('cnt').value = parseInt(backcnt) + 1;
    }
    else {
        document.getElementById('cnt').value = parseInt(cnt) + 1;
    }

    cnt = document.getElementById('cnt').value;
    if (cnt < tt.length) {
       
       document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
        document.getElementById('accnt').value = "";
        document.getElementById('accnt').focus();
        if (backbtn == true) {

            document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
        }
        else {
            document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
            //lesscnt = parseInt(lesscnt) - 1;
        }
        backbtn = false;
      
        return false;
    }
    else if (cnt > 0) {
        //alert('Next');
        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;
            // var fresul = "<Table><tr><th>ID</th><th>Account</tr>"
            // listItems.push(fresul);
            if (scond == true) {
                for (var i = 0; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("accnt" + i));
                    //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.Account);
                    arrlist.push(orderData.stts);
                    arrlist.push(orderData.OCRDebtAccountNo);
                    arrlist.push(orderData.CBSDebtAccountNo);
                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("accnt" + i));
                    //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.Account);
                    arrlist.push(orderData.stts);
                    arrlist.push(orderData.OCRDebtAccountNo);
                    arrlist.push(orderData.CBSDebtAccountNo);
                }
            }

            //------------------
            //var localData1 = window.localStorage;
            //var orderData1 = JSON.parse(localData1.getItem("abid"));
            //alert(orderData1.name);
            //---------------------
            // listItems.push("<\Table>");
            //var divInsert = document.getElementById("result");
            //divInsert.innerHTML = listItems.join("");
            //------------------------------- Calling Ajax for taking more data------------------

            $.ajax({

             url: '/IWDataEntry/DebtAc',
                data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[cnt - 1].FrontGreyImagePath }),

                type: 'POST',
                contentType: 'application/json; charset=utf-8',

                dataType: 'json',
                success: function (result) {
                    if (result == false) {
                        window.location = 'DeSelection?id=1';
                    }
                    else {
                        tt = result;
                        document.getElementById('tempcnt').value = 1;
                        document.getElementById('cnt').value = 1;


                        if (tt != null && tt != "") {
                            scond = true;
                            //-------------Saving Last data in storage---
                            var accnt = "accnt0"
                            var acnt = {
                                "Account": tt[0].EntryDebtAccountNo,
                                "ID": tt[0].ID,
                                "stts": tt[0].stts,
                                "OCRDebtAccountNo": tt[0].OCRDebtAccountNo,
                                "CBSDebtAccountNo": tt[0].CBSDebtAccountNo,
                            };
                            if (Modernizr.localstorage) {

                                var localacct = window.localStorage;
                                var chqacnt = JSON.stringify(acnt);
                                localacct.setItem(accnt, chqacnt);

                            }
                            //----------------------------------------------------------------------//
                            document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                            document.getElementById('accnt').value = "";
                            document.getElementById('accnt').focus();
                        }
                        else {
                            alert('No Data Found!!');
                        }
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

}

function DbValid() {
    var Acct = document.getElementById('accnt').value;
    var acmin = document.getElementById('acmin').value;
    //Acct = Acct.replace(/^0+/, '')
    Acct = Acct.length


    if (Acct == "") {
        alert("Account no field should not be empty !");
        return false;
    }
    if (Acct < acmin) {

        alert("Minimum account no sould be " + acmin + " digits");
        return false;
    }
    Acct = document.getElementById('accnt').value.replace(/^0+/, '')
    if (Acct == "") {
        alert("Invalid Account Number!");
        return false;
    }
}

function MatchWith(indexvl) {
    var ocr = 0;
    var cbs = 0;

    if (tt[indexvl].OCRDebtAccountNo != document.getElementById('accnt').value) {
        ocr = 1;
        //alert(tt[indexvl].OCRDebtAccountNo + " :" + document.getElementById('accnt').value);
    }
    if (tt[indexvl].CBSDebtAccountNo != document.getElementById('accnt').value) {
        cbs = 1;
        // alert(tt[indexvl].CBSDebtAccountNo + " :" + document.getElementById('accnt').value);
    }
    // alert((ocr+cbs));
    return (ocr + cbs);
}

function validatedatestr() {
    
    // var i = 0;
    var abc = imageconvert();
}