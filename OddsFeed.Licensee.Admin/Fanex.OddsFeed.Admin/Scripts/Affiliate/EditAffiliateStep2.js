$(document).ready(function () {

    showingDomainList();

    //customize height of Ipdiv    
    $("#IpDiv").height($("#IpTable").height());

    $("#newIP").click(function () {
        //validate IP
        var valid = OddsFeedAdminManager.validateIP('add');

        //check maximum number of IP 
        if (valid) {
            var numberOfIPs = $("#numberOfIPs").val();
            valid = OddsFeedAdminManager.validateNumberOfIP('add', numberOfIPs, 'IpTable');          
        }

        //add new IP row
        if (valid) {
            $("#IpTable > tbody").append("<tr><td><input name=\"IP\" type=\"text\" /></td><td><input name=\"IP\" type=\"text\" /></td> <td><span style=\"padding-left:25px\"></span><span title=\"Save\" class=\"action save\" onclick=\"SaveIPSetting(this,'','')\"  ></span><span style=\"padding-left:30px\"></span><span title=\"Delete\" class=\"action del\" onclick=\"removeRow(this,'','');\"></span></td></tr>");
            var h = $("#IpDiv").height();
            $("#IpDiv").height(h + 30);
        }
    });

    $("#editstep2Save").click(function () {
        $("#editStep2 .result-message-box").addClass("hidden");

        //validate IP        
        var valid = OddsFeedAdminManager.validateIP('save');

        //check maximum number of IP 
        if (valid) {
            var numberOfIPs = $("#numberOfIPs").val();
            valid = OddsFeedAdminManager.validateNumberOfIP('save', numberOfIPs, 'IpTable');          
        }

        if (!valid) return;

        valid = CheckValidation_AssignSetting();
        if (!valid) return;

        //step2
        var affID = $("#affID").val();
        var selectedOptions = $('#slAssignedDomain option');
        var selectedDomains = $.map(selectedOptions, function (option) {
            return option.value;
        }).join(',');
        var IPs = getDataFromTable("#IpTable");
        var updateParams = {
            affId: affID,
            domains: selectedDomains,
            listIPs: IPs.join(",")
        };

        $.ajax({
            type: 'POST',
            url: 'EditSetting',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(updateParams),
            cache: false,
            beforeSend: function (xhr) {
            },
            success: function (result) {
                var success = result != null && result.IsSuccess == true;
                OddsFeedAdminManager.informActionResult(result, $("#editStep2 .result-message-box"));
                if (success) {
                    OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + "/Affiliate/List");
                }
            },
            error: function (xhr, status, err) {
            }
        });

    });
});
//--------------------------------------------------------------------------------------------------------------------

function SaveIPSetting(obj, IpFromOld, IpToOld) {
    //    DoUpdateIPRange_EditSetting();

    $("#editStep2 .result-message-box").addClass("hidden");
    $(".success").addClass("hidden");
    $("#errorStep2").addClass("hidden");
    $('#slAssignedDomain').removeClass('error');

    var currentRow = $(obj).closest("tr");

    //validate IP    
    var valid = OddsFeedAdminManager.validateIP('save');
    
    //check maximum number of IP 
    if (valid) {
        var numberOfIPs = $("#numberOfIPs").val();
        valid = OddsFeedAdminManager.validateNumberOfIP('save', numberOfIPs, 'IpTable');
    }

    if (!valid) return;

    var affID = $("#affID").val();

    var columns = $(currentRow).children();
    var IpFrom = $(columns[0]).children()[0].value;
    var IpTo = $(columns[1]).children()[0].value;

    var url = currentDomainURL + '/Affiliate/EditIPSetting?affId=' + affID + '&fromIP=' + IpFrom + '&toIP=' + IpTo + '&fromIPOld=' + IpFromOld + '&toIPOld=' + IpToOld //step2

    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        beforeSend: function (xhr) {
        },
        success: function (result) {
            result = result.toLowerCase();
            if (result == 'successfully!') {
                OddsFeedAdminManager.informActionResult({
                    IsSuccess: true,
                    ResultMessage: "The IP has been saved successfully!"
                }, $("#editStep2 .result-message-box"));
            }
            else {
                $("#editStep2 .result-message-box").addClass("hidden");
            }
        },
        error: function (xhr, status, err) {
        }
    });

    $(columns[2]).find(".action.save").attr("onclick", "SaveIPSetting(this,'" + IpFrom + "','" + IpTo + "');");
    $(columns[2]).find(".action.del").attr("onclick", "removeRow(this,'" + IpFrom + "','" + IpTo + "');");
};


function CheckValidation_AssignSetting() {
    var res = true;
    if ($("#slAssignedDomain option").length == 0) {
        $("#errorStep2").empty().append("<ul><li>" + viewResources.Error_Require_AssignedDomain + "</li></ul>");
        $("#errorStep2").show('fast');
        $('#slAssignedDomain').addClass('error');
        res = false;
    } else {
        $("#errorStep2").addClass("hidden");
        $('#slAssignedDomain').removeClass('error');
    }

    if (!res) {
        $("#errorStep2").removeClass("hidden");
    } else {
        $("#errorStep2").addClass("hidden");
    }

    return res;
};