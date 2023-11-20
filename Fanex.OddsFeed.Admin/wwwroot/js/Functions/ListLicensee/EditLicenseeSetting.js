$(document).ready(function () {

    showingDomainList();
    var listIPFrom = $('#ListIPFrom').val();
    var listIPTo = $('#ListIPTo').val();
    $("#errorStep2_NumberOfIPs").removeClass("validation-summary-errors");
    $('#IpTable').removeClass('error');

    if (!(listIPFrom == "" || listIPFrom == undefined || listIPTo == "" || listIPTo == undefined)) {
        $('#IpTable').find('tbody').find('tr').remove();
        listIPFrom = listIPFrom.split(',');
        listIPTo = listIPTo.split(',');
        for (var i = 0; i < listIPFrom.length; i++) {
            addRow(listIPFrom[i], listIPTo[i]);
        }
    }

    //customize height of Ipdiv    
    $("#IpDiv").height($("#IpTable").height());

    $("#Save").click(function () {
        $(".result-message-box").addClass("hidden");

        //validate IP
        var valid = OddsFeedAdminManager.validateIP('save');

        //check maximum number of IP 
        if (valid) {
            var numberOfIPs = OddsFeedAdminManager.getMaximumNumberOfIPs();
            valid = OddsFeedAdminManager.validateNumberOfIP('save', numberOfIPs, 'IpTable');           
        }

        if (!valid) return;

        valid = CheckValidation_Step2();
        if (!valid) return;

        var selectedDomains = $.map($('#AssignedDomainList option'), function (option) {
            return option.value;
        }).join(',');
        $('#DomainList').val(selectedDomains);

        // IP
        $('#ListIPFrom').val(getDataFromTable("#IpTable", "FromIP"));
        $('#ListIPTo').val(getDataFromTable("#IpTable", "ToIP"));

        //step2
        var affID = $("#affID").val();
        var selectedOptions = $('#slAssignedDomain option');
        var selectedDomains = $.map(selectedOptions, function (option) {
            return option.value;
        }).join(',');
        var data = {
            SystemID: parseInt($('#formEditLicenseeSetting #SystemID').val()),
            DomainList: $('#formEditLicenseeSetting #DomainList').val(),
            ListIPFrom: $('#formEditLicenseeSetting #ListIPFrom').val(),
            ListIPTo: $('#formEditLicenseeSetting #ListIPTo').val()
        };
        var IPs = getDataFromTable("#IpTable");
        $.ajax({
            url: 'EditSettingPost',
            type: 'POST',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: 'html',
            async: true,
            cache: false,
            success: function (errCode) {
                if (errCode != 0) {
                    GetErrorMessage(errCode);
                }
                else {
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: true,
                        ResultMessage: "The licensee's information has been updated successfully!"
                    }, $(".result-message-box"));
                    OddsFeedAdminManager.confirmAfterSuccess(currentDomainURL + "Licensee/ListLicensee");
                }
            },
            error: function (xhr, status, err) {
            }
        });

    });

    $("#Cancel").click(function () {
        confirm('Are you sure to cancel edit licensee?', function () {
            document.location = currentDomainURL + "Licensee/ListLicensee";
        });
    });
});
//--------------------------------------------------------------------------------------------------------------------

function CheckValidation_Step2() {
    var res = true;
    if ($("#AssignedDomainList option").length == 0) {
        $("#errorStep2").empty().append("<ul><li>The Assigned Domains field is required.</li></ul>");
        $("#errorStep2").show('fast');
        $('#AssignedDomainList').addClass('error');
        res = false;
    } else {
        $("#errorStep2").addClass("hidden");
        $('#AssignedDomainList').removeClass('error');
    }

    if (!res) {
        $("#errorStep2").removeClass("hidden");
    } else {
        $("#errorStep2").addClass("hidden");
    }

    return res;
};















