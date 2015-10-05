
function grd_onDataBound(args) {
    placeIconsOnGridCreateEditDeleteButtons();
}

function placeIconsOnGridCreateEditDeleteButtons()
{
    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
    $(".k-grid-Delete").find("span").addClass("k-icon k-delete");
    $(".k-grid-Create").find("span").addClass("k-icon k-add");
}

function DeleteGridRow(e, gridControl, deleteUrlToCall) {
    if (confirm('Are you sure you want to delete this record ?')) {
        var id = GetGridSelectedRowId(e, gridControl);
        $.post(deleteUrlToCall + '?id=' + id, function (data) {
            if (data !== undefined && data !== null) {
                if (data.Success == 1) {
                    gridControl.data('kendoGrid').dataSource.read();
                    gridControl.data('kendoGrid').refresh();
                } else {
                    alert(data.ErrorMessage);
                }
            }
        });
    }
}

function EditGridRow(e, gridControl, editUrlToCall) {
    e.preventDefault();
    var id = GetGridSelectedRowId(e, gridControl);
    window.location.href = editUrlToCall + "/" + id;
}

function GetGridSelectedRowId(e, gridControl) {
    return gridControl.data("kendoGrid").dataItem($(e.currentTarget).closest("tr")).Id;
}

function GetGridSelectedRowObject(e, gridControl) {
    return gridControl.data("kendoGrid").dataItem($(e.currentTarget).closest("tr"));
}

function grid_OnError(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        alert(message);
    }
}
