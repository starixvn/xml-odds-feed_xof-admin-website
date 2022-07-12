var _subAccId = 0;
var _subAccName = "";
var gridOptions
        , _pageIndex = 0
        , _pageSize = 10;


$(document).ready(function () {
    //Process redirect to SubAcc list.
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


function Search() {
    $('#PageSize').val(_pageSize);
    $('#PageIndex').val(_pageIndex);

    var searchParams = {
        Username: $("#Username").val(),
        PageSize: _pageSize,
        PageIndex: _pageIndex
    };

    $.ajax({
        url: "GetSubAccountList",
        type: 'POST',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(searchParams),
        cache: false,
        success: function (result) {
            result = result.replace(/\n/g, "<\/br>")
                           .replace("\\\\", "\\", "g");
            result = eval(result);
            if (result[0].data.length > 0) {
                var dataLength = result[0].data.length;
                for (var i = 0; i < dataLength; i++) {
                    var columnsLength = result[0].data[i].Columns.length;
                    for (var j = 0; j < columnsLength; j++) {
                        result[0].data[i].Columns[j].Value = OddsFeedAdminManager.escapeHTML(result[0].data[i].Columns[j].Value);
                    }
                }
            }

            gridOptions =
            {
                columns: [
                    { HeaderText: "#", Width: 20, Name: "Row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "UserID", Width: 100, Name: "UserID", HeaderAlign: "center", CellAlign: "right" },
                    { HeaderText: "Username", Width: 100, Name: "UserName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Action", Width: 50, Name: "Action", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Active", Width: 50, Name: "IsActive", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "First Name", Width: 100, Name: "FirstName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Last Name", Width: 100, Name: "LastName", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Email", Width: 170, Name: "Email", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Creator", Width: 100, Name: "Creator", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Created Date", Width: 120, Name: "CreatedDate", HeaderAlign: "center", CellAlign: "center" }
                ],
                bodyRows: result[0].data,
                hideYBackGroundScroller: true,
                gridExpandHeight: 150,
                allowTruncate: true,
                //numberOfFrozenColumn: 2,
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: _pageSize,
                    currentPage: _pageIndex,
                    totalItem: result[1].totalItem,
                    itemsPerPageArray: [10, 20, 50, 100, 500],
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            for (var i = 0; i < gridOptions.bodyRows.length; i++) {
                // Modify "Active" column                
                gridOptions.bodyRows[i].Columns[3].Row = i;
                gridOptions.bodyRows[i].Columns[3].CustomCellRender = ActiveColumn;

                // Modify "Action" column
                gridOptions.bodyRows[i].Columns = OddsFeedAdminManager.insertAtIndex(
                    gridOptions.bodyRows[i].Columns,
                    3,
                    { Name: "Action", Row: i });
                //                gridOptions.bodyRows[i].Columns.push({ Name: "Action", Row: i });
                gridOptions.bodyRows[i].Columns[3].CustomCellRender = ActionColumn;
            }

            $("#SearchResult").fanexGrid(gridOptions);
            $("#SearchResult").fanexGrid("hideColumns", gridOptions, [1]);

        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
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

function ActiveColumn(cell, cellFormated) {
    var subAccId = gridOptions.bodyRows[cell.Row].Columns[1].Value;
    if (cell.Value == "True") {
        return "<img title='Deactivate this account' class='ico-tick' alt='' src='" + currentDomainURL + "App_Themes/Default/Images/blank.png'  onclick='editActive(this," + subAccId + ")'/>";
    }
    else {
        return "<img title='Activate this account' class='ico-untick' alt='' src='" + currentDomainURL + "App_Themes/Default/Images/blank.png'  onclick='editActive(this," + subAccId + ")'/>";
    }
}

function ActionColumn(cell, cellFormated) {
    return "<div class='btn-action subAccount_" + cell.Row + "' onclick=\"javascript:ShowAction('" + cell.Row + "');\"></div>";
}

function ShowAction(cellRow) {
    var subAccId = gridOptions.bodyRows[cellRow].Columns[1].Value;
    var userName = gridOptions.bodyRows[cellRow].Columns[2].Value;
    var firstName = gridOptions.bodyRows[cellRow].Columns[4].Value;
    var lastName = gridOptions.bodyRows[cellRow].Columns[5].Value;
    var email = gridOptions.bodyRows[cellRow].Columns[6].Value;
    var isActive = gridOptions.bodyRows[cellRow].Columns[3].Value;

    _subAccId = subAccId;
    _subAccName = userName;
    if (isActive == "") {
        isActive = "False";
    }

    $.contextMenu({
        selector: '.btn-action.subAccount_' + cellRow,
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
            deleteSubAccount: { name: "Delete this sub-account", height: 25, className: "iconDelAff", callback: function (key, opt) { deleteSubAccount(_subAccId, _subAccName); } },
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
    var url = 'ListExistedSubAccount?username=' + temp;

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