function updateTips(t) {
    $(".validateTips")
    .text(t)
    .addClass("ui-state-highlight");
    setTimeout(function () {
        $(".validateTips").removeClass("ui-state-highlight", 1500);
    }, 500);
}

function checkRequired(o, n) {
    if (o.val() == undefined || o.val().length <= 0) {
        o.addClass("ui-state-error");
        updateTips("Please enter " + n + ".");
        return false;
    } else {
        return true;
    }
}

function checkLength(o, n, min, max) {
    if (o.val().length > max || o.val().length < min) {
        o.addClass("ui-state-error");
        updateTips("Length of " + n + " must be between " +
min + " and " + max + ".");
        return false;
    } else {
        return true;
    }
}

function checkRegexp(o, regexp, n) {
    if (o.val() == undefined || o.val().length <= 0) {
        return true;
    }

    if (!(regexp.test(o.val()))) {
        o.addClass("ui-state-error");
        updateTips(n);
        return false;
    } else {
        return true;
    }
}

function GetErrorMessage(errCode) {
    $.ajax({
        url: "GetMessage",
        type: "POST",
        data: "ErrCode=" + errCode,
        success: function (result) {
            return updateTips(result);
        }
    });
}

function OpenDialog(id) {
    $(".validateTips").text("").removeClass("ui-state-highlight"); ;
    $("#" + id).dialog("open");
}