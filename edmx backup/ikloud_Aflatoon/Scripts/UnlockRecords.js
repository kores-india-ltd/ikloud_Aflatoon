$(document).ready(function () {

    //----------------showlockRcd-----------------

    $("#showlockRcd").click(function () {
        var defiled;
        var modelname = $('input[name=choosrdio]:checked').val();
        if ($("#clrtype").val() == "Outward" && modelname == "DataEntry") {
            defiled = $("#defiled").val();
        }
        else if ($("#clrtype").val() == "Inward" && modelname == "DataEntry") {

            defiled = $("#IWdefiled").val();
        }
        document.getElementById('loader').style.display = "";

        $.ajax({
            url: RootUrl + 'UnlockRecords/unlockData',
            data: { modename: modelname, fieldname: defiled },
            dataType: 'html',
            type: 'POST',
            success: function (data) {
                $('#ShowDetails').empty();
                $('#ShowDetails').html(data);
                document.getElementById('loader').style.display = "none";
            }
        });

    });
    //----------------Clear Locks--------------
    $("#Unlockrcd").click(function () {
        var actulalockfield;
        //var modelname = $('input[name=choosrdio]:checked').val();
        //if ($("#clrtype").val() == "Outward" && modelname == "DataEntry") {
        //    defiled = $("#defiled").val();
        //}
        //else if ($("#clrtype").val() == "Inward" && modelname == "DataEntry") {

        //    defiled = $("#IWdefiled").val();
        //}
        actulalockfield = $("#clickedradio").val();

        document.getElementById('loader').style.display = "";

        $.ajax({
            url: RootUrl + 'UnlockRecords/clearLocks',
            data: { fieldname: actulalockfield },
            dataType: 'html',
            type: 'POST',
            success: function (data) {
                $('#ShowDetails').empty();
                //$('#ShowDetails').html(data);
                alert('Records Unlocked Successfully!!');
                document.getElementById('loader').style.display = "none";
            },
            error: function () {
                alert("Error while unlocking records!!");
            }
        });

    });
    ////--------------Close Button Called----------------
    $("#btncls").click(function () {
        window.location = RootUrl + 'Home/IWIndex';
    });
});

function getRadioval() {

    //-------------Radio Button---------------
    //   $("#btnView").attr("disabled", "disabled");
    $("#showlockRcd").removeAttr("disabled");
    $("#Unlockrcd").attr("disabled", "disabled");
    var radioval = $('input[name=choosrdio]:checked').val();

    if (radioval == "DataEntry") {
        if ($("#clrtype").val() == "Outward") {

            document.getElementById('deselect').style.display = "";

        }
        else {

            document.getElementById('iwdeselect').style.display = "";


        }
        // document.getElementById('ShowDetails').style.display = " ";
    }
    else {
        document.getElementById('deselect').style.display = "none";
        // document.getElementById('ShowDetails').style.display = " ";
    }
}