
var data;
var tt;
var lesscnt;
var backbtn;
var backcnt;
var scond = false;
var cnrslt;
var dtte;
var cnt;
var tempcnt;
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
    //-------------- idslist--------------------//
    for (var z = 1; z < tt.length; z++) {
        idslst.push(tt[z].ID)
    }
    $('#iwDateQc').bind("cut copy paste", function (e) {
        e.preventDefault();
    });

    $(document).bind("contextmenu", function (e) {

        e.preventDefault();

    });
    //-------------Full Image Calling---------------
    //-------------Full Image Calling---------------

    $("#iwDateQc").keydown(function (event) {
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
    if (document.getElementById('nodata').value != false) {
        if (tt.length > 0) {
            // alert('Aila');
            var gf = $("#DEbySnpt").val();
            //  alert(gf);
            if (gf == "True") {
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
            document.getElementById('iwDateQc').focus();
            document.getElementById("btnback").disabled = true;
        }
    }

    $("#btnok").click(function () {
        // alert('jalebi');
        var result = Dvalid()
        if (result == false) {
            document.getElementById('iwDateQc').focus();
            document.getElementById('iwDateQc').select();
        }
        else {
            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var iwDateQc = "iwDateQc";

            if (backbtn == true) {

                var tot = MatchWith(backcnt);
                if (tot == 2) {
                    cnrslt = confirm("Entered value is not maching with D1 and D2\n are sure to accept with this value?");
                    if (cnrslt == false) {
                        document.getElementById('iwDateQc').focus();
                        document.getElementById('iwDateQc').select();
                    }
                }
                else {
                    cnrslt = true;
                }
                iwDateQc = iwDateQc + backcnt
                dtte = {
                    "Date": $("#iwDateQc").val(),
                    "ID": tt[backcnt].ID,
                    "sttsdtqc": cnrslt,
                    "ICRDate": tt[backcnt].ICRDate,
                    "EntryDate": tt[backcnt].EntryDate,
                };
            }

            else {
                var tot1 = MatchWith(tempcnt);
                if (tot1 == 2) {
                    cnrslt = confirm("Entered value is not maching with D1 and D2\n are sure to accept with this value?");
                    if (cnrslt == false) {
                        document.getElementById('iwDateQc').focus();
                        document.getElementById('iwDateQc').select();
                    }
                }
                else {
                    cnrslt = true;
                }
                iwDateQc = iwDateQc + cnt;
                dtte = {
                    "Date": $("#iwDateQc").val(),
                    "ID": tt[tempcnt].ID,
                    "sttsdtqc": cnrslt,
                    "ICRDate": tt[tempcnt].ICRDate,
                    "EntryDate": tt[tempcnt].EntryDate,
                };

            }
            if (cnrslt == true) {
                common(iwDateQc);
            }
            else {
                // alert('Okk');
                document.getElementById('iwDateQc').focus();
                document.getElementById("btnback").disabled = true;
            }


        }
    });

    //-------------------------------------Reject--------------------------------//
    $("#btnRejct").click(function () {

        if ($("#iwDateQc").val() == "" || $("#iwDateQc").val().length < 6) {
            alert("Please enter 6 times '000000'(zero's)");
            document.getElementById('iwDateQc').focus();
            document.getElementById('iwDateQc').select();
            return false;
        }
        else {
            document.getElementById("btnback").disabled = false;
            cnt = document.getElementById('cnt').value;
            tempcnt = document.getElementById('tempcnt').value;
            var iwDateQc = "iwDateQc";

            if (backbtn == true) {

                iwDateQc = iwDateQc + backcnt
                dtte = {
                    "Date": "xxxxxx",
                    "ID": tt[backcnt].ID,
                    "sttsdtqc": false,
                    "ICRDate": tt[backcnt].ICRDate,
                    "EntryDate": tt[backcnt].EntryDate,
                };
            }

            else {

                iwDateQc = iwDateQc + cnt;
                dtte = {
                    "Date": "xxxxxx",
                    "ID": tt[tempcnt].ID,
                    "sttsdtqc": false,
                    "ICRDate": tt[tempcnt].ICRDate,
                    "EntryDate": tt[tempcnt].EntryDate,
                };

            }
            common(iwDateQc);
            backbtn = false;
        }


        $("input[value='Accept']").attr("disabled", false);
    });
    //----------------------------------------Back Button-------------------------//

    $("#btnback").click(function () {

        document.getElementById("btnback").disabled = true;
        //if (scondbck == true) {

        //    if (Modernizr.localstorage) {
        //        alert('iw0');
        //        backbtn = true;
        //        var iwdate = "iwdate0"
        //        var localData = window.localStorage;
        //        var orderData = JSON.parse(localData.getItem(iwdate));

        //        document.getElementById('myimg').src = tt[0].FrontGreyImagePath;

        //        document.getElementById('iwDate').value = tt[0].EntryDate;
        //        document.getElementById('iwDate').focus();
        //    }
        //}
        //else {
        if (Modernizr.localstorage) {

            backbtn = true;
            var iwDateQc = "iwDateQc"
            var cnt = document.getElementById('cnt').value;
            iwDateQc = iwDateQc + (parseInt(cnt) - 1)
            backcnt = parseInt(cnt) - 1;
            var localData = window.localStorage;
            var orderData = JSON.parse(localData.getItem(iwDateQc));

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

            document.getElementById('iwDateQc').value = orderData.Date;
            document.getElementById('iwDateQc').focus();
        }
        //}

    });
    $("#bbtnClose").click(function () {

        if (Modernizr.localstorage) {
            var listItems = [];
            var arrlist = [];
            var localData = window.localStorage;

            if (scond == true) {
                for (var i = 0; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwDateQc" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.Date);
                    arrlist.push(orderData.sttsdtqc);
                    arrlist.push(orderData.ICRDate);
                    arrlist.push(orderData.EntryDate);
                }
            }
            else {
                for (var i = 1; i < cnt; i++) {

                    var orderData = JSON.parse(localData.getItem("iwDateQc" + i));
                    arrlist.push(orderData.ID);
                    arrlist.push(orderData.Date);
                    arrlist.push(orderData.sttsdtqc);
                    arrlist.push(orderData.ICRDate);
                    arrlist.push(orderData.EntryDate);
                }
            }

            $.ajax({

                url: RootUrl + 'IWDataEntry/IWDateQC',
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
        var next_idx = $('input[type=text]').index(this) + 1;
        var tot_idx = $('body').find('input[type=text]').length;
        if (e.keyCode == 13) {
            if (tot_idx == next_idx) {
                $("input[value='Accept']").attr("disabled", true);
                //go to the first text element if focused in the last text input element12.	
                $("input[value='Accept']").focus().click();
            }
            else
                //go to the next text input element.
                $('input[type=text]:eq(' + next_idx + ')').focus().select();
            /// $("input[value='Accept']").focus().click();
            //                $('button[type=submit] .NavButton').click();
            // return true;
        }
    });
    $("#iwDateQc").keypress(function (event) {

        if (event.shiftKey) {
            //if ((event.keyCode >= 44 && event.keyCode <= 59) || (event.keyCode >= 91 && event.keyCode >= 93) || event.keyCode == 61 || event.keyCode == 39) {
            event.preventDefault();
            //}
        }

        if (event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 38 || event.keyCode == 40 || (event.charCode > 47 && event.charCode < 58)) {
            // alert('if');
        }
        else {
            event.preventDefault();
        }
    });

    //-------------------------------Common For Accept and Reject----------------
    function common(val) {
        //alert('cool!!');
        if (Modernizr.localstorage) {
            var localacct = window.localStorage;
            var chqdate = JSON.stringify(dtte);
            localacct.setItem(val, chqdate);
        }

        if (backbtn == true) {
            document.getElementById('cnt').value = parseInt(backcnt) + 1;
        }
        else {
            document.getElementById('cnt').value = parseInt(cnt) + 1;
        }
        cnt = document.getElementById('cnt').value;
        if (cnt < tt.length) {

            if (document.getElementById('DEbySnpt').value == "True") {

                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "";
                document.getElementById("myfulimg").src = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "none";
            }
            else {
                document.getElementById('myimg').src = tt[cnt].FrontGreyImagePath;
                document.getElementById('AcSnipimg').style.display = "none";
                document.getElementById("myfulimg").src = tt[cnt].FrontTiffImagePath;// document.getElementById("FrontGrayImgPath").value;
                document.getElementById('AcFullimg').style.display = "";
            }
            fullimg = tt[cnt].FrontTiffImagePath;
            document.getElementById('iwDateQc').value = "";
            document.getElementById('iwDateQc').focus();
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

                if (scond == true) {
                    for (var i = 0; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwDateQc" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.Date);
                        arrlist.push(orderData.sttsdtqc);
                        arrlist.push(orderData.ICRDate);
                        arrlist.push(orderData.EntryDate);
                    }
                }
                else {
                    for (var i = 1; i < cnt; i++) {

                        var orderData = JSON.parse(localData.getItem("iwDateQc" + i));
                        arrlist.push(orderData.ID);
                        arrlist.push(orderData.Date);
                        arrlist.push(orderData.sttsdtqc);
                        arrlist.push(orderData.ICRDate);
                        arrlist.push(orderData.EntryDate);
                    }
                }

                //------------------------------- Calling Ajax for taking more data------------------

                $.ajax({

                    url: RootUrl + 'IWDataEntry/IWDateQC',
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
                                //-------------- idslist--------------------//
                                for (var z = 0; z < tt.length; z++) {
                                    idslst.push(tt[z].ID)
                                }
                                
                                //-------Remove save objects from browser---//
                                window.localStorage.clear();
                                //-------------Saving Last data in storage---
                                var iwDateQc = "iwDateQc0"
                                var dtte = {
                                    "Date": tt[0].QCDate,
                                    "ID": tt[0].ID,
                                    "sttsdtqc": tt[0].sttsdtqc,
                                    "ICRDate": tt[0].ICRDate,
                                    "EntryDate": tt[0].EntryDate,
                                };
                                if (Modernizr.localstorage) {

                                    var localacct = window.localStorage;
                                    var chqdate = JSON.stringify(dtte);
                                    localacct.setItem(iwDateQc, chqdate);

                                }
                                //----------------------------------------------------------------------//
                                if (document.getElementById('DEbySnpt').value == "True") {
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
                                document.getElementById('iwDateQc').value = "";
                                document.getElementById('iwDateQc').focus();
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


function Dvalid() {

    var dat = document.getElementById('iwDateQc').value;
    if (dat == "") {
        //alert('aila');
        alert("Date field should not be empty !");
        document.getElementById('iwDateQc').focus();
        document.getElementById('iwDateQc').select();
        return false;
    }
    else if (dat == "000000") {
        alert("Date not valid !");
        document.getElementById('iwDateQc').focus();
        document.getElementById('iwDateQc').select();
        return false;
    }
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

        var dd, mm, yy;
        var finldat = new String(dat);
        //// alert(finldat);

        dd = finldat.substring(0, 2)
        mm = finldat.substring(2, 4)
        yy = finldat.substring(4, 6)

        var onlydate = dd + '/' + mm + '/' + '20' + yy;
        var rtn = validatedate(onlydate);
        if (rtn == false) {
            return false;
        }

        //var finldat = new String(dat);
        //// alert(finldat);

        //dd = finldat.substring(0, 2)
        //mm = finldat.substring(2, 4)
        //yy = finldat.substring(4, 6)
        ////alert(dd+'-'+ mm +'-'+'20'+yy);
        //var d = new Date();
        //var n = d.getFullYear().toString().substring(2);
        ////alert(n);
        //if (yy > n) {
        //    alert('Please enter correct date!');
        //    return false;
        //}
        //if (dd > 31) {
        //    alert('Please enter correct date!');

        //    return false;
        //}
        //if (mm > 12) {
        //    alert('Please enter correct date!');

        //    return false;
        //}
    }
    //------------------------------------Post Date and Stale Cheques ----///

    var stlmntdt = document.getElementById('stlmnt').value;
    var sesondt = document.getElementById('sesson').value;

    var fnldate = '20' + yy + '/' + mm + '/' + dd;
    var staldat = new Date(sesondt);
    var postdat = new Date(stlmntdt);
    var d3 = new Date(fnldate);

    var timeDiff = Math.abs(staldat.getTime() - d3.getTime());
    var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));


    if (postdat < d3) {
        alert('Post date\n Please Reject this cheque!!');
        return false;
    }
    if (staldat >= d3) {
        alert('Stale Cheque\\n Please Reject this cheque!!');
        return false;
    }
}
function MatchWith(indexvl) {
    var icr = 0;
    var mnl = 0;
    var finaldate;
    var tempdate = document.getElementById('iwDateQc').value;
    finaldate = "20" + tempdate.substring(4, 6) + "-" + tempdate.substring(2, 4) + "-" + tempdate.substring(0, 2);

    if (tt[indexvl].ICRDate != finaldate) {
        icr = 1;
        // alert(tt[indexvl].ICRDate + " :" + finaldate);
    }
    if (tt[indexvl].EntryDate != finaldate) {
        mnl = 1;
        //alert(tt[indexvl].EntryDate + " :" + finaldate);
    }
    // alert((icr + mnl));
    return (icr + mnl);
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
function fullImage() {
    //  alert('ok');
    document.getElementById('iwimg').style.display = 'block'
    // alert(document.getElementById('myimg').data);
    document.getElementById('modelmyfulimg').src = fullimg;// document.getElementById('myimg').src;
}
function ChangeImage(imagetype) {
    // alert(imagetype);
    debugger;
    var indexcnt = document.getElementById('cnt').value;
    if (imagetype == "FTiff") {

        document.getElementById('myfulimg').src = fullimg.replace(/_G/g, "F");//tt[indexcnt].FrontTiffImagePath.replace(/_G/g, "F");
    }
    else if (imagetype == "BTiff") {
        //alert('Browser not supporting!!!');
        document.getElementById('myfulimg').src = fullimg.replace(/_G/g, "_B");//tt[indexcnt].FrontTiffImagePath.replace(/_G/g, "_B");
    }
    else if (imagetype == "FGray") {

        document.getElementById('myfulimg').src = fullimg;//tt[indexcnt].FrontTiffImagePath;
    }

}