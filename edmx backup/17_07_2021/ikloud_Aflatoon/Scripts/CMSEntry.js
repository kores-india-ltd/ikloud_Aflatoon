
var tt;
var lesscnt;
var dataSlp;
var validcall = true;
var currentval;
var splitval;
var add1mstrSrlno1, add1mstrSrlno2, add1mstrSrlno3, add1mstrSrlno4, add1mstrSrlno5;
var add1dataType1, add1dataType2, add1dataType3, add1dataType4, add1dataType5;
var add1Size1, add1Size2, add1Size3, add1Size4, add1Size5;
var addmandate1, addmandate2, addmandate3, addmandate4, addmandate5;
var addLegend1, addLegend2, addLegend3, addLegend4, addLegend5;
var prodDivisnReq, prodDivisnMandate, prodHireReq, prodHireMandate, instrDtlsReq, instrDtlsMandate, prodDrawReq, prodDrawMandate, prodAddnlReq, prodAddnlMandate,
    prodSubcustReq, prodSubcustMandate, prodSubcustEntryLvl, prodPickupReq;

function updateSlip(modelDataJSON) {
    debugger;
    dataSlp = JSON.stringify(modelDataJSON);
    // alert(dataSlp);
    tt = JSON.parse(dataSlp);

    d = tt;
    // alert(d);
    //--------------Add1----------
    splitval = d.split(",");
    // alert(splitval);
    addLegend1 = splitval[11].split(":")[1];
    addmandate1 = splitval[12].split(":")[1];
    add1Size1 = splitval[41].split(":")[1];
    add1dataType1 = splitval[42].split(":")[1];
    add1mstrSrlno1 = splitval[43].split(":")[1];

    //  alert(add1dataType1

    //-----------------Add2---------------------
    addLegend2 = splitval[15].split(":")[1];
    addmandate2 = splitval[16].split(":")[1];
    add1Size2 = splitval[44].split(":")[1];
    add1dataType2 = splitval[45].split(":")[1];
    add1mstrSrlno2 = splitval[46].split(":")[1];
    //alert(addmandate2);
    //-----------------Add3---------------------
    addLegend3 = splitval[19].split(":")[1];
    addmandate3 = splitval[20].split(":")[1];
    add1Size3 = splitval[47].split(":")[1];
    add1dataType3 = splitval[48].split(":")[1];
    add1mstrSrlno3 = splitval[49].split(":")[1];
    // alert(addmandate3);
    //-----------------Add4---------------------
    addLegend4 = splitval[23].split(":")[1];
    addmandate4 = splitval[24].split(":")[1];
    add1Size4 = splitval[50].split(":")[1];
    add1dataType4 = splitval[51].split(":")[1];
    add1mstrSrlno4 = splitval[52].split(":")[1];
    //alert(addmandate4);

    //-----------------Add5---------------------
    addLegend5 = splitval[27].split(":")[1];
    addmandate1 = splitval[28].split(":")[1];
    add1Size5 = splitval[53].split(":")[1];
    add1dataType5 = splitval[54].split(":")[1];
    add1mstrSrlno5 = splitval[55].split(":")[1];
    //---------------------Extra Fields-------------
    prodDivisnReq = splitval[61].split(":")[1];//---prev 58
    prodDivisnMandate = splitval[62].split(":")[1];
    prodHireReq = splitval[63].split(":")[1];
    prodHireMandate = splitval[64].split(":")[1];
    instrDtlsReq = splitval[65].split(":")[1];
    instrDtlsMandate = splitval[66].split(":")[1];
    prodDrawReq = splitval[67].split(":")[1];
    prodDrawMandate = splitval[68].split(":")[1];
    prodSubcustReq = splitval[71].split(":")[1];
    prodSubcustMandate = splitval[72].split(":")[1];
    prodSubcustEntryLvl = splitval[73].split(":")[1];
    prodPickupReq = splitval[74].split(":")[1];

    //alert(addmandate5);

    //alert(add1Size1 + ' dt: ' + add1dataType1 + ' addmstr: ' + add1mstrSrlno1 + '\n' + add1Size2 + ' dt: ' + add1dataType2 + ' addmstr: ' + add1mstrSrlno3 +
    //    '\n' + add1Size3 + ' dt: ' + add1dataType3 + ' addmstr: ' + add1mstrSrlno3 + '\n' + add1Size4 + ' dt: ' + add1dataType4 + ' addmstr: ' + add1mstrSrlno4 +
    //    '\n' + add1Size5 + ' dt: ' + add1dataType5 + ' addmstr: ' + add1mstrSrlno5);

    // alert(add1mstrSrlno5);

    if (tt.length > 0) {
        //--------------Setting textbox values----------------//

        // alert(addLegend1);
        if (prodHireReq == '"Y"') {
            $("#HiratchyCode").focus();
        }
        else if (prodDivisnReq == '"Y"') {
            $("#DivisionCode").focus();
        }
        else if (prodDrawReq == '"Y"') {
            $("#CustDrawerCode").focus();
        }
        else if (prodSubcustReq == '"Y"' && prodSubcustEntryLvl == '"D"') {
            $("#SubcustomerCode").focus();
        }
        else if (prodPickupReq == '"Y"') {
            $("#ProdLCCPickup").focus();
        }
        else if (addLegend1 != null) {
            $("#Additional1").focus();
        }
        else if (addLegend2 != null) {
            $("#Additional2").focus();
        }
        else if (addLegend3 != null) {
            $("#Additional3").focus();
        }
        else if (addLegend4 != null) {
            $("#Additiona4").focus();
        }
        else if (addLegend5 != null) {
            $("#Additional5").focus();
        }
        //..................End Settings texbox values----------------//

    }
}

$(document).ready(function () {
    //  debugger;
    $("#btnClientCd").click(function () {
        // alert($("#ClientCode").val());
        if ($("#ClientCode").val() == "") {
            alert('Please select ClientCode!!');
            $("#ClientCode").focus();
            return false;
        }

        $.ajax({
            url: RootUrl + 'CMSDataEntry/AdditionalDataEntry',
            data: { clientCode: $("#ClientCode").val() },
            type: 'POST',
            dataType: 'html',
            success: function (result) {
                if (result == "false") {
                    alert('No Data was found !!!');
                }
                else {
                    //$('#form').removeData('validator');
                    //$('#form').removeData('unobtrusiveValidation');
                    //$("#form").each(function () { $.data($(this)[0], 'validator', false); }); //enable to display the error messages
                    //$.validator.unobtrusive.parse("#form");
                    $('#clntSlip').html(result);
                }
            }

        });
    });
    //--,
    //-----------------------Close Button--------------
    $("#btnClose").click(function () {
        debugger;
        $("#clntSlip").empty();
        $("#getcheqs").empty();
        document.getElementById('btnslp').style.display = "none";
        document.getElementById('clntSlip').style.display = "none";
    });
    //-----------------------Referel---------------
    $("#btnRefrel").click(function () {
        debugger;
        var extrvalarry = [];
        extrvalarry.push($("#HiratchyCode").val());
        extrvalarry.push($("#DivisionCode").val());
        extrvalarry.push($("#CustDrawerCode").val());
        extrvalarry.push($("#SubcustomerCode").val());
        extrvalarry.push($("#ProdLCCPickup").val());
        extrvalarry.push($("#Additional1ID").val());
        extrvalarry.push($("#Additional2ID").val());
        extrvalarry.push($("#Additional3ID").val());
        extrvalarry.push($("#Additional4ID").val());
        extrvalarry.push($("#Additional5ID").val());
        extrvalarry.push($("#Additional1").val());
        extrvalarry.push($("#Additional2").val());
        extrvalarry.push($("#Additional3").val());
        extrvalarry.push($("#Additional4").val());
        extrvalarry.push($("#Additional5").val());
        extrvalarry.push($("#SlipRefNo").val());
        extrvalarry.push($("#ProdEffectiveDate").val());
        extrvalarry.push($("#InstrumentType").val());

        $.ajax({
            url: RootUrl + 'CMSDataEntry/UpdateSlipAdditional',
            data: JSON.stringify({ cmsAddInput: tt, extrafields: extrvalarry }),
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            success: function (data) {

                if (data == "false") {
                    // alert('jk');
                    //window.location = RootUrl + 'CMSDataEntry/AdditionalDataEntry?id=' + $("#ClientCode").val();
                    $.ajax({
                        url: RootUrl + 'CMSDataEntry/AdditionalDataEntry',
                        data: { clientCode: $("#ClientCode").val() },
                        type: 'POST',
                        // contentType: 'application/json; charset=utf-8',
                        dataType: 'html',
                        success: function (resultSlp) {
                            //alert(resultSlp); 
                            if (resultSlp == "false") {
                                alert('No Data was Found!!');
                                $("#clntSlip").empty();
                                $("#getcheqs").empty();
                                document.getElementById('btnslp').style.display = "none";
                            }
                            else {
                                //$('#form').removeData('validator');
                                //$('#form').removeData('unobtrusiveValidation');
                                //$("#form").each(function () { $.data($(this)[0], 'validator', false); }); //enable to display the error messages
                                //$.validator.unobtrusive.parse("#form");

                                $('#clntSlip').html(resultSlp);
                            }
                        }

                    });
                }
                else {
                    // alert('Data successfully updated!!');
                    $("#clntSlip").empty();
                    $("#getcheqs").empty();
                    $('#clntSlip').html(data);
                }
            }

        });
    });
    //------------------------------------New---------------------------

    $("body").on("keypress", "#SlipRefNo,#Additional1,#Additional2,#Additional3,#Additional4,#Additional5", function (event) {
        //alert('ok');
        var filedval = $(this).attr('id');
        if (filedval == "SlipRefNo") {
            if (event.shiftKey) {
                event.preventDefault();
            }
            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.charCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

            }
            else {
                event.preventDefault();
            }
        }
        else if (filedval == "Additional1") {
            if (event.shiftKey) {
                //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
                //}
            }
            if (add1mstrSrlno1.replace('"', "").replace('"', "") != "0") {

                if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                }
                else {
                    event.preventDefault();
                }
            }
            else {
                if (add1dataType1 == '"I"') {
                    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
                        // alert('if');
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else if (add1dataType1 == '"T"') {
                    if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else {
                    event.preventDefault();
                }
            }
        }
        else if (filedval == "Additional2") {
            if (event.shiftKey) {
                //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
                //}
            }
            if (add1mstrSrlno2.replace('"', "").replace('"', "") != "0") {

                if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                }
                else {
                    event.preventDefault();
                }
            }
            else {

                if (add1dataType2 == '"I"') {
                    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
                        // alert('if');
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else if (add1dataType2 == '"T"') {
                    if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else {
                    event.preventDefault();
                }
            }

        }
        else if (filedval == "Additional3") {
            if (event.shiftKey) {
                //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
                //}
            }
            if (add1mstrSrlno3.replace('"', "").replace('"', "") != "0") {

                if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                }
                else {
                    event.preventDefault();
                }
            }
            else {
                if (add1dataType3 == '"I"') {
                    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
                        // alert('if');
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else if (add1dataType3 == '"T"') {
                    if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else {
                    event.preventDefault();
                }
            }
        }
        else if (filedval == "Additional4") {
            if (event.shiftKey) {
                //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
                //}
            }
            if (add1mstrSrlno4.replace('"', "").replace('"', "") != "0") {

                if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                }
                else {
                    event.preventDefault();
                }
            }
            else {
                if (add1dataType4 == '"I"') {
                    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
                        // alert('if');
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else if (add1dataType4 == '"T"') {
                    if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else {
                    event.preventDefault();
                }
            }
        }
        else if (filedval == "Additional5") {
            if (event.shiftKey) {
                //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
                //}
            }
            if (add1mstrSrlno5.replace('"', "").replace('"', "") != "0") {

                if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                }
                else {
                    event.preventDefault();
                }
            }
            else {
                if (add1dataType5 == '"I"') {
                    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
                        // alert('if');
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else if (add1dataType5 == '"T"') {
                    if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
                    }
                    else {
                        event.preventDefault();
                    }
                }
                else {
                    event.preventDefault();
                }
            }
        }



    });

    //$(".numericOnly").bind('keypress', function (event) {
    //    alert('Hello');
    //    if (event.shiftKey) {
    //        event.preventDefault();
    //    }
    //    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.charCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

    //    }
    //    else {
    //        event.preventDefault();
    //    }
    //});
    ////------------------------------Additional1-----KeyPress----------------
    //$("#Additional1").keypress(function (event) {
    //    debugger;

    //    if (event.shiftKey) {
    //        //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
    //        event.preventDefault();
    //        //}
    //    }
    //    if (add1mstrSrlno1.replace('"', "").replace('"', "") != "0") {

    //        if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }
    //    else {
    //        if (add1dataType1 == '"I"') {
    //            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
    //                // alert('if');
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else if (add1dataType1 == '"T"') {
    //            if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }

    //});
    ////---------------
    ////------------------------------Additional2-----KeyPress----------------
    //$("#Additional2").keypress(function (event) {
    //    debugger;
    //    // alert(add1mstrSrlno2.replace('"', "").replace('"', ""));
    //    if (event.shiftKey) {
    //        //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
    //        event.preventDefault();
    //        //}
    //    }
    //    if (add1mstrSrlno2.replace('"', "").replace('"', "") != "0") {

    //        if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }
    //    else {

    //        if (add1dataType2 == '"I"') {
    //            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
    //                // alert('if');
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else if (add1dataType2 == '"T"') {
    //            if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }

    //});

    ////---------------
    ////------------------------------Additional3-----KeyPress----------------
    //$("#Additional3").keypress(function (event) {
    //    debugger;
    //    if (event.shiftKey) {
    //        //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
    //        event.preventDefault();
    //        //}
    //    }
    //    if (add1mstrSrlno3.replace('"', "").replace('"', "") != "0") {

    //        if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }
    //    else {
    //        if (add1dataType3 == '"I"') {
    //            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
    //                // alert('if');
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else if (add1dataType3 == '"T"') {
    //            if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }

    //});
    ////---------------
    ////------------------------------Additional4-----KeyPress----------------
    //$("#Additional4").keypress(function (event) {
    //    debugger;
    //    if (event.shiftKey) {
    //        //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
    //        event.preventDefault();
    //        //}
    //    }
    //    if (add1mstrSrlno4.replace('"', "").replace('"', "") != "0") {

    //        if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }
    //    else {
    //        if (add1dataType4 == '"I"') {
    //            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
    //                // alert('if');
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else if (add1dataType4 == '"T"') {
    //            if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }

    //});
    ////---------------
    ////------------------------------Additional5-----KeyPress----------------
    //$("#Additional5").keypress(function (event) {
    //    debugger;
    //    if (event.shiftKey) {
    //        //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
    //        event.preventDefault();
    //        //}
    //    }
    //    if (add1mstrSrlno5.replace('"', "").replace('"', "") != "0") {

    //        if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }
    //    else {
    //        if (add1dataType5 == '"I"') {
    //            if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
    //                // alert('if');
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else if (add1dataType5 == '"T"') {
    //            if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || event.keyCode == 46 || event.charCode == 32 || (event.charCode > 47 && event.charCode < 58) || (event.charCode > 64 && event.charCode < 91) || (event.charCode > 95 && event.charCode < 123) || event.charCode == 46 || (event.charCode >= 37 && event.charCode <= 40)) {
    //            }
    //            else {
    //                event.preventDefault();
    //            }
    //        }
    //        else {
    //            event.preventDefault();
    //        }
    //    }


    //});
    //---------------

    //$("#SlipRefNo").keypress(function (event) {
    //    debugger;
    //    if (event.shiftKey) {
    //        event.preventDefault();
    //    }
    //    if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.charCode == 40 || (event.charCode > 47 && event.charCode < 58)) {

    //    }
    //    else {
    //        event.preventDefault();
    //    }
    //});

    var value = 0;
    callrotate = function () {

        value += 180;
        $("#myimg").rotate({ animateTo: value })
    }

    //------------------------------

    $("#slpadd").click(function () {

        var clntflg = clientSideValid();
        if (clntflg == false) {
            return false;
        }

        var extrvalarry = [];
        extrvalarry.push($("#HiratchyCode").val());
        extrvalarry.push($("#DivisionCode").val());
        extrvalarry.push($("#CustDrawerCode").val());
        extrvalarry.push($("#SubcustomerCode").val());
        extrvalarry.push($("#ProdLCCPickup").val());
        extrvalarry.push($("#Additional1ID").val());
        extrvalarry.push($("#Additional2ID").val());
        extrvalarry.push($("#Additional3ID").val());
        extrvalarry.push($("#Additional4ID").val());
        extrvalarry.push($("#Additional5ID").val());
        extrvalarry.push($("#Additional1").val());
        extrvalarry.push($("#Additional2").val());
        extrvalarry.push($("#Additional3").val());
        extrvalarry.push($("#Additional4").val());
        extrvalarry.push($("#Additional5").val());
        extrvalarry.push($("#SlipRefNo").val());
        extrvalarry.push($("#ProdEffectiveDate").val());
        extrvalarry.push($("#InstrumentType").val());

        $.ajax({
            url: RootUrl + 'CMSDataEntry/UpdateSlipAdditional',
            data: JSON.stringify({ cmsAddInput: tt, extrafields: extrvalarry }),
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            success: function (data) {

                if (data == "false") {
                    // alert('jk');
                    //window.location = RootUrl + 'CMSDataEntry/AdditionalDataEntry?id=' + $("#ClientCode").val();
                    $.ajax({
                        url: RootUrl + 'CMSDataEntry/AdditionalDataEntry',
                        data: { clientCode: $("#ClientCode").val() },
                        type: 'POST',
                        // contentType: 'application/json; charset=utf-8',
                        dataType: 'html',
                        success: function (resultSlp) {
                            //alert(resultSlp); 
                            if (resultSlp == "false") {
                                alert('No Data was Found!!');
                                $("#clntSlip").empty();
                                $("#getcheqs").empty();
                                document.getElementById('btnslp').style.display = "none";
                            }
                            else {
                                //$('#form').removeData('validator');
                                //$('#form').removeData('unobtrusiveValidation');
                                //$("#form").each(function () { $.data($(this)[0], 'validator', false); }); //enable to display the error messages
                                //$.validator.unobtrusive.parse("#form");

                                $('#clntSlip').html(resultSlp);
                            }
                        }

                    });
                }
                else {
                    // alert('Data successfully updated!!');
                    $("#clntSlip").empty();
                    $("#getcheqs").empty();
                    $('#clntSlip').html(data);
                }
            }

        });

    });
});
//----------------------- validating textbox value from database----


function AllFieldsValiation(val) {
    //alert(val);

    var FieldValue
    var addval = "0";
    if ($("#" + val).val() == null || $("#" + val).val() == "") {
        alert('Field should not be empty!!');
        $("#" + val).focus();
        //alert($("#" + val).val());
        validcall = false;
        return false;
    }
    if (val == "HiratchyCode") {
        var FieldValue = $("#HiratchyCode").val();
        validcall = true;
    }
    else if (val == "DivisionCode") {
        var FieldValue = $("#DivisionCode").val();
        validcall = true;
    }
    else if (val == "CustDrawerCode") {
        var FieldValue = $("#CustDrawerCode").val();
        validcall = false;

    }
    else if (val == "SubcustomerCode") {
        var FieldValue = $("#SubcustomerCode").val();
        validcall = true;

    }
    else if (val == "ProdLCCPickup") {
        var FieldValue = $("#ProdLCCPickup").val();
        validcall = true;

    }
    else if (val == "Additional1") {
        var FieldValue = $("#Additional1").val();
        if (add1mstrSrlno1 == '"0"') {
            // alert('Yes!!');
            validcall = false;
        }
        else {
            addval = add1mstrSrlno1;
            //  alert(addval);
            validcall = true;
        }

    }
    else if (val == "Additional2") {
        var FieldValue = $("#Additional2").val();
        if (add1mstrSrlno2 == '"0"') {
            // alert('2Yes!!');
            validcall = false;
        }
        else {
            validcall = true;
            addval = add1mstrSrlno2;
        }

    }
    else if (val == "Additional3") {
        var FieldValue = $("#Additional3").val();
        if (add1mstrSrlno3 == '"0"') {
            // alert('3Yes!!');
            validcall = false;
        }
        else {
            validcall = true;
            addval = add1mstrSrlno3;
        }
    }
    else if (val == "Additional4") {
        var FieldValue = $("#Additional4").val();
        if (add1mstrSrlno4 == '"0"') {
            // alert('4Yes!!');
            validcall = false;
        }
        else {
            validcall = true;
            addval = add1mstrSrlno4;
        }
    }
    else if (val == "Additional5") {
        var FieldValue = $("#Additional5").val();
        if (add1mstrSrlno5 == '"0"') {
            // alert('5Yes!!');
            validcall = false;
        }
        else {
            validcall = true;
            addval = add1mstrSrlno5;
        }
    }

    if (validcall == true) {

        $.ajax({
            url: RootUrl + 'CMSDataEntry/ValidateMaster',
            data: { instrumentType: $("#instrumentType").val(), Fieldname: val, FieldValue: FieldValue, additionalval: addval, CustCode: $("#ClientCode").val(), BranchCode: $("#BranchCode").val() },
            type: 'Get',
            dataType: 'html',
            success: function (result) {
                if (result == "false") {
                    alert('Entered value was not matched with master value!!');
                    $("#" + val).focus();
                    $("#" + val).val('');
                    return false;
                }
            }

        });

    }
}
//-------------------------------------ClientSide Validation------------------------------------------
function clientSideValid() {
    if (document.getElementById('InstrumentType').value == "S") {
        //alert(prodDivisnMandate);
        //alert($("#DivisionCode").val());
        debugger;
        if (prodDivisnMandate == '"Y"') {
            if ($("#DivisionCode").val() == "") {
                alert('Please enter division code!!');
                $('#DivisionCode').val('');
                $("#DivisionCode").focus();
                return false;
            }
            else {
                var rdtn = AllFieldsValiation('DivisionCode');
                if (rdtn == false) {
                    $("#DivisionCode").focus();
                    return false;
                }
            }
        }

        if (prodHireMandate == '"Y"') {
            if ($("#HiratchyCode").val() == "") {
                alert('Please enter hiratchyCode code!!');
                $('#HiratchyCode').val('');
                $("#HiratchyCode").focus();
                return false;
            }
            else {
                var rhtn = AllFieldsValiation('HiratchyCode');
                if (rhtn == false) {
                    $("#HiratchyCode").focus();
                    return false
                }

            }
        }

        if (prodDrawMandate == '"Y"') {
            if ($("#CustDrawerCode").val() == "") {
                alert('Please enter drawer name!!');
                $('#CustDrawerCode').val('');
                $("#CustDrawerCode").focus();
                return false;
            }
            //else {
            //    var rtnc = AllFieldsValiation('CustDrawerCode');
            //    if (rtnc == false) {
            //        return false;
            //    }

            //}
        }

        if (prodSubcustMandate == '"Y"') {
            if ($("#SubcustomerCode").val() == "") {
                alert('Please enter Subcustomer code!!');
                $('#SubcustomerCode').val('');
                $("#SubcustomerCode").focus();
                return false;
            }
            else {
                var rtnsub = AllFieldsValiation('SubcustomerCode');
                if (rtnsub == false) {
                    return false;
                }

            }
        }

        if (prodPickupReq == '"Y"') {
            if ($("#ProdLCCPickup").val() == "") {
                alert('Please enter LCC pickup code!!');
                $('#ProdLCCPickup').val('');
                $("#ProdLCCPickup").focus();
                return false;
            }
            else {
                var rtnpick = AllFieldsValiation('ProdLCCPickup')
                if (rtnpick == false) {
                    return false;
                }

            }
        }

        if ($("#SlipRefNo").val() == "") {
            alert('Please enter Slip refrence no!!');
            $('#SlipRefNo').val('');
            $("#SlipRefNo").focus();
            return false;
        }

        if (add1mstrSrlno1 != "null" && add1mstrSrlno1 != "") {
            if ($("#Additional1").val() == "" && addmandate1 == "Y") {
                alert('Please enter ' + addLegend1 + ' !!');
                $('#Additional1').val('');
                $("#Additional1").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional1'));
            }

        }
        if (add1mstrSrlno2 != "null" && add1mstrSrlno2 != "") {
            if ($("#Additional2").val() == "" && addmandate2 == "Y") {
                alert('Please enter ' + addLegend2 + ' !!');
                $('#Additional2').val('');
                $("#Additional2").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional2'));
            }
        }
        if (add1mstrSrlno3 != "null" && add1mstrSrlno3 != "") {
            if ($("#Additional3").val() == "" && addmandate3 == "Y") {
                alert('Please enter ' + addLegend3 + ' !!');
                $('#Additional3').val('');
                $("#Additional3").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional3'));
            }
        }
        if (add1mstrSrlno4 != "null" && add1mstrSrlno4 != "") {
            if ($("#Additional4").val() == "" && addmandate4 == "Y") {
                alert('Please enter ' + addLegend4 + ' !!');
                $('#Additional4').val('');
                $("#Additional4").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional4'));
            }
        }
        if (add1mstrSrlno5 != "null" && add1mstrSrlno5 != "") {
            if ($("#Additional5").val() == "" && addmandate5 == "Y") {
                alert('Please enter ' + addLegend5 + ' !!');
                $("#Additional5").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional5'));
            }

        }
    }
    else if (document.getElementById('InstrumentType').value == "C") {

        if (add1mstrSrlno1 != "null" && add1mstrSrlno1 != "") {
            if ($("#Additional1").val() == "" && addmandate1 == "Y") {
                alert('Please enter ' + addLegend1 + ' !!');
                $('#Additional1').val('');
                $("#Additional1").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional1'));
            }

        }
        if (add1mstrSrlno2 != "null" && add1mstrSrlno2 != "") {
            if ($("#Additional2").val() == "" && addmandate2 == "Y") {
                alert('Please enter ' + addLegend2 + ' !!');
                $('#Additional2').val('');
                $("#Additional2").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional2'));
            }
        }
        if (add1mstrSrlno3 != "null" && add1mstrSrlno3 != "") {
            if ($("#Additional3").val() == "" && addmandate3 == "Y") {
                alert('Please enter ' + addLegend3 + ' !!');
                $('#Additional3').val('');
                $("#Additional3").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional3'));
            }
        }
        if (add1mstrSrlno4 != "null" && add1mstrSrlno4 != "") {
            if ($("#Additional4").val() == "" && addmandate4 == "Y") {
                alert('Please enter ' + addLegend4 + ' !!');
                $('#Additional4').val('');
                $("#Additional4").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional4'));
            }
        }
        if (add1mstrSrlno5 != "null" && add1mstrSrlno5 != "") {
            if ($("#Additional5").val() == "" && addmandate5 == "Y") {
                alert('Please enter ' + addLegend5 + ' !!');
                $('#Additional5').val('');
                $("#Additional5").focus();
                return false;
            }
            else {
                return (AllFieldsValiation('Additional5'));
            }

        }
    }
}
//---------------

//});

function fullImage() {
    //  alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}

function ChangeImage(imagetype) {
    //  alert(document.getElementById('backimpath').value);
    // var indexcnt = document.getElementById('cnt').value;
    //if (imagetype == "FTiff") {

    //    document.getElementById('myimg').src = tt[indexcnt].FrontTiffImagePath;
    //}
    if (imagetype == "BTiff") {
        //alert('Browser not supporting!!!'); 

        document.getElementById('myimg').src = document.getElementById('backimpath').value;
    }
    else if (imagetype == "FGray") {


        document.getElementById('myimg').src = document.getElementById('impath').value;
    }

}



