toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "progressBar": true,
    "rtl": true,
    "positionClass": "toast-bottom-left",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": 300,
    "hideDuration": 500,
    "timeOut": 3000,
    "extendedTimeOut": 1000,
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

function TosterWithOutTitel(TosterState, TosterMassage) {
    switch (TosterState) {
        case "success":
            toastr.success(TosterMassage);
            break;
        case "info":
            toastr.info(TosterMassage);
            break;
        case "error":
            toastr.error(TosterMassage);
            break;
        case "warning":
            toastr.warning(TosterMassage);
            break;
        default:
            toastr.error("invalid");
            break;
    }
};

function TosterWithTitel(TosterState, TosterMassage, titel) {
    switch (TosterState) {
        case "success":
            toastr.success(TosterMassage, titel);
            break;
        case "info":
            toastr.info(TosterMassage, titel);
            break;
        case "error":
            toastr.error(TosterMassage, titel);
            break;
        case "warning":
            toastr.warning(TosterMassage, titel);
            break;
        default:
            toastr.error("invalid");
            break;
    }
};


function TosterJustTitel(TosterState, titel) {
    switch (TosterState) {
        case "success":
            toastr.success("", titel);
            break;
        case "info":
            toastr.info("", titel);
            break;
        case "error":
            toastr.error("", titel);
            break;
        case "warning":
            toastr.warning("", titel);
            break;
        default:
            toastr.error("invalid");
            break;
    }
};
