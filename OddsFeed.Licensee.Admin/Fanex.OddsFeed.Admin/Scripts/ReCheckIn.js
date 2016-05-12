//function ReCheckin() {
//    var KickedOut = 3;
//    var strOnUserId = $("#hdfOnUserId").val();

//    $.ajax({
//        url: location.protocol + '//' + location.host + "/OddsFeedAdmin/Login/ReCheckLogin",
//        type: "POST",
//        dataType: "json",
//        success: function (result) {
//            if (result == "1") {
//                window.location.href = location.protocol + '//' + location.host + "/OddsFeedAdmin/Login/LogOut";
//            }
//        },
//        error: function (request, status, error) {
//        }
//    });
//}

//function closeBrowser() {
//    var index = window;
//    while (index != index.parent) // Jump out
//    {
//        index = index.parent;
//    }

//    index.close();
//}

//$(document).ready(function () {
//    setInterval("ReCheckin()", 5000);
//});

var Recheckin = function ($) {

    return {
        time: null,
        root: null,
        headlineMsgBox: null,
        kickedOutMsg: null,

        init: function () {
            Recheckin.time = 5000;
            Recheckin.root = currentDomainURL;// location.protocol + '//' + location.host;
            Recheckin.kickedOutMsg = 'You have logged in from another session or your login session has expired!';
        },

        reCheckin: function () {

            $.ajax({
                url: Recheckin.root + "/Login/ReCheckLogin",
                type: "POST",
                dataType: "json",
                success: function (result) {
                    if (result == "1") {
                        Recheckin.headlineMsgBox.showInfo(Recheckin.kickedOutMsg, { closeBtn: false });
                    }
                },
                error: function (request, status, error) {
                    var result = $.parseJSON(request.responseText);
                    if (result.code == 999999) {
                        location.href = "/Tolerance/UnderMaintainance";
                    }
                }
            });
        },

        closeBrowser: function () {
            var index = window;
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
            window.location.href = Recheckin.root + "/Login/LogOut";
        }
    });

    setInterval("Recheckin.reCheckin()", Recheckin.time);
});