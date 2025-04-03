

$(document).ready(function () {


    $.ajax({
        url: RootUrl + 'ScanningCutOffTime/GetDomainList',
        // type: 'Post',
        contentType: 'application/json; charset=utf-8',
        dataType: 'html',
        data: {},
        //async: false,
        success: function (domainCodeLists) {
            console.log("In Domain");
            $("#DomainSelect").empty();

            var domainLists = JSON.parse(domainCodeLists);
            console.log(domainLists);
            $("#DomainSelect").append(
                $('<option></option>').val(0).html("-----Select-----"));
            $.each(domainLists, function (i, DomainSelect) {
                $("#DomainSelect").append(
                    $('<option></option>').val(DomainSelect.Id).html(DomainSelect.Name));
                //$('<option></option>').val(BranchSelect.BranchCode).html(BranchSelect.BranchCode + " (" + BranchSelect.BranchName + ")"));
            });


        }
    });

    $('#DomainSelect').change(function () {

        //EnableOrDisableSelect();
        var domainId = document.getElementById("DomainSelect").value;

        $.ajax({
            url: RootUrl + 'ScanningCutOffTime/GetScanningCutOffTimeForDomain',
            // type: 'Post',
            contentType: 'application/json; charset=utf-8',
            dataType: 'html',
            data: { DomainId: domainId },
            //async: false,
            success: function (data) {
                debugger;
                var modifiedString = data.replace(/"/g, '');
                var jsonSt = modifiedString.split(',');

                var jsonNew = jsonSt[0].split(':');
                var jsonSt_H = jsonNew[0];
                var jsonSt_M = jsonNew[1];

                document.getElementById('scanningCutOffTimeHours').value = jsonSt_H;
                document.getElementById('scanningCutOffTimeMinutes').value = jsonSt_M;

                var isDisabled = jsonSt[1];

                var rdDisabled = document.getElementById('rdDisable');
                var rdEnabled = document.getElementById('rdEnable');

                if (isDisabled == "1" || isDisabled == "N") {
                    
                    rdDisabled.checked = true;
                    rdEnabled.checked = false;

                    document.getElementById('scanningCutOffTimeHours').readOnly = true;
                    document.getElementById('scanningCutOffTimeMinutes').readOnly = true;
                    document.getElementById('tempExtensionTime').readOnly = true;
                    document.getElementById('allowedInstrumentCount').readOnly = true;

                    document.getElementById('scanAlertTimeHours').readOnly = true;
                    document.getElementById('scanAlertTimeMinutes').readOnly = true;
                }
                else if (isDisabled == "0" || isDisabled == "Y") {
                    
                    rdEnabled.checked = true;
                    rdDisabled.checked = false;

                    document.getElementById('scanningCutOffTimeHours').readOnly = false;
                    document.getElementById('scanningCutOffTimeMinutes').readOnly = false;
                    document.getElementById('tempExtensionTime').readOnly = false;
                    document.getElementById('allowedInstrumentCount').readOnly = false;

                    document.getElementById('scanAlertTimeHours').readOnly = false;
                    document.getElementById('scanAlertTimeMinutes').readOnly = false;
                }

                document.getElementById('tempExtensionTime').value = jsonSt[2];
                document.getElementById('allowedInstrumentCount').value = jsonSt[3];
                document.getElementById('instrumentScannedAfterCutOffTime').value = jsonSt[4];

                var jsonNew_ScanAlert = jsonSt[5].split(':');
                var jsonStScanAlert_H = jsonNew_ScanAlert[0];
                var jsonStScanAlert_M = jsonNew_ScanAlert[1];

                document.getElementById('scanAlertTimeHours').value = jsonStScanAlert_H;
                document.getElementById('scanAlertTimeMinutes').value = jsonStScanAlert_M;

            }
        });
    });

    $("#allowedInstrumentCount1").on('keypress', function () {

        //debugger;
        
        if ($(this).val().length > 4) {
            alert("Length cannot be more than four digits.");
            return false;
        }

    });

    document.getElementById("rdEnable").addEventListener("change", function () {
        // Check if the radio button is selected
        debugger;
        if (this.checked) {
            document.getElementById('scanningCutOffTimeHours').readOnly = false;
            document.getElementById('scanningCutOffTimeMinutes').readOnly = false;
            document.getElementById('tempExtensionTime').readOnly = false;
            document.getElementById('allowedInstrumentCount').readOnly = false;

            document.getElementById('scanAlertTimeHours').readOnly = false;
            document.getElementById('scanAlertTimeMinutes').readOnly = false;

        }
    });

    document.getElementById("rdDisable").addEventListener("change", function () {
        // Check if the radio button is selected
        debugger;
        if (this.checked) {
            
            document.getElementById('scanningCutOffTimeHours').readOnly = true;
            document.getElementById('scanningCutOffTimeMinutes').readOnly = true;
            document.getElementById('tempExtensionTime').readOnly = true;
            document.getElementById('allowedInstrumentCount').readOnly = true;

            document.getElementById('scanAlertTimeHours').readOnly = true;
            document.getElementById('scanAlertTimeMinutes').readOnly = true;
        }
    });

    $("#btnOk").click(function () {

        debugger;
        var result = validateForm();
        debugger;
        if (result == false) {

            return false;
        }
        else {

            var domainId = document.getElementById("DomainSelect").value;
            var selectedRadio = document.querySelector('input[name="gridRadios"]:checked');
            var selectedStatus = selectedRadio.value;
            var isEnabled = "";
            if (selectedStatus == "Enable") {
                isEnabled = "0";
            }
            else if (selectedStatus == "Disable") {
                isEnabled = "1";
            }

            var scanningCutOfTime_H = document.getElementById('scanningCutOffTimeHours').value;
            var scanningCutOfTime_M = document.getElementById('scanningCutOffTimeMinutes').value;
            var scanningCutOfTime = scanningCutOfTime_H + ':' + scanningCutOfTime_M;

            var tempExtensionTime = document.getElementById('tempExtensionTime').value;
            if (tempExtensionTime == "") {
                tempExtensionTime = "0";
            }

            var allowedInstrumentCount = document.getElementById('allowedInstrumentCount').value;
            if (allowedInstrumentCount == "") {
                allowedInstrumentCount = "0";
            }

            var scanAlertTime_H = document.getElementById('scanAlertTimeHours').value;
            var scanAlertTime_M = document.getElementById('scanAlertTimeMinutes').value;
            var scanAlertTime = scanAlertTime_H + ':' + scanAlertTime_M;

            $.ajax({
                url: RootUrl + 'ScanningCutOffTime/Update_ScanningCutOffTime',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ DomainId: domainId, IsEnabled: isEnabled, ScanningCutOfTime: scanningCutOfTime, 
                    TempExtensionTime: tempExtensionTime, AllowedInstrumentCount: allowedInstrumentCount, ScanAlertTime: scanAlertTime }),
                dataType: 'html',
                success: function (result) {

                },
                error: function (err) {
                    debugger;
                    alert("Error - " + err);
                }
            });
        }

    });

});

