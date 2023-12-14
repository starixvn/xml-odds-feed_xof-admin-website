let Recheckin = function ($) {
    return {
        time: null,
        root: null,
        headlineMsgBox: null,
        kickedOutMsg: null,
        init: function () {
            Recheckin.time = 30000;
            Recheckin.kickedOutMsg = 'You have logged in from another session or your login session has expired!';
        },
        reCheckin: function () {
            $.ajax({
                url: currentDomainURL + "Login/ReCheckLogin",
                type: "POST",
                dataType: "json",
                success: function (result) {
                    if (result == "1") {
                        Recheckin.headlineMsgBox.showInfo(Recheckin.kickedOutMsg, { closeBtn: false });
                    }
                },
                error: function (request, status, error) {
                    let result = $.parseJSON(request.responseText);
                    if (result.code == 999999) {
                        location.href = currentDomainURL + "Error/UnderMaintainance";
                    }
                }
            });
        },

        closeBrowser: function () {
            let index = window;
            while (index != index.parent) // Jump out
            {
                index = index.parent;
            }
            index.close();
        }
    }

} (jQuery);


$(document).ready(function () {
    Recheckin.init();

    Recheckin.headlineMsgBox = $.msgbox({
        message: '',
        //containerId: 'msg-bar',
        type: 'err',
        modal: false,
        position: ['top:130', 'center'],
        theme: '',
        zIndex: 9999,
        duration: 2000,
        useAnimation: false,
        beforeClose: function () {
            window.location = currentDomainURL + "Login/LogOut";
        }
    });

    setInterval("Recheckin.reCheckin()", Recheckin.time);
});