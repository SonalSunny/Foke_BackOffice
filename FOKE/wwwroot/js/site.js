// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getControlValue(id, defReturnValue) {
    var val = $("#" + id).val();
    if (typeof val === 'undefined' || val == null) {
        return defReturnValue;
    }
    else {
        return val;
    }
}

function setControlValue(id, defReturnValue) {
    var control = $("#" + id);
    if (typeof val === 'undefined' || val == null) {
        control.val(defReturnValue);
    }
}

function showPageLoader() {
    $("#divLoader").show();
}

function hidePageLoader() {
    $("#divLoader").hide();
}

$(function () {
    $(':button,:submit').click(function (e) {
        if ($(e.target).hasClass("btn-req-loader")) {
            if ($("form").valid()) {
               top.showPageLoader();
            }
        }
    });
});

function getAjaxPagedList(pageUrl, data, targetID) {
    if (typeof targetID === 'undefined' || targetID == "") {
        targetID = "_postList";
    }
    showInTableLoader();
    $.ajax({
        url: pageUrl,
        type: 'GET',
        cache: false,
        data: data,
        success: function (resp) {
            $("#" + targetID).html(resp);
            hideInTableLoader();
        }, error: function (jqXHR, textStatus, errorThrown) {
            hideInTableLoader();
        }
    });
}


function postAjaxDeleteMasterData(pageUrl, data, callback) {
    top.showPageLoader();
    $.ajax({
        url: pageUrl,
        type: 'POST',
        cache: false,
        data: data,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (resp) {
            top.hidePageLoader();
            if (resp.returnMessage == 'Deactivated') {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Deactivated Successfully!'
                })
                callback(true);
            }
            else if (resp.returnMessage == 'Activated') {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Activate Successfully!'
                })
                callback(true);
            }
            else if (resp.returnMessage == 'Approved') {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Approved Successfully!'
                })
                callback(true);
            }
            else if (resp.returnMessage == 'Rejected') {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Rejected Successfully!'
                })
                callback(true);
            }





            else {
                if (resp.status == 'OK') {
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: 'Deleted Successfully!'
                    })
                    callback(true);
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Failed to delete record!!',
                        text: resp.returnMessage
                    })
                }
            }

        }, error: function (jqXHR, textStatus, errorThrown) {
            top.hidePageLoader();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!'
            })
            callback(false);
        }
    });
}
function postAjaxStatusChange(pageUrl, data, callback) {
    top.showPageLoader();
    $.ajax({
        url: pageUrl,
        type: 'POST',
        cache: false,
        data: data,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (resp) {
            top.hidePageLoader();
            if (resp.status == 'OK') {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Changed Successfully!'
                })
                callback(true);
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Failed to Change record!!',
                    text: resp.returnMessage
                })
            }
        }, error: function (jqXHR, textStatus, errorThrown) {
            top.hidePageLoader();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!'
            })
            callback(false);
        }
    });
}


function postAjaxExportData(pageUrl, data, callback) {
    top.showPageLoader();
    $.ajax({
        url: pageUrl,
        type: 'POST',
        cache: false,
        data: data,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (resp) {
            top.hidePageLoader();
            
        }, error: function (jqXHR, textStatus, errorThrown) {
            top.hidePageLoader();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Something went wrong!'
            })
            callback(false);
        }
    });
}

function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function postAjaxApproveMasterData(pageUrl, data, callback) {
    top.showPageLoader();
    $.ajax({
        url: pageUrl,
        type: 'POST',
        cache: false,
        data: data,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (resp) {
            top.hidePageLoader();
            if (resp.status == 'OK') {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Successfully Done !'
                })
                callback(true);
            }
            else {
                Swal.fire({
                    icon: 'error',
                    title: 'Failed to save record!!',
                    text: resp.returnMessage
                })
            }
        }, error: function (jqXHR, textStatus, errorThrown) {
            top.hidePageLoader();
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Please check your Internet Connection'
            })
            callback(false);
        }
    });
}
$(".toggle-password").click(function () {
    $(this).toggleClass("fa-eye fa-eye-slash"); // Toggle between fa-eye and fa-eye-slash classes
    var input = $($(this).attr("toggle"));

    // Check the current type of input and toggle it
    if (input.attr("type") === "password") {
        input.attr("type", "text"); // Show password (plain text)
    } else {
        input.attr("type", "password"); // Hide password (dots)
    }
});
