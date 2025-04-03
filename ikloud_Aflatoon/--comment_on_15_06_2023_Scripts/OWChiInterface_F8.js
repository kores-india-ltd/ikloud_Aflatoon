function Close() {
    //console.log("In home page");
    var url = RootUrl + "Home/IWIndex";
    window.location.href = url;
}

function CheckStatus() {
    DisableAllButtons();
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/CheckDEM_Session',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: {},
        success: function (data) {
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
            
        }
    });
}

function TerminateRequest() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/TerminateRequest',
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

}

function CreateCXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/CreateCXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 1 },
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            //document.getElementById('CreateCXF').disabled = true;
            document.getElementById('CreateCXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function UploadCXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/UploadCXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 2 },
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            //document.getElementById('UploadCXF').disabled = true;
            document.getElementById('UploadCXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function DownloadRES_OACK_CXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/DownloadRES_OACK_CXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 3 },
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            //document.getElementById('DownloadRES_OACK_CXF').disabled = true;
            document.getElementById('DownloadRES_OACK_CXF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function Read_PPS_File() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/Read_PPS_File',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 10 },
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            //document.getElementById('Read_PPS_File').disabled = true;
            document.getElementById('Read_PPS_File').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function CreateCIIF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/CreateCIIF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 11 },
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            //document.getElementById('CreateCIIF').disabled = true;
            document.getElementById('CreateCIIF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function UploadCIIF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/UploadCIIF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 12 },
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            //document.getElementById('UploadCIIF').disabled = true;
            document.getElementById('UploadCIIF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function DownloadRES() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/DownloadRES',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 13 },
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            //document.getElementById('DownloadRES').disabled = true;
            document.getElementById('DownloadRES').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}

function ReadBRF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/ReadBRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 20 },
        success: function (data) {
            console.log(data);
            //alert(data.Result1);
            //document.getElementById('ReadBRF').disabled = true;
            document.getElementById('ReadBRF').style.backgroundColor = 'gray';
            DisableAllButtons();
        }
    });
}


function Show_Files_CXF() {
    document.getElementById('basicModal').style.display = 'block';
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/Show_Files_CXF',
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
        url: RootUrl + 'OWChiInterface_F8/Show_Files_CIIF',
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
        url: RootUrl + 'OWChiInterface_F8/Show_Files_BRF',
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
        url: RootUrl + 'OWChiInterface_F8/Show_Records_CXF',
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
        url: RootUrl + 'OWChiInterface_F8/Show_Records_BRF',
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
        url: RootUrl + 'OWChiInterface_F8/Show_Records_PPS',
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

function Create_IB_CXF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/Create_IB_CXF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 25 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            document.getElementById('Create_IB_CXF').disabled = true;
        }
    });
}

function Read_IB_RRF() {
    $.ajax({
        url: RootUrl + 'OWChiInterface_F8/Read_IB_RRF',
        contentType: 'application/json; charset=utf-8',
        dataType: 'JSON',
        data: { Id: 26 },
        success: function (data) {
            console.log(data);
            alert(data.Result1);
            document.getElementById('Read_IB_RRF').disabled = true;
        }
    });
}



