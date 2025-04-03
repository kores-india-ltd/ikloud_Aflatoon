var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var Action;
var dtte;
var cnt;
var tempcnt;
var Sact;
var fullimg;
var idslst = [];
var cbsresult = false;
var strCbsClientDtls;
var strJointHolders;

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
  //  debugger;
    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].Id)
    }
    //  alert(idslst.length);
    //------------ idslist end----------------//
    $('#slpAccount').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    //-------------Full Image Calling---------------

    $("#slpAccount").keydown(function (event) {
        //alert(event.keyCode);
        //alert(fullimg);
        if (event.keyCode == 113) {

            document.getElementById('AcSnipimg').style.display = "none";
            document.getElementById("myfulimg").data = fullimg;// document.getElementById("FrontGrayImgPath").value;
            document.getElementById('AcFullimg').style.display = "";
        }
        else if (event.keyCode == 115) {

            document.getElementById('AcSnipimg').style.display = "";
            document.getElementById("myfulimg").data = fullimg;// document.getElementById("FrontGrayImgPath").value;   fullimg = tt[1].FrontTiffImagePath;
            document.getElementById('AcFullimg').style.display = "none";
        }

    });
    //----------------------------------------------

    if (document.getElementById('nodata').value != false) {

        if (tt.length > 0) {
            //debugger;
            //var gf = ;
            //  alert(gf);
            if ($("#DEbySnpt").val() == "1") {
                document.getElementById('myimg').data = tt[1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").data = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').data = tt[1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").data = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[1].FrontTiffImagePath;

            document.getElementById('slpAccount').focus();
            document.getElementById("btnback").disabled = true;
            // alert('ok');
            // alert(tt[1].ID);
        }
    }

    $("#btnok").click(function () {
        //alert('ok');
        var dbResult = DbValid();
        if (dbResult == false) {
            document.getElementById('slpAccount').focus();
            document.getElementById('slpAccount').select();
            $("input[value='Accept']").attr("disabled", false);
        }
        else {
            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var SlpAct = "SlpAct";

            if (backbtn == true) {

                Action = "A";
                SlpAct = SlpAct + backcnt
                Sact = {
                    "AccountNo2": $("#slpAccount").val(),
                    "AccountNo1": tt[backcnt].AccountNo1,
                    "Id": tt[backcnt].Id,
                    "Action": Action,
                    "Status": tt[backcnt].Status,
                    "RawDataId": tt[backcnt].RawDataId,
                    "CustomerId": tt[backcnt].CustomerId,
                    "DomainId": tt[backcnt].DomainId,
                    "ScanningNodeId": tt[backcnt].ScanningNodeId,
                    "SlipAccountNoSettings": tt[backcnt].SlipAccountNoSettings,
                    "CbsClinDtls": strCbsClientDtls,
                    "CbsjointHlds": strJointHolders,
                };
            }

            else {
                //var tot1 = MatchWith(tempcnt);
                //if (tot1 == 2) {
                //    cnrslt = confirm("Entered value is not maching with D1 and D2\n are sure to accept with this value?");
                //    if (cnrslt == false) {
                //        document.getElementById('SlpAmount').focus();
                //        document.getElementById('SlpAmount').select();
                //    }
                //}
                //else {
                Action = "A";
                //}
                SlpAct = SlpAct + cnt;
                Sact = {
                    "AccountNo2": $("#slpAccount").val(),
                    "AccountNo1": tt[tempcnt].AccountNo1,
                    "Id": tt[tempcnt].Id,
                    "Action": Action,
                    "Status": tt[tempcnt].Status,
                    "RawDataId": tt[tempcnt].RawDataId,
                    "CustomerId": tt[tempcnt].CustomerId,
                    "DomainId": tt[tempcnt].DomainId,
                    "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                    "SlipAccountNoSettings": tt[tempcnt].SlipAccountNoSettings,
                    "CbsClinDtls": strCbsClientDtls,
                    "CbsjointHlds": strJointHolders,
                };

            }
            commondbt(SlpAct);

            backbtn = false;
            $("input[value='Accept']").attr("disabled", false);
        }
    });

    //-------------------------------------Reject--------------------------------//
    $("#btnRejct").click(function () {


        document.getElementById("btnback").disabled = false;
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var Slpamt = "Slpamt";

        if (backbtn == true) {

            Slpamt = Slpamt + backcnt
            Slpmt = {
                "SlipAmount": "0.0",
                "Id": tt[backcnt].Id,
                "Action": "RJ",
                "Status": tt[backcnt].Status,
                "RawDataId": tt[backcnt].RawDataId,
                "CustomerId": tt[backcnt].CustomerId,
                "DomainId": tt[backcnt].DomainId,
                "ScanningNodeId": tt[backcnt].ScanningNodeId,
                "CbsClinDtls": strCbsClientDtls,
                "CbsjointHlds": strJointHolders,

            };
        }

        else {

            Slpamt = Slpamt + tempcnt
            Slpmt = {
                "SlipAmount": "0.0",
                "Id": tt[tempcnt].Id,
                "Action": "RJ",
                "Status": tt[tempcnt].Status,
                "RawDataId": tt[tempcnt].RawDataId,
                "CustomerId": tt[tempcnt].CustomerId,
                "DomainId": tt[tempcnt].DomainId,
                "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                "CbsClinDtls": strCbsClientDtls,
                "CbsjointHlds": strJointHolders,
            };
            //backbtn = false;  comment on 01-06-2016
        }

        commondbt(Slpamt);

        $("input[value='Accept']").attr("disabled", false);
    });
    //-------------------------------------Reject--------------------------------//
    $("#btnRefer").click(function () {


        document.getElementById("btnback").disabled = false;
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var Slpamt = "Slpamt";

        if (backbtn == true) {

            Slpamt = Slpamt + backcnt
            Slpmt = {
                "SlipAmount": "0.0",
                "Id": tt[backcnt].Id,
                "Action": "R",
                "Status": tt[backcnt].Status,
                "RawDataId": tt[backcnt].RawDataId,
                "CustomerId": tt[backcnt].CustomerId,
                "DomainId": tt[backcnt].DomainId,
                "ScanningNodeId": tt[backcnt].ScanningNodeId,
            };
        }

        else {

            Slpamt = Slpamt + tempcnt
            Slpmt = {
                "SlipAmount": "0.0",
                "Id": tt[tempcnt].Id,
                "Action": "R",
                "Status": tt[tempcnt].Status,
                "RawDataId": tt[tempcnt].RawDataId,
                "CustomerId": tt[tempcnt].CustomerId,
                "DomainId": tt[tempcnt].DomainId,
                "ScanningNodeId": tt[tempcnt].ScanningNodeId,
            };
            //backbtn = false;  comment on 01-06-2016
        }

        commondbt(Slpamt);

        $("input[value='Accept']").attr("disabled", false);
    });
    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;

        if (Modernizr.localstorage) {

            backbtn = true;
            var SlpAct = "SlpAct"
            cnt = document.getElementById('cnt').value;
            //  alert(cnt);
            SlpAct = SlpAct + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            // alert(tt[backcnt].ID);
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(SlpAct));

            // document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
            if (document.getElementById('DEbySnpt').value == "1") {
                document.getElementById('myimg').data = tt[cnt - 1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").data = tt[cnt - 1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').data = tt[cnt - 1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").data = tt[cnt - 1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";

            }

            fullimg = tt[cnt - 1].FrontTiffImagePath;
            // alert(fullimg);
            document.getElementById('slpAccount').value = orderData.AccountNo2;
            document.getElementById('slpAccount').focus();
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

                    var orderData = JSON.parse(localData.getItem("SlpAct" + i));
                    //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                    arrlist.push(orderData.Id);
                    arrlist.push(orderData.AccountNo2);
                    arrlist.push(orderData.AccountNo1);
                    arrlist.push(orderData.Action);
                    arrlist.push(orderData.Status);
                    arrlist.push(orderData.RawDataId);
                    arrlist.push(orderData.CustomerId);
                    arrlist.push(orderData.DomainId);
                    arrlist.push(orderData.ScanningNodeId);
                    arrlist.push(orderData.SlipAccountNoSettings);
                    arrlist.push(orderData.CbsClinDtls);
                    arrlist.push(orderData.CbsjointHlds);


                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("SlpAct" + i));
                    //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                    arrlist.push(orderData.Id);
                    arrlist.push(orderData.AccountNo2);
                    arrlist.push(orderData.AccountNo1);
                    arrlist.push(orderData.Action);
                    arrlist.push(orderData.Status);
                    arrlist.push(orderData.RawDataId);
                    arrlist.push(orderData.CustomerId);
                    arrlist.push(orderData.DomainId);
                    arrlist.push(orderData.ScanningNodeId);
                    arrlist.push(orderData.SlipAccountNoSettings);
                    arrlist.push(orderData.CbsClinDtls);
                    arrlist.push(orderData.CbsjointHlds);

                }
            }


            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(pcnt);
            $.ajax({

                url: RootUrl + 'OWDataEntry/SlpAccountCapture',
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

    $("#slpAccount").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }
       
        if ($("#allowAlpha").val() == "1") {
            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 96 && event.charCode < 123)) {

            }
            else {
                event.preventDefault();
            }
        }
        else {
            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

            }
            else {
                event.preventDefault();
            }
        }



    });

    //-------------------------------Common For Accept and Reject----------------
    function commondbt(val) {
        //alert('cool!!');
        if (Modernizr.localstorage) {

            var localacct = window.localStorage;
            var chqacnt = JSON.stringify(Sact);
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
            //alert('best');
            //  alert("cnt val: "+cnt);
            //  alert(tt[cnt].FrontGreyImagePath);
            //  document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
            document.getElementById('myimg').removeAttribute('data');
            document.getElementById('myfulimg').removeAttribute('data');
            if (document.getElementById('DEbySnpt').value == "1") {

                document.getElementById('myimg').data = tt[cnt].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").data = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').data = tt[cnt].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").data = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[cnt].FrontTiffImagePath;
            document.getElementById('slpAccount').value = "";
            document.getElementById('slpAccount').focus();

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
            // alert('call');
            if (Modernizr.localstorage) {
                var listItems = [];
                var arrlist = [];
                var localData = window.localStorage;

                if (scond == true) {
                    for (var i = 0; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("SlpAct" + i));
                        //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.AccountNo2);
                        arrlist.push(orderData.AccountNo1);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.SlipAccountNoSettings);
                        arrlist.push(orderData.CbsClinDtls);
                        arrlist.push(orderData.CbsjointHlds);
                    }
                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("SlpAct" + i));
                        //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.AccountNo2);
                        arrlist.push(orderData.AccountNo1);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.SlipAccountNoSettings);
                        arrlist.push(orderData.CbsClinDtls);
                        arrlist.push(orderData.CbsjointHlds);

                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;
                //alert("cnt: " + cnt);
                // alert(tt[(cnt - 1)].FrontGreyImagePath);

                $.ajax({

                    url: RootUrl + 'OWDataEntry/SlpAccountCapture',
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
                                    idslst.push(tt[z].Id)
                                }

                                //------------ idslist end----------------//
                                //-------------Saving Last data in storage---
                                var SlpAct = "SlpAct0"
                                var Sact = {
                                    "AccountNo2": tt[0].AccountNo2,
                                    "AccountNo1": tt[0].AccountNo1,
                                    "Id": tt[0].Id,
                                    "Action": tt[0].Action,
                                    "Status": tt[0].Status,
                                    "RawDataId": tt[0].RawDataId,
                                    "CustomerId": tt[0].CustomerId,
                                    "DomainId": tt[0].DomainId,
                                    "ScanningNodeId": tt[0].ScanningNodeId,
                                    "SlipAccountNoSettings": tt[0].SlipAccountNoSettings,
                                    "CbsClinDtls": tt[0].CbsClinDtls,
                                    "CbsjointHlds": tt[0].CbsjointHlds,

                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqacnt = JSON.stringify(Sact);
                                    localacct.setItem(SlpAct, chqacnt);

                                }
                                //----------------------------------------------------------------------//
                                // document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                document.getElementById('myimg').removeAttribute('data');
                                document.getElementById('myfulimg').removeAttribute('data');
                                if (document.getElementById('DEbySnpt').value == "1") {
                                    document.getElementById('myimg').data = tt[1].FrontGreyImagePath;
                                    document.getElementById('AcSnipimg').style.display = "";
                                    document.getElementById("myfulimg").data = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                                    document.getElementById('AcFullimg').style.display = "none";
                                }
                                else {
                                    document.getElementById('myimg').data = tt[1].FrontGreyImagePath;
                                    document.getElementById('AcSnipimg').style.display = "none";
                                    document.getElementById("myfulimg").data = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                                    document.getElementById('AcFullimg').style.display = "";
                                }

                                fullimg = tt[1].FrontTiffImagePath;
                                document.getElementById('slpAccount').value = "";
                                document.getElementById('slpAccount').focus();

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



function DbValid() {
    // debugger;
    var acmin = document.getElementById('acmin').value;
    var Acct = document.getElementById('slpAccount').value;
    var Accval = Acct;
    Acct = Acct.length
    //debugger;
    if (Acct == "") {
        alert("Account no field should not be empty !");
        document.getElementById('slpAccount').focus();
        return false;
    }
    if (Acct < acmin) {

        alert("Minimum account no sould be " + acmin + " digits");
        document.getElementById('slpAccount').focus();
        return false;
    }
    Acct = document.getElementById('slpAccount').value.replace(/^0+/, '')
    if (Acct == "") {
        alert("Invalid Account Number!");
        document.getElementById('slpAccount').focus();
        return false;
    }

    if (Acct.length > acmin) {
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
        if (index > 6 && Accval.substring(0, 7) != "9999999") {
            alert("Invalid Account Number!");
            document.getElementById('slpAccount').focus();
            return false;
        }

    }
    //if (Accval=="9999999999999999") {

    //}
    //
    //debugger;
    strCbsClientDtls = null;
    strJointHolders = null;
    var checklength = $("#toacntlength").val();
    if (Accval != "9999999999999999".substring(0, checklength)) {
        $.ajax({
            type: "POST",
            url: RootUrl + 'OWDataEntry/GetCBSDtls',
            dataType: 'html',
            data: { ac: $("#slpAccount").val() },
            async: false,
            success: function (accresult) {
                var obj = $.parseJSON(accresult);
                cbsresult = obj['acstatus'];
                strCbsClientDtls = obj['strCbsClientsDetls'];
                strJointHolders = obj['strJoinHldrs'];
                //cbsbtn = true;
            }
        });
        if (cbsresult == "false" || cbsresult == false) {
            alert("Invalid Account Number!");
            document.getElementById('slpAccount').focus();
            return false;
        }
    }
    else {
        strCbsClientDtls = "|F|Account does not exist";
        strJointHolders = "|F|";
    }

}



//function MatchWith(indexvl) {
//    var ocr = 0;
//    var cbs = 0;

//    if (tt[indexvl].OCRDebtAccountNo != document.getElementById('accnt').value) {
//        ocr = 1;
//        //alert(tt[indexvl].OCRDebtAccountNo + " :" + document.getElementById('accnt').value);
//    }
//    if (tt[indexvl].CBSDebtAccountNo != document.getElementById('accnt').value) {
//        cbs = 1;
//        // alert(tt[indexvl].CBSDebtAccountNo + " :" + document.getElementById('accnt').value);
//    }
//    // alert((ocr+cbs));
//    return (ocr + cbs);
//}