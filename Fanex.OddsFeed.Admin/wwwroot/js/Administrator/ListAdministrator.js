var gridOptions
    ,_pageIndex = 0
    ,_pageSize = 10;
$(document).ready(function () {
    $("#Username").Watermark("Username");

    $("#Username").autocomplete({
        source: [],
        minLength: 1
    });

    $("#Username").keyup(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            Search();
        }
    });

    Search();
});

function updateTips(t) {
    $(".validateTips")
    .text(t)
    .addClass("ui-state-highlight");
    setTimeout(function () {
        $(".validateTips").removeClass("ui-state-highlight", 1500);
    }, 500);
}

function checkRequired(o, n) {
    if (o.val() == undefined || o.val().length <= 0) {
        o.addClass("ui-state-error");
        updateTips("Please enter " + n + ".");
        return false;
    } else {
        return true;
    }
}

function checkLength(o, n, min, max) {
    if (o.val().length > max || o.val().length < min) {
        o.addClass("ui-state-error");
        updateTips("Length of " + n + " must be between " + min + " and " + max + ".");
        return false;
    } else {
        return true;
    }
}

function checkRegexp(o, regexp, n) {
    if (!(regexp.test(o.val()))) {
        o.addClass("ui-state-error");
        updateTips(n);
        return false;
    } else {
        return true;
    }
}

function renderBodyRows(users) {
    var rows = [];
    var totalRows = users.length;
    for (var i = 0; i < totalRows; i++) {
        var user = users[i];
        var colactive = '';
        if (user.IsActive) {
            colactive = "<a title='Deactivate this account' class='ico-tick' onclick='javascript:editActive(this, " + JSON.stringify({ userId: user.Id, username: user.Username }) + ");' />";
        } else {
            colactive = "<a title='Activate this account' class='ico-untick' onclick='javascript:editActive(this, " + JSON.stringify({ userId: user.Id, username: user.Username }) + ");' />";
        }

        var row = {
            Columns: [
                { Name: 'Row', Value: user.Row },
                { Name: 'Username', Value: user.Username },
                {
                    Name: 'Action',
                    Value: "<div class='btn-action' onclick='javascript:ShowAction(" + JSON.stringify(user) + ")'></div>"
                },
                { Name: 'Isactive', Value: colactive },
                { Name: 'Rolename', Value: user.RoleName },
                { Name: 'Firstname', Value: user.FirstName },
                { Name: 'Lastname', Value: user.LastName },
                { Name: 'Email', Value: user.Email },
                { Name: 'Creator', Value: user.Creator },
                { Name: 'Createdate', Value: user.CreateDate }
            ]
        };

        rows.push(row);
    }

    return rows;
}

function Search() {
    $('#PageSize').val(_pageSize);
    $('#PageIndex').val(_pageIndex);

    var searchParams = JSON.stringify({
        SystemID: parseInt($("#SystemID option:selected").val(), 10),
        Username: $("#Username").val(),
        IsActive: parseInt($("#isActive option:selected").val()),
        RoleID: parseInt($("#RoleID option:selected").val()),
        PageSize: _pageSize,
        PageIndex: _pageIndex
    });

    $.ajax({
        url: currentDomainURL + "Administrator/GetListAdminBySystemAndNameAndRole",
        type: 'POST',
        cache: false,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: searchParams,
        async: true,
        success: function (result) {
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 40, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    {
                        HeaderText: "Username", Width: 100, Name: "Username", AllowTruncate: true,
                        TdClass: "boldusername", CssClass: "boldusername", HeaderAlign: "center", CellAlign: "left"
                    },
                    { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Active", Width: 65, Name: "Isactive", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt", Sort: true },
                    { HeaderText: "Role", Width: 120, Name: "Rolename", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true, CssClass: "header-mt", Sort: true },
                    { HeaderText: "First Name", Width: 100, Name: "Firstname", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Last Name", Width: 100, Name: "Lastname", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Email", Width: 170, Name: "Email", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Creator", Width: 110, Name: "Creator", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true, CssClass: "header-mt", Sort: true },
                    {
                        HeaderText: "Created Date", Width: 120, Name: "Createdate", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt",
                        Sort: true, FormatDateTime: 'DD MMM YYYY HH:mm:ss', CustomSortFunction: function (col1Value, col2Value, sortDirection) {
                            if (sortDirection === 'asc') {
                                return moment(col1Value).diff(col2Value);
                            } else {
                                return moment(col2Value).diff(col1Value);
                            }
                        }
                    }
                ],
                bodyRows: renderBodyRows(result.data),
                hideYBackGroundScroller: true,
                gridExpandHeight: 250,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: _pageSize,
                    currentPage: _pageIndex,
                    totalItem: result.totalItems,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                },
                sortOption: {
                    afterGridSorted: function (bodyRows, columnName, currentDirection) {
                        $("#SearchResult").find('table#tblContent_SearchResult td.free-grid-td-0').each(function (index, item) {
                            $(item).find('div.table-cell').html(index + 1);
                        });

                        return bodyRows;
                    }
                }
            };

            $("#SearchResult").fanexGrid(gridOptions);
        }
    });
}

function ShowAction(user) {
    _adminId = user.Id;
    _adminName = user.Username;

    $.contextMenu('destroy');

    if (user.RoleID === 1) { //ITAdmin role
        $.contextMenu({
            selector: '.btn-action',
            items: {
                editInfo: {
                    name: "Edit information",
                    height: 25,
                    className: "iconEditInfo",
                    callback: function (key, opt) {
                        var paramsString = $.param({
                            username: _adminName
                        });
                        window.location = currentDomainURL + "Administrator/AdministratorEditProfile/" + _adminId + "?" + paramsString;
                    }
                },
                resetPassword: {
                    name: "Reset password",
                    height: 25,
                    className: "iconResetPwd",
                    callback: function (key, opt) {
                        var paramsString = $.param({
                            username: _adminName,
                            role: user.RoleID
                        });
                        window.location = currentDomainURL + "Administrator/AdministratorResetPwd/" + _adminId + "?" + paramsString;
                    }
                }
            },
            trigger: "left",
            ignoreRightClick: true,
            autoHide: true
        });
    } else {//Licensse role
        $.contextMenu({
            selector: '.btn-action',
            items: {
                editSystem: {
                    name: "Assign licensee",
                    height: 25,
                    className: "iconEditSystem",
                    callback: function (key, opt) {
                        var paramsString = $.param({
                            username: _adminName,
                            role: user.RoleID
                        });
                        window.location = currentDomainURL + "Administrator/AdministratorEditSystem/" + _adminId + "?" + paramsString;
                    }
                },
                editInfo: {
                    name: "Edit information",
                    height: 25,
                    className: "iconEditInfo",
                    callback: function (key, opt) {
                        var paramsString = $.param({
                            username: _adminName
                        });
                        window.location = currentDomainURL + "Administrator/AdministratorEditProfile/" + _adminId + "?" + paramsString;
                    }
                },
                resetPassword: {
                    name: "Reset password",
                    height: 25,
                    className: "iconResetPwd",
                    callback: function (key, opt) {
                        var paramsString = $.param({
                            username: _adminName,
                            role: user.RoleID
                        });

                        window.location = currentDomainURL + "Administrator/AdministratorResetPwd/" + _adminId + "?" + paramsString;
                    }
                }
            },
            trigger: "left",
            ignoreRightClick: true,
            autoHide: true
        });
    }
}

function ReRenderAfterPaged(page_index, itemsPerPage, jq) {
    if (itemsPerPage != $('#PageSize').val()) {
        $('#PageIndex').val(0);
        _pageIndex = 0;
    } else {
        $('#PageIndex').val(page_index);
        _pageIndex = page_index
    }

    $('#PageSize').val(itemsPerPage);
    _pageSize = itemsPerPage;

    gridOptions.pagerOption.currentPage = page_index;
    Search();

    return false;
}

function GetErrorMessage(errCode) {
    $.ajax({
        url: "GetMessage",
        type: "POST",
        data: "ErrCode=" + errCode,
        success: function (result) {
            return updateTips(result);
        }
    });
}

function StartAutoSuggesttion() {
    var urlParam = $.param({
        username: $("#Username").val()
    });

    var url = 'ListExistedAdministrator?' + urlParam;
    $.ajax({
        url: url,
        type: 'POST',
        dataType: 'html',
        cache: false,
        success: function (result) {
            var sourceAdmin = result.split(',');
            $("#Username").autocomplete({
                source: sourceAdmin,
                minLength: 1
            });
        },
        error: function (xhr, status, err) {
        }
    });
}