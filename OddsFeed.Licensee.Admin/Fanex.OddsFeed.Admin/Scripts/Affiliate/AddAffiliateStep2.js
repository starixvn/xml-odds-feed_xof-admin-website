$(document).ready(function () {

    $("#addNewIP").click(function () {
        //validate IP
        var valid = OddsFeedAdminManager.validateIP('add');

        //check maximum number of IP 
        if (valid) {
            var numberOfIPs = $("#numberOfIPs").val();
            valid = OddsFeedAdminManager.validateNumberOfIP('add', numberOfIPs, 'IpTable');        
        }

        //add new IP row
        if (valid) {
            $("#IpTable > tbody").append("<tr><td><input name=\"IP\" type=\"text\" /></td><td><input name=\"IP\" type=\"text\" /></td> <td></span><span style=\"padding-left:30px\"></span><span title=\"Delete\" class=\"action del\" onclick=\"removeRowTemporary(this);\"></span></td></tr>");
            var h = $("#IpDiv").height();
            $("#IpDiv").height(h + 30);
        }
    });

    $("#IpTable").height(60);
    $("#IpDiv").height(60);

    $("#step2Previous").click(function () {
        $("div.step-active").attr("class", "step-inactive");
        $("#panelStep1").attr("class", "step-active");

        $("div.step").hide();
        HideView_AssignSetting();
        $("#step1").show();
    });

    $("#step2Next").click(function () {
        $("#errorStep2").addClass("hidden");

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

        $("div.step-active").attr("class", "step-inactive");
        $("#panelStep3").attr("class", "step-active");

        $("div.step").hide();
        $("#step3").show();
    });
});

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
function HideView_AssignSetting() {
    $('#slAssignedDomain').removeClass('error');
    $("#errorStep2").addClass("hidden");
};