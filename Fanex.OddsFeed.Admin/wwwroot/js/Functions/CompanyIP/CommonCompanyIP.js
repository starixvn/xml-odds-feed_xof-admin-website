$(document).ready(function () {
    $("#newIP").click(function (IPFrom, IPTo) {
        var valid = true;
        valid = ValidateIP('add');

        //add new IP row
        if (valid) {
            addRow(IPFrom, IPTo);
        }
    });
});

function addRow(IPFrom, IPTo, Description) {
    var valid = true;
    valid = ValidateIP('add');

    //add new IP row
    if (valid) {
        var str = '';
        if (IPFrom == "" || IPFrom == undefined || IPTo == "" || IPTo == undefined) {
            str = "<tr><td><input name=\"FromIP\" type=\"text\" /></td><td><input name=\"ToIP\" type=\"text\" /></td><td><input name=\"Description\" type=\"text\" /></td><td><span style=\"padding-left:16px\"></span><span title='Save' class=\"action save\" onclick=\"SaveIPSetting(this,'','')\"  ></span><span style=\"padding-left:30px\"></span><span title='Delete' class=\"action del\" onclick=\"removeRow(this);\"></span></td></tr>";
        } else {
            str = "<tr><td><input name=\"FromIP\" type=\"text\" value=\"" + IPFrom + "\" /></td><td><input name=\"ToIP\" type=\"text\" value=\"" + IPTo + "\" /></td><td><input name=\"Description\" type=\"text\" value=\"" + (Description ? Description : '') + "\" /></td><td><span style=\"padding-left:16px\"></span><span title='Save' class=\"action save\" onclick=\"SaveIPSetting(this,'" + IPFrom + "','" + IPTo + "')\"  ></span><span style=\"padding-left:30px\"></span><span title='Delete' class=\"action del\" onclick=\"removeRow(this,'" + IPFrom + "','" + IPTo + "');\"></span></td></tr>";
        }

        $("#IpTable > tbody").append(str);
        var h = $("#IpDiv").height();
        $("#IpDiv").height(h + 30);
    }
};

function removeRow(obj, IPFromOld, IPToOld) {

    var currentRow = $(obj).closest("tr");
    var columns = $(currentRow).children();

    if (IPFromOld == undefined || IPToOld == undefined) {        
        IPFromOld = $(columns[0]).children()[0].value;
        IPToOld = $(columns[1]).children()[0].value;
    }

    if (IPFromOld != "" && IPToOld != "") {
        
        //check validation
        var valid = ValidateUpdatingIP('saveOneIP', IPFromOld, IPToOld);
        if (!valid) {
            return;
        }
        if (confirm('Do you want to delete this IP range?')) {
            $(".result-message-box").addClass("hidden");

            var description = $(columns[2]).children()[0].value;

            var updateParams = {
                IPFromOld: IPFromOld,
                IPToOld: IPToOld
            };

            $.ajax({
                url: currentDomainURL + 'Company/DeleteCompanyIPSetting',
                type: 'POST',
                cache: false,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(updateParams),
                async: true,
                cache: false,
                success: function (result) {
                    if (result && result.IsSuccessful) {
                        $(".result-message-box").empty()
                            .append("<ul><li>" + result.Message + "</li></ul>")
                            .addClass("success-message-box")
                            .removeClass("error-message-box")
                            .removeClass("hidden");
                    } else {
                        GetErrorMessage(result.ErrorCode);
                    }

                    $(obj).closest("tr").remove();
                    var h = $("#IpDiv").height();
                    $("#IpDiv").height(h - 30);
                    $("#IpTable").height(h - 30);
                    $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 100);
                },
                error: function (xhr, status, err) {
                }
            });
        }
    }
};

function getDataFromTable(id, type) {
    var data = '';
    var fields = $(id).find('input[name=' + type + ']').serializeArray();

    $.each(fields, function (i, field) {
        //        if (field.value.length != 0)
        data += $.trim(field.value) + ',';
    });
    data = data.substr(0, data.length - 1);
    return data;
};

