function showConfirmationMessage3() {
   /* var constanciaId = '<%= txtConstanciaId.Text %>';*/
    var constanciaId = document.getElementById("txtConstanciaId.ClientID").value;

    $.ajax({
        type: "POST",
        url: "Seguimiento.aspx/GetSessionValues",
        data: JSON.stringify({ constanciaId: constanciaId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var values = response.d.split("|");
            var constanciaId = values[0];
            var estado = values[1];
            var fechaCreacion = values[2];
            var otrosDatos = values[3];

            var tableHtml = "<table border='0' width='100%'>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2' align='center'><strong><font size='+2'>Estado de la Constancia</font></strong></td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td>Número de Constancia:</td><td>" + constanciaId + "</td></tr>" +
                "<tr><td>Estado:</td><td>" + estado + "</td></tr>" +
                "<tr><td>Fecha de Creación:</td><td>" + fechaCreacion + "</td></tr>" +
                "<tr><td>Otros Datos:</td><td>" + otrosDatos + "</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>" +
                "</table>";

            document.getElementById("popupContent").innerHTML = tableHtml;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error al obtener valores de la constancia: " + thrownError);
        }
    });
}

function showConfirmationMessage2(constanciaId) {
    $.ajax({
        type: "POST",
        url: "Seguimiento.aspx/GetSessionValues",
        data: JSON.stringify({ constanciaId: constanciaId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log("Respuesta del servidor:", response.d); // Imprime la respuesta completa
            if (response.d.startsWith("Error:")) {
                console.error("Error del servidor: " + response.d);
                alert("Ocurrió un error al obtener los datos de la constancia.");
                return;
            }

            var values = response.d.split("|");
            if (values.length < 5) {
                console.error("Respuesta del servidor incompleta: " + response.d);
                alert("Ocurrió un error al procesar los datos de la constancia.");
                return;
            }

            var constanciaId = values[0];
            var estado = values[1];
            var fechaCreacion = values[2];
            var otrosDatos = values[3];
            var archivoconstanciaBase64 = values[4];

            var tableHtml = "<table border='0' width='100%'>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2' align='center'><strong><font size='+2'>Estado de la Constancia</font></strong></td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td>Número de Constancia:</td><td>" + constanciaId + "</td></tr>" +
                "<tr><td>Estado:</td><td>" + estado + "</td></tr>" +
                "<tr><td>Fecha de Creación:</td><td>" + fechaCreacion + "</td></tr>" +
                "<tr><td>Observaciones:</td><td>" + otrosDatos + "</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>";

            if (estado.toLowerCase() === "finalizada" && archivoconstanciaBase64) {
                tableHtml += "<tr><td colspan='2' align='center'>" +
                    "<button id='btnDescargarConstancia' class='btn btn-success'>Descargar Constancia</button>" +
                    "</td></tr>";

                // Agregar un manejador de eventos para el botón de descarga
                setTimeout(function () {
                    document.getElementById("btnDescargarConstancia").addEventListener("click", function () {
                        downloadBase64File(archivoconstanciaBase64, "Constancia_" + constanciaId + ".pdf");
                    });
                }, 100);
            }

            tableHtml += "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>";
            tableHtml += "</table>";

            document.getElementById("popupContent").innerHTML = tableHtml;

            // Mostrar el popup
            setTimeout(function () { popupSeguimiento.Show(); }, 500);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error al obtener valores de la constancia: " + thrownError);
        }
    });
}
            // Verificar si el popup existe
            //var popupElement = document.getElementById("popupSeguimiento");
            //if (!popupElement) {
            //    console.error("Error: No se encontró el elemento 'popupSeguimiento'.");
            //    return;
            //}

            // Insertar contenido en el popup
//            popupElement.innerHTML = tableHtml;

//            // Mostrar el popup si es un DevExpress ASPxPopupControl
//            if (typeof popupSeguimiento !== "undefined" && popupSeguimiento.Show) {
//                popupSeguimiento.Show();
//            } else {
//                console.warn("popupSeguimiento no está definido como un control DevExpress.");
//                popupElement.style.display = "block"; // Alternativa si es un div normal
//            }
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            console.error("Error en la solicitud AJAX: " + thrownError);
//            alert("Ocurrió un error al obtener los datos de la constancia.");
//        }
//    });
//}



function isValidBase64(str) {
    try {
        atob(str);
        return true;
    } catch (e) {
        return false;
    }
}

function downloadBase64File(base64Data, fileName) {
    if (!isValidBase64(base64Data)) {
        console.error("La cadena Base64 no está correctamente codificada.");
        alert("Ocurrió un error al descargar la constancia. La cadena Base64 no es válida.");
        return;
    }

    try {
        // Convertir Base64 a un Blob
        var byteCharacters = atob(base64Data);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        var blob = new Blob([byteArray], { type: "application/pdf" });

        // Crear un enlace temporal para la descarga
        var link = document.createElement("a");
        link.href = window.URL.createObjectURL(blob);
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    } catch (error) {
        console.error("Error al descargar el archivo: ", error);
        alert("Ocurrió un error al descargar la constancia. Por favor, inténtelo de nuevo.");
    }
}

function ClosePopupRelacionado1(s, e) {
    Relacionado1.Hide();
}


//$(document).ready(function () {
//    $("#<%= txtCodigoVerificacion.ClientID %>").on("keyup", function () {
//        var codigoIngresado = $(this).val();

//        $.ajax({
//            type: "POST",
//            url: "Seguimiento.aspx/VerificarCodigo",
//            data: JSON.stringify({ codigoIngresado: codigoIngresado }),
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (response) {
//                if (response.d) {
//                    $("#<%= btnBuscar.ClientID %>").prop("disabled", false);
//                    $("#<%= lblMensaje.ClientID %>").text("Código verificado correctamente.").css("color", "green");
//                } else {
//                    $("#<%= btnBuscar.ClientID %>").prop("disabled", true);
//                    $("#<%= lblMensaje.ClientID %>").text("Código incorrecto. Intente nuevamente.").css("color", "red");
//                }
//            },
//            error: function () {
//                console.error("Error en la verificación.");
//            }
//        });
//    });
//});

//function showConfirmationMessage22(constanciaId) {
//    $.ajax({
//        type: "POST",
//        url: "Seguimiento.aspx/GetSessionValues",
//        data: JSON.stringify({ constanciaId: constanciaId }),
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            var values = response.d.split("|");
//            var constanciaId = values[0];
//            var estado = values[1];
//            var fechaCreacion = values[2];
//            var otrosDatos = values[3];
//            var enlaceDescarga = values[4];

//            var tableHtml = "<table border='0' width='100%'>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2' align='center'><strong><font size='+2'>Estado de la Constancia</font></strong></td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td>Número de Constancia:</td><td>" + constanciaId + "</td></tr>" +
//                "<tr><td>Estado:</td><td>" + estado + "</td></tr>" +
//                "<tr><td>Fecha de Creación:</td><td>" + fechaCreacion + "</td></tr>" +
//                "<tr><td>Observaciones:</td><td>" + otrosDatos + "</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>";

//            if (estado.toLowerCase() === "lista") {
//                tableHtml += "<tr><td colspan='2' align='center'><a href='" + enlaceDescarga + "' class='btn btn-success' download>Descargar Constancia</a></td></tr>";
//            }

//            tableHtml += "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>";
//            tableHtml += "</table>";

//            // Verificar si el popup existe
//            var popupElement = document.getElementById("popupSeguimiento");
//            if (!popupElement) {
//                console.error("Error: No se encontró el elemento 'popupSeguimiento'.");
//                return;
//            }

//            // Insertar contenido en el popup
//            popupElement.innerHTML = tableHtml;

//            // Mostrar el popup si es un DevExpress ASPxPopupControl
//            if (typeof popupSeguimiento !== "undefined" && popupSeguimiento.Show) {
//                popupSeguimiento.Show();
//            } else {
//                console.warn("popupSeguimiento no está definido como un control DevExpress.");
//                popupElement.style.display = "block"; // Alternativa si es un div normal
//            }
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            console.log("Error al obtener valores de la constancia: " + thrownError);
//        }
//    });
//}
//function ClosePopupRelacionado1(s, e) {
//    //var ckPolitica = ASPxClientControl.GetControlCollection().GetByName("ckPolitica");
//    //var btnEnviarCodigo = ASPxClientControl.GetControlCollection().GetByName("btnEnviarCodigo");
//    Relacionado1.Hide();
//    //if (!ckPolitica.GetChecked()) {
//    //    btnEnviarCodigo.SetEnabled(false);
//    //}
//}

function GuardarConstancia() {
    var grid = ASPxClientControl.GetControlCollection().GetByName('GV_PreUsuarios');
    if (grid) {
        var selectedRowKeys = grid.GetSelectedKeysOnPage();
        if (selectedRowKeys.length > 0) {
            var constanciaId = selectedRowKeys[0]; // Obtener el ID de la constancia seleccionada
            $.ajax({
                type: "POST",
                url: "Solicitudes_finalizadas.aspx/GuardarConstancia",
                data: JSON.stringify({ constanciaId: constanciaId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d) {
                        Swal.fire({
                            title: "¡Éxito!",
                            text: "Constancia guardada correctamente.",
                            icon: "success",
                            confirmButtonColor: "#1F497D"
                        });
                    } else {
                        Swal.fire({
                            title: "¡Error!",
                            text: "No se pudo guardar la constancia.",
                            icon: "error",
                            confirmButtonColor: "#1F497D"
                        });
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    Swal.fire({
                        title: "¡Error!",
                        text: "Ocurrió un error al intentar guardar la constancia.",
                        icon: "error",
                        confirmButtonColor: "#1F497D"
                    });
                }
            });
        } else {
            Swal.fire({
                title: "¡Alerta!",
                text: "Por favor seleccione una fila para guardar la constancia.",
                icon: "warning",
                confirmButtonColor: "#1F497D"
            });
        }
    }
}