$(document).ready(function () {
    // alert("hi");

   // $("#select").prop("disabled", true);

    var flag = $("#flghdId").val();

    if (flag == "L3") {
        //$("#AmtSelection option:last").prop("selected", true);
        $("#AmtSelection option").prop("disabled", true); // Disable all options
        $("#AmtSelection option:last").prop("disabled", false).prop("selected", true); // Enable and select only the last option
    }


    $("#select").click(function (e) {
        debugger;

        var isQueueSelected = $("input[name='queue']:checked").length > 0;
        var Flg = $("#flghdId").val();


        
        if (!isQueueSelected) {
            alert("Please select a Queue option");
            e.preventDefault(); 
            return false;
        }

        var selectedQueueValue = $("input[name='queue']:checked").val();
        var selectedAmtValue = $("#AmtSelection").val(); 
        var expirytime = $("#SessionExpiryTime").val(); 

        if (Flg == "L1") {
        

          // window.location.href = `/IWL1/Index?flag=${Flg}&queue=${selectedQueueValue}&AmtValue=${selectedAmtValue}&ExpiryTime=${expirytime}`;
           // window.location.href = `/IWL1/Index`;
        } else if (Flg == "L2") {
            window.location.href = "/IWL2/Index"; 
        }


    });










});


function GoToHomePage() {
    console.log("In home page");
    var url = RootUrl + "Home/IWIndex";
    window.location.href = url;
};

//window.addEventListener('beforeunload', function (event) {
//    // Custom logic before alert
//    debugger;
//    alert('You are about to leave the page!'); // Optional custom alert (won't stop navigation)

//    // Show the browser's default confirmation dialog
//    event.preventDefault(); // Required for most modern browsers
//    event.returnValue = ''; // Required for older browsers
//});