function ValidateIP(action) {
    var valid = true;

    var rows = $('#IpTable tbody tr');
    if (rows.length > 0) {
        var lastRow = rows[rows.length - 1];
        var columns = $(lastRow).children();
        var inputFrom = $(columns[0]).children()[0].value;
        var inputTo = $(columns[1]).children()[0].value;

        //check validation for lastrow
        valid = ValidateUpdatingIP(action, inputFrom, inputTo);

        //        //check maximum number of IP 
        //        if (valid) {
        //            valid = ValidateNumberOfIP(action);
        //        }
    }

    return valid;
}
function SaveIPSetting(obj, IpFromOld, IpToOld) {
    $(".result-message-box").addClass("hidden");
    var valid = true;
    var currentRow = $(obj).closest("tr");

    var columns = $(currentRow).children();
    var IpFrom = $(columns[0]).children()[0].value;
    var IpTo = $(columns[1]).children()[0].value;
    var description = $(columns[2]).children()[0].value;

    //check validation
    valid = ValidateUpdatingIP('saveOneIP', IpFrom, IpTo);

    if (!valid) return;
    var data = JSON.stringify({
        IPFrom: IpFrom,
        IPTo: IpTo,
        IPFromOld: IpFromOld,
        IpToOld: IpToOld,
        Description: description
    });

    $.ajax({
        type: 'POST',
        url: currentDomainURL + 'Company/EditCompanyIPSetting',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: data,
        cache: false,
        success: function (errCode) {
            if (errCode != 0) {
                GetErrorMessage(errCode);
            }
            else {
                OddsFeedAdminManager.informActionResult({
                    IsSuccess: true,
                    ResultMessage: "The IP has been saved successfully!"
                }, $(".result-message-box"));
            }

            $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 100);
        },
        error: function (xhr, status, err) {
        }
    });
};

function ValidateUpdatingIP(action, inputFrom, inputTo) {
    // --- Validate --- //   
    var valid = true;
    var errorMsg = "Error!<br>";

    if (action == 'save') {
        // If IP From and IP To is null, ignore it
        if (inputFrom.length == 0 && inputTo.length == 0) {
            return true;
        }
    } else if (action == 'add' || action == 'saveOneIP') {
        // --- Check if user enter IP from and IP to --- //
        if (inputFrom.length == 0) {
            errorMsg += "Please enter the IP From<br>";
            valid = false;
        }
        if (inputTo.length == 0) {
            errorMsg += "Please enter the IP To<br>";
            valid = false;
        }
    }

    // --- Validate IP String --- ///
    if (valid) {
        if (!ValidateIPString(inputFrom)) {
            valid = false;
            errorMsg += "'" + inputFrom + "' is not a valid IP.<br>";
        }
        if (!ValidateIPString(inputTo)) {
            valid = false;
            errorMsg += "'" + inputTo + "' is not a valid IP.<br>";
        }
    }
    if (valid) {
        // --- Validate IP range --- ///
        var newIPFrom = ConvertIPToUint(inputFrom);
        var newIPTo = ConvertIPToUint(inputTo);
        if (newIPFrom > newIPTo) {
            valid = false;
            errorMsg += " The IP From must be less than IP To.<br>";
        }
    }
    // --- Check if overlap IP --- //
    if (!valid) alert(errorMsg);
    else valid = ValidateIPRange();

    return valid;
}

