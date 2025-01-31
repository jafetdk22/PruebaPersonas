"use strict";
var datatable;
var Personas = function () {
    var init = function () {
        var startDate = '';
        var endDate = '';

        // Detectar cambios en los campos de fecha
        $('#fechaInicio, #fechaFin').change(function () {
            startDate = $('#fechaInicio').val();
            endDate = $('#fechaFin').val();
            if (startDate && endDate) {
                // Actualizar la tabla con los nuevos filtros
                datatable.ajax.reload();
            }
        });

        datatable = $('#kt_personas').DataTable({
            order: [],
            retrieve: true,
            pageLength: 15,
            language: {
                "decimal": "",
                "emptyTable": "No hay información",
                "info": "  de _TOTAL_ entradas",
                "infoEmpty": "  de 0 entradas",
                "infoFiltered": "(Filtrado de _MAX_ total entradas)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ ",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "Buscar:",
                "order": [[0, 'asc'], [1, 'asc'], [2, 'asc']],
                "zeroRecords": "No se encontraron resultados para mostrar",
                "paginate": {
                    "first": "Primero",
                    "last": "Último",
                    "next": "&raquo;",
                    "previous": "&laquo;"
                }
            },
            responsive: true,
            pagingType: 'simple_numbers',
            searching: true,
            lengthMenu: [15, 25, 50, 100],
            processing: true,
            serverSide: true,
            ordering: true,
            ajax: {
                url: siteLocation + 'Persona/ListarPersonas',
                type: 'POST',
                data: function (d) {
                    // Incluir las fechas seleccionadas en la petición
                    d.fechaInicio = startDate;
                    d.fechaFin = endDate;
                }
            },
            columnDefs: [{
                "defaultContent": "-",
                "targets": "_all"
            }],
            columns: [
                { data: 'nombres', name: '', title: 'Nombres' },
                { data: 'apellidoPaterno', name: '', title: 'Apellido Paterno' },
                { data: 'apellidoMaterno', name: '', title: 'Apellido Materno' },
                { data: 'fechaNacimiento', name: '', title: 'Fecha Nacimiento' },
                { data: 'nivelEducativo', name: '', title: 'Nivel Educativo' },
                { data: 'numeroCelular', name: '', title: 'Numero Celular' },
                { data: 'fechaRegistro', name: '', title: 'Fecha de Registro' },
                {
                    title: 'Acciones',
                    orderable: false,
                    data: null,
                    defaultContent: '',
                    render: function (data, type, row) {
                        return `
                        <button type="button" class="btn-sm btn btn-warning"
                            style="width:120px"
                            onclick="Personas.EditarPersona(this)"
                            data-id="${row.id}" 
                            data-nombres="${row.nombres}" 
                            data-apellidoPaterno="${row.apellidoPaterno}" 
                            data-apellidoMaterno="${row.apellidoMaterno}" 
                            data-fechaNacimiento="${row.fechaNacimiento}" 
                            data-nivelEducativo="${row.nivelEducativo}" 
                            data-numeroCelular="${row.numeroCelular}" 
                            data-fechaRegistro="${row.fechaRegistro}">
                             <i class="fonticon-content-marketing"></i>&nbsp;
                             Editar
                        </button>
                        <button type="button" class="btn-sm btn btn-danger mt-3"
                            style="width:120px"
                            onclick="Personas.EliminarPersona(${row.id})">
                             <i class="bi bi-trash-fill"></i>&nbsp;
                             Eliminar
                        </button>
                    `;
                    }
                }
            ]
        });
    };

    var recargar = function () {
        // Recargar la tabla
        datatable.ajax.reload(null, false); // El segundo parámetro es para no resetear la paginación
    };
    var CerrarModal = function () {
        $("#modal_registro .modal-body").html('');

        $("#modalLabelTitle").text(" ");
        $('#modal_registro').modal('hide');
    }
    const listeners = function () {
        const btnRegitro = document.getElementById("NuevaPersona");
        btnRegitro.addEventListener('click', function () {
            $.ajax({
                url: siteLocation + 'Persona/RegisterView',
                type: 'GET',
                success: function (result) {
                    $("#modal_registro .modal-body").html(result);

                    if (result) {
                        init_validations("#formRegistro");
                    }
                    $("#modalLabelTitle").text("Nueva Persona");
                    $('#modal_registro').modal('show');
                },
                error: function (xhr, status, error) {
                    console.error('Error en la petición Ajax:', error);
                }
            });
        });
    }

    const init_validations = function (form) {
        $(form).validate({
            rules: {
                nombres: {
                    required: true,
                    maxlength: 100,
                },
                apellidoPaterno: {
                    required: true,
                    maxlength: 100,
                },
                apellidoMaterno: {
                    required: true,
                    maxlength: 100,
                },
                fechaNacimiento: {
                    required: true,
                    date: true,
                },
                nivelEducativo: {
                    required: true,
                    maxlength: 50,
                },
                numeroCelular: {
                    required: true,
                    maxlength: 15,
                }

            },
            messages: {
                nombres: {
                    required: "El campo Nombres es obligatorio.",
                    maxlength: "El campo Nombres no debe superar los 100 caracteres.",
                },
                apellidoPaterno: {
                    required: "El campo Apellido Paterno es obligatorio.",
                    maxlength: "El campo Apellido Paterno no debe superar los 100 caracteres.",
                },
                apellidoMaterno: {
                    required: "El campo Apellido Materno es obligatorio.",
                    maxlength: "El campo Apellido Materno no debe superar los 100 caracteres.",
                },
                fechaNacimiento: {
                    required: "El campo Fecha de Nacimiento es obligatorio.",
                    date: "Por favor ingrese una fecha válida.",
                },
                nivelEducativo: {
                    required: "El campo Nivel Educativo es obligatorio.",
                    maxlength: "El campo Nivel Educativo no debe superar los 50 caracteres.",
                },
                numeroCelular: {
                    required: "El campo Número de Celular es obligatorio.",
                    maxlength: "El campo Número de Celular no debe superar los 15 caracteres.",
                },
            },
            errorElement: "div",
            errorPlacement: function (error, element) {
                error.addClass("invalid-feedback");
                element.closest(".fv-row").append(error);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass("is-invalid").removeClass("is-valid");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).addClass("is-valid").removeClass("is-invalid");
            },
            submitHandler: function (form) {
                event.preventDefault();

                const formData = $(form).serialize();

                $.ajax({
                    url: siteLocation + "Persona/InsertarPersona",
                    method: 'POST',
                    data: formData,
                    success: function (response) {
                        // Manejar la respuesta exitosa
                        if (response.success) {
                            recargar();
                            CerrarModal();
                        }
                        Swal.fire({
                            icon: response.success ? 'success' : 'error',
                            title: response.success ? 'Formulario enviado correctamente' : 'Hubo un error',
                            text: response.message, // Usar el mensaje de la respuesta
                            confirmButtonText: 'Aceptar'
                        });
                    },
                    error: function (xhr, status, error) {
                        // Manejar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Hubo un error',
                            text: 'Hubo un error al enviar el formulario.',
                            confirmButtonText: 'Aceptar'
                        });
                    }
                });
            }
        });
    };

    const init_validationsEditar = function (form) {
        $(form).validate({
            rules: {
                id: {
                    required: true,
                },
                nombres: {
                    required: true,
                    maxlength: 100,
                },
                apellidoPaterno: {
                    required: true,
                    maxlength: 100,
                },
                apellidoMaterno: {
                    required: true,
                    maxlength: 100,
                },
                fechaNacimiento: {
                    required: true,
                    date: true,
                },
                nivelEducativo: {
                    required: true,
                    maxlength: 50,
                },
                numeroCelular: {
                    required: true,
                    maxlength: 15,
                }
            },
            messages: {
                id: {
                    required: "Ocurrio un problema.",
                },
                nombres: {
                    required: "El campo Nombres es obligatorio.",
                    maxlength: "El campo Nombres no debe superar los 100 caracteres.",
                },
                apellidoPaterno: {
                    required: "El campo Apellido Paterno es obligatorio.",
                    maxlength: "El campo Apellido Paterno no debe superar los 100 caracteres.",
                },
                apellidoMaterno: {
                    required: "El campo Apellido Materno es obligatorio.",
                    maxlength: "El campo Apellido Materno no debe superar los 100 caracteres.",
                },
                fechaNacimiento: {
                    required: "El campo Fecha de Nacimiento es obligatorio.",
                    date: "Por favor ingrese una fecha válida.",
                },
                nivelEducativo: {
                    required: "El campo Nivel Educativo es obligatorio.",
                    maxlength: "El campo Nivel Educativo no debe superar los 50 caracteres.",
                },
                numeroCelular: {
                    required: "El campo Número de Celular es obligatorio.",
                    maxlength: "El campo Número de Celular no debe superar los 15 caracteres.",
                }
            },
            errorElement: "div",
            errorPlacement: function (error, element) {
                error.addClass("invalid-feedback");
                element.closest(".fv-row").append(error);
            },
            highlight: function (element, errorClass, validClass) {
                $(element).addClass("is-invalid").removeClass("is-valid");
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).addClass("is-valid").removeClass("is-invalid");
            },
            submitHandler: function (form) {
                event.preventDefault();

                const formData = $(form).serialize();

                $.ajax({
                    url: siteLocation + "Persona/EditarPersona",
                    method: 'PUT',
                    data: formData,
                    success: function (response) {
                        // Manejar la respuesta exitosa
                        if (response.success) {
                            recargar();
                            CerrarModal();
                        }
                        Swal.fire({
                            icon: response.success ? 'success' : 'error',
                            title: response.success ? 'Formulario enviado correctamente' : 'Hubo un error',
                            text: response.message, // Usar el mensaje de la respuesta
                            confirmButtonText: 'Aceptar'
                        });
                    },
                    error: function (xhr, status, error) {
                        // Manejar errores
                        Swal.fire({
                            icon: 'error',
                            title: 'Hubo un error',
                            text: 'Hubo un error al enviar el formulario.',
                            confirmButtonText: 'Aceptar'
                        });
                    }
                });
            }
        });
    };

    const EditarPersona = function (element) {
        var id = element.dataset.id;
        var nombres = element.dataset.nombres;
        var apellidoPaterno = element.dataset.apellidopaterno;
        var apellidoMaterno = element.dataset.apellidomaterno;
        var fechaNacimiento = element.dataset.fechanacimiento;
        var nivelEducativo = element.dataset.niveleducativo;
        var numeroCelular = element.dataset.numerocelular;
        var estatus = element.dataset.estatus;

        var fecha = new Date(fechaNacimiento);
        var año = fecha.getFullYear();
        var mes = ("0" + (fecha.getMonth() + 1)).slice(-2);
        var dia = ("0" + fecha.getDate()).slice(-2);
        var fechaFormateada = año + "-" + mes + "-" + dia;
        // Realizar la petición Ajax
        $.ajax({
            url: siteLocation + 'Persona/EditView',
            type: 'GET',
            success: function (result) {
                $("#modal_registro .modal-body").html(result);

                $("#idPersona").val(id);
                $("#nombres").val(nombres);
                $("#apellidoPaterno").val(apellidoPaterno);
                $("#apellidoMaterno").val(apellidoMaterno);
                $("#fechaNacimiento").val(fechaFormateada);
                $("#nivelEducativo").val(nivelEducativo);
                $("#numeroCelular").val(numeroCelular);
                $("#estatus").val(estatus);

                if (result) {
                    init_validationsEditar("#formEditar");
                }

                // Modificar el título del modal y mostrarlo
                $("#modalLabelTitle").text("Editar Persona");
                $('#modal_registro').modal('show');
            },
            error: function (xhr, status, error) {
                console.error('Error en la petición Ajax:', error);
            }
        });
    };
    const EliminarPersona = function (id) {
        // Mostrar el cuadro de confirmación con SweetAlert2
        Swal.fire({
            title: '¿Estás seguro?',
            text: "¡Esta acción no se puede deshacer!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            // Si el usuario confirma la acción
            if (result.isConfirmed) {
                // Realizar la solicitud Ajax para eliminar el registro
                $.ajax({
                    url: siteLocation + 'Persona/EliminarPersona', // Ruta del controlador
                    type: 'POST',
                    data: { id: parseInt(id) }, // Enviamos 'id' como un parámetro simple, no como JSON
                    success: function (response) {
                        if (response.success) {
                            recargar();
                            CerrarModal();
                            // Mostrar mensaje de éxito
                            Swal.fire(
                                'Eliminado',
                                response.message,
                                'success'
                            );
                        } else {
                            // Mostrar mensaje de error
                            Swal.fire(
                                'Error',
                                response.message,
                                'error'
                            );
                        }
                    },
                    error: function (xhr, status, error) {
                        // Mostrar mensaje de error si algo falla
                        Swal.fire(
                            'Error',
                            'Ocurrió un error al eliminar la persona.',
                            'error'
                        );
                    }
                });
            }
        });
    };



    return {
        init: function () {
            init();
            listeners();
        },
        EditarPersona: function (element) {
            EditarPersona(element)
        },
        EliminarPersona: function (id) {
            EliminarPersona(id);
        }
    }
}();

KTUtil.onDOMContentLoaded(function () {
    Personas.init();
});
