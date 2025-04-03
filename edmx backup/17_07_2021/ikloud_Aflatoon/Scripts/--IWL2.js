
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
var idslst = [];
var cbsbtn = false;
var signaturecall = false;

//var scondbck = false;
function passval(array) {
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}

var autogetcbsdtls;
var getcbsdtls;
var getreason;
var getIwlogs;
var tempdat;
var fnldate;
var yr, mm, dd;

$(document).ready(function () {


    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].ID)
    }

    $('#accnt,#ChqDate').bind("cut paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    //  alert(idslst.length);
    //------------ idslist end----------------//
    //-----------------ShortCut----for CBS---
    $("#accnt").keydown(function (event) {
        //alert(event.keyCode);
        if (event.keyCode == 123) {
            getcbsdtls(); //CbsDetails --------
            return false;
        }
    });

    if (tt.length > 0) {


        //document.getElementById("p2").style.color = "blue";
        if (tt[1].Date.length > 6) {
            tempdat = tt[1].Date.split("-");
            yr = tempdat[0];
            yr = yr.substring(2, 4);
            mm = tempdat[1];
            dd = tempdat[2];
            fnldate = dd + mm + yr;
        }
        else {
            tempdat = tt[1].Date;
            yr = tempdat.substring(4, 6);
            mm = tempdat.substring(2, 4);
            dd = tempdat.substring(0, 2);
            fnldate = dd + mm + yr;
        }

        // alert(fnldate);
        document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

        document.getElementById('ChqnoQC').innerHTML = tt[1].XMLSerialNo;
        document.getElementById('SortQC').innerHTML = tt[1].XMLPayorBankRoutNo;
        document.getElementById('SANQC').innerHTML = tt[1].XMLSAN;
        document.getElementById('TransQC').innerHTML = tt[1].XMLTransCode;
        //document.getElementById('h2amt').value = "";
        document.getElementById('ChqDate').value = fnldate;
        document.getElementById("iwAmt").innerHTML = addCommas(tt[1].XMLAmount);//added on 12-05-2017;
        document.getElementById('accnt').value = tt[1].DbtAccountNo;


        document.getElementById('mtrn').value = tt[1].ID;
        document.getElementById('oldact').value = tt[1].DbtAccountNo;
        document.getElementById('IWDecision').focus();
        document.getElementById("btnback").disabled = true
        //-----------------P2F Indicator----------------
        document.getElementById('Acdtls').innerHTML = "";
        document.getElementById('Micrdtls').innerHTML = "";

        if (tt[1].DocType == "C") {
            document.getElementById('doctype').innerHTML = "P2F Indicater : Y";
        }
        else {
            document.getElementById('doctype').innerHTML = "P2F Indicater : N";
        }
        //-----------------Presenting Bank Code----------------
        document.getElementById('presenting').innerHTML = "Presenting Bank : " + tt[1].PresentingBankRoutNo

        //------------------------MICR Flags-----------------------------
        if (tt[1].XMLMICRRepairFlags != null || tt[1].XMLMICRRepairFlags == "") {

            $("#Acdtls").removeClass();
            $("#Micrdtls").removeClass();

            if (tt[1].XMLMICRRepairFlags.substring(4, 5) == "0") {
                document.getElementById('Acdtls').innerHTML = " Not Verified";//Credit A/C :
                document.getElementById("Acdtls").classList.add("w3-highway-red");
            }
            else if (tt[1].XMLMICRRepairFlags.substring(4, 5) == "5") {
                document.getElementById('Acdtls').innerHTML = " Old Account";//Credit A/C :
                document.getElementById("Acdtls").classList.add("w3-highway-green");
            }
            else if (tt[1].XMLMICRRepairFlags.substring(4, 5) == "9") {
                document.getElementById('Acdtls').innerHTML = " New Account";//Credit A/C :
                document.getElementById("Acdtls").classList.add("w3-highway-green");
            }
            if (tt[1].XMLMICRRepairFlags.substring(0, 4) == "1111" && tt[1].XMLMICRRepairFlags.substring(5, 6) == "1") {
                document.getElementById('Micrdtls').innerHTML = " MICR Modified-" + tt[1].XMLMICRRepairFlags;//MICR Status:
                document.getElementById("Micrdtls").classList.add("w3-highway-red");
            }
            else {
                document.getElementById('Micrdtls').innerHTML = " MICR Not Modified-" + tt[1].XMLMICRRepairFlags;//MICR Status:
                document.getElementById("Micrdtls").classList.add("w3-highway-green");
            }
        }

        //---------------Call Cbs Details--------
        autogetcbsdtls(tt[1].CBSClientAccountDtls, tt[1].CBSJointHoldersName, tt[1].DbtAccountNo);//Commented for cbi
        bankName(tt[1].PresentingBankRoutNo);  //-------------For bank name

        // debugger;
        //------------------L1 Reject---------------
        document.getElementById('l1decision').innerHTML = ""
        document.getElementById('L1rejectDecrp').innerHTML = ""
        // debugger;
        if (tt[1].L1Status == 1 || tt[1].L1Status == 2 || tt[1].L1Status == 5) {
            // document.getElementById('l1dec').style.display = "";
            if (tt[1].L1Status == 1) {
                $("#l1decision").removeClass();
                document.getElementById("l1decision").innerHTML = "Y";
                // document.getElementById("l1dec").classList.add("w3-panel w3-green");
                document.getElementById("l1decision").classList.add("w3-text-green");
                document.getElementById('L1rejectDecrp').style.display = "none";
            }

            if (tt[1].L1Status == 2 || tt[1].L1Status == 5) {
                $("#l1decision").removeClass();
                document.getElementById('l1decision').innerHTML = "R"
                document.getElementById("l1decision").classList.add("w3-text-red");
                document.getElementById('L1rejectDecrp').style.display = "";

                getReturnDecrp(tt[1].RejectReason);
            }
        }
        else if (tt[1].L1Status == 0) {

            document.getElementById('l1decision').innerHTML = ""
            document.getElementById('L1rejectDecrp').innerHTML = ""
        }

    }



    //-----------------Ok Button------------------------
    $("#btnok").click(function () {

       // debugger;
        if ($("#waittime").val() == "0") {
            alert('Please wait complete data are not loaded....!!!');
            return false;
        }

        var result = IWLICQC()

        if (result == false) {
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
                    cnrslt = btnval;
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
                    cnrslt = btnval;
                    nextcall = true;
                }


                iwL1 = iwL1 + backcnt
                L1 = {
                    "DbtAccountNo": $("#accnt").val(),
                    // "ActualAmount": $("#iwAmt").val(),
                    "Date": $("#ChqDate").val(),
                    //"EntrySerialNo": $("#ChqnoQC").val(),
                    //"EntryPayorBankRoutNo": $("#SortQC").val(),
                    //"EntrySAN": $("#SANQC").val(),
                    //"EntryTransCode": $("#TransQC").val(),
                    "ID": tt[backcnt].ID,
                    "L2Opsts": cnrslt,
                    "OpsStatus": tt[backcnt].OpsStatus,
                    "XMLAmount": tt[backcnt].XMLAmount,
                    "XMLSerialNo": tt[backcnt].XMLSerialNo,
                    "XMLPayorBankRoutNo": tt[backcnt].XMLPayorBankRoutNo,
                    "XMLSAN": tt[backcnt].XMLSAN,
                    "XMLTransCode": tt[backcnt].XMLTransCode,
                    "RejectReason": tt[backcnt].RejectReason,
                    "L2Rejectreason": $("#IWRemark").val(),
                    "CBSClientAccountDtls": $("#cbsdls").val(),
                    "CBSJointHoldersName": $("#JoinHldrs").val(),
                    "L1Status": tt[backcnt].L1Status,
                    "DbtAccountNoOld": tt[backcnt].DbtAccountNoOld,
                    "DateOld": fnldate,// tt[tempcnt].DateOld,
                    "PresentingBankRoutNo": tt[backcnt].PresentingBankRoutNo,
                    "DocType": tt[backcnt].DocType,
                    "XMLMICRRepairFlags": tt[backcnt].XMLMICRRepairFlags,
                };
            }

            else {
                if (btnval == "R") {
                    cnrslt = btnval;
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
                    cnrslt = btnval;
                    nextcall = true;
                }


                iwL1 = iwL1 + cnt;
                L1 = {

                    "DbtAccountNo": $("#accnt").val(),
                    // "ActualAmount": $("#iwAmt").val(),
                    "Date": $("#ChqDate").val(),
                    //"EntrySerialNo": $("#ChqnoQC").val(),
                    //"EntryPayorBankRoutNo": $("#SortQC").val(),
                    //"EntrySAN": $("#SANQC").val(),
                    //"EntryTransCode": $("#TransQC").val(),
                    "ID": tt[tempcnt].ID,
                    "L2Opsts": cnrslt,
                    "OpsStatus": tt[tempcnt].OpsStatus,
                    "XMLAmount": tt[tempcnt].XMLAmount,
                    "XMLSerialNo": tt[tempcnt].XMLSerialNo,
                    "XMLPayorBankRoutNo": tt[tempcnt].XMLPayorBankRoutNo,
                    "XMLSAN": tt[tempcnt].XMLSAN,
                    "XMLTransCode": tt[tempcnt].XMLTransCode,
                    "RejectReason": tt[tempcnt].RejectReason,
                    "L2Rejectreason": $("#IWRemark").val(),
                    "CBSClientAccountDtls": $("#cbsdls").val(),
                    "CBSJointHoldersName": $("#JoinHldrs").val(),
                    "L1Status": tt[tempcnt].L1Status,
                    "DbtAccountNoOld": tt[tempcnt].DbtAccountNoOld,
                    "DateOld": fnldate,// tt[tempcnt].DateOld,
                    "PresentingBankRoutNo": tt[tempcnt].PresentingBankRoutNo,
                    "DocType": tt[tempcnt].DocType,
                    "XMLMICRRepairFlags": tt[tempcnt].XMLMICRRepairFlags,
                };

            }
            //
            if (nextcall == true) {
                document.getElementById("btnback").disabled = false;
                common(iwL1);
            }
            else {

                document.getElementById('IWDecision').focus();
                document.getElementById("btnback").disabled = true;
            }

            backbtn = false;
        }
    });
    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;

        if (Modernizr.localstorage) {

            backbtn = true;
            var iwL1 = "iwL1"
            var cnt = document.getElementById('cnt').value;
            iwL1 = iwL1 + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(iwL1));

            document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;

            document.getElementById('accnt').value = orderData.DbtAccountNo
            document.getElementById('ChqDate').value = orderData.Date
            document.getElementById('iwAmt').innerHTML = orderData.XMLAmount
            document.getElementById('ChqnoQC').innerHTML = orderData.XMLSerialNo;
            document.getElementById('SortQC').innerHTML = orderData.XMLPayorBankRoutNo;
            document.getElementById('SANQC').innerHTML = orderData.XMLSAN;
            document.getElementById('TransQC').innerHTML = orderData.XMLTransCode;
            document.getElementById('IWDecision').focus();
            document.getElementById("rejectreasondescrpsn").value = "";

            //debugger;
            //--------------Added On 31/05/2017--------------
            fnldate = orderData.DateOld;
            //if (tt[cnt].DateOld.length > 6) {
            //    tempdat = tt[cnt].Date.split("-");
            //    yr = tempdat[0];
            //    yr = yr.substring(2, 4);
            //    mm = tempdat[1];
            //    dd = tempdat[2];
            //    fnldate = dd + mm + yr;
            //}
            //else {
            //    tempdat = tt[cnt].Date;
            //    yr = tempdat.substring(4, 6);
            //    mm = tempdat.substring(2, 4);
            //    dd = tempdat.substring(0, 2);
            //    fnldate = dd + mm + yr;
            //}

            //-----------------P2F Indicator----------------
            document.getElementById('Acdtls').innerHTML = "";
            document.getElementById('Micrdtls').innerHTML = "";

            if (tt[backcnt].DocType == "C") {
                document.getElementById('doctype').innerHTML = "P2F Indicater : Y";
            }
            else {
                document.getElementById('doctype').innerHTML = "P2F Indicater : N";
            }
            //-----------------Presenting Bank Code----------------
            document.getElementById('presenting').innerHTML = "Presenting Bank : " + tt[backcnt].PresentingBankRoutNo

            //------------------------MICR Flags-----------------------------
            if (tt[backcnt].XMLMICRRepairFlags != null || tt[backcnt].XMLMICRRepairFlags == "") {

                $("#Acdtls").removeClass();
                $("#Micrdtls").removeClass();

                if (tt[backcnt].XMLMICRRepairFlags.substring(4, 5) == "0") {
                    document.getElementById('Acdtls').innerHTML = " Not Verified";//Credit A/C :
                    document.getElementById("Acdtls").classList.add("w3-highway-red");
                }
                else if (tt[backcnt].XMLMICRRepairFlags.substring(4, 5) == "5") {
                    document.getElementById('Acdtls').innerHTML = " Old Account";//Credit A/C :
                    document.getElementById("Acdtls").classList.add("w3-highway-green");
                }
                else if (tt[backcnt].XMLMICRRepairFlags.substring(4, 5) == "9") {
                    document.getElementById('Acdtls').innerHTML = " New Account";//Credit A/C :
                    document.getElementById("Acdtls").classList.add("w3-highway-green");
                }
                if (tt[backcnt].XMLMICRRepairFlags.substring(0, 4) == "1111" && tt[backcnt].XMLMICRRepairFlags.substring(5, 6) == "1") {
                    document.getElementById('Micrdtls').innerHTML = " MICR Modified-" + tt[backcnt].XMLMICRRepairFlags;//MICR Status:
                    document.getElementById("Micrdtls").classList.add("w3-highway-red");
                }
                else {
                    document.getElementById('Micrdtls').innerHTML = " MICR Not Modified-" + tt[backcnt].XMLMICRRepairFlags;//MICR Status:
                    document.getElementById("Micrdtls").classList.add("w3-highway-green");
                }
            }
            //---------------Call Cbs Details--------
            autogetcbsdtls(orderData.CBSClientAccountDtls, orderData.CBSJointHoldersName, orderData.DbtAccountNo); //commented for cbi
            bankName(tt[1].PresentingBankRoutNo);  //-------------For bank name

            document.getElementById('l1decision').innerHTML = ""
            document.getElementById('L1rejectDecrp').innerHTML = ""
            //------------------L1 Reject---------------
            if (tt[backcnt].L1Status == 1 || tt[backcnt].L1Status == 2 || tt[backcnt].L1Status == 5) {
                //document.getElementById('l1dec').style.display = "";
                //remove on 31-05-2017  || tt[backcnt].L1Status == 0
                if (tt[backcnt].L1Status == 1) {
                    $("#l1decision").removeClass();
                    document.getElementById('l1decision').innerHTML = "Y"
                    document.getElementById("l1decision").classList.add("w3-text-green");
                    // document.getElementById("L1lbl").classList.add("w3-highway-green");
                    document.getElementById('L1rejectDecrp').style.display = "none";
                }

                if (tt[backcnt].L1Status == 2 || tt[backcnt].L1Status == 5) {
                    $("#l1decision").removeClass();
                    document.getElementById('l1decision').innerHTML = "R"
                    document.getElementById("l1decision").classList.add("w3-text-red");
                    //   document.getElementById("L1lbl").classList.add("w3-highway-red");

                    document.getElementById('L1rejectDecrp').style.display = "";
                    // document.getElementById("L1rejectDecrp").classList.add("w3-highway-red");

                    getReturnDecrp(tt[backcnt].RejectReason);
                }
            }
            else if (tt[backcnt].L1Status == 0) {

                document.getElementById('l1decision').innerHTML = ""
                document.getElementById('L1rejectDecrp').innerHTML = ""
            }
        }

    });
    //--------------Close---------------------------------------
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
                    //  arrlist.push(orderData.ActualAmount);
                    arrlist.push(orderData.Date);
                    //arrlist.push(orderData.EntrySerialNo);
                    //arrlist.push(orderData.EntryPayorBankRoutNo);
                    //arrlist.push(orderData.EntrySAN);
                    //arrlist.push(orderData.EntryTransCode);
                    arrlist.push(orderData.L2Opsts);
                    arrlist.push(orderData.OpsStatus);
                    arrlist.push(orderData.XMLAmount);
                    arrlist.push(orderData.XMLSerialNo);
                    arrlist.push(orderData.XMLPayorBankRoutNo);
                    arrlist.push(orderData.XMLSAN);
                    arrlist.push(orderData.XMLTransCode);
                    arrlist.push(orderData.RejectReason);
                    arrlist.push(orderData.L2Rejectreason);
                    arrlist.push(orderData.CBSClientAccountDtls);
                    arrlist.push(orderData.CBSJointHoldersName);
                    arrlist.push(orderData.L1Status);
                    arrlist.push(orderData.DbtAccountNoOld);
                    arrlist.push(orderData.DateOld);

                    arrlist.push(orderData.PresentingBankRoutNo);
                    arrlist.push(orderData.DocType);
                    arrlist.push(orderData.XMLMICRRepairFlags);

                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwL1" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.DbtAccountNo);
                    //  arrlist.push(orderData.ActualAmount);
                    arrlist.push(orderData.Date);
                    //arrlist.push(orderData.EntrySerialNo);
                    //arrlist.push(orderData.EntryPayorBankRoutNo);
                    //arrlist.push(orderData.EntrySAN);
                    //arrlist.push(orderData.EntryTransCode);
                    arrlist.push(orderData.L2Opsts);
                    arrlist.push(orderData.OpsStatus);
                    arrlist.push(orderData.XMLAmount);
                    arrlist.push(orderData.XMLSerialNo);
                    arrlist.push(orderData.XMLPayorBankRoutNo);
                    arrlist.push(orderData.XMLSAN);
                    arrlist.push(orderData.XMLTransCode);
                    arrlist.push(orderData.RejectReason);
                    arrlist.push(orderData.L2Rejectreason);
                    arrlist.push(orderData.CBSClientAccountDtls);
                    arrlist.push(orderData.CBSJointHoldersName);
                    arrlist.push(orderData.L1Status);
                    arrlist.push(orderData.DbtAccountNoOld);
                    arrlist.push(orderData.DateOld);

                    arrlist.push(orderData.PresentingBankRoutNo);
                    arrlist.push(orderData.DocType);
                    arrlist.push(orderData.XMLMICRRepairFlags);
                }
            }
            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(pcnt);
            $.ajax({

                url: RootUrl + 'IWL2/IWl2',
                data: JSON.stringify({ lst: arrlist, snd: scond, btnClose: "Close", idlst: idslst }),

                type: 'POST',
                contentType: 'application/json; charset=utf-8',

                dataType: 'json',
                success: function (result) {
                    if (result == false) {
                        window.location = RootUrl + 'IWL2/Selection';
                    }
                }

            });
        }
    });
    //------------------------------------------------------------
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
    //---------------- Data Entry -----------------------------------

    $("form input").keydown(function (e) {

        next_idx = $('input[type=text]').index(this) + 1;
        tot_idx = $('body').find('input[type=text]').length;
        //if (next_idx == 2) {
        //    cbsbtn = false
        //}
        if (next_idx == 3) {
            next_idx = tot_idx;
        }
        else {
            next_idx = 2;
        }
        if (e.keyCode == 13) {

            if (tot_idx == next_idx) {

                $("input[value='Ok']").click();
            }
            else if (next_idx == 2) {
                var tempccnt = $("#cnt").val();
                var tempacntno;
                if (document.getElementById('oldact').value == "") {
                    tempacntno = tt[tempccnt].DbtAccountNo;
                }
                else {
                    tempacntno = document.getElementById('oldact').value;
                }

                if ($("#accnt").val() == "") {
                    alert("Please enter account number!!");
                    return false;
                }
                if (tempacntno != $("#accnt").val()) {

                    document.getElementById('oldact').value = $("#accnt").val();
                    getcbsdtls();
                }
                else {

                    next_idx = 2;
                    $('input[type=text]:eq(' + next_idx + ')').focus().select();
                }
            }

            else {
                $('input[type=text]:eq(' + next_idx + ')').focus().select();

            }
        }
    });
    //---
    $("#accnt").keypress(function (event) {

        // alert(event.keyCode);
        //debugger;
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

    $("#ChqnoQC,#SortQC,#SANQC,#TransQC,#ChqDate,#IWRemark").keypress(function (event) {

        // alert(event.keyCode);
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
        //debugger;

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

        if (event.keyCode == 113) {
            windowcall(); //Sign
            return false;
        }
    });
    //----------------------IwRemark(Reject reason)
    //$("#IWRemark").keydown(function (event) {
    //    //alert(event.keyCode);
    //    var chr = document.getElementById('IWDecision').value.toUpperCase();
    //    if (event.shiftKey) {
    //        event.preventDefault();
    //    }

    //    if (chr == "R") {
    //        if ((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode > 95 && event.keyCode < 106) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
    //        }
    //        else {
    //            event.preventDefault(); // || event.keyCode == 68 || event.keyCode == 100 || event.keyCode == 112 
    //        }
    //    }
    //    if (event.keyCode == 114) {
    //        getreason();
    //    }
    //});

    $("#IWRemark").keyup(function (event) {
        var chkcode = false;
        if ((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode > 95 && event.keyCode < 106) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            event.preventDefault(); // || event.keyCode == 68 || event.keyCode == 100 || event.keyCode == 112 
        }
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
        else {
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

    //----------Amount---------
    $("#iwAmt").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || event.keyCode == 46 || (event.charCode > 47 && event.charCode < 58)) {

            var amtval = $("#iwAmt").val();
            if (amtval.length > 0) {
                var splitstr = amtval.split('.');
                // debugger;
                if (splitstr[1] != null) {
                    var strlength = splitstr[1].length;
                    if (strlength > 1) {
                        event.preventDefault();
                    }
                }
            }
        }
        else {
            // alert('else');
            //if (event.keyCode < 95) {
            //if (event.keyCode == 32 || event.keyCode < 48 || (event.keyCode > 57)) {
            event.preventDefault();
            //  }
        }
    });


    $("#IWRemark").keyup(function (event) {

        var rejctrcd = $("#IWRemark").val();
        if (rejctrcd.length == 2) {
            var rjctresnlTemp = document.getElementById('rtnlist');
            var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
            for (var i = 0; i < rjctresnlTemp.length; i++) {
                if (rejctrcd == rjctresnlTemp[i].value) {
                    document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
                    break;
                }
            }
        }
        else {
            document.getElementById("rejectreasondescrpsn").value = "";
        }
    });

    //-------------------------------Common For Accept and Reject----------------
    function common(val) {

        //------------------ Added On 19/05/2017-----
        document.getElementById("waittime").value = "1";
        //---------------------------

        var tempdat;
        // var yr, mm, dd, fnldate;
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
        // alert(cnt);
        if (cnt < tt.length) {
            //  alert(tt.length);
            //  alert(tt[cnt].FrontGreyImagePath);
            document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
            //--------------Filling value from OCR1--------------
            document.getElementById('accnt').value = "";
            document.getElementById('ChqDate').value = "";
            document.getElementById('iwAmt').innerHTML = "";
            document.getElementById('ChqnoQC').innerHTML = "";
            document.getElementById('SortQC').innerHTML = "";
            document.getElementById('SANQC').innerHTML = "";
            document.getElementById('TransQC').innerHTML = "";

            //document.getElementById("h2amt").innerHTML = "";
            document.getElementById("cbsdetails").innerHTML = "";
            document.getElementById('IWDecision').value = "";
            document.getElementById('IWRemark').value = "";
            document.getElementById('rtncd').style.display = "none";
            document.getElementById("rejectreasondescrpsn").value = "";
            // document.getElementById("cbsdiv").innerHTML = "";
            document.getElementById('Acdtls').innerHTML = "";
            document.getElementById('Micrdtls').innerHTML = "";
            document.getElementById('bankname').innerHTML = "";
            document.getElementById('IWDecision').focus();
            document.getElementById('oldact').value = "";
            cbsbtn = false;
            //------------------------------------------------------------

            //document.getElementById("p2").style.color = "blue";
            if (tt[cnt].Date.length > 6) {
                tempdat = tt[cnt].Date.split("-");
                yr = tempdat[0];
                yr = yr.substring(2, 4);
                mm = tempdat[1];
                dd = tempdat[2];
                fnldate = dd + mm + yr;
            }
            else {
                tempdat = tt[cnt].Date;
                yr = tempdat.substring(4, 6);
                mm = tempdat.substring(2, 4);
                dd = tempdat.substring(0, 2);
                fnldate = dd + mm + yr;
            }

            document.getElementById('ChqnoQC').innerHTML = tt[cnt].XMLSerialNo;
            document.getElementById('SortQC').innerHTML = tt[cnt].XMLPayorBankRoutNo;
            document.getElementById('SANQC').innerHTML = tt[cnt].XMLSAN;
            document.getElementById('TransQC').innerHTML = tt[cnt].XMLTransCode;
            document.getElementById('accnt').value = tt[cnt].DbtAccountNo;
            document.getElementById('ChqDate').value = fnldate;
            document.getElementById('iwAmt').innerHTML = addCommas(tt[cnt].XMLAmount);//added on 12-05-2017; tt[cnt].XMLAmount;
            document.getElementById('mtrn').value = tt[cnt].ID;

            document.getElementById('oldact').value = tt[cnt].DbtAccountNo;
            signaturecall = false;

            if (tt[cnt].DocType == "C") {
                document.getElementById('doctype').innerHTML = "P2F Indicater : Y";
            }
            else {
                document.getElementById('doctype').innerHTML = "P2F Indicater : N";
            }
            //-----------------Presenting Bank Code----------------
            document.getElementById('presenting').innerHTML = "Presenting Bank : " + tt[cnt].PresentingBankRoutNo

            //alert(tt[cnt].XMLMICRRepairFlags);
            //------------------------MICR Flags-----------------------------
            if (tt[cnt].XMLMICRRepairFlags != null || tt[cnt].XMLMICRRepairFlags == "") {

                $("#Acdtls").removeClass();
                $("#Micrdtls").removeClass();

                if (tt[cnt].XMLMICRRepairFlags.substring(4, 5) == "0") {
                    document.getElementById('Acdtls').innerHTML = " Not Verified";//Credit A/C :
                    document.getElementById("Acdtls").classList.add("w3-highway-red");
                }
                else if (tt[cnt].XMLMICRRepairFlags.substring(4, 5) == "5") {
                    document.getElementById('Acdtls').innerHTML = " Old Account";//Credit A/C :
                    document.getElementById("Acdtls").classList.add("w3-highway-green");
                }
                else if (tt[cnt].XMLMICRRepairFlags.substring(4, 5) == "9") {
                    document.getElementById('Acdtls').innerHTML = " New Account";//Credit A/C :
                    document.getElementById("Acdtls").classList.add("w3-highway-green");
                }
                if (tt[cnt].XMLMICRRepairFlags.substring(0, 4) == "1111" && tt[cnt].XMLMICRRepairFlags.substring(5, 6) == "1") {
                    document.getElementById('Micrdtls').innerHTML = " MICR Modified-" + tt[cnt].XMLMICRRepairFlags;//MICR Status:
                    document.getElementById("Micrdtls").classList.add("w3-highway-red");
                }
                else {
                    document.getElementById('Micrdtls').innerHTML = " MICR Not Modified-" + tt[cnt].XMLMICRRepairFlags;//MICR Status:
                    document.getElementById("Micrdtls").classList.add("w3-highway-green");
                }
            }

            //---------------Call Cbs Details--------
            document.getElementById('l1decision').innerHTML = ""
            document.getElementById('L1rejectDecrp').innerHTML = ""

            autogetcbsdtls(tt[cnt].CBSClientAccountDtls, tt[cnt].CBSJointHoldersName, tt[cnt].DbtAccountNo);// commented for cbi
            bankName(tt[cnt].PresentingBankRoutNo);  //-------------For bank name
            //------------------L1 Reject---------------
            // debugger;
            if (tt[cnt].L1Status == 1 || tt[cnt].L1Status == 2 || tt[cnt].L1Status == 5) {
                //document.getElementById('l1dec').style.display = "";
                if (tt[cnt].L1Status == 1) {
                    $("#l1decision").removeClass();
                    document.getElementById('l1decision').innerHTML = "Y"
                    document.getElementById("l1decision").classList.add("w3-text-green");
                    //document.getElementById("l1dec").classList.add("w3-panel w3-green");
                    document.getElementById('L1rejectDecrp').style.display = "none";
                }

                if (tt[cnt].L1Status == 2 || tt[cnt].L1Status == 5) {
                    $("#l1decision").removeClass();
                    document.getElementById('l1decision').innerHTML = "R"
                    document.getElementById("l1decision").classList.add("w3-text-red");

                    document.getElementById('L1rejectDecrp').style.display = "";

                    getReturnDecrp(tt[cnt].RejectReason);
                }
            }
            else if (tt[cnt].L1Status == 0) {

                document.getElementById('l1decision').innerHTML = ""
                document.getElementById('L1rejectDecrp').innerHTML = ""
            }

            if (backbtn == true) {
                document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
            }
            else {
                document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
            }
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
                        //  arrlist.push(orderData.ActualAmount);
                        arrlist.push(orderData.Date);
                        //arrlist.push(orderData.EntrySerialNo);
                        //arrlist.push(orderData.EntryPayorBankRoutNo);
                        //arrlist.push(orderData.EntrySAN);
                        //arrlist.push(orderData.EntryTransCode);
                        arrlist.push(orderData.L2Opsts);
                        arrlist.push(orderData.OpsStatus);
                        arrlist.push(orderData.XMLAmount);
                        arrlist.push(orderData.XMLSerialNo);
                        arrlist.push(orderData.XMLPayorBankRoutNo);
                        arrlist.push(orderData.XMLSAN);
                        arrlist.push(orderData.XMLTransCode);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.L2Rejectreason);
                        arrlist.push(orderData.CBSClientAccountDtls);
                        arrlist.push(orderData.CBSJointHoldersName);
                        arrlist.push(orderData.L1Status);
                        arrlist.push(orderData.DbtAccountNoOld);
                        arrlist.push(orderData.DateOld);

                        arrlist.push(orderData.PresentingBankRoutNo);
                        arrlist.push(orderData.DocType);
                        arrlist.push(orderData.XMLMICRRepairFlags);
                    }
                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwL1" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.DbtAccountNo);
                        //  arrlist.push(orderData.ActualAmount);
                        arrlist.push(orderData.Date);
                        //arrlist.push(orderData.EntrySerialNo);
                        //arrlist.push(orderData.EntryPayorBankRoutNo);
                        //arrlist.push(orderData.EntrySAN);
                        //arrlist.push(orderData.EntryTransCode);
                        arrlist.push(orderData.L2Opsts);
                        arrlist.push(orderData.OpsStatus);
                        arrlist.push(orderData.XMLAmount);
                        arrlist.push(orderData.XMLSerialNo);
                        arrlist.push(orderData.XMLPayorBankRoutNo);
                        arrlist.push(orderData.XMLSAN);
                        arrlist.push(orderData.XMLTransCode);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.L2Rejectreason);
                        arrlist.push(orderData.CBSClientAccountDtls);
                        arrlist.push(orderData.CBSJointHoldersName);
                        arrlist.push(orderData.L1Status);
                        arrlist.push(orderData.DbtAccountNoOld);
                        arrlist.push(orderData.DateOld);

                        arrlist.push(orderData.PresentingBankRoutNo);
                        arrlist.push(orderData.DocType);
                        arrlist.push(orderData.XMLMICRRepairFlags);
                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------
                // alert(cnt);
                next_idx = 0;
                tot_idx = 0;
                // var pcnt = cnt;
                //alert(pcnt);
                $.ajax({

                    url: RootUrl + 'IWL2/IWl2',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[cnt - 1].FrontGreyImagePath, idlst: idslst }),

                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',

                    dataType: 'json',
                    success: function (result) {
                        if (result == false) {
                            window.location = RootUrl + 'IWL2/Selection';
                        }
                        else {
                            tt = result;
                            // alert('tt- ' + tt.length);
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

                                // debugger;
                                //-------------Saving Last data in storage---
                                var iwL1 = "iwL10"
                                var L1 = {
                                    "DbtAccountNo": tt[0].DbtAccountNo,
                                    // "ActualAmount": tt[0].ActualAmount,
                                    "Date": tt[0].Date,
                                    //"EntrySerialNo": tt[0].EntrySerialNo,
                                    //"EntryPayorBankRoutNo": tt[0].EntryPayorBankRoutNo,
                                    //"EntrySAN": tt[0].EntrySAN,
                                    //"EntryTransCode": tt[0].EntryTransCode,
                                    "ID": tt[0].ID,
                                    "L2Opsts": cnrslt,
                                    "OpsStatus": tt[0].OpsStatus,
                                    "XMLAmount": tt[0].XMLAmount,
                                    "XMLSerialNo": tt[0].XMLSerialNo,
                                    "XMLPayorBankRoutNo": tt[0].XMLPayorBankRoutNo,
                                    "XMLSAN": tt[0].XMLSAN,
                                    "XMLTransCode": tt[0].XMLTransCode,
                                    "RejectReason": tt[0].RejectReason,
                                    "L2Rejectreason": tt[0].L2Rejectreason,
                                    "CBSClientAccountDtls": tt[0].CBSClientAccountDtls,
                                    "CBSJointHoldersName": tt[0].CBSJointHoldersName,
                                    "L1Status": tt[0].L1Status,
                                    "DbtAccountNoOld": tt[0].DbtAccountNoOld,
                                    "DateOld": tt[0].DateOld,
                                    "PresentingBankRoutNo": tt[0].PresentingBankRoutNo,
                                    "DocType": tt[0].DocType,
                                    "XMLMICRRepairFlags": tt[0].XMLMICRRepairFlags,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqiwmicr = JSON.stringify(L1);
                                    localacct.setItem(iwL1, chqiwmicr);

                                }
                                // alert(tt[1].FrontGreyImagePath);
                                // debugger;
                                //----------------------------------------------------------------------//
                                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                document.getElementById('accnt').value = "";
                                document.getElementById('ChqDate').value = "";
                                document.getElementById('iwAmt').innerHTML = "";
                                document.getElementById('ChqnoQC').innerHTML = "";
                                document.getElementById('SortQC').innerHTML = "";
                                document.getElementById('SANQC').innerHTML = "";
                                document.getElementById('TransQC').innerHTML = "";

                                // document.getElementById("h2amt").innerHTML = "";
                                document.getElementById("cbsdetails").innerHTML = "";
                                document.getElementById('IWDecision').value = "";
                                document.getElementById('IWRemark').value = "";
                                document.getElementById('rtncd').style.display = "none";
                                document.getElementById("rejectreasondescrpsn").value = "";
                                signaturecall = false;
                                //----
                                if (tt[1].Date.length > 6) {
                                    tempdat = tt[1].Date.split("-");
                                    yr = tempdat[0];
                                    yr = yr.substring(2, 4);
                                    mm = tempdat[1];
                                    dd = tempdat[2];
                                    fnldate = dd + mm + yr;
                                }
                                else {
                                    tempdat = tt[1].Date;
                                    yr = tempdat.substring(4, 6);
                                    mm = tempdat.substring(2, 4);
                                    dd = tempdat.substring(0, 2);
                                    fnldate = dd + mm + yr;
                                }

                                document.getElementById('ChqnoQC').innerHTML = tt[1].XMLSerialNo;
                                document.getElementById('SortQC').innerHTML = tt[1].XMLPayorBankRoutNo;
                                document.getElementById('SANQC').innerHTML = tt[1].XMLSAN;
                                document.getElementById('TransQC').innerHTML = tt[1].XMLTransCode;
                                document.getElementById('ChqDate').value = fnldate;
                                document.getElementById("iwAmt").innerHTML = addCommas(tt[1].XMLAmount);//added on 12-05-2017;tt[1].XMLAmount;
                                document.getElementById('accnt').value = tt[1].DbtAccountNo;
                                document.getElementById('mtrn').value = tt[1].ID;

                                document.getElementById('Acdtls').innerHTML = "";
                                document.getElementById('Micrdtls').innerHTML = "";
                                document.getElementById('bankname').innerHTML = "";
                                document.getElementById('IWDecision').focus();
                                document.getElementById('oldact').value = tt[1].DbtAccountNo;
                                //-----------------P2F Indicator----------------

                                if (tt[1].DocType == "C") {
                                    document.getElementById('doctype').innerHTML = "P2F Indicater : Y";
                                }
                                else {
                                    document.getElementById('doctype').innerHTML = "P2F Indicater : N";
                                }
                                //-----------------Presenting Bank Code----------------
                                document.getElementById('presenting').innerHTML = "Presenting Bank : " + tt[1].PresentingBankRoutNo

                                //------------------------MICR Flags-----------------------------
                                if (tt[1].XMLMICRRepairFlags != null || tt[1].XMLMICRRepairFlags == "") {

                                    $("#Acdtls").removeClass();
                                    $("#Micrdtls").removeClass();

                                    if (tt[1].XMLMICRRepairFlags.substring(4, 5) == "0") {
                                        document.getElementById('Acdtls').innerHTML = " Not Verified";//Credit A/C :
                                        document.getElementById("Acdtls").classList.add("w3-highway-red");
                                    }
                                    else if (tt[1].XMLMICRRepairFlags.substring(4, 5) == "5") {
                                        document.getElementById('Acdtls').innerHTML = " Old Account";//Credit A/C :
                                        document.getElementById("Acdtls").classList.add("w3-highway-green");
                                    }
                                    else if (tt[1].XMLMICRRepairFlags.substring(4, 5) == "9") {
                                        document.getElementById('Acdtls').innerHTML = " New Account";//Credit A/C :
                                        document.getElementById("Acdtls").classList.add("w3-highway-green");
                                    }
                                    if (tt[1].XMLMICRRepairFlags.substring(0, 4) == "1111" && tt[1].XMLMICRRepairFlags.substring(5, 6) == "1") {
                                        document.getElementById('Micrdtls').innerHTML = " MICR Modified-" + tt[1].XMLMICRRepairFlags;//MICR Status:
                                        document.getElementById("Micrdtls").classList.add("w3-highway-red");
                                    }
                                    else {
                                        document.getElementById('Micrdtls').innerHTML = " MICR Not Modified-" + tt[1].XMLMICRRepairFlags;//MICR Status:
                                        document.getElementById("Micrdtls").classList.add("w3-highway-green");
                                    }
                                }

                                //---------------Setting focus to textbox----------------//
                                //---------------Call Cbs Details--------
                                cbsbtn = false;
                                document.getElementById('l1decision').innerHTML = ""
                                document.getElementById('L1rejectDecrp').innerHTML = ""

                                autogetcbsdtls(tt[1].CBSClientAccountDtls, tt[1].CBSJointHoldersName, tt[1].DbtAccountNo);// commenetd for cbi
                                bankName(tt[1].PresentingBankRoutNo);  //-------------For bank name

                                //------------------L1 Reject---------------
                                if (tt[1].L1Status == 1 || tt[1].L1Status == 2 || tt[1].L1Status == 5) {

                                    if (tt[1].L1Status == 1) {
                                        $("#l1decision").removeClass();
                                        document.getElementById('l1decision').innerHTML = "Y"

                                        document.getElementById("l1decision").classList.add("w3-text-green");
                                        document.getElementById('L1rejectDecrp').style.display = "none";
                                    }

                                    if (tt[1].L1Status == 2 || tt[1].L1Status == 5) {
                                        $("#l1decision").removeClass();
                                        document.getElementById('l1decision').innerHTML = "R"

                                        document.getElementById("l1decision").classList.add("w3-text-red");

                                        document.getElementById('L1rejectDecrp').style.display = "";


                                        getReturnDecrp(tt[1].RejectReason);
                                    }
                                }
                                else if (tt[1].L1Status == 0) {

                                    document.getElementById('l1decision').innerHTML = ""
                                    document.getElementById('L1rejectDecrp').innerHTML = ""
                                }
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

    //autogetcbsdtls = function (cbs, joint) {
    //    $.ajax({
    //        url: RootUrl + 'IWL2/GetCBSDtls',
    //        dataType: 'html',
    //        data: { strcbsdls: cbs, strJoinHldrs: joint },
    //        success: function (data) {
    //            //alert(data);
    //            $('#cbsdetails').html(data);
    //            //$('#dialogEditUser').dialog('open');
    //        }
    //    });
    //}


    //-----------------Dialog box----------------activitylogs

    //-------------
    getcbsdtls = function () {
        // alert('Well!!');
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
                url: RootUrl + 'IWL2/GetCBSDtls',
                dataType: 'html',
                type: 'POST',
                data: { ac: $("#accnt").val() },
                success: function (data) {
                    //alert(data);
                    cbsbtn = true;
                    // signaturecall = false;
                    $('#cbsdetails').html(data);

                }
            });
        }
    }


});

