
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
var Foutcnt;
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
var ChequeAmountTotal = 0;
var tempAmtValue = 0;
var firstcall = false;
var randomPayeeName = "";
var narrationReqirdflg = false;
var narrationMandate = false;
var InstrumentType;
var SlipUserNarration;
var Slipdecision;
var SlipID;
var SlipRawaDataID;
var strModified = "0000000000";
var gclientflag = false;
var tiffimagecall = false;

var custhirlikwth = "";
var custdivnlikwth = "";

function passval(array) {

    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}

var getreason;
var getcbsdtls;
var clientdtls;
var subclientdtls;

var tempdat;
var fnldate;
var yr, mm, dd;

$(document).ready(function () {

    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        //alert('for ' + tt[z].Id);
        idslst.push(tt[z].Id)
    }
    // alert(idslst.length);
    //------------ idslist end----------------//
    $('#accnt,#ClientCd,#slpamount,#Amt,#ChqDate,#ChqnoQC,#SortQC,#SANQC,#TransQC,#IWRemark,#nartext').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    //$(document).bind("contextmenu", function (e) {
    //    e.preventDefault();
    //});

    if (tt.length > 0) {
        debugger;
        document.getElementById('tiffimg').style.display = "none";
	    document.getElementById('myimg').style.display = "block";
        document.getElementById('myimg').src = RootUrl + 'Icons/Loading.jpg';
        //document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

        var frontGreyimgUrl = tt[1].FrontGreyImagePath;
        $.ajax({
            url: frontGreyimgUrl,
            type: 'HEAD',
            error: function () {
                //file not exists
                alert('Image not loaded correctly!!!');
                document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
            },
            success: function () {
                //file exists
                console.log("Front image url success " + frontGreyimgUrl);
                document.getElementById('myimg').src = frontGreyimgUrl;
            }
        });

        document.getElementById('nartext').value = "";
        InstrumentType = tt[1].InstrumentType;

        //$('#IWRemark').val("");
        //document.getElementById('rejectreasondescrpsn').value = "";

        $("#ClientCd").val("");
        $("#txtslpno").val("");
        //-------Remove save objects from browser---//
        window.localStorage.clear();
        //------------ idslist end----------------//
        // debugger;
        //alert(tt[1].CreditAccountNo.toUpperCase());
        document.getElementById('tempinstrtype').value = tt[1].InstrumentType;
        document.getElementById('scanningType').value = tt[1].ScanningType;

        if (tt[1].InstrumentType == "S") {
            document.getElementById('slipDiv1').style.display = "";
            document.getElementById('pickupLocationDiv').style.display = "";
            document.getElementById('chqDiv1').style.display = "none";
            document.getElementById('chqDiv2').style.display = "none";
            document.getElementById('MICR').style.display = "none";

            var slpDate = "";
	    //var sDate = new Date(tt[1].CMSSlipDate);
	    //var yr1 = sDate.getFullYear();
	    //yr = yr1.substring(2, 2);
	    //mm = sDate.getMonth() + 1;
	    //dd = sDate.getDate();
            tempdat = tt[1].CMSSlipDate.split("-");
            yr = tempdat[0];
            yr = yr.substring(2, 4);
            mm = tempdat[1];
            dd = tempdat[2];
            slpDate = dd + mm + yr;
            
            document.getElementById('pickupLocation').value = tt[1].CMSSlipPickupLocationCode;
            document.getElementById('pickupLocationID').value = tt[1].CMSSlipPickupLocationId;
            document.getElementById('slipRefNo').value = tt[1].SlipRefNo;
            document.getElementById('slipDate').value = slpDate;
            document.getElementById('slipAmount').value = tt[1].SlipAmount;
            document.getElementById('numberOfInstrument').value = tt[1].CMSNumberOfInstruments;
            document.getElementById('clientCode').value = tt[1].ClientCode;
            document.getElementById('clientName').value = tt[1].PayeeName;

            //document.getElementById('slpamt').style.display = "";
            //document.getElementById('chqamt').style.display = "none";

            //    debugger;
            //document.getElementById('slpamount').value = addCommas(Number(tt[1].SlipAmount).toFixed(2));
            //alert($("#PayeeName").prop("disabled", true));
            //document.getElementById('sliplabl').style.display = "none";
            //document.getElementById('divctsnoncts').style.display = "none";
            //document.getElementById('lblslpimg').style.display = "none";

        }
        else {
            document.getElementById('slipDiv1').style.display = "none";
            document.getElementById('pickupLocationDiv').style.display = "none";
            document.getElementById('chqDiv1').style.display = "";
            document.getElementById('chqDiv2').style.display = "";
            document.getElementById('MICR').style.display = "";

            //document.getElementById('slpamt').style.display = "none";
            //document.getElementById('chqamt').style.display = "";
            //document.getElementById('Chqacnt').style.display = "";
            //document.getElementById('slpacnt').style.display = "none";
            //document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;

            //document.getElementById('clientPayeeName').value = "";
            //document.getElementById('draweeName').value = "";
            //document.getElementById('chequeDate').value = "";
            //document.getElementById('chequeAmount').value = "";
            //document.getElementById('chequeRemarks').value = "";
            //document.getElementById('ChqnoQC').value = "";
            //document.getElementById('SortQC').value = "";
            //document.getElementById('SANQC').value = "";
            //document.getElementById('TransQC').value = "";

            document.getElementById('ChqnoQC').value = tt[1].ChequeNoFinal;
            document.getElementById('SortQC').value = tt[1].SortCodeFinal;
            document.getElementById('SANQC').value = tt[1].SANFinal;
            document.getElementById('TransQC').value = tt[1].TransCodeFinal;
            //document.getElementById('Slipamt').innerHTML = tt[1].SlipAmount;
            //document.getElementById('sliplabl').style.display = "";

            //document.getElementById('divctsnoncts').style.display = "";
            //document.getElementById('lblslpimg').style.display = "";

            //$('#ctsnocts').val(tt[1].ClearingType);
        }
        //--------------------------------------------------------------
        document.getElementById('strbranchcd').innerHTML = tt[1].BranchCode;
        document.getElementById('ScanningID').innerHTML = tt[1].ScanningNodeId;
        document.getElementById('strBatchNo').innerHTML = tt[1].BatchNo;
        document.getElementById('strBatchSeqNO').innerHTML = tt[1].BatchSeqNo;
        //--------------------------------------------------------------
        //document.getElementById("btnback").disabled = true
        document.getElementById('rejectreasondescrpsn').value = "";
    }

    $("#ok").click(function () {
        debugger;
        nextcall = false;

        var result = IWLICQC();
        //var result = true;

        if (result == false) {
            //disableEnablefield('E');
            return false;
        }
        else {
            var owL1 = "owL1";
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var btnval = document.getElementById('Decision').value.toUpperCase();

            if (btnval == "R") {
                cnrslt = false;

                //-------------------------valid Function for validation---------------
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
                cnrslt = true;
                nextcall = true;
            }

            debugger;
            owL1 = owL1 + cnt;

            if (InstrumentType == "S") {

                Slipdecision = btnval;
                SlipID = tt[tempcnt].Id;
                SlipRawaDataID = tt[tempcnt].RawDataId;

                L1 = {
                    "Id": tt[tempcnt].Id,
                    "CustomerId": tt[tempcnt].CustomerId,
                    "DomainId": tt[tempcnt].DomainId,
                    "BranchCode": tt[tempcnt].BranchCode,
                    "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                    "BatchNo": tt[tempcnt].BatchNo,
                    "ProcessingDate": tt[tempcnt].ProcessingDate,
                    "InstrumentType": tt[tempcnt].InstrumentType,
                    "ScanningType": tt[tempcnt].ScanningType,
                    "SlipNo": tt[tempcnt].SlipNo,
                    "RawDataId": tt[tempcnt].RawDataId,
                    "ClientCode": $("#clientCode").val(),
                    "PickupLocationCode": $("#pickupLocation").val(),
                    "PickupLocationId": $("#pickupLocationID").val(),
                    "SlipRefNo": $("#slipRefNo").val(),
                    "CreditAccountNo": $("#clientAccountNo").val(),
                    "SlipAmount": parseFloat($("#slipAmount").val().replace(/,/g, '')),
                    "SlipDate": $("#slipDate").val(),

                    "FinalAmount": "",
                    "FinalDate": "",
                    "ChequeNoFinal": "",
                    "SortCodeFinal": "",
                    "SANFinal": "",
                    "TransCodeFinal": "",

                    "NoOfInstrument": $("#numberOfInstrument").val(),

                    "PayeeName": $("#clientName").val(), // $("#Payee").val(),
                    "DraweeName": "", // $("#Payee").val(),
                    "UserNarration": "",
                    "SlipID": SlipID,
                    "SlipRawaDataID": SlipRawaDataID,
                    "Action": btnval,
                    "RejectReason": $("#IWRemark").val(),
                    "rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                    "Slipdecision": Slipdecision,

                    //"ChequeAmountTotal": tt[tempcnt].ChequeAmountTotal,
                    //"SlipUserNarration": SlipUserNarration,
                    //"Slipdecision": Slipdecision,
                    //"rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                    //"ctsNonCtsMark": $("#ctsnocts").val(),
                    //"Modified": strModified,
                    //"LICNo": $("#txtslpno").val(),
                    //"custhirlikwth": custhirlikwth,
                    //"custdivnlikwth": custdivnlikwth,

                };

            }
            else {

                L1 = {
                    "Id": tt[tempcnt].Id,
                    "CustomerId": tt[tempcnt].CustomerId,
                    "DomainId": tt[tempcnt].DomainId,
                    "BranchCode": tt[tempcnt].BranchCode,
                    "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                    "BatchNo": tt[tempcnt].BatchNo,
                    "ProcessingDate": tt[tempcnt].ProcessingDate,
                    "InstrumentType": tt[tempcnt].InstrumentType,
                    "ScanningType": tt[tempcnt].ScanningType,
                    "SlipNo": tt[tempcnt].SlipNo,
                    "RawDataId": tt[tempcnt].RawDataId,
                    "ClientCode": "",
                    "PickupLocationCode": "",
                    "PickupLocationId": "",
                    "SlipRefNo": "",
                    "CreditAccountNo": "",
                    "SlipAmount": "",
                    "SlipDate": "",

                    "FinalAmount": parseFloat($("#chequeAmount").val().replace(/,/g, '')),
                    "FinalDate": $("#chequeDate").val(),
                    "ChequeNoFinal": $("#ChqnoQC").val(),
                    "SortCodeFinal": $("#SortQC").val(),
                    "SANFinal": $("#SANQC").val(),
                    "TransCodeFinal": $("#TransQC").val(),

                    "NoOfInstrument": "",

                    "PayeeName": $("#clientPayeeName").val(), // $("#Payee").val(),
                    "DraweeName": $("#draweeName").val(), // $("#Payee").val(),
                    "UserNarration": $("#chequeRemarks").val(),
                    "SlipID": SlipID,
                    "SlipRawaDataID": SlipRawaDataID,
                    "Action": btnval,
                    "RejectReason": $("#IWRemark").val(),
                    "rejectreasondescrpsn": $("#rejectreasondescrpsn").val(),
                    "Slipdecision": Slipdecision,
                };

            }
            
            

            nextcall = true;
        }

        if (nextcall == true) {

            common(owL1);
        }
        //else {
        //    // alert('Okk');
        //    document.getElementById('accnt').focus();
        //    document.getElementById("btnback").disabled = true;
        //}
    });

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

    $("#Decision").keydown(function (event) {

        // alert(event.keyCode);
        tempcnt = document.getElementById('tempcnt').value;

        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }
        if (document.getElementById("blockkey").value == "1") {
            if (event.keyCode == 65 || event.keyCode == 97) {
                event.preventDefault();
                alert('You can not accept the cheque!');
                return false;
            } // || event.keyCode == 99//refer || event.keyCode == 70
            if (event.keyCode == 65 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
                //if (event.keyCode == 82 || event.keyCode == 114) {
                //    document.getElementById('rtncd').style.display = "";
                //    document.getElementById('IWRemark').style.width = "10%";
                //    document.getElementById('IWRemark').focus();
                //}
                //else {
                //    document.getElementById('rtncd').style.display = "none";
                //}
            }
            else {
                event.preventDefault();
            }
        }
        else {
            if (tt[tempcnt].InstrumentType == 'S') {
                if (tt[1].ScanningType == 3 || tt[1].ScanningType == 5) {
                    if (event.keyCode == 65 || event.keyCode == 70 || event.keyCode == 102 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

                    }
                    else {
                        event.preventDefault();
                    }

                }
                else {
                    if (event.keyCode == 65 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

                    }
                    else {
                        event.preventDefault();
                    }
                }

            }
            else {
                if (event.keyCode == 65 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

                }
                else {
                    event.preventDefault();
                }

            }

            //if (event.keyCode == 65 || event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

            //    //if (event.keyCode == 82 || event.keyCode == 114) {
            //    //    document.getElementById('rtncd').style.display = "";
            //    //    document.getElementById('IWRemark').style.width = "10%";
            //    //    document.getElementById('IWRemark').focus();
            //    //}
            //    //else {
            //    //    document.getElementById('rtncd').style.display = "none";
            //    //}
            //}
            //else {
            //    event.preventDefault();
            //}

        }
    });

    $("#btnClose").click(function () {

        // debugger;

        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;

            if (scond == true) {
                var i;
                if (tt[0].callby == "Cheq") {
                    i = 0;
                }
                else {
                    i = 1;
                }
                for (i = i; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL1" + i));
                    //alert(orderData.Id);
                    if (orderData.Id != null) {
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.BranchCode);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.BatchNo);
                        arrlist.push(orderData.ProcessingDate);
                        arrlist.push(orderData.InstrumentType);
                        arrlist.push(orderData.ScanningType);
                        arrlist.push(orderData.SlipNo);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.ClientCode);
                        arrlist.push(orderData.PickupLocationCode);
                        arrlist.push(orderData.PickupLocationId);
                        arrlist.push(orderData.SlipRefNo);
                        arrlist.push(orderData.CreditAccountNo);
                        arrlist.push(orderData.SlipAmount);
                        arrlist.push(orderData.SlipDate);
                        arrlist.push(orderData.FinalAmount);
                        arrlist.push(orderData.FinalDate);
                        arrlist.push(orderData.ChequeNoFinal);
                        arrlist.push(orderData.SortCodeFinal);
                        arrlist.push(orderData.SANFinal);
                        arrlist.push(orderData.TransCodeFinal);
                        arrlist.push(orderData.NoOfInstrument);
                        arrlist.push(orderData.PayeeName);
                        arrlist.push(orderData.DraweeName);
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.SlipID);
                        arrlist.push(orderData.SlipRawaDataID);
                        arrlist.push(orderData.Slipdecision);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.rejectreasondescrpsn);

                    }

                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL1" + i));
                    if (orderData.Id != null) {
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.BranchCode);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.BatchNo);
                        arrlist.push(orderData.ProcessingDate);
                        arrlist.push(orderData.InstrumentType);
                        arrlist.push(orderData.ScanningType);
                        arrlist.push(orderData.SlipNo);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.ClientCode);
                        arrlist.push(orderData.PickupLocationCode);
                        arrlist.push(orderData.PickupLocationId);
                        arrlist.push(orderData.SlipRefNo);
                        arrlist.push(orderData.CreditAccountNo);
                        arrlist.push(orderData.SlipAmount);
                        arrlist.push(orderData.SlipDate);
                        arrlist.push(orderData.FinalAmount);
                        arrlist.push(orderData.FinalDate);
                        arrlist.push(orderData.ChequeNoFinal);
                        arrlist.push(orderData.SortCodeFinal);
                        arrlist.push(orderData.SANFinal);
                        arrlist.push(orderData.TransCodeFinal);
                        arrlist.push(orderData.NoOfInstrument);
                        arrlist.push(orderData.PayeeName);
                        arrlist.push(orderData.DraweeName);
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.SlipID);
                        arrlist.push(orderData.SlipRawaDataID);
                        arrlist.push(orderData.Slipdecision);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.rejectreasondescrpsn);

                    }

                }
            }
            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(idslst);
            //if (orderData.InstrumentType == "S") {
            $.ajax({

                url: RootUrl + 'OWCMSVerification/OWCMSVerification',
                data: JSON.stringify({ lst: arrlist, snd: scond, btnClose: "Close", idlst: idslst, ChequeAmountTotal: parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) }),

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

});

function common(val) {
    debugger;

    if (Modernizr.localstorage) {
        var localacct = window.localStorage;
        var chqiwmicr = JSON.stringify(L1);
        localacct.setItem(val, chqiwmicr);
    }

    document.getElementById('cnt').value = parseInt(cnt) + 1;
    cnt = document.getElementById('cnt').value;

    if (cnt < tt.length) {
        var Tfimg = document.getElementById('myimg1');
        if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
            document.getElementById('tiffimg').style.display = "none";
            document.getElementById("myimg").style.display = "";
        }

        document.getElementById('myimg').removeAttribute('src');

        document.getElementById('tiffimg').style.display = "none";
	    document.getElementById('myimg').style.display = "block";
        document.getElementById('myimg').src = RootUrl + 'Icons/Loading.jpg';
        //document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;

        var frontGreyimgUrl = tt[cnt].FrontGreyImagePath;
        $.ajax({
            url: frontGreyimgUrl,
            type: 'HEAD',
            error: function () {
                //file not exists
                alert('Image not loaded correctly!!!');
                document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
            },
            success: function () {
                //file exists
                console.log("Front image url success " + frontGreyimgUrl);
                document.getElementById('myimg').src = frontGreyimgUrl;
            }
        });

        document.getElementById('Decision').value = "";
        document.getElementById('IWRemark').value = "";
        document.getElementById('rtncd').style.display = "none";

        document.getElementById('tempinstrtype').value = tt[cnt].InstrumentType;
        document.getElementById('scanningType').value = tt[cnt].ScanningType;

        if (tt[cnt].InstrumentType == "S") {
            document.getElementById('slipDiv1').style.display = "";
            document.getElementById('pickupLocationDiv').style.display = "";
            document.getElementById('chqDiv1').style.display = "none";
            document.getElementById('chqDiv2').style.display = "none";
            document.getElementById('MICR').style.display = "none";

            document.getElementById('pickupLocation').value = "";
            document.getElementById('pickupLocationID').value = "";
            document.getElementById('slipRefNo').value = "";
            document.getElementById('slipDate').value = "";
            document.getElementById('slipAmount').value = "";
            document.getElementById('numberOfInstrument').value = "";
            document.getElementById('clientCode').value = "";
            document.getElementById('clientName').value = "";

            InstrumentType = tt[cnt].InstrumentType;

        }
        else {
            document.getElementById('slipDiv1').style.display = "none";
            document.getElementById('pickupLocationDiv').style.display = "none";
            document.getElementById('chqDiv1').style.display = "";
            document.getElementById('chqDiv2').style.display = "";

            document.getElementById('MICR').style.display = "";

            document.getElementById('clientPayeeName').value = "";
            document.getElementById('draweeName').value = "";
            document.getElementById('chequeDate').value = "";
            document.getElementById('chequeAmount').value = "";
            document.getElementById('chequeRemarks').value = "";
            document.getElementById('ChqnoQC').value = "";
            document.getElementById('SortQC').value = "";
            document.getElementById('SANQC').value = "";
            document.getElementById('TransQC').value = "";
            InstrumentType = tt[cnt].InstrumentType;

            if (tt[cnt].Slipdecision.toUpperCase() == "R") {
                $('#Decision').attr('readonly', true);
                $('#Decision').val("R");
                $('#IWRemark').attr('readonly', true);
                $('#IWRemark').val(tt[cnt].RejectReason);
                document.getElementById('rtncd').style.display = "block";
                $('#rejectreasondescrpsn').attr('readonly', true);
            }
            else {
                $('#Decision').attr('readonly', false);
                $('#Decision').val("");
                $('#IWRemark').attr('readonly', false);
                $('#IWRemark').val("");
                document.getElementById('rtncd').style.display = "none";
            }

            //--------------------------------------------------CheqDte---
            if (tt[cnt].FinalDate != "" || tt[cnt].FinalDate != null) {

                if (tt[cnt].FinalDate.length > 6) {
                    tempdat = tt[cnt].FinalDate.split("-");
                    yr = tempdat[0];
                    yr = yr.substring(2, 4);
                    mm = tempdat[1];
                    dd = tempdat[2];
                    fnldate = dd + mm + yr;
                }
                else {
                    tempdat = tt[cnt].FinalDate;
                    yr = tempdat.substring(4, 6);
                    mm = tempdat.substring(2, 4);
                    dd = tempdat.substring(0, 2);
                    fnldate = dd + mm + yr;
                }
            }
            else {
                fnldate = "";
            }

            document.getElementById('draweeName').value = tt[cnt].DraweeName;
            //document.getElementById('chequeAmount').value = addCommas(Number(tt[cnt].FinalAmount).toFixed(2));
            document.getElementById('chequeAmount').value = tt[cnt].FinalAmount;
            document.getElementById('chequeDate').value = fnldate;
            document.getElementById('clientPayeeName').value = tt[cnt].PayeeName;
            document.getElementById('chequeRemarks').value = tt[cnt].UserNarration;
            document.getElementById('ChqnoQC').value = tt[cnt].ChequeNoFinal;
            document.getElementById('SortQC').value = tt[cnt].SortCodeFinal;
            document.getElementById('SANQC').value = tt[cnt].SANFinal;
            document.getElementById('TransQC').value = tt[cnt].TransCodeFinal;
        }

        //--------------------------------------------------------------
        document.getElementById('strbranchcd').innerHTML = tt[cnt].BranchCode;
        document.getElementById('ScanningID').innerHTML = tt[cnt].ScanningNodeId;
        document.getElementById('strBatchNo').innerHTML = tt[cnt].BatchNo;
        document.getElementById('strBatchSeqNO').innerHTML = tt[cnt].BatchSeqNo;
        //--------------------------------------------------------------

        document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;

        backbtn = false;
    }
    else if (cnt > 0) {
        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;

            if (scond == true) {
                // alert('For if');
                var i;
                if (tt[0].callby == "Cheq") {
                    i = 0;
                }
                else {
                    i = 1;
                }
                for (i = i; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("owL1" + i));
                    //alert(orderData.Id);
                    if (orderData.Id != null) {
                        arrlist.push(orderData.Id);             //======= 0
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.BranchCode);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.BatchNo);
                        arrlist.push(orderData.ProcessingDate);
                        arrlist.push(orderData.InstrumentType);     //===== 7
                        arrlist.push(orderData.ScanningType);
                        arrlist.push(orderData.SlipNo);
                        arrlist.push(orderData.RawDataId);      //======= 10
                        arrlist.push(orderData.ClientCode);
                        arrlist.push(orderData.PickupLocationCode);
                        arrlist.push(orderData.PickupLocationId);
                        arrlist.push(orderData.SlipRefNo);
                        arrlist.push(orderData.CreditAccountNo);    //======= 15
                        arrlist.push(orderData.SlipAmount);
                        arrlist.push(orderData.SlipDate);
                        arrlist.push(orderData.FinalAmount);
                        arrlist.push(orderData.FinalDate);
                        arrlist.push(orderData.ChequeNoFinal);      //======== 20
                        arrlist.push(orderData.SortCodeFinal);
                        arrlist.push(orderData.SANFinal);
                        arrlist.push(orderData.TransCodeFinal);
                        arrlist.push(orderData.NoOfInstrument);
                        arrlist.push(orderData.PayeeName);          //========= 25
                        arrlist.push(orderData.DraweeName);
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.SlipID);
                        arrlist.push(orderData.SlipRawaDataID);     //====== 29
                        arrlist.push(orderData.Slipdecision);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.rejectreasondescrpsn);   //=====33

                    }

                }
            }
            else {

                for (var i = 1; i < cnt; i++) {
                    var orderData = JSON.parse(localData.getItem("owL1" + i));

                    if (orderData.Id != null) {
                        arrlist.push(orderData.Id);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.BranchCode);
                        arrlist.push(orderData.ScanningNodeId);
                        arrlist.push(orderData.BatchNo);
                        arrlist.push(orderData.ProcessingDate);
                        arrlist.push(orderData.InstrumentType);
                        arrlist.push(orderData.ScanningType);
                        arrlist.push(orderData.SlipNo);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.ClientCode);
                        arrlist.push(orderData.PickupLocationCode);
                        arrlist.push(orderData.PickupLocationId);
                        arrlist.push(orderData.SlipRefNo);
                        arrlist.push(orderData.CreditAccountNo);
                        arrlist.push(orderData.SlipAmount);
                        arrlist.push(orderData.SlipDate);
                        arrlist.push(orderData.FinalAmount);
                        arrlist.push(orderData.FinalDate);
                        arrlist.push(orderData.ChequeNoFinal);
                        arrlist.push(orderData.SortCodeFinal);
                        arrlist.push(orderData.SANFinal);
                        arrlist.push(orderData.TransCodeFinal);
                        arrlist.push(orderData.NoOfInstrument);
                        arrlist.push(orderData.PayeeName);
                        arrlist.push(orderData.DraweeName);
                        arrlist.push(orderData.UserNarration);
                        arrlist.push(orderData.SlipID);
                        arrlist.push(orderData.SlipRawaDataID);
                        arrlist.push(orderData.Slipdecision);
                        arrlist.push(orderData.Action);
                        arrlist.push(orderData.RejectReason);
                        arrlist.push(orderData.rejectreasondescrpsn);

                    }

                }
            }

            //------------------------------- Calling Ajax for taking more data------------------
            // alert('finaly');
            next_idx = 0;
            tot_idx = 0;
            var pcnt = cnt;

            $.ajax({

                url: RootUrl + 'OWCMSVerification/OWCMSVerification',
                data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[pcnt - 1].FrontGreyImagePath, idlst: idslst, ChequeAmountTotal: parseFloat(String(ChequeAmountTotal).replace(/,/g, '')) }),

                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                async: false,
                dataType: 'json',
                success: function (result) {
                    debugger;
                    if (result == false) {
                        window.location = RootUrl + 'Home/IWIndex?id=1';
                    }
                    else {
                        tt = result;
                        debugger;
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
                            //-------Remove save objects from browser---//
                            window.localStorage.clear();
                            //------------ idslist end----------------//

                            //-------------Saving Last data in storage---
                            var owL1 = "owL10"

                            InstrumentType = tt[1].InstrumentType;
                            debugger;

                            if (tt[0].callby == "Cheq") {

                                var L1 = {
                                    "CreditAccountNo": tt[0].CreditAccountNo,//$("#accnt").val(),
                                    "SlipAmount": tt[0].SlipAmount,// parseFloat($("#slpamount").val().replace(/,/g, '')),
                                    "BatchNo": tt[0].BatchNo,
                                    "ClearingType": tt[0].ClearingType,
                                    "InstrumentType": tt[0].InstrumentType,
                                    "SlipNo": tt[0].SlipNo,
                                    "ProcessingDate": tt[0].ProcessingDate,
                                    "BranchCode": tt[0].BranchCode,
                                    "ScanningNodeId": tt[0].ScanningNodeId,
                                    "ClientCode": tt[0].ClientCode,
                                    "SlipRefNo": tt[0].SlipRefNo,
                                    "Id": tt[0].Id,
                                    "RawDataId": tt[0].RawDataId,
                                    "DomainId": tt[0].DomainId,
                                    "CustomerId": tt[0].CustomerId,
                                    "FinalAmount": tt[0].FinalAmount,
                                    "FinalDate": tt[0].FinalDate,
                                    "ChequeNoFinal": tt[0].ChequeNoFinal,
                                    "SortCodeFinal": tt[0].SortCodeFinal,
                                    "SANFinal": tt[0].SANFinal,
                                    "TransCodeFinal": tt[0].TransCodeFinal,
                                    "PayeeName": tt[0].PayeeName,
                                    "DraweeName": tt[0].DraweeName,
                                    "UserNarration": tt[0].UserNarration,
                                    "SlipID": tt[0].SlipID,
                                    "SlipRawaDataID": tt[0].SlipRawaDataID,
                                    "ScanningType": tt[0].ScanningType,
                                    "Slipdecision": tt[0].Slipdecision,
                                    "Action": tt[0].Action,
                                    "RejectReason": tt[0].RejectReason,
                                    "rejectreasondescrpsn": tt[0].RejectReasonDescription,
				    "CMSSlipDate": tt[0].SlipDate,
                                    "CMSSlipPickupLocationId": tt[0].PickupLocationId,
                                    "CMSSlipPickupLocationCode": tt[0].PickupLocationCode,
                                    "CMSNumberOfInstruments": tt[0].NoOfInstrument,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqiwmicr = JSON.stringify(L1);
                                    localacct.setItem(owL1, chqiwmicr);

                                }
                            }

                            var Tfimg = document.getElementById('myimg1');
                            if (typeof (Tfimg) != 'undefined' && Tfimg != null) {
                                document.getElementById('myimg1').style.display = "none";
                            }

                            document.getElementById('myimg').removeAttribute('src');
                            document.getElementById('tiffimg').style.display = "none";
                            document.getElementById('myimg').style.display = "block";
				            document.getElementById('myimg').src = RootUrl + 'Icons/Loading.jpg';
                            //document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

                            var frontGreyimgUrl = tt[1].FrontGreyImagePath;
                            $.ajax({
                                url: frontGreyimgUrl,
                                type: 'HEAD',
                                error: function () {
                                    //file not exists
                                    alert('Image not loaded correctly!!!');
                                    document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                                },
                                success: function () {
                                    //file exists
                                    console.log("Front image url success " + frontGreyimgUrl);
                                    document.getElementById('myimg').src = frontGreyimgUrl;
                                }
                            });

                            document.getElementById('Decision').value = "";
                            document.getElementById('IWRemark').value = "";
                            $('#IWRemark').val("");
                            document.getElementById('rtncd').style.display = "none";

                            //$("#frmL1").removeClass();
                            document.getElementById('scanningType').value = tt[1].ScanningType;
                            document.getElementById('tempinstrtype').value = tt[1].InstrumentType;

                            if (tt[1].InstrumentType == "S") {
                                document.getElementById('slipDiv1').style.display = "";
                                document.getElementById('pickupLocationDiv').style.display = "";
                                document.getElementById('chqDiv1').style.display = "none";
                                document.getElementById('chqDiv2').style.display = "none";
                                document.getElementById('MICR').style.display = "none";

                                document.getElementById('pickupLocation').value = "";
                                document.getElementById('pickupLocationID').value = "";
                                document.getElementById('slipRefNo').value = "";
                                document.getElementById('slipDate').value = "";
                                document.getElementById('slipAmount').value = "";
                                document.getElementById('numberOfInstrument').value = "";
                                document.getElementById('clientCode').value = "";
                                document.getElementById('clientName').value = "";

                                var slpDate = "";
                                tempdat = tt[1].SlipDate.split("-");
                                yr = tempdat[0];
                                yr = yr.substring(2, 4);
                                mm = tempdat[1];
                                dd = tempdat[2];
                                slpDate = dd + mm + yr;

                                document.getElementById('pickupLocation').value = tt[1].PickupLocationCode;
                                document.getElementById('pickupLocationID').value = tt[1].PickupLocationId;
                                document.getElementById('slipRefNo').value = tt[1].SlipRefNo;
                                document.getElementById('slipDate').value = slpDate;
                                document.getElementById('slipAmount').value = tt[1].SlipAmount;
                                document.getElementById('numberOfInstrument').value = tt[1].NoOfInstrument;
                                document.getElementById('clientCode').value = tt[1].ClientCode;
                                document.getElementById('clientName').value = tt[1].PayeeName;

                                $('#Decision').attr('readonly', false);
                                $('#Decision').val("");
                                $('#IWRemark').attr('readonly', false);
                                $('#IWRemark').val("");
                                document.getElementById('rejectreasondescrpsn').value = "";

                                if (tt[1].ScanningType == 3) {

                                }

                            }
                            else {
                                document.getElementById('slipDiv1').style.display = "none";
                                document.getElementById('pickupLocationDiv').style.display = "none";
                                document.getElementById('chqDiv1').style.display = "";
                                document.getElementById('chqDiv2').style.display = "";

                                //document.getElementById('slpamt').style.display = "none";
                                //document.getElementById('chqamt').style.display = "";
                                //document.getElementById('Chqacnt').style.display = "";
                                //document.getElementById('slpacnt').style.display = "none";
                                document.getElementById('MICR').style.display = "";
                                //document.getElementById('chequeAcct').innerHTML = tt[1].CreditAccountNo;

                                document.getElementById('clientPayeeName').value = "";
                                document.getElementById('draweeName').value = "";
                                document.getElementById('chequeDate').value = "";
                                document.getElementById('chequeAmount').value = "";
                                document.getElementById('chequeRemarks').value = "";
                                document.getElementById('ChqnoQC').value = "";
                                document.getElementById('SortQC').value = "";
                                document.getElementById('SANQC').value = "";
                                document.getElementById('TransQC').value = "";

                                //--------------------------------------------------CheqDte---
                                if (tt[1].FinalDate != "" || tt[1].FinalDate != null) {

                                    if (tt[1].FinalDate.length > 6) {
                                        tempdat = tt[1].FinalDate.split("-");
                                        yr = tempdat[0];
                                        yr = yr.substring(2, 4);
                                        mm = tempdat[1];
                                        dd = tempdat[2];
                                        fnldate = dd + mm + yr;
                                    }
                                    else {
                                        tempdat = tt[1].FinalDate;
                                        yr = tempdat.substring(4, 6);
                                        mm = tempdat.substring(2, 4);
                                        dd = tempdat.substring(0, 2);
                                        fnldate = dd + mm + yr;
                                    }
                                }
                                else {
                                    fnldate = "";
                                }

                                document.getElementById('draweeName').value = tt[1].DraweeName;
                                //document.getElementById('chequeAmount').value = addCommas(Number(tt[cnt].FinalAmount).toFixed(2));
                                document.getElementById('chequeAmount').value = tt[1].FinalAmount;
                                document.getElementById('chequeDate').value = fnldate;
                                document.getElementById('chequeRemarks').value = tt[1].UserNarration;
                                document.getElementById('clientPayeeName').value = tt[1].PayeeName;
                                document.getElementById('ChqnoQC').value = tt[1].ChequeNoFinal;
                                document.getElementById('SortQC').value = tt[1].SortCodeFinal;
                                document.getElementById('SANQC').value = tt[1].SANFinal;
                                document.getElementById('TransQC').value = tt[1].TransCodeFinal;
                                //document.getElementById('Slipamt').innerHTML = tt[1].SlipAmount;
                                //document.getElementById('sliplabl').style.display = "";

                                //document.getElementById('divctsnoncts').style.display = "";
                                //document.getElementById('lblslpimg').style.display = "";
                                //$('#ctsnocts').val(tt[1].ClearingType);


                                if (tt[1].Slipdecision.toUpperCase() == "R") {
                                    $('#Decision').attr('readonly', true);
                                    $('#Decision').val("R");
                                    $('#IWRemark').attr('readonly', true);
                                    $('#IWRemark').val(tt[1].RejectReason);
                                    document.getElementById('rtncd').style.display = "block";
                                    $('#rejectreasondescrpsn').attr('readonly', true);
                                }
                                else {
                                    $('#Decision').attr('readonly', false);
                                    $('#Decision').val("");
                                    $('#IWRemark').attr('readonly', false);
                                    $('#IWRemark').val("");
                                    document.getElementById('rejectreasondescrpsn').value = "";
                                    //document.getElementById('subCodeReturnDescription').value = "";
                                    //document.getElementById('IWsubcode').value = "";
                                }

                            }
                            document.getElementById('strbranchcd').innerHTML = tt[1].BranchCode;
                            document.getElementById('ScanningID').innerHTML = tt[1].ScanningNodeId;
                            document.getElementById('strBatchNo').innerHTML = tt[1].BatchNo;
                            document.getElementById('strBatchSeqNO').innerHTML = tt[1].BatchSeqNo;
                        }
                    }
                },
                error: function (error) {
                    console.log(error);
                    alert(error);
                }
            });
        }
        else {
            alert('No data found!!');
        }
    }
}

