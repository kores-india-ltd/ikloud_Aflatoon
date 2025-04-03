var dataLists = "";

$(document).ready(function () {

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

    var waiveC = document.getElementById('waiveCharge').value;

    if (waiveC == 'N') {
        document.getElementById('waiveYes').checked = false;
        document.getElementById('waiveNo').checked = true;
    }
    else {
        document.getElementById('waiveYes').checked = true;
        document.getElementById('waiveNo').checked = false;
    }

    var status = document.getElementById('DecisionTaken').value;
    var rejectCode = document.getElementById('RejectCodeTaken').value;
    debugger;
    if (status == "2" || status == "3") {
        var statusText = "";
        if (status == "2") {
            statusText = "A";
            document.getElementById('Decision').value = statusText;
            
            IWVef();
        }
        else {
            statusText = "R";
            document.getElementById('Decision').value = statusText;
            document.getElementById('IWRemark').value = rejectCode;
            IWVef();
            debugger;
            reasonselected(rejectCode);
        }
        
    }

    //$("#btnClose").click(function () {
    //    debugger;
    //    //console.log("In Close Home button");
    //    //var url = RootUrl + "IwSuspenseQueue/Index";
    //    //window.location.href = url;
    //    //window.location.href = window.history.back()
    //    //history.go(-1);
    //    //var url = window.
    //    history.back();
    //});

    $("#btnSubmit").click(function () {

        

        var result = IWQC();

        if (result == false) {

            return false;
        }
        else {
            var currentId = document.getElementById('currentId').value;
            var IWdecn = document.getElementById('Decision').value.toUpperCase();
            var rejectCode = document.getElementById('IWRemark').value.toUpperCase();
            var isWaiveCharges = $("input[type='radio'][name='waiveCharges']:checked").val();
            debugger;
            $.ajax({
                url: RootUrl + 'IwSuspenseQueue/Update_SuspenseQueue_Data',
                //type: 'Post',
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                data: { Id: currentId, Decision: IWdecn, RejectCode: rejectCode, IsWaiveCharges: isWaiveCharges },
                //async: false,
                success: function (data) {
                    console.log(data);
                    debugger;

                    /*if (data == "Suceess") {*/
                        var branchId = document.getElementById('branchCode').value;

                        var url = RootUrl + "IwSuspenseQueue?branchId=" + branchId;
                        window.location.href = url;

                        //$.ajax({
                        //    url: RootUrl + 'IwSuspenseQueue/Index',
                        //    // type: 'Post',
                        //    contentType: 'application/json; charset=utf-8',
                        //    dataType: 'html',
                        //    data: { branchCode: branchCode },
                        //    //async: false,
                        //    success: function (data) {
                        //        console.log(data);
                        //        debugger;


                        //    },
                        //    error: function () {

                        //        alert("Record not updated, please try again!!!");
                        //    }

                        //});
                        
                    /*}*/

                },
                error: function () {
                    
                    alert("Record not updated, please try again!!!");
                }

            });
        }

        
    });

    $("#btnViewImage").click(function () {
        debugger;
        var branchCode = document.getElementById('branchCode').value;
        var chequeAmount = document.getElementById('chequeAmount').value;
        var chequeNumber = document.getElementById('chequeNumber').value;
        var udk = document.getElementById('udk').value;
        var udkItemSeqNo = "";
        var udkPresentmentDate = "";
        var udkChequeNo = "";
        var udkSortCode = "";
        var udkTransCode = "";

        udkItemSeqNo = udk.substr(1, 14);
        udkPresentmentDate = udk.substr(15, 8);
        udkChequeNo = udk.substr(23, 6);
        udkSortCode = udk.substr(29, 9);
        udkTransCode = udk.substr(38, 2);
        
        debugger;
        $.ajax({
            url: RootUrl + 'IwSuspenseQueue/Get_SuspenseQueue_ImageData',
            //type: 'GET',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: {
                BranchCode: branchCode, ChequeAmount: chequeAmount, ChequeNumber: chequeNumber, udkItemSeqNo: udkItemSeqNo,
                udkPresentmentDate: udkPresentmentDate, udkChequeNo: udkChequeNo, udkSortCode: udkSortCode, udkTransCode: udkTransCode
            },
            //data: JSON.stringify({ BranchCode: branchCode, ChequeAmount: chequeAmount, ChequeNumber: chequeNumber }),
            //async: false,
            success: function (data) {
                console.log(data);
                debugger;
                dataLists = JSON.parse(data);
                document.getElementById("tblDetails").style.display = "";
                $("#tblDetails").find("tr:gt(0)").remove();
                var dataString = "";
                for (var x = 0; x < dataLists.length; x++) {
                        debugger;
                    var path = dataLists[x].FrontGreyImagePath.replace('/','//');
                    dataString = dataString + "<tr><td style='display: none;'>" + x + "</td ><td>" + dataLists[x].ChequeNo + "</td> <td>" + dataLists[x].SortCode + "</td> <td>"
                        + dataLists[x].SAN + "</td><td>" + dataLists[x].TransCode + "</td><td>" + dataLists[x].ChequeAmount
                        + "</td><td><a onclick='loadRecord(" + x + ");'>View</a></td></tr>";
                }

                $('#tbody').append(dataString);


            },
            error: function (data) {
                debugger;
                alert("Service Unavailable, please try again!!!");
            }

        });
       
    });
});

