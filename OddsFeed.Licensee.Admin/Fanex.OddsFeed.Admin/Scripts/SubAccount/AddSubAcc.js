$(document).ready(function () {
    //Process redirect to SubAccount list.
    var successed = $("#successed").val();
    if (successed == "true") {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location = currentDomainURL + "/SubAccount/List";
        });
    }

    //step1
    $('#step1 input[type="text"]').val('');

    //step2
    $('#step2 input[type="text"]').val('');
    $('#step2 input[type="checkbox"]').attr("checked", true); //check all systems when adding new sub-account    
    $("#chkAllSystem").click(function () {
        if ($('#chkAllSystem').is(":checked")) {
            $('.chkSystem').each(function () { this.checked = true; });
        } else {
            $('.chkSystem').each(function () { this.checked = false; });
        }
    });

    $(".chkSystem").click(function () {
        if ($('.chkSystem:checked').length == $('.chkSystem').length) {
            $('#chkAllSystem').attr("checked", true);
        } else {
            $('#chkAllSystem').attr("checked", false);
        }
    });

    //step3    
    $('#step3 input[type="checkbox"]').attr("checked", true); //check all permission flags when adding new sub-account    

    //cancel
    $(".cancel").click(function () {
        confirm('Are you sure to cancel adding new sub-account?', function () {
            document.location = currentDomainURL + "/SubAccount/List";
        });
    });

});