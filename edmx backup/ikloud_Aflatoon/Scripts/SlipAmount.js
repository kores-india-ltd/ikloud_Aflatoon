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
var Slpmt;
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
        idslst.push(tt[z].Id)
    }
    //  alert(idslst.length);
    //------------ idslist end----------------//

    //------------ idslist end----------------//
    $('#SlpAmount').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    //-------------Full Image Calling---------------

    $("#SlpAmount").keydown(function (event) {
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
            var gf = $("#DEbySnpt").val();
            //  alert(gf);
            if (gf == "True") {
                document.getElementById('myimg').data = tt[1].FrontGrayImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").data = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').data = tt[1].FrontGrayImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").data = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[1].FrontTiffImagePath;

            document.getElementById('SlpAmount').focus();
            document.getElementById("btnback").disabled = true;
            // alert('ok');
            // alert(tt[1].ID);
        }
    }

    $("#btnok").click(function () {
        //alert('ok');
        var dbResult = DbValid();
        if (dbResult == false) {
            document.getElementById('SlpAmount').focus();
            document.getElementById('SlpAmount').select();
        }
        else {
            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var Slpamt = "Slpamt";

            if (backbtn == true) {

                //var tot = MatchWith(backcnt);
                //if (tot == 2) {
                //    cnrslt = confirm("Entered value is not maching with D1 and D2\n are sure to accept with this value?");
                //    if (cnrslt == false) {
                //        document.getElementById('SlpAmount').focus();
                //        document.getElementById('SlpAmount').select();
                //    }
                //}
                //else {
                Action = "A";
                //}
                Slpamt = Slpamt + backcnt
                Slpmt = {
                    "SlipAmount": $("#SlpAmount").val(),
                    "Id": tt[backcnt].Id,
                    "Action": Action,
                    "Status": tt[backcnt].Status,
                    "RawDataId": tt[backcnt].RawDataId,
                    "CustomerId": tt[backcnt].CustomerId,
                    "DomainId": tt[backcnt].DomainId,
                    "ScanningNodeId": tt[backcnt].ScanningNodeId,
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
                Slpamt = Slpamt + cnt;
                Slpmt = {
                    "SlipAmount": $("#SlpAmount").val(),
                    "Id": tt[tempcnt].Id,
                    "Action": Action,
                    "Status": tt[tempcnt].Status,
                    "RawDataId": tt[tempcnt].RawDataId,
                    "CustomerId": tt[tempcnt].CustomerId,
                    "DomainId": tt[tempcnt].DomainId,
                    "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                };

            }
            // if (cnrslt == true) {
            // alert('aya');
            commondbt(Slpamt);
            // }
            //else {
            //    document.getElementById('SlpAmount').focus();
            //    document.getElementById("btnback").disabled = true;
            //}

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
            var Slpamt = "Slpamt"
            cnt = document.getElementById('cnt').value;
            //  alert(cnt);
            Slpamt = Slpamt + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            // alert(tt[backcnt].ID);
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(Slpamt));

            // document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
            if (document.getElementById('DEbySnpt').value == "True") {
                document.getElementById('myimg').data = tt[cnt - 1].FrontGrayImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").data = tt[cnt - 1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').data = tt[cnt - 1].FrontGrayImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").data = tt[cnt - 1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";

            }

            fullimg = tt[cnt - 1].FrontTiffImagePath;
            // alert(fullimg);
            document.getElementById('SlpAmount').value = orderData.SlipAmount;
            document.getElementById('SlpAmount').focus();
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

                    var orderData = JSON.parse(localData.getItem("Slpamt" + i));
                    //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                    arrlist.push(orderData.Id);
                    arrlist.push(orderData.SlipAmount);
                    arrlist.push(orderData.Action);
                    arrlist.push(orderData.Status);
                    arrlist.push(orderData.RawDataId);
                    arrlist.push(orderData.CustomerId);
                    arrlist.push(orderData.DomainId);
                    arrlist.push(orderData.ScanningNodeId);

                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("Slpamt" + i));
                    //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                    arrlist.push(orderData.Id);
                    arrlist.push(orderData.SlipAmount);
                    arrlist.push(orderData.Action);
                    arrlist.push(orderData.Status);
                    arrlist.push(orderData.RawDataId);
                    arrlist.push(orderData.CustomerId);
                    arrlist.push(orderData.DomainId);
                    arrlist.push(orderData.ScanningNodeId);

                }
            }


            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(pcnt);
            $.ajax({

                url: RootUrl + 'OWDataEntry/SlipAmount',
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

    $("#SlpAmount").keypress(function (event) {
        // alert(event.keyCode);
        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || event.keyCode == 46 || (event.charCode > 47 && event.charCode < 58)) {

            // alert('ok');
            var amtval = $("#SlpAmount").val();
            //   debugger;
            var intcont;
            if (amtval.length > 0) {

                for (var i = 0; i < amtval.length; i++) {

                    if (amtval.charAt(i) == ".") {
                        intcont++;
                    }
                    if (intcont > 1) {
                        event.preventDefault();
                    }
                }
                var splitstr = amtval.split('.');

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

    //-------------------------------Common For Accept and Reject----------------
    function commondbt(val) {
        //alert('cool!!');
        if (Modernizr.localstorage) {

            var localacct = window.localStorage;
            var chqacnt = JSON.stringify(Slpmt);
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
            if (document.getElementById('DEbySnpt').value == "True") {

                document.getElementById('myimg').data = tt[cnt].FrontGrayImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").data = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').data = tt[cnt].FrontGrayImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").data = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[cnt].FrontTiffImagePath;
            document.getElementById('SlpAmount').value = "";
            document.getElementById('SlpAmount').focus();

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
            debugger;
            if (Modernizr.localstorage) {
                var listItems = [];
                var arrlist = [];
                var localData = window.localStorage;

                if (scond == true) {
                    for (var i = 0; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("Slpamt" + i));
                        //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.SlipAmount);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.ScanningNodeId);
                    }
                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("Slpamt" + i));
                        //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.SlipAmount);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.ScanningNodeId);

                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;

                //debugger;

                $.ajax({

                    url: RootUrl + 'OWDataEntry/SlipAmount',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[pcnt - 1].FrontGrayImagePath, fulimg: fullimg, idlst: idslst }),

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
                                var Slpamt = "Slpamt0"
                                var Slpmt = {
                                    "SlipAmount": tt[0].SlipAmount,
                                    "Id": tt[0].Id,
                                    "Action": tt[0].Action,
                                    "Status": tt[0].Status,
                                    "RawDataId": tt[0].RawDataId,
                                    "CustomerId": tt[0].CustomerId,
                                    "DomainId": tt[0].DomainId,
                                    "ScanningNodeId": tt[0].ScanningNodeId,

                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqacnt = JSON.stringify(Slpmt);
                                    localacct.setItem(Slpamt, chqacnt);

                                }
                                //----------------------------------------------------------------------//
                                // document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                document.getElementById('myimg').removeAttribute('data');
                                document.getElementById('myfulimg').removeAttribute('data');
                                if (document.getElementById('DEbySnpt').value == "True") {
                                    document.getElementById('myimg').data = tt[1].FrontGrayImagePath;
                                    document.getElementById('AcSnipimg').style.display = "";
                                    document.getElementById("myfulimg").data = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                                    document.getElementById('AcFullimg').style.display = "none";
                                }
                                else {
                                    document.getElementById('myimg').data = tt[1].FrontGrayImagePath;
                                    document.getElementById('AcSnipimg').style.display = "none";
                                    document.getElementById("myfulimg").data = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                                    document.getElementById('AcFullimg').style.display = "";
                                }

                                fullimg = tt[1].FrontTiffImagePath;
                                document.getElementById('SlpAmount').value = "";
                                document.getElementById('SlpAmount').focus();

                                $("input[value='Accept']").attr("disabled", false);
                            }
                            else {
                                alert('No Data Found!!');
                            }
                        }

                    },
                    error: function () {
                        alert(result);
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
    //alert('chod');
    var amt = document.getElementById('SlpAmount').value;
    var intcont = 0;

    // debugger;
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
    else if (amt1 == ".0") {
        alert('Amount not valid !');

        return false;
    }
    else if (amt1 == ".00") {
        alert('Amount not valid !');

        return false;
    }
    else if (amt1 == "0.00") {
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
    else if (amt1 > 999999999) {
        var amtflg = confirm('Please verify the amount carefully. It looks you are inputting account number in amount!!\n Do you want to accept this amount?');
        if (amtflg == false) {
            return false;
        }
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