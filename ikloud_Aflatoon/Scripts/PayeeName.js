
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var lastimgpath;
var amtwrd;
var paye;
var cnt;
var tempcnt;
var rejectresn = 0;
var flg = false;
var idslst = [];
//var scondbck = false;
function passval(array) {
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}

var selectedReason;
$(document).ready(function () {
    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].ID)
    }

    $('#PayeeName').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    if (tt.length > 0) {
        //alert(tt.length);
        document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
        document.getElementById('PayeeName').value = tt[1].XMLPayee;
        //Calling CAR To LAR-------------amtwrd
        amtwrd = number2text(tt[1].XMLAmount)
        document.getElementById('amtwrd').innerHTML = amtwrd;
        document.getElementById('PayeeName').focus();
        document.getElementById('PayeeName').select();
        document.getElementById("btnback").disabled = true;
        lastimgpath = tt[tt.length - 1].FrontGreyImagePath;
    }


    $("#btnok").click(function () {
        rejectresn = 0;

        var result = PValid()

        if (result == false) {
            document.getElementById('PayeeName').focus();
            document.getElementById('PayeeName').select();
        }
        else {

            document.getElementById("btnback").disabled = false;
            //  scondbck = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var iwPayee = "iwPayee";
            if (backbtn == true) {
                iwPayee = iwPayee + backcnt;
                paye = {
                    "Payee": $("#PayeeName").val(),
                    "ID": tt[backcnt].ID,
                    "XMLPayee": tt[backcnt].XMLPayee,
                    "XMLAmount": tt[backcnt].XMLAmount,
                    "OpsStatus": tt[backcnt].OpsStatus,
                };
            }

            else {

                iwPayee = iwPayee + cnt;
                paye = {
                    "Payee": $("#PayeeName").val(),
                    "ID": tt[tempcnt].ID,
                    "XMLPayee": tt[tempcnt].XMLPayee,
                    "XMLAmount": tt[tempcnt].XMLAmount,
                    "RejctReason": rejectresn,
                    "OpsStatus": tt[tempcnt].OpsStatus,
                };
                backbtn = false;
            }

            common(iwPayee);
            $("input[value='Accept']").attr("disabled", false);
        }
    });


    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;

        if (Modernizr.localstorage) {

            backbtn = true;

            var iwPayee = "iwPayee"
            cnt = document.getElementById('cnt').value;
            iwPayee = iwPayee + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;


            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(iwPayee));

            document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;

            document.getElementById('PayeeName').value = orderData.Payee;
            //Calling CAR To LAR-------------amtwrd
            amtwrd = number2text(orderData.XMLAmount)
            document.getElementById('amtwrd').innerHTML = amtwrd;
            document.getElementById('PayeeName').focus();
            document.getElementById('PayeeName').select();
        }
        $("input[value='Accept']").attr("disabled", false);

    });

    $("#bbtnClose").click(function () {

       
        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;

            if (scond == true) {
                for (var i = 0; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwPayee" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.Payee);
                    arrlist.push(orderData.XMLPayee);
                    arrlist.push(orderData.XMLAmount);
                    arrlist.push(orderData.RejctReason);
                    arrlist.push(orderData.OpsStatus);
                }

            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwPayee" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.Payee);
                    arrlist.push(orderData.XMLPayee);
                    arrlist.push(orderData.XMLAmount);
                    arrlist.push(orderData.RejctReason);
                    arrlist.push(orderData.OpsStatus);
                }
            }

            $.ajax({

                url: RootUrl + 'IWDataEntry/IWPayee',
                data: JSON.stringify({ lst: arrlist, snd: scond, btnClose: "Close", idlst: idslst }),

                type: 'POST',
                contentType: 'application/json; charset=utf-8',

                dataType: 'json',
                success: function (result) {
                    if (result == false) {
                        window.location = RootUrl + 'Home/IWIndex';
                    }
                }

            });

        }

    });

    //------------------------------------------------------------
    var value = 0
    $("#myimg").rotate({
        bind:
          {
              click: function () {
                  value += 180;
                  $(this).rotate({ animateTo: value })
              }
          }

    });

    //---------------- Data Entry -----------------------------------

    //$("form input").keydown(function (e) {
    //  //  alert('Aya');
    //    //alert(e.keyCode);
    //    var next_idx = $('input[type=text]').index(this) + 1;
    //    var tot_idx = $('body').find('input[type=text]').length;
    //    if (e.keyCode == 13) {
    //        if (tot_idx == next_idx) {
    //            //go to the first text element if focused in the last text input element12.	
    //            $("input[value='Accept']").attr("disabled", true);
    //            $("input[value='Accept']").focus().click();
    //        }
    //        else
    //            //go to the next text input element.
    //            $('input[type=text]:eq(' + next_idx + ')').focus().select();

    //    }
    //});
    $("form input").keydown(function (e) {
        if (e.keyCode == 13) {
            $("input[value='Accept']").attr("disabled", true);
            $("input[value='Accept']").focus().click();


        }
    });
    //----------------------Validation----------------------
    //$("#IwAmount").keypress(function (event) {

    //    if (event.shiftKey) {
    //        //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
    //        event.preventDefault();
    //        //}
    //    }

    //    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || (event.keyCode > 47 && event.keyCode < 58)) {

    //    }
    //    else {
    //        event.preventDefault();
    //    }
    //});

    $("#PayeeName").keypress(function (event) {


        if (event.shiftKey) {
            //if ((event.charCode >= 44 && event.charCode <= 59) || (event.charCode >= 91 && event.charCode >= 93) || event.charCode == 61 || event.charCode == 39) {
            event.preventDefault();
            // }
        }
        var vrpay = document.getElementById("PayeeName").value;
        // alert(vrpay.length);
        if (vrpay.length == 0) {
            if (event.charCode == 32) {
                alert('Blank space are not allowed!');
                event.preventDefault();
                return false;
            }
        }

        if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
        }
        else {
            event.preventDefault();
        }
    });

    selectedReason = function (val) {

        if (val == "Close") {
            flg = false;
            rejectresn = 0;

        }
        else {
            rejectresn = val;
            flg = true;
            //---------------------- document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var iwPayee = "iwPayee";


            if (backbtn == true) {
                iwPayee = iwPayee + backcnt;
                paye = {
                    "Payee": $("#PayeeName").val(),
                    "ID": tt[backcnt].ID,
                    "XMLPayee": tt[backcnt].XMLPayee,
                    "XMLAmount": tt[backcnt].XMLAmount,
                    "RejctReason": rejectresn,
                    "OpsStatus": tt[backcnt].OpsStatus,
                };
            }

            else {

                iwPayee = iwPayee + cnt;
                paye = {
                    "Payee": $("#PayeeName").val(),
                    "ID": tt[tempcnt].ID,
                    "XMLPayee": tt[tempcnt].XMLPayee,
                    "XMLAmount": tt[tempcnt].XMLAmount,
                    "RejctReason": rejectresn,
                    "OpsStatus": tt[tempcnt].OpsStatus,
                };

            }
            if (flg == true) {
                common(iwPayee);
            }
            else {
                document.getElementById('PayeeName').focus();
                document.getElementById("btnback").disabled = true;
                // document.getElementById('rjctrsn').style.display = 'none'
            }

            backbtn = false;
        }
        document.getElementById('rjctrsn').style.display = 'none'
    }


    function common(val) {

        if (Modernizr.localstorage) {
            var localacct = window.localStorage;
            var chqdate = JSON.stringify(paye);
            localacct.setItem(val, chqdate);
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
            document.getElementById('PayeeName').value = "";
            document.getElementById('PayeeName').value = tt[cnt].XMLPayee;
            //Calling CAR To LAR-------------amtwrd
            amtwrd = number2text(tt[cnt].XMLAmount)

            document.getElementById('amtwrd').innerHTML = amtwrd;

            document.getElementById('PayeeName').focus();
            document.getElementById('PayeeName').select();

            if (backbtn == true) {

                document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
            }
            else {
                document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
                //lesscnt = parseInt(lesscnt) - 1;
            }
            backbtn = false;

            $("input[value='Accept']").attr("disabled", false);
        }
        else if (cnt > 0) {

            if (Modernizr.localstorage) {
                var listItems = [];
                var arrlist = [];
                var localData = window.localStorage;

                if (scond == true) {
                    for (var i = 0; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwPayee" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.Payee);
                        arrlist.push(orderData.XMLPayee);
                        arrlist.push(orderData.XMLAmount);
                        arrlist.push(orderData.RejctReason);
                        arrlist.push(orderData.OpsStatus);
                    }

                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwPayee" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.Payee);
                        arrlist.push(orderData.XMLPayee);
                        arrlist.push(orderData.XMLAmount);
                        arrlist.push(orderData.RejctReason);
                        arrlist.push(orderData.OpsStatus);
                    }


                }

                //------------------------------- Calling Ajax for taking more data------------------
                // alert(cnt);
                $.ajax({

                    url: RootUrl + 'IWDataEntry/IWPayee',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: lastimgpath, idlst: idslst }),

                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',

                    dataType: 'json',
                    success: function (result) {
                        if (result == false) {
                            window.location = RootUrl + 'Home/IWIndex?id=1';
                        }
                        else {
                           
                            tt = result;

                            document.getElementById('tempcnt').value = 1;
                            document.getElementById('cnt').value = 1;
                            cnt = 1;
                            tempcnt = 1;

                            lastimgpath = "";
                            lastimgpath = tt[tt.length - 1].FrontGreyImagePath;

                            if (tt != null && tt != "") {
                                scond = true;
                                idslst = [];
                                //-------------- idslist--------------------//
                                for (var z = 0; z < tt.length; z++) {
                                    idslst.push(tt[z].ID)
                                }
                                //-------------Saving Last data in storage---
                                debugger;
                                var iwPayee = "iwPayee0"
                                var Paye = {
                                    "Payee": tt[0].EntryPayee,
                                    "ID": tt[0].ID,
                                    "XMLPayee": tt[0].XMLPayee,
                                    "XMLAmount": tt[0].XMLAmount,
                                    "RejctReason": tt[0].RejectReason,
                                    "OpsStatus": tt[0].OpsStatus,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqpaye = JSON.stringify(Paye);
                                    localacct.setItem(iwPayee, chqpaye);

                                }
                                //----------------------------------------------------------------------//
                                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                document.getElementById('PayeeName').value = "";
                                document.getElementById('PayeeName').value = tt[1].XMLPayee;
                                //Calling CAR To LAR-------------amtwrd
                                amtwrd = number2text(tt[1].XMLAmount)
                                document.getElementById('amtwrd').innerHTML = amtwrd;
                                document.getElementById('PayeeName').focus();
                                document.getElementById('PayeeName').select();
                                $("input[value='Accept']").attr("disabled", false);

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

});


function PValid() {

    var PAyee = document.getElementById('PayeeName').value;

    if (PAyee == "") {
        //alert('aila');
        alert("Payee field should not be empty !");
        return false;
    }
    if (PAyee.length < 3 && PAyee != "") {
        alert("Enter minimum 3 character for payee name !");
        return false;
    }

}
function fullImage() {
    //  alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').data);
    document.getElementById('modelmyfulimg').src = document.getElementById('myimg').src;
}
function ChangeImage(imagetype) {
    // alert(imagetype);
    debugger;
    var indexcnt = document.getElementById('cnt').value;
    if (imagetype == "FTiff") {

        document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath.replace(/_G/g, "F");
    }
    else if (imagetype == "BTiff") {
        //alert('Browser not supporting!!!');
        document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath.replace(/_G/g, "_B");
    }
    else if (imagetype == "FGray") {

        document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath;
    }

}

//function selectedReason(val)
//{
//    if (val == "Close") {
//        flg = false;
//        rejectresn = 0;
//    }
//    else {
//        rejectresn = val;
//        flg = true;
//        //---------------------- document.getElementById("btnback").disabled = false;
//        cnt = document.getElementById('cnt').value;
//        tempcnt = document.getElementById('tempcnt').value;
//        var iwPayee = "iwPayee";


//        if (backbtn == true) {
//            iwPayee = iwPayee + backcnt;
//            paye = {
//                "Payee": $("#PayeeName").val(),
//                "ID": tt[backcnt].ID,
//                "XMLPayee": tt[backcnt].XMLPayee,
//                "XMLAmount": tt[backcnt].XMLAmount,
//                "RejctReason": rejectresn,
//                "OpsStatus": tt[backcnt].OpsStatus,
//            };
//        }

//        else {

//            iwPayee = iwPayee + cnt;
//            paye = {
//                "Payee": $("#PayeeName").val(),
//                "ID": tt[tempcnt].ID,
//                "XMLPayee": tt[tempcnt].XMLPayee,
//                "XMLAmount": tt[tempcnt].XMLAmount,
//                "RejctReason": rejectresn,
//                "OpsStatus": tt[tempcnt].OpsStatus,
//            };

//        }
//        if (flg == true) {
//            common(iwPayee);
//        }
//        else {
//            document.getElementById('PayeeName').focus();
//            document.getElementById("btnback").disabled = true;
//        }

//        backbtn = false;
//    }
//    document.getElementById('rjctrsn').style.display = 'none'
//}
