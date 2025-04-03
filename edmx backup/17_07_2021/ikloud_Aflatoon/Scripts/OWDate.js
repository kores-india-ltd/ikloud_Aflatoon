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
var Cdate;
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
    $('#chqDate').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    //-------------Full Image Calling---------------

    $("#chqDate").keydown(function (event) {
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

            document.getElementById('chqDate').focus();
            document.getElementById("btnback").disabled = true;
            // alert('ok');
            // alert(tt[1].ID);
        }
    }

    $("#btnok").click(function () {
        //   alert('ok');
        var dbResult = DbValid();
        if (dbResult == false) {
            document.getElementById('chqDate').focus();
            document.getElementById('chqDate').select();
        }
        else {
            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var Chqdate = "Chqdate";

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
                Chqdate = Chqdate + backcnt
                Cdate = {
                    "Date2": $("#chqDate").val(),
                    "Date1": tt[backcnt].Date1,
                    "Id": tt[backcnt].Id,
                    "Action": Action,
                    "Status": tt[backcnt].Status,
                    "RawDataId": tt[backcnt].RawDataId,
                    "CustomerId": tt[backcnt].CustomerId,
                    "DomainId": tt[backcnt].DomainId,
                    "ScanningNodeId": tt[backcnt].ScanningNodeId,
                    "ChequeDateSettings": tt[backcnt].ChequeDateSettings,
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
                Chqdate = Chqdate + cnt;
                Cdate = {
                    "Date2": $("#chqDate").val(),
                    "Date1": tt[tempcnt].Date1,
                    "Id": tt[tempcnt].Id,
                    "Action": Action,
                    "Status": tt[tempcnt].Status,
                    "RawDataId": tt[tempcnt].RawDataId,
                    "CustomerId": tt[tempcnt].CustomerId,
                    "DomainId": tt[tempcnt].DomainId,
                    "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                    "ChequeDateSettings": tt[tempcnt].ChequeDateSettings,
                };

            }
            // if (cnrslt == true) {
            // alert('aya');
            commondbt(Chqdate);
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
                "ID": tt[tempcnt].ID,
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
                "ID": tt[backcnt].ID,
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
                "ID": tt[tempcnt].ID,
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
            var Chqdate = "Chqdate"
            cnt = document.getElementById('cnt').value;
            //  alert(cnt);
            Chqdate = Chqdate + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            // alert(tt[backcnt].ID);
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(Chqdate));

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
            document.getElementById('chqDate').value = orderData.Date2;
            document.getElementById('chqDate').focus();
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

                    var orderData = JSON.parse(localData.getItem("Chqdate" + i));
                    //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                    arrlist.push(orderData.Id);
                    arrlist.push(orderData.Date2);
                    arrlist.push(orderData.Date1);
                    arrlist.push(orderData.Action);
                    arrlist.push(orderData.Status);
                    arrlist.push(orderData.RawDataId);
                    arrlist.push(orderData.CustomerId);
                    arrlist.push(orderData.DomainId);
                    arrlist.push(orderData.ScanningNodeId);
                    arrlist.push(orderData.ChequeDateSettings);

                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("Chqdate" + i));
                    //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                    arrlist.push(orderData.Id);
                    arrlist.push(orderData.Date2);
                    arrlist.push(orderData.Date1);
                    arrlist.push(orderData.Action);
                    arrlist.push(orderData.Status);
                    arrlist.push(orderData.RawDataId);
                    arrlist.push(orderData.CustomerId);
                    arrlist.push(orderData.DomainId);
                    arrlist.push(orderData.ScanningNodeId);
                    arrlist.push(orderData.ChequeDateSettings);

                }
            }


            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(pcnt);
            $.ajax({

                url: RootUrl + 'OWDataEntry/ChqDate',
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

    $("#chqDate").keypress(function (event) {

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
        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.charCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            // alert('else');
            //if (event.keyCode < 95) {
            //if (event.keyCode == 32 || event.keyCode < 48 || (event.keyCode > 57)) {
            event.preventDefault();
            //  }
        }
    });

    //-------------------------------Common For Accept and Reject----------------
    function commondbt(val) {
        //alert('cool!!');
        if (Modernizr.localstorage) {

            var localacct = window.localStorage;
            var chqacnt = JSON.stringify(Cdate);
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
            document.getElementById('chqDate').value = "";
            document.getElementById('chqDate').focus();

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

                        var orderData = JSON.parse(localData.getItem("Chqdate" + i));
                        //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.Date2);
                        arrlist.push(orderData.Date1);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.ChequeDateSettings);
                    }
                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("Chqdate" + i));
                        //  var row = "<tr><td>" + orderData.ID + "</td><td>" + orderData.Account + "</td></tr>"
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.Date2);
                        arrlist.push(orderData.Date1);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.ChequeDateSettings);

                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;
                //  alert("cnt: " + cnt);
                //alert(tt[(cnt - 1)].FrontGreyImagePath);

                $.ajax({

                    url: RootUrl + 'OWDataEntry/ChqDate',
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
                                var ChqAmt = "Chqdate0"
                                var Camt = {
                                    "Date2": tt[0].Date2,
                                    "Date1": tt[0].Date1,
                                    "Id": tt[0].Id,
                                    "Action": tt[0].Action,
                                    "Status": tt[0].Status,
                                    "RawDataId": tt[0].RawDataId,
                                    "CustomerId": tt[0].CustomerId,
                                    "DomainId": tt[0].DomainId,
                                    "ScanningNodeId": tt[0].ScanningNodeId,
                                    "ChequeDateSettings": tt[0].ChequeDateSettings,

                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqacnt = JSON.stringify(Camt);
                                    localacct.setItem(ChqAmt, chqacnt);

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
                                document.getElementById('chqDate').value = "";
                                document.getElementById('chqDate').focus();

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

    var dat = document.getElementById('chqDate').value;
    if (dat == "") {
        //alert('aila');
        alert("Date field should not be empty !");
        document.getElementById('chqDate').focus();
        document.getElementById('chqDate').select();
        return false;
    }
        //COMMENETD FOR UNDATED/INVALID DATE

        //else if (dat == "000000") {
        //    alert("Date not valid !");
        //    document.getElementById('chqDate').focus();
        //    document.getElementById('chqDate').select();
        //    return false;
        //}
    else if (dat == "") {
        //alert('aila');
        alert("Date field should not be empty !");

        return false;
    }
    else if (dat.length < 6) {
        alert("Date not valid !");

        return false;
    }
    else {



        // Create list of days of a month [assume there is no leap year by default]  
        //var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        var dd, mm, yy;
        var finldat = new String(dat);
        //// alert(finldat);

        dd = finldat.substring(0, 2)
        mm = finldat.substring(2, 4)
        yy = finldat.substring(4, 6)

        var onlydate = dd + '/' + mm + '/' + '20' + yy;
        if (dat != "000000") {
            var rtn = validatedate(onlydate);
            if (rtn == false) {
                return false;
            }
        }

    }
    //------------------------------------Post Date and Stale Cheques ----///

    //var stlmntdt = document.getElementById('processdate').value;
    //var sesondt = stlmntdt;//document.getElementById('processdate').value;

    //var fnldate = '20' + yy + '/' + mm + '/' + dd;
    //var staldat = new Date(sesondt);
    //var postdat = new Date(stlmntdt);
    //var d3 = new Date(fnldate);

    //var timeDiff = Math.abs(staldat.getTime() - d3.getTime());
    //var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));


    //if (postdat < d3) {
    //    alert('Post date\n Please Reject this cheque!!');
    //    return false;
    //}
    //if (diffDays > 90) {
    //    alert('Stale Cheque\\n Please Reject this cheque!!');
    //    return false;
    //}


    ///----------------------------------------------------------------------------------------//
}


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