import toastr from 'toastr'

window.toastr = toastr;

window.getFile = (element) => {
    const fileInpunt = element;
    if (fileInpunt && fileInpunt.files && fileInpunt.files.length > 0) {
        return fileInpunt.files[0];
    }

    return null;
};

window.stream = async (file) => {
    return new Response(file).body;
};

window.showModal = function () {
    var myModal = new bootstrap.Modal(document.getElementById('exampleModal'));
    myModal.show();
};

window.ShowToastr = (type, message) => {
    try {
        if (type === "success") {
            toastr.success(message, "Operacion Correcta", { timeOut: 10000 });
        } else if (type === "error") {
            toastr.error(message, "Operacion Fallida", { timeOut: 10000 });
        }
    } catch (error) {
        console.error("Error en la función ShowToastr:", error);
    }
}
