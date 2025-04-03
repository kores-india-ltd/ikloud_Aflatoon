
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var amnt;
var cnt;
var tempcnt;
var xml = 0;
var icr = 0;
var tot = 0;
var tot1 = 0;
var fullimg;
var idslst = [];

//var scondbck = false;
function passval(array) {
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}


$(document).ready(function () {
    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].ID)
    }
    //------------ idslist end----------------//
    $('#iwAmt').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    //-------------Full Image Calling---------------

    $("#iwAmt").keydown(function (event) {
        //alert(event.keyCode);
        //alert(fullimg);
        if (event.keyCode == 113) {

            document.getElementById('AcSnipimg').style.display = "none";
            document.getElementById("myfulimg").src = fullimg;// document.getElementById("FrontGrayImgPath").value;
            document.getElementById('AcFullimg').style.display = "";
        }
        else if (event.keyCode == 115) {

            document.getElementById('AcSnipimg').style.display = "";
            document.getElementById("myfulimg").src = fullimg;// document.getElementById("FrontGrayImgPath").value;   fullimg = tt[1].FrontTiffImagePath;
            document.getElementById('AcFullimg').style.display = "none";
        }

    });
    if (document.getElementById('nodata').value != false) {
        if (tt.length > 0) {
            var gf = $("#DEbySnpt").val();
            //  alert(gf);
            if (gf == "True") {
                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[1].FrontTiffImagePath;
            document.getElementById('iwAmt').focus();
            document.getElementById("btnback").disabled = true;
        }

    }
    $("#btnok").click(function () {
        //alert('JJ');
        var AmResult = AmValid();
        if (AmResult == false) {
            document.getElementById('iwAmt').focus();
            document.getElementById('iwAmt').select();
        }
        else {

            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var iwamnt = "iwamnt";
            if (backbtn == true) {
                tot = 0;
                tot = MatchWith(backcnt);
                if (tot == 1) {
                    alert('Entered value not matching with xml value!!');
                    document.getElementById('iwAmt').focus();
                    document.getElementById('iwAmt').select();
                }
                //else if (tot == 2) {
                //    cnrslt = confirm("Entered value is not maching with XML and D1\n are you sure to accept with this value?");
                //    if (cnrslt == false) {
                //        document.getElementById('iwAmt').focus();
                //        document.getElementById('iwAmt').select();
                //    }
                //}
                else {
                    cnrslt = true;
                }

                iwamnt = iwamnt + backcnt
                amnt = {
                    "Amount": $("#iwAmt").val(),
                    "ID": tt[backcnt].ID,
                    "sttsamt": cnrslt,
                    "XMLAmount": tt[backcnt].Amount,
                };
            }

            else {
                tot1 = 0;
                tot1 = MatchWith(tempcnt);
                if (xml == 1) {
                    alert('Entered value not matching with xml value!!');
                    document.getElementById('iwAmt').focus();
                    document.getElementById('iwAmt').select();
                }
                //else if (tot1 == 2) {
                //    cnrslt = confirm("Entered value is not maching with XML and D1\n are sure to accept with this value?");
                //    if (cnrslt == false) {
                //        document.getElementById('iwAmt').focus();
                //        document.getElementById('iwAmt').select();
                //    }
                //}
                else {
                    cnrslt = true;
                }
                iwamnt = iwamnt + cnt;
                amnt = {
                    "Amount": $("#iwAmt").val(),
                    "ID": tt[tempcnt].ID,
                    "sttsamt": cnrslt,
                    "XMLAmount": tt[tempcnt].Amount,
                };
            }
            if (cnrslt == true) {
                commoncall(iwamnt);
            }
            else {
                // alert('Okk');
                document.getElementById('iwAmt').focus();
                document.getElementById("btnback").disabled = true;
            }
            //commoncall(iwamnt);
            //backbtn = false;
            $("input[value='Accept']").attr("disabled", false);
        }

    });
    //-------------------------------------Reject--------------------------------//
    $("#btnRejct").click(function () {


        document.getElementById("btnback").disabled = false;
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var iwamnt = "iwamnt";

        if ($("#iwAmt").val() == "") {
            alert('Blank value is  not allowed\n Please enter 0(zero)!!');
            $("#iwAmt").focus();
            return false;
        }
        else {
            if (backbtn == true) {

                iwamnt = iwamnt + backcnt
                amnt = {
                    "Amount": "0",
                    "ID": tt[backcnt].ID,
                    "sttsamt": false,
                    "XMLAmount": tt[backcnt].Amount,
                };
            }

            else {

                iwamnt = iwamnt + cnt;
                amnt = {
                    "Amount": "0",
                    "ID": tt[tempcnt].ID,
                    "sttsamt": false,
                    "XMLAmount": tt[tempcnt].Amount,
                };

            }
            commoncall(iwamnt);
            backbtn = false;
            $("input[value='Accept']").attr("disabled", false);
        }


    });
    //----------------------------------------Back Button-------------------------// 

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
            var iwamnt = "iwamnt"
            cnt = document.getElementById('cnt').value;
            iwamnt = iwamnt + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(iwamnt));

            if (document.getElementById('DEbySnpt').value == "True") {
                document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").src = tt[cnt - 1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").src = tt[cnt - 1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";

            }

            fullimg = tt[cnt - 1].FrontTiffImagePath;


            document.getElementById('iwAmt').value = orderData.Amount;
            document.getElementById('iwAmt').focus();
        }
        // }

    });
    $("#bbtnClose").click(function () {

        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;

            if (scond == true) {
                for (var i = 0; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwamnt" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.Amount);
                    arrlist.push(orderData.sttsamt);
                    arrlist.push(orderData.XMLAmount);
                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwamnt" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.Amount);
                    arrlist.push(orderData.sttsamt);
                    arrlist.push(orderData.XMLAmount);
                }
            }

            $.ajax({

                url: RootUrl + 'IWDataEntry/IWAmount',
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


    //---------------- Data Entry -----------------------------------

    $("form input").keydown(function (e) {
        if (e.keyCode == 13) {
            $("input[value='Accept']").attr("disabled", true);
            $("input[value='Accept']").focus().click();


        }
    });
    //-------------------------------------Rotate image-----------------------
    var value = 0
    $("#myfulimg").rotate({
        bind:
          {
              click: function () {
                  value += 180;
                  $(this).rotate({ animateTo: value })
              }
          }

    });
    //-----------------------
    //$("form input").keydown(function (e) {
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
    //        /// $("input[value='Accept']").focus().click();
    //        //                $('button[type=submit] .NavButton').click();
    //        // return true;
    //    }
    //});
    //----------------------Validation----------------------
    $("#iwAmt").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || event.charCode == 46 || (event.charCode > 47 && event.charCode < 58)) {

            var amtval = $("#iwAmt").val();
            if (amtval.length > 0) {
                var splitstr = amtval.split('.');
                // debugger;
                if (splitstr[1] != null) {
                    var strlength = splitstr[1].length;
                    if (strlength > 1) {
                        // alert('Enter only 2 digit after decimal!');
                        event.preventDefault();
                    }

                }
            }

        }
        else {
            event.preventDefault();
        }
    });

    $('input#iwAmt').change(function () {
        if ($(this).val() != "") {
            //alert('ok');
            var num = parseFloat($(this).val());
            var cleanNum = num.toFixed(2);
            $(this).val(cleanNum);
            if (num / num < 1) {
                //alert('Please enter only 2 decimal places, we have truncated extra points');
                //   $('#error').text('Please enter only 2 decimal places, we have truncated extra points');
            }
        }
    });
    //--------------------------------------
    ////-------------------------------------Rotate image-----------------------

    //$("#myfulimg").rotate({
    //    bind:
    //      {
    //          click: function () {
    //              alert('aya');
    //              value += 180;
    //              $(this).rotate({ animateTo: value })
    //          }
    //      }

    //});
    //----------------------------------Common----Call---------------------------------------//
    function commoncall(val) {

        if (Modernizr.localstorage) {

            var localacct = window.localStorage;
            var chqamt = JSON.stringify(amnt);
            localacct.setItem(val, chqamt);

        }

        if (backbtn == true) {
            document.getElementById('cnt').value = parseInt(backcnt) + 1;
        }
        else {
            document.getElementById('cnt').value = parseInt(cnt) + 1;
        }

        cnt = document.getElementById('cnt').value;
        if (cnt < tt.length) {

            if (document.getElementById('DEbySnpt').value == "True") {

                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").src = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").src = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[cnt].FrontTiffImagePath;
            document.getElementById('iwAmt').value = "";
            document.getElementById('iwAmt').focus();
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

                        var orderData = JSON.parse(localData.getItem("iwamnt" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.Amount);
                        arrlist.push(orderData.sttsamt);
                        arrlist.push(orderData.XMLAmount);
                    }
                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwamnt" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.Amount);
                        arrlist.push(orderData.sttsamt);
                        arrlist.push(orderData.XMLAmount);
                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------

                $.ajax({

                    url: RootUrl + 'IWDataEntry/IWAmount',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[cnt - 1].FrontGreyImagePath, fulimg: fullimg, idlst: idslst }),

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

                            if (tt != null && tt != "") {
                                scond = true;
                                idslst = [];
                                //-------------- idslist--------------------//
                                for (var z = 0; z < tt.length; z++) {
                                    idslst.push(tt[z].ID)
                                }

                                //------------ idslist end----------------//
                                //-------------Saving Last data in storage---
                                var iwamnt = "iwamnt0"
                                var amnt = {
                                    "Amount": tt[0].EntryAmount,
                                    "ID": tt[0].ID,
                                    "sttsamt": tt[0].sttsamt,
                                    "XMLAmount": tt[0].Amount,
                                    "FrontTiffImagePath": tt[0].FrontTiffImagePath,
                                    "FrontGreyImagePath": tt[0].FrontGreyImagePath,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqacnt = JSON.stringify(amnt);
                                    localacct.setItem(iwamnt, chqacnt);

                                }
                                //----------------------------------------------------------------------//
                                if (document.getElementById('DEbySnpt').value == "True") {
                                    document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                    document.getElementById('AcSnipimg').style.display = "";
                                    document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                                    document.getElementById('AcFullimg').style.display = "none";
                                }
                                else {
                                    document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                    document.getElementById('AcSnipimg').style.display = "none";
                                    document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                                    document.getElementById('AcFullimg').style.display = "";
                                }
                                fullimg = tt[1].FrontTiffImagePath;
                                document.getElementById('iwAmt').value = "";
                                document.getElementById('iwAmt').focus();
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


//----------------------------------------------------------------------------------------//
function AmValid() {
    amt = document.getElementById('iwAmt').value;
    //alert(amt);   
    var intcont = 0;
    for (var i = 0; i < amt.length; i++) {

        if (amt.charAt(i) == ".") {
            intcont++;
        }
        if (intcont > 1) {
            alert('Enter valid amount!');

            return false;
        }
    }

    if (amt == "NaN") {
        alert('Enter valid amount!');

        return false;
    }

    amt1 = amt;
    amt = amt.replace(/^0+/, '')
    amt = amt.length;
    if (amt1 == ".") {
        alert('Amount field should not be dot(.) !');

        return false;
    }
    else if (amt1 == "0.00") {
        alert('Amount field should not be zero(0) !');

        return false;
    }
    else if (amt1 == "0.0") {
        alert('Amount field should not be zero(0) !');

        return false;
    }
    else if (amt1 == ".0") {
        alert('Amount field should not be zero(0) !');

        return false;
    }
    else if (amt1 == ".00") {
        alert('Amount field should not be zero(0) !');

        return false;
    }
    else if (amt < 1) {
        alert('Amount field should not be empty !');

        return false;
    }
    else if (amt > 15) {
        alert('Amount not valid !');

        return false;
    }
}
//------------------------------------------//
function MatchWith(indexvl) {

    xml = 0;
    icr = 0;
    var tempdate = document.getElementById('iwAmt').value;

    if (tt[indexvl].Amount != tempdate) {
        xml = 1;
        //alert(tt[indexvl].Amount + " :" + tempdate);
    }
    //if (tt[indexvl].EntryAmount != tempdate) {
    //    icr = 1;
    //    // alert(tt[indexvl].EntryAmount + " :" + tempdate);
    //}
    // alert((icr + mnl));
    return (xml);
}

function fullImage() {
    //  alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').data);
    document.getElementById('modelmyfulimg').src = fullimg;// document.getElementById('myimg').src;
}
function ChangeImage(imagetype) {
    // alert(imagetype);
    debugger;
    var indexcnt = document.getElementById('cnt').value;
    if (imagetype == "FTiff") {

        document.getElementById('myfulimg').src = fullimg.replace(/_G/g, "F");//tt[indexcnt].FrontTiffImagePath.replace(/_G/g, "F");
    }
    else if (imagetype == "BTiff") {
        //alert('Browser not supporting!!!');
        document.getElementById('myfulimg').src = fullimg.replace(/_G/g, "_B");//tt[indexcnt].FrontTiffImagePath.replace(/_G/g, "_B");
    }
    else if (imagetype == "FGray") {

        document.getElementById('myfulimg').src = fullimg;//tt[indexcnt].FrontTiffImagePath;
    }

}
