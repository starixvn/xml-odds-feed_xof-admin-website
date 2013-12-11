$(document).ready(function () {
    //Process redirect to Afiiliate list.
    var successed = $("#successed").val();
    if (successed == "true") {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location = currentDomainURL + "/Affiliate/List";
        });
    }

    //$("#isActiveAllAffiliate").attr('checked', false);

    // Register Active Dialog box
    $("#activeLicenseeDialog").dialog({
        autoOpen: false,
        height: 'auto',
        width: 350,
        modal: true,
        buttons: {
            "Submit": function () {
                var dialog = $(this);
                $(".result-message-box", dialog).addClass("hidden");
                $(".ui-state-error", dialog).removeClass("ui-state-error error");

                var isActiveAllAffiliate = $('#isActiveAllAffiliate', dialog).is(':checked');

                var url = 'EditActive?SystemID=' + dialog.data('licenseeID') + '&IsActive=true' + '&IsActiveAllAffiliate=' + isActiveAllAffiliate;

                $.ajax({
                    url: url,
                    type: 'POST',
                    dataType: 'html',
                    cache: false,
                    success: function (errCode) {
                        if (errCode != 0) {
                            GetErrorMessage(errCode);
                        }
                        else {
                            Submit();
                            $('#isActiveAllAffiliate', dialog).attr('checked', false);
                            $("#activeLicenseeDialog").dialog("close");
                        }
                    },
                    error: function (xhr, status, err) {
                    }
                });

            },
            Cancel: function () {
                var dialog = $(this);
                $(".result-message-box", dialog).addClass("hidden");
                $(".ui-state-error", dialog).removeClass("ui-state-error error");
                dialog.dialog("close");
            }
        },
        close: function () {
            $('input, select', $(this)).val("").removeClass("ui-state-error");            
        }
    });


});

//--------------------------------------------------------------------------------------------------------------------

function editActive(obj, licenseeID) {

    if ($(obj).hasClass('ico-tick')) {

        confirm('Do you want to deactivate this licensee ?', function () {
            $(obj).removeClass('ico-tick').addClass('ico-untick');

            var url = 'EditActive?SystemID=' + licenseeID + '&IsActive=false';

            //todo
            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'html',
                cache: false,
                success: function (errCode) {
                    if (errCode != 0) {
                        GetErrorMessage(errCode);
                    }
                    else {
                        Submit();
                    }
                },
                error: function (xhr, status, err) {
                }
            });
        });
    }
    else {
        $("#activeLicenseeDialog").data("licenseeID", licenseeID);        
        OpenDialog('activeLicenseeDialog');
    }

};

function deleteLicensee(licenseeID, name ) {
    confirm('Do you want to delete this licensee (' +name + ') ?', function () {
        var url = 'DeleteLicensee?SystemID=' + licenseeID

        //todo
        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'html',
            cache: false,
            success: function (errCode) {
                if (errCode != 0) {
                    GetErrorMessage(errCode);
                }
                else {
                    Submit();
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: true,
                        ResultMessage: "The licensee has been deleted successfully!"
                    }, $(".result-message-box"));
                }
            },
            error: function (xhr, status, err) {
                //NAP.handleAjaxError(xhr, status, err);
            }
        });
    });
};