function bankName(bankcode) {
    // alert('hello');
    $.ajax({
        url: RootUrl + 'IWL2/GetBankName',
        dataType: 'html',
        data: { bankcode: bankcode },
        success: function (databank) {
            //alert(data);
            $('#bankname').html(databank);
            //$('#dialogEditUser').dialog('open');
        }
    });
}

function autogetcbsdtls(cbs, joint, acno) {
    // alert('hello');
    $.ajax({
        url: RootUrl + 'IWL2/GetCBSDtls',
        dataType: 'html',
        type: 'POST',
        data: { ac: acno, strcbsdls: cbs, strJoinHldrs: joint },
        success: function (data) {
            cbsbtn = true;
            //signaturecall = false;
            $('#cbsdetails').html(data);
            //$('#dialogEditUser').dialog('open');
        }
    });
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
//-------------------
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


//--------------------------Signature fetching----
function windowcall() {

    signaturecall = true;

    var url = document.getElementById('sign').value;
    url = url + '=' + document.getElementById('accnt').value;
    // document.getElementById('signchk').value = "Y";
    window.open(url, 'Signature', 'width=500,height=500,left=900,scrollbars=yes,titlebar=yes,resizable=no,location=yes,toolbar=0,status=1').focus();
}


//-----------------------Activity Logs----------------
function getIwlogs() {

    $("#activitylogs").dialog({
        draggable: true,
        height: 500,
        width: 600,
        position: { my: "Center Top", at: "center center", of: window },

    });

    $.ajax({
        url: RootUrl + 'IWL2/getIwlogs',
        dataType: 'html',
        data: { id: $("#mtrn").val() },
        success: function (data) {
            //alert(data);
            $('#activitylogs').html(data);
            $('#activitylogs').dialog('open');
        }
    });

}

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

//function getreason() {

//    $("#dialogEditUser").dialog({
//        draggable: true,
//        height: 400,
//        width: 500,
//        position: { my: "Center Top", at: "center center", of: window },
//        buttons: [
//        {
//            text: "minimize",
//            click: function () {
//                $(this).parents('.ui-dialog').animate({
//                    height: '40px',
//                    top: $(window).height() - 40
//                }, 400);
//            }
//        }]
//    });

//    $.ajax({
//        url: RootUrl + 'IWL2/RejectReason',
//        dataType: 'html',
//        data: { id: 0 },
//        success: function (data) {
//            //alert(data);
//            $('#dialogEditUser').html(data);
//            $('#dialogEditUser').dialog('open');
//        }
//    });

//}

function IWLICQC() {

    var IWdecn = document.getElementById('IWDecision').value.toUpperCase();
    //----------------DBTAC-----------------------
    var Acct = document.getElementById('accnt').value;
    var acmin = document.getElementById('acmin').value;
    //Acct = Acct.replace(/^0+/, '') 
    var tempacntno = document.getElementById('oldact').value;
    if ($("#accnt").val() == "") {
        alert("Please enter account number!!")
        return false;
    }
    //debugger;
    if (tempacntno != $("#accnt").val()) {

        document.getElementById('oldact').value = $("#accnt").val();
        cbsbtn = false
        // getcbsdtls();
    }
    if (document.getElementById("blockkey").value == "1" && IWdecn == "A") {
        alert('You can not accept this cheque!');
        return false;
    } // || event.keyCode == 99
    ///---------------Added on 19-04-2017---------------
    var prevval = "";
    var nextval;
    var index = 0;
    //debugger;
    if (Acct.length > acmin) {
        //for (var i = 0; i < acmin; i++) {
        //    nextval = "";
        //    nextval = Acct.charAt(i);
        //    if (prevval != "") {
        //        if (prevval == nextval) {
        //            prevval = nextval;
        //            index = index + 1;
        //        }
        //        else {
        //            index = 0;
        //        }
        //    }
        //    else {
        //        prevval = nextval
        //    }   Allowcase
        //}
        if (Acct != "9999999999999999" && IWdecn == "R" && document.getElementById("blockkey").value == "1" && document.getElementById("Allowcase").value != "1") {
            alert("If account is invalid on cheque then please reject with 16 times 9(9999999999999999)!");
            return false;
        }
    }
    //----------------------------------
    var Accval = Acct;
    Acct = Acct.length
    if (signaturecall == false) {
        alert("Please press F2 for signature!!");
        document.getElementById('IWDecision').focus();
        return false;
    }

    //debugger;
    var tempacnt = $("#cnt").val();//documemt.getElementById('').value;


    if (tt[tempacnt].DbtAccountNo != $("#accnt").val() && cbsbtn == false) {
        alert("Click on 'GetDetails' button/press F12 !");
        document.getElementById('accnt').focus();
        return false;
    }

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

    //-------------------------------ChqDate-------------------------------------------------//

    var dd, mm, yy;
    var finldat;
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
    else if (IWdecn == "A" && dat == "000000") {
        alert("Date not valid !");
        document.getElementById('ChqDate').focus();
        document.getElementById('ChqDate').select();
        return false;
    }
        // else if (dat.length < 2 || dat.length == 3 || dat.length == 5) {
    else if (dat.length < 6) {
        alert("Date not valid !");
        document.getElementById('ChqDate').focus();
        document.getElementById('ChqDate').select();
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

    var fnaldate = '20' + yy + '/' + mm + '/' + dd;
    var staldat = new Date(sesondt);
    var postdat = new Date(stlmntdt);
    var d3 = new Date(fnaldate);

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

    //var chqdt = document.getElementById('ChqDate').value;
    //if (chqdt.length <= 0 || chqdt.length < 2) {
    //    alert('Please enter correct Date!');
    //    document.getElementById('ChqDate').focus();
    //    return false;
    //}

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
    else if (IWdecn == "A" && dat == "000000") {
        event.preventDefault();
        alert('You can not accept this cheque\n Please correct date or reject the cheque!');
        return false;
    }
    else if (IWdecn == "R") {
        if (document.getElementById('IWRemark').value == "") {
            alert('Please enter reject reason!');
            document.getElementById('IWRemark').focus();
            return false;
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

function getReturnDecrp(rtncode) {
    //alert(rtncode);
    var rjctresnl = document.getElementById('rtnlist');
    var rtnlistDescrp = document.getElementById('rtnlistDescrp');
    for (var i = 0; i < rjctresnl.length; i++) {
        //  alert(rjctresnl[i].value);
        if (rtncode == rjctresnl[i].value) {
            // alert('aila In');
            document.getElementById('L1rejectDecrp').innerHTML = rtnlistDescrp[i].value;
            break;
        }
    }
}

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
        //
        // alert('Saruche!');

        document.getElementById('rtncd').style.display = "none";

        //var tempPayeeName = document.getElementById('PayeeName').value;
        //// alert(tempPayeeName.substring(0, 5));
        //if ((tempPayeeName.toUpperCase().substring(0, 5) == "NNNNN" || (tempPayeeName.toUpperCase().substring(0, 5) == "00000"))) {
        //    alert('Please Check Payee Name!!!');
        //    return false;
        //}
    }
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




