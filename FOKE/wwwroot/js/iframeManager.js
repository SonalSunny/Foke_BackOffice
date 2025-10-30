var wh; var ww; var resizeTimer = null;
$(document).ready(function () {
    wh = $(window).height();
    ww = $(window).width();
    $(window).bind("resize", function () {
        wh = $(window).height();
        ww = $(window).width();
        if (resizeTimer) clearTimeout(resizeTimer); resizeTimer = setTimeout(sh, 500);
    });
    sh();

    $(".ChangePage").on('click', function () {
        var cpp = document.getElementById("ContFrame");
        cpp.contentWindow.document.body.innerHTML = '<div class="page_loader_parent"> <span class="page_loader"></span> </div>';
        cpp.src = $(this).attr("src");
    });
});
function sh() {
    $("#ContFrame").height(wh - 80);
    //  if ($(".pop").is(":visible")) {
    //     setPopup(ww, wh);
    //  }
}

function loadContentIframe(src) {
    var cpp = document.getElementById("ContFrame");
    cpp.contentWindow.document.body.innerHTML = '<div class="page_loader_parent"> <span class="page_loader"></span> </div>';
    cpp.src = src;
    return false;
}
function hidePdfViewer() {
    $("#pdfViewer").attr("src", ""); // Clear PDF
    $("#pdfViewerWrapper").hide();
    $("#mainContentWrapper").show();
    $("#kt_app_main").show();
    $("#pdfViewerWrapperexcell").hide();
}
function hidePdfViewermanage() {
    $("#pdfViewer").attr("src", ""); // Clear PDF
    $("#pdfViewerWrapper").hide();
    $("#mainContentWrappermanage").show();
    $("#pdfViewerWrapperexcell").hide();
}
function downloadexcell() {
    // Get the image URL from the data-url attribute
    let fileUrl = $("#hiddenFileUrl").val(); // More reliable way to fetch data attributes
    console.log("Download fileUrl: " + fileUrl);
    if (fileUrl && fileUrl.trim() !== "") {
        $("#pdfViewer").attr("src", fileUrl);
    } else {
        Swal.fire({
            icon: "error",
            title: "Attachment Not Found",
            text: "The file URL is missing or invalid.",
            confirmButtonColor: "#d33"
        });
    }
}
