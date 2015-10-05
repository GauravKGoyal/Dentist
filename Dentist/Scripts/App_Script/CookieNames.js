function ClearSelectedPatient() {
    $.removeCookie("D_SelectedPatient", { path: '/' } );
};

function SetSelectedPatient(patientId, firstName, lastName) {
    var selectedPatient = { Id: patientId, FullName: firstName + " " + lastName };
    var cookieValue = $.param(selectedPatient);
    $.cookie("D_SelectedPatient", cookieValue, { path: '/' });
}

function GetSelectedPatient() {
    var cookieValue = $.cookie("D_SelectedPatient");
    if (cookieValue) {
        var selectedPatient = $.deparam(cookieValue);
        return selectedPatient;
    }
    return null;
}
