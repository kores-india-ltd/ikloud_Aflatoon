function Close() {
    //console.log("In home page");
    var url = RootUrl + "Home/IWIndex";
    window.location.href = url;
}

function CheckStatus() {
    debugger;
    /*DisableAllButtons();*/
    $.ajax({
        url: RootUrl + 'OWChiInterface/CheckDEM_Session',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: {},
        success: function (data) {
            debugger;
            console.log(data);
            //alert(data.Result1);
            if (data.IS_Create_CXF_Status === "2" || data.IS_Create_CXF_Status === "4") {
                alert("Create CXF Task Completed Successfully!!!");
                EnableAllButtons();
                //document.getElementById('CreateCXF').disabled = false;
                //document.getElementById('UploadCXF').disabled = false;
                //document.getElementById('DownloadRES_OACK_CXF').disabled = false;
                //document.getElementById('Show_Records_CXF').disabled = false;
                //document.getElementById('Show_Files_CXF').disabled = false;
            }
            if (data.IS_Upload_CXF_Status === "2" || data.IS_Upload_CXF_Status === "4") {
                alert("Upload CXF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Download_Res_Oack_Status === "2" || data.IS_Download_Res_Oack_Status === "4") {
                alert("Download RES OACK Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Read_PPS_File_Status === "2" || data.IS_Read_PPS_File_Status === "4") {
                alert("Read PPS File Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_PPS_Create_CIIF_Status === "2" || data.IS_PPS_Create_CIIF_Status === "4") {
                alert("PPS Create CIIF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_PPS_Upload_CIIF_Status === "2" || data.IS_PPS_Upload_CIIF_Status === "4") {
                alert("PPS Upload CIIF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_PPS_Download_Res_Status === "2" || data.IS_PPS_Download_Res_Status === "4") {
                alert("PPS Download RES Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Read_BRF_Status === "2" || data.IS_Read_BRF_Status === "4") {
                alert("Read BRF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Load_Vendor_File_Status === "2" || data.IS_Load_Vendor_File_Status === "4") {
                alert("Load Vendor File Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Read_LVB_CXF_Status === "2" || data.IS_Read_LVB_CXF_Status === "4") {
                alert("Read LVB CXF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Read_LVB_BRF_Status === "2" || data.IS_Read_LVB_BRF_Status === "4") {
                alert("Read LVB BRF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Read_LVB_PPS_Status === "2" || data.IS_Read_LVB_PPS_Status === "4") {
                alert("Read LVB PPS Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Create_IB_CXF_Status === "2" || data.IS_Create_IB_CXF_Status === "4") {
                alert("Create IB CXF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Read_IB_RRF_Status === "2" || data.IS_Read_IB_RRF_Status === "4") {
                alert("Read IB RRF Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Outward_Extract_Status === "2" || data.IS_Outward_Extract_Status === "4") {
                alert("Outward Extract Task Completed Successfully!!!");
                EnableAllButtons();
            }
            if (data.IS_Return_Extract_Status === "2" || data.IS_Return_Extract_Status === "4") {
                alert("Return Extract Task Completed Successfully!!!");
                EnableAllButtons();
            }
        }
    });

    DisableAllButtons();
}

function TerminateRequest() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/TerminateRequest',
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
    document.getElementById('CreateCXF').disabled = true;
    document.getElementById('UploadCXF').disabled = true;
    document.getElementById('DownloadRES_OACK_CXF').disabled = true;
    //document.getElementById('Show_Records_CXF').disabled = true;
    //document.getElementById('Show_Files_CXF').disabled = true;

    document.getElementById('Read_PPS_File').disabled = true;
    document.getElementById('CreateCIIF').disabled = true;
    document.getElementById('UploadCIIF').disabled = true;
    document.getElementById('DownloadRES').disabled = true;
    //document.getElementById('Show_Records_CIIF').disabled = true;
    //document.getElementById('Show_Files_CIIF').disabled = true;

    document.getElementById('ReadBRF').disabled = true;
    //document.getElementById('Show_Records_BRF').disabled = true;
    //document.getElementById('Show_Files_BRF').disabled = true;

    document.getElementById('ReadVendorFile').disabled = true;
    //document.getElementById('Show_Records_VendorFile').disabled = true;
    //document.getElementById('Show_Files_VendorFile').disabled = true;

    document.getElementById('Read_LVB_CXF').disabled = true;
    //document.getElementById('Show_Records_LVB_CXF').disabled = true;
    //document.getElementById('Show_Files_LVB_CXF').disabled = true;

    document.getElementById('Read_LVB_BRF').disabled = true;
    //document.getElementById('Show_Records_LVB_BRF').disabled = true;
    //document.getElementById('Show_Files_LVB_BRF').disabled = true;

    document.getElementById('Read_LVB_PPS').disabled = true;

    document.getElementById('Create_IB_CXF').disabled = true;
    document.getElementById('Read_IB_RRF').disabled = true;

    document.getElementById('OutwardExtract').disabled = true;
    document.getElementById('ReturnExtract').disabled = true;
}

function EnableAllButtons() {
    document.getElementById('CreateCXF').disabled = false;
    document.getElementById('UploadCXF').disabled = false;
    document.getElementById('DownloadRES_OACK_CXF').disabled = false;
    //document.getElementById('Show_Records_CXF').disabled = false;
    //document.getElementById('Show_Files_CXF').disabled = false;

    document.getElementById('Read_PPS_File').disabled = false;
    document.getElementById('CreateCIIF').disabled = false;
    document.getElementById('UploadCIIF').disabled = false;
    document.getElementById('DownloadRES').disabled = false;
    //document.getElementById('Show_Records_CIIF').disabled = false;
    //document.getElementById('Show_Files_CIIF').disabled = false;

    document.getElementById('ReadBRF').disabled = false;
    //document.getElementById('Show_Records_BRF').disabled = false;
    //document.getElementById('Show_Files_BRF').disabled = false;

    document.getElementById('ReadVendorFile').disabled = false;
    //document.getElementById('Show_Records_VendorFile').disabled = false;
    //document.getElementById('Show_Files_VendorFile').disabled = false;

    document.getElementById('Read_LVB_CXF').disabled = false;
    //document.getElementById('Show_Records_LVB_CXF').disabled = false;
    //document.getElementById('Show_Files_LVB_CXF').disabled = false;

    document.getElementById('Read_LVB_BRF').disabled = false;
    //document.getElementById('Show_Records_LVB_BRF').disabled = false;
    //document.getElementById('Show_Files_LVB_BRF').disabled = false;

    document.getElementById('Read_LVB_PPS').disabled = false;

    document.getElementById('Create_IB_CXF').disabled = false;
    document.getElementById('Read_IB_RRF').disabled = false;

    document.getElementById('OutwardExtract').disabled = false;
    document.getElementById('ReturnExtract').disabled = false;
}

function CreateCXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/CreateCXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 1 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('CreateCXF').disabled = true;
            //document.getElementById('UploadCXF').disabled = true;
            //document.getElementById('DownloadRES_OACK_CXF').disabled = true;
            //document.getElementById('Show_Records_CXF').disabled = true;
            //document.getElementById('Show_Files_CXF').disabled = true;
            document.getElementById('CreateCXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function UploadCXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/UploadCXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 2 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('UploadCXF').disabled = true;
            //document.getElementById('CreateCXF').disabled = true;
            //document.getElementById('DownloadRES_OACK_CXF').disabled = true;
            //document.getElementById('Show_Records_CXF').disabled = true;
            //document.getElementById('Show_Files_CXF').disabled = true;
            document.getElementById('UploadCXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function DownloadRES_OACK_CXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/DownloadRES_OACK_CXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 3 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('DownloadRES_OACK_CXF').disabled = true;
            document.getElementById('DownloadRES_OACK_CXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function Read_PPS_File() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/Read_PPS_File',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 10 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('Read_PPS_File').disabled = true;
            document.getElementById('Read_PPS_File').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function CreateCIIF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/CreateCIIF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 11 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('CreateCIIF').disabled = true;
            document.getElementById('CreateCIIF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function UploadCIIF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/UploadCIIF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 12 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('UploadCIIF').disabled = true;
            document.getElementById('UploadCIIF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function DownloadRES() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/DownloadRES',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 13 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('DownloadRES').disabled = true;
            document.getElementById('DownloadRES').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function ReadBRF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/ReadBRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 20 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('ReadBRF').disabled = true;
            document.getElementById('ReadBRF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function ReadVendorFile() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/ReadVendorFile',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 23 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('ReadVendorFile').disabled = true;
            document.getElementById('ReadVendorFile').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function Read_LVB_BRF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/Read_LVB_BRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 21 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('Read_LVB_BRF').disabled = true;
            document.getElementById('Read_LVB_BRF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function Read_LVB_CXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/Read_LVB_CXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 24 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('Read_LVB_CXF').disabled = true;
            document.getElementById('Read_LVB_CXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function Read_LVB_PPS() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/Read_LVB_PPS',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 27 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('Read_LVB_CXF').disabled = true;
            document.getElementById('Read_LVB_PPS').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function Create_IB_CXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/Create_IB_CXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 25 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('Create_IB_CXF').disabled = true;
            document.getElementById('Create_IB_CXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function Read_IB_RRF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/Read_IB_RRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 26 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('Read_IB_RRF').disabled = true;
            document.getElementById('Read_IB_RRF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function OutwardExtract() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/OutwardExtract',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 30 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('OutwardExtract').disabled = true;
            document.getElementById('OutwardExtract').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function ReturnExtract() {
    $.ajax({
        url: RootUrl + 'OWChiInterface/ReturnExtract',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 31 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('ReturnExtract').disabled = true;
            document.getElementById('ReturnExtract').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

//============== For Show Files ==========================

function Show_Files_CXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Files_CXF',
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

function Show_Files_CIIF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Files_CIIF',
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

function Show_Files_VendorFile() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Files_VendorFile',
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

function Show_Files_LVB_BRF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Files_LVB_BRF',
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

function Show_Files_LVB_CXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Files_LVB_CXF',
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

function Show_Files_BRF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Files_BRF',
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

//============== For Record Count ==========================

function Show_Records_CXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Records_CXF',
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

function Show_Records_VendorFile() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Records_VendorFile',
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

function Show_Records_BRF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Records_BRF',
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

function Show_Records_PPS() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Records_PPS',
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

function Show_Records_LVB_BRF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Records_LVB_BRF',
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

function Show_Records_LVB_CXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface/Show_Records_LVB_CXF',
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




