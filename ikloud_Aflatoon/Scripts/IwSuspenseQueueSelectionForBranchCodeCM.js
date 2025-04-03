$(document).ready(function () {

    //$("#select").prop("disabled", true);

    $.ajax({
        url: RootUrl + 'IwSuspenseQueueCM/SelectBranchCodes',
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
                $('<option></option>').val(0).html("-----All-----"));
            $.each(branchLists, function (i, BranchSelect) {
                $("#BranchSelect").append(
                    $('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCodeName));
                //$('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCode + " (" + BranchSelect.BranchName + ")"));
            });


        }
    });

    $('#BranchSelect').change(function () {

        EnableOrDisableSelect();
    });

    function EnableOrDisableSelect() {
        console.log("In");
        console.log($("#BranchSelect").val());
        //	 || $("#BranchSelect").val() !== 0
        if ($("#BranchSelect").val() !== "") {

            $("#select").prop("disabled", false);
        }
        else {
            $("#select").prop("disabled", true);
            console.log("select disabled true");
        }
    };

    function GoToHomePage() {
        console.log("In home page");
        var url = RootUrl + "Home/IWIndex";
        window.location.href = url;
    };
});

function GoToHomePage() {
    console.log("In home page");
    var url = RootUrl + "Home/IWIndex";
    window.location.href = url;
};

function GoToRefreshPage() {
    console.log("In home page");
    $.ajax({
        url: RootUrl + 'IwSuspenseQueueCM/SelectBranchCodes',
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
                $('<option></option>').val(0).html("-----All-----"));
            $.each(branchLists, function (i, BranchSelect) {
                $("#BranchSelect").append(
                    $('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCodeName));
                //$('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCode + " (" + BranchSelect.BranchName + ")"));
            });


        }
    });
};

function validateForm() {
    const checkerRadio = document.getElementById("radioChecker");
    const makerRadio = document.getElementById("radioMaker");
    const errorSpan = document.getElementById("radioError");

    if (!checkerRadio.checked && !makerRadio.checked) {
        errorSpan.style.display = "inline"; // Show the error message
        return false; // Prevent form submission
    }

    errorSpan.style.display = "none"; // Hide the error message
    return true; // Allow form submission
}