function IWQC() {
    var IWdecn = document.getElementById('Decision').value.toUpperCase();

    if (IWdecn == "") {
        alert('Please enter decision!');
        document.getElementById('Decision').focus();
        return false;
    }

    else if (IWdecn != "A" && IWdecn != "R" && IWdecn != "F") {
        // alert(IWdecn);
        alert('Decision not correct!');
        document.getElementById('Decision').focus();
        return false;
    }
    else if (IWdecn == "R") {

        if (document.getElementById('IWRemark').value == "") {
            alert('Please enter reject reason code!');
            document.getElementById('IWRemark').focus();
            return false;
        }

        if (document.getElementById('rejectreasondescrpsn').value == "") {
            alert('Please enter reject reason description!');
            document.getElementById('rejectreasondescrpsn').focus();
            return false;
        }


    }
}

function gotoPreviousPage(branchId) {
    console.log("BranchCode - " + branchId);

    var url = RootUrl + "IwSuspenseQueue?branchId=" + branchId;
    window.location.href = url;
}

function IWVef() {
    //rtncd
    document.getElementById('rtncd').style.display = "none";
    debugger;
    var bankCode = document.getElementById('BankCode').value;
    chr = document.getElementById('Decision').value.toUpperCase();
    var chr = document.getElementById('Decision').value.toUpperCase();
    document.getElementById('Decision').value = chr;
    var iwrk = document.getElementById('IWRemark').value;
    if (chr == "R") {
        if (iwrk == "") {
            if (bankCode === "641") {
                //document.getElementsByClassName("test").style.display = "";
                document.getElementById('rtncd').style.display = "";
                var module = $('.test');
                $('.test').css("display", "none");
            }
            else {
                document.getElementById('rtncd').style.display = "";
                document.getElementById('IWRemark').style.width = "30%";
                document.getElementById('IWRemark').focus();
            }

        }
        else {
            // alert('aila');
            document.getElementById('rtncd').style.display = "";
            document.getElementById('Decision').focus();
        }

    }
    else {
        document.getElementById('rtncd').style.display = "none";
        document.getElementById('IWRemark').value = "";
        document.getElementById("rejectreasondescrpsn").value = "";
    }
}

function reasonselected(rtnval) {
    debugger;
    document.getElementById('IWRemark').value = rtnval;
    document.getElementById('RejectReason').style.display = 'none';
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
    // }
}

function loadRecord(index) {
    console.log(index);
    var imagePath = dataLists[index].FrontGreyImagePath;
    //imagePath = "C:/Amol/1_Front.jpg";
    document.getElementById('imageDiv').style.display = "";
    document.getElementById('myfulimg').src = imagePath;
}

function hideImage() {
    document.getElementById("imageDiv").style.display = "none";
}