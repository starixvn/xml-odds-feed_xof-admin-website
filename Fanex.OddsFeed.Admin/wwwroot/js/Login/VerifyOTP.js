let msg, otp, btnSubmit, innerMsg;

window.onload = function () {
    setTimeout('window.history.forward()', 0);

    msg = document.getElementById('msg');
    innerMsg = document.getElementById('inner-msg');
    otp = document.getElementById('otp');
    btnSubmit = document.getElementById('submit');

    otp.focus();

    let signIn = function () {
        $.ajax({
            type: 'POST',
            url: 'signIn',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            cache: false,
            success: function () {
                document.location = currentDomainURL + "Dashboard/ShowDashboard"; 
            }
        });
    };

    let VerifyOTP = function () {
        msg.style.display = 'none';
        let isValid = true;

        otp.value = otp.value.trim();
        if (otp.value.length == 0) {
            otp.classList.add('err');
            isValid = false;
            shake('otp', 5, 100);
        } else if (otp.classList != undefined) {
            otp.classList.remove('err');
        }
    
        if (!isValid) {
            return;
        }

        const params = {
            otp: otp.value,
        };

        $.ajax({
            type: 'POST',
            url: 'VerifyOTP',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            cache: false,
            success: function (result) {
                if (result.isSuccessful) {
                    signIn();
                } else {
                    innerMsg.innerHTML = result.message;
                    msg.style.display = 'block';
                }
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

    otp.onkeyup = function (e) {
        if (e && e.keyCode == 13) {
            VerifyOTP();
        }
    };

    btnSubmit.onclick = function () {
        VerifyOTP();
    };
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
