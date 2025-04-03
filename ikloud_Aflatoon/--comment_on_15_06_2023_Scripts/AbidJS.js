// Jquery ------------ ---------------------
//-----------use In User Creation ---and modification
$(document).ready(function () {

    $(document).bind('keypress', function (e) {
        //alert('hoyee');
        //alert(e.keyCode);
        if (e.keyCode == 113) {
            //alert('Aila');
            document.getElementById('divImgSlpAc1').style.display = "none";
            document.getElementById("Chqflm").src = document.getElementById("FrontGrayImgPath").value;
            document.getElementById('divflm').style.display = "";
        } //
        //alert(document.getElementById('Clrtyp').value);
        //        if (document.getElementById('chngpass')!="") {

        //        }
        if (document.getElementById('Clrtyp').value == "IW") {
            if (e.keyCode == 114) {//-------------F3------------
                document.getElementById('payeename').value = "VODAFONE";
            }
            if (e.keyCode == 115) {//-------------F4------------
                document.getElementById('payeename').value = "AIRTEL";
            }
            if (e.keyCode == 117) {//-------------F6------------
                document.getElementById('payeename').value = "HDFC BANK";
            }
            if (e.keyCode == 118) {//-------------F7------------
                document.getElementById('payeename').value = "TORRENT POWER";
            }
            if (e.keyCode == 119) {//-------------F8------------
                document.getElementById('payeename').value = "LIC OF INDIA";
            }
            if (e.keyCode == 120) {//-------------F9------------
                document.getElementById('payeename').value = "MSEDCL";
            }
            if (e.keyCode == 121) {//-------------F10------------
                document.getElementById('payeename').value = "RCIL AC";
            }
            if (e.keyCode == 122) {//-------------F11------------
                document.getElementById('payeename').value = "TATA POWER";
            }
            if (e.keyCode == 123) {//-------------F12------------
                document.getElementById('payeename').value = "CITI BANK CC AC";
            }
        }

    });
    //--------------------------
    //    $(document).bind('keypress', function (e) {
    //        alert(e.keyCode)
    //        if (e.keyCode == 113) { // return
    //            //$('#UVRadio').trigger('click');
    //            //alert('okk');
    //            alert('Aila');
    //            document.getElementById('divImgSlpAc1').style.display = "none";
    //            document.getElementById("Chqflm").src = document.getElementById("FrontGrayImgPath").value;
    //            document.getElementById('divflm').style.display = "";
    //        }
    //    });
    //--------------------------

    //$('#ex1').zoom({ url: document.getElementById("Chqimg").value });
    // alert('hello');
    //$('#Chqimg')       
    //.wrap('<span style="display:inline-block"></span>')
    //.css('display', 'block')
    //.parent()
    //.zoom({ url: document.getElementById("Chqimg").value });

    //$("input[name=Rdo]").change(function () {
    //    //alert("caleed");
    //    if ($("#Radio1").attr("checked")) {
    //        //alert("caleed");
    //        $('#Chqimg')
    //                   .wrap('<span style="display:inline-block"></span>')
    //                   .css('display', 'block')
    //                   .parent()
    //                   .zoom({ url: document.getElementById("Chqimg").value });
    //    }
    //    else {
    //        //alert("caleed");
    //        $('#Chqimgbk')
    //                  .wrap('<span style="display:inline-block"></span>')
    //                  .css('display', 'block')
    //                  .parent()
    //                  .zoom({ url: document.getElementById("Chqimgbk").value });
    //    }

    //});

    //$('#Rdo').click(function (e) {
    //    alert("caleed");
    //    $('#Chqimg')
    //.wrap('<span style="display:inline-block"></span>')
    //.css('display', 'block')
    //.parent()
    //.zoom({ url: document.getElementById("Chqimg").value });
    //});
    //-------------------------- Create User -------------------------
    //    $("#policynm").bind("change", function () {
    //        // alert('ok');
    //        var e = document.getElementById('policynm')

    //        var strUser = e.options[e.selectedIndex].text;

    //        if (strUser == "Select") {
    //            alert('Please select policy type !');
    //            document.getElementById('policynm').focus();
    //            return false;
    //        }
    //        $.ajax({
    //            url: '/CreateUser/PolicyDetails',
    //            dataType: 'html',
    //            data: { name: $("#policynm").val() },
    //            success: function (data) {
    //                //alert(data);
    //                $('#changdetls').html(data);

    //            }
    //        });
    //    });
    //--------------------------------------------------------------------
    //---------------- Data Entry -----------------------------------

    $("form input").keydown(function (e) {
        //alert('Aila');
        var next_idx = $('input[type=text]').index(this) + 1;
        var tot_idx = $('body').find('input[type=text]').length;
        if (event.keyCode == 13) {
            if (tot_idx == next_idx)
            //go to the first text element if focused in the last text input element12.	
                $("input[value='Accept']").focus().click();
            else
            //go to the next text input element.
                $('input[type=text]:eq(' + next_idx + ')').focus().select();
            /// $("input[value='Accept']").focus().click();
            //                $('button[type=submit] .NavButton').click();
            // return true;
        }
    });

    //--------------------- End -----------------------------------------
    $("input").attr('autocomplete', 'off');

    $("#vfnlvl").change(function () {
        var brname = $('select[name="vfnlvl"] option:selected').index();

        $("select#Venlvel").prop('selectedIndex', brname);
        //alert(document.getElementById('Venlvel').value);

    });
    //----------- use in Data entry module--- AND IW Data Entry--------
    $("#chqAmount,#SlipAmount,#OwPayeeName").keydown(function (event) {

        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }

        if (event.keyCode == 110 || event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.keyCode > 95 && event.keyCode < 106) || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
            //alert('ok');
        }
        else {
            //if (event.keyCode < 95) {
            if (event.keyCode == 32 || event.keyCode < 48 || (event.keyCode > 57)) {
                event.preventDefault();
            }
        }
    });



    //--------------
    $("#chqcrdAcnt,#chqDate,#accountno,#SlipAcct,#PhoneNo").keydown(function (event) {

        //alert(event.keyCode);
        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }
        //        if (event.keyCode == 113) {
        //            Fullimg(); //Sign
        //            return false;
        //        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.keyCode > 64 && event.keyCode < 91) || (event.keyCode > 95 && event.keyCode < 106) || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            //if (event.keyCode < 95) {
            if (event.keyCode == 32 || event.keyCode < 48 || (event.keyCode > 57)) {
                event.preventDefault();
            }
        }
    });

    //--------------------- Checking Decimal value--------- In DE---Amount----------

    $('input#chqAmount,#SlipAmount').change(function () {
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
    //--------Using jquery--- domain selection-----

    $('#create_user,#modf_user').click(function () {
        var final = 'empty';
        $('.ads_Checkbox:checked').each(function () {
            var values = $(this).val();
            final += ',' + values;
        });
        document.getElementById("hddomain").value = final;
        //alert(document.getElementById("hddomain").value);
    });
    //------------ IW Data Entry --------------

    $("#payeename").keydown(function (event) {
        // alert(event.keyCode);
        if (event.shiftKey) {
            if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
                event.preventDefault();
            }
        }
        var vrpay = document.getElementById("payeename").value;
        // alert(vrpay.length);
        if (vrpay.length == 0) {
            if (event.keyCode == 32) {
                alert('Blank space are not allowed!');
                event.preventDefault();
                return false;
            }
        }

        if (event.keyCode == 190 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 39 || event.keyCode == 40 || (event.keyCode > 64 && event.keyCode < 91) || (event.keyCode > 95 && event.keyCode < 123) || event.keyCode == 46 || (event.keyCode >= 37 && event.keyCode <= 40)) {
        }
        else {
            //if (event.keyCode < 95) {
            if ((event.keyCode > 31 && event.keyCode > 65) || (event.keyCode > 90 && event.keyCode > 97)) {
                event.preventDefault();
            }
        }
    });
});
//----------------- IW-LOCK Record-----------------------------------------------
//-----------------------------------------------------LockUnlock
$("#RdoVerf").click(function () {
    $("#SelectBox").attr("disabled", "disabled");
});
$("#RdoReVerf").click(function () {
    $("#SelectBox").attr("disabled", "disabled");
});
$("#RdoDE").click(function () {
    $("#SelectBox").removeAttr("disabled");
});
$("#RdoQC").click(function () {
    $("#SelectBox").attr("disabled", "disabled");
});
$("#BtnLocked").click(function () {
    if ($("#RdoDE").is(':checked')) {
        if ($("#SelectBox").val() == '(Select)') {
            alert('Please select Type!');
            $("#SelectBox").focus();
            return false;
        }
    }
});
$("#BtnUnlock").click(function () {
    if ($("#chkCount").val() == 0) {
        alert('No Record Found!');
        return false;
    }
    else {
        //            return confirm('Are you sure to Unlock these records?');
        var msg = confirm('Are you sure to Unlock these records?')

        if (msg == true) {

            return true;
        }
        else {

            return false;
        }
    }

});
//----------------testing---------
function Boss() {
    alert('Hata Yaar');
}
//--------------- Call action From ajax ----------------------//
function navigate(target) {
    //Perform your AJAX call to your Controller Action
    //    $.post(target, { UserName: $('#UserName').val(), Password: $('#Password').val() });
    $.post(target, { UserName: $('#UserName').val(), Password: $('#Password').val() });
}