function validateForm() {
    debugger;
    //====== DomainName validation
    var domainId = document.getElementById("DomainSelect").value;

    if (domainId == 0 || domainId == "0" || domainId == "") {
        alert('Please select domain');
        return false;
    }

    //========== Status validation
    var selectedRadio = document.querySelector('input[name="gridRadios"]:checked');

    if (!selectedRadio) {
        alert('Please select status');
        return false;
    }

    //========= ScanningCutOffTime validation
    var scanningCutOfTime_H = document.getElementById('scanningCutOffTimeHours').value;
    var scanningCutOfTime_M = document.getElementById('scanningCutOffTimeMinutes').value;

    if (scanningCutOfTime_H == "") {
        alert('Please enter scanning cut off time in Hours');
        return false;
    }

    if (scanningCutOfTime_M == "") {
        alert('Please enter scanning cut off time in Minutes');
        return false;
    }

    //========= ScanAlertTime validation
    var scanAlertTime_H = document.getElementById('scanAlertTimeHours').value;
    var scanAlertTime_M = document.getElementById('scanAlertTimeMinutes').value;

    if (scanAlertTime_H == "") {
        alert('Please enter scan alert time in Hours');
        return false;
    }

    if (scanAlertTime_M == "") {
        alert('Please enter scan alert time in Minutes');
        return false;
    }

    //======== Temporary Time Extention validation
    //var tempExtensionTime = document.getElementById('tempExtensionTime').value;

    //if (tempExtensionTime == "") {
    //    alert('Please enter temporary extension time in minutes');
    //    return false;
    //}

    //======== Temporary Time Extention validation
    //var allowedInstrumentCount = document.getElementById('allowedInstrumentCount').value;
    //if (allowedInstrumentCount == "") {
    //    alert('Please enter allowed instrument count');
    //    return false;
    //}

    // Get the value of the time input field
    //var timeInput = document.getElementById("tempExtensionTime").value;

    //// Define a regular expression to match the HH:MM format
    //var timePattern = /^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$/;

    //// Check if the input matches the pattern
    //if (!timePattern.test(timeInput)) {
    //    // If it doesn't match, display an error message
    //    alert("Please enter a valid time in HH:MM format.");
    //    return false; // Prevent form submission
    //}

    // If the input is valid, allow the form to submit
    //return true;
}

function limitToFourDigits(inputElement) {
    // Get the input value
    var inputValue = inputElement.value;

    // Remove any non-digit characters (e.g., spaces)
    inputValue = inputValue.replace(/\D/g, '');

    // Ensure the input is not longer than four digits
    if (inputValue.length > 3) {
        // Truncate the input to four digits
        inputValue = inputValue.slice(0, 3);
    }

    // Update the input value with the sanitized value
    inputElement.value = inputValue;
}

function limitTo23(inputElement) {
    // Get the input value
    var inputValue = inputElement.value;

    // Remove any non-digit characters (e.g., spaces)
    inputValue = inputValue.replace(/\D/g, '');

    // Ensure the input is not greater than 23
    if (inputValue !== '') {
        var numericValue = parseInt(inputValue, 10);
        if (numericValue > 23) {
            // Limit the input to 23
            inputValue = '23';
        }
    }

    // Update the input value with the sanitized value
    inputElement.value = inputValue;
}

function limitTo59(inputElement) {
    // Get the input value
    var inputValue = inputElement.value;

    // Remove any non-digit characters (e.g., spaces)
    inputValue = inputValue.replace(/\D/g, '');

    // Ensure the input is not greater than 23
    if (inputValue !== '') {
        var numericValue = parseInt(inputValue, 10);
        if (numericValue > 59) {
            // Limit the input to 23
            inputValue = '59';
        }
    }

    // Update the input value with the sanitized value
    inputElement.value = inputValue;
}



