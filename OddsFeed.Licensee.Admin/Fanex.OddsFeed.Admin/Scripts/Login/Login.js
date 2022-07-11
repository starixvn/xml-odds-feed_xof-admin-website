var msg, username, password, btnSubmit, validate;

window.onload = function () {
    setTimeout('window.history.forward()', 0);

    msg = document.getElementById('msg');
    innerMsg = document.getElementById('inner-msg');
    username = document.getElementById('username');
    password = document.getElementById('password');
    validate = document.getElementById('validate');
    btnSubmit = document.getElementById('submit');

    username.focus();

    checkBrowser(); // check if the client's browser is supported

    if (innerMsg.innerHTML.length > 0) {
        msg.style.display = 'block';
        shake('msg', 5, 100);
    }
    else {
        msg.style.display = 'none';
    }

    validate.onkeyup = function (e) {
        if (e && e.keyCode == 13) {
            DoLogin();
        }
    };

    btnSubmit.onclick = function () {
        DoLogin();
    };
};


String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
};

function shake(target, times, duration) {
    for (var i = times, j = 0; i >= 0; i--) {
        setTimeout(target + ".style.left = '-" + i + "0px'", j++ * duration);
        setTimeout(target + ".style.left = '" + i + "0px'", j++ * duration);
    }
}

function checkBrowser() {
    var msieVersion = getIeVersion();

    if (msieVersion != -1 && msieVersion < 8.0) {
        var warnPnl = document.getElementById('not-supported-browser-panel');
        if (warnPnl.style.display == 'none') { warnPnl.style.display = 'block' }
    }
};

// Returns the version of Internet Explorer or -1
// (indicating the use of another browser).
function getIeVersion() {
    var version = -1; // Return value assumes failure.

    if (window.navigator.userAgent.indexOf("MSIE") > 0) {
        var usrAgent = navigator.userAgent;
        var reg = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");

        if (reg.exec(usrAgent) != null) version = parseFloat(RegExp.$1);
    }

    return version;
};

function setvaluefocus(e, isok, id) {
    if (isok == 1) {
        if (e.value == document.getElementById(id).value) {
            e.focus()
            e.value = '';

        }
    }
    else {
        if (e.value == '') {
            e.value = document.getElementById(id).value;
        }
    }
};

function ChangeCapchaCode(imgId) {
    var d = new Date();
    var t_date = d.getDate();      // Returns the day of the month
    var t_mon = d.getMonth();      // Returns the month as a digit
    var t_year = d.getFullYear();  // Returns 4 digit year
    var t_hour = d.getHours();     // Returns hours
    var t_min = d.getMinutes();    // Returns minutes
    var t_sec = d.getSeconds();    // Returns seocnds
    var t_mil = d.getMilliseconds();
    document.getElementById(imgId).src = $("#fullDomainName").val() + '/login/ShowCaptchaImage?v=' + t_date + t_mon + t_year + t_hour + t_min + t_sec + t_mil;
};

function DoLogin() {
    msg.style.display = 'none';
    var isValid = true;
    username.value = username.value.trim();
    if (username.value.length == 0) {
        username.classList.add('err');
        isValid = false;
        shake('username', 5, 100);
    } else if (username.classList != undefined) {
        username.classList.remove('err');
    }

    password.value = password.value.trim();
    if (password.value.length == 0) {
        password.classList.add('err');
        isValid = false;
        shake('password', 5, 100);
    } else if (password.classList != undefined) {
        password.classList.remove('err');
    }

    validate.value = validate.value.trim();
    if (validate.value.length == 0) {
        validate.classList.add('err');
        isValid = false;
        shake('validate', 5, 100);
    } else if (validate.classList != undefined) {
        validate.classList.remove('err');
    }

    if (!isValid) {
        return;
    }

    var loginParams = {
        UserName: username.value,
        UserPwd: password.value,
        Validate: validate.value
    };

    $.ajax({
        type: 'POST',
        url: 'Index',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(loginParams),
        cache: false,
        beforeSend: function (xhr) {
        },
        success: function (result) {
            if (result.IsSuccessful) {
                document.location = $("#fullDomainName").val() + "/Dashboard/ShowDashboard";
            } else {
                innerMsg.innerHTML = result.Message;
                msg.style.display = 'block';
                ChangeCapchaCode($("#fullDomainName").val(), 'imgCaptchaId');
            }
        },
        error: function (xhr, status, err) {
        }
    });
};
