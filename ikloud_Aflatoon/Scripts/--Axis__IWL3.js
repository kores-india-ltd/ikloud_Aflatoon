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
var mopclick = true;
var strModified = "0000000000";
var chkpositive = false;

//================= Amol changes for Hold on 26/08/2023 start =====================
var HoldLocationId = "";
var HoldReasonText = "";
var flghold = false;
//================= Amol changes for Hold on 26/08/2023 end =====================

function passval(array) {
debugger;
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
var getL1Logs;
var fnldate;
var tempdat;
var yr, mm, dd;

$(document).ready(function () {

    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].ID)
    }
    //  alert(idslst.length);
    //------------ idslist end----------------//copy
    $('#accnt,#ChqDate').bind("cut paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

//--------------For MOP-----------------01-11-2020---------
    $(document).keypress(function (event) {

        if (event.keyCode == 77 || event.keyCode == 109) {
            mopclick = false;
        }
	if (event.keyCode == 83 || event.keyCode == 115) {
            windowcall(); //Sign
            return false;        
	} 

    });


    //-----------------ShortCut----for CBS---
    $("#accnt").keydown(function (event) {
        // alert(event.keyCode);
        if (event.keyCode == 123) {
            getcbsdtls(tt[1].ID); //CbsDetails
            return false;
        }
    });

    if (tt.length > 0) {

	//-------Remove save objects from browser---//
              window.localStorage.clear();
debugger;

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

        document.getElementById('myimg').src = tt[1].FrontGreyImagePath;

        document.getElementById('ChqnoQC').innerHTML = tt[1].XMLSerialNo;
        document.getElementById('SortQC').innerHTML = tt[1].XMLPayorBankRoutNo;
        document.getElementById('SANQC').innerHTML = tt[1].XMLSAN;
        document.getElementById('TransQC').innerHTML = tt[1].XMLTransCode;
        //document.getElementById('h2amt').value = "";
        document.getElementById('ChqDate').value = fnldate;
        document.getElementById("iwAmt").innerHTML = addCommas(tt[1].XMLAmount);//added on 12-05-2017 tt[1].XMLAmount;
        document.getElementById('accnt').value = tt[1].DbtAccountNo;

        document.getElementById('mtrn').value = tt[1].ID;
        document.getElementById('IWDecision').focus();
        document.getElementById("btnback").disabled = true
        document.getElementById('oldact').value = tt[1].DbtAccountNo;
        document.getElementById('L2rejectDecrp').innerHTML = "";
        document.getElementById('L1rejectDecrp').innerHTML = "";

        //------------------P2F Indicator--------------------
        if (tt[1].DocType == "C") {
            document.getElementById('doctype').innerHTML = "P2F Indicater : Y";
        }
        else {
            document.getElementById('doctype').innerHTML = "P2F Indicater : N";
        }
        //-----------------Presenting Bank Code----------------
        document.getElementById('presenting').innerHTML = "Presenting Bank : " + tt[1].PresentingBankRoutNo

        //------------------------MICR Flags-----------------------------
        // debugger;
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
            }//tt[1].XMLMICRRepairFlags.substring(0, 4) == "1111" &&
            if ( tt[1].XMLMICRRepairFlags.substring(5, 6) == "1") {
                document.getElementById('Micrdtls').innerHTML = " MICR Modified-" + tt[1].XMLMICRRepairFlags;//MICR Status:
                document.getElementById("Micrdtls").classList.add("w3-highway-red");
            }
            else {
                document.getElementById('Micrdtls').innerHTML = " MICR Not Modified-" + tt[1].XMLMICRRepairFlags;//MICR Status:
                document.getElementById("Micrdtls").classList.add("w3-highway-green");
            }

		if (tt[1].XMLMICRRepairFlags.substring(5, 6) == "1") {
                    document.getElementById("frmdiv").classList.add("w3-highway-schoolbus");
                }


        }

		//------------L2 Modification--------------------------29-10-2020--------------
        //-------------Account-----------------------
        if (tt[1].Modified2.charAt(0) == "1") {
            document.getElementById("accnt").style.backgroundColor = "red";
        }
        else {
            document.getElementById("accnt").style.backgroundColor = "white";
        }        
        //-------------ChqDate-----------------------------------------------------------------
        if (tt[1].Modified2.charAt(3) == "1") {
            document.getElementById("ChqDate").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqDate").style.backgroundColor = "white";
        }


        //---------------Call Cbs Details--------
        getcbsdtls(tt[1].ID);
        //autogetcbsdtls(tt[1].CBSClientAccountDtls, tt[1].CBSJointHoldersName); //Commented For CBi
        bankName(tt[1].PresentingBankRoutNo);

	//----------------MOP-----------//
        if (document.getElementById('mopcheck').value == "True") {
            mopclick = false;
        }
        else {
            mopclick = true;
        }
        //--------------MOP END-----------------//


        // debugger;
        document.getElementById('L1rejectDecrp').innerHTML = "";
        document.getElementById("l1decision").innerHTML = "";
        document.getElementById("l2decision").innerHTML = "";
        document.getElementById('L2rejectDecrp').innerHTML = "";

	 //----------AI Final Decision------------------        
        getAIDecision(tt[1].ID);

        //------------------L1 Reject---------------
        if (tt[1].L1Status == 1 || tt[1].L1Status == 2 || tt[1].L1Status == 5 || tt[1].L2Status == 2 || tt[1].L2Status == 3 || tt[1].L2Status == 4 || tt[1].L2Status == 6 || tt[1].L2Status == 7 || tt[1].L2Status == 8 || tt[1].L2Status == 9 || tt[1].L2Status == 10 || tt[1].L2Status == 11) {
            
            if (tt[1].L1Status == 1) {
                $("#l1decision").removeClass();
                document.getElementById("l1decision").innerHTML = "A";
                document.getElementById("l1decision").classList.add("w3-text-green");
                document.getElementById('L1rejectDecrp').style.display = "none";
                // document.getElementById('L2rejectDecrp').style.display = "none"; 
            }
            if (tt[1].L2Status == 1 || tt[1].L2Status == 2 || tt[1].L2Status == 3 || tt[1].L2Status == 6 || tt[1].L2Status == 8) {
                $("#l2decision").removeClass();
                document.getElementById("l2decision").innerHTML = "A";
                document.getElementById("l2decision").classList.add("w3-text-green");
            }
            if (tt[1].L2Status == 10 || tt[1].L2Status == 11) {
                $("#l2decision").removeClass();
                document.getElementById("l2decision").innerHTML = "M";
                document.getElementById("l2decision").classList.add("w3-text-orange");

            }

            if (tt[1].L1Status == 2 || tt[1].L1Status == 5) {
                $("#l1decision").removeClass();
                document.getElementById("l1decision").innerHTML = "R";
                document.getElementById("l1decision").classList.add("w3-text-red");
                document.getElementById('L1rejectDecrp').style.display = "";
                //---------
                getL1Logs(tt[1].ID);

            }
            if (tt[1].L2Status == 4 || tt[1].L2Status == 7 || tt[1].L2Status == 9) {
                $("#l2decision").removeClass();
                document.getElementById("l2decision").innerHTML = "R";
                document.getElementById("l2decision").classList.add("w3-text-red");
                document.getElementById('L2rejectDecrp').style.display = "";
                // alert(tt[1].RejectReason);
                getReturnDecrp(tt[1].RejectReason);
            }

        }
    }

    $("#ok").click(function () {
        //debugger;
        console.log('positive amt:- ' + $("#pAmt").html());
        console.log('positive date:- ' + $("#pDate").html());
        console.log('positive payee:- ' + $("#pPayee").html());
        console.log('ChqnoQC:- ' + $("#ChqnoQC").html());

        var result = IWLICQC()
        if (result == false) {
            return false;
        }
        else {
            var ppAmt = "";
            var ppDt = "";
            var ppPayee = "";
            var ppSrcOrigin = "";
            var ppReceivedDate = "";
            if ($("#pAmt").html() == "Not Found") {
                ppAmt = "";
            }
            else {
                ppAmt = $("#pAmt").html();
            }

            if ($("#pDate").html() == "Not Found") {
                ppDt = "";
            }
            else {
                ppDt = $("#pDate").html();
            }

            if ($("#pPayee").html() == "Not Found") {
                ppPayee = "";
            }
            else {
                ppPayee = $("#pPayee").html();
            }

            if ($("#pSrcOfOrigin").html() == "Not Found") {
                ppSrcOrigin = "";
            }
            else {
                ppSrcOrigin = $("#pSrcOfOrigin").html();
            }

            if ($("#pReceivedDate").html() == "Not Found") {
                ppReceivedDate = "";
            }
            else {
                ppReceivedDate = $("#pReceivedDate").html();
            }

            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var iwL1 = "iwL1";
            var btnval = document.getElementById('IWDecision').value.toUpperCase();

            if (backbtn == true) {
                if (btnval == "R") {
                    cnrslt = btnval;
                    nextcall = true;
                }
                else {
                    cnrslt = btnval;
                    nextcall = true;
                }
                //else {
                //    var tot = MatchWith(backcnt);
                //    var totamt = MatchAmt(backcnt);
                //    if (tot == 0) {
                //        cnrslt = confirm("Entered value of MICR is not maching with XML MICR\n are sure to accept with this value?");
                //        if (cnrslt == false) {
                //            document.getElementById('ChqnoQC').focus();
                //            document.getElementById('ChqnoQC').select();
                //        }
                //        else {
                //            nextcall = true;
                //        }
                //    }
                //    else if (totamt == 0) {
                //        cnrslt = confirm("Entered amount is not maching with XML amount\n are sure to accept with this value?");
                //        if (cnrslt == false) {
                //            document.getElementById('iwAmt').focus();
                //            document.getElementById('iwAmt').select();
                //        }
                //        else {
                //            nextcall = true;
                //        }
                //    }
                //    else {
                //        cnrslt = true;
                //        nextcall = true;
                //    }
                //}


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
                    "DbtAccountNoOld": tt[backcnt].DbtAccountNoOld,
                    "DateOld": fnldate,//tt[backcnt].DateOld,
                    "L1Status": tt[backcnt].L1Status,
                    "L2Status": tt[backcnt].L2Status,
                    "PresentingBankRoutNo": tt[backcnt].PresentingBankRoutNo,
                    "DocType":tt[backcnt].DocType,
                    "XMLMICRRepairFlags": tt[backcnt].XMLMICRRepairFlags,
			        "Modified3": strModified,
                    "Modified2": tt[backcnt].Modified2,
                    "pAmt": ppAmt,
                    "pDate": ppDt,
                    "pPayee": ppPayee,
                    "pSrcOfOrigin": ppSrcOrigin,
                    "pReceivedDate": ppReceivedDate,
                    //======= Amol changes for Hold on 26/08/2023 start =============
                    "HoldLocationId": HoldLocationId,
                    "HoldReason": HoldReasonText,
                    //======= Amol changes for Hold on 26/08/2023 end =============
                };
            }

            else {
                if (btnval == "R") {
                    cnrslt = btnval;
                    nextcall = true;
                }
                else {
                    cnrslt = btnval;
                    nextcall = true;
                }
                //else {

                //    var tot1 = MatchWith(tempcnt);
                //    var totamt1 = MatchAmt(tempcnt);
                //    if (tot1 == 0) {
                //        cnrslt = confirm("Entered MICR value is not maching with XML MICR\n are sure to accept with this value?");
                //        if (cnrslt == false) {
                //            document.getElementById('ChqnoQC').focus();
                //            document.getElementById('ChqnoQC').select();
                //        }
                //        else {
                //            nextcall = true;
                //        }
                //    }
                //    else if (totamt1 == 0) {
                //        cnrslt = confirm("Entered amount is not maching with XML amount\n are sure to accept with this value?");
                //        if (cnrslt == false) {
                //            document.getElementById('iwAmt').focus();
                //            document.getElementById('iwAmt').select();
                //        }
                //        else {
                //            nextcall = true;
                //        }
                //    }
                //    else {
                //        cnrslt = true;
                //        nextcall = true;
                //    }
                //}
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
                    "DbtAccountNoOld": tt[tempcnt].DbtAccountNoOld,
                    "DateOld": fnldate,// tt[tempcnt].DateOld,
                    "L1Status": tt[tempcnt].L1Status,
                    "L2Status": tt[tempcnt].L2Status,
                    "PresentingBankRoutNo": tt[tempcnt].PresentingBankRoutNo,
                    "DocType": tt[tempcnt].DocType,
                    "XMLMICRRepairFlags": tt[tempcnt].XMLMICRRepairFlags,
			        "Modified3": strModified,
                    "Modified2": tt[tempcnt].Modified2,
                    "pAmt": ppAmt,
                    "pDate": ppDt,
                    "pPayee": ppPayee,
                    "pSrcOfOrigin": ppSrcOrigin,
                    "pReceivedDate": ppReceivedDate,
                    //======= Amol changes for Hold on 26/08/2023 start =============
                    "HoldLocationId": HoldLocationId,
                    "HoldReason": HoldReasonText,
                    //======= Amol changes for Hold on 26/08/2023 end =============
                };

            }

            if (nextcall == true) {
                // alert($("#IWRemark").val());
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
	document.getElementById('myimg').removeAttribute('src');

        if (Modernizr.localstorage) {

            backbtn = true;
            var iwL1 = "iwL1"
            var cnt = document.getElementById('cnt').value;
            iwL1 = iwL1 + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(iwL1));

            document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;

          /*  if (tt[cnt - 1].Date.length > 6) {
                tempdat = tt[cnt - 1].Date.split("-");
                yr = tempdat[0];
                yr = yr.substring(2, 4);
                mm = tempdat[1];
                dd = tempdat[2];
                fnldate = dd + mm + yr;
            }
            else {
                tempdat = tt[cnt - 1].Date;
                yr = tempdat.substring(4, 6);
                mm = tempdat.substring(2, 4);
                dd = tempdat.substring(0, 2);
                fnldate = dd + mm + yr;
            }*/
		fnldate =orderData.DateOld;

            document.getElementById('accnt').value = orderData.DbtAccountNo;
            document.getElementById('ChqDate').value = orderData.Date;
            document.getElementById('iwAmt').innerHTML = orderData.XMLAmount
            document.getElementById('ChqnoQC').innerHTML = orderData.XMLSerialNo;
            document.getElementById('SortQC').innerHTML = orderData.XMLPayorBankRoutNo;
            document.getElementById('SANQC').innerHTML = orderData.XMLSAN;
            document.getElementById('TransQC').innerHTML = orderData.XMLTransCode;
            document.getElementById('IWDecision').focus();

            document.getElementById('L2rejectDecrp').innerHTML = "";
            document.getElementById('L1rejectDecrp').innerHTML = "";
	    document.getElementById("rejectreasondescrpsn").value = "";

	    document.getElementById('doctype').innerHTML ="";
	    document.getElementById('bankname').innerHTML = "";

            //------------------P2F Indicator--------------------
            if (tt[backcnt].DocType == "C") {
                document.getElementById('doctype').innerHTML = "P2F Indicater : Y";
            }
            else {
                document.getElementById('doctype').innerHTML = "P2F Indicater : N";
            }
            //-----------------Presenting Bank Code----------------
            document.getElementById('presenting').innerHTML = "Presenting Bank : " + tt[backcnt].PresentingBankRoutNo

            //------------------------MICR Flags-----------------------------
            // debugger;
            if (tt[backcnt].XMLMICRRepairFlags != null || tt[backcnt].XMLMICRRepairFlags == "") {

                $("#Acdtls").removeClass();
                $("#Micrdtls").removeClass();
		$("#frmdiv").removeClass();


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
                if (tt[backcnt].XMLMICRRepairFlags.substring(5, 6) == "1") {
                    document.getElementById('Micrdtls').innerHTML = " MICR Modified-" + tt[backcnt].XMLMICRRepairFlags;//MICR Status:
                    document.getElementById("Micrdtls").classList.add("w3-highway-red");
                }
                else {
                    document.getElementById('Micrdtls').innerHTML = " MICR Not Modified-" + tt[backcnt].XMLMICRRepairFlags;//MICR Status:
                    document.getElementById("Micrdtls").classList.add("w3-highway-green");
                }
		if (tt[backcnt].XMLMICRRepairFlags.substring(5, 6) == "1") {
                    document.getElementById("frmdiv").classList.add("w3-highway-schoolbus");
                }


		

            }

		 //------------L2 Modification--------------------------29-10-2020--------------
        //-------------Account-----------------------
        if (tt[backcnt].Modified2.charAt(0) == "1") {
            document.getElementById("accnt").style.backgroundColor = "red";
        }
        else {
            document.getElementById("accnt").style.backgroundColor = "white";
        }        
        //-------------ChqDate-----------------------------------------------------------------
        if (tt[backcnt].Modified2.charAt(3) == "1") {
            document.getElementById("ChqDate").style.backgroundColor = "red";
        }
        else {
            document.getElementById("ChqDate").style.backgroundColor = "white";
        }

            //---------------Call Cbs Details--------
            getcbsdtls(tt[backcnt].ID);
            //autogetcbsdtls(orderData.DbtAccountNo);
            bankName(tt[backcnt].PresentingBankRoutNo);
            //------------------L1 Reject---------------
            //debugger;
            // debugger;
            document.getElementById('L1rejectDecrp').innerHTML = "";
            document.getElementById("l1decision").innerHTML = "";
            document.getElementById("l2decision").innerHTML = "";
            document.getElementById('L2rejectDecrp').innerHTML = "";

            if (tt[backcnt].L1Status == 1 || tt[backcnt].L1Status == 2 || tt[backcnt].L1Status == 5 || tt[backcnt].L2Status == 2 || tt[backcnt].L2Status == 3 || tt[backcnt].L2Status == 4 || tt[backcnt].L2Status == 6 || tt[backcnt].L2Status == 7 || tt[backcnt].L2Status == 8 || tt[backcnt].L2Status == 9 || tt[backcnt].L2Status == 10 || tt[backcnt].L2Status == 11) {
               // document.getElementById('l1dec').style.display = "";

                if (tt[backcnt].L1Status == 1) {
                    $("#l1decision").removeClass();
                    document.getElementById("l1decision").innerHTML = "A";
                    document.getElementById("l1decision").classList.add("w3-text-green");
                    document.getElementById('L1rejectDecrp').style.display = "none";
                }
                if (tt[backcnt].L2Status == 2 || tt[backcnt].L2Status == 3 || tt[backcnt].L2Status == 6 || tt[backcnt].L2Status == 8) {

                    $("#l2decision").removeClass();
                    document.getElementById("l2decision").innerHTML = "A";
                    document.getElementById("l2decision").classList.add("w3-text-green");

                }
                if (tt[backcnt].L2Status == 10 || tt[backcnt].L2Status == 11) {
                    $("#l2decision").removeClass();
                    document.getElementById("l2decision").innerHTML = "M";
                    document.getElementById("l2decision").classList.add("w3-text-orange");

                }

                if (tt[backcnt].L1Status == 2 || tt[backcnt].L1Status == 5) {
                    $("#l1decision").removeClass();
                    document.getElementById("l1decision").innerHTML = "R";
                    document.getElementById("l1decision").classList.add("w3-text-red");
					document.getElementById('L1rejectDecrp').style.display = "";
                    //---------
                    getL1Logs(tt[backcnt].ID);

                }
                if (tt[backcnt].L2Status == 4 || tt[backcnt].L2Status == 7 || tt[backcnt].L2Status == 9) {
                    $("#l2decision").removeClass();
                    document.getElementById("l2decision").innerHTML = "R";
                    document.getElementById("l2decision").classList.add("w3-text-red");
					document.getElementById('L2rejectDecrp').style.display = "";
                    getReturnDecrp(tt[1].RejectReason);
                }

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
                    arrlist.push(orderData.Date);                    
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
                    arrlist.push(orderData.DbtAccountNoOld);
                    arrlist.push(orderData.DateOld);
                    arrlist.push(orderData.L1Status);
                    arrlist.push(orderData.L2Status);
                    arrlist.push(orderData.PresentingBankRoutNo);
                    arrlist.push(orderData.DocType);
                    arrlist.push(orderData.XMLMICRRepairFlags);

			        arrlist.push(orderData.Modified3);
                    arrlist.push(orderData.Modified2);

                    arrlist.push(orderData.pAmt);
                    arrlist.push(orderData.pDate);
                    arrlist.push(orderData.pPayee);
                    arrlist.push(orderData.pSrcOfOrigin);
                    arrlist.push(orderData.pReceivedDate);
                    //======= Amol changes for Hold on 26/08/2023 start =============
                    arrlist.push(orderData.HoldLocationId);
                    arrlist.push(orderData.HoldReason);
                    //======= Amol changes for Hold on 26/08/2023 end =============
                }
            }
            else {

                for (var i = 1; i < cnt; i++) {


                    var orderData = JSON.parse(localData.getItem("iwL1" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.DbtAccountNo);                    
                    arrlist.push(orderData.Date);                    
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
                    arrlist.push(orderData.DbtAccountNoOld);
                    arrlist.push(orderData.DateOld);
                    arrlist.push(orderData.L1Status);
                    arrlist.push(orderData.L2Status);
                    arrlist.push(orderData.PresentingBankRoutNo);
                    arrlist.push(orderData.DocType);
                    arrlist.push(orderData.XMLMICRRepairFlags);

			        arrlist.push(orderData.Modified3);
                    arrlist.push(orderData.Modified2);

                    arrlist.push(orderData.pAmt);
                    arrlist.push(orderData.pDate);
                    arrlist.push(orderData.pPayee);
                    arrlist.push(orderData.pSrcOfOrigin);
                    arrlist.push(orderData.pReceivedDate);
                    //======= Amol changes for Hold on 26/08/2023 start =============
                    arrlist.push(orderData.HoldLocationId);
                    arrlist.push(orderData.HoldReason);
                    //======= Amol changes for Hold on 26/08/2023 end =============
                }
            }
            //------------------------------- Calling Ajax for taking more data------------------

            //var pcnt = cnt;

            $.ajax({

                url: RootUrl + 'IWL3/IWl3',
                data: JSON.stringify({ lst: arrlist, snd: scond, btnClose: "Close", idlst: idslst }),

                type: 'POST',
                contentType: 'application/json; charset=utf-8',

                dataType: 'json',
                success: function (result) {
                    if (result == false) {
                        //window.location = RootUrl + 'Home/IWIndex?id=1';
                        window.location = RootUrl + 'Home/IWIndex';
                    }
                },
                 error: function () {
                        //alert("Service Unavailable!!! Please Login Again");
			window.location = RootUrl + 'Home/IWIndex';
                    }

            });
        }
    });
    //------------------------------------------------------------
    //------------------------------------------------------------
   /* var value = 0;
    callrotate = function () {
        value += 180;
        $("#myimg").rotate({ animateTo: value })
    }*/

	callrotate =function () {
        var img = document.getElementById('myimg');
		if(img.style.transform == 'rotate(180deg)')
        img.style.transform = 'rotate(360deg)';
		else
		img.style.transform = 'rotate(180deg)';
    }



   

    //---------------- Data Entry -----------------------------------

    $("form input").keydown(function (e) {

        next_idx = $('input[type=text]').index(this) + 1;
        tot_idx = $('body').find('input[type=text]').length;

        //if (next_idx == 2) {
        //    cbsbtn = false
        //}
        //debugger;
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
                    getcbsdtls(tt[tempccnt].ID);
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
    $("#accnt").keypress(function (event) {

        // alert(event.keyCode);
        // debugger;
        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
            cbsbtn = false;
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
        //  alert(document.getElementById("blockkey").value);
	var dAccval = document.getElementById('accnt').value;

        if (document.getElementById("blockkey").value == "1") {
            if (event.keyCode == 65 || event.keyCode == 97) {
                event.preventDefault();
                alert('You can not accept this cheque!');
                return false;
            } 
	        else if ((event.keyCode == 82 || event.keyCode == 114) && dAccval.toString().substring(0, 7) == "9999999") {
                event.preventDefault();
                alert('You can not reject this cheque!');
                return false;
            }

            if (event.keyCode == 82 || event.keyCode == 114 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {

            }
            else {
                event.preventDefault();
            }
        }
        else {

            if (event.keyCode == 113 || event.keyCode == 72 || event.keyCode == 82 || event.keyCode == 65 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
            }
            else {
                event.preventDefault();
            }

        }

	//---------------01-11-2020-----
	 if (event.keyCode == 77 || event.keyCode == 109) {
            mopclick = false;
		event.preventDefault();
        }
	if (event.keyCode == 83 || event.keyCode == 115) {
            windowcall(); //Sign
	event.preventDefault();
            return false;        
	}

//------------END---------------


        if (event.keyCode == 113) {
            windowcall(); //Sign
            return false;
        }
	if (event.keyCode == 115) {//-------F4---MOP
            mopclick = false;
            return false;
        }

	//---------------17-12-2020------80 112
	if (event.keyCode == 80 || event.keyCode == 112) {
	  $("#chkppaye").prop("checked",true);
            chkpositive = true;
	   event.preventDefault();
        }

	//----------------END-------------

    });
    //----------------------IwRemark(Reject reason)
    $("#IWRemark").keydown(function (event) {
        //alert(event.keyCode);
        var chr = document.getElementById('IWDecision').value.toUpperCase();
        if (event.shiftKey) {
            event.preventDefault();
        }

        if (chr == "R") {
            if ((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode > 95 && event.keyCode < 106) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 13 || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
            }
            else {
                event.preventDefault(); // || event.keyCode == 68 || event.keyCode == 100 || event.keyCode == 112 
            }
        }
        if (event.keyCode == 114) {
            getreason();
        }
    });

	//--------------Positive pay check box--------------
    $('#chkppaye').change(function () {

        if (this.checked) {
            chkpositive = true;
        }
        else {
            chkpositive = false;
        }
    });
    //-----------------------END------------------------


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


    //$("#IWRemark").keyup(function (event) {

    //    var rejctrcd = $("#IWRemark").val();
    //    if (rejctrcd.length >= 2) {
    //        var rjctresnlTemp = document.getElementById('rtnlist');
    //        var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
    //        for (var i = 0; i < rjctresnlTemp.length; i++) {
    //            if (rejctrcd == rjctresnlTemp[i].value) {
    //                document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
    //                break;
    //            }
    //        }
    //    }
    //});

    $("#IWRemark").keyup(function (event) {
        var chkcode = false;
        var rejctrcd = $("#IWRemark").val();
        if (rejctrcd.length >= 2) {
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
        if (rejctrcd.length >= 2) {
            if (chkcode == false) {
                alert('Please enter correct return code!!');
                document.getElementById('IWRemark').value = "";
                document.getElementById('IWRemark').focus();
            }
        }
    });

	//--------------------------Do All inpute Changes----------------Validation----29-10-2020-----------
    $("#accnt").focusout(function () {
        //  debugger;
        Foutcnt = document.getElementById('cnt').value;

        if (tt[Foutcnt].DbtAccountNo != $("#accnt").val()) {
	    realmodified = true; 
            strModified = setCharAt(strModified, 0, '1');
        }
        else {
		realmodified = false;
            strModified = setCharAt(strModified, 0, '0');
        }
    });
    $("#ChqDate").focusout(function () {
        // debugger;
        if (fnldate != $("#ChqDate").val()) {
            realmodified = true;
            strModified = setCharAt(strModified, 3, '1');
        }
        else {
            realmodified = false;
            strModified = setCharAt(strModified, 3, '0');
        }
    });


    //-------------------------------Common For Accept and Reject----------------
    function common(val) {
        //alert('cool!!');
        //var tempdat;
        //var yr, mm, dd, fnldate;

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
            //alert("cnt val: "+cnt);
            //  ----Added On 12/07/2017---------------
			document.getElementById('myimg').removeAttribute('src');

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

            document.getElementById('IWDecision').focus();
            cbsbtn = false;
            signaturecall = false;
		mopclick = true;
	 	strModified = "0000000000";
		chkpositive = false;

            $("#chkppaye").prop("checked", false);

            document.getElementById("pAmt").innerHTML = "";
            document.getElementById("pDate").innerHTML = "";
            document.getElementById("pPayee").innerHTML = "";
            document.getElementById("pSrcOfOrigin").innerHTML = "";
            document.getElementById("pReceivedDate").innerHTML = "";


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
            //------------------------------------------------------------
            document.getElementById('ChqnoQC').innerHTML = tt[cnt].XMLSerialNo;
            document.getElementById('SortQC').innerHTML = tt[cnt].XMLPayorBankRoutNo;
            document.getElementById('SANQC').innerHTML = tt[cnt].XMLSAN;
            document.getElementById('TransQC').innerHTML = tt[cnt].XMLTransCode;
            document.getElementById('accnt').value = tt[cnt].DbtAccountNo;
            document.getElementById('ChqDate').value = fnldate;
            document.getElementById('iwAmt').innerHTML =addCommas(tt[cnt].XMLAmount);//added on 12-05-2017; tt[cnt].XMLAmount;
            document.getElementById('mtrn').value = tt[cnt].ID;
            cbsbtn = false;
            document.getElementById('oldact').value = tt[cnt].DbtAccountNo;

	    document.getElementById('doctype').innerHTML ="";
	    document.getElementById('bankname').innerHTML = "";
            //------------------P2F Indicator--------------------
            if (tt[cnt].DocType == "C") {
                document.getElementById('doctype').innerHTML = "P2F Indicater : Y";
            }
            else {
                document.getElementById('doctype').innerHTML = "P2F Indicater : N";
            }
            //-----------------Presenting Bank Code----------------
            document.getElementById('presenting').innerHTML = "Presenting Bank : " + tt[cnt].PresentingBankRoutNo

            //------------------------MICR Flags-----------------------------
            if (tt[cnt].XMLMICRRepairFlags != null || tt[cnt].XMLMICRRepairFlags == "") {

                $("#Acdtls").removeClass();
                $("#Micrdtls").removeClass();
		$("#frmdiv").removeClass();


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
                }//tt[cnt].XMLMICRRepairFlags.substring(0, 4) == "1111" &&
                if (tt[cnt].XMLMICRRepairFlags.substring(5, 6) == "1") {
                    document.getElementById('Micrdtls').innerHTML = " MICR Modified-" + tt[cnt].XMLMICRRepairFlags;//MICR Status:
                    document.getElementById("Micrdtls").classList.add("w3-highway-red");
                }
                else {
                    document.getElementById('Micrdtls').innerHTML = " MICR Not Modified-" + tt[cnt].XMLMICRRepairFlags;//MICR Status:
                    document.getElementById("Micrdtls").classList.add("w3-highway-green");
                }

		if (tt[cnt].XMLMICRRepairFlags.substring(5, 6) == "1") {
                    document.getElementById("frmdiv").classList.add("w3-highway-schoolbus");
                }
		

            }

		//------------L2 Modification--------------------------29-10-2020--------------
            //-------------Account-----------------------
            if (tt[cnt].Modified2.charAt(0) == "1") {
                document.getElementById("accnt").style.backgroundColor = "red";
            }
            else {
                document.getElementById("accnt").style.backgroundColor = "white";
            }           
            //-------------ChqDate-----------------------------------------------------------------
            if (tt[cnt].Modified2.charAt(3) == "1") {
                document.getElementById("ChqDate").style.backgroundColor = "red";
            }
            else {
                document.getElementById("ChqDate").style.backgroundColor = "white";
            }
	
	   $("#cbsdetails").empty();

            //---------------Call Cbs Details--------
            getcbsdtls(tt[cnt].ID);
            //autogetcbsdtls(tt[cnt].CBSClientAccountDtls, tt[cnt].CBSJointHoldersName); //Commented For CBI
            bankName(tt[cnt].PresentingBankRoutNo);

		//----------------MOP-----------//
            if (document.getElementById('mopcheck').value == "True") {
                mopclick = false;
            }
            else {
                mopclick = true;
            }
            //--------------MOP END-----------------//


            //------------------L1 Reject---------------
            //-------------Clear the Fields-------------------------//
            document.getElementById('L1rejectDecrp').innerHTML = "";
            document.getElementById("l1decision").innerHTML = "";
            document.getElementById("l2decision").innerHTML = "";
            document.getElementById('L2rejectDecrp').innerHTML = "";
            //----------------------------------------------------------//

		//----------AI Final Decision------------------        
            getAIDecision(tt[cnt].ID);

           // debugger;
            if (tt[cnt].L1Status == 1 || tt[cnt].L1Status == 2 || tt[cnt].L1Status == 5 || tt[cnt].L2Status == 2 || tt[cnt].L2Status == 3 || tt[cnt].L2Status == 4 || tt[cnt].L2Status == 6 || tt[cnt].L2Status == 7 || tt[cnt].L2Status == 8 || tt[cnt].L2Status == 9 || tt[cnt].L2Status == 10 || tt[cnt].L2Status == 11) {
               
                if (tt[cnt].L1Status == 1) {

                    $("#l1decision").removeClass();
                    document.getElementById("l1decision").innerHTML = "A";

                    document.getElementById("l1decision").classList.add("w3-text-green");

                    document.getElementById('L1rejectDecrp').style.display = "none";
                }
                if (tt[cnt].L2Status == 1 || tt[cnt].L2Status == 2 || tt[cnt].L2Status == 3 || tt[cnt].L2Status == 6 || tt[cnt].L2Status == 8) {
                    $("#l2decision").removeClass();
                    document.getElementById("l2decision").innerHTML = "A";
                    document.getElementById("l2decision").classList.add("w3-text-green");
                }
                if (tt[cnt].L2Status == 10 || tt[cnt].L2Status == 11) {
                    $("#l2decision").removeClass();
                    document.getElementById("l2decision").innerHTML = "M";
                    document.getElementById("l2decision").classList.add("w3-text-orange");

                }

                if (tt[cnt].L1Status == 2 || tt[cnt].L1Status == 5) {
                    $("#l1decision").removeClass();
                    document.getElementById("l1decision").innerHTML = "R";
                    document.getElementById("l1decision").classList.add("w3-text-red");
                    document.getElementById('L1rejectDecrp').style.display = "";
                    //---------
                    getL1Logs(tt[cnt].ID);

                }
                if (tt[cnt].L2Status == 4 || tt[cnt].L2Status == 7 || tt[cnt].L2Status == 9) {
                    $("#l2decision").removeClass();
                    document.getElementById("l2decision").innerHTML = "R";
                    document.getElementById('L2rejectDecrp').style.display = "";
                    document.getElementById("l2decision").classList.add("w3-text-red");

                    getReturnDecrp(tt[cnt].RejectReason);
                }

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
                        arrlist.push(orderData.Date);                        
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
                        arrlist.push(orderData.DbtAccountNoOld);
                        arrlist.push(orderData.DateOld);
                        arrlist.push(orderData.L1Status);
                        arrlist.push(orderData.L2Status);
                        arrlist.push(orderData.PresentingBankRoutNo);
                        arrlist.push(orderData.DocType);
                        arrlist.push(orderData.XMLMICRRepairFlags);
			            arrlist.push(orderData.Modified3);
                        arrlist.push(orderData.Modified2);

                        arrlist.push(orderData.pAmt);
                        arrlist.push(orderData.pDate);
                        arrlist.push(orderData.pPayee);
                        arrlist.push(orderData.pSrcOfOrigin);
                        arrlist.push(orderData.pReceivedDate);
                        //======= Amol changes for Hold on 26/08/2023 start =============
                        arrlist.push(orderData.HoldLocationId);
                        arrlist.push(orderData.HoldReason);
                        //======= Amol changes for Hold on 26/08/2023 end =============
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
                        arrlist.push(orderData.DbtAccountNoOld);
                        arrlist.push(orderData.DateOld);
                        arrlist.push(orderData.L1Status);
                        arrlist.push(orderData.L2Status);
                        arrlist.push(orderData.PresentingBankRoutNo);
                        arrlist.push(orderData.DocType);
                        arrlist.push(orderData.XMLMICRRepairFlags);
			            arrlist.push(orderData.Modified3);
                        arrlist.push(orderData.Modified2);

                        arrlist.push(orderData.pAmt);
                        arrlist.push(orderData.pDate);
                        arrlist.push(orderData.pPayee);
                        arrlist.push(orderData.pSrcOfOrigin);
                        arrlist.push(orderData.pReceivedDate);
                        //======= Amol changes for Hold on 26/08/2023 start =============
                        arrlist.push(orderData.HoldLocationId);
                        arrlist.push(orderData.HoldReason);
                        //======= Amol changes for Hold on 26/08/2023 end =============
                    }
                }
                console.log(arrlist);
                debugger;
                //------------------------------- Calling Ajax for taking more data------------------
                // alert(cnt);
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;
                //alert(pcnt);
                //  debugger;
                $.ajax({

                    url: RootUrl + 'IWL3/IWl3',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[pcnt - 1].FrontGreyImagePath, idlst: idslst }),

                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',

                    dataType: 'json',
                    success: function (result) {
                        if (result == false) {

			//-------Remove save objects from browser---//
                             window.localStorage.clear();

                            window.location = RootUrl + 'Home/IWIndex';
                        }
                        else {

                            // debugger;
                            tt = result;
                            document.getElementById('tempcnt').value = 1;
                            document.getElementById('cnt').value = 1;
				
				cnt=1;
				tempcnt=1;

                            if (tt != null && tt != "") {
                                scond = true;
                                //  scondbck = true;
                                idslst = [];
                                //-------------- idslist--------------------//
                                for (var z = 0; z < tt.length; z++) {
                                    idslst.push(tt[z].ID)
                                }
                                //-------------Saving Last data in storage---
                                var iwL1 = "iwL10"
                                var L1 = {
                                    "DbtAccountNo": tt[0].DbtAccountNo,
                                    "Date": tt[0].Date,
                                    "ID": tt[0].ID,
                                    "L2Opsts": tt[0].L2Opsts,
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
                                    "DbtAccountNoOld": tt[0].DbtAccountNoOld,
                                    "DateOld": tt[0].DateOld,
                                    "L1Status": tt[0].L1Status,
                                    "L2Status": tt[0].L2Status,
                                    "PresentingBankRoutNo": tt[0].PresentingBankRoutNo,
                                    "DocType": tt[0].DocType,
                                    "XMLMICRRepairFlags": tt[0].XMLMICRRepairFlags,
					                "Modified3": tt[0].Modified3,
                                    "Modified2": tt[0].Modified2,
                                    //"pAmt": tt[0].pAmt,
                                    //"pDate": tt[0].pDate,
                                    //"pPayee": tt[0].pPayee,
                                    //"pSrcOfOrigin": tt[0].pSrcOfOrigin,
                                    //"pReceivedDate": tt[0].pReceivedDate,

                                    //======= Amol changes for Hold on 26/08/2023 start =============
                                    "HoldLocationId": tt[0].HoldLocationId,
                                    "HoldReason": tt[0].HoldReason,
                                    //======= Amol changes for Hold on 26/08/2023 end =============
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqiwmicr = JSON.stringify(L1);
                                    localacct.setItem(iwL1, chqiwmicr);

                                }
                                // alert(tt[1].FrontGreyImagePath);
                                //------------------------------------12/07/2017----------------------------------//
				                document.getElementById('myimg').removeAttribute('src');

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


                                document.getElementById('IWDecision').focus();
                                cbsbtn = false;
                                signaturecall = false;
				                mopclick = true;
				                strModified = "0000000000";
				                chkpositive = false;

                                document.getElementById("pAmt").innerHTML = "";
                                document.getElementById("pDate").innerHTML = "";
                                document.getElementById("pPayee").innerHTML = "";
                                document.getElementById("pSrcOfOrigin").innerHTML = "";
                                document.getElementById("pReceivedDate").innerHTML = "";

                                $("#chkppaye").prop("checked", false);

                                document.getElementById('oldact').value = tt[1].DbtAccountNo;

				                document.getElementById('doctype').innerHTML ="";
		                        document.getElementById('bankname').innerHTML = "";

                                //------------------P2F Indicator--------------------
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
					                $("#frmdiv").removeClass();


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
                                    if (tt[1].XMLMICRRepairFlags.substring(5, 6) == "1") {
                                        document.getElementById('Micrdtls').innerHTML = " MICR Modified-" + tt[1].XMLMICRRepairFlags;//MICR Status:
                                        document.getElementById("Micrdtls").classList.add("w3-highway-red");
                                    }
                                    else {
                                        document.getElementById('Micrdtls').innerHTML = " MICR Not Modified-" + tt[1].XMLMICRRepairFlags;//MICR Status:
                                        document.getElementById("Micrdtls").classList.add("w3-highway-green");
                                    }

					                if (tt[1].XMLMICRRepairFlags.substring(5, 6) == "1") {
                  			            document.getElementById("frmdiv").classList.add("w3-highway-schoolbus");
               				        }

                                }

				//------------L2 Modification--------------------------29-10-2020--------------
                                //-------------Account-----------------------
                                if (tt[1].Modified2.charAt(0) == "1") {
                                    document.getElementById("accnt").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("accnt").style.backgroundColor = "white";
                                }
                                //-------------ChqDate-----------------------------------------------------------------
                                if (tt[1].Modified2.charAt(3) == "1") {
                                    document.getElementById("ChqDate").style.backgroundColor = "red";
                                }
                                else {
                                    document.getElementById("ChqDate").style.backgroundColor = "white";
                                }
				                $("#cbsdetails").empty();

                                //---------------Setting focus to textbox----------------//
                                //---------------Call Cbs Details--------
                                getcbsdtls(tt[1].ID);
                                //autogetcbsdtls(tt[1].CBSClientAccountDtls, tt[1].CBSJointHoldersName);// Commented For CBI
                                bankName(tt[1].PresentingBankRoutNo);

				                //----------------MOP-----------//
                                if (document.getElementById('mopcheck').value == "True") {
                                    mopclick = false;
                                }
                                else {
                                    mopclick = true;
                                }
                                //--------------MOP END-----------------//


                                //-------------Clear the Fields-------------------------//

                                document.getElementById('L1rejectDecrp').innerHTML = "";
                                document.getElementById("l1decision").innerHTML = "";
                                document.getElementById("l2decision").innerHTML = "";
                                document.getElementById('L2rejectDecrp').innerHTML = "";
                                //----------------------------------------------------------//

				                //----------AI Final Decision------------------        
                                getAIDecision(tt[1].ID);

                                // debugger;
                                //------------------L1 Reject---------------
                                if (tt[1].L1Status == 1 || tt[1].L1Status == 2 || tt[1].L1Status == 5 || tt[1].L2Status == 2 || tt[1].L2Status == 3 || tt[1].L2Status == 4 || tt[1].L2Status == 6 || tt[1].L2Status == 7 || tt[1].L2Status == 8 || tt[1].L2Status == 9 || tt[1].L2Status == 10 || tt[1].L2Status == 11) {
                                   // document.getElementById('l1dec').style.display = "";

                                    if (tt[1].L1Status == 1) {
                                        $("#l1decision").removeClass();
                                        document.getElementById("l1decision").innerHTML = "A";
                                        document.getElementById("l1decision").classList.add("w3-text-green");

                                        document.getElementById('L1rejectDecrp').style.display = "none";
                                        // document.getElementById('L2rejectDecrp').style.display = "none";
                                    }
                                    if (tt[1].L2Status == 1 || tt[1].L2Status == 2 || tt[1].L2Status == 3 || tt[1].L2Status == 6 || tt[1].L2Status == 8) {
                                        $("#l2decision").removeClass();
                                        document.getElementById("l2decision").innerHTML = "A";
                                        document.getElementById("l2decision").classList.add("w3-text-green");

                                    }
                                    if (tt[1].L2Status == 10 || tt[1].L2Status == 11) {
                                        $("#l2decision").removeClass();
                                        document.getElementById("l2decision").innerHTML = "M";
                                        document.getElementById("l2decision").classList.add("w3-text-orange");

                                    }

                                    if (tt[1].L1Status == 2 || tt[1].L1Status == 5) {
                                        $("#l1decision").removeClass();
                                        document.getElementById("l1decision").innerHTML = "R";
                                        document.getElementById("l1decision").classList.add("w3-text-red");
                                        document.getElementById('L1rejectDecrp').style.display = "";
                                        //---------
                                        getL1Logs(tt[1].ID);

                                    }
                                    if (tt[1].L2Status == 4 || tt[1].L2Status == 7 || tt[1].L2Status == 9) {
                                        $("#l2decision").removeClass();
                                        document.getElementById("l2decision").innerHTML = "R";
                                        document.getElementById('L2rejectDecrp').style.display = "";
                                        document.getElementById("l2decision").classList.add("w3-text-red");
                                        getReturnDecrp(tt[1].RejectReason);
                                    }

                                }
                            }
                            else {
				                //-------Remove save objects from browser---//
                                window.localStorage.clear();
                                alert('No Data Found!!');
                            }
                        }

                    },
                    error: function () {
                        alert("Service Unavailable!!! Please Login Again");
                    }
                });
            }
        }

        else {
		        //-------Remove save objects from browser---//
                 window.localStorage.clear();
                alert('No data found!!');
        }
    }

    function getcbsdtls(recordId) {
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
            //-------------GetPositiveData-------------------//
            GetPositiveData($("#accnt").val(), document.getElementById('ChqnoQC').innerHTML, recordId);
            //------------GetPositiveData END----------------------------//

            $.ajax({
                url: RootUrl + 'IWL3/GetCBSDtls',
                dataType: 'html',
                type: 'POST',
                data: { ac: $("#accnt").val() },
                async: false,
                success: function (data) {
                    cbsbtn = true;
                    // signaturecall = false;
                    $('#cbsdetails').html(data);

                },
                error: function () {
                    alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
                }
            });

        }
    }

    //-------------------For Autocalling-----------------------
    //autogetcbsdtls = function (cbs, joint) {
    //    alert('Aila');
    //    $.ajax({
    //        url: RootUrl + 'IWL3/GetCBSDtls',
    //        dataType: 'html',
    //        data: { strcbsdls: cbs, strJoinHldrs: joint },
    //        success: function (data) {
    //            //alert(data);
    //            $('#cbsdetails').html(data);
    //            //$('#dialogEditUser').dialog('open');
    //        }
    //    });
    //}

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
        },
        error: function () {
                    alert("Service Unavailable!!! Please Login Again");
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
   // var indexcnt = document.getElementById('cnt').value;
	var indexcnt;
	document.getElementById('myimg').removeAttribute('src');

	if (backbtn == true)
	{
		indexcnt=backcnt;
	}
	else
	{
		indexcnt = document.getElementById('cnt').value;	
	}

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

//----------------Reject Reason---------------
//---------------------------IWActivity Logs-------------------
function getL1Logs(iwmntr) {
    // debugger;
    $.ajax({
        url: RootUrl + 'IWL3/getL1logs',
        dataType: 'html',
        data: { id: iwmntr },
        async: false,
        success: function (l1datareject) {
            document.getElementById('L1rejectDecrp').innerHTML = l1datareject;
            document.getElementById('L1rejectDecrp').style.display = "";
            //$('#cbsdetails').html(data);
            //$('#dialogEditUser').dialog('open');
        },
		error: function () {
                alert("Service Unavailable!!! Please Login Again");
        }
    });
}

function autogetcbsdtls(cbs, joint) {
    //alert('Aila');
    var Acct = document.getElementById('accnt').value;
    $.ajax({
        url: RootUrl + 'IWL3/GetCBSDtls',
        dataType: 'html',
        type: 'POST',
	async:false,
        data: { ac: $("#accnt").val()},
        success: function (data) {
            cbsbtn = true;
            //signaturecall = false;
            $('#cbsdetails').html(data);
            //$('#dialogEditUser').dialog('open');
        },
        error: function () {
                        alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });

	//-------------GetPositiveData-------------------//
   GetPositiveData($("#accnt").val(), document.getElementById('ChqnoQC').innerHTML);
    //------------GetPositiveData END----------------------------//

}

function getcbsdtls() {
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
        //-------------GetPositiveData-------------------//
        GetPositiveData($("#accnt").val(), document.getElementById('ChqnoQC').innerHTML, $("#mtrn").val());
    //------------GetPositiveData END----------------------------//

        $.ajax({
            url: RootUrl + 'IWL3/GetCBSDtls',
            dataType: 'html',
            type: 'POST',
            data: { ac: $("#accnt").val() },
	    async:false,
            success: function (data) {
                cbsbtn = true;
               // signaturecall = false;
                $('#cbsdetails').html(data);

            },
            error: function () {
                        alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
                    }
        });

    }
}

//-----------------------Activity Logs----------------
function getIwlogs() {
	console.log('In Logs');
    $(document).ready(function () {
        $("#activitylogs").dialog({
            draggable: true,
            height: 700,
            width: 900,
            position: { my: "Center Top", at: "center center", of: window },
            //buttons: [
            //{
            //    text: "minimize",
            //    click: function () {
            //        $(this).parents('.ui-dialog').animate({
            //            height: '40px',
            //            top: $(window).height() - 40
            //        }, 400); 
            //    }
            //}]
        });
	console.log('In Logs going to call ajax');
        $.ajax({
            url: RootUrl + 'IWL3/getIwlogs',
            dataType: 'html',
            data: { id: $("#mtrn").val() },
            success: function (data) {
                //alert(data);
		console.log(data);
                $('#activitylogs').html(data);
                $('#activitylogs').dialog('open');
            },
            error: function () {
                  alert("Service Unavailable!!! Please Login Again");
            }
        });
    });
}

//----------------------
function GetPositiveData(AccountNo, ChequeNo, recordId, fromSrc) {

debugger;

	 	document.getElementById("pAmt").innerHTML= "Not Found";
                document.getElementById("pDate").innerHTML = "Not Found";
    document.getElementById("pPayee").innerHTML = "Not Found";
    document.getElementById("pSrcOfOrigin").innerHTML = "Not Found";
    document.getElementById("pReceivedDate").innerHTML = "Not Found";
var parseData = [];
    document.getElementById('loader').style.display = "";
	document.getElementById('PositiveDataDiv').style.display = "none";
    $.ajax({
        url: RootUrl + 'IWL3/GetPositiveData',
        dataType: 'html',
        type: 'POST',
        asyn: false,
        data: { AccountNo: AccountNo, ChequeNo: ChequeNo, ID: recordId, FromSrc: fromSrc, SAN: document.getElementById('SANQC').innerHTML },
        success: function (data) {
            
            parseData = data.split("|");
            if (parseData[0].replace(/"/g, '') === "F") {
                document.getElementById("pAmt").innerHTML= "Not Found";
                document.getElementById("pDate").innerHTML = "Not Found";
                document.getElementById("pPayee").innerHTML = "Not Found";
                document.getElementById("pSrcOfOrigin").innerHTML = "Not Found";
                document.getElementById("pReceivedDate").innerHTML = "Not Found";
            }
            else {
                document.getElementById("pAmt").innerHTML = parseData[0].replace(/"/g, '');
                document.getElementById("pDate").innerHTML = parseData[1].replace(/"/g, '');
                document.getElementById("pPayee").innerHTML = parseData[2].replace(/"/g, '');
                document.getElementById("pSrcOfOrigin").innerHTML = parseData[3].replace(/"/g, '');
                document.getElementById("pReceivedDate").innerHTML = parseData[4].replace(/"/g, '');
            }
            document.getElementById('loader').style.display = "none";
            document.getElementById('PositiveDataDiv').style.display = "";

            if (Number(document.getElementById('iwAmt').innerHTML.toString().replace(/,/g, '')) >= Number(document.getElementById('positivepay').value) &&
                document.getElementById("pAmt").innerHTML === "Not Found" && document.getElementById("pDate").innerHTML === "Not Found" &&
                document.getElementById("pPayee").innerHTML === "Not Found") {
                document.getElementById('refreshImage').style.display = 'block';
            }
	    else{
		document.getElementById('refreshImage').style.display = 'none';
	    }
        },
	    error: function () {
            alert("Positive Pay data not fetched.Please click refresh icon or login again!!!");
            document.getElementById('PositiveDataDiv').style.display = "";
            document.getElementById('refreshImage').style.display = 'block';
        }
    });
}
//----------------------

function callPositivePayFunction() {
    //-------------GetPositiveData-------------------//
    var fromSrc = "1";
    GetPositiveData($("#accnt").val(), document.getElementById('ChqnoQC').innerHTML, $("#mtrn").val(), fromSrc);
    //------------GetPositiveData END----------------------------//
    document.getElementById('refreshImage').style.display = 'none';
}

function reasonselected(rtnval) {
    var rtnrjctdescrn = document.getElementById('rtndescrp').value;
    //-----valid Function for validation---------------
    var rslt = valid(document.getElementById('rtndescrp').value, rtnval);

    if (rslt == false) {
        //alert('Please select reject reason!!');
        document.getElementById('rtndescrp').focus();
        document.getElementById('RejectReason').style.display = 'block';
        return false;
    }
    else {
        document.getElementById('IWRemark').value = rtnval;
        document.getElementById('RejectReason').style.display = 'none';
        var rejctrcd = $("#IWRemark").val();
        if (rejctrcd.length >= 2) {
            var rjctresnlTemp = document.getElementById('rtnlist');
            var rtnlistDescrpTemp = document.getElementById('rtnlistDescrp');
            for (var i = 0; i < rjctresnlTemp.length; i++) {
                if (rejctrcd == rjctresnlTemp[i].value) {
                    if (rejctrcd == "88") {
                        document.getElementById("rejectreasondescrpsn").value = rtnrjctdescrn;
                    }
                    else {
                        document.getElementById("rejectreasondescrpsn").value = rtnlistDescrpTemp[i].value;
                    }


                    break;
                }
            }
        }
    }
}
//---------------iw Search mannual return validation----
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



function IWLICQC() {
    // alert('abid');
    var IWdecn = document.getElementById('IWDecision').value.toUpperCase();

    //========= Amol changes for Hold on 26/08/2023 start ==========
    if (IWdecn == "H" && flghold == true) {
        alert('Click on ok button to save hold reason!');
        return false;
    }
    //========= Amol changes for Hold on 26/08/2023 end ==========

    //----------------DBTAC-----------------------
    var Acct = document.getElementById('accnt').value;
    var acmin = document.getElementById('acmin').value;
    //Acct = Acct.replace(/^0+/, '')
   

    var tempacntno = document.getElementById('oldact').value;
    if ($("#accnt").val() == "") {
        alert("Please enter account number!!")
        return false;
    }
    if (tempacntno != $("#accnt").val()) {

        document.getElementById('oldact').value = $("#accnt").val();
       // cbsbtn = false;
       // getcbsdtls();
    }
    if (document.getElementById("blockkey").value == "1" && IWdecn == "A") {
        alert('You can not accept this cheque!');
        return false;
    } 

   else if (document.getElementById("blockkey").value == "1" && IWdecn == "R" && Acct.toString().substring(0, 7) == "9999999") {
        alert('You can not reject this cheque!');
        return false;
    }  
	
    if(cbsbtn == false){

	alert('Please click on Getdetails!');
        return false;

     }


    ///---------------Added on 19-04-2017---------------
    var prevval = "";
    var nextval;
    var index = 0;
    //debugger;
    if (Acct.length > acmin) {
        
        if (Acct!= "9999999999999999" && IWdecn == "R" && document.getElementById("blockkey").value == "1" && document.getElementById("Allowcase").value != "1") {
            alert("If account is invalid on cheque then please reject with 16 times 9(9999999999999999)!");
            return false;
        }
    }
    //----------------------------------
    var Accval = Acct;
    Acct = Acct.length

    if (signaturecall == false) {
        alert("Please press S for signature!!");
        document.getElementById('IWDecision').focus();
        return false;
    }

//-----MOP---------------20-10-2020-------------
    if (mopclick == true && (Accval.toString().slice(-9) != "017610110" && Accval.toString().slice(-9) != "010633007" && Accval!= "2740121001126" && Accval!= "2300121001127" && Accval!= "2450121001125" && Accval!= "469012100118" && Accval!= "12100105")) {
        alert("Please press M for MOP!!");
        document.getElementById('IWDecision').focus();
        return false;
    }
    //------------------END-------------------------


	//----------------Positive Pay Validation-----------//
    if (Number(document.getElementById('iwAmt').innerHTML.toString().replace(/,/g, '')) >= Number(document.getElementById('positivepay').value)) {
        if (chkpositive == false && IWdecn == "A") {
            alert("Please click on payee checkbox!!");
            document.getElementById('IWDecision').focus();
            return false;
        }
        if (document.getElementById('iwAmt').innerHTML.toString().replace(/,/g, '') != document.getElementById('pAmt').innerHTML && IWdecn == "A") {
            alert("Amount not matching with positive pay!!");
            document.getElementById('IWDecision').focus();
            return false;
        }

        // debugger;
        var pdd, pmm, pyy;
        var pfinldat;
        var pdat = document.getElementById('pDate').innerHTML;

        pfinldat = new String(pdat);

        if (pdat.length == 6) {

            pdd = pfinldat.substring(0, 2)
            pmm = pfinldat.substring(2, 4)
            pyy = pfinldat.substring(4, 6)
        }
        if (IWdecn == "A") {
            var ponlydate = pdd + '/' + pmm + '/' + '20' + pyy;
            var prtn = validatedate(ponlydate);
            if (prtn == false) {
                alert("Positive pay date not valid!!");
                return false;
            }
        }

        var pstlmntdt = document.getElementById('stlmnt').value;
        var psesondt = document.getElementById('sesson').value;

        var pfnaldate = '20' + pyy + '/' + pmm + '/' + pdd;
        var pstaldat = new Date(psesondt);
        var ppostdat = new Date(pstlmntdt);
        var pd3 = new Date(pfnaldate);


        if (IWdecn == "A") {
            if (ppostdat <= d3) {
                alert('Positive pay post date!!');
                return false;
            }
            if (pstaldat >= d3) {
                alert('Positive pay stale cheque!!');
                return false;
            }
        }

    }


	if (IWdecn == "A") {
        if (document.getElementById("pAmt").innerHTML !== "Not Found") {
            if (Number(document.getElementById('iwAmt').innerHTML.toString().replace(/,/g, '')) !== Number(document.getElementById('pAmt').innerHTML)) {
                alert('Amount is not match with positive pay amount!');
                return false;
            }
        }

        if (document.getElementById("pDate").innerHTML !== "Not Found") {
            if (document.getElementById('ChqDate').value !== document.getElementById('pDate').innerHTML) {
                alert('Date is not match with positive pay date!');
                return false;
            }
        }
    }
    //------------------END-----------------------------//



    var tempacnt = document.getElementById('cnt').value;
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
    //-------------------------ChqDate-------------------------------------------------//
    var dd, mm, yy;
    var finldat;

    //debugger;
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
                return false;
            }
        }

    }
    //------------------------------------Post Date and Stale Cheques ----///

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
//---------------------
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
                document.getElementById('ChqDate').focus();
                document.getElementById('ChqDate').select();
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
                document.getElementById('ChqDate').focus();
                document.getElementById('ChqDate').select();
                return false;
            }
            if ((lyear == true) && (dd > 29)) {
                alert('Invalid date!');
                document.getElementById('ChqDate').focus();
                document.getElementById('ChqDate').select();
                return false;
            }
        }
    }
    else {
        alert("Invalid date !");
        document.getElementById('ChqDate').focus();
        document.getElementById('ChqDate').select();
        //  document.form1.text1.focus();
        return false;
    }
    //  return true;
}
//--------------------------
function getAIRejectDecrip(recordID) {

    $.ajax({
        url: RootUrl + 'IWL3/getAIRejectDecrp',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: { ID: recordID },
        success: function (data) {
            if (data != null) {                
                document.getElementById('AIrejectDecrp').style.display = "";
                document.getElementById('AIrejectDecrp').innerHTML = data;
                //document.getElementById("AIrejectDecrp").classList.add("w3-text-red");
            }
        }
    });
}

//-------------------------- Get AI Decision-----------
function getAIDecision(AiId) {

    $("#AIdecision").removeClass();
    document.getElementById("AIdecision").innerHTML = "";

    $.ajax({
        url: RootUrl + 'IWL3/getAIDecision',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: { ID: AiId },
        success: function (data) {
            if (data != null) {
                if (data.replace('"','').replace('"','') == "False") {
                    document.getElementById("AIdecision").innerHTML = "R";
                    document.getElementById("AIdecision").classList.add("w3-text-red");
			getAIRejectDecrip(AiId);

                }
                else if (data.replace('"','').replace('"','') == "") {
                    document.getElementById("AIdecision").innerHTML = "NA";
                    document.getElementById("AIdecision").classList.add("w3-text-red");
                }
	       else{
			document.getElementById("AIdecision").innerHTML = "A";
                    document.getElementById("AIdecision").classList.add("w3-text-green");

                   }
            }
        }
    });
}
//--------------------------

//--------------

function getReturnDecrp(rtncode) {
    //alert('ok');
    var rjctresnl = document.getElementById('rtnlist');
    var rtnlistDescrp = document.getElementById('rtnlistDescrp');
    for (var i = 0; i < rjctresnl.length; i++) {

        if (rtncode == rjctresnl[i].value) {
            // alert(rtnlistDescrp[i].value);
            document.getElementById("L2rejectDecrp").innerHTML = "";
            document.getElementById("L2rejectDecrp").innerHTML = rtnlistDescrp[i].value;
            //document.getElementById('L2rejectDecrp').vlaue = rtnlistDescrp[i].value;
            document.getElementById('L2rejectDecrp').style.display = "";
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

    //================= Amol changes for Hold on 26/08/2023 start =====================
    if (chr == "H") {
        flghold = true;
        document.getElementById('holdreason').style.display = 'block';
        document.getElementById('LocationMaster').value = "";
        document.getElementById('hldrsn').value = "";
        document.getElementById("LocationMaster").focus();
    }
    //================= Amol changes for Hold on 26/08/2023 end =====================

    if (chr == "R") {
        if (iwrk == "") {
            document.getElementById('rtncd').style.display = "";
            document.getElementById('IWRemark').style.width = "30px";
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
    //debugger;
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

function setCharAt(str, index, chr) {
    if (index > str.length - 1) return str;
    return str.substr(0, index) + chr + str.substr(index + 1);
}

//================= Amol changes for Hold on 26/08/2023 start =====================
function setholdreason() {
    debugger;
    var holdLocationId = document.getElementById("LocationMaster").value;
    if (holdLocationId == "" || holdLocationId == null) {
        alert('Please select hold location!!');
        document.getElementById("LocationMaster").focus();
        return false;
    }

    var hldreason = document.getElementById("hldrsn").value;
    if (hldreason == "") {
        alert('Please enter hold reason!!');
        document.getElementById("hldrsn").focus();
        return false;
    }
    else if (hldreason.length < 5) {

        alert('Please enter minimum 5 character!!');
        document.getElementById("hldrsn").focus();
        return false;
    }
    flghold = false;
    HoldReasonText = document.getElementById('hldrsn').value;
    IsChqHold = HoldReasonText;
    HoldLocationId = document.getElementById('LocationMaster').value;
    document.getElementById('holdreason').style.display = 'none';
    //document.getElementById('hldrsn').value = "";

}
//================= Amol changes for Hold on 26/08/2023 end =====================


