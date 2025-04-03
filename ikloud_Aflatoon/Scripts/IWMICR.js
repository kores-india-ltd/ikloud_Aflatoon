
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var micr;
var cnt;
var tempcnt;
var chq;
var srtcd;
var sancd;
var trcd;
var next_idx = 0;
var tot_idx = 0;
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

    //console.log("pending count: " + document.getElementById('Pending_Count_MICR').value);
    //document.getElementById('MICR_PendingCount').innerHTML = document.getElementById('Pending_Count_MICR').value;

    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].Id)
    }
    //-------------Full Image Calling---------------
    $('#ChqnoQC,#SortQC,#SANQC,#TransQC').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });

    $("#ChqnoQC,#SortQC,#SANQC,#TransQC").keydown(function (event) {
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

    if (tt.length > 0) {

        if (document.getElementById('nodata').value != false) {
            var gf = $("#DEbySnpt").val();
            //  alert(gf);
            if (gf == "True") {
                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                //document.getElementById('AcSnipimg').style.display = "";
                //document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                //document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                //document.getElementById('AcSnipimg').style.display = "none";
                //document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                //document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[1].FrontTiffImagePath;
            //--------------Filling value from OCR1--------------
            document.getElementById('ChqnoQC').value = tt[1].XMLSerialNo;
            document.getElementById('SortQC').value = tt[1].XMLPayorBankRoutNo;
            document.getElementById('SANQC').value = tt[1].XMLSAN;
            document.getElementById('TransQC').value = tt[1].XMLTransCode;

            //document.getElementById('ChqnoQC').value = tt[1].SerialNoOCR1;
            //document.getElementById('SortQC').value = tt[1].PayorBankRoutNoOCR1;
            //document.getElementById('SANQC').value = tt[1].SANOCR1;
            //document.getElementById('TransQC').value = tt[1].TransCodeOCR1;
            //---------------Hilighting with red color---------------
            if (tt[1].MICRRepairFlags.substring(0, 1) == "1") {
                document.getElementById('ChqnoQC').style.color = "red";
                chq = true;
            }
            if (tt[1].MICRRepairFlags.substring(1, 2) == "1") {
                document.getElementById('SortQC').style.color = "red";
                srtcd = true;
            }
            if (tt[1].MICRRepairFlags.substring(2, 3) == "1") {
                document.getElementById('SANQC').style.color = "red";
                sancd = true;
            }
            if (tt[1].MICRRepairFlags.substring(3, 4) == "1") {
                document.getElementById('TransQC').style.color = "red";
                trcd = true;
            }
            //---------------Setting focus to textbox----------------//
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();

            //if (chq == true) {
            //    document.getElementById('ChqnoQC').focus();
            //    document.getElementById('ChqnoQC').select();
            //}
            //else if (srtcd == true) {
            //    document.getElementById('SortQC').focus();
            //    document.getElementById('SortQC').select();
            //}
            //else if (sancd == true) {
            //    document.getElementById('SANQC').focus();
            //    document.getElementById('SANQC').select();
            //}
            //else if (trcd == true) {
            //    document.getElementById('TransQC').focus();
            //    document.getElementById('TransQC').select();
            //}
            //document.getElementById('iwDateQc').focus();
            document.getElementById("btnback").disabled = true;
        }
    }

    $("#btnok").click(function () {
        // debugger;
        var result = Dvalid();
        console.log(result);
        if (result == false) {
            document.getElementById("btnok").disabled = false;
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
            var iwMICR = "iwMICR";
            console.log(backbtn);
            if (backbtn == true) {

                var tot = MatchWith(backcnt);
                console.log(backbtn);
                if (tot == 0) {
                    cnrslt = confirm("Entered value is not maching with XML/D1/D2\n are sure to accept with this value?");
                    if (cnrslt == false) {
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
                    }
                }
                else {
                    cnrslt = true;
                }
                iwMICR = iwMICR + backcnt
                micr = {
                    "EntrySerialNo": $("#ChqnoQC").val(),
                    "EntryPayorBankRoutNo": $("#SortQC").val(),
                    "EntrySAN": $("#SANQC").val(),
                    "EntryTransCode": $("#TransQC").val(),
                    "ID": tt[backcnt].ID,
                    "sttsdtqc": cnrslt,
                    "XMLSerialNo": tt[backcnt].XMLSerialNo,
                    "SerialNoOCR1": tt[backcnt].SerialNoOCR1,
                    "SerialNoOCR2": tt[backcnt].SerialNoOCR2,
                    "XMLPayorBankRoutNo": tt[backcnt].XMLPayorBankRoutNo,
                    "PayorBankRoutNoOCR1": tt[backcnt].PayorBankRoutNoOCR1,
                    "PayorBankRoutNoOCR2": tt[backcnt].PayorBankRoutNoOCR2,
                    "XMLSAN": tt[backcnt].XMLSAN,
                    "SANOCR1": tt[backcnt].SANOCR1,
                    "SANOCR2": tt[backcnt].SANOCR2,
                    "XMLTransCode": tt[backcnt].XMLTransCode,
                    "TransCodeOCR1": tt[backcnt].TransCodeOCR1,
                    "TransCodeOCR2": tt[backcnt].TransCodeOCR2,
                    "MICRRepairFlags": tt[backcnt].MICRRepairFlags,
                };
            }

            else {
                var tot1 = MatchWith(tempcnt);
                console.log(tot1);
                if (tot1 == 0) {
                    cnrslt = confirm("Entered value is not maching with XML/D1/D2\n are sure to accept with this value?");
                    if (cnrslt == false) {
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
                    }
                }
                else {
                    cnrslt = true;
                }
                iwMICR = iwMICR + cnt;
                micr = {
                    "EntrySerialNo": $("#ChqnoQC").val(),
                    "EntryPayorBankRoutNo": $("#SortQC").val(),
                    "EntrySAN": $("#SANQC").val(),
                    "EntryTransCode": $("#TransQC").val(),
                    "ID": tt[tempcnt].ID,
                    "sttsdtqc": cnrslt,
                    "XMLSerialNo": tt[tempcnt].XMLSerialNo,
                    "SerialNoOCR1": tt[tempcnt].SerialNoOCR1,
                    "SerialNoOCR2": tt[tempcnt].SerialNoOCR2,
                    "XMLPayorBankRoutNo": tt[tempcnt].XMLPayorBankRoutNo,
                    "PayorBankRoutNoOCR1": tt[tempcnt].PayorBankRoutNoOCR1,
                    "PayorBankRoutNoOCR2": tt[tempcnt].PayorBankRoutNoOCR2,
                    "XMLSAN": tt[tempcnt].XMLSAN,
                    "SANOCR1": tt[tempcnt].SANOCR1,
                    "SANOCR2": tt[tempcnt].SANOCR2,
                    "XMLTransCode": tt[tempcnt].XMLTransCode,
                    "TransCodeOCR1": tt[tempcnt].TransCodeOCR1,
                    "TransCodeOCR2": tt[tempcnt].TransCodeOCR2,
                    "MICRRepairFlags": tt[tempcnt].MICRRepairFlags,
                };

            }
            console.log(cnrslt);
            if (cnrslt == true) {
                common(iwMICR);

            }
            else {
                // alert('Okk');
                document.getElementById('ChqnoQC').focus();
                document.getElementById("btnback").disabled = true;
            }
            backbtn = false;
            $("input[value='Accept']").attr("disabled", false);
            document.getElementById('countDiv').style.display = 'none';
        }
    });

    //-------------------------------------Reject--------------------------------//
    $("#btnRejct").click(function () {


        document.getElementById("btnback").disabled = false;
        cnt = document.getElementById('cnt').value;
        tempcnt = document.getElementById('tempcnt').value;
        var iwMICR = "iwMICR";

        if (backbtn == true) {

            iwMICR = iwMICR + backcnt
            micr = {
                "EntrySerialNo": $("#ChqnoQC").val(),
                "EntryPayorBankRoutNo": $("#SortQC").val(),
                "EntrySAN": $("#SANQC").val(),
                "EntryTransCode": $("#TransQC").val(),
                "ID": tt[backcnt].ID,
                "sttsdtqc": cnrslt,
                "XMLSerialNo": tt[backcnt].XMLSerialNo,
                "SerialNoOCR1": tt[backcnt].SerialNoOCR1,
                "SerialNoOCR2": tt[backcnt].SerialNoOCR2,
                "XMLPayorBankRoutNo": tt[backcnt].XMLPayorBankRoutNo,
                "PayorBankRoutNoOCR1": tt[backcnt].PayorBankRoutNoOCR1,
                "PayorBankRoutNoOCR2": tt[backcnt].PayorBankRoutNoOCR2,
                "XMLSAN": tt[backcnt].XMLSAN,
                "SANOCR1": tt[backcnt].SANOCR1,
                "SANOCR2": tt[backcnt].SANOCR2,
                "XMLTransCode": tt[backcnt].XMLTransCode,
                "TransCodeOCR1": tt[backcnt].TransCodeOCR1,
                "TransCodeOCR2": tt[backcnt].TransCodeOCR2,
                "MICRRepairFlags": tt[backcnt].MICRRepairFlags,
            };
        }

        else {

            iwMICR = iwMICR + cnt;
            micr = {
                "EntrySerialNo": $("#ChqnoQC").val(),
                "EntryPayorBankRoutNo": $("#SortQC").val(),
                "EntrySAN": $("#SANQC").val(),
                "EntryTransCode": $("#TransQC").val(),
                "ID": tt[tempcnt].ID,
                "sttsdtqc": cnrslt,
                "XMLSerialNo": tt[tempcnt].XMLSerialNo,
                "SerialNoOCR1": tt[tempcnt].SerialNoOCR1,
                "SerialNoOCR2": tt[tempcnt].SerialNoOCR2,
                "XMLPayorBankRoutNo": tt[tempcnt].XMLPayorBankRoutNo,
                "PayorBankRoutNoOCR1": tt[tempcnt].PayorBankRoutNoOCR1,
                "PayorBankRoutNoOCR2": tt[tempcnt].PayorBankRoutNoOCR2,
                "XMLSAN": tt[tempcnt].XMLSAN,
                "SANOCR1": tt[tempcnt].SANOCR1,
                "SANOCR2": tt[tempcnt].SANOCR2,
                "XMLTransCode": tt[tempcnt].XMLTransCode,
                "TransCodeOCR1": tt[tempcnt].TransCodeOCR1,
                "TransCodeOCR2": tt[tempcnt].TransCodeOCR2,
                "MICRRepairFlags": tt[tempcnt].MICRRepairFlags,
            };

        }
        common(iwMICR);
        backbtn = false;
    });

    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;

        if (Modernizr.localstorage) {

            backbtn = true;
            var iwMICR = "iwMICR"
            cnt = document.getElementById('cnt').value;
            iwMICR = iwMICR + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(iwMICR));

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

            document.getElementById('ChqnoQC').value = orderData.EntrySerialNo;
            document.getElementById('SortQC').value = orderData.EntryPayorBankRoutNo;
            document.getElementById('SANQC').value = orderData.EntrySAN;
            document.getElementById('TransQC').value = orderData.EntryTransCode;
            document.getElementById('ChqnoQC').focus();
        }

    });

    //------------------------------------------------------------
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
        //alert('Aila');
        next_idx = $('input[type=text]').index(this) + 1;
        tot_idx = $('body').find('input[type=text]').length;
        if (e.keyCode == 13) {
            if (tot_idx == next_idx) {
                $("input[value='Accept']").attr("disabled", true);
                $("input[value='Accept']").focus().click();
                //
            }

            else
                //go to the next text input element.
                $('input[type=text]:eq(' + next_idx + ')').focus().select();
            /// $("input[value='Accept']").focus().click();
            //                $('button[type=submit] .NavButton').click();
            // return true;
        }
    });

    $("#bbtnClose").click(function () {

        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;

            if (scond == true) {
                for (var i = 0; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwMICR" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.EntrySerialNo);
                    arrlist.push(orderData.EntryPayorBankRoutNo);
                    arrlist.push(orderData.EntrySAN);
                    arrlist.push(orderData.EntryTransCode);
                    arrlist.push(orderData.sttsdtqc);
                    arrlist.push(orderData.XMLSerialNo);
                    arrlist.push(orderData.SerialNoOCR1);
                    arrlist.push(orderData.SerialNoOCR2);
                    arrlist.push(orderData.XMLPayorBankRoutNo);
                    arrlist.push(orderData.PayorBankRoutNoOCR1);
                    arrlist.push(orderData.PayorBankRoutNoOCR2);
                    arrlist.push(orderData.XMLSAN);
                    arrlist.push(orderData.SANOCR1);
                    arrlist.push(orderData.SANOCR2);
                    arrlist.push(orderData.XMLTransCode);
                    arrlist.push(orderData.TransCodeOCR1);
                    arrlist.push(orderData.TransCodeOCR2);
                    arrlist.push(orderData.MICRRepairFlags);
                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwMICR" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.EntrySerialNo);
                    arrlist.push(orderData.EntryPayorBankRoutNo);
                    arrlist.push(orderData.EntrySAN);
                    arrlist.push(orderData.EntryTransCode);
                    arrlist.push(orderData.sttsdtqc);
                    arrlist.push(orderData.XMLSerialNo);
                    arrlist.push(orderData.SerialNoOCR1);
                    arrlist.push(orderData.SerialNoOCR2);
                    arrlist.push(orderData.XMLPayorBankRoutNo);
                    arrlist.push(orderData.PayorBankRoutNoOCR1);
                    arrlist.push(orderData.PayorBankRoutNoOCR2);
                    arrlist.push(orderData.XMLSAN);
                    arrlist.push(orderData.SANOCR1);
                    arrlist.push(orderData.SANOCR2);
                    arrlist.push(orderData.XMLTransCode);
                    arrlist.push(orderData.TransCodeOCR1);
                    arrlist.push(orderData.TransCodeOCR2);
                    arrlist.push(orderData.MICRRepairFlags);
                }
            }

            $.ajax({

                url: RootUrl + 'IWDataEntry/IWMICR',
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

    $("#ChqnoQC,#SortQC,#SANQC,#TransQC").keypress(function (event) {

        // alert(event.keyCode);
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
        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
            // alert('if');
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
    function common(val) {
        //alert('cool!!');
        if (Modernizr.localstorage) {
            var localacct = window.localStorage;
            var chqiwmicr = JSON.stringify(micr);
            localacct.setItem(val, chqiwmicr);
        }

        if (backbtn == true) {
            document.getElementById('cnt').value = parseInt(backcnt) + 1;
        }
        else {
            document.getElementById('cnt').value = parseInt(cnt) + 1;
        }
        cnt = document.getElementById('cnt').value;
        console.log("cnt " + cnt);
        console.log("tt length " + tt.length);
        //document.getElementById('MICR_PendingCount').innerHTML = document.getElementById('Pending_Count_MICR').value;
        if (cnt < tt.length) {

            if (document.getElementById('DEbySnpt').value == "True") {

                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                //document.getElementById('AcSnipimg').style.display = "";
                //document.getElementById("myfulimg").src = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                //document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                //document.getElementById('AcSnipimg').style.display = "none";
                //document.getElementById("myfulimg").src = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                //document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[cnt].FrontTiffImagePath;
            //--------------Filling value from OCR1--------------
            document.getElementById('ChqnoQC').value = tt[cnt].XMLSerialNo;
            document.getElementById('SortQC').value = tt[cnt].XMLPayorBankRoutNo;
            document.getElementById('SANQC').value = tt[cnt].XMLSAN;
            document.getElementById('TransQC').value = tt[cnt].XMLTransCode;

            //document.getElementById('ChqnoQC').value = tt[cnt].SerialNoOCR1;
            //document.getElementById('SortQC').value = tt[cnt].PayorBankRoutNoOCR1;
            //document.getElementById('SANQC').value = tt[cnt].SANOCR1;
            //document.getElementById('TransQC').value = tt[cnt].TransCodeOCR1;
            //---------------Hilighting with red color---------------
            if (tt[cnt].MICRRepairFlags.substring(0, 1) == "1") {
                document.getElementById('ChqnoQC').style.color = "red";
                chq = true;
            }
            if (tt[cnt].MICRRepairFlags.substring(1, 2) == "1") {
                document.getElementById('SortQC').style.color = "red";
                srtcd = true;
            }
            if (tt[cnt].MICRRepairFlags.substring(2, 3) == "1") {
                document.getElementById('SANQC').style.color = "red";
                sancd = true;
            }
            if (tt[cnt].MICRRepairFlags.substring(3, 4) == "1") {
                document.getElementById('TransQC').style.color = "red";
                trcd = true;
            }
            document.getElementById('ChqnoQC').focus();
            document.getElementById('ChqnoQC').select();
            //---------------Setting focus to textbox----------------//
            //if (chq == true) {
            //    document.getElementById('ChqnoQC').focus();
            //    document.getElementById('ChqnoQC').select();
            //}
            //else if (srtcd == true) {
            //    document.getElementById('SortQC').focus();
            //    document.getElementById('SortQC').select();
            //}
            //else if (sancd == true) {
            //    document.getElementById('SANQC').focus();
            //    document.getElementById('SANQC').select();
            //}
            //else if (trcd == true) {
            //    document.getElementById('TransQC').focus();
            //    document.getElementById('TransQC').select();
            //}
            if (backbtn == true) {
                document.getElementById('tempcnt').value = parseInt(backcnt) + 1;
            }
            else {
                document.getElementById('tempcnt').value = parseInt(tempcnt) + 1;
            }
            backbtn = false;
            $("input[value='Accept']").attr("disabled", false);
            document.getElementById('countDiv').style.display = 'none';
        }
        else if (cnt > 0) {
            console.log("In else if");
            if (Modernizr.localstorage) {
                var listItems = [];
                var arrlist = [];
                var localData = window.localStorage;
                console.log(scond);
                if (scond == true) {
                    for (var i = 0; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwMICR" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.EntrySerialNo);
                        arrlist.push(orderData.EntryPayorBankRoutNo);
                        arrlist.push(orderData.EntrySAN);
                        arrlist.push(orderData.EntryTransCode);
                        arrlist.push(orderData.sttsdtqc);
                        arrlist.push(orderData.XMLSerialNo);
                        arrlist.push(orderData.SerialNoOCR1);
                        arrlist.push(orderData.SerialNoOCR2);
                        arrlist.push(orderData.XMLPayorBankRoutNo);
                        arrlist.push(orderData.PayorBankRoutNoOCR1);
                        arrlist.push(orderData.PayorBankRoutNoOCR2);
                        arrlist.push(orderData.XMLSAN);
                        arrlist.push(orderData.SANOCR1);
                        arrlist.push(orderData.SANOCR2);
                        arrlist.push(orderData.XMLTransCode);
                        arrlist.push(orderData.TransCodeOCR1);
                        arrlist.push(orderData.TransCodeOCR2);
                        arrlist.push(orderData.MICRRepairFlags);
                    }
                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwMICR" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.EntrySerialNo);
                        arrlist.push(orderData.EntryPayorBankRoutNo);
                        arrlist.push(orderData.EntrySAN);
                        arrlist.push(orderData.EntryTransCode);
                        arrlist.push(orderData.sttsdtqc);
                        arrlist.push(orderData.XMLSerialNo);
                        arrlist.push(orderData.SerialNoOCR1);
                        arrlist.push(orderData.SerialNoOCR2);
                        arrlist.push(orderData.XMLPayorBankRoutNo);
                        arrlist.push(orderData.PayorBankRoutNoOCR1);
                        arrlist.push(orderData.PayorBankRoutNoOCR2);
                        arrlist.push(orderData.XMLSAN);
                        arrlist.push(orderData.SANOCR1);
                        arrlist.push(orderData.SANOCR2);
                        arrlist.push(orderData.XMLTransCode);
                        arrlist.push(orderData.TransCodeOCR1);
                        arrlist.push(orderData.TransCodeOCR2);
                        arrlist.push(orderData.MICRRepairFlags);
                    }
                }
                console.log("calling ajax");
                //------------------------------- Calling Ajax for taking more data------------------
                // alert(cnt);
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;
                $.ajax({

                    url: RootUrl + 'IWDataEntry/IWMICR',
                    data: JSON.stringify({ lst: arrlist, snd: scond, img: tt[pcnt - 1].FrontGreyImagePath, fulimg: fullimg, idlst: idslst }),

                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',

                    dataType: 'json',
                    success: function (result) {
                        if (result == false) {
                            window.location = RootUrl + 'Home/IWIndex?id=1';
                        }
                        else {
                            console.log("In ajax success");
                            tt = result;
                            document.getElementById('tempcnt').value = 1;
                            document.getElementById('cnt').value = 1;
                            cnt = 1;
                            tempcnt = 1;

                            if (tt != null && tt != "") {
                                scond = true;
                                //-------------- idslist--------------------//
                                for (var z = 0; z < tt.length; z++) {
                                    idslst.push(tt[z].ID)
                                }

                                //-------Remove save objects from browser---//
                                window.localStorage.clear();
                                //-------------Saving Last data in storage---
                                var iwMICR = "iwMICR0"
                                var micr = {
                                    "EntrySerialNo": tt[0].EntrySerialNo,
                                    "EntryPayorBankRoutNo": tt[0].EntryPayorBankRoutNo,
                                    "EntrySAN": tt[0].EntrySAN,
                                    "EntryTransCode": tt[0].EntryTransCode,
                                    "ID": tt[0].ID,
                                    "sttsdtqc": tt[0].sttsdtqc,
                                    "XMLSerialNo": tt[0].XMLSerialNo,
                                    "SerialNoOCR1": tt[0].SerialNoOCR1,
                                    "SerialNoOCR2": tt[0].SerialNoOCR2,
                                    "XMLPayorBankRoutNo": tt[0].XMLPayorBankRoutNo,
                                    "PayorBankRoutNoOCR1": tt[0].PayorBankRoutNoOCR1,
                                    "PayorBankRoutNoOCR2": tt[0].PayorBankRoutNoOCR2,
                                    "XMLSAN": tt[0].XMLSAN,
                                    "SANOCR1": tt[0].SANOCR1,
                                    "SANOCR2": tt[0].SANOCR2,
                                    "XMLTransCode": tt[0].XMLTransCode,
                                    "TransCodeOCR1": tt[0].TransCodeOCR1,
                                    "TransCodeOCR2": tt[0].TransCodeOCR2,
                                    "MICRRepairFlags": tt[0].MICRRepairFlags,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqiwmicr = JSON.stringify(micr);
                                    localacct.setItem(iwMICR, chqiwmicr);

                                }
                                // alert(tt[1].FrontGreyImagePath);
                                //----------------------------------------------------------------------//
                                if (document.getElementById('DEbySnpt').value == "True") {
                                    document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                    //document.getElementById('AcSnipimg').style.display = "";
                                    //document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                                    //document.getElementById('AcFullimg').style.display = "none";
                                }
                                else {
                                    document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                                    //document.getElementById('AcSnipimg').style.display = "none";
                                    //document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                                    //document.getElementById('AcFullimg').style.display = "";
                                }

                                fullimg = tt[1].FrontTiffImagePath;
                                //--------------Filling value from OCR1--------------
                                document.getElementById('ChqnoQC').value = tt[1].XMLSerialNo;
                                document.getElementById('SortQC').value = tt[1].XMLPayorBankRoutNo;
                                document.getElementById('SANQC').value = tt[1].XMLSAN;
                                document.getElementById('TransQC').value = tt[1].XMLTransCode;

                                //document.getElementById('ChqnoQC').value = tt[1].SerialNoOCR2;
                                //document.getElementById('SortQC').value = tt[1].PayorBankRoutNoOCR2;
                                //document.getElementById('SANQC').value = tt[1].SANOCR2;
                                //document.getElementById('TransQC').value = tt[1].TransCodeOCR2;
                                //---------------Hilighting with red color---------------
                                if (tt[1].MICRRepairFlags.substring(0, 1) == "1") {
                                    document.getElementById('ChqnoQC').style.color = "red";
                                    chq = true;
                                }
                                if (tt[1].MICRRepairFlags.substring(1, 2) == "1") {
                                    document.getElementById('SortQC').style.color = "red";
                                    srtcd = true;
                                }
                                if (tt[1].MICRRepairFlags.substring(2, 3) == "1") {
                                    document.getElementById('SANQC').style.color = "red";
                                    sancd = true;
                                }
                                if (tt[1].MICRRepairFlags.substring(3, 4) == "1") {
                                    document.getElementById('TransQC').style.color = "red";
                                    trcd = true;
                                }
                                //---------------Setting focus to textbox----------------//
                                document.getElementById('ChqnoQC').focus();
                                document.getElementById('ChqnoQC').select();
                                //if (chq == true) {
                                //    document.getElementById('ChqnoQC').focus();
                                //    document.getElementById('ChqnoQC').select();
                                //}
                                //else if (srtcd == true) {
                                //    document.getElementById('SortQC').focus();
                                //    document.getElementById('SortQC').select();
                                //}
                                //else if (sancd == true) {
                                //    document.getElementById('SANQC').focus();
                                //    document.getElementById('SANQC').select();
                                //}
                                //else if (trcd == true) {
                                //    document.getElementById('TransQC').focus();
                                //    document.getElementById('TransQC').select();
                                //}
                                $("input[value='Accept']").attr("disabled", false);
                                document.getElementById('countDiv').style.display = 'none';
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


    $("#btnShowCount").click(function () {
        document.getElementById('countDiv').style.display = '';
        $.ajax({
            url: RootUrl + 'IWDataEntry/ShowPendingCount',
            contentType: 'application/json; charset=utf-8',
            dataType: 'JSON',
            data: {},
            success: function (data) {
                console.log(data);
                document.getElementById('Total_Count').innerText = data.Total_Count;
                document.getElementById('MICR_PendingCount').innerText = data.MICR_Pending_Count;
                document.getElementById('ICR_PendingCount').innerText = data.ICR_Pending_Count;
            }
        });
    });
});

function Dvalid() {

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
    else if (SortQC.length < 9 || SortQC == "000000000" || isNaN(SortQC)) {
        alert("Sort code is not valid !");
        srtcd = true;
        chq = false;
        return false;
    }
    else if (ChqnoQC.length < 6 || ChqnoQC == "000000" || isNaN(ChqnoQC)) {
        alert("Cheque number is not valid !");
        chq = true;
        return false;
    }
    else if (SANQC.length < 6 || isNaN(SANQC)) {
        alert("SAN code is not valid !");
        sancd = true;
        srtcd = false;
        chq = false;
        return false;
    }
    else if (TransQC.length < 2 || TransQC == "00" || TransQC.substring(0, 1) == "0" || isNaN(TransQC)) {
        alert("Trans code is not valid !");
        trcd = true;
        sancd = false;
        srtcd = false;
        chq = false;
        return false;
    }
    // debugger;
    var rtnflg = validYrnscodes();
    trcd = true;
    sancd = false;
    srtcd = false;
    chq = false;
    return rtnflg;


}

function validYrnscodes() {
    var temptrnscd = document.getElementById('TransQC').value;
    var flg;
    $.ajax({

        url: RootUrl + "IWDataEntry/ValidTrans",
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
function MatchWith(indexvl) {
    var match = 0;

    if (document.getElementById("BankCode").value == "641") {
        match = 1;
    }
    else {
        var tempchqno = document.getElementById('ChqnoQC').value;
        var tempsortcd = document.getElementById('SortQC').value;
        var tempsan = document.getElementById('SANQC').value;
        var temptrnscd = document.getElementById('TransQC').value;


        if ((tt[indexvl].XMLSerialNo == tempchqno) && (tt[indexvl].XMLPayorBankRoutNo == tempsortcd) && (tt[indexvl].XMLSAN == tempsan) && (tt[indexvl].XMLTransCode == temptrnscd)) {
            match = 1;
            // alert(tt[indexvl].ICRDate + " :" + finaldate);
        }
        else if ((tt[indexvl].SerialNoOCR1 == tempchqno) && (tt[indexvl].PayorBankRoutNoOCR1 == tempsortcd) && (tt[indexvl].SANOCR1 == tempsan) && (tt[indexvl].TransCodeOCR1 == temptrnscd)) {
            match = 1;
            //alert(tt[indexvl].EntryDate + " :" + finaldate);
        }
        else if ((tt[indexvl].SerialNoOCR2 == tempchqno) && (tt[indexvl].PayorBankRoutNoOCR2 == tempsortcd) && (tt[indexvl].SANOCR2 == tempsan) && (tt[indexvl].TransCodeOCR2 == temptrnscd)) {
            match = 1;
            //alert(tt[indexvl].EntryDate + " :" + finaldate);
        }
        // alert((icr + mnl));
    }
    
    return (match);
}

function fullImage() {
    //  alert('ok');
    document.getElementById('iwimg').style.display = 'block';
    // alert(document.getElementById('myimg').data);
    //document.getElementById('modelmyfulimg').src = fullimg;// document.getElementById('myimg').src;
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}

function fullImageTiff() {
    //  alert('ok');
    document.getElementById('iwimg1').style.display = 'block';
    // alert(document.getElementById('myimg').data);
    //document.getElementById('modelmyfulimg').src = fullimg;// document.getElementById('myimg').src;
    document.getElementById('myfulimg1').src = document.getElementById('myimg1').src;
}

var value = 0;
callrotate = function () {
    value += 180;
    $("#myimg,#ftiffimg").rotate({ animateTo: value });
}

function ChangeImage(imagetype) {
    // alert(imagetype);
    debugger;
    var ImagePath = "";
    var indexcnt = document.getElementById('cnt').value;
    if (imagetype == "FTiff") {
        ImagePath = tt[indexcnt].FrontTiffImagePath;
        //document.getElementById('myfulimg').src = tt[indexcnt].FrontTiffImagePath.replace(/_G/g, "F");

        $.ajax({
            url: RootUrl + 'IWDataEntry/getTiffImageNew',
            dataType: 'html',
            data: { httpwebimgpath: ImagePath },
            success: function (Slipdata) {
                //debugger;
                $('#divtiff').html(Slipdata);
                //document.getElementById('myimg').src = Slipdata;
                document.getElementById('myimg').style.display = "none";
                document.getElementById('divtiff').style.display = "block";

            }
        });
    }
    else if (imagetype == "BTiff") {
        //alert('Browser not supporting!!!');
        //document.getElementById('myfulimg').src = tt[indexcnt].FrontTiffImagePath.replace(/_G/g, "_B");
        ImagePath = tt[indexcnt].BackTiffImagePath;

        $.ajax({
            url: RootUrl + 'IWDataEntry/getTiffImageNew',
            dataType: 'html',
            data: { httpwebimgpath: ImagePath },
            success: function (Slipdata) {
                //debugger;
                $('#divtiff').html(Slipdata);
                //document.getElementById('myimg').src = Slipdata;
                document.getElementById('myimg').style.display = "none";
                document.getElementById('divtiff').style.display = "block";

            }
        });
    }
    else if (imagetype == "FGray") {

        //document.getElementById('myfulimg').src = tt[indexcnt].FrontGreyImagePath;
        document.getElementById('myimg').style.display = "block";
        document.getElementById('divtiff').style.display = "none";
        document.getElementById('myimg').src = tt[indexcnt].FrontGreyImagePath;
    }

}

