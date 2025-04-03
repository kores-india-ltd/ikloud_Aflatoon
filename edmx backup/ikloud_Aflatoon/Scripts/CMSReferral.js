var FImg;
var BImg;
var btn;
var blnSupp;
var blnClientCode = false;
$(document).ready(function () {
    document.getElementById('ClientCode').focus();
    
   // debugger;
    //alert('call');

    //$('input[type = "text"]').keydown(function () {
    //    var txt = $(this).attr('id');
       
    //    if (event.keyCode == 13 && txt == "ClientCode") {
    //        alert(txt);
    //        var tempclnt = document.getElementById('oldClient').value;
    //        if (tempclnt != $("#ClientCode").val()) {

    //            document.getElementById('oldClient').value = $("#ClientCode").val();
    //            if ($("#ClientCode").val() != "") {
    //                clientdtls();
    //            }
    //            else {
    //                alert('Please enter Client Code !!');
    //                $("#ClientCode").focus();
    //                return false;
    //            }
    //        }
    //    }


    //});

    //clientdtls = function () {

    //    if ($("#ClientCode").val() != "") {           
    //        $.ajax({
    //            url: RootUrl + 'CMSClientCodeMissing/GetClientDlts',             
    //            dataType: 'html',
    //            data: { ac: $("#ClientCode").val() },
    //            success: function (data) {
    //              alert(data);
    //                $('#clientdetails').html(data);
    //               // document.getElementById('gclientdetails').innerHTML = data
                    
    //            }
    //        });
    //    }
    //    else {
    //        $('#clientdetails').empty();
    //        alert('Please enter Client Code !!');
    //        $("#ClientCode").focus();
    //        return false;
    //    }

    //}


    $("#ClientCode").keyup(function (event) {
       // alert(event.keyCode);
        
            if (event.keyCode == 8 ) {
               // alert(event.keyCode);
                $('#PayeeNameValidation').val('');
                $('#txtpayee').val('');
                $('#SubClntCode').val('');
                // $('#lblclientdetails').empty('');
                $('#clientdetails').empty('');

                //  document.getElementById('clientdetails').style.display = 'none';
                document.getElementById('SubClntID').style.display = 'none';
            }
           
       
        
    });

    // $("#btnShowChq").click(function () {
    $('input[type="button"]').click(function () {
        btn = $(this).attr('id');
        //alert(btn);

        if (btn == "btnShowChq" || btn == "btnSupDoc") {
            var url = RootUrl + "CMSClientCodeMissing/ShowCMSChq";
            $.ajax({
                url: url,
                type: "Post",
                //dataType: "html",
                dataType: "json",
                data: { CustomerId: $('#CustomerId').val(), DomainId: $('#DomainId').val(), ScanningNodeId: $('#ScanningNodeId').val(), SlipNo: $('#SlipNo').val(), BatchNo: $('#BatchNo').val(), BtnCicked: btn },
                success: function (ChqImgPath) {
                    //alert(ChqImgPath);
                    if (ChqImgPath != true) {

                        FImg = ChqImgPath.FrontGrayImagePath.replace('"', "").replace('"', "");
                        BImg = ChqImgPath.BackGreyImagePath.replace('"', "").replace('"', "");

                        if (btn == "btnShowChq") {
                            if ($("#btnShowChq").val() == "Show Cheque") {

                                if ($('input:radio[name=ImgToggle]:checked').val() == "Front") {
                                    document.getElementById('myimg').src = ChqImgPath.FrontGrayImagePath.replace('"', "").replace('"', "");

                                }
                                else {
                                    document.getElementById('myimg').src = ChqImgPath.BackGreyImagePath.replace('"', "").replace('"', "");
                                }
                                //document.getElementById('myimg').src = ChqImgPath.FrontGrayImagePath.replace('"', "").replace('"', "");
                                $("#btnShowChq").prop('value', 'Show Slip');
                            }
                            else {


                                if ($('input:radio[name=ImgToggle]:checked').val() == "Front") {
                                    document.getElementById('myimg').src = $("#FrontGrayImagePath").val().replace('"', "").replace('"', "");

                                }
                                else {
                                    document.getElementById('myimg').src = $("#BackGreyImagePath").val().replace('"', "").replace('"', "");
                                }
                                // document.getElementById('myimg').src = $("#BackGreyImagePath").val().replace('"', "").replace('"', "");
                                $("#btnShowChq").prop('value', 'Show Cheque');

                            }
                        }
                        else {
                            // alert(FImg);

                            document.getElementById('myimg').src = FImg;
                        }
                    }
                    else {
                        blnSupp = ChqImgPath;
                        alert('Image not found!');

                    }
                }
            });
        }

    });

    $('#btnClose').click(function () {

        var url = RootUrl + "CMSClientCodeMissing/ClientCodeSelection?LockID=" + $('#ID').val() + "&LockModule=L1"
        window.location.href = url;
    });

    //$('#btnCloseClnt').click(function () {
    //    alert('OK');
    //    var url = RootUrl + "Home/IWIndex";
    //    window.location.href = url;
    //});

    $('#btnReject').click(function () {
        document.getElementById('DivRejectDesc').style.display = 'block';
        if ($('#RejectDesc').val().length == 0) {
            $('#RejectDesc').focus();
            alert('Please enter Reject Reason Description!');
            return false;
        }
        document.getElementById('rejectreasondescrpsn').focus();

    });

    $('#btnAccept').click(function () {
        debugger;
        //if (blnClientCode == false)
        //{
        //    alert('Please Click on Get Client Name button');
        //    $('#ClientCode').focus();
        //    return false;
        //}
        alert($('#PayeeNameValidation').val());

        if ($('#ClientCode').val().length == 0 ) {
            $('#ClientCode').focus();
            alert('Please enter Client Code!');
            document.getElementById('ClientCode').focus();
            return false;
        }

       
        if ($('#SubClntCode').val().length == 0 && document.getElementById('SubClntReq').value == 'Y') {
            document.getElementById('clientdetails').style.display = 'block';
            document.getElementById('SubClntID').style.display = 'block';
            $('#SubClntCode').focus();
            alert('Please enter Sub Client Code!');
            document.getElementById('SubClntCode').focus();
            return false;
        }

       
        if ($('#PayeeNameValidation').val().length == 0) {
            $('#SubClntCode').focus();
            alert('Please Click on Get Client Name button to fetch payeen name');
            document.getElementById('SubClntCode').focus();
            return false;
        }
       
        if ($('#PayeeNameValidation').val() == 'Customer is Suspended') {
            $('#ClientCode').focus();
            alert('You can not accept this slip beacuse customer is suspended');
          
            return false;
        }

      

    });

    //$("#ClientCode").focus(function () {
    //   // alert("Handler for .focus() called.");
    //});

    $("#SubClntCode").mouseup(function (e) {
        $('#PayeeNameValidation').val('');
        $('#txtpayee').val('');
        document.getElementById('clientdetails').style.display = 'none';
    });

    $("#ClientCode").mouseup(function (e) {
       
        $('#PayeeNameValidation').val('');
        $('#txtpayee').val('');
        $('#SubClntCode').val('');
       // $('#lblclientdetails').empty('');
        $('#clientdetails').empty('');
       
      //  document.getElementById('clientdetails').style.display = 'none';
        document.getElementById('SubClntID').style.display = 'none';
    });

  

    $('#btnClientCode').click(function () {
      
        document.getElementById('clientdetails').style.display = 'block';
        document.getElementById('SubClntID').style.display = 'block';
        if ($('#ClientCode').val().length == 0) {
            $('#ClientCode').focus();
            alert('Please enter Client Code!');
            document.getElementById('ClientCode').focus();
           
            return false;
        }
       
  
        if ($('#SubClntCode').val().length == 0 && document.getElementById('SubClntReq').value=='Y') {
            $('#SubClntCode').focus();
            alert('Please enter Sub Client Code!');
            document.getElementById('SubClntCode').focus();
            blnClientCode = false;
            return false;
        }
      
        blnClientCode = true;
    });


    $("#frontimg").click(function () {
        //alert(FImg);
        if (btn == "btnSupDoc" && blnSupp != true) {
            document.getElementById('myimg').src = FImg;


        }
        else {
            if ($("#btnShowChq").val() == "Show Cheque") {

                document.getElementById('myimg').src = $("#FrontGrayImagePath").val().replace('"', "").replace('"', "");
            }
            else {

                document.getElementById('myimg').src = FImg;

            }
        }

    });

    $("#backimg").click(function () {
        if (btn == "btnSupDoc" && blnSupp != true) {
            document.getElementById('myimg').src = BImg;

        }
        else {
            if ($("#btnShowChq").val() == "Show Cheque") {


                document.getElementById('myimg').src = $("#BackGreyImagePath").val().replace('"', "").replace('"', "");
            }
            else {

                document.getElementById('myimg').src = BImg;
            }
        }
    });

    

});

function fullImage() {
    // alert('ok');
    document.getElementById('Vouimg').style.display = 'block'
    // alert(document.getElementById('myimg').src);
    document.getElementById('myfulimg').src = document.getElementById('myimg').src;
}

var value = 0;
callrotate = function () {
    alert('ok');
    value += 180;
    $("#myimg").rotate({ animateTo: value })
}

