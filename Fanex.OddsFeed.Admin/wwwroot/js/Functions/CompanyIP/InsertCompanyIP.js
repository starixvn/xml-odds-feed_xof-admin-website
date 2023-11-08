$(document).ready(function () {

    var listIPFrom = $('#ListIPFrom').val();
    var listIPTo = $('#ListIPTo').val();
    var listDescription = $('#ListDescription').val();

    if (!(listIPFrom == "" || listIPFrom == undefined || listIPTo == "" || listIPTo == undefined)) {
        $('#IpTable').find('tbody').find('tr').remove();
        listIPFrom = listIPFrom.split(',');
        listIPTo = listIPTo.split(',');
        listDescription = listDescription.split('\n');
        for (var i = 0; i < listIPFrom.length; i++) {
            addRow(listIPFrom[i], listIPTo[i], listDescription[i]);
        }
    }

    addRow();

    //customize height of Ipdiv    
    $("#IpDiv").height($("#IpTable").height());

    $("#Save").click(function () {
        $(".result-message-box").addClass("hidden");
        //check maximum number of IP
        var valid = ValidateIP('save');
        if (!valid) return;

        // IP
        $('#ListIPFrom').val(getDataFromTable("#IpTable", "FromIP"));
        $('#ListIPTo').val(getDataFromTable("#IpTable", "ToIP"));
        $('#ListDescription').val(getDataFromTable_SingleLineText("#IpTable", "Description"));

        var data = JSON.stringify({
            ListIPFrom: $('#ListIPFrom').val(),
            ListIPTo: $('#ListIPTo').val(),
            ListDescription: $('#ListDescription').val()
        });

        $.ajax({
            type: 'POST',
            url: currentDomainURL + 'Company/EditCompanyIPs',
            contentType: "application/json; charset=utf-8",
            data: data,
            dataType: 'json',
            cache: false,
            success: function (errCode) {
                if (errCode != 0) {
                    GetErrorMessage(errCode);
                }
                else {
                    OddsFeedAdminManager.informActionResult({
                        IsSuccess: true,
                        ResultMessage: "The IPs have been saved successfully!"
                    }, $(".result-message-box"));
                }

                $('.content_scroll').eq(0).animate({ scrollTop: 0 }, 100);
            },
            error: function (xhr, status, err) {
            }
        });

    });

    $("#Cancel").click(function () {
        if (confirm('Do you want to exit for editing Company IP?')) {
            document.location = currentDomainURL + "Dashboard/ShowDashboard";
        }
    });
});
//--------------------------------------------------------------------------------------------------------------------