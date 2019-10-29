
$(function () {

    // Turn input element into a pond
    $('.fileUpload-multiple').filepond();

    // Turn input element into a pond with configuration options
    $('.fileUpload-multiple').filepond({
        allowMultiple: true
    });

    // Set allowMultiple property to true
    $('.fileUpload-multiple').filepond('allowMultiple', true);

    // Listen for addfile event
    $('.fileUpload-multiple').on('FilePond:addfile', function (e) {
        console.log('file added event', e);
    });
});


$(function () {

    // Turn input element into a pond
    $('.fileUpload-single').filepond();

    // Turn input element into a pond with configuration options
    $('.fileUpload-single').filepond({
        allowMultiple: true
    });

    // Set allowMultiple property to true
    $('.fileUpload-single').filepond('allowMultiple', false);

    // Listen for addfile event
    $('.fileUpload-single').on('FilePond:addfile', function (e) {
        console.log('file added event', e);
    });
});