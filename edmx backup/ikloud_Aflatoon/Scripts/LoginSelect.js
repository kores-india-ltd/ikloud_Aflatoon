$(document).ready(function () {

    $('#Org').change(function () {

        if ($("#Org").val() != "select" || $("#Org").val() != null) {

            $.ajax({
                url: RootUrl + 'Login/_SelectedCustomers',
                // type: 'Post',
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                data: { id: $("#Org").val() },
                //async: false,
                success: function (custlist) {

                    $("#CustBag").empty();

                    var temcustlist = JSON.parse(custlist);
                    $("#CustBag").append(
                    $('<option></option>').val(0).html("-----Select-----"));
                    $.each(temcustlist, function (i, CustBag) {
                        $("#CustBag").append(
                           $('<option></option>').val(CustBag.Id).html(CustBag.Name));
                    });
                    // document.getElementById("orgcust").style.display = "block";
                    document.getElementById("divcust").style.display = "";
                    document.getElementById("divprocdate").style.display = "none";
                    //$('#dialogEditUser').html(data);
                    //$('#dialogEditUser').dialog('open');
                }
            });

        }
        $("#go").prop("disabled", true);

    });
    $('#CustBag').change(function () {

        //alert("customer select");
        if ($("#CustBag").val() != "select" || $("#CustBag").val() != null) {
            var selectedOption = $("#CustBag option:selected").text();
            document.getElementById('CustName').value = selectedOption;

            $.ajax({
                url: RootUrl + 'Login/_SelectedProcDate',
                // type: 'Post',
                contentType: 'application/json; charset=utf-8',
                dataType: 'html',
                data: { id: $("#CustBag").val() },
                //async: false,
                success: function (data) {
                    // $('#divprocdate').html(data);
                    $("#ProcDate").empty();
                   // debugger;

                    if ($("#clrtype").val() == "Outward") {

                        $("#Domainselect").empty();
                        var dataarray = [];
                        var dataarrayFinal = [];
                        dataarray = data.split("procdate");
                        dataarrayFinal = dataarray[1].split("domlst");
                        var temcustlist = dataarrayFinal[0]; //data[0];
                        temcustlist = temcustlist.substring(2, temcustlist.length - 2);
                        // temcustlist = temcustlist + '"';

                        temcustlist = JSON.parse(temcustlist);

                        $("#ProcDate").append(
                        $('<option></option>').val(0).html("-----Select-----"));
                        $.each(temcustlist, function (i, ProcDate) {
                            $("#ProcDate").append(
                               $('<option></option>').val(ProcDate.CustomerId).html(ProcDate.ProcessingDate));
                        });
                        //---------------------Domain filling-------------
                        var temdomlist = dataarrayFinal[1];
                        temdomlist = temdomlist.substring(2, temdomlist.length - 1);

                        temdomlist = JSON.parse(temdomlist);

                        $("#Domainselect").append(
                       $('<option></option>').val(0).html("ALL"));
                        $.each(temdomlist, function (i, Domainselect) {
                            $("#Domainselect").append(
                               $('<option></option>').val(Domainselect.Id).html(Domainselect.Name));
                        });
                        document.getElementById('domval').value = $("#Domainselect option:selected").text();
                    }
                    else {
                        

                        var temcustlist = JSON.parse(data);
                        $("#ProcDate").append(
                        $('<option></option>').val(0).html("-----Select-----"));
                        $.each(temcustlist, function (i, ProcDate) {
                            $("#ProcDate").append(
                               $('<option></option>').val(ProcDate.CustomerId).html(ProcDate.ProcessingDate));
                        });
                    }


                }
            });

            if ($("#CustBag").val() == "0") {
                $("#go").prop("disabled", true);
                document.getElementById("divprocdate").style.display = "none";
                document.getElementById("divdomain").style.display = "none";
            }
            else {
                $("#go").prop("disabled", false);
                document.getElementById("divprocdate").style.display = "block";
                if ($("#clrtype").val() == "Outward") {
                    document.getElementById("divdomain").style.display = "block";
                }
                else {
                    document.getElementById("divdomain").style.display = "none";
                }

            }
        }
    });
    //
    $('#ProcDate').change(function () {
        //debugger;
        var selectedOption = $("#ProcDate option:selected").text();
        document.getElementById('procdateval').value = selectedOption;
    });

    $('#Domainselect').change(function () {
       
        
        var selectedOption = $("#Domainselect option:selected").val();
        document.getElementById('domval').value = selectedOption;
        //alert("domain selected" + document.getElementById('domval').value);
        var x = 0;
        
        //alert($("#clrtype").val());

        debugger;

        $.ajax({
            url: RootUrl + 'Login/_GetBranches',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: { id: $("#Domainselect").val() },
            success: function (data) {
                if ($("#clrtype").val() == "Outward")
                {
                    //alert("ok");
                    $("#Branchselect").empty()

                    x = 10;
                    var dataarrayFinal = [];
                    dataarrayFinal = data.split("result1");
                    var temcustlist = dataarrayFinal[0]; //data[0];
                    temcustlist = temcustlist.substring(2, temcustlist.length - 2);
                    // temcustlist = temcustlist + '"';

                    x = 11;

                    temcustlist = JSON.parse(temcustlist);
                    x = 1;
                   
                    //---------------------Branch filling-------------
                    var temdomlist = dataarrayFinal[1];
                    temdomlist = temdomlist.substring(2, temdomlist.length - 1);
                    x = 2;
                    temdomlist = JSON.parse(temdomlist);
                    x = 3;
                    $("#Branchselect").append(
                    $('<option></option>').val(0).html("ALL"));
                    $.each(temdomlist, function (i, Branchselect) {
                        $("#Branchselect").append(
                           $('<option></option>').val(Branchselect.Id).html(Branchselect.Name));
                    });

                    x = 4;
                    document.getElementById('brnchid').value = $("#Branchselect option:selected").text();
                    document.getElementById("divBrnch").style.display = "block";
                    x = 5;
                    
                }
            },
            error:
                function (xhr, textStatus, error) {
                    //alert("end of ajax" + x.toString());
                }

        });

        //alert("end of ajax" + x.toString());
    });

    //$("#cancel").click(function () {
    //    window.location = RootUrl + "Login/Logout?user=" + $("#lName").val();
    //});

    $("#go").click(function () {

        if ($("#ProcDate").val() == "0") {
            alert('Please select processing date!!!');
            return false;
        }


    });
});