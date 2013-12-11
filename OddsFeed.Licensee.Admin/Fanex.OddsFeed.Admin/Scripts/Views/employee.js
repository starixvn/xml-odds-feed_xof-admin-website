$(document).ready(function () {
    $('#keyword').keyup(function (event) {
        if (event.keyCode == 13) {
            query();
        }
    });

    var q = getParam('query');
    if (q != null) {
        $('#keyword').val(q);
    }

    //search onload
    query();

    $('#query').click(function (evt) {
        evt.preventDefault();
        query();
    });

});

function query() {

    var query = $('#keyword').val();
    actingButton('#query', true);

    if ($("#enable").attr("checked")) {
        enable = true;
    }
    else {
        if ($("#active").attr("checked")) {
            enable = false;
        }
        else {
            enable = null;
        }
    }

    var isDelete = null;
    if ($("#IsDeleted").attr("checked")) {
        isDelete = true;
    }
    else {
        isDelete = null;
    }

    console.log(_global.root);

    $.getJSON(
    _global.root + 'api/employee/getemployees'

    , { query: query, groupId: $('#group').val(), enable: enable, isDelete: isDelete }
    , function (data) {
        console.log(data);
        if (data.length > 0) {
            rowIndex = 1;

            $('#data tbody').children().remove();
            $('#itemTemplate').tmpl(data).appendTo('#data');

            $('.nick').tooltip({
                track: true,
                delay: 0,
                showURL: false,
                bodyHandler: function () {
                    return '<img src="/employee/avatar/' + $(this).attr('data-code') + '" />';
                }
            });

        }
        else {
            $('#data tbody').children().remove();
            $(noInformation(12)).appendTo('#data');
        }
        reloadButtonText('#query');
    }).error(function () {
        reloadButtonText('#query');
        alert(_messages.errorTryAgain);
    });
    $('#keyword').focus().select();
}

var rowIndex = 1;
function index() {
    return rowIndex++;
}