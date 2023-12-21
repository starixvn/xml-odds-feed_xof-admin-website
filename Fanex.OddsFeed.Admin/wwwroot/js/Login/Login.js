let msg, username, password, btnSubmit, validate, innerMsg;

window.onload = function () {
    setTimeout('window.history.forward()', 0);

    msg = document.getElementById('msg');
    innerMsg = document.getElementById('inner-msg');
    username = document.getElementById('username');
    password = document.getElementById('password');
    validate = document.getElementById('validate');
    btnSubmit = document.getElementById('submit');

    username.focus();

    let DoLogin = function () {
        msg.style.display = 'none';
        let isValid = true;
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
    
        const url = `Index?UserName=${username.value}&UserPwd=${password.value}&Validate=${validate.value}`;
    
        $.ajax({
            type: 'POST',
            url: url,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function (result) {
                if (result.isSuccessful) {
                    document.location = currentDomainURL + "Dashboard/ShowDashboard";
                } else {
                    innerMsg.innerHTML = result.message;
                    msg.style.display = 'block';
                    getCaptcha();
                }
            },
            error: function (xhr, status, err) {
            }
        });
    };

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

    $('#refreshCaptcha').on('click', function() {
        const d = new Date();
        const t_date = d.getDate();      // Returns the day of the month
        const t_mon = d.getMonth();      // Returns the month as a digit
        const t_year = d.getFullYear();  // Returns 4 digit year
        const t_hour = d.getHours();     // Returns hours
        const t_min = d.getMinutes();    // Returns minutes
        const t_sec = d.getSeconds();    // Returns seocnds
        const t_mil = d.getMilliseconds();
        document.getElementById('imgCaptchaId').src = currentDomainURL + 'login/ShowCaptchaImage?v=' + t_date + t_mon + t_year + t_hour + t_min + t_sec + t_mil;
    });
};


String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
};

function shake(target, times, duration) {
    for (let i = times, j = 0; i >= 0; i--) {
        setTimeout(target + ".style.left = '-" + i + "0px'", j++ * duration);
        setTimeout(target + ".style.left = '" + i + "0px'", j++ * duration);
    }
}
