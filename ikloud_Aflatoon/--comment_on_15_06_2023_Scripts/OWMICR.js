
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var Status = 0;
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
var activefullimg = false;
var validateflg = false;
//var scondbck = false;
function passval(array) {
    data = JSON.stringify(array);
    tt = JSON.parse(data);

    lesscnt = tt.length;
    backbtn = false;
    backcnt = 0;
}


$(document).ready(function () {
    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].Id)
    }
    //------------ idslist end----------------//
    $('#ChqnoQC,#SortQC,#SANQC,#TransQC').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {
        e.preventDefault();
    });
    //-------------Full Image Calling---------------

    $("#ChqnoQC,#SortQC,#SANQC,#TransQC").keydown(function (event) {
        //alert(event.keyCode);
        //alert(fullimg);
        if (event.keyCode == 113) {

            document.getElementById('AcSnipimg').style.display = "none";
            document.getElementById("myfulimg").src = fullimg;// document.getElementById("FrontGrayImgPath").value;
            document.getElementById('AcFullimg').style.display = "";
            activefullimg = true;
        }
        else if (event.keyCode == 115) {

            document.getElementById('AcSnipimg').style.display = "";
            document.getElementById("myfulimg").src = fullimg;// document.getElementById("FrontGrayImgPath").value;   fullimg = tt[1].FrontTiffImagePath;
            document.getElementById('AcFullimg').style.display = "none";
            activefullimg = false;
        }

    });

    if (tt.length > 0) {
      
        if (document.getElementById('nodata').value != false) {
            //var gf = ;
            //  alert(gf);
            if ($("#DEbySnpt").val() == "1") {
                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
                activefullimg = false;
            }
            else {
                document.getElementById('myimg').src = tt[1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").src = tt[1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
                activefullimg = true;
            }
            fullimg = tt[1].FrontTiffImagePath;
            //--------------Filling value from OCR1--------------
            document.getElementById('ChqnoQC').value = tt[1].ChequeNoMICR;
            document.getElementById('SortQC').value = tt[1].SortCodeMICR;
            document.getElementById('SANQC').value = tt[1].SANMICR;
            document.getElementById('TransQC').value = tt[1].TransCodeMICR;
            //---------------Hilighting with red color---------------
            if (tt[1].MICRRepairStatus.substring(0, 1) == "1") {
                document.getElementById('ChqnoQC').style.color = "red";
                chq = true;
            }
            if (tt[1].MICRRepairStatus.substring(1, 2) == "1") {
                document.getElementById('SortQC').style.color = "red";
                srtcd = true;
            }
            if (tt[1].MICRRepairStatus.substring(2, 3) == "1") {
                document.getElementById('SANQC').style.color = "red";
                sancd = true;
            }
            if (tt[1].MICRRepairStatus.substring(3, 4) == "1") {
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
        var result = Dvalid()
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

            if (backbtn == true) {

                var tot = 1;//MatchWith(backcnt);
                if (tot == 0) {
                    validateflg = confirm("Entered value is not maching with XML/D1/D2\n are sure to accept with this value?");
                    if (validateflg == false) {
                        
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
                    Status = 2;
                    validateflg = true;
                }
                iwMICR = iwMICR + backcnt
                micr = {
                    "EntrySerialNo": $("#ChqnoQC").val(),
                    "EntryPayorBankRoutNo": $("#SortQC").val(),
                    "EntrySAN": $("#SANQC").val(),
                    "EntryTransCode": $("#TransQC").val(),
                    "ID": tt[backcnt].Id,
                    "Status": Status,
                    "ChequeNoMICR": tt[backcnt].ChequeNoMICR,
                    "SortCodeMICR": tt[backcnt].SortCodeMICR,
                    "SANMICR": tt[backcnt].SANMICR,
                    "TransCodeMICR": tt[backcnt].TransCodeMICR,
                    "ChequeNoNI": tt[backcnt].ChequeNoNI,
                    "SortCodeNI": tt[backcnt].SortCodeNI,
                    "SANNI": tt[backcnt].SANNI,
                    "TransCodeNI": tt[backcnt].TransCodeNI,
                    "ChequeNoPara": tt[backcnt].ChequeNoPara,
                    "SortCodePara": tt[backcnt].SortCodePara,
                    "SANPara": tt[backcnt].SANPara,
                    "TransCodePara": tt[backcnt].TransCodePara,
                    "MICRRepairStatus": tt[backcnt].MICRRepairStatus,
                    "RawDataId": tt[backcnt].RawDataId,
                    "CustomerId": tt[backcnt].CustomerId,
                    "DomainId": tt[backcnt].DomainId,
                    "ScanningNodeId": tt[backcnt].ScanningNodeId,
                };
            }

            else {
                var tot1 = 1;//MatchWith(tempcnt);
                if (tot1 == 0) {
                    validateflg = confirm("Entered value is not maching with XML/D1/D2\n are sure to accept with this value?");
                    if (validateflg == false) {
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
                    Status = 2;
                    validateflg = true;
                }
                iwMICR = iwMICR + cnt;
                micr = {
                    "EntrySerialNo": $("#ChqnoQC").val(),
                    "EntryPayorBankRoutNo": $("#SortQC").val(),
                    "EntrySAN": $("#SANQC").val(),
                    "EntryTransCode": $("#TransQC").val(),
                    "ID": tt[tempcnt].Id,
                    "Status": Status,
                    "ChequeNoMICR": tt[tempcnt].ChequeNoMICR,
                    "SortCodeMICR": tt[tempcnt].SortCodeMICR,
                    "SANMICR": tt[tempcnt].SANMICR,
                    "TransCodeMICR": tt[tempcnt].TransCodeMICR,
                    "ChequeNoNI": tt[tempcnt].ChequeNoNI,
                    "SortCodeNI": tt[tempcnt].SortCodeNI,
                    "SANNI": tt[tempcnt].SANNI,
                    "TransCodeNI": tt[tempcnt].TransCodeNI,
                    "ChequeNoPara": tt[tempcnt].ChequeNoPara,
                    "SortCodePara": tt[tempcnt].SortCodePara,
                    "SANPara": tt[tempcnt].SANPara,
                    "TransCodePara": tt[tempcnt].TransCodePara,
                    "MICRRepairStatus": tt[tempcnt].MICRRepairStatus,
                    "RawDataId": tt[tempcnt].RawDataId,
                    "CustomerId": tt[tempcnt].CustomerId,
                    "DomainId": tt[tempcnt].DomainId,
                    "ScanningNodeId": tt[tempcnt].ScanningNodeId,
                };

            }
            if (validateflg == true) {
                common(iwMICR);

            }
            else {
                // alert('Okk');
                document.getElementById('ChqnoQC').focus();
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
        var iwMICR = "iwMICR";

        if (backbtn == true) {

            iwMICR = iwMICR + backcnt
            micr = {
                "EntrySerialNo": $("#ChqnoQC").val(),
                "EntryPayorBankRoutNo": $("#SortQC").val(),
                "EntrySAN": $("#SANQC").val(),
                "EntryTransCode": $("#TransQC").val(),
                "ID": tt[backcnt].Id,
                "Status": Status,
                "ChequeNoMICR": tt[backcnt].ChequeNoMICR,
                "SortCodeMICR": tt[backcnt].SortCodeMICR,
                "SANMICR": tt[backcnt].SANMICR,
                "TransCodeMICR": tt[backcnt].TransCodeMICR,
                "ChequeNoNI": tt[backcnt].ChequeNoNI,
                "SortCodeNI": tt[backcnt].SortCodeNI,
                "SANNI": tt[backcnt].SANNI,
                "TransCodeNI": tt[backcnt].TransCodeNI,
                "ChequeNoPara": tt[backcnt].ChequeNoPara,
                "SortCodePara": tt[backcnt].SortCodePara,
                "SANPara": tt[backcnt].SANPara,
                "TransCodePara": tt[backcnt].TransCodePara,
                "MICRRepairStatus": tt[backcnt].MICRRepairStatus,
                "RawDataId": tt[backcnt].RawDataId,
                "CustomerId": tt[backcnt].CustomerId,
                "DomainId": tt[backcnt].DomainId,
                "ScanningNodeId": tt[backcnt].ScanningNodeId,
            };
        }

        else {

            iwMICR = iwMICR + cnt;
            micr = {
                "EntrySerialNo": $("#ChqnoQC").val(),
                "EntryPayorBankRoutNo": $("#SortQC").val(),
                "EntrySAN": $("#SANQC").val(),
                "EntryTransCode": $("#TransQC").val(),
                "ID": tt[tempcnt].Id,
                "Status": Status,
                "ChequeNoMICR": tt[tempcnt].ChequeNoMICR,
                "SortCodeMICR": tt[tempcnt].SortCodeMICR,
                "SANMICR": tt[tempcnt].SANMICR,
                "TransCodeMICR": tt[tempcnt].TransCodeMICR,
                "ChequeNoNI": tt[tempcnt].ChequeNoNI,
                "SortCodeNI": tt[tempcnt].SortCodeNI,
                "SANNI": tt[tempcnt].SANNI,
                "TransCodeNI": tt[tempcnt].TransCodeNI,
                "ChequeNoPara": tt[tempcnt].ChequeNoPara,
                "SortCodePara": tt[tempcnt].SortCodePara,
                "SANPara": tt[tempcnt].SANPara,
                "TransCodePara": tt[tempcnt].TransCodePara,
                "MICRRepairStatus": tt[tempcnt].MICRRepairStatus,
                "RawDataId": tt[tempcnt].RawDataId,
                "CustomerId": tt[tempcnt].CustomerId,
                "DomainId": tt[tempcnt].DomainId,
                "ScanningNodeId": tt[tempcnt].ScanningNodeId,
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

            if (document.getElementById('DEbySnpt').value == "1") {
                document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").src = tt[cnt - 1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
                activefullimg = false;
            }
            else {
                document.getElementById('myimg').src = tt[cnt - 1].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").src = tt[cnt - 1].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
                activefullimg = true;

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
                    arrlist.push(orderData.Status);
                    arrlist.push(orderData.ChequeNoMICR);
                    arrlist.push(orderData.SortCodeMICR);
                    arrlist.push(orderData.SANMICR);
                    arrlist.push(orderData.TransCodeMICR);
                    arrlist.push(orderData.ChequeNoNI);
                    arrlist.push(orderData.SortCodeNI);
                    arrlist.push(orderData.SANNI);
                    arrlist.push(orderData.TransCodeNI);
                    arrlist.push(orderData.ChequeNoPara);
                    arrlist.push(orderData.SortCodePara);
                    arrlist.push(orderData.SANPara);
                    arrlist.push(orderData.TransCodePara);
                    arrlist.push(orderData.MICRRepairStatus);

                    arrlist.push(orderData.RawDataId);
                    arrlist.push(orderData.CustomerId);
                    arrlist.push(orderData.DomainId);
                    arrlist.push(orderData.ScanningNodeId);
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
                    arrlist.push(orderData.Status);
                    arrlist.push(orderData.ChequeNoMICR);
                    arrlist.push(orderData.SortCodeMICR);
                    arrlist.push(orderData.SANMICR);
                    arrlist.push(orderData.TransCodeMICR);
                    arrlist.push(orderData.ChequeNoNI);
                    arrlist.push(orderData.SortCodeNI);
                    arrlist.push(orderData.SANNI);
                    arrlist.push(orderData.TransCodeNI);
                    arrlist.push(orderData.ChequeNoPara);
                    arrlist.push(orderData.SortCodePara);
                    arrlist.push(orderData.SANPara);
                    arrlist.push(orderData.TransCodePara);
                    arrlist.push(orderData.MICRRepairStatus);
                    arrlist.push(orderData.RawDataId);
                    arrlist.push(orderData.CustomerId);
                    arrlist.push(orderData.DomainId);
                    arrlist.push(orderData.ScanningNodeId);
                }
            }

            $.ajax({

                url: RootUrl + 'OWDataEntry/OWMICR',
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
        if (cnt < tt.length) {

            if (document.getElementById('DEbySnpt').value == "1") {

                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").src = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
                activefullimg = false;
            }
            else {
                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").src = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
                activefullimg = true;
            }
            fullimg = tt[cnt].FrontTiffImagePath;
            //--------------Filling value from OCR1--------------
            document.getElementById('ChqnoQC').value = tt[cnt].ChequeNoMICR;
            document.getElementById('SortQC').value = tt[cnt].SortCodeMICR;
            document.getElementById('SANQC').value = tt[cnt].SANMICR;
            document.getElementById('TransQC').value = tt[cnt].TransCodeMICR;
            //---------------Hilighting with red color---------------
            if (tt[cnt].MICRRepairStatus.substring(0, 1) == "1") {
                document.getElementById('ChqnoQC').style.color = "red";
                chq = true;
            }
            if (tt[cnt].MICRRepairStatus.substring(1, 2) == "1") {
                document.getElementById('SortQC').style.color = "red";
                srtcd = true;
            }
            if (tt[cnt].MICRRepairStatus.substring(2, 3) == "1") {
                document.getElementById('SANQC').style.color = "red";
                sancd = true;
            }
            if (tt[cnt].MICRRepairStatus.substring(3, 4) == "1") {
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
        }
        else if (cnt > 0) {

            if (Modernizr.localstorage) {
                var listItems = [];
                var arrlist = [];
                var localData = window.localStorage;
                debugger;
                if (scond == true) {
                    for (var i = 0; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwMICR" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.EntrySerialNo);
                        arrlist.push(orderData.EntryPayorBankRoutNo);
                        arrlist.push(orderData.EntrySAN);
                        arrlist.push(orderData.EntryTransCode);
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.ChequeNoMICR);
                        arrlist.push(orderData.SortCodeMICR);
                        arrlist.push(orderData.SANMICR);
                        arrlist.push(orderData.TransCodeMICR);
                        arrlist.push(orderData.ChequeNoNI);
                        arrlist.push(orderData.SortCodeNI);
                        arrlist.push(orderData.SANNI);
                        arrlist.push(orderData.TransCodeNI);
                        arrlist.push(orderData.ChequeNoPara);
                        arrlist.push(orderData.SortCodePara);
                        arrlist.push(orderData.SANPara);
                        arrlist.push(orderData.TransCodePara);
                        arrlist.push(orderData.MICRRepairStatus);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.ScanningNodeId);
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
                        arrlist.push(orderData.Status);
                        arrlist.push(orderData.ChequeNoMICR);
                        arrlist.push(orderData.SortCodeMICR);
                        arrlist.push(orderData.SANMICR);
                        arrlist.push(orderData.TransCodeMICR);
                        arrlist.push(orderData.ChequeNoNI);
                        arrlist.push(orderData.SortCodeNI);
                        arrlist.push(orderData.SANNI);
                        arrlist.push(orderData.TransCodeNI);
                        arrlist.push(orderData.ChequeNoPara);
                        arrlist.push(orderData.SortCodePara);
                        arrlist.push(orderData.SANPara);
                        arrlist.push(orderData.TransCodePara);
                        arrlist.push(orderData.MICRRepairStatus);
                        arrlist.push(orderData.RawDataId);
                        arrlist.push(orderData.CustomerId);
                        arrlist.push(orderData.DomainId);
                        arrlist.push(orderData.ScanningNodeId);
                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------
                // alert(cnt);
                next_idx = 0;
                tot_idx = 0;
                var pcnt = cnt;
                $.ajax({

                    url: RootUrl + 'OWDataEntry/OWMICR',
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
                                //-------------- idslist--------------------//
                                for (var z = 0; z < tt.length; z++) {
                                    idslst.push(tt[z].Id)
                                }
                                //-------------Saving Last data in storage---
                                var iwMICR = "iwMICR0"
                                var micr = {
                                    "EntrySerialNo": tt[0].ChequeNoFinal,
                                    "EntryPayorBankRoutNo": tt[0].SortCodeFinal,
                                    "EntrySAN": tt[0].SANFinal,
                                    "EntryTransCode": tt[0].TransCodeFinal,
                                    "ID": tt[0].Id,
                                    "Status": Status,
                                    "ChequeNoMICR": tt[0].ChequeNoMICR,
                                    "SortCodeMICR": tt[0].SortCodeMICR,
                                    "SANMICR": tt[0].SANMICR,
                                    "TransCodeMICR": tt[0].TransCodeMICR,
                                    "ChequeNoNI": tt[0].ChequeNoNI,
                                    "SortCodeNI": tt[0].SortCodeNI,
                                    "SANNI": tt[0].SANNI,
                                    "TransCodeNI": tt[0].TransCodeNI,
                                    "ChequeNoPara": tt[0].ChequeNoPara,
                                    "SortCodePara": tt[0].SortCodePara,
                                    "SANPara": tt[0].SANPara,
                                    "TransCodePara": tt[0].TransCodePara,
                                    "MICRRepairStatus": tt[0].MICRRepairStatus,
                                    "RawDataId": tt[0].RawDataId,
                                    "CustomerId": tt[0].CustomerId,
                                    "DomainId": tt[0].DomainId,
                                    "ScanningNodeId": tt[0].ScanningNodeId,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqiwmicr = JSON.stringify(micr);
                                    localacct.setItem(iwMICR, chqiwmicr);

                                }
                                // alert(tt[1].FrontGreyImagePath);
                                //----------------------------------------------------------------------//
                                if (document.getElementById('DEbySnpt').value == "1") {
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
                                //--------------Filling value from OCR1--------------
                                document.getElementById('ChqnoQC').value = tt[1].ChequeNoMICR;
                                document.getElementById('SortQC').value = tt[1].SortCodeMICR;
                                document.getElementById('SANQC').value = tt[1].SANMICR;
                                document.getElementById('TransQC').value = tt[1].TransCodeMICR;
                                //---------------Hilighting with red color---------------
                                if (tt[1].MICRRepairStatus.substring(0, 1) == "1") {
                                    document.getElementById('ChqnoQC').style.color = "red";
                                    chq = true;
                                }
                                if (tt[1].MICRRepairStatus.substring(1, 2) == "1") {
                                    document.getElementById('SortQC').style.color = "red";
                                    srtcd = true;
                                }
                                if (tt[1].MICRRepairStatus.substring(2, 3) == "1") {
                                    document.getElementById('SANQC').style.color = "red";
                                    sancd = true;
                                }
                                if (tt[1].MICRRepairStatus.substring(3, 4) == "1") {
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

    var value = 0;
    callrotate = function () {
        value += 180;
        $("#myimg").rotate({ animateTo: value })
    }

});

function fullImage() {
    //  alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);activefullimg = true;
    if (activefullimg == true) {
        document.getElementById('fulimg').src = document.getElementById('myfulimg').src;
    }
    else {
        document.getElementById('fulimg').src = document.getElementById('myimg').src;
    }

}

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
    else if (SANQC.length < 6  || isNaN(SANQC)) {
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
    return (match);
}
