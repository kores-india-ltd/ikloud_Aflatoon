﻿
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var nextcall;
var L1;
var cnt;
var tempcnt;
var chq;
var srtcd;
var sancd;
var trcd;
var next_idx = 0;
var tot_idx = 0;
var cbsbtn = false;
var btnvalacpt = false;
var idslst = [];
//var scondbck = false;
function passval(array) {
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}

var getreason;
var getcbsdtls;

$(document).ready(function () {

    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].ID)
    }
    //  alert(idslst.length);
    //------------ idslist end----------------//
    $('#accnt,#iwAmt,#ChqDate,#ChqnoQC,#SortQC,#SANQC,#TransQC').bind("cut paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    //-----------------ShortCut----for CBS---
    $("#accnt").keydown(function (event) {
        //   alert('aila');
        if (event.keyCode == 123) {
            getcbsdtls(); //CbsDetails
            return false;
        }
    });
    if (tt.length > 0) {
        debugger;
        // alert('Aila');document.getElementById("p2").style.color = "blue";

        document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

        document.getElementById('ChqnoQC').value = tt[1].EntrySerialNo;
        document.getElementById('SortQC').value = tt[1].EntryPayorBankRoutNo;
        document.getElementById('SANQC').value = tt[1].EntrySAN;
        document.getElementById('TransQC').value = tt[1].EntryTransCode;
        document.getElementById('xmlamt').innerHTML = addCommas(tt[1].XMLAmount); //tt[1].XMLAmount; tt[1].XMLAmount;
        //document.getElementById('h2amt').value = "";
        document.getElementById('accnt').focus();
        document.getElementById("btnback").disabled = true

        $.ajax({
            //url: RootUrl +'/IWL1/GetCBSDtls',
            url: '/IWL1/GetCBSDtls',
            dataType: 'html',
            type: 'POST',
            data: { ac: tt[1].EntrySAN },
            success: function (data) {
                $('#cbsdetails').html(data);
                cbsbtn = true;
            }
        });
        //---------------Setting focus to textbox----------------//

        //document.getElementById('iwDateQc').focus();
        //document.getElementById("btnback").disabled = true;
    }

    $("#ok").click(function () {
        alert("ok");
        debugger;
        nextcall = false;
        var result = IWLICQC()
        if (result == false) {
            if (chq == true) {
                document.getElementById('ChqnoQC').focus();
                document.getElementById('ChqnoQC').select();
            }
            else if (srtcd == true) {
                document.getElementById('SortQC').focus();
                document.getElementById('SortQC').select();
            }
            else if (sancd == true) {
                document.getElementById('SANQC').focus();
                document.getElementById('SANQC').select();
            }
            else if (trcd == true) {
                document.getElementById('TransQC').focus();
                document.getElementById('TransQC').select();
            }
            return false;
        }
        else {
            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var iwL1 = "iwL1";
            var btnval = document.getElementById('IWDecision').value.toUpperCase();

            if (backbtn == true) {
                if (btnval == "R") {
                    cnrslt = false;
                    //-----valid Function for validation---------------
                    var rslt = valid(document.getElementById("rejectreasondescrpsn").value, document.getElementById('IWRemark').value);
                    if (rslt == false) {
                        //alert('Please select reject reason!!');
                        document.getElementById('rejectreasondescrpsn').focus();
                        nextcall = false;
                        return false;
                    }
                    else {
                        nextcall = true;
                    }
                }
                else {

                    var tot = MatchWith(backcnt);
                    var totamt = MatchAmt(backcnt);
                    if (tot == 0) {
                        cnrslt = confirm("Entered value of MICR is not maching with XML MICR\n are sure to accept with this value?");
                        if (cnrslt == false) {
                            document.getElementById('ChqnoQC').focus();
                            document.getElementById('ChqnoQC').select();
                        }
                        else {
                            nextcall = true;
                        }
                    }
                    else if (totamt == 0) {
                        cnrslt = confirm("Entered amount is not maching with XML amount\n are sure to accept with this value?");
                        if (cnrslt == false) {
                            document.getElementById('iwAmt').focus();
                            document.getElementById('iwAmt').select();
                        }
                        else {
                            nextcall = true;
                        }
                    }
                    else {
                        cnrslt = true;
                        nextcall = true;
                    }
                }


                iwL1 = iwL1 + backcnt
                L1 = {
                    "DbtAccountNo": $("#accnt").val(),
                    "ActualAmount": $("#iwAmt").val(),
                    "Date": $("#ChqDate").val(),
                    "EntrySerialNo": $("#ChqnoQC").val(),
                    "EntryPayorBankRoutNo": $("#SortQC").val(),
                    "EntrySAN": $("#SANQC").val(),
                    "EntryTransCode": $("#TransQC").val(),
                    "ID": tt[backcnt].ID,
                    "sttsdtqc": cnrslt,
                    "XMLAmount": tt[backcnt].XMLAmount,
                    "XMLSerialNo": tt[backcnt].XMLSerialNo,
                    "XMLPayorBankRoutNo": tt[backcnt].XMLPayorBankRoutNo,
                    "XMLSAN": tt[backcnt].XMLSAN,
                    "XMLTransCode": tt[backcnt].XMLTransCode,
                    "RejectReason": $("#IWRemark").val(),
                    "CBSClientAccountDtls": $("#cbsdls").val(),
                    "CBSJointHoldersName": $("#JoinHldrs").val(),
                };
            }

            else {
                if (btnval == "R") {
                    cnrslt = false;
                    //-----valid Function for validation---------------
                    var rslt = valid(document.getElementById("rejectreasondescrpsn").value, document.getElementById('IWRemark').value);
                    if (rslt == false) {
                        //alert('Please select reject reason!!');
                        document.getElementById('rejectreasondescrpsn').focus();
                        //  document.getElementById('RejectReason').style.display = 'block';
                        nextcall = false;
                        return false;
                    }
                    else {
                        nextcall = true;
                    }
                }
                else {

                    var tot1 = MatchWith(tempcnt);
                    var totamt1 = MatchAmt(tempcnt);
                    if (tot1 == 0) {
                        cnrslt = confirm("Entered MICR value is not maching with XML MICR\n are sure to accept with this value?");
                        if (cnrslt == false) {
                            document.getElementById('ChqnoQC').focus();
                            document.getElementById('ChqnoQC').select();
                        }
                        else {
                            nextcall = true;
                        }
                    }
                    else if (totamt1 == 0) {
                        cnrslt = confirm("Entered amount is not maching with XML amount\n are sure to accept with this value?");
                        if (cnrslt == false) {
                            document.getElementById('iwAmt').focus();
                            document.getElementById('iwAmt').select();
                        }
                        else {
                            nextcall = true;
                        }
                    }
                    else {
                        cnrslt = true;
                        nextcall = true;
                    }
                }
                iwL1 = iwL1 + cnt;
                L1 = {
                    "DbtAccountNo": $("#accnt").val(),
                    "ActualAmount": $("#iwAmt").val(),
                    "Date": $("#ChqDate").val(),
                    "EntrySerialNo": $("#ChqnoQC").val(),
                    "EntryPayorBankRoutNo": $("#SortQC").val(),
                    "EntrySAN": $("#SANQC").val(),
                    "EntryTransCode": $("#TransQC").val(),
                    "ID": tt[tempcnt].ID,
                    "sttsdtqc": cnrslt,
                    "XMLAmount": tt[tempcnt].XMLAmount,
                    "XMLSerialNo": tt[tempcnt].XMLSerialNo,
                    "XMLPayorBankRoutNo": tt[tempcnt].XMLPayorBankRoutNo,
                    "XMLSAN": tt[tempcnt].XMLSAN,
                    "XMLTransCode": tt[tempcnt].XMLTransCode,
                    "RejectReason": $("#IWRemark").val(),
                    "CBSClientAccountDtls": $("#cbsdls").val(),
                    "CBSJointHoldersName": $("#JoinHldrs").val(),
                };


            }

            if (nextcall == true) {
                //alert($("#IWRemark").val());
                common(iwL1);
            }
            else {
                // alert('Okk');
                document.getElementById('accnt').focus();
                document.getElementById("btnback").disabled = true;
            }


        }
    });

    //-------------------------------------Reject--------------------------------//
    $("#btnRejct").click(function () {


        document.getElementById("btnback").disabled = false;
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var iwL1 = "iwL1";

        if (backbtn == true) {

            iwL1 = iwL1 + backcnt
            L1 = {
                "DbtAccountNo": $("#accnt").val(),
                "ActualAmount": $("#iwAmt").val(),
                "Date": $("#ChqDate").val(),
                "EntrySerialNo": $("#ChqnoQC").val(),
                "EntryPayorBankRoutNo": $("#SortQC").val(),
                "EntrySAN": $("#SANQC").val(),
                "EntryTransCode": $("#TransQC").val(),
                "ID": tt[backcnt].ID,
                "sttsdtqc": cnrslt,
                "XMLAmount": tt[backcnt].XMLAmount,
                "XMLSerialNo": tt[backcnt].XMLSerialNo,
                "XMLPayorBankRoutNo": tt[backcnt].XMLPayorBankRoutNo,
                "XMLSAN": tt[backcnt].XMLSAN,
                "XMLTransCode": tt[backcnt].XMLTransCode,
                "CBSClientAccountDtls": tt[backcnt].CBSClientAccountDtls,
                "CBSJointHoldersName": tt[backcnt].CBSJointHoldersName,
            };
        }

        else {

            iwL1 = iwL1 + cnt;
            L1 = {
                "DbtAccountNo": $("#accnt").val(),
                "ActualAmount": $("#iwAmt").val(),
                "Date": $("#ChqDate").val(),
                "EntrySerialNo": $("#ChqnoQC").val(),
                "EntryPayorBankRoutNo": $("#SortQC").val(),
                "EntrySAN": $("#SANQC").val(),
                "EntryTransCode": $("#TransQC").val(),
                "ID": tt[tempcnt].ID,
                "sttsdtqc": cnrslt,
                "XMLAmount": tt[tempcnt].XMLAmount,
                "XMLSerialNo": tt[tempcnt].XMLSerialNo,
                "XMLPayorBankRoutNo": tt[tempcnt].XMLPayorBankRoutNo,
                "XMLSAN": tt[tempcnt].XMLSAN,
                "XMLTransCode": tt[tempcnt].XMLTransCode,
            };

        }
        common(iwL1);
        backbtn = false;
    });
    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;

        if (Modernizr.localstorage) {

            backbtn = true;
            var iwL1 = "iwL1"
            cnt = document.getElementById('cnt').value;
            iwL1 = iwL1 + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(iwL1));

            document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;

            document.getElementById('accnt').value = orderData.DbtAccountNo
            document.getElementById('ChqDate').value = orderData.Date
            document.getElementById('iwAmt').value = orderData.ActualAmount
            document.getElementById('ChqnoQC').value = orderData.EntrySerialNo;
            document.getElementById('SortQC').value = orderData.EntryPayorBankRoutNo;
            document.getElementById('SANQC').value = orderData.EntrySAN;
            document.getElementById('TransQC').value = orderData.EntryTransCode;
            document.getElementById('xmlamt').innerHTML = tt[cnt - 1].XMLAmount;
            document.getElementById('accnt').focus();
            //---------------------Filling cbsDetails------
            $.ajax({
                url: RootUrl + 'IWL1/GetCBSDtls',
                //url: RootUrl +'/IWL1/GetCBSDtls',
                dataType: 'html',
                type: 'POST',
                data: { strcbsdls: orderData.CBSClientAccountDtls, strJoinHldrs: orderData.CBSJointHoldersName },
                success: function (data) {
                    $('#cbsdetails').html(data);
                    cbsbtn = true;
                }
            });
        }
    });
    //--------------Reject---------------------------------------
    $("#btnClose").click(function () {

        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;

            if (scond == true) {
                for (var i = 0; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwL1" + i));

                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.DbtAccountNo);
                    arrlist.push(orderData.ActualAmount);
                    arrlist.push(orderData.Date);
                    arrlist.push(orderData.EntrySerialNo);
                    arrlist.push(orderData.EntryPayorBankRoutNo);
                    arrlist.push(orderData.EntrySAN);
                    arrlist.push(orderData.EntryTransCode);
                    arrlist.push(orderData.sttsdtqc);
                    arrlist.push(orderData.XMLAmount);
                    arrlist.push(orderData.XMLSerialNo);
                    arrlist.push(orderData.XMLPayorBankRoutNo);
                    arrlist.push(orderData.XMLSAN);
                    arrlist.push(orderData.XMLTransCode);
                    arrlist.push(orderData.RejectReason);
                    arrlist.push(orderData.CBSClientAccountDtls);
                    arrlist.push(orderData.CBSJointHoldersName);
                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwL1" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.DbtAccountNo);
                    arrlist.push(orderData.ActualAmount);
                    arrlist.push(orderData.Date);
                    arrlist.push(orderData.EntrySerialNo);
                    arrlist.push(orderData.EntryPayorBankRoutNo);
                    arrlist.push(orderData.EntrySAN);
                    arrlist.push(orderData.EntryTransCode);
                    arrlist.push(orderData.sttsdtqc);
                    arrlist.push(orderData.XMLAmount);
                    arrlist.push(orderData.XMLSerialNo);
                    arrlist.push(orderData.XMLPayorBankRoutNo);
                    arrlist.push(orderData.XMLSAN);
                    arrlist.push(orderData.XMLTransCode);
                    arrlist.push(orderData.RejectReason);
                    arrlist.push(orderData.CBSClientAccountDtls);
                    arrlist.push(orderData.CBSJointHoldersName);
                }
            }
            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(pcnt);
            $.ajax({

                //url: 'IWL1/IWl1',
                url: RootUrl + 'IWL1/IWl1',
                data: JSON.stringify({ lst: arrlist, snd: scond, btnClose: "Close", idlst: idslst }),

                type: 'POST',
                contentType: 'application/json; charset=utf-8',

                dataType: 'json',
                success: function (result) {
                    if (result == false) {
                        window.location = RootUrl + 'IWL1/Selection';
                        //window.location = 'IWL1/Selection';
                    }
                }
            });
        }


    });
    //------------------------------------------------------------

    var value = 0;
    callrotate = function () {
        value += 180;
        $("#myimg").rotate({ animateTo: value })
    }
    //$("#myimg").rotate({
    //    bind:
    //      {
    //          click: function () {
    //              value += 180;
    //              $(this).rotate({ animateTo: value })
    //          }
    //      }

    //});
    $("#ChqDate").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.charCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            event.preventDefault();
        }
    });
    //---------------- Data Entry -----------------------------------

    $("form input").keydown(function (e) {
        // alert('Aila');
        next_idx = $('input[type=text]').index(this) + 1;
        tot_idx = $('body').find('input[type=text]').length;
        //if (next_idx == 2) {
        //    cbsbtn = false
        //}
        if (next_idx == 3) {

            next_idx = next_idx + 3;
        }
        if (next_idx == 10) {
            //  alert('kk');
            btnvalacpt = true;
            next_idx = 3;//next_idx + 1;
        }
        if (next_idx == 4 || next_idx == 5) {
            next_idx = 10;//prev it was 11
        }

        // debugger;
        if (e.keyCode == 13) {

            if (tot_idx == next_idx) {
                $("input[value='Ok']").click();
            }
            else if (next_idx == 1) {

                var tempacntno = document.getElementById('oldact').value;
                if ($("#accnt").val() == "") {
                    alert("Please enter account number!!")
                    return false;
                }
                if (tempacntno != $("#accnt").val()) {

                    document.getElementById('oldact').value = $("#accnt").val();
                    getcbsdtls();
                }
                else {
                    $('input[type=text]:eq(' + next_idx + ')').focus().select();
                }
            }

            else {
                $('input[type=text]:eq(' + next_idx + ')').focus().select();

            }
        }
    });

    $("#accnt").keypress(function (event) {

        // alert(event.keyCode);
        // debugger;
        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
            cbsbtn = false
        }
        else {
            event.preventDefault();
        }
    });

    $("#ChqnoQC,#SortQC,#SANQC,#TransQC,#IWRemark").keypress(function (event) {

        //alert(event.charCode);
        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

        }
        else {
            event.preventDefault();
        }
    });
    $("#IWDecision").keydown(function (event) {

        //alert(event.keyCode);

        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }
        if (document.getElementById("blockkey").value == "1") {
            if (event.keyCode == 65 || event.keyCode == 97) {
                event.preventDefault();
                alert('You can not accept this cheque!');
                return false;
            } // || event.keyCode == 99
            if (event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

            }
            else {
                event.preventDefault();
            }
        }
        else {

            if (event.keyCode == 113 || event.keyCode == 82 || event.keyCode == 65 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
            }
            else {
                event.preventDefault();
            }

        }
    });
    //----------Amount---------
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
    //------------rtndescrp---------------------------------

    $("#IWRemark").keyup(function (event) {
        var chkcode = false;
        var rejctrcd = $("#IWRemark").val();
        if (rejctrcd.length == 2) {
            var rjctresnlTemp = document.getElementById('rtnlist');
            var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
            for (var i = 0; i < rjctresnlTemp.length; i++) {
                if (rejctrcd == rjctresnlTemp[i].value) {
                    document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
                    chkcode = true;
                    break;
                }
            }
        }
        if (rejctrcd == "88") {

            document.getElementById("rejectreasondescrpsn").readOnly = false;

        }
        else if (rejctrcd != "88") {

            document.getElementById("rejectreasondescrpsn").readOnly = true;
        }
        if (rejctrcd.length == 2) {
            if (chkcode == false) {
                alert('Please enter correct return code!!');
                document.getElementById('IWRemark').value = "";
                document.getElementById('IWRemark').focus();
            }
        }
    });


    //-------------------------------Common For Accept and Reject-----------------------------
    function common(val) {
        //alert(document.getElementById('cbsdls').value);

        if (Modernizr.localstorage) {
            var localacct = window.localStorage;
            var chqiwmicr = JSON.stringify(L1);
            localacct.setItem(val, chqiwmicr);
        }

        if (backbtn == true) {
            document.getElementById('cnt').value = parseInt(backcnt) + 1;
        }
        else {
            document.getElementById('cnt').value = parseInt(cnt) + 1;
        }
        cnt = document.getElementById('cnt').value;
        if (cnt < tt.length) {

            //  alert(tt[cnt].FrontGreyImagePath);
            document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
            //--------------Filling value from OCR1--------------
            document.getElementById('accnt').value = "";
            document.getElementById('ChqDate').value = "";
            document.getElementById('iwAmt').value = "";
            document.getElementById('ChqnoQC').value = "";
            document.getElementById('SortQC').value = "";
            document.getElementById('SANQC').value = "";
            document.getElementById('TransQC').value = "";
            document.getElementById('oldact').value = "";
            // document.getElementById("h2amt").value = "";
            // document.getElementById("drowcbs").innerHTML = "";
            document.getElementById('IWDecision').value = "";
            document.getElementById('IWRemark').value = "";
            document.getElementById('rtncd').style.display = "none";
            document.getElementById("cbsdetails").innerHTML = "";
            document.getElementById('rejectreasondescrpsn').value = "";
            document.getElementById('xmlamt').innerHTML = "";

            //----

            document.getElementById('ChqnoQC').value = tt[cnt].EntrySerialNo;
            document.getElementById('SortQC').value = tt[cnt].EntryPayorBankRoutNo;
            document.getElementById('SANQC').value = tt[cnt].EntrySAN;
            document.getElementById('TransQC').value = tt[cnt].EntryTransCode;
            document.getElementById('xmlamt').innerHTML = addCommas(tt[cnt].XMLAmount); //tt[1].XMLAmount;tt[cnt].XMLAmount;

            document.getElementById('accnt').focus();
            cbsbtn = false;

            // document.getElementById("h2amt").innerHTML = "{{iwAmt|number:2}}";
            if (backbtn == true) {
                document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
            }
            else {
                document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
            }

            $.ajax({
                //url: RootUrl +'/IWL1/GetCBSDtls',
                url: RootUrl + 'IWL1/GetCBSDtls',
                dataType: 'html',
                type: 'POST',
                data: { ac: tt[cnt].EntrySAN },
                success: function (data) {
                    $('#cbsdetails').html(data);
                    cbsbtn = true;
                }
            });

            backbtn = false;
        }
        else if (cnt > 0) {

            if (Modernizr.localstorage) {
                var listItems = [];
                var arrlist = [];
                var localData = window.localStorage;

                if (scond == true) {
                    for (var i = 0; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwL1" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.DbtAccountNo);
                        arrlist.push(orderData.ActualAmount);
                        arrlist.push(orderData.Date);
                        arrlist.push(orderData.EntrySerialNo);
                        arrlist.push(orderData.EntryPayorBankRoutNo);
                        arrlist.push(orderData.EntrySAN);
                        arrlist.push(orderData.EntryTransCode);
                        arrlist.push(orderData.sttsdtqc);
                        arrlist.push(orderData.XMLAmount);
                        arrlist.push(orderData.XMLSerialNo);
                        arrlist.push(orderData.XMLPayorBankRoutNo);
                        arrlist.push(orderData.XMLSAN);
                        arrlist.push(orderData.XMLTransCode);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.CBSClientAccountDtls);
                        arrlist.push(orderData.CBSJointHoldersName);

                    }
                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwL1" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.DbtAccountNo);
                        arrlist.push(orderData.ActualAmount);
                        arrlist.push(orderData.Date);
                        arrlist.push(orderData.EntrySerialNo);
                        arrlist.push(orderData.EntryPayorBankRoutNo);
                        arrlist.push(orderData.EntrySAN);
                        arrlist.push(orderData.EntryTransCode);
                        arrlist.push(orderData.sttsdtqc);
                        arrlist.push(orderData.XMLAmount);
                        arrlist.push(orderData.XMLSerialNo);
                        arrlist.push(orderData.XMLPayorBankRoutNo);
                        arrlist.push(orderData.XMLSAN);
                        arrlist.push(orderData.XMLTransCode);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.CBSClientAccountDtls);
                        arrlist.push(orderData.CBSJointHoldersName);
                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------
                // alert(cnt);
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;
                //alert(pcnt);
                $.ajax({

                    url: RootUrl + 'IWL1/IWl1',
                    //url: 'IWL1/IWl1',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[pcnt - 1].FrontGreyImagePath, idlst: idslst }),

                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',

                    dataType: 'json',
                    success: function (result) {
                        if (result == false) {
                            window.location = RootUrl + 'IWL1/Selection';
                            //window.location = 'IWL1/Selection';
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
                                //-------Remove save objects from browser---//
                                window.localStorage.clear();
                                //------------ idslist end----------------//
                                //  scondbck = true;
                                //-------------Saving Last data in storage---
                                var iwL1 = "iwL10"
                                var L1 = {
                                    "DbtAccountNo": tt[0].DbtAccountNo,
                                    "ActualAmount": tt[0].ActualAmount,
                                    "Date": tt[0].Date,
                                    "EntrySerialNo": tt[0].EntrySerialNo,
                                    "EntryPayorBankRoutNo": tt[0].EntryPayorBankRoutNo,
                                    "EntrySAN": tt[0].EntrySAN,
                                    "EntryTransCode": tt[0].EntryTransCode,
                                    "ID": tt[0].ID,
                                    "sttsdtqc": tt[0].sttsdtqc,
                                    "XMLAmount": tt[0].XMLAmount,
                                    "XMLSerialNo": tt[0].XMLSerialNo,
                                    "XMLPayorBankRoutNo": tt[0].XMLPayorBankRoutNo,
                                    "XMLSAN": tt[0].XMLSAN,
                                    "XMLTransCode": tt[0].XMLTransCode,
                                    "RejectReason": tt[0].RejectReason,
                                    "CBSClientAccountDtls": tt[0].CBSClientAccountDtls,
                                    "CBSJointHoldersName": tt[0].CBSJointHoldersName,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqiwmicr = JSON.stringify(L1);
                                    localacct.setItem(iwL1, chqiwmicr);

                                }
                                // alert(tt[1].FrontGreyImagePath);
                                //----------------------------------------------------------------------//
                                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                document.getElementById('accnt').value = "";
                                document.getElementById('ChqDate').value = "";
                                document.getElementById('iwAmt').value = "";
                                document.getElementById('ChqnoQC').value = "";
                                document.getElementById('SortQC').value = "";
                                document.getElementById('SANQC').value = "";
                                document.getElementById('TransQC').value = "";
                                document.getElementById('accnt').focus();
                                document.getElementById('oldact').value = "";
                                // document.getElementById("h2amt").value = "";
                                // document.getElementById("drowcbs").innerHTML = "";
                                document.getElementById('IWDecision').value = "";
                                document.getElementById('IWRemark').value = "";
                                document.getElementById('rtncd').style.display = "none";
                                document.getElementById("cbsdetails").innerHTML = "";
                                document.getElementById('xmlamt').innerHTML = "";
                                //----
                                document.getElementById('ChqnoQC').value = tt[1].EntrySerialNo;
                                document.getElementById('SortQC').value = tt[1].EntryPayorBankRoutNo;
                                document.getElementById('SANQC').value = tt[1].EntrySAN;
                                document.getElementById('TransQC').value = tt[1].EntryTransCode;
                                document.getElementById('xmlamt').innerHTML = addCommas(tt[1].XMLAmount); //tt[1].XMLAmount;
                                //   document.getElementById("h2amt").innerHTML = "{{iwAmt|number:2}}";
                                //---------------Setting focus to textbox----------------//

                                //document.getElementById('iwDateQc').focus();
                                $.ajax({
                                    //url: RootUrl +'/IWL1/GetCBSDtls',
                                    url: RootUrl + 'IWL1/GetCBSDtls',
                                    dataType: 'html',
                                    type: 'POST',
                                    data: { ac: tt[1].EntrySAN },
                                    success: function (data) {
                                        $('#cbsdetails').html(data);
                                        cbsbtn = true;
                                    }
                                });
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

    getcbsdtls = function () {

        // debugger;
        var Acct = document.getElementById('accnt').value;
        var acmin = document.getElementById('acmin').value;
        //Acct = Acct.replace(/^0+/, '')
        Acct = Acct.length


        if (Acct == "") {
            alert("Account no field should not be empty !");
            document.getElementById('accnt').focus();
            return false;
        }
        if (Acct < acmin) {

            alert("Minimum account no sould be " + acmin + " digits");
            document.getElementById('accnt').focus();
            return false;
        }
        Acct = document.getElementById('accnt').value.replace(/^0+/, '')
        if (Acct == "") {
            alert("Invalid Account Number!");
            document.getElementById('accnt').focus();
            return false;
        }
        else {
            $.ajax({
                url: RootUrl + 'IWL1/GetCBSDtls',
                //url: 'IWL1/GetCBSDtls',
                dataType: 'html',
                type: 'POST',
                data: { ac: $("#accnt").val() },
                success: function (data) {
                    //debugger;
                    //alert(data);
                    $('#cbsdetails').html(data);
                    cbsbtn = true;
                },
                error: function () {
                    alert("error");
                }
            });
        }
    }

    //-----------------Dialog box----------------
    //getreason = function () {

    //    $(document).ready(function () {
    //        $("#dialogEditUser").dialog({
    //            draggable: true,
    //            height: 400,
    //            width: 500,
    //            buttons: [
    //            {
    //                text: "minimize",
    //                click: function () {
    //                    $(this).parents('.ui-dialog').animate({
    //                        height: '40px',
    //                        top: $(window).height() - 40
    //                    }, 400);
    //                }
    //            }]
    //        });

    //        $.ajax({
    //            url: RootUrl + 'IWL1/RejectReason',
    //            dataType: 'html',
    //            data: { id: 0 },
    //            success: function (data) {
    //                //alert(data);
    //                $('#dialogEditUser').html(data);
    //                $('#dialogEditUser').dialog('open');
    //            }
    //        });
    //    });
    //}
});

function fullImage() {
    //  alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}
function ChangeImage(imagetype) {
    // alert(imagetype);
    var indexcnt = document.getElementById('cnt').value;
    if (imagetype == "FTiff") {

        document.getElementById('myimg').src = tt[indexcnt].FrontTiffImagePath;
    }
    else if (imagetype == "BTiff") {
        //alert('Browser not supporting!!!');
        document.getElementById('myimg').src = tt[indexcnt].BackTiffImagePath;
    }
    else if (imagetype == "FGray") {

        document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath;
    }

}


function reasonselected(rtnval) {
    //var rtnrjctdescrn = document.getElementById('rtndescrp').value;
    ////-----valid Function for validation---------------
    //var rslt = valid(document.getElementById('rtndescrp').value, rtnval);

    //if (rslt == false) {
    //    //alert('Please select reject reason!!');
    //    document.getElementById('rtndescrp').focus();
    //    document.getElementById('RejectReason').style.display = 'block';
    //    return false;
    //}
    // else {
    document.getElementById('IWRemark').value = rtnval;
    document.getElementById('RejectReason').style.display = 'none';
    var rejctrcd = $("#IWRemark").val();
    if (rejctrcd.length == 2) {
        var rjctresnlTemp = document.getElementById('rtnlist');
        var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
        for (var i = 0; i < rjctresnlTemp.length; i++) {
            if (rejctrcd == rjctresnlTemp[i].value) {
                //if (rejctrcd == "88") {
                //    document.getElementById("rejectreasondescrpsn").value = rtnrjctdescrn;
                //}
                //else {
                document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
                //}
                break;
            }
        }
    }
    // }
}
//---------------
function valid(RejectDescription, Reasoncode) {
    if (Reasoncode == "") {
        alert('Reason code not selected !');
        return false;
    }
    if (Reasoncode == "88" && RejectDescription == "") {
        alert('Please enter reject description!');
        //document.getElementById('RejectDescription').style.display = "";
        return false;
    }
    else if (Reasoncode == "88") {
        var str1 = RejectDescription.toUpperCase();
        var str = str1.replace(/\s+/g, " ");

        for (var j = 0; j < str.length; j++) {
            if (str.charAt(j) == "&" || str.charAt(j) == "<" || str.charAt(j) == ">" || str.charAt(j) == "'" || str.charAt(j) == '"') {
                alert("some special characters are not allowed\n & < > ' ");
                return false;
            }
        }
        if (str.length < 6) {
            alert('Please enter minimum 6 character for reject description!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('RejectDescription').focus();
            return false;
        }
        // alert(str);
        if (str.indexOf("OTHER REASON") >= 0 || str.indexOf("OTHERREASON") >= 0) {
            alert('You can not mention "other reason"!!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('RejectDescription').focus();
            return false;
        }
        else if (str.indexOf("OTHER") >= 0 && str.indexOf("REASON") >= 0) {
            alert('You can not mention "other reason"!!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('RejectDescription').focus();
            return false;
        }
        var re = /[^0-9]/g;
        // alert(str.charAt(0));
        // alert(re.test(str.charAt(0)));
        if (re.test(str.charAt(0)) == false || /^[a-zA-Z0-9- ]*$/.test(str.charAt(0)) == false) {
            alert('Please start with alphabets!!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('RejectDescription').focus();
            return false;
        }

        var prv = false;
        next = false
        for (var i = 0; i < str.length; i++) {
            if (/^[a-zA-Z0-9- ]*$/.test(str.charAt(i)) == false) {
                if (prv == true) {
                    next = true;
                    break;
                }
                prv = true;
            }
            else {
                prv = false;
            }
        }

        if (next == true) {
            alert('Repetitive special characters are not allowed!!');
            //document.getElementById('RejectDescription').style.display = "";
            //document.getElementById('lblrjctds').style.display = "";
            //document.getElementById('RejectDescription')
            // document.getElementById('RejectDescription').focus();
            return false;
        }
    }
}


function IWLICQC() {
    // alert('abid');
    var IWdecn = document.getElementById('IWDecision').value.toUpperCase();
    //----------------DBTAC-----------------------
    var Acct = document.getElementById('accnt').value;
    var acmin = document.getElementById('acmin').value;

    var tempacntno = document.getElementById('oldact').value;
    if ($("#accnt").val() == "") {
        alert("Please enter account number!!")
        return false;
    }
    if (tempacntno != $("#accnt").val()) {

        document.getElementById('oldact').value = $("#accnt").val();
        cbsbtn = false
        //getcbsdtls();
    }
    if (document.getElementById("blockkey").value == "1" && IWdecn == "A") {
        alert('You can not accept this cheque!');
        return false;
    } // || event.keyCode == 99


    var Accval = Acct;
    Acct = Acct.length
    //  debugger;

    if (Acct == "") {
        alert("Account no field should not be empty !");
        document.getElementById('accnt').focus();
        return false;
    }
    if (Acct < acmin) {

        alert("Minimum account no sould be " + acmin + " digits");
        document.getElementById('accnt').focus();
        return false;
    }

    Acct = document.getElementById('accnt').value.replace(/^0+/, '')
    if (Acct == "") {
        alert("Invalid Account Number!");
        document.getElementById('accnt').focus();
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
            document.getElementById('accnt').focus();
            return false;
        }

    }
    //if (cbsbtn == false) {
    //    alert("Click on 'GetDetails' button/press F12 !");
    //    document.getElementById('accnt').focus();
    //    return false;
    //}

    //----------------------------Amount---------------------//
    amt = document.getElementById('iwAmt').value;
    //alert(amt);   
    var intcont = 0;
    for (var i = 0; i < amt.length; i++) {

        if (amt.charAt(i) == ".") {
            intcont++;
        }
        if (intcont > 1) {
            alert('Enter valid amount!');
            document.getElementById('iwAmt').focus();
            return false;
        }
    }

    if (amt == "NaN") {
        alert('Enter valid amount!');
        document.getElementById('iwAmt').focus();
        return false;
    }

    amt1 = amt;
    amt = amt.replace(/^0+/, '')
    amt = amt.length;
    if (amt1 == ".") {
        alert('Amount field should not be dot(.) !');
        document.getElementById('iwAmt').focus();
        return false;
    }
    else if (amt1 == "0.00") {
        alert('Amount field should not be zero(0) !');
        document.getElementById('iwAmt').focus();
        return false;
    }
    else if (amt < 1) {
        alert('Amount field should not be empty !');
        document.getElementById('iwAmt').focus();
        return false;
    }
    else if (amt > 15) {
        alert('Amount not valid !');
        document.getElementById('iwAmt').focus();
        return false;
    }

    var tempacnt;
    if (backbtn == true) {
        tempacnt = backcnt;
    }
    else {
        tempacnt = $("#cnt").val();
    }
    if (tt[tempacnt].XMLAmount != $("#iwAmt").val() && IWdecn == "A") {

        alert('Entered Amount is not matching with NPCI Amount\ Please correct the amount or reject the cheque!');
        document.getElementById('iwAmt').focus();
        return false;
    }
    //------------------------Sort code---------------
    var ChqnoQC = document.getElementById('ChqnoQC').value;
    var SortQC = document.getElementById('SortQC').value;
    var SANQC = document.getElementById('SANQC').value;
    var TransQC = document.getElementById('TransQC').value;
    if (ChqnoQC == "") {
        //alert('aila');
        alert("Cheque no should not be empty !");
        chq = true;
        return false;
    }
    else if (SortQC == "") {
        //alert('aila');
        alert("Sort code should not be empty !");
        srtcd = true;
        chq = false;
        return false;
    }
    else if (SANQC == "") {
        alert("SAN code should not be empty !");
        sancd = true;
        srtcd = false;
        chq = false;
        return false;
    }
    else if (TransQC == "") {
        alert("Trans code should not be empty !");
        trcd = true;
        sancd = false;
        srtcd = false;
        chq = false;
        return false;
    }
    else if (SortQC.length < 9 || SortQC == "000000000") {
        alert("Sort code is not valid !");
        srtcd = true;
        chq = false;
        return false;
    }
    else if (ChqnoQC.length < 6 || ChqnoQC == "000000") {
        alert("Cheque number is not valid !");
        chq = true;
        return false;
    }
    else if (SANQC.length < 6) {
        alert("SAN code is not valid !");
        sancd = true;
        srtcd = false;
        chq = false;
        return false;
    }
    else if (TransQC.length < 2 || TransQC == "00" || TransQC.substring(0, 1) == "0") {
        alert("Trans code is not valid !");
        trcd = true;
        sancd = false;
        srtcd = false;
        chq = false;
        return false;
    }
    var rtnflg = validYrnscodes();
    if (rtnflg == false) {
        alert("Trans code is not valid !");
        trcd = true;
        sancd = false;
        srtcd = false;
        chq = false;
        return false;
    }



    //-------------------------ChqDate-------------------------------------------------//

    var dd, mm, yy;
    dat = document.getElementById('ChqDate').value;
    if (dat == "") {
        //alert('aila');
        alert("Date field should not be empty !");
        document.getElementById('ChqDate').focus();
        document.getElementById('ChqDate').select();
        return false;
    }
    else if ((dat.toUpperCase().charAt(dat.length - 1) == "X") && (IWdecn.toUpperCase() == "A")) {
        alert("Date is not correct, Please reject this cheque!!");
        document.getElementById('IWDecision').focus();
        return false;
    }
        //else if (dat == "000000") {  Commented On 22-01-2017
        //    alert("Date not valid !");
        //    document.getElementById('ChqDate').focus();
        //    document.getElementById('ChqDate').select();
        //    return false;
        //}
        // else if (dat.length < 2 || dat.length == 3 || dat.length == 5) {
    else if (dat.length < 6) {
        alert("Date not valid !");
        document.getElementById('ChqDate').focus();
        document.getElementById('ChqDate').select();
        return false;
    }

    else {

        debugger;
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

        if (IWdecn == "A" || (IWdecn == "R" && $("#ChqDate").val() != "000000")) {
            var onlydate = dd + '/' + mm + '/' + '20' + yy;


            var rtn = validatedate(onlydate);
            if (rtn == false) {
                document.getElementById('ChqDate').focus();
                document.getElementById('ChqDate').select();
                return false;
            }
        }


    }
    //------------------------------------Post Date and Stale Cheques ----///
    //debugger;
    var stlmntdt = document.getElementById('stlmnt').value;
    var sesondt = document.getElementById('sesson').value;

    var fnldate = '20' + yy + '/' + mm + '/' + dd;
    var staldat = new Date(sesondt);
    var postdat = new Date(stlmntdt);
    var d3 = new Date(fnldate);

    //var timeDiff = Math.abs(staldat.getTime() - d3.getTime());
    //var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

    if (IWdecn == "A") {
        if (postdat <= d3) {
            alert('Post date!!');
            document.getElementById('ChqDate').focus();
            document.getElementById('ChqDate').select();
            return false;
        }
        if (staldat >= d3) {
            alert('Stale Cheque!!');
            document.getElementById('ChqDate').focus();
            document.getElementById('ChqDate').select();
            return false;
        }

    }

    ///----------------------------------------------------------------------------------------//

    var chqdt = document.getElementById('ChqDate').value;
    if (chqdt.length <= 0 || chqdt.length < 2) {
        alert('Please enter correct Date!');
        document.getElementById('ChqDate').focus();
        return false;
    }

    if (IWdecn == "A" && document.getElementById('blockkey') == "1") {
        event.preventDefault();
        alert('You can not accept this cheque!');
        return false;
    }
    if (IWdecn == "A" && dat == "000000") {
        event.preventDefault();
        alert('You can not accept this cheque\n Please correct date or reject the cheque!');
        return false;
    }

    if (IWdecn == "") {
        alert('Please enter decision!');
        document.getElementById('IWDecision').focus();
        return false;
    }

    else if (IWdecn != "A" && IWdecn != "R") {
        // alert(IWdecn);
        alert('Decision not correct!');
        document.getElementById('IWDecision').focus();
        return false;
    }
    else if (IWdecn == "R") {
        if (document.getElementById('IWRemark').value == "") {
            alert('Please enter reject reason!');
            document.getElementById('IWRemark').focus();
            return false;
        }
        else if (document.getElementById('IWRemark').value == "88") {
            var dscflg = valid(document.getElementById('rejectreasondescrpsn').value, document.getElementById('IWRemark').value);
            if (dscflg == false) {
                // alert('In valid reject reason!');
                document.getElementById('rejectreasondescrpsn').focus();
                return false;
            }
        }
        var flg = false;
        var iwrked = document.getElementById('IWRemark').value;
        var rjctresnl = document.getElementById('rtnlist');
        for (var i = 0; i < rjctresnl.length; i++) {

            if (iwrked == rjctresnl[i].value) {
                flg = true;
                // alert(rjctresnl[i].value);
                break;
            }
        }
        if (flg == false) {
            alert('In valid reject reason!');
            document.getElementById('IWRemark').focus();
            return false;
        }
    }
}
//------------------

function validYrnscodes() {
    var temptrnscd = document.getElementById('TransQC').value;
    var flg;
    $.ajax({

        url: RootUrl + "IWDataEntry/ValidTrans",
        //url: "IWDataEntry/ValidTrans",
        data: { transcode: temptrnscd },
        dataType: "html",
        async: false,
        success: function (trnsresult) {
            if (trnsresult == "false") {
                alert('Trans code not valid!!');

                flg = false;
            }
            else {
                flg = true;
            }
        }

    });
    return flg;
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





function MatchWith(indexvl) {

    var match = 0;
    var tempchqno = document.getElementById('ChqnoQC').value;
    var tempsortcd = document.getElementById('SortQC').value;
    var tempsan = document.getElementById('SANQC').value;
    var temptrnscd = document.getElementById('TransQC').value;


    if ((tt[indexvl].XMLSerialNo == tempchqno) && (tt[indexvl].XMLPayorBankRoutNo == tempsortcd) && (tt[indexvl].XMLSAN == tempsan) && (tt[indexvl].XMLTransCode == temptrnscd)) {
        match = 1;
    }
    return (match);
}
function MatchAmt(indexvl) {
    var match = 0;
    var tempAmount = document.getElementById('iwAmt').value;

    if (tt[indexvl].XMLAmount == tempAmount) {
        match = 1;
    }
    return (match);
}

function IWVef() {
    //rtncd
    document.getElementById('rtncd').style.display = "none";


    chr = document.getElementById('IWDecision').value.toUpperCase();
    var chr = document.getElementById('IWDecision').value.toUpperCase();
    var iwrk = document.getElementById('IWRemark').value;
    if (chr == "R") {
        if (iwrk == "") {
            document.getElementById('rtncd').style.display = "";
            document.getElementById('IWRemark').style.width = "10%";
            document.getElementById('rejectreasondescrpsn').value = "";
            document.getElementById('IWRemark').focus();
        }
        else {
            // alert('aila');
            document.getElementById('rtncd').style.display = "";
            document.getElementById('IWDecision').focus();
        }

    }
    else {

        document.getElementById('rtncd').style.display = "none";
    }
}

function fnGetvalue(inpt) {
    var vlu = document.getElementById(inpt).value();
    alert(vlu);

}

function addCommas(amtval) {
    var flg = true;
    var count = 0;
    var finaloutpt = "";
    //        var nStr = document.getElementById("amt").value;
    var nStr = amtval;
    // alert(nStr);
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';

    var amount = new String(x1);
    amount = amount.split("").reverse();

    //        for (var i = x1.length-1; i>0; i--) {
    for (var i = 0; i <= amount.length - 1; i++) {
        // alert;
        //alert(amount[i]);
        finaloutpt = amount[i] + finaloutpt;
        // alert(finaloutpt);
        if (flg == true) {
            if (count == 2) {
                // alert('cool');
                flg = false;
                if (amount.length <= 3) {
                }
                else {
                    finaloutpt = "," + finaloutpt;
                }
                count = 0;
            }
        }
        else {
            if (count == 2 && i <= amount.length - 2) {
                // flg = false;
                //alert('aila');
                finaloutpt = "," + finaloutpt;
                count = 0;
            }
        }
        count = count + 1;
    }

    return (finaloutpt + x2);
}



