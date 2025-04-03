function checkfile() {
    debugger;
    var file = getNameFromPath($("#fileToUpload").val());
    if (file != null) {
        var extension = file.substr((file.lastIndexOf('.') + 1));
        //  alert(extension);
        switch (extension) {
            //case 'txt' || 'dat':
            case 'txt':
                //case 'png':
                //case 'gif':
                //case 'pdf':
                flag = true;
                break;
            case 'TXT':
                flag = true;
                break;
            default:
                flag = false;
        }
    }
    if (flag == false) {
        $("#spanfile").text("You can upload only txt extension file");
        document.getElementById("btnSubmit").disabled = true;
        return false;

    }
    else {

        if (file != null) {
            let fileName = file.substr(0, file.lastIndexOf('.'));
            //var pattern = /^[a-zA-Z0-9_]+$/;
            if (isAlphanumericWithUnderscore(fileName)) {
                console.log("String is valid.");
                document.getElementById("btnSubmit").disabled = false;
            } else {
                //console.log("String is not valid.");
                $("#spanfile").text("Please upload file name format as alphanumeric with underscore");
                document.getElementById("btnSubmit").disabled = true;
                return false;
            }

            var size = GetFileSize('fileToUpload');
            if (size > 3) {
                $("#spanfile").text("You can upload file up to 3 MB");
                document.getElementById("btnSubmit").disabled = true;
                return false;
            }
            else {
                $("#spanfile").text("");
                document.getElementById("btnSubmit").disabled = false;
            }
        }
        
    }
}
$(function () {
    $("#fileToUpload").change(function () {
        checkfile();
    });

});

//function GetFileSize(fileid) {
//    debugger;
//    try {
//        var fileSize = 0;
//        //for IE
//        if ($.browser.msie) {
//            //before making an object of ActiveXObject, 
//            //please make sure ActiveX is enabled in your IE browser
//            var objFSO = new ActiveXObject("Scripting.FileSystemObject"); var filePath = $("#" + fileid)[0].value;
//            var objFile = objFSO.getFile(filePath);
//            var fileSize = objFile.size; //size in kb
//            fileSize = fileSize / 1048576; //size in mb 
//        }
//        //for FF, Safari, Opeara and Others
//        else {
//            fileSize = $("#" + fileid)[0].files[0].size //size in kb
//            fileSize = fileSize / 1048576; //size in mb 
//        }

//        // alert("Uploaded File Size is" + fileSize + "MB");
//        return fileSize;
//    }
//    catch (e) {
//        alert("Error is :" + e);
//    }
//}

//get file path from client system
function getNameFromPath(strFilepath) {

    var objRE = new RegExp(/([^\/\\]+)$/);
    var strName = objRE.exec(strFilepath);

    if (strName == null) {
        return null;
    }
    else {
        return strName[0];
    }

}

function isAlphanumericWithUnderscore(inputString) {
    // Define the regular expression pattern
    var pattern = /^[a-zA-Z0-9_]+$/;

    // Test the input string against the pattern
    return pattern.test(inputString);
}

function GetFileSize(fileid) {
    debugger;
    try {
        var fileSize = 0;

        // Check if the browser is Internet Explorer
        if (navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0) {
            var objFSO = new ActiveXObject("Scripting.FileSystemObject");
            var filePath = $("#" + fileid).val();
            var objFile = objFSO.getFile(filePath);
            fileSize = objFile.size; // Size in bytes
        }
        // For other browsers (Firefox, Safari, Opera, etc.)
        else {
            var inputElement = document.getElementById(fileid);
            if (inputElement.files && inputElement.files.length > 0) {
                fileSize = inputElement.files[0].size; // Size in bytes
            }
        }

        // Convert size to megabytes
        fileSize = fileSize / (1024 * 1024);

        // Return the file size in megabytes
        return fileSize;
    } catch (e) {
        console.error("Error:", e);
        return 0; // Return 0 if there's an error
    }
}