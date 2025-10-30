function showToaster(type, subject, message) {
    Swal.fire({
        text: "Here's a basic example of SweetAlert!",
        icon: "success",
        buttonsStyling: false,
        confirmButtonText: "Ok, got it!",
        customClass: {
            confirmButton: "btn btn-primary"
        }
    });
}