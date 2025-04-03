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
        
        actulalockfield = $("#clickedradio").val();

        document.getElementById('loader').style.display = "";

        var oTable = document.getElementById('myTable');
        var rowLength = oTable.rows.length;

        var id = "";
        var rawDataId = "";
        var status = "";
        var userId = "";
        //loops through rows    
        for (i = 0; i < rowLength; i++) {

            if (i !== 0) {
                var oCells = oTable.rows.item(i).cells;
                var cellLength = oCells.length;
                //loops through each cell in current row
                for (var j = 0; j < cellLength; j++) {

                    if (j === 0) {
                        id = oCells.item(j).innerHTML;
                    }
                    else if (j === 1) {
                        rawDataId = oCells.item(j).innerHTML;
                    }
                    else if (j === 2) {
                        status = oCells.item(j).innerHTML;
                    }
                    else if (j === 3) {
                        userId = oCells.item(j).innerHTML;
                    }
                    
                }

                console.log(id);
                console.log(rawDataId);
                console.log(status);
                console.log(userId);

                $.ajax({
                    url: RootUrl + 'UnlockRecords/clearLocks',
                    data: { fieldname: actulalockfield, Id: id, RawDataId: rawDataId, Status: status, UserId: userId },
                        dataType: 'html',
                        type: 'POST',
                        success: function (data) {
                        $('#ShowDetails').empty();
                        //$('#ShowDetails').html(data);
                        //alert('Records Unlocked Successfully!!');
                        //document.getElementById('loader').style.display = "none";
                    },
                    error: function () {
                        alert("Error while unlocking records!!");
                    }
                });
            }
            
        }
        alert('Records Unlocked Successfully!!');
        document.getElementById('loader').style.display = "none";
        

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