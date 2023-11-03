var _subAccId = 0;
var _subAccName = "";
var gridOptions,
    _pageIndex = 0,
    _pageSize = 10;


$(document).ready(function () {
    var successed = $("#successed").val();
    if (successed == "true") {
        confirm('Successfull!\nPress OK to continue', function () {
            document.location = currentDomainURL + "SubAccount/List";
        });
    }

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

//--------------------------------------------------------------------------------------------------------------------
function OnSearch() {
    _pageIndex = 0;
    _pageSize = 10;

    Search();
};

function renderBodyRows(users) {
    var rows = [];
    var totalRows = users.length;
    for (var i = 0; i < totalRows; i++) {
        var user = users[i];
        var colactive = '';
        if (user.isActive) {
            colactive = "<img title='Deactivate this account' class='ico-tick' src='"
                + currentDomainURL + "App_Themes/Default/Images/blank.png' onclick='javascript:editActive(this, " + user.userId + ")' />";
        } else {
            colactive = "<img title='Activate this account' class='ico-untick' src='"
                + currentDomainURL + "App_Themes/Default/Images/blank.png' onclick='javascript:editActive(this, " + user.userId + "')' />";
        }

        var row = {
            Columns: [
                { Name: 'Row', Value: user.row },
                { Name: 'Username', Value: user.username },
                {
                    Name: 'Action',
                    Value: "<div class='btn-action subAccount_" + user.userId + "' onclick='javascript:ShowAction(" + JSON.stringify(user) + ");'></div>"
                },
                { Name: 'IsActive', Value: colactive },
                { Name: 'FirstName', Value: user.firstName },
                { Name: 'LastName', Value: user.lastName },
                { Name: 'Email', Value: user.email },
                { Name: 'Creator', Value: user.creator },
                { Name: 'CreatedDate', Value: user.createdDate }
            ]
        };

        rows.push(row);
    }

    return rows;
}

function Search() {
    $('#PageSize').val(_pageSize);
    $('#PageIndex').val(_pageIndex);

    var searchParams = {
        Username: $("#Username").val(),
        IsActive: $("#isActive option:selected").val(),
        PageSize: _pageSize,
        PageIndex: _pageIndex
    };

    $.ajax({
        url: currentDomainURL + "SubAccount/GetSubAccountList",
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(searchParams),
        cache: false,
        success: function (result) {
            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Username", Width: 150, Name: "Username", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Active", Width: 70, Name: "IsActive", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt", Sort: true },
                    { HeaderText: "First Name", Width: 120, Name: "FirstName", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Last Name", Width: 120, Name: "LastName", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Email", Width: 170, Name: "Email", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true },
                    { HeaderText: "Creator", Width: 120, Name: "Creator", HeaderAlign: "center", CellAlign: "left", CssClass: "header-mt", AllowTruncate: true, Sort: true },
                    {
                        HeaderText: "Created Date", Width: 120, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center", CssClass: "header-mt",
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

function ShowAction(user) {
    var subAccId = user.userId;
    var userName = user.username;

    _subAccId = subAccId;
    _subAccName = userName;

    $.contextMenu({
        selector: '.btn-action.subAccount_' + subAccId,
        items: {
            assignLicensee: {
                name: "Assign licensee",
                className: "iconEditLicensee",
                callback: function (key, opt) {
                    var urlParam = $.param({
                        username: _subAccName
                    });

                    window.location = currentDomainURL + "SubAccount/AssignLicensee/" + _subAccId + "?" + urlParam;
                }
            },
            deleteSubAccount: {
                name: "Delete this sub-account", height: 25, className: "iconDelAff",
                callback: function (key, opt) {
                    deleteSubAccount(_subAccId, _subAccName);
                }
            },
            editInfo: {
                name: "Edit information",
                height: 25,
                className: "iconEditInfo",
                callback: function (key, opt) {
                    var urlParam = $.param({
                        username: _subAccName,
                        SubAccountID: _subAccId
                    });

                    window.location = currentDomainURL + "SubAccount/EditSubAccInfo/" + _subAccId + "?" + urlParam;
                }
            },
            editPermission: {
                name: "Edit permission",
                height: 25,
                className: "iconEditSetting",
                callback: function (key, opt) { window.location = currentDomainURL + "SubAccount/EditPermission/" + _subAccId + "?userName=" + _subAccName; }
            },
            resetPassword: {
                name: "Reset password",
                className: "iconResetPass",
                callback: function (key, opt) {
                    window.location = currentDomainURL + "SubAccount/ResetPassword/" + _subAccId + "?userName=" + _subAccName;
                }
            }
        },
        trigger: "left",
        ignoreRightClick: true,
        autoHide: true
    });
}

function StartAutoSuggesttion() {
    var temp = $("#Username").val()
    var url = currentDomainURL + 'SubAccount/ListExistedSubAccount?username=' + temp;

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
        }
    });
}