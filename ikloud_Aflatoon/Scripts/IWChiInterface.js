function Close() {
    //console.log("In home page");
    var url = RootUrl + "Home/IWIndex";
    window.location.href = url;
}

function CheckStatus() {
    DisableAllButtons();
    $.ajax({
        url: RootUrl + 'IWChiInterface/CheckDEM_Session',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: {},
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            if (data.IS_Create_RRF_Status === "2" || data.IS_Create_RRF_Status === "4") {
                alert("Create RRF Task Completed Successfully!!!");
                EnableAllButtons();
                //document.getElementById('CreateCXF').disabled = false;
                //document.getElementById('UploadCXF').disabled = false;
                //document.getElementById('DownloadRES_OACK_CXF').disabled = false;
                //document.getElementById('Show_Records_CXF').disabled = false;
                //document.getElementById('Show_Files_CXF').disabled = false;
            }
            if (data.IS_Upload_RRF_Status === "2" || data.IS_Upload_RRF_Status === "4") {
                alert("Upload RRF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Load_Res_Ocr_RRF_Status === "2" || data.IS_Load_Res_Ocr_RRF_Status === "4") {
                alert("Load Res Ocr RRF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Load_PXF_Status === "2" || data.IS_Load_PXF_Status === "4") {
                alert("Load PXF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Extract_Data_PXF_Status === "2" || data.IS_Extract_Data_PXF_Status === "4") {
                alert("Extract Data PXF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Generate_RMI_Status === "2" || data.IS_Generate_RMI_Status === "4") {
                alert("Generate RMI Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Generate_PXF_Status === "2" || data.IS_Generate_PXF_Status === "4") {
                alert("Generate PXF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Read_RRF_Status === "2" || data.IS_Read_RRF_Status === "4") {
                alert("Read RRF Task Completed Successfully!!!");
                EnableAllButtons();
            }
        }
    });
}

function TerminateRequest() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/TerminateRequest',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: {},
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            EnableAllButtons();
        }
    });
}

function DisableAllButtons() {
    document.getElementById('CreateRRF').disabled = true;
    document.getElementById('UploadRRF').disabled = true;
    document.getElementById('LoadResOcrRRF').disabled = true;
    document.getElementById('LoadCHM').disabled = true;
    //document.getElementById('Show_Records_RRF').disabled = true;
    //document.getElementById('Show_Files_RRF').disabled = true;

    document.getElementById('LoadPXF').disabled = true;
    document.getElementById('ExtractDataPXF').disabled = true;
    document.getElementById('GenerateRMI').disabled = true;
    //document.getElementById('Show_Records_PXF').disabled = true;
    //document.getElementById('Show_Files_PXF').disabled = true;

    document.getElementById('GeneratePXF').disabled = true;
    //document.getElementById('Show_Records_LVB_PXF').disabled = true;
    //document.getElementById('Show_Files_LVB_PXF').disabled = true;

    document.getElementById('ReadRRF').disabled = true;
    //document.getElementById('Show_Records_LVB_RRF').disabled = true;
    //document.getElementById('Show_Files_LVB_RRF').disabled = true;
}

function EnableAllButtons() {
    document.getElementById('CreateRRF').disabled = false;
    document.getElementById('UploadRRF').disabled = false;
    document.getElementById('LoadResOcrRRF').disabled = false;
    document.getElementById('LoadCHM').disabled = false;
    //document.getElementById('Show_Records_RRF').disabled = false;
    //document.getElementById('Show_Files_RRF').disabled = false;

    document.getElementById('LoadPXF').disabled = false;
    document.getElementById('ExtractDataPXF').disabled = false;
    document.getElementById('GenerateRMI').disabled = false;
    //document.getElementById('Show_Records_PXF').disabled = false;
    //document.getElementById('Show_Files_PXF').disabled = false;

    document.getElementById('GeneratePXF').disabled = false;
    //document.getElementById('Show_Records_LVB_PXF').disabled = false;
    //document.getElementById('Show_Files_LVB_PXF').disabled = false;

    document.getElementById('ReadRRF').disabled = false;
    //document.getElementById('Show_Records_LVB_RRF').disabled = false;
    //document.getElementById('Show_Files_LVB_RRF').disabled = false;
}

function CreateRRF() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/CreateRrf',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 10 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('CreateRRF').disabled  = true;
            document.getElementById('CreateRRF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function UploadRRF() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/UploadRrf',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 11 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('UploadRRF').disabled = true;
            document.getElementById('UploadRRF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function LoadResOcrRRF() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/LoadResOcrRrf',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 12 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('LoadResOcrRRF').disabled = true;
            document.getElementById('LoadResOcrRRF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

//========== Added LoadCHM functionality by Amol on 29/05/2024 start ==========
function LoadCHM() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/LoadCHM',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 30 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('LoadResOcrRRF').disabled = true;
            document.getElementById('LoadCHM').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}
//========== Added LoadCHM functionality by Amol on 29/05/2024 end ==========


function LoadPXF() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/LoadPxf',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 1 },
        success: function (data) {
            console.log(data);
            debugger;
            if (data.Result4 == "F") {
                //alert(data.Result5);
                let res = false;
                res = confirm(data.Result5);
                if (res == false) {
                    return false;
                }
                else {
                    $.ajax({
                        url: RootUrl + 'IWChiInterface/LoadPxf',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'JSON',
                        data: { Id: 1, UserConfirmed: 'Y' },
                        success: function (data) {
                            console.log(data);
                            debugger;
                            alert(data.Result1);
                            //document.getElementById('LoadPXF').disabled = true;
                            document.getElementById('LoadPXF').style.backgroundColor = 'gray';
                            DisableAllButtons();
                        }
                    });
                }
            }
            else {
                alert(data.Result1);
                //document.getElementById('LoadPXF').disabled = true;
                document.getElementById('LoadPXF').style.backgroundColor = 'gray';
                DisableAllButtons();
            }
            
        }
    });
}

function ExtractDataPXF() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/ExtractDataPxf',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 2 },
        success: function (data) {
            console.log(data);
            debugger;
            if (data.Result4 == "F") {
                //alert(data.Result5);
                let res = false;
                res = confirm(data.Result5);
                if (res == false) {
                    return false;
                }
                else {
                    $.ajax({
                        url: RootUrl + 'IWChiInterface/ExtractDataPxf',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'JSON',
                        data: { Id: 2, UserConfirmed: 'Y' },
                        success: function (data) {
                            console.log(data);
                            debugger;
                            alert(data.Result1);
                            //document.getElementById('ExtractDataPXF').disabled = true;
                            document.getElementById('ExtractDataPXF').style.backgroundColor = 'gray';
                            DisableAllButtons();
                        }
                    });
                }
            }
            else {
                alert(data.Result1);
                //document.getElementById('ExtractDataPXF').disabled = true;
                document.getElementById('ExtractDataPXF').style.backgroundColor = 'gray';
                DisableAllButtons();
            }
            
        }
    });
}

function GenerateRMI() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/GenerateRmi',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 3 },
        success: function (data) {
            console.log(data);
            debugger;
            if (data.Result4 == "F") {
                //alert(data.Result5);
                let res = false;
                res = confirm(data.Result5);
                if (res == false) {
                    return false;
                }
                else {
                    $.ajax({
                        url: RootUrl + 'IWChiInterface/GenerateRmi',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'JSON',
                        data: { Id: 3, UserConfirmed: 'Y' },
                        success: function (data) {
                            console.log(data);
                            debugger;
                            alert(data.Result1);
                            //document.getElementById('GenerateRMI').disabled = true;
                            document.getElementById('GenerateRMI').style.backgroundColor = 'gray';
                            DisableAllButtons();
                        }
                    });
                }
            }
            else {
                alert(data.Result1);
                //document.getElementById('GenerateRMI').disabled = true;
                document.getElementById('GenerateRMI').style.backgroundColor = 'gray';
                DisableAllButtons();
            }
            
        }
    });
}

function GeneratePXF() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/GeneratePxf',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 20 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('GeneratePXF').disabled = true;
            document.getElementById('GeneratePXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function ReadRRF() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/ReadRrf',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 21 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('ReadRRF').disabled = true;
            document.getElementById('ReadRRF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function Show_Files_RRF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'IWChiInterface/Show_Files_RRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        success: function (data) {
            console.log(data);

            $('#tableData').html(data);
        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });
    document.getElementById('basicModal').style.display = 'block';
}

function Show_Files_PXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'IWChiInterface/Show_Files_PXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        success: function (data) {
            console.log(data);

            $('#tableData').html(data);
        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });
    document.getElementById('basicModal').style.display = 'block';
}

function Show_Files_LVB_PXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'IWChiInterface/Show_Files_LVB_PXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        success: function (data) {
            console.log(data);

            $('#tableData').html(data);
        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });
    document.getElementById('basicModal').style.display = 'block';
}

function Show_Files_LVB_RRF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'IWChiInterface/Show_Files_LVB_RRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        success: function (data) {
            console.log(data);

            $('#tableData').html(data);
        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });
    document.getElementById('basicModal').style.display = 'block';
}

//================== Show Records Count ======================================

function Show_Records_RRF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'IWChiInterface/Show_Records_RRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        success: function (data) {
            console.log(data);

            $('#tableData').html(data);
        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });
    document.getElementById('basicModal').style.display = 'block';
}

function Show_Records_PXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'IWChiInterface/Show_Records_PXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        success: function (data) {
            console.log(data);

            $('#tableData').html(data);
        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });
    document.getElementById('basicModal').style.display = 'block';
}

function Show_Records_LVB_PXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'IWChiInterface/Show_Records_LVB_PXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        success: function (data) {
            console.log(data);

            $('#tableData').html(data);
        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });
    document.getElementById('basicModal').style.display = 'block';
}

function Show_Records_LVB_RRF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'IWChiInterface/Show_Records_LVB_RRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        success: function (data) {
            console.log(data);

            $('#tableData').html(data);
        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });
    document.getElementById('basicModal').style.display = 'block';
}

//mark all accept/return
function MarkAllAccept() {
    // alert("Test");
    var flag = "accept";
    document.getElementById('login').style.display = 'block';
    document.getElementById('user').value = "";
    document.getElementById('passd').value = "";


}

function MarkAllReject() {
    var flag = "rejct";
    document.getElementById('login').style.display = 'block';
    document.getElementById('user').value = "";
    document.getElementById('passd').value = "";

}

function openModal(value) {
    debugger;

    if (value == "Accept") {

        var timestamp = document.getElementById("ExpiryTimeForAccept").value;
        if (timestamp == "") {
            alert("Please select Expiry Time.");

            return false;
        }



        flag = "Accept";
    }
    else if (value == "reject") {

        var timestamp = document.getElementById("ExpiryTimeForReject").value;
        var returncode = document.getElementById("returnreason").value;
        var returnDiscription = document.getElementById("returnreason").value;

        var code = returncode.split(' - ')[0];

        if (timestamp == "") {
            alert("Please select Expiry Time.");

            return false;
        }

        if (code === "") {
            alert("Please select a reject reason.");

            return false;
        }

        if (code == "88") {
            if (document.getElementById("enterReturnReason").value == "") {
                alert("Please Enter Other reason");
                document.getElementById("enterReturnReason").focus();
                return false;
            }
        }

        flag = "reject";
    }

    document.getElementById('login').style.display = 'block';
    document.getElementById('user').value = "";
    document.getElementById('user').focus();
    document.getElementById('passd').value = "";


}

function cancelclick() {
    document.getElementById('login').style.display = 'none';
}
function loginclick() {
    debugger;

    if ($("#user").val() == "" || $("#user").val() == null) {
        alert('Please enter user name !');
        $("#user").focus();
        return false;
    }
    if ($("#passd").val() == "" || $("#passd").val() == null) {
        alert('Please enter password !');
        $("#passd").focus();
        return false;
    }


    //-----------------Encrept the Data----------------
    valpass = $("#passd").val();
    var xyz = "";
    var PQR = "";
    //for (var i = 0; i < valpass.length; i++) {
    //    xyz = xyz + String.fromCharCode(valpass.charCodeAt(i) - 13);
    //}
    xyz = valpass;

    $.ajax({

        url: RootUrl + 'SOD/login',
        type: "POST",
        dataType: "html",
        data: { uname: $("#user").val(), upass: xyz, procdate: $("#ProcessingDate").val(), custid: $('#CustomerId').val(), loglevel: "User" },
        success: function (data) {
            debugger;
            if (data == "true") {
                document.getElementById('login').style.display = 'none';
                if (flag == "Accept") {
                    var timestamp = document.getElementById("ExpiryTimeForAccept").value;
                    if (timestamp == "") {
                        alert("Please select Expiry Time.");

                        return false;
                    }
                    document.getElementById('login').style.display = 'none';
                    var conf = confirm("are you sure to mark all accept..?");
                    if (conf == true) {
                        MarkAllAcceptUpdate();
                    }
                    else {
                        return false;
                    }


                }
                else if (flag == "reject") {
                    var timestamp = document.getElementById("ExpiryTimeForReject").value;
                    var returncode = document.getElementById("returnreason").value;
                    var returnDiscription = document.getElementById("returnreason").value;

                    if (timestamp == "") {
                        alert("Please select Expiry Time.");

                        return false;
                    }

                    if (returncode === "") {
                        alert("Please select a reject reason.");

                        return false;
                    }
                    document.getElementById('login').style.display = 'none';
                    var conf = confirm("are you sure to mark all return..?");
                    if (conf == true) {
                        MarkAllRejectUpdate();
                    }
                    else {
                        return false;
                    }

                }

            }
            else {
                alert('UserName or Passowrd is wrong!!');
                return false;
            }

        }
    });


}
function MarkAllAcceptUpdate() {


    debugger;
    var timestamp = document.getElementById("ExpiryTimeForAccept").value;

    $.ajax({
        url: RootUrl + 'IWChiInterface/MarkAllAcceptUpdate',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: { 'TimeStamp': timestamp },
        success: function (data) {
            var alrt = JSON.parse(data);
            alert(alrt.message);


        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });

}


function MarkAllRejectUpdate() {


    debugger;
    var timestamp = document.getElementById("ExpiryTimeForReject").value;
    var returncode = document.getElementById("returnreason").value;
    var returnDiscription = document.getElementById("returnreason").value;

    if (timestamp == "") {
        alert("Please select Expiry Time.");

        return false;
    }

    if (returncode === "") {
        alert("Please select a reject reason.");

        return false;
    }

    var code = returncode.split(' - ')[0];
    var description = returnDiscription.split(' - ')[1] || '';

    // Check if the reason requires a description
    if (code === '88') {
        description = document.getElementById("enterReturnReason").value;

        if (description.trim() === "") {
            alert("Please enter Other Reason.");
            document.getElementById("enterReturnReason").focus();
            return;
        }
    }

    $.ajax({
        url: RootUrl + 'IWChiInterface/MarkAllRejectUpdate',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        async:false,
        data: { 'ItemExpiryTime': timestamp, 'returnCode': code, 'description': description },
        success: function (data) {
            var alrt = JSON.parse(data);
            alert(alrt.message);


        },
        error: function () {
            //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
        }
    });

}




function Show_Records_AcceptAll(value) {

    debugger;

    var timestamp = "";
    if (value == "acceptR") {
        timestamp = document.getElementById("ExpiryTimeForAccept").value;
    }
    else if (value == "rejectR") {
        timestamp = document.getElementById("ExpiryTimeForReject").value;
    }


    if (timestamp == "") {
        alert("Please Select Expiry Time");
        return false;
    }
    else {

        $.ajax({
            url: RootUrl + 'IWChiInterface/GetShowCountAccept',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: { 'TimeStamp': timestamp },
            success: function (data) {
                $('#tableData').html(data);

                // Show the modal only after a successful AJAX call
                document.getElementById('basicModal').style.display = 'block';


            },
            error: function () {
                //alert("Finacle is down, Kindly contact to support team !!! Please Login Again");
            }
        });

    }


}

function Show_Files_AcceptAll(value) {
    debugger;

    var ItemExpiryTime = "";

    document.getElementById('basicModal').style.display = 'block';
    if (value == "acceptF") {
        ItemExpiryTime = document.getElementById("ExpiryTimeForAccept").value;
    }
    else if (value == "rejectF") {
        ItemExpiryTime = document.getElementById("ExpiryTimeForReject").value;
    }


    if (ItemExpiryTime === "") {
        alert("Please select an expiry time.");

        return false;
    }
    else {

        $.ajax({
            url: RootUrl + 'IWChiInterface/GetShowAcceptFile',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: { 'TimeStamp': ItemExpiryTime },
            success: function (data) {
                console.log(data);

                $('#tableData').html(data);
            },
            error: function () {

            }
        });
        document.getElementById('basicModal').style.display = 'block';
    }



}

//reset fail api call 17-03-25
function ResetAPICall(id) {
    debugger;

    var buttontype = "";
    if (id == "1") { buttontype = "Casa Posting"; } else if (id == "2") { buttontype = "Return Charge Collection"; }


    var conf = confirm("Do you want to proceed with resetting " + buttontype + "?");

    if (conf == true) {

        $.ajax({
            url: RootUrl + 'IWChiInterface/ResetFailAPI',
            contentType: 'application/json; charset=utf-8',
            dataType: 'JSON',
            //type: 'POST',
            data: { ApiType: id },
            success: function (data) {
                debugger;
                console.log(data);
                // var obj = JSON.parse(data);
                if (data == "Success") {
                    alert(buttontype + " Reset Successfully!!!");
                }
                else {
                    alert("No Record Update");
                }


            },
            error: function (error) {
                debugger;
                alert("Error Occured==>" + error);
            }
        });


    }



}