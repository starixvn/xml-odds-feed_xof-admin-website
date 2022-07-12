var gridOptions
        , _pageIndex = 0
        , _pageSize = 10;
$(document).ready(function () {

    //RegisterDialog();

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
        updateTips("Length of " + n + " must be between " +
                                        min + " and " + max + ".");
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

function Search() {
    $('#PageSize').val(_pageSize);
    $('#PageIndex').val(_pageIndex);

    var searchParams = {
        SystemID: $("#SystemID option:selected").val(),
        Username: $("#Username").val(),
        RoleID: $("#RoleID option:selected").val(),
        PageSize: _pageSize,
        PageIndex: _pageIndex
    };

    $.ajax({
        url: "GetListAdminBySystemAndNameAndRole",
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
                    { HeaderText: "#", Width: 20, Name: "row", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "UserId", Width: 20, Name: "userid", HeaderAlign: "center", CellAlign: "right" },
                    { HeaderText: "Username", Width: 100, Name: "username", TdClass: "boldusername", CssClass: "boldusername", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Action", Width: 50, Name: "action", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Active", Width: 50, Name: "isactive", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Role ID", Width: 20, Name: "roleid", HeaderAlign: "center", CellAlign: "center" },
                    { HeaderText: "Role", Width: 120, Name: "rolename", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "First Name", Width: 100, Name: "firstname", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Last Name", Width: 100, Name: "lastname", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Email", Width: 170, Name: "email", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Creator", Width: 100, Name: "creator", HeaderAlign: "center", CellAlign: "left" },
                    { HeaderText: "Created Date", Width: 120, Name: "createdate", HeaderAlign: "center", CellAlign: "center" }
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
                },
                columnsNeedHide: [1, 5],
                showColumnsShowHide: false
            };

            for (var i = 0; i < gridOptions.bodyRows.length; i++) {
                // Modify "Active" column                
                gridOptions.bodyRows[i].Columns[3].Row = i;
                gridOptions.bodyRows[i].Columns[3].CustomCellRender = ActiveColumn;

                // Modify "Action" column
                gridOptions.bodyRows[i].Columns = OddsFeedAdminManager.insertAtIndex(
                    gridOptions.bodyRows[i].Columns,
                    3,
                    { Name: "action", Row: i });
                //                gridOptions.bodyRows[i].Columns.push({ Name: "action", Row: i });
                gridOptions.bodyRows[i].Columns[3].CustomCellRender = ActionColumn;
            }

            $("#SearchResult").fanexGrid(gridOptions);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(xhr);
        },
        complete: function (jqXHR, textStatus) {
        }
    });
}

function ActiveColumn(cell, cellFormated) {
    var userId = gridOptions.bodyRows[cell.Row].Columns[1].Value;
    var username = gridOptions.bodyRows[cell.Row].Columns[2].Value;
    if (cell.Value == "True") {
        return "<img title='Deactivate this account' class='ico-tick' alt='' src='" + currentDomainURL + "App_Themes/Default/Images/blank.png'  onclick='editActive(this," + userId + ",\"" + username + "\")'/>";
    }
    else {
        return "<img title='Activate this account' class='ico-untick' alt='' src='" + currentDomainURL + "App_Themes/Default/Images/blank.png'  onclick='editActive(this," + userId + ",\"" + username + "\")'/>";
    }
}

function ActionColumn(cell, cellFormated) {
    var userID = gridOptions.bodyRows[cell.Row].Columns[1].Value;
    var userName = gridOptions.bodyRows[cell.Row].Columns[2].Value;
    var role = gridOptions.bodyRows[cell.Row].Columns[5].Value;
    var firstname = gridOptions.bodyRows[cell.Row].Columns[7].Value;
    var lastname = gridOptions.bodyRows[cell.Row].Columns[8].Value;
    var email = gridOptions.bodyRows[cell.Row].Columns[9].Value;

    return "<div class='btn-action' onclick=\"javascript:ShowAction('" + cell.Row + "');\"></div>";
}

function ShowAction(cellRow) {
    var userID = gridOptions.bodyRows[cellRow].Columns[1].Value;
    var userName = gridOptions.bodyRows[cellRow].Columns[2].Value;
    var role = gridOptions.bodyRows[cellRow].Columns[5].Value;
    var firstname = gridOptions.bodyRows[cellRow].Columns[7].Value;
    var lastname = gridOptions.bodyRows[cellRow].Columns[8].Value;
    var email = gridOptions.bodyRows[cellRow].Columns[9].Value;

    _adminId = userID;
    _adminName = userName;

    $.contextMenu('destroy');

    if (role == 1) { //ITAdmin role
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

                        window.location.href = currentDomainURL + "Administrator/AdministratorEditProfile/" +
                        _adminId + "?" + paramsString;
                    }
                },
                resetPassword: {
                    name: "Reset password",
                    height: 25,
                    className: "iconResetPwd",
                    callback: function (key, opt) {
                        var paramsString = $.param({
                            username: _adminName,
                            role: role
                        });

                        window.location.href = currentDomainURL + "Administrator/AdministratorResetPwd/" +
                                            _adminId + "?" + paramsString;
                    }
                }
            },
            trigger: "left",
            ignoreRightClick: true,
            autoHide: true
        });
    }
    else {//Licensse role
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
                            role: role
                        });

                        window.location.href = currentDomainURL + "Administrator/AdministratorEditSystem/" +
                                    _adminId + "?" + paramsString;
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

                        window.location.href = currentDomainURL + "Administrator/AdministratorEditProfile/" +
                        _adminId + "?" + paramsString;
                    }
                },
                resetPassword: {
                    name: "Reset password",
                    height: 25,
                    className: "iconResetPwd",
                    callback: function (key, opt) {
                        var paramsString = $.param({
                            username: _adminName,
                            role: role
                        });

                        window.location.href = currentDomainURL + "Administrator/AdministratorResetPwd/" +
                                            _adminId + "?" + paramsString;
                    }
                }
            },
            trigger: "left",
            ignoreRightClick: true,
            autoHide: true
        });
    }
}

//--------------------------------- downlines are old
function FunctionColumn(cell, cellFormated) {
    return "<div style='text-align:center;'><a href='javascript:UpdateLicensee(" +
        cell.Value.toString() + ")'><img src='" + currentDomainURL +
        "App_Themes/Default/Images/Edit.png' alt='Edit'></a>&nbsp&nbsp<a href='javascript:DeleteLicensee(" +
        cell.Value.toString() + ")'><img src='" + currentDomainURL + "App_Themes/Default/Images/Delete.gif' alt='Delete'></a></div>";
}

function ConvertFromJsonToFanexData(jsonObject) {
    var rows = [];

    for (var i = 0; i < jsonObject.length; i++) {
        var col = []
        for (var key in jsonObject[i]) {
            col.push({ Name: key.toString(), Value: jsonObject[i][key] });
        }
        rows.push({ Columns: col });
    }

    return rows;
}

function GenerateColModel(jsonObject) {
    var colModel = [];

    for (var key in jsonObject[0]) {
        colModel.push({ HeaderText: key.toString(), Width: 100, Name: key.toString(), CellAlign: "center", Sort: true });
    }

    return colModel;
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

function OpenDialog(id) {
    $(".validateTips").text("").removeClass("ui-state-highlight"); ;
    $("#" + id).dialog("open");
}

function UpdateLicensee(row) {
    $("#SystemID", $("#dialog-form-edit")).val(gridOptions.bodyRows[row].Columns[0].Value);
    $("#SystemName", $("#dialog-form-edit")).val(gridOptions.bodyRows[row].Columns[1].Value);
    $("#CreatedDate", $("#dialog-form-edit")).html(gridOptions.bodyRows[row].Columns[2].Value);
    $("#LastUpdate", $("#dialog-form-edit")).html(gridOptions.bodyRows[row].Columns[2].Value);
    OpenDialog("dialog-form-edit");
}

function DeleteLicensee(row) {
    confirm("Are you sure you want to delete this Licensee?", function () {
        $.ajax({
            url: "DeleteLicensee",
            type: "POST",
            data: "SystemID=" + gridOptions.bodyRows[row].Columns[0].Value,
            success: function (errCode) {
                if (errCode != 0) {
                    GetErrorMessage(errCode);
                }
                else {
                    Submit();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //alert("500 internal server error.");
            },
            complete: function (jqXHR, textStatus) {
            }
        });
    })
}

function RegisterDialog() {
    // Register Create Dialog box
    $("#dialog-form").dialog({
        autoOpen: false,
        height: 'auto',
        width: 350,
        modal: true,
        buttons: {
            "Submit": function () {
                var dialog = $(this);
                var bValid = true;
                bValid = bValid && checkRequired($('#SystemName', dialog), "Licensee Name");
                bValid = bValid && checkRegexp($('#SystemName', dialog), /^[a-z]([0-9a-z_])+$/i, "Username may consist of a-z, 0-9, underscores, begin with a letter.");
                $("#SystemName", $(this)).removeClass("ui-state-error");
                if (bValid) {
                    $.ajax({
                        url: "CreateLicensee",
                        type: "POST",
                        data: $("form", $("#dialog-form")).eq(0).serialize(),
                        success: function (errCode) {
                            if (errCode != 0) {
                                GetErrorMessage(errCode);
                            }
                            else {
                                dialog.dialog("close");
                                Submit();
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //alert("500 internal server error.");
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });

                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            $('#SystemName', $(this)).val("").removeClass("ui-state-error");
        }
    });

    $("#dialog-form-edit").dialog({
        autoOpen: false,
        height: 'auto',
        width: 350,
        modal: true,
        buttons: {
            "Submit": function () {
                var dialog = $(this);
                var bValid = true;
                bValid = bValid && checkRequired($('#SystemName', dialog), "Licensee Name");
                bValid = bValid && checkRegexp($('#SystemName', dialog), /^[a-z]([0-9a-z_])+$/i, "Username may consist of a-z, 0-9, underscores, begin with a letter.");
                $("#SystemName", $(this)).removeClass("ui-state-error");
                if (bValid) {
                    $.ajax({
                        url: "UpdateLicensee",
                        type: "POST",
                        data: $("form", $("#dialog-form-edit")).eq(0).serialize(),
                        success: function (errCode) {
                            if (errCode != 0) {
                                GetErrorMessage(errCode);
                            }
                            else {
                                dialog.dialog("close");
                                Submit();
                            }
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                            //alert("500 internal server error.");
                        },
                        complete: function (jqXHR, textStatus) {
                        }
                    });

                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            $('#SystemName', $(this)).val("").removeClass("ui-state-error");
        }
    });

    $("#dialog-form, #dialog-form-edit").keypress(function (e) {
        if (e.keyCode == $.ui.keyCode.ENTER) {
            return false;
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
