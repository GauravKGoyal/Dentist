
function grd_onDataBound(args) {
    placeIconsOnGridCreateEditDeleteButtons();
}

function placeIconsOnGridCreateEditDeleteButtons()
{
    $(".k-grid-Edit").find("span").addClass("k-icon k-edit");
    $(".k-grid-Delete").find("span").addClass("k-icon k-delete");
    $(".k-grid-Create").find("span").addClass("k-icon k-add");
}