// --- Check if there is overlap ip range--- //
function ValidateIPRange() {
    var iplist = new Array();
    var arrayIndex = 0;
    $('#IpTable tbody tr').each(function (index) {
        var columns = jQuery(this).children();
        if (columns.length > 1) {
            var inputFrom = jQuery(columns[0]).children()[0].value;
            var inputTo = jQuery(columns[1]).children()[0].value;
            var iprange = new Array();
            iprange[0] = ConvertIPToUint(inputFrom);
            iprange[1] = ConvertIPToUint(inputTo);
            iplist[arrayIndex] = iprange;
            arrayIndex++;
        }
    });

    var valid = true;
    var errorMsg = "";
    // --- Check if there is an overlap ip range --- //
    var errorArr = [];
    for (var i = 0; i < arrayIndex; i++) {
        var iprange = iplist[i];
        for (var j = i + 1; j < arrayIndex; j++) {
            var temprange = iplist[j];
            // --- case Start2< Start1< End1<End2 --- //
            // --- case Start2< Start1< End2<End1 --- //
            if (iprange[0] >= temprange[0] && iprange[0] <= temprange[1]) {
                errorArr.push("IP Range " + ConvertUintToIP(iprange[0]) + ' -> ' + ConvertUintToIP(iprange[1])
                               + " is overlap with IP Range "
                               + ConvertUintToIP(temprange[0]) + ' -> ' + ConvertUintToIP(temprange[1]));
                valid = false;
            }
            // --- case Start1< Start2< End1<End2 --- //
            // --- case Start1< Start2< End2<End1 --- //
            else if (temprange[0] >= iprange[0] && temprange[0] <= iprange[1]) {
                errorArr.push("IP Range " + ConvertUintToIP(iprange[0]) + ' -> ' + ConvertUintToIP(iprange[1])
                               + " is overlap with IP Range "
                               + ConvertUintToIP(temprange[0]) + ' -> ' + ConvertUintToIP(temprange[1]));
                valid = false;
            }
        }
    }

    if (!valid) {
        alert(errorArr.join("<br />"));
    }

    return valid;

}

function ValidateIPString(ipaddr) {
    //Remember, this function will validate only Class C IP.
    //change to other IP Classes as you need
    ipaddr = ipaddr.replace(/\s/g, "");  //remove spaces for checking
    var re = /^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/; //regex. check for digits and in
    //all 4 quadrants of the IP
    if (re.test(ipaddr)) {
        //split into units with dots "."
        var parts = ipaddr.split(".");
        //if the first unit/quadrant of the IP is zero
        if (parseInt(parseFloat(parts[0])) == 0) {
            return false;
        }
        //if the fourth unit/quadrant of the IP is zero
        if (parseInt(parseFloat(parts[3])) == 0) {
            return false;
        }
        //if any part is greater than 255
        for (var i = 0; i < parts.length; i++) {
            if (parseInt(parseFloat(parts[i])) > 255) {
                return false;
            }
        }
        return true;
    } else {
        return false;
    }
}

function ConvertUintToIP(ip) {
    var d = ip % 256;
    ip = (ip - d) / 256;
    var c = ip % 256;
    ip = (ip - c) / 256;
    var b = ip % 256;
    ip = (ip - b) / 256;
    var a = ip;
    return a + '.' + b + '.' + c + '.' + d;
}

function ConvertIPToUint(ip) {
    ip = ip.trim();
    var data = ip.split('.');
    if (data.length == 4) {
        var a = Number(data[0]) * 256 * 256 * 256;
        var b = Number(data[1]) * 256 * 256;
        var c = Number(data[2]) * 256;
        var d = Number(data[3]);
        return a + b + c + d;
    }
    return 0;
}

function GetErrorMessage(errCode) {
    $.ajax({
        url: "GetMessage",
        type: "POST",
        data: "ErrCode=" + errCode,
        success: function (result) {
            OddsFeedAdminManager.informActionResult({
                IsSuccess: false,
                ResultMessage: result
            }, $(".result-message-box"));
        }
    });
}

function getDataFromTable_SingleLineText(id, type) {
    var data = '';
    var fields = $(id).find('input[name=' + type + ']').serializeArray();
    $.each(fields, function (i, field) {
        data += (field.value == "" ? "\n" : $.trim(field.value)) + '\n';
    });

    data = data.substr(0, data.length - 1);
    return data;
};
