$(document).ready(function () {

    $("#btnView").click(function () {
        //var filetyp = $("#FileType").val();
        $("#progress").hide();
      //  debugger;
        var filetyp = $('select[name="FileType"] option:selected').text();
        //alert(filetyp);
        if (filetyp == "Select") {
            alert("Please select file type!");
            $('#FileType').focus();
            $("#rsltrec").hide();
            return false;
        }

        // var Clrtype = $('select[name="Sesntype"] option:selected').text();
        if ($("#ClearingType").val() == "Select" || $("#ClearingType").val() == "") {
            alert("Please select clearing type!");
            $('#ClearingType').focus();
            $("#rsltrec").hide();
            return false;
        }

    });
    $("#FileType").change(function () {
        if ($("#FileType").val() != "Select") {
            $("#btnView").removeAttr("disabled");
        }
        else {
            $("#btnView").attr("disabled", "disabled");
        }

    });

    $("#btnClose").click(function () {
        //alert('ok');
        window.location = RootUrl + 'Home/IWIndex?id=0';

    });

    //$("#btnDown").click(function () {
    //    $("#pass").hide();
    //    return true;

    //});
    //$("#btnReDown").click(function () {


    //    $("#progress").hide();

    //    var filetyp = $('select[name="FileType"] option:selected').text();
    //    if (filetyp == "Select") {
    //        alert("Please select file type!");
    //        $('#FileType').focus();
    //        $(".rsltrec").hide();
    //        return false;
    //    }
    //});


});

