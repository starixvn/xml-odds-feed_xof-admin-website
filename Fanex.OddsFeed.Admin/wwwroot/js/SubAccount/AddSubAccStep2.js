$(document).ready(function () {

    $("#step2Previous").click(function () {
        $("div.step-active").attr("class", "step-inactive");
        $("#panelStep1").attr("class", "step-active");

        $("#errorStep2").addClass("hidden");
        $(".licenseeList").removeClass("error");
        $("div.step").hide();
        $("#step1").show();
    });

    $("#step2Next").click(function () {
        var ret = CheckValidation_Step2();
        if (ret) {
            $("div.step-active").attr("class", "step-inactive");
            $("#panelStep3").attr("class", "step-active");

            $("div.step").hide();
            $("#step3").show();
        }
    });
});

//---------------------------------------------------------------------------------------------
function CheckValidation_Step2() {
    var ret = true;

    // validate licensee list
    if ($('.chkSystem:checked').length > 0) {
        $("#licenseeReqErr").addClass("hidden");
    } else {
        ret = false;
        $("#licenseeReqErr").removeClass("hidden");
    }

    if (ret) {
        $("#errorStep2").addClass("hidden");
        $(".licenseeList").removeClass("error");
    } else {
        $("#errorStep2").removeClass("hidden");
        $(".licenseeList").addClass("error");
    }

    return ret;
}