function IWVef() {
    //rtncd
    document.getElementById('rtncd').style.display = "none";


    chr = document.getElementById('Decision').value.toUpperCase();
    var chr = document.getElementById('Decision').value.toUpperCase();
    var iwrk = document.getElementById('IWRemark').value;
    if (chr == "R") {
        if (iwrk == "") {
            document.getElementById('rtncd').style.display = "";
            document.getElementById('IWRemark').style.width = "10%";
            document.getElementById('IWRemark').focus();
        }
        else {
            // alert('aila');
            document.getElementById('rtncd').style.display = "";
            //document.getElementById('IWDecision').focus();
        }

    }
    else {
        document.getElementById('rtncd').style.display = "none";
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
            if (str.charAt(j) == "<" || str.charAt(j) == ">" || str.charAt(j) == '"') {
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

function ChangeImage(imagetype) {
    //debugger;
    var indexcnt = document.getElementById('cnt').value;
    if (imagetype == "FTiff") {

        var i = tt[indexcnt].FrontTiffImagePath;
        $.ajax({
            url: i,
            type: 'HEAD',
            error: function () {
                //file not exists
                alert('Image not loaded correctly!!!');
                document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
            },
            success: function () {
                //file exists
                //console.log("Front image url success " + frontGreyimgUrl);
                $.ajax({
                    url: RootUrl + 'OWCMSDataEntry/getTiffImage',
                    dataType: 'html',
                    data: { httpwebimgpath: i },
                    success: function (Slipdata) {
                        //debugger;
                        $('#tiffimg').html(Slipdata);
                        //document.getElementById('myimg').src = Slipdata;
                        document.getElementById('myimg').style.display = "none";
                        document.getElementById('tiffimg').style.display = "block";

                    }
                });
            }
        });

    }
    else if (imagetype == "BTiff") {

        var i = tt[indexcnt].BackTiffImagePath;
        $.ajax({
            url: i,
            type: 'HEAD',
            error: function () {
                //file not exists
                alert('Image not loaded correctly!!!');
                document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
            },
            success: function () {
                //file exists
                //console.log("Front image url success " + frontGreyimgUrl);
                $.ajax({
                    url: RootUrl + 'OWCMSDataEntry/getTiffImage',
                    dataType: 'html',
                    data: { httpwebimgpath: i },
                    success: function (Slipdata) {
                        //debugger;
                        $('#tiffimg').html(Slipdata);
                        //document.getElementById('myimg').src = Slipdata;
                        document.getElementById('myimg').style.display = "none";
                        document.getElementById('tiffimg').style.display = "block";
                    }
                });
            }
        });

        
    }
    else if (imagetype == "FGray") {
        //debugger;

        var i = tt[indexcnt].FrontGreyImagePath;
        //document.getElementById('myimg').src = i;

        $.ajax({
            url: i,
            type: 'HEAD',
            error: function () {
                //file not exists
                alert('Image not loaded correctly!!!');
                document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
            },
            success: function () {
                //file exists
                //console.log("Front image url success " + frontGreyimgUrl);
                document.getElementById('myimg').src = i;
            }
        });

        //$.ajax({
        //    url: RootUrl + 'OWCMSDataEntry/getTiffImage',
        //    dataType: 'html',
        //    data: { httpwebimgpath: i },
        //    success: function (Slipdata) {
        //        //debugger;
        //        $('#divtiff').html(Slipdata);
        //        //document.getElementById('myimg').src = Slipdata;
        //        document.getElementById('myimg').style.display = "none";
        //        document.getElementById('divtiff').style.display = "block";
        //    }
        //});
    }
    else if (imagetype == "FUV") {
        //debugger;
        if (tt[indexcnt].FrontUVImagePath != null) {
            var i = tt[indexcnt].FrontUVImagePath;
            $.ajax({
                url: i,
                type: 'HEAD',
                error: function () {
                    //file not exists
                    alert('Image not loaded correctly!!!');
                    document.getElementById('myimg').src = RootUrl + 'Icons/noimagefound.jpg';
                },
                success: function () {
                    //file exists
                    //console.log("Front image url success " + frontGreyimgUrl);
                    $.ajax({
                        url: RootUrl + 'OWCMSDataEntry/getTiffImage',
                        dataType: 'html',
                        data: { httpwebimgpath: i },
                        success: function (Slipdata) {
                            //debugger;
                            $('#tiffimg').html(Slipdata);
                            //document.getElementById('myimg').src = Slipdata;
                            document.getElementById('myimg').style.display = "none";
                            document.getElementById('tiffimg').style.display = "block";
                        }
                    });
                }
            });

            
        }
    }

}

function fullImageTiff() {
    //debugger;
    //alert('ok');
    console.log("zoom tiff");
    document.getElementById('iwimg1').style.display = 'block';
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg1').src = document.getElementById('myimg1').src;
};

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

function IWLICQC() {

    var IWdecn = document.getElementById('Decision').value.toUpperCase();
    var valcnt = document.getElementById('cnt').value;

    if (tt[valcnt].InstrumentType == "S") {

        if (!$('#slipDate').val()) {
            alert('Date cannot be blank.');
            $("#slipDate").focus();
            return false;
        }

        else if (!$('#slipAmount').val()) {
            alert('Amount cannot be blank.');
            $("#slipAmount").focus();
            return false;
        }

        else if (!$('#slipRefNo').val()) {
            alert('Slip Ref cannot be blank.');
            $("#slipRefNo").focus();
            return false;
        }

        else if (!$('#pickupLocation').val()) {
            alert('Pickup Location cannot be blank.');
            $("#pickupLocation").focus();
            return false;
        }

        else if (!$('#numberOfInstrument').val()) {
            alert('Number of instrument cannot be blank.');
            $("#numberOfInstrument").focus();
            return false;
        }

        else if (!$('#clientCode').val()) {
            alert('Client Code cannot be blank.');
            $("#clientCode").focus();
            return false;
        }

        else if (!$('#clientName').val()) {
            alert('Client Name cannot be blank.');
            $("#clientName").focus();
            return false;
        }

        //============= amount validation start ==============================

        //----------------------------Amount---------------------//
        amt = document.getElementById('slipAmount').value;
        //alert(amt);   
        var intcont = 0;
        for (var i = 0; i < amt.length; i++) {

            if (amt.charAt(i) == ".") {
                intcont++;
            }
            if (intcont > 1) {
                alert('Enter valid amount!');
                document.getElementById('slipAmount').focus();

                return false;
            }
        }

        if (amt == "NaN") {
            alert('Enter valid amount!');
            document.getElementById('slipAmount').focus();

            return false;
        }

        amt1 = amt;
        var amtval = amt;
        amtval = amtval.replace(/^0+/, '')
        //amt = amt.replace(/^0+/, '')
        amt = $("#slipAmount").val().replace(/,/g, '');
        amt = amt.length;
        if (amtval == ".") {
            alert('Amount field should not be dot(.) !');
            document.getElementById('slipAmount').focus();
            return false;
        }
        else if (amtval == "0.00") {
            alert('Amount field should not be zero(0) !');
            document.getElementById('slipAmount').focus();
            return false;
        }
        else if (amt < 1) {
            alert('Amount field should not be empty !');
            document.getElementById('slipAmount').focus();
            return false;
        }
        else if (amt > 15) {
            alert('Amount not valid !');
            document.getElementById('slipAmount').focus();
            return false;
        }

        //============= amount validation end =============================

        //============= date validation start =========================
        var dd, mm, yy;
        dat = document.getElementById('slipDate').value;
        var chqdt = document.getElementById('slipDate').value;
        if (chqdt.length <= 0 || chqdt.length < 2) {
            alert('Please enter correct Date!');
            document.getElementById('slipDate').focus();
            return false;
        }
        if (dat == "") {
            //alert('aila');
            alert("Date field should not be empty !");
            document.getElementById('slipDate').focus();
            document.getElementById('slipDate').select();
            return false;
        }
        //else if ((dat.toUpperCase().charAt(dat.length - 1) == "X") && (IWdecn.toUpperCase() == "A")) {
        //    alert("Date is not correct");
        //    document.getElementById('IWDecision').focus();
        //    return false;
        //}
        else if (dat.length < 6) {
            alert("Date not valid !");
            document.getElementById('slipDate').focus();
            document.getElementById('slipDate').select();
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

            if ($("#slipDate").val() != "000000") {
                var onlydate = dd + '/' + mm + '/' + '20' + yy;


                var rtn = validatedate(onlydate);
                if (rtn == false) {
                    document.getElementById('slipDate').focus();
                    document.getElementById('slipDate').select();
                    return false;
                }
            }

        }

    //================== date validation end ==========================

    }

    if (tt[valcnt].InstrumentType == "C") {

        if (!$('#chequeDate').val()) {
            alert('Date cannot be blank.');
            $("#chequeDate").focus();
            return false;
        }

        else if (!$('#chequeAmount').val()) {
            alert('Amount cannot be blank.');
            $("#chequeAmount").focus();
            return false;
        }


        else if (!$('#draweeName').val()) {
            alert('Drawee name cannot be blank.');
            $("#draweeName").focus();
            return false;
        }

        else if (!$('#clientPayeeName').val()) {
            alert('Client Name cannot be blank.');
            $("#clientPayeeName").focus();
            return false;
        }

        //============= amount validation start ==============================

        //----------------------------Amount---------------------//
        amt = document.getElementById('chequeAmount').value;
        //alert(amt);   
        var intcont = 0;
        for (var i = 0; i < amt.length; i++) {

            if (amt.charAt(i) == ".") {
                intcont++;
            }
            if (intcont > 1) {
                alert('Enter valid amount!');
                document.getElementById('chequeAmount').focus();

                return false;
            }
        }

        if (amt == "NaN") {
            alert('Enter valid amount!');
            document.getElementById('chequeAmount').focus();

            return false;
        }

        amt1 = amt;
        var amtval = amt;
        amtval = amtval.replace(/^0+/, '')
        //amt = amt.replace(/^0+/, '')
        amt = $("#chequeAmount").val().replace(/,/g, '');
        amt = amt.length;
        if (amtval == ".") {
            alert('Amount field should not be dot(.) !');
            document.getElementById('chequeAmount').focus();
            return false;
        }
        else if (amtval == "0.00") {
            alert('Amount field should not be zero(0) !');
            document.getElementById('chequeAmount').focus();
            return false;
        }
        else if (amt < 1) {
            alert('Amount field should not be empty !');
            document.getElementById('chequeAmount').focus();
            return false;
        }
        else if (amt > 15) {
            alert('Amount not valid !');
            document.getElementById('chequeAmount').focus();
            return false;
        }

        //============= amount validation end =============================

        //============= date validation start =========================
        var dd, mm, yy;
        dat = document.getElementById('chequeDate').value;
        var chqdt = document.getElementById('chequeDate').value;
        if (chqdt.length <= 0 || chqdt.length < 2) {
            alert('Please enter correct Date!');
            document.getElementById('chequeDate').focus();
            return false;
        }
        if (dat == "") {
            //alert('aila');
            alert("Date field should not be empty !");
            document.getElementById('chequeDate').focus();
            document.getElementById('chequeDate').select();
            return false;
        }
        //else if ((dat.toUpperCase().charAt(dat.length - 1) == "X") && (IWdecn.toUpperCase() == "A")) {
        //    alert("Date is not correct");
        //    document.getElementById('IWDecision').focus();
        //    return false;
        //}
        else if (dat.length < 6) {
            alert("Date not valid !");
            document.getElementById('chequeDate').focus();
            document.getElementById('chequeDate').select();
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

            if ($("#chequeDate").val() != "000000") {
                var onlydate = dd + '/' + mm + '/' + '20' + yy;


                var rtn = validatedate(onlydate);
                if (rtn == false) {
                    document.getElementById('chequeDate').focus();
                    document.getElementById('chequeDate').select();
                    return false;
                }
            }

        }

    //================== date validation end ==========================

        ///------------------------------------Post Date and Stale Cheques ----///

        debugger;
        var postdate = document.getElementById('postdt').value;
        var staledate = document.getElementById('staledt').value;

        var fnldate;
        if (finldat.length < 10) {
            fnldate = '20' + yy + '/' + mm + '/' + dd;
        }
        else {
            fnldate = finldat;
        }

        var staldat = new Date(staledate);
        var postdat = new Date(postdate);
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
        
    }

    if (IWdecn == "") {
        alert('Please enter decision!');
        document.getElementById('Decision').focus();
        return false;
    }

    else if (IWdecn != "A" && IWdecn != "R") {
        // alert(IWdecn);
        alert('Decision not correct!');
        document.getElementById('Decision').focus();
        return false;
    }
    else if (IWdecn == "A") {
        ////------------------------Sort code---------------

        var ChqnoQC = document.getElementById('ChqnoQC').value;
        var SortQC = document.getElementById('SortQC').value;
        var SANQC = document.getElementById('SANQC').value;
        var TransQC = document.getElementById('TransQC').value;

        var numbers = /^[0-9]+$/;

        if (!ChqnoQC.match(numbers)) {
            alert('Please enter correct cheque No!');
            document.getElementById('ChqnoQC').focus();
            return false;
        }

        if (!SortQC.match(numbers)) {
            alert('Please enter correct sort code No!');
            document.getElementById('SortQC').focus();
            return false;
        }

        if (!SANQC.match(numbers)) {
            alert('Please enter correct SAN No!');
            document.getElementById('SANQC').focus();
            return false;
        }

        if (!TransQC.match(numbers)) {
            alert('Please enter correct Trans Code!');
            document.getElementById('TransQC').focus();
            return false;
        }

        if (ChqnoQC.length <= 0 || ChqnoQC.length < 6) {
            alert('Please enter cheque No!');
            document.getElementById('ChqnoQC').focus();
            return false;
        }
        else if (ChqnoQC == "") {
            //alert('aila');
            alert("Cheque no should not be empty !");
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();
            return false;
        }
        else if (SortQC.length <= 0 || SortQC.length < 9) {
            alert('Please enter sort code!');
            document.getElementById('SortQC').focus();
            return false;
        }
        else if (SortQC == "") {
            //alert('aila');
            alert("Sort code should not be empty !");
            document.getElementById('SortQC').focus();
            document.getElementById('SortQC').select();
            return false;
        }
        else if (SANQC.length <= 0 || SANQC.length < 6) {
            alert('Please enter san No!');
            document.getElementById('SANQC').focus();
            return false;
        }
        else if (SANQC == "") {
            alert("SAN code should not be empty !");
            document.getElementById('SANQC').focus();
            document.getElementById('SANQC').select();
            return false;
        }
        else if (TransQC.length <= 0 || TransQC.length < 2) {
            alert('Please enter trans code!');
            document.getElementById('TransQC').focus();
            return false;
        }
        else if (TransQC == "") {
            alert("Trans code should not be empty !");
            document.getElementById('TransQC').focus();
            document.getElementById('TransQC').select();
            return false;
        }
        else if (ChqnoQC.length < 6 || ChqnoQC == "000000") {
            alert("Cheque no is not valid !");
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();
            return false;
        }
        else if (SortQC.length < 9 || SortQC == "000000000") {
            alert("Sort code no is not valid !");
            document.getElementById('SortQC').focus();
            document.getElementById('SortQC').select();
            return false;
        }
        else if (SANQC.length < 6) {
            alert("SAN code no is not valid !");
            document.getElementById('SANQC').focus();
            document.getElementById('SANQC').select();
            return false;
        }
        else if (ChqnoQC.length < 6 || ChqnoQC == "000000" || isNaN(ChqnoQC)) {
            alert("Cheque no is not valid !");
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();
            return false;
        }
        else if (TransQC.length < 2 || TransQC == "00" || TransQC.substring(0, 1) == "0") {
            alert("Trans code is not valid !");
            document.getElementById('TransQC').focus();
            document.getElementById('TransQC').select();
            return false;
        }
        else if (SANQC.length > 6 && TransQC.length < 3) {
            alert("Please enter valid SAN and TransCode !");
            document.getElementById('SANQC').focus();
            return false;
        }
        else if (TransQC.length > 2 && SANQC.length < 7) {
            alert("Please enter valid SAN and TransCode !");
            document.getElementById('SANQC').focus();
            return false;
        }

        var rtnflg = validYrnscodes();
        if (rtnflg == false) {
            alert("Trans code is not valid !");
            document.getElementById('TransQC').focus();
            document.getElementById('TransQC').select();
            return false;
        }
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
        // debugger;
        if (iwrked.length == 1) {
            //-------------------------
            var pad = "0"
            iwrked = pad.substring(0, iwrked.length) + iwrked;
            //-------------------------
        }

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