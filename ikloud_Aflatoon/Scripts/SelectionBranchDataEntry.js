$(document).ready(function () {
    
    $("#select").prop("disabled", true);
    debugger;
    $.ajax({
        url: RootUrl + 'OWBranchLevelDataEntry/SelectScanningNode',
        // type: 'Post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        //async: false,
        success: function (scanningNodeLists) {
            console.log("In ScanningNode");
            $("#ScanningNodeSelect").empty();

            var scanningLists = JSON.parse(scanningNodeLists);
            console.log(scanningLists.length);
            if (scanningLists.length == 1) {
                $.each(scanningLists, function (i, ScanningNodeSelect) {
                    $("#ScanningNodeSelect").append(
                        $('<option></option>').val(ScanningNodeSelect.Id).html(ScanningNodeSelect.Id));
                });
                LoadBranchCodes();
            }
            else {
                $("#ScanningNodeSelect").append(
                    $('<option></option>').val(0).html("-----Select-----"));
                $.each(scanningLists, function (i, ScanningNodeSelect) {
                    $("#ScanningNodeSelect").append(
                        $('<option></option>').val(ScanningNodeSelect.Id).html(ScanningNodeSelect.Id));
                });
            }
            

            //document.getElementById("divBatch").style.display = "none";

        }
    });

    function LoadBranchCodes() {
        $.ajax({
            url: RootUrl + 'OWBranchLevelDataEntry/SelectBranchCodes',
            // type: 'Post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: {},
            //async: false,
            success: function (branchCodeLists) {
                console.log("In BranchCode");
                $("#BranchSelect").empty();

                var branchLists = JSON.parse(branchCodeLists);
                console.log(branchLists);
                $("#BranchSelect").append(
                    $('<option></option>').val(0).html("-----Select-----"));
                $.each(branchLists, function (i, BranchSelect) {
                    $("#BranchSelect").append(
                        //$('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCodeName));
                        $('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCode + " (" + BranchSelect.BranchName + ")"));
                });

                //document.getElementById("divBatch").style.display = "none";

            }
        });
    }

    $("#btnCloseBatch").click(function () {

        //document.getElementById("rejectDiv").style.display = "none";
        //=============Update status of branch data entry status in BatchMaster table as 'A' Available from 'L'

        $.ajax({
            url: RootUrl + 'OWBranchLevelDataEntry/Update_Close_StatusForBatch',
            // type: 'Post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: {},
            //async: false,
            success: function (data) {

            }

        });
        console.log("In Close button");
        var url = RootUrl + "OWBranchLevelDataEntry/SelectionForBranchDataEntry";
        window.location.href = url;
    });

    $('#ScanningNodeSelect').change(function () {

        if ($("#ScanningNodeSelect").val() !== "select" || $("#ScanningNodeSelect").val() !== null) {

            $.ajax({
                url: RootUrl + 'OWBranchLevelDataEntry/SelectBranchCodes',
                // type: 'Post',
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                data: {},
                //async: false,
                success: function (branchCodeLists) {
                    console.log("In BranchCode");
                    $("#BranchSelect").empty();

                    var branchLists = JSON.parse(branchCodeLists);
                    console.log(branchLists);
                    $("#BranchSelect").append(
                        $('<option></option>').val(0).html("-----Select-----"));
                    $.each(branchLists, function (i, BranchSelect) {
                        $("#BranchSelect").append(
                            //$('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCodeName));
                        $('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCode + " (" + BranchSelect.BranchName + ")"));
                    });

                    //document.getElementById("divBatch").style.display = "none";

                }
            });
        }
        EnableOrDisableSelect();
    });

    $('#BranchSelect').change(function () {

        if ($("#BranchSelect").val() !== "select" || $("#BranchSelect").val() !== null) {

            $.ajax({
                url: RootUrl + 'OWBranchLevelDataEntry/SelectScanningTypes',
                // type: 'Post',
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                data: { id: $("#BranchSelect").val(), scanningNodeId: $("#ScanningNodeSelect").val() },
                //async: false,
                success: function (scanningTypeLists) {
                    console.log("In ScanningType");
                    $("#SelectScanningType").empty();

                    var scanningLists = JSON.parse(scanningTypeLists);
                    console.log(scanningLists);
                    $("#SelectScanningType").append(
                        $('<option></option>').val(0).html("-----Select-----"));
                    $.each(scanningLists, function (i, SelectScanningType) {
                        $("#SelectScanningType").append(
                            $('<option></option>').val(SelectScanningType.ID).html(SelectScanningType.Description));
                    });

                    //document.getElementById("divBatch").style.display = "none";

                }
            });
        }
        EnableOrDisableSelect();
    });

    $('#SelectScanningType').change(function () {

        if ($("#BranchSelect").val() !== "select" || $("#BranchSelect").val() !== null || $("#SelectScanningType").val() !== "select" || $("#SelectScanningType").val() !== null) {
            console.log($("#SelectScanningType").val());
            var sendData = {
                id: $("#BranchSelect").val(),
                scanningTypeId: $("#SelectScanningType").val()
            };

            $.ajax({
                url: RootUrl + 'OWBranchLevelDataEntry/SelectBatchNos',
                //type: 'Post',
                contentType: 'application/json; charset=utf-8',
                dataType: 'JSON',
                data: { id: $("#BranchSelect").val(), scanningTypeId: $("#SelectScanningType").val(), scanningNodeId: $("#ScanningNodeSelect").val() },
                //data: JSON.stringify({ dat: sendData }),
                //async: false,
                success: function (batchnoslist) {
                    console.log(batchnoslist);
                    $("#BatchSelect").empty();

                    //var batchLists = JSON.parse(batchnoslist);
                    var batchLists = batchnoslist;
                    $("#BatchSelect").append(
                        $('<option></option>').val(0).html("-----Select-----"));
                    $.each(batchLists, function (i, BatchSelect) {
                        $("#BatchSelect").append(
                            $('<option></option>').val(BatchSelect.Id).html(BatchSelect.BatchNo));
                    });

                }
            });
            $("#select").prop("disabled", false);
        }
        else {
            $("#select").prop("disabled", true);
        }
        EnableOrDisableSelect();
    });

    function EnableOrDisableSelect() {
        console.log("In");
        console.log($("#BranchSelect").val());
        console.log($("#SelectScanningType").val());
        console.log($("#BatchSelect").val());
        if ($("#BranchSelect").val() !== "" || $("#BranchSelect").val() !== 0) {
            if ($("#SelectScanningType").val() !== "" && $("#SelectScanningType").val() !== 0) {
                if ($("#BatchSelect").val() !== "" && $("#BatchSelect").val() !== 0) {
                    $("#select").prop("disabled", false);
                    console.log("BatchSelect disabled false");
                }
                else {
                    $("#select").prop("disabled", true);
                    console.log("BatchSelect disabled true");
                }
            }
            else {
                $("#select").prop("disabled", true);
                console.log("SelectScanningType disabled true");
            }
            
        }
        else {
            $("#select").prop("disabled", true);
            console.log("select disabled true");
        }
    };

    $('#BatchSelect').change(function () {
        console.log($("#BatchSelect").val());
        if ($("#BatchSelect").val() !== "" && $("#BatchSelect").val() !== 0 && $("#BatchSelect").val() !== "0") {
            $("#select").prop("disabled", false);
            console.log("BatchSelect disabled false");
        }
        else {
            $("#select").prop("disabled", true);
            console.log("BatchSelect disabled true");
        }
    });

    function GoToHomePage() {
        console.log("In home page");
        var url = RootUrl + "Home/IWIndex";
        window.location.href = url;
    }

    //$("#select").click(function () {
    //    console.log("Hiii");
    //    if ($("#BranchSelect").val() == "0") {
    //        alert('Please select Branch!!!');
    //        return false;
    //    }
    //    if ($("#SelectScanningType").val() == "0") {
    //        alert('Please select Scanning Type!!!');
    //        return false;
    //    }
    //    if ($("#BatchSelect").val() == "0") {
    //        alert('Please select Batch!!!');
    //        return false;
    //    }

    //    //$.ajax({
    //    //    url: RootUrl + 'OWBranchLevelDataEntry/BranchDataEntry',
    //    //    type: 'Post',
    //    //    contentType: 'application/json; charset=utf-8',
    //    //    dataType: 'html',
    //    //    data: { branchCode: $("#BranchSelect").val(), batchNo: $("#BatchSelect").val()},
    //    //    //async: false,
    //    //    success: function (data) {
    //    //        debugger;
    //    //        console.log(data);
    //    //        window.location = "/OWBranchLevelDataEntry/BranchDataEntry";

    //    //    }
    //    //});

    //});

});

function GoToHomePage() {
    console.log("In home page");
    var url = RootUrl + "Home/IWIndex";
    window.location.href = url;
}



