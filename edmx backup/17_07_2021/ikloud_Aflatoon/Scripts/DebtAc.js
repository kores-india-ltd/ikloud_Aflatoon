
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var userdec;
var dtte;
var cnt;
var tempcnt;
var acnt;
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

var commondbt;
$(document).ready(function () {
    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].ID)
    }
    //  alert(idslst.length);
    $('#accnt').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    //------------ idslist end----------------//

    //-------------Full Image Calling---------------

    $("#accnt").keydown(function (event) {
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
    //----------------------------------------------

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

            document.getElementById('accnt').focus();
            document.getElementById("btnback").disabled = true;
            // alert('ok');
            // alert(tt[1].ID);
        }
    }

    $("#btnok").click(function () {


        //  alert('jalebi');
        var dbResult = DbValid($("#btnok").val());
        if (dbResult == false) {
            document.getElementById('accnt').focus();
            document.getElementById('accnt').select();
            $("input[value='Accept']").attr("disabled", false);
        }
        else {
            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var accnt = "accnt";

            if (backbtn == true) {

                var tot = MatchWith(backcnt);
                if (tot == 2) {
                    cnrslt = confirm("Entered value is not maching with D1 and D2\n are sure to accept with this value?");
                    if (cnrslt == false) {
                        document.getElementById('accnt').focus();
                        document.getElementById('accnt').select();
                    }
                    else {//--ELSE-------------Added On 14-03-2017----------
                        cnrslt = true;
                        userdec = false;
                    }//---------------Added On 14-03-2017----------
                }
                else {
                    cnrslt = true;
                    userdec = true;
                }
                accnt = accnt + backcnt
                acnt = {
                    "Account": $("#accnt").val(),
                    "ID": tt[backcnt].ID,
                    "stts": userdec,
                    "OCRDebtAccountNo": tt[backcnt].OCRDebtAccountNo,
                    "CBSDebtAccountNo": tt[backcnt].CBSDebtAccountNo,
                };
            }

            else {
                var tot1 = MatchWith(tempcnt);
                if (tot1 == 2) {
                    cnrslt = confirm("Entered value is not maching with D1 and D2\n are sure to accept with this value?");
                    if (cnrslt == false) {
                        document.getElementById('accnt').focus();
                        document.getElementById('accnt').select();
                    }
                    else {//--ELSE-------------Added On 14-03-2017----------
                        cnrslt = true;
                        userdec = false;
                    }//---------------Added On 14-03-2017----------
                }
                else {
                    cnrslt = true;
                    userdec = true;
                }
                accnt = accnt + cnt;
                acnt = {
                    "Account": $("#accnt").val(),
                    "ID": tt[tempcnt].ID,
                    "stts": userdec,
                    "OCRDebtAccountNo": tt[tempcnt].OCRDebtAccountNo,
                    "CBSDebtAccountNo": tt[tempcnt].CBSDebtAccountNo,
                };

            }
            if (cnrslt == true) {
                commondbt(accnt);
            }
            else {
                document.getElementById('accnt').focus();
                document.getElementById("btnback").disabled = true;
            }

            backbtn = false;
            $("input[value='Accept']").attr("disabled", false);
        }
    });

    //-------------------------------------Reject--------------------------------//
    $("#btnRejct").click(function () {


        document.getElementById("btnback").disabled = false;
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var accnt = "accnt";
        var dbResult = DbValid($("#btnRejct").val());

        if (dbResult == false) {
            document.getElementById('accnt').focus();
            document.getElementById('accnt').select();
        }
        else {
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

                accnt = accnt + tempcnt
                acnt = {
                    "Account": "9999999999",
                    "ID": tt[tempcnt].ID,
                    "stts": false,
                    "OCRDebtAccountNo": tt[tempcnt].OCRDebtAccountNo,
                    "CBSDebtAccountNo": tt[tempcnt].CBSDebtAccountNo,
                };
                //backbtn = false;  comment on 01-06-2016
            }
            commondbt(accnt);

            $("input[value='Accept']").attr("disabled", false);
        }


    });
    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;
        //if (scondbck == true) {

        //    if (Modernizr.localstorage) {
        //        alert('iw0');
        //        backbtn = true;
        //        var iwdate = "iwdate0"
        //        var localData = window.localStorage;
        //        var orderData = JSON.parse(localData.getItem(iwdate));

        //        document.getElementById('myimg').src = tt[0].FrontGreyImagePath;

        //        document.getElementById('iwDate').value = tt[0].EntryDate;
        //        document.getElementById('iwDate').focus();
        //    }
        //}
        //else {
        if (Modernizr.localstorage) {

            backbtn = true;
            var accnt = "accnt"
            cnt = document.getElementById('cnt').value;
            //  alert(cnt);
            accnt = accnt + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            // alert(tt[backcnt].ID);
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(accnt));

            // document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
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
            // alert(fullimg);
            document.getElementById('accnt').value = orderData.Account;
            document.getElementById('accnt').focus();
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


            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(pcnt);
            $.ajax({

                url: RootUrl + 'IWDataEntry/DebtAc',
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

    //---------------- Data Entry -----------------------------------

    $("form input").keydown(function (e) {
        if (e.keyCode == 13) {
            $("input[value='Accept']").attr("disabled", true);
            $("input[value='Accept']").focus().click();


        }
    });

    $("#accnt").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
            // alert('if');
        }
        else {
            event.preventDefault();
        }
    });

    //-------------------------------Common For Accept and Reject----------------
    function commondbt(val) {
        //alert('cool!!');
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
            //alert("cnt val: "+cnt);
            //  alert(tt[cnt].FrontGreyImagePath);
            //  document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
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
            document.getElementById('accnt').value = "";
            document.getElementById('accnt').focus();

            if (backbtn == true) {
                document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
            }
            else {
                document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
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

                //------------------------------- Calling Ajax for taking more data------------------
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;

                $.ajax({

                    url: RootUrl + 'IWDataEntry/DebtAc',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[pcnt - 1].FrontGreyImagePath, fulimg: fullimg, idlst: idslst }),

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
                                //-------Remove save objects from browser---//
                                window.localStorage.clear();
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
                                // document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
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
                                document.getElementById('accnt').value = "";
                                document.getElementById('accnt').focus();

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



function DbValid(btnvalue) {
    var Acct = document.getElementById('accnt').value;
    var acmin = document.getElementById('acmin').value;
    //Acct = Acct.replace(/^0+/, '')
    debugger;
    var Accval = Acct;
    Acct = Acct.length


    if (Acct == "") {
        alert("Account no field should not be empty !");
        return false;
    }
    if (Acct < acmin) {

        alert("Minimum account no sould be " + acmin + " digits");
        return false;
    }
    //   debugger;
    if (Acct > acmin) {
        var prevval = "";
        var nextval;
        var index = 0;
        for (var i = 0; i < acmin; i++) {
            nextval = "";
            nextval = Accval.charAt(i);
            if (prevval != "") {
                if (prevval == nextval) {
                    prevval = nextval;
                    index = index + 1;
                }
                else {
                    index = 0;
                }
            }
            else {
                prevval = nextval
            }
        }
        debugger;
        if (btnvalue == "Accept") {
            if (index > 6 && Accval.substring(0, 7) != "9999999" || index > 6 && Accval.substring(0, 7) == "9999999") {
                alert("Invalid Account Number!");
                return false;
            }
        }
        else {
            if (index > 6 && Accval.substring(0, 7) != "9999999") {
                alert("Invalid Account Number!");
                return false;
            }
        }
       

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