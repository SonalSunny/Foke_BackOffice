$(document).ready(function () {
    $('input[type=file1]').change(function () {
        $(this).simpleUpload("/api/FileUploader/Upload", {
            start: function (file) {
                $("#kt_upload_preview_zone").removeClass("d-none");
                $("#kt_upload_dropzone").addClass("d-none");
                $("#k_upload_fileName").html(file.name);
                $("#k_upload_fileSzie").html("<strong>" + (file.size).formatBytes() + "</strong>");
                $("#k_upload_progress").css("opacity", 1);
                //style="opacity: 0; width: 100%;"
                console.log("upload started");
            },
            progress: function (progress) {
                $("#k_upload_progress").css("width", Math.round(progress) + "%");
                console.log("upload progress: " + Math.round(progress) + "%");
            },
            success: function (data) {
                if (data.Status == "200") {

                }
                console.log("upload successful!");
                console.log(data);
            },
            error: function (error) {
                console.log("upload error: " + error.name + ": " + error.message);
            }

        });

    });

    Number.prototype.formatBytes = function () {
        var units = ['B', 'KB', 'MB', 'GB', 'TB'],
            bytes = this,
            i;

        for (i = 0; bytes >= 1024 && i < 4; i++) {
            bytes /= 1024;
        }

        return bytes.toFixed(2) + units[i];
    }

});