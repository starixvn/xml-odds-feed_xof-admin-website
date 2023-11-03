$(document).ready(function () {
    $('#addNewIP').click(function () {
        // validate IP
        var valid = OddsFeedAdminManager.validateIP('add');

        //check maximum number of IP 
        if (valid) {
            var numberOfIPs = OddsFeedAdminManager.getMaximumNumberOfIPs();
            valid = OddsFeedAdminManager.validateNumberOfIP('add', numberOfIPs, 'IpTable');
        }

        // add new IP row
        if (valid) {
            $('#IpTable > tbody').append("<tr><td><input name=\"FromIP\" type=\"text\" /></td><td><input name=\"ToIP\" type=\"text\" /></td> <td></span><span style=\"padding-left:30px\"></span><span title=\"Delete\" class=\"action del\" onclick=\"removeRowTemporary(this);\"></span></td></tr>");
            var h = $('#IpDiv').height();
            $('#IpDiv').height(h + 30);
        }
    });

    $('#IpTable').height(60);
    $('#IpDiv').height(70);

    $('#step2Previous').click(function () {
        $('div.step-active').attr('class', 'step-inactive');
        $('#panelStep1').attr('class', 'step-active licensee-info-settings');

        $('div.step').hide();
        $('#step1').show();
        $('#errorStep2').addClass('hidden');
        $('#AssignedDomainList').removeClass('error');
    });

    $('#step2Next').click(function () {
        // validate IP
        var valid = OddsFeedAdminManager.validateIP('save');

        // check maximum number of IP 
        if (valid) {
            var numberOfIPs = OddsFeedAdminManager.getMaximumNumberOfIPs();
            valid = OddsFeedAdminManager.validateNumberOfIP('save', numberOfIPs, 'IpTable');
        }

        if (!valid) return;

        valid = CheckValidation_Step2();
        if (!valid) return;

        $('div.step-active').attr('class', 'step-inactive');
        $('#panelStep3').attr('class', 'step-active');

        $('div.step').hide();
        $('#step3').show();

    });
});

function CheckValidation_Step2() {
    var res = true;
    if ($('#AssignedDomainList option').length == 0) {
        $('#errorStep2').empty().append('<ul><li>The Assigned Domains field is required.</li></ul>');
        $('#errorStep2').show('fast');
        $('#AssignedDomainList').addClass('error');
        res = false;
    } else {
        $('#errorStep2').addClass('hidden');
        $('#AssignedDomainList').removeClass('error');
    }

    if (!res) {
        $('#errorStep2').removeClass('hidden');
    } else {
        $('#errorStep2').addClass('hidden');
    }

    return res;
}