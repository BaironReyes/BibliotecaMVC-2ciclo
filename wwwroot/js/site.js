// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function(){
    $(document).on('submit', 'form.swal-delete-form', function (e) {
        e.preventDefault();
        var form = this;
        Swal.fire({
            title: 'Está seguro que quiere eliminar este autor?',
            text: "No podrá revertir esta acción!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33', 
            confirmButtonText: 'Sí, eliminar!',
            cancelButtonText: 'Cancelar',
        })
            .then(function (result) {
                if (result.isConfirmed) {
                    form.submit();
                }
            });
    });

    $(document).on('submit', 'form.swal-save-form', function (e) {
        e.preventDefault();
        var form = this;
        Swal.fire({
            title: 'Está seguro que quiere guardar estos cambios?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: 'green',
            confirmButtonText: 'Sí, guardar!',
            cancelButtonText: 'Cancelar',
        })
            .then(function (result) {
                if (result.isConfirmed) {
                    form.submit();
                }
            });
    });
})