function myConfirm(dialogText, okFunc, cancelFunc, dialogTitle) {
    $('<div style="padding: 10px; ; word-wrap: break-word;">' + dialogText + '</div>').dialog({
        dialogClass: "no-close",
        draggable: false,
        modal: true,
        resizable: false,
        width: 'auto',
        title: dialogTitle || 'Confirm',
        minHeight: 75,
        buttons: {
            OK: function () {
                if (typeof (okFunc) == 'function') { okFunc(); }
                $(this).dialog('destroy');
            },
            Abort: function () {
                if (typeof (cancelFunc) == 'function') { cancelFunc(); }
                $(this).dialog('destroy');
            }
        }
    });

};
