var nickName;
$(document).ready(function () {
//    $("#FromDate, #ToDate").datepicker({
//        changeMonth: true,
//        changeYear: true
//    });

    $("#Employee").autocomplete({
        source: function (request, response) {
            $.ajax({
                dataType: 'json',
                url: _global.root + 'api/employee/getemployees',
                data: {
                    query: request.term,
                    $top: 10
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.NickName + ' (' + item.FullName + ')',
                            value: item.Id
                        }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            nickName = ui.item.label;
            $('#EmpId').val(ui.item.value);
        },
        focus: function (event, ui) {
            nickName = ui.item.label;
            $('#Employee').val(nickName);
            $('#EmpId').val(ui.item.value);
            event.preventDefault();
        },
        change: function (event, ui) {
            nickName = ui.item.label;
            $('#Employee').val(nickName);
            $('#EmpId').val(ui.item.value);
        },
        close: function () {
            $('#Employee').val(nickName);
        }
    });

    $('input[type=text]').keypress(function (evt) {
        if (evt.keyCode == 13) {
            view();
        }
    });
    $('#view').click(function (evt) {
        evt.preventDefault();
        view();
    });

    view();
});

function view() {
    var $form = $('form');
    if (!$form.valid()) return;

    actingButton('#view', true);

    //if no user is selected, clear the empId also
    if ($('#Employee').val() == '') $('#EmpId').val('');
    $('#nodata').remove();
    $.getJSON("/api/report/getemployeeattendances", $('form').serialize(),function (response) {
        if (response.length > 0) {
            rowIndex = 1;
            $('#data tbody').children().remove();
            $('#itemTemplate').tmpl(response).appendTo('#data');

        }
        else {
            $('#data tbody').children().remove();
            $(noInformation(13)).appendTo('#data');
        }
        reloadButtonText('#view');
    }).error(function () {
        reloadButtonText('#view');
        alert(_messages.errorTryAgain);
    });
}

var rowIndex = 1;
function index() {
    return rowIndex++;
}
