var wh; var ww;
wh = $(window).height(); ww = $(window).width();
var ph; var pw;
function showPopup(title, url, w, h, iframeid) {
    $("#hidCallbackFrom").val(iframeid);
    ph = h; pw = w;
    $("#popupTitle").html(title);
    //$("#kIframemodal").modal({
    //    backdrop: 'static',
    //    keyboard: false
    //});
    $("#kIframemodal").modal("show");
    var pcc = document.getElementById("ifrmPopup");
    //cpp.contentWindow.document.body.innerHTML = '<div style="font-size:12px; text-align:center;padding-top:' + (h - 50) / 2 + 'px;"><img src="/Images/loader.gif" alt="L o a d i n g..." /></div>';
    pcc.contentWindow.document.body.innerHTML = "<div class='preloader-backdrop'><div class='page-preloader'></div></div>";
    pcc.src = url;
    try { pcc.focus(); } catch (err) { }
    $("#ifrmPopup").height(h - 8);
    $(".modal-dialog").css("maxWidth", w);
    $(".modal-dialog").height(h);
    $(".modal-content").height(h + 35);
    /*  $("#ifrmPopup").height(h - 28);*/
    //$(".modal-dialog").height(h).width(w);
    //$(".modal-content").height(h).width(w);
    /*$(".modal-body").height(h).width(w);*/
}

function hidePopup() {
    $('#kIframemodal').modal('hide');
}

function hidePopupWithRefresh(rBtnName, val, vField) {
    $('#kIframemodal').modal('hide');
    refreshContentFromPopup(rBtnName, val, vField);
}

function refreshContentFromPopup(rBtnName, val, vField) {
    if (rBtnName != "") {
        try {
            var frame = $("#hidCallbackFrom").val();
            var cpframe = document.getElementById(frame);
            if (cpframe.contentWindow.document.getElementById(vField) != null) {
                cpframe.contentWindow.document.getElementById(vField).value = val
            }
            if (cpframe.contentWindow.document.getElementById(rBtnName) != null) {
                cpframe.contentWindow.document.getElementById(rBtnName).click();
            }
            /* $('#kIframemodal').modal('hide');*/
        }
        catch (err) {
        }
    }
}


function showFileUpladerPopup(title, url, w, h, iframeid) {
    $("#hidCallbackFrom").val(iframeid);
    ph = h; pw = w;
    $("#popupTitle").html(title);
    //$("#kIframemodal").modal({
    //    backdrop: 'static',
    //    keyboard: false
    //});
    $("#kFileUploadermodal").modal("show");
    var pcc = document.getElementById("ifrmFileUploaderPopup");
    //cpp.contentWindow.document.body.innerHTML = '<div style="font-size:12px; text-align:center;padding-top:' + (h - 50) / 2 + 'px;"><img src="/Images/loader.gif" alt="L o a d i n g..." /></div>';
    pcc.contentWindow.document.body.innerHTML = "<div class='preloader-backdrop'><div class='page-preloader'>Loading</div></div>";
    pcc.src = url;
    try { pcc.focus(); } catch (err) { }
    $("#ifrmPopup").height(h - 8);
    $(".modal-dialog").css("maxWidth", w);
    $(".modal-dialog").height(h);
    $(".modal-content").height(h + 35);
    /*  $("#ifrmPopup").height(h - 28);*/
    //$(".modal-dialog").height(h).width(w);
    //$(".modal-content").height(h).width(w);
    /*$(".modal-body").height(h).width(w);*/
}