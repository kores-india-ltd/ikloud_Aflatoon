
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

    $("#clientCode").keyup(function (event) {
        var subCode = $("#clientCode").val().toUpperCase();
        $("#clientCode").val(subCode);
    });

    $("#pickupLocation").keyup(function (event) {
        var subCode = $("#pickupLocation").val().toUpperCase();
        $("#pickupLocation").val(subCode);
    });

    if (tt.length > 0) {
        debugger;
        document.getElementById('tiffimg').style.display = "none";
	    document.getElementById('myimg').style.display = "block";
        document.getElementById('myimg').src = RootUrl + 'Icons/Loading.jpg';

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
        //document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
        
        //document.getElementById('ChqCnt').innerHTML = tt[1].SlipChequeCount;
        //document.getElementById('totamt').innerHTML = tt[1].ChequeAmountTotal;// addCommas(Number(tt[1].ChequeAmountTotal).toFixed(2)) //tt[1].ChequeAmountTotal;

        //ChequeAmountTotal = tt[1].ChequeAmountTotal;

        //if ((tt[1].ChequeAmountTotal != "" && tt[1].ChequeAmountTotal != 0) && (tt[1].SlipAmount != "" && tt[1].SlipAmount != 0)) {
        //    if (tt[1].ChequeAmountTotal != tt[1].SlipAmount) {
        //        document.getElementById("frmL1").classList.add("w3-highway-red");
        //    }
        //}
        //else {
        //    document.getElementById("frmL1").classList.add("w3-highway-red");
        //}

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

            //document.getElementById('pickupLocation').value = "";
            //document.getElementById('slipRefNo').value = "";
            //document.getElementById('slipDate').value = "";
            //document.getElementById('slipAmount').value = "";
            //document.getElementById('numberOfInstrument').value = "";
            //document.getElementById('clientCode').value = "";
            //document.getElementById('clientName').value = "";
            
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
        //document.getElementById('rejectreasondescrpsn').value = "";
        
    }

    $("#btnSave").click(function () {
        debugger;
        nextcall = false;

        var result = AllSlipValidation();
        //var result = true;

        if (result == false) {
            //disableEnablefield('E');
            return false;
        }
        else {
            var owL1 = "owL1";
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;

            if (InstrumentType == "S") {
                
                
                SlipID = tt[tempcnt].Id;
                SlipRawaDataID = tt[tempcnt].RawDataId;
            }
            else {
                
            }
            debugger;
            owL1 = owL1 + cnt;
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

    $("#btnChqSave").click(function () {
        nextcall = false;

        var result = AllChequeValidation();
        //var result = true;

        if (result == false) {
            //disableEnablefield('E');
            return false;
        }
        else {
            var owL1 = "owL1";
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;

            if (InstrumentType == "S") {

                SlipID = tt[tempcnt].Id;
                SlipRawaDataID = tt[tempcnt].RawDataId;
            }
            else {

            }
            debugger;
            owL1 = owL1 + cnt;

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

            };

            nextcall = true;
        }

        if (nextcall == true) {

            common(owL1);
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

                    }

                }
            }
            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(idslst);
            //if (orderData.InstrumentType == "S") {
            $.ajax({

                url: RootUrl + 'OWCMSDataEntry/OWCMSDataEntry',
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

    $("#btnChqClose").click(function () {

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

                    }

                }
            }
            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;
            //alert(idslst);
            //if (orderData.InstrumentType == "S") {
            $.ajax({

                url: RootUrl + 'OWCMSDataEntry/OWCMSDataEntry',
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

var value = 0;
    callrotate = function () {
        //value += 180;
        //$("#myimg,#myimg1").rotate({ animateTo: value })
        if (value == 0) {
            value += 180;
            $("#myimg,#myimg1").rotate({ animateTo: value })
        }
        else {
            value += 180;
            $("#myimg,#myimg1").rotate({ animateTo: value })
            value = 0;
        }
    }

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

        document.getElementById('tempinstrtype').value = tt[cnt].InstrumentType;
        document.getElementById('scanningType').value = tt[cnt].ScanningType;

        if (tt[cnt].InstrumentType == "S") {
            document.getElementById('slipDiv1').style.display = "";
            document.getElementById('pickupLocationDiv').style.display = "";
            document.getElementById('chqDiv1').style.display = "none";
            document.getElementById('chqDiv2').style.display = "none";
            document.getElementById('MICR').style.display = "none";

            document.getElementById('pickupLocation').value = "";
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

            document.getElementById('clientPayeeName').value = tt[cnt].PayeeName;
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

                    }

                }
            }

            //------------------------------- Calling Ajax for taking more data------------------
            // alert('finaly');
            next_idx = 0;
            tot_idx = 0;
            var pcnt = cnt;

            $.ajax({

                url: RootUrl + 'OWCMSDataEntry/OWCMSDataEntry',
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
                                document.getElementById('slipRefNo').value = "";
                                document.getElementById('slipDate').value = "";
                                document.getElementById('slipAmount').value = "";
                                document.getElementById('numberOfInstrument').value = "";
                                document.getElementById('clientCode').value = "";
                                document.getElementById('clientName').value = "";


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

function clientCodeSelected(code, name, accountNo) {
    
    document.getElementById('clientCode').value = code;
    document.getElementById('clientName').value = name;
    document.getElementById('clientAccountNo').value = accountNo;
    document.getElementById('ClientCodeWindow').style.display = 'none';
    
}

function getClientDetails() {
    debugger;
    var clntcnt = document.getElementById('cnt').value;

    if ($("#clientCode").val() != "") {

        $.ajax({
            url: RootUrl + 'OWCMSDataEntry/GetClientDetails',
            dataType: 'html',
            data: { ac: $("#clientCode").val().toUpperCase() },
            async: false,
            success: function (data) {
                debugger;
                gclientflag = true;
                var custname = JSON.parse(data);

                if (custname.CustomerName == null) {
                    alert("Please enter valid client code!!!");
                    //document.getElementById('clientCode').focus();
                    return false;
                }
                else {
                    $('#clientName').val(custname.CustomerName);
                    $('#clientAccountNo').val(custname.CustomerAccountNo);
                }
                
                // cbsbtn = true;
            },
            error: function () {
                alert("An error occurred while procesing your request. Service Unavailable  !!!...\\n Please Login Again");
            }
        });
    }
    else {
        alert('Please enter Client Code !!');
        gclientflag = true;
        $("#clientCode").focus();
        return false;
    }

}

function pickupCodeSelected(code, name, id) {

    document.getElementById('pickupLocation').value = code;
    document.getElementById('pickupLocationID').value = id;
    //document.getElementById('clientName').value = name;
    document.getElementById('PickupLocationWindow').style.display = 'none';

}

function getLocationDetails() {
    debugger;
    var clntcnt = document.getElementById('cnt').value;

    if ($("#pickupLocation").val() != "") {

        $.ajax({
            url: RootUrl + 'OWCMSDataEntry/GetLocationDetails',
            dataType: 'html',
            data: { ac: $("#pickupLocation").val().toUpperCase() },
            async: false,
            success: function (data) {
                debugger;
                gclientflag = true;
                var locationCode = JSON.parse(data);

                if (locationCode.LocationCode == null) {
                    alert("Please enter valid location code!!!");
                    //document.getElementById('pickupLocation').focus();
                    return false;
                }
                else {
                    $('#pickupLocation').val(locationCode.LocationCode);
                    $('#pickupLocationID').val(locationCode.ID);
                }
                
                // cbsbtn = true;
            },
            error: function () {
                alert("An error occurred while procesing your request. Service Unavailable  !!!...\\n Please Login Again");
            }
        });
    }
    else {
        alert('Please enter Location Code !!');
        gclientflag = true;
        $("#pickupLocation").focus();
        return false;
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
	
        document.getElementById('tiffimg').style.display = "none";
	    document.getElementById('myimg').style.display = "block";
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

function AllSlipValidation() {

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

function AllChequeValidation() {

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
	debugger;
	var ChqnoQC = document.getElementById('ChqnoQC').value;
    var SortQC = document.getElementById('SortQC').value;
    var SANQC = document.getElementById('SANQC').value;
    var TransQC = document.getElementById('TransQC').value;

	var numbers = /^[0-9]+$/;

	if(!ChqnoQC.match(numbers)){
	 alert('Please enter correct cheque No!');
         document.getElementById('ChqnoQC').focus();
	 return false;
	}

	if(!SortQC.match(numbers)){
	 alert('Please enter correct sort code No!');
         document.getElementById('SortQC').focus();
	 return false;
	}

	if(!SANQC.match(numbers)){
	 alert('Please enter correct SAN No!');
         document.getElementById('SANQC').focus();
	 return false;
	}

	if(!TransQC.match(numbers)){
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
        
        return false;
    }
    
    else if (ChqnoQC.length < 6 || ChqnoQC == "000000") {
        alert("Cheque no is not valid !");
        document.getElementById('ChqnoQC').focus();
        
        return false;
    }
    else if (SortQC.length < 9 || SortQC == "000000000") {
        alert("Sort code no is not valid !");
        document.getElementById('SortQC').focus();
        
        return false;
    }
    else if (SANQC.length < 6) {
        alert("SAN code no is not valid !");
        document.getElementById('SANQC').focus();
        
        return false;
    }
    else if (ChqnoQC.length < 6 || ChqnoQC == "000000" || isNaN(ChqnoQC)) {
        alert("Cheque no is not valid !");
        document.getElementById('ChqnoQC').focus();
        
        return false;
    }
    else if (TransQC.length < 2 || TransQC == "00" || TransQC.substring(0, 1) == "0") {
        alert("Trans code is not valid !");
        document.getElementById('TransQC').focus();
        
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