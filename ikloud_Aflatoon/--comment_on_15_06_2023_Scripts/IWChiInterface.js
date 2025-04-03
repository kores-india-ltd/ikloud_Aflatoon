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

function LoadPXF() {
    $.ajax({
        url: RootUrl + 'IWChiInterface/LoadPxf',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 1 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            //document.getElementById('LoadPXF').disabled = true;
            document.getElementById('LoadPXF').style.backgroundColor = 'gray';
            DisableAllButtons();
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
            alert(data.Result1);
            //document.getElementById('ExtractDataPXF').disabled = true;
            document.getElementById('ExtractDataPXF').style.backgroundColor = 'gray';
            DisableAllButtons();
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
            alert(data.Result1);
            //document.getElementById('GenerateRMI').disabled = true;
            document.getElementById('GenerateRMI').style.backgroundColor = 'gray';
            DisableAllButtons();
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