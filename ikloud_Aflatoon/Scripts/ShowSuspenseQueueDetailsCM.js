$(document).ready(function () {

    var makerchecker = document.getElementById("MakerCheckerRole").value;
    var MakerReturnReason = document.getElementById("MakerReturnReason").value;
    var MakerReturnReasonDiscription = document.getElementById("MakerReturnReasonDiscription").value;
    var CheckerSendBackToMakerDiscription = document.getElementById("CheckerSendBackToMakerDiscription").value;
    var MakerDecision = document.getElementById("MakerDecision").value;


    var IWDecision = document.getElementById("IWDecision").value;
    var rejectreasondescrpsn = document.getElementById("rejectreasondescrpsn").value;
    var rtncd = document.getElementById("rtncd").value;

    if (makerchecker == "Checker") {
        
        if (MakerReturnReason != "" && MakerDecision != "2") {
            document.getElementById("rtncd").style.display = "";
            document.getElementById("IWDecision").value = "R";
            document.getElementById("IWRemark").value = document.getElementById("MakerReturnReason").value;
            document.getElementById("rejectreasondescrpsn").value = document.getElementById("MakerReturnReasonDiscription").value;

            //disable in checker
           // document.getElementById("IWDecision").disabled = true;
          //  document.getElementById("IWRemark").disabled = true;
          //  document.getElementById("rejectreasondescrpsn").disabled = true;
          //  document.getElementById('rejectreasonmodal').onclick = null;


        }
        else {
            document.getElementById("IWDecision").value = "A";
          //  document.getElementById("IWDecision").disabled = true;
        }
       
    }
    else {
        document.getElementById("rtncd").style.display = 'none';    

        if (CheckerSendBackToMakerDiscription != "") {
            document.getElementById("checkerreturnremark").innerHTML = document.getElementById("CheckerSendBackToMakerDiscription").value;
        }
        else {
            document.getElementById("chekarreturnid").style.display ='none'
        }




    }


    $("#btnSave").click(function () {
        debugger;

        var makerchecker = document.getElementById("MakerCheckerRole").value;

        if (makerchecker == "Maker") {
            var branchId = document.getElementById("BranchId").value;
            var ExceptionId = document.getElementById("ExceptionId").value;

            var ChqNo = document.getElementById("ChqNo").value;
            var XmlSortCode = document.getElementById("XmlSortCode").value;
            var XmlSan = document.getElementById("XmlSan").value;
            var XmlTc = document.getElementById("XmlTc").value;
            var Accno = document.getElementById("AccountNo").value;

            var id = document.getElementById("Id").value;

            var decision = document.getElementById("IWDecision").value;
            var rejectReason = document.getElementById("IWRemark").value;
            var rejectReasonDiscription = document.getElementById("rejectreasondescrpsn").value;

            var AccName = document.getElementById("AccountDropdown").value;


            var accstatus = document.getElementById("accStatus").value;

            var APiData = document.getElementById("API_Data").value;

            //micr validation
            if (!ChqNo || !XmlSortCode || !XmlSan || !XmlTc) {
                let message = "Please fill in the following fields:\n";

                if (!ChqNo) message += "- Cheque Number\n";
                if (!XmlSortCode) message += "- Sort Code\n";
                if (!XmlSan) message += "- San\n";
                if (!XmlTc) message += "- Tc\n";

                alert(message);
                return false; 
            }








            if (accstatus == "Invalid Account") {
                alert("Invalid Account");
                document.getElementById("AccountNo").focus();
                return false;
            }

            if (decision == "") {
                alert("Please Enter Decision");
                document.getElementById("IWDecision").focus();
                return false;
                
            }

            if (decision == "R" || decision == "r") {

                if (rejectReason == "") {
                    alert("Please Enter Reason Code");
                    document.getElementById("IWRemark").focus();
                    return false;


                }


                if (rejectReasonDiscription == "") {
                    alert("Please Select Return Reason");
                    document.getElementById("rejectreasondescrpsn").focus();
                    return false;
                }


                // 88 rejectreason validation for 
                if (rejectReason == "88") {

                   var str1 = document.getElementById("rejectreasondescrpsn").value.toUpperCase();
                    var result = ValidOtherReason(str1);
                    if (result == false)
                    {
                        return false;
                    }


                }



            }


            ob = {
                id: id,
                makerchecker: makerchecker,
                branchId: branchId,
                ExceptionId: ExceptionId,
                ChqNo: ChqNo,
                XmlSortCode: XmlSortCode,
                XmlSan: XmlSan,
                XmlTc: XmlTc,
                decision: decision,
                rejectReason: rejectReason,
                rejectReasonDiscription: rejectReasonDiscription,
                Accnumber: Accno,
                AccountName: AccName,
                APiData: APiData
            };



            $.ajax({
                url: RootUrl + 'IwSuspenseQueueCM/UpdateSQData',
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                data: ob,
                //data: { id: id, makerchecker: makerchecker, branchId: branchId, ExceptionId: ExceptionId, ChqNo: ChqNo, XmlSortCode: XmlSortCode, XmlSan: XmlSan, XmlTc: XmlTc, decision: decision, rejectReason: rejectReason },
                success: function (data) {
                    debugger;
                    if (data != null) {

                        alert(data);

                        var ob = JSON.parse(data);
                        if (ob == "Update successfully") {
                            window.location = RootUrl + 'IwSuspenseQueueCM/Index?branchId=' + branchId + '&ExceptionId=' + ExceptionId;
                        }

                    }
                }
            });

        }
        else if (makerchecker == "Checker")
        {
            var id = document.getElementById("Id").value;
            var branchId = document.getElementById("BranchId").value;
            var ExceptionId = document.getElementById("ExceptionId").value;
            var decision = document.getElementById("IWDecision").value;

            var IWRemark = document.getElementById("IWRemark").value;
            var rejectreasondescrpsn = document.getElementById("rejectreasondescrpsn").value;

            $.ajax({
                url: RootUrl + 'IwSuspenseQueueCM/UpdateSQData_Checker',
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                data: { id: id, makerchecker: makerchecker, decision: decision, ReturnCode: IWRemark, ReturnDescription: rejectreasondescrpsn },
                success: function (data) {
                    debugger;
                    if (data != null) {

                        alert(data);
                        var ob = JSON.parse(data);
                        if (ob == "Save successfully") {

                            window.location = RootUrl + 'IwSuspenseQueueCM/Index?branchId=' + branchId + '&ExceptionId=' + ExceptionId;
                        }

                    }
                }
            });
        }


        


    });



    $("#btnSendBackToMaker").click(function () {
        debugger;

        var SendBckToMakerRemark = document.getElementById("SendBckToMakerRemark").value;
        var id = document.getElementById("Id").value;

        var branchId = document.getElementById("BranchId").value;
        var ExceptionId = document.getElementById("ExceptionId").value;

        if (SendBckToMakerRemark == "") {
            alert("Please Enter Remark ");
            return false;
        }




        $.ajax({
            url: RootUrl + 'IwSuspenseQueueCM/UpdateSendBackToMaker',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            
            data: { id: id, SendBckToMakerRemark: SendBckToMakerRemark},
            success: function (data) {
                debugger;
                if (data != null) {
                    
                    alert(data);
                    var ob = JSON.parse(data);
                    if (ob == "Remark Send successfully") {

                        window.location = RootUrl + 'IwSuspenseQueueCM/Index?branchId=' + branchId + '&ExceptionId=' + ExceptionId;
                    }

                }
            }
        });

    });



    $("#btnClose").click(function () {
        debugger;

        var branchId = document.getElementById("BranchId").value;
        var ExceptionId = document.getElementById("ExceptionId").value;
        

        window.location = RootUrl + 'IwSuspenseQueueCM/Index?branchId=' + branchId + '&ExceptionId=' + ExceptionId;

        // window.location = RootUrl + 'IwSuspenseQueueCM/Index';

    });









    $("#IWRemark").keyup(function (event) {
        debugger;
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


    $("#IWRemarkex").keyup(function (event) {
        debugger;
        var chkcode = false;
        var rejctrcd = $("#IWRemarkex").val();
        if (rejctrcd.length == 2) {
            var rjctresnlTemp = document.getElementById('rtnlistex');
            var rtnlistDescrpTemp = document.getElementById('rtnlistDescrpex');
            for (var i = 0; i < rjctresnlTemp.length; i++) {
                if (rejctrcd == rjctresnlTemp[i].value) {
                    document.getElementById("rejectreasondescrpsnex").value = rtnlistDescrpTemp[i].value;
                    chkcode = true;
                    break;
                }
            }
        }
        if (rejctrcd == "88") {

            document.getElementById("rejectreasondescrpsnex").readOnly = false;

        }
        else if (rejctrcd != "88") {

            document.getElementById("rejectreasondescrpsnex").readOnly = true;
        }
        if (rejctrcd.length == 2) {
            if (chkcode == false) {
                alert('Please enter correct return code!!');
                document.getElementById('IWRemarkex').value = "";
                document.getElementById('IWRemarkex').focus();
            }
        }
    });



    $("#AccountNo").focusout(function (event) {
        GetApiValue();

    });

    GetApiValue();



});


function IWVef() {
    //rtncd
    debugger;
    document.getElementById('rtncd').style.display = "none";
    document.getElementById('rtncdex').style.display = "none";


    chr = document.getElementById('IWDecision').value.toUpperCase();
    var chr = document.getElementById('IWDecision').value.toUpperCase();
    var iwrk = document.getElementById('IWRemark').value;
    var iwrkex = document.getElementById('IWRemarkex').value;
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

            document.getElementById('rejectreasondescrpsn').value = "";
            document.getElementById('IWRemark').value = "";
            document.getElementById('IWDecision').focus();
        }

    }
    else if (chr == "E") {
        if (iwrkex == "") {
            document.getElementById('rtncdex').style.display = "";
            document.getElementById('IWRemarkex').style.width = "10%";
            document.getElementById('rejectreasondescrpsnex').value = "";
            document.getElementById('IWRemarkex').focus();
        }
        else {
            // alert('aila');
            document.getElementById('rtncdex').style.display = "";
            document.getElementById('rejectreasondescrpsnex').value = "";
            document.getElementById('IWRemarkex').value = "";
            document.getElementById('IWDecision').focus();
        }
    }
    else {

        document.getElementById('rtncd').style.display = "none";
        document.getElementById('rtncdex').style.display = "none";
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


function exreasonselected(rtnval) {
    debugger;
    document.getElementById('IWRemarkex').value = rtnval;
    document.getElementById('RejectReasonforExtension').style.display = 'none';
    var rejctrcd = $("#IWRemarkex").val();
    if (rejctrcd.length == 2) {
        var rjctresnlTemp = document.getElementById('rtnlistex');
        var rtnlistDescrpTemp = document.getElementById('rtnlistDescrpex');
        for (var i = 0; i < rjctresnlTemp.length; i++) {
            if (rejctrcd == rjctresnlTemp[i].value) {
                //if (rejctrcd == "88") {
                //    document.getElementById("rejectreasondescrpsn").value = rtnrjctdescrn;
                //}
                //else {
                document.getElementById("rejectreasondescrpsnex").value = rtnlistDescrpTemp[i].value;
                //}
                break;
            }
        }
    }
    // }
}


function checkInput() {
    const input = document.getElementById("SendBckToMakerRemark").value;
    const button = document.getElementById("btnSave");

    // Disable button if the input is not empty, enable it otherwise
    button.disabled = input.trim() !== "";
}


function GetApiValue() {
    debugger;

    var makerchecker = document.getElementById("MakerCheckerRole").value;

    if (makerchecker == "Checker") {
        document.getElementById("AccountNo").removeAttribute("disabled");
    }

    var Acct = document.getElementById('AccountNo').value;
    var acmin = document.getElementById('acmin').value;
    //Acct = Acct.replace(/^0+/, '')
    Acct = Acct.length


    if (Acct == "") {
        alert("Account no field should not be empty !");
       // document.getElementById('AccountNo').focus();
        return false;
    }
    if (Acct < acmin) {

        alert("Minimum account no sould be " + acmin + " digits");
       // document.getElementById('AccountNo').focus();
        return false;
    }
    Acct = document.getElementById('AccountNo').value.replace(/^0+/, '')
    if (Acct == "") {
        alert("Invalid Account Number!");
        document.getElementById('AccountNo').focus();
        return false;
    }
    else {


        $.ajax({
            url: RootUrl + 'IwSuspenseQueueCM/GetCurrentAPIValue',
            dataType: 'html',
            type: 'POST',
            async:false,
            data: { ac: $("#AccountNo").val() },
            success: function (response) {

                debugger;

                var data = JSON.parse(response);

                if (makerchecker == "Checker")
                {
                    document.getElementById("AccountNo").setAttribute("disabled", "disabled");
                }

                document.getElementById("accStatus").value = data.AccStatus;

                if (data.AccStatus == "Invalid Account") {
                    alert("Invalid Account");

                }

                document.getElementById("API_Data").value = data.API_Data;

                document.getElementById("currentavlableamt").innerHTML = data.CurrentAccBal;

                var payeeNameField = document.getElementById("AccountDropdown");

                if (Array.isArray(data.PayeeName) && data.PayeeName.length > 0) {
                    payeeNameField.innerHTML = "";
                    data.PayeeName.forEach(function (name) {
                        let option = document.createElement("option");
                        option.value = name;
                        option.textContent = name;
                        payeeNameField.appendChild(option);
                    });
                }
                else
                {
                    payeeNameField.textContent = "No AccName found";
                }



            },
            error: function (e) {
                console.log(e);
                alert("error");
            }
        });
    }



}


function ValidOtherReason(str1) {

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

    return true;

}