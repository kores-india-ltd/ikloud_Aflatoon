
$(document).ready(function () {
    $("#btnClientCd").click(function () {
        // alert($("#ClientCode").val());
        if ($("#ClientCode").val() == "") {
            alert('Please select ClientCode!!');
            $("#ClientCode").focus();
            return false;
        }

        $.ajax({
            url: RootUrl + 'CMSDataEntry/AdditionalDataEntry',
            data: { clientCode: $("#ClientCode").val() },
            type: 'POST',
            dataType: 'html',
            success: function (result) {
                if (result == "false") {
                    alert('No Data was found !!!');
                }
                else {
                    $('#clntSlip').html(result);
                }
            }

        });
    });
});
