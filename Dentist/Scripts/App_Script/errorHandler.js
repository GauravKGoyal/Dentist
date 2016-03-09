function handleCreateUpdateResponseError(ctrl, response) {
    var handled = false;
    if (hasErrorMessage(response)) {
        ctrl.errorMessage = response.data.message;
        handled = true;
    }
    if (hasModelStateErrors(response) === true) {
        ctrl.modelState = response.data.modelState;
        handled = true;
    }
    if (!handled) {
        alert("Failed to update patient's note");
    }
}

/// List is used to show delete option therefore error has to be shown in popup
function handleDeleteResponseError(response) {
    var handled = false;
    if (hasErrorMessage(response)) {
        alert(response.data.message);
        handled = true;
    }
    if (!handled) {
        alert("Failed to delete record");
        handled = true;
    }
}

function hasModelStateErrors(response) {
    return ((response.status = 400) && (response.data.modelState !== 'undefined'))
}
function hasErrorMessage(response) {
    return (response.data.message !== 'undefined')
}