//-------------------------------------------------------------------------------
function Fullimg() {
    //alert(document.getElementById("FrontGrayImgPath").value);
    document.getElementById("Chqflm").src = document.getElementById("FrontGrayImgPath").value;
    document.getElementById('divImgSlpAc1').style.display = "none";
    document.getElementById('divflm').style.display = "";

}
//------------------ JavaScript------use in --User creattion and modification----module-------------
function getdetails(radio_val) {
    var radio = document.getElementById(radio_val).value;

    if (radio == "Cheque") {
        //alert(radio);
        if (document.getElementById("tslip").style.visibility == "visible") {
            //alert(radio);
            document.getElementById("tslip").style.visibility = "hidden";
            document.getElementById('chtbl').style.display = "block";
        }
        else {

            document.getElementById('chtbl').style.display = "block";
            document.getElementById('tslip').style.display = "none";
            document.getElementById('SAc').checked = false;
            document.getElementById('SAmt').checked = false;
        }
    }
    else
        if (document.getElementById("chtbl").style.visibility == "visible") {
            document.getElementById("chtbl").style.visibility = "hidden";
            document.getElementById('tslip').style.display = "block";
        }
        else {

            document.getElementById('chtbl').style.display = "none";
            document.getElementById('tslip').style.display = "block";
            document.getElementById('Date').checked = false;
            document.getElementById('CAc').checked = false;
            document.getElementById('DAc').checked = false;
            document.getElementById('pname').checked = false;
            document.getElementById('doname').checked = false;
            document.getElementById('Amt').checked = false;
        }
}
//------------------------
function getBranches(radio_val) {
    var radio = document.getElementById(radio_val).value;

    if (radio == "All") {
        document.getElementById('chkdomain').style.display = 'none';
    }
    else
        document.getElementById('chkdomain').style.display = 'block';
    document.getElementById('chkdomain').style.width = '20%';
    document.getElementById('chkdomain').style.height = '10%';
}
function show() {

    if (document.getElementById("vf").checked == true) {
        document.getElementById('br').style.display = 'block';
    }
    else {
        document.getElementById('br').style.display = 'none';
    }
}

function openadmin() {

    if (document.getElementById("chkadmin").checked == true) {
        document.getElementById('rowadmin').style.display = 'block';
    }
    else {
        document.getElementById('rowadmin').style.display = 'none';
    }
}
function hello() {

    var temp = document.getElementById("domain");
    var brid = document.getElementById("brID");
    var ht = brid.options[temp.selectedIndex].text
    document.getElementById('brid').value = ht;
}

////------ Domain selection in DE-------------------
function appenddata(check) {
    var data = document.getElementById("hddomain").value;
    if (data == "")
        data = "empty";
    if (document.getElementById(check).checked == true) {
        //alert(data);
        document.getElementById("hddomain").value = data + ',' + document.getElementById(check).value;
    }
    //          alert(document.getElementById("hddomain").value);

}

//--------- Use In Data Entery module ----------
function BackFrontImg(fulimg) {
    // alert(fulimg);

    if (fulimg == "fullimg") {

        document.getElementById("divsnpt").style.display = "none";
        if (document.getElementById("divsnpt").style.visibility == "hiddenhidden") {
            document.getElementById("divsnpt").style.visibility = "hidden";
        }
        //    document.getElementById('divsnpt').style.display = "none";
        document.getElementById('chqimg1').src = document.getElementById('FrontTiffImagePath').value;
        document.getElementById('chqimg1').style.width = "900";
        document.getElementById('chqimg1').style.height = "400";
        document.getElementById('divsnptBack').style.display = "";
        document.getElementById('chqimg1').style.display = "";
        //document.getElementById('bacImg').value = "Back Image";
    }
    else {
        if (document.getElementById('bacImg').value == "Back Image") {

            document.getElementById('chqimg').src = document.getElementById('BackTiffImagePath').value;
            document.getElementById('chqimg').style.width = "900";
            document.getElementById('chqimg').style.height = "400";
            document.getElementById('bacImg').value = "Front Image";
        }
        else {
            document.getElementById('chqimg').src = document.getElementById('FrontTiffImagePath').value;
            document.getElementById('chqimg').style.width = "900";
            document.getElementById('chqimg').style.height = "400";
            document.getElementById('bacImg').value = "Back Image";
        }
    }



}
function front(btnid) {
    //-------- Text Validation --------------
    document.getElementById('tempbtnm').value = document.getElementById('Prev').value;
    if (btnid != "Acpt" && btnid != "Rjct" && btnid != "cls") {

        if (document.getElementById('Prev').value == "Previous") {
            document.getElementById('PrevNxtVal').value = document.getElementById('Prev').value;
            //document.getElementById('Prev').value = "Next";
            //  alert(document.getElementById('Prev').value);
            return true;
        }
        else if (document.getElementById('Prev').value == "Next") {
            document.getElementById('PrevNxtVal').value = document.getElementById('Prev').value;
            // document.getElementById('Prev').value = "Previous";
            //                alert(document.getElementById('Prev').value);
            return true;
        }
    }
    else {

        if (btnid == "Acpt") {
            var finldat, amt, amt1, dat, acn, payemn;
            //---------------Cheque  ---- Validation  --------------------gPayName
            //------------PayeeName---------------
            if (document.getElementById('gPayName').value != "") {
                payemn = document.getElementById('OwPayeeName').value;

                if (payemn.length < 4 && payemn != "") {
                    alert('Slip no should be 4 digit !');
                    document.getElementById('OwPayeeName').focus();
                    document.getElementById('OwPayeeName').select();
                    return false;
                }
            }
            //------------------------------------
            if (document.getElementById('gAmt').value != "") {
                amt = document.getElementById('chqAmount').value;
                //alert(amt);   
                var intcont = 0;
                for (var i = 0; i < amt.length; i++) {

                    if (amt.charAt(i) == ".") {
                        intcont++;
                    }
                    if (intcont > 1) {
                        alert('Enter valid amount!');
                        document.getElementById('chqAmount').select();
                        return false;
                    }
                }

                if (amt == "NaN") {
                    alert('Enter valid amount!');
                    document.getElementById('chqAmount').select();
                    return false;
                }

                amt1 = amt;
                amt = amt.replace(/^0+/, '')
                amt = amt.length;
                if (amt1 == ".") {
                    alert('Amount field should not be dot(.) !');
                    document.getElementById('chqAmount').focus();
                    document.getElementById('chqAmount').select();
                    return false;
                }
                else if (amt1 == "0.00") {
                    alert('Amount field should not be zero(0) !');
                    document.getElementById('chqAmount').focus();
                    document.getElementById('chqAmount').select();
                    return false;
                }
                else if (amt < 1) {
                    alert('Amount field should not be empty !');
                    document.getElementById('chqAmount').focus();
                    document.getElementById('chqAmount').select();
                    return false;
                }
                else if (amt > 15) {
                    alert('Amount not valid !');
                    document.getElementById('chqAmount').focus();
                    document.getElementById('chqAmount').select();
                    return false;
                }

            }
            if (document.getElementById('gDate').value != "") {
                dat = document.getElementById('chqDate').value;
                if (dat == "") {
                    //alert('aila');
                    alert("Date field should not be empty !");
                    document.getElementById('chqDate').focus();
                    document.getElementById('chqDate').select();
                    return false;
                }
                else if (dat == "000000") {
                    alert("Date not valid !");
                    document.getElementById('chqDate').focus();
                    document.getElementById('chqDate').select();
                    return false;
                }
                else if (dat.length < 6) {
                    alert("Date not valid !");
                    document.getElementById('chqDate').focus();
                    document.getElementById('chqDate').select();
                    return false;
                }
                else {
                    var dd, mm, yy;
                    finldat = new String(dat);
                    // alert(finldat);

                    dd = finldat.substring(0, 2)
                    mm = finldat.substring(2, 4)
                    yy = finldat.substring(4, 6)
                    //alert(dd+'-'+ mm +'-'+'20'+yy);
                    if (dd > 31) {
                        alert('Please enter correct date!');
                        document.getElementById('chqDate').focus();
                        document.getElementById('chqDate').select();
                        return false;
                    }
                    if (mm > 12) {
                        alert('Please enter correct date!');
                        document.getElementById('chqDate').focus();
                        document.getElementById('chqDate').select();
                        return false;
                    }
                }
            }

            if (document.getElementById('MTNL').value == "False") {
                //alert('A/C');
                if (document.getElementById('gCrAC').value != "") {
                    acn = document.getElementById('chqcrdAcnt').value;

                    acn = acn.length
                    if (acn == "") {
                        alert("Account no field should not be empty !");
                        document.getElementById('chqcrdAcnt').focus();
                        return false;
                    }
                    if (acn < document.getElementById('acFromlngt').value) {
                        alert("Minimum account no sould be " + document.getElementById('acFromlngt').value + "digits");
                        document.getElementById('chqcrdAcnt').focus();
                        return false;
                    }
                    acn = document.getElementById('chqcrdAcnt').value.replace(/^0+/, '')
                    if (acn == "") {
                        alert("Invalid Account Number!");
                        document.getElementById('chqcrdAcnt').focus();
                        return false;
                    }
                }
            }
            else if (document.getElementById('MTNL').value == "True") {
                //alert('Phone!');
                if (document.getElementById('gCrAC').value != "") {
                    acn = document.getElementById('chqcrdAcnt').value;
                    // acn = acn.replace(/^0+/, '')
                    acn = acn.length

                    if (acn == "") {
                        alert("Phone no field should not be empty !");
                        document.getElementById('chqcrdAcnt').focus();
                        return false;
                    }
                    if (acn < 8) {
                        alert("Minimum phone no sould be 8 digits");
                        document.getElementById('chqcrdAcnt').focus();
                        return false;
                    }
                    acn = document.getElementById('chqcrdAcnt').value.replace(/^0+/, '')
                    if (acn == "") {
                        alert("Invalid Account Number!");
                        document.getElementById('chqcrdAcnt').focus();
                        return false;
                    }
                }

            }
            //--------------- Slip ---- A/C --------------------
            //--------------------------

            if (document.getElementById('gslipAcno').value != "") {
                acn = document.getElementById('SlipAcct').value;
                //                acn = acn.replace(/^0+/, '')
                acn = acn.length
                // alert(acn);
                if (acn == "") {
                    alert("Account no field should not be empty !");
                    document.getElementById('SlipAcct').focus();
                    return false;
                }
                if (acn < document.getElementById('acFromlngt').value) {
                    alert("Minimum account no sould be " + document.getElementById('acFromlngt').value + "digits");
                    document.getElementById('SlipAcct').focus();
                    return false;
                }
                acn = document.getElementById('SlipAcct').value.replace(/^0+/, '')
                // alert(acn);
                if (acn == "") {
                    alert("Invalid Account Number!");
                    document.getElementById('SlipAcct').focus();
                    return false;
                }
            }

            //-------------- Slip Amount --------------------------------

            if (document.getElementById('gslipamt').value != "") {
                // alert('Slip');
                amt = document.getElementById('SlipAmount').value;
                //alert(amt);
                var intcont = 0;
                for (var i = 0; i < amt.length; i++) {

                    if (amt.charAt(i) == ".") {
                        intcont++;
                    }
                    if (intcont > 1) {
                        alert('Enter valid amount!');
                        document.getElementById('SlipAmount').select();
                        return false;
                    }
                }

                if (amt == "NaN") {
                    alert('Enter valid amount!');
                    document.getElementById('SlipAmount').select();
                    return false;
                }

                amt1 = amt;
                amt = amt.replace(/^0+/, '')
                amt = amt.length;
                if (amt1 == ".") {
                    alert('Amount field should not be dot(.) !');
                    document.getElementById('SlipAmount').focus();
                    document.getElementById('SlipAmount').select();
                    return false;
                }
                else if (amt1 == "0.00") {
                    alert('Amount field should not be zero(0) !');
                    document.getElementById('SlipAmount').focus();
                    document.getElementById('SlipAmount').select();
                    return false;
                }
                else if (amt < 1) {
                    alert('Amount field should not be empty !');
                    document.getElementById('SlipAmount').focus();
                    document.getElementById('SlipAmount').select();
                    return false;
                }
                else if (amt > 15) {
                    alert('Amount not valid !');
                    document.getElementById('SlipAmount').focus();
                    document.getElementById('SlipAmount').select();
                    return false;
                }

            }

            //document.getElementById('PrevNxtVal').value = document.getElementById(btnid).value;
        }
        return true;
    }
}

//--------------- Validate()

function valid() {
    if (document.getElementById("title").value == "select" || document.getElementById("title").value == "") {
        alert('Please select title');
        //        document.getElementsByName("title").item(0).focus();
        document.getElementById("title").focus();
        return false;
    }
    
    var e = document.getElementById('policynm')
    var strUser = e.options[e.selectedIndex].text;
    if (strUser == "Select") 
    {
        alert('Please select policy type !');
        document.getElementById('policynm').focus();
        document.getElementById('policynm').select();

        return false;
    }
    if (document.getElementById("vf").checked == true) {
        if (document.getElementById('vfnlvl').value=="Select") {
            alert('Please select verification level !');
            return false;
        }
    }
    return true;
}

function checkselect(type) {
    // alert('OK');
    if (type == "zero") {
        var z = document.getElementById("zeropr");
        if (z.selectedIndex == 0) {
            alert('Please select process id!');
            document.getElementById("zeropr").focus();
            return false;
        }
        else
            return true;
    }
    else if (type == "IWVf") {
        var ff = document.getElementById("degrp");
        if (ff.selectedIndex == 0) {
            alert('Please select clearing type!');
            document.getElementById("degrp").focus();
            return false;
        }
        else
            return true;
    }
    else {
        var dd = document.getElementById("degrp");
        if (dd.selectedIndex == 0) {
            alert('Please select data entry group!');
            document.getElementById("degrp").focus();
            return false;
        }
        else
            return true;
    }

}
//--------------- Use In Inward ----------------------
function IWfront(btnid) {

    document.getElementById('tempbtnm').value = document.getElementById('Prev').value;
    // alert(document.getElementById('tempbtnm').value);
    if (btnid == "Acpt") {


        if (document.getElementById('gDebAC').value != "") {
            var Acct = document.getElementById('accountno').value;
            //Acct = Acct.replace(/^0+/, '')
            Acct = Acct.length


            if (Acct == "") {
                alert("Account no field should not be empty !");
                document.getElementById('accountno').focus();
                document.getElementById('accountno').select();
                return false;
            }
            if (Acct < document.getElementById('acFromlngt').value) {
                alert("Minimum account no sould be " + document.getElementById('acFromlngt').value + "digits");
                document.getElementById('accountno').focus();
                // document.getElementById('accountno').select();
                return false;
            }
            Acct = document.getElementById('accountno').value.replace(/^0+/, '')
            if (Acct == "") {
                alert("Invalid Account Number!");
                document.getElementById('chqcrdAcnt').focus();
                document.getElementById('accountno').select();
                return false;
            }

        }

        if (document.getElementById('gPayName').value != "") {
            var PAyee = document.getElementById('payeename').value;
            if (PAyee == "") {
                //alert('aila');
                alert("Payee field should not be empty !");
                document.getElementById('payeename').focus();
                document.getElementById('payeename').select();
                return false;
            }
            if (PAyee.length < 5 && PAyee != "") {
                //alert('aila');
                alert("Enter minimum 5 character for payee name !");
                document.getElementById('payeename').focus();
                document.getElementById('payeename').select();
                return false;
            }
        }
        if (document.getElementById('gDate').value != "") {
            var dat = document.getElementById('chqDate').value;
            if (dat == "") {
                //alert('aila');
                alert("Date field should not be empty !");
                document.getElementById('chqDate').focus();
                document.getElementById('chqDate').select();
                return false;
            }
            else if (dat.length < 6) {
                alert("Date not valid !");
                document.getElementById('chqDate').focus();
                document.getElementById('chqDate').select();
                return false;
            }
            else {
                var dd, mm, yy;
                var finldat = new String(dat);
                // alert(finldat);

                dd = finldat.substring(0, 2)
                mm = finldat.substring(2, 4)
                yy = finldat.substring(4, 6)
                //alert(dd+'-'+ mm +'-'+'20'+yy);
                if (dd > 31) {
                    alert('Please enter correct date!');
                    document.getElementById('chqDate').focus();
                    document.getElementById('chqDate').select();
                    return false;
                }
                if (mm > 12) {
                    alert('Please enter correct date!');
                    document.getElementById('chqDate').focus();
                    document.getElementById('chqDate').select();
                    return false;
                }
            }
        }
    }
    document.getElementById('PrevNxtVal').value = document.getElementById(btnid).value;
    return true;
}
function changePassword() {
    //alert('ok');
    //alert(document.getElementById("Aplhanumericmadate").value);
    //alert(document.getElementById("Specialcharmandate").value);
    var newpwd = document.getElementById('NewPassword').value;
    if (document.getElementById('OldPassword').value == "") {
        alert('Please enter correct old password!');
        document.getElementById('OldPassword').focus();
        document.getElementById('OldPassword').select();
        return false;
    }
    if (newpwd == "") {
        alert('Please enter correct new password!');
        document.getElementById('NewPassword').focus();
        return false;
    }
    // alert('Ok'); 
    if (document.getElementById('ConfrmPassword').value == "") {
        alert('Please enter correct confirm password!');
        document.getElementById('ConfrmPassword').focus();
        document.getElementById('ConfrmPassword').select();
        return false;
    }
    if (newpwd.length < document.getElementById("minpwdlength").value) {
        alert('Minimum Password length should be ' + document.getElementById("minpwdlength").value + ' character !');
        document.getElementById('NewPassword').focus();
        document.getElementById('NewPassword').select();
        return false;
    }
    if (newpwd.length > document.getElementById("maxpwdlength").value) {
        alert('Maximum Password length should no be greater than ' + document.getElementById("maxpwdlength").value + ' character !');
        document.getElementById('NewPassword').focus();
        document.getElementById('NewPassword').select();
        return false;
    }

    if ((document.getElementById("Aplhanumericmadate").value == "True") && (document.getElementById("Specialcharmandate").value == "True")) {
        var tt = /^(?=.*[@!#\$\^%&*()+=\-\[\]\\\;,\.\/\{\}\|\:<>\? ]+?).*[^_\W]+?.*/;
        var ret = /^.*(?=.{4,10})(?=.*\d)(?=.*[a-zA-Z]).*$/;
        ///(?=.*[@!#\$\^%&*()+=\-\[\]\\\';,\.\/\{\}\|\":<>\? ]+?).*[^_\W]+?.*/
        if ((!ret.test(newpwd)) || (!tt.test(newpwd))) {
            //alert('fhjkd');
            alert('Password should contain alphanumeric and one special charater and one capital alphabet!');
            document.getElementById('NewPassword').focus();
            document.getElementById('NewPassword').select();
            return false;
        }
    }
    else if (document.getElementById("Aplhanumericmadate").value == "True") {
        //var re = /^(?:[0-9]+[a-z]|[a-z]+[0-9])[a-z0-9]*$/;
        var re = /^.*(?=.{4,10})(?=.*\d)(?=.*[a-zA-Z]).*$/;
        if (!re.test(newpwd)) {
            alert('Password should contain alphanumeric value!');
            document.getElementById('NewPassword').focus();
            document.getElementById('NewPassword').select();
            return false;
        }
    }
    if (document.getElementById('ConfrmPassword').value != document.getElementById('NewPassword').value) {
        alert('New password and confirm password is not matching!');
        document.getElementById('ConfrmPassword').focus();
        document.getElementById('ConfrmPassword').select();
        return false;
    }
    return true;
}

function policydetails() {
    var e = document.getElementById('policynm')

    var strUser = e.options[e.selectedIndex].text;

    if (strUser == "Select") {
        alert('Please select policy type !');
        document.getElementById('policynm').focus();
        return false;
    }
    $(document).ready(function () {
        $("#dialogEditUser").dialog({ autoOpen: false });
        $.ajax({
            url: '/CreateUser/PolicyDetails',
            dataType: 'html',
            data: { name: $("#policynm").val() },
            success: function (data) {
                //alert(data);
                $('#dialogEditUser').html(data);
                $('#dialogEditUser').dialog('open');
            }
        });
    });
}



