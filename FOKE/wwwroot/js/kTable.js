
$(function () {
    $.fn.kTable = function (options) {
        var srcParams = $.extend({
            submitButton: '#btnRefresh',
            resetButton: '#btnReset',
            resetquoButton: '#btnReset1',
            targetContainer: '#_postList',
            searchInputField: '#txtSearchInputField',
            searchColumnChooser: '#globalSearchColumnChooser',
            resourceUrl: null,
            pageSizeField: "#hidpageSize",
            loadOnPageLoad: true,
            sOrderField: "#hidSortOrder", /*1 fro assending 0 fro descending*/
            sOrderColumnField: "#hidSortColumn",
            exportUrl: null,
            exportButton: '#btnExport',
            radiobutton: '#hidRadio',
            searchfield: '#hidSearchfield'

        }, options);

        if (srcParams.loadOnPageLoad) {
            LoadPagedListData();
        }
        $(srcParams.submitButton).click(function (e) {
            hidePageLoader();
            LoadPagedListData();
        });

        $(srcParams.resetButton).click(function (e) {
            //if (typeof srcParams.searchColumnChooser !== 'undefined' && srcParams.searchColumnChooser != null && srcParams.searchColumnChooser != '') {
            //    $(srcParams.searchColumnChooser).val("All").trigger('change');
            //}
            //if (typeof srcParams.searchInputField !== 'undefined' && srcParams.searchInputField != null && srcParams.searchInputField != '') {
            //    $(srcParams.searchInputField).val("");
            //}
            //$(srcParams.submitButton).click();
            // Prevent default action if necessary
            e.preventDefault();

            // Reload the page
            location.reload();
        });
        $(srcParams.resetquoButton).click(function (e) {
            if (typeof srcParams.searchColumnChooser !== 'undefined' && srcParams.searchColumnChooser != null && srcParams.searchColumnChooser != '') {
                $(srcParams.searchColumnChooser).val("Select").trigger('change');
            }
            if (typeof srcParams.searchInputField !== 'undefined' && srcParams.searchInputField != null && srcParams.searchInputField != '') {
                $(srcParams.searchInputField).val("").trigger('change');
            }
            $(srcParams.submitButton).click();
        });
        $(srcParams.exportButton).click(function (e) {
            var postData = $(this).data('postdata') || {};

            var exportType = $(this).data('exportType') || 'excel'; // Default to Excel
          

            // Append handler for Razor Page
            var dynamicExportUrl = $(this).data('export-url') || srcParams.exportUrl;

          

           
            $.ajax({
                url:dynamicExportUrl,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(postData),
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                    Swal.fire({
                        title: 'Please Wait!',
                        didOpen: () => {
                            Swal.showLoading()
                        }
                    });
                },
            }).done(function (data, statusText, xhdr) {
                swal.close()
                try {
                    if (data.tFileName != "") {
                        window.location.href = "/api/Report/Download?tFile=" + data.tFileName + "&fileName=" + data.fileName;
                    }
                }
                catch (e) {

                }
            }).fail(function (xhdr, statusText, errorText) {
                swal.close()
            });
        });

        function LoadPagedListData() {
            var pageNumber = 1;
            var pageSize = 10;
            var sortOrder = 1;
            var sortColumn = "";
            var globalSearch = "";
            var globalSearchColumn = "";

            if (typeof srcParams.pageSizeField !== 'undefined' && srcParams.pageSizeField != null && srcParams.pageSizeField != '') {
                pageSize = $(srcParams.pageSizeField).val();
                if (!(typeof pageSize !== 'undefined' && pageSize != null && pageSize != '')) {
                    pageSize = 10;
                }
            }
            if (typeof srcParams.sOrderField !== 'undefined' && srcParams.sOrderField != null && srcParams.sOrderField != '') {
                sortOrder = $(srcParams.sOrderField).val();
            }
            if (typeof srcParams.sOrderColumnField !== 'undefined' && srcParams.sOrderColumnField != null && srcParams.sOrderColumnField != '') {
                sortColumn = $(srcParams.sOrderColumnField).val();
            }
            if (typeof srcParams.searchInputField !== 'undefined' && srcParams.searchInputField != null && srcParams.searchInputField != '') {
                globalSearch = $(srcParams.searchInputField).val();
            }
            if (typeof srcParams.searchColumnChooser !== 'undefined' && srcParams.searchColumnChooser != null && srcParams.searchColumnChooser != '') {
                globalSearchColumn = $(srcParams.searchColumnChooser).val();
            }
            if (typeof srcParams.sOrderColumn !== 'undefined' && srcParams.sOrderColumn != null && srcParams.sOrderColumn != '') {
                globalSearchColumn = srcParams.sOrderColumn;
            }
            var searchfield = $(srcParams.searchfield).val();
            if (searchfield === "" || isNaN(searchfield)) {
                searchfield = null;
            } else {
                searchfield = parseInt(searchfield); // ensure it's passed as number
            }
            var radioValue = $(srcParams.radiobutton).val() || "Unpaid";
            var data = { pn: 1, ps: pageSize, so: sortOrder, sc: sortColumn, gs: globalSearch, gsc: globalSearchColumn, searchfield: searchfield, Value: radioValue }

            getAjaxPagedList(srcParams.resourceUrl, data, srcParams.targetContainer);
        }
    }

    $('.table-sortable').one("click", ".sorting", function (e) {
        var fName = $(this).closest('th').attr('area-field');
        $("#hidSortColumn").val(fName);
        $("#hidSortOrder").val("0");
        $(this).removeClass('sorting').addClass('sorting_desc');
        $(this).find("span").find("i").removeClass('fa-sort').addClass('fas fa-sort-down');
        $("#btnRefresh").trigger("click");
    });



    $('.table-sortable').one("click", ".sorting_asc", function (e) {
        var fName = $(this).closest('th').attr('area-field');
        $("#hidSortColumn").val(fName);
        $("#hidSortOrder").val("1");
        $(this).removeClass('sorting_asc').addClass('sorting_desc');
        //$(this).removeClass('sorting').addClass('sorting_desc');
        //$(this).removeClass('fa-sort').addClass('fas fa-sort-up');
        $(this).find("span").find("i").removeClass('fas fa-sort-down').addClass('fas fa-sort-up');
        $("#btnRefresh").trigger("click");
    });



    $('.table-sortable').one("click", ".sorting_desc", function (e) {
        var fName = $(this).closest('th').attr('area-field');
        $("#hidSortColumn").val(fName);
        $("#hidSortOrder").val("0");
        $(this).removeClass('sorting_desc').addClass('sorting_asc');
        //$(this).removeClass('sorting').addClass('sorting_asc');


        //$(this).removeClass('fa-sort').addClass('fas fa-sort-down');
        $(this).find("span").find("i").removeClass('fas fa-sort-up').addClass('fas fa-sort-down');
        $("#btnRefresh").trigger("click");
    });

    var $table = $('.table-sortable');
    var th = $table.find("th.sorting");

    var sColumn = $("#hidSortColumn").val();
    var sOrder = $("#hidSortOrder").val();

    if (th.length > 0 && sColumn != "") {
        th.each(function (index, tr) {
            var fName = $(this).attr('area-field');
            if (sColumn == fName) {

                if (sOrder == "0") {
                    $(this).removeClass('sorting_desc').addClass('sorting_asc');
                    $(this).removeClass('sorting').addClass('sorting_asc');

                    $(this).find("span").find("i").removeClass('fa-sort').addClass('fas fa-sort-down');
                    $(this).find("span").find("i").removeClass('fas fa-sort-up').addClass('fas fa-sort-down');
                }
                else {
                    $(this).removeClass('sorting_asc').addClass('sorting_desc');
                    $(this).removeClass('sorting').addClass('sorting_desc');

                    $(this).find("span").find("i").removeClass('fa-sort').addClass('fas fa-sort-up');
                    $(this).find("span").find("i").removeClass('fas fa-sort-down').addClass('fas fa-sort-up');
                }
            }
        });
    }

    $('.table-filter-type').one('click', function (event) {
        var fField = $(this).attr('filter-field');
        var fFieldName = $(this).attr('filter-label');
        event.preventDefault();
        $("#hidSearchField").val(fField);
        $("#table-search-global").attr("placeholder", fFieldName);
        var cTextVal = $("#table-search-global").val();
        if (cTextVal != "") {
            $("#btnRefresh").trigger("click");
        }
    })

    $("#kt_datatable_page_size").change(function () {
        var pageSize = this.value;
        debugger
        $("#hidpageSize").val(pageSize);
        $("#btnRefresh").trigger("click");
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



});

function showInTableLoader() {
    $(".loading-ct").show();
}

function hideInTableLoader() {
    $(".loading-ct").hide();
}

function applyFilterCompleted() {
    hidePageLoader();
    var test = $('node').kTable();
    $.kTable.LoadPagedListData();
}


