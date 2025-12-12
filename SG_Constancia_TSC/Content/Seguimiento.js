

function showConfirmationMessage3() {
   
    var constanciaId = document.getElementById("txtConstanciaId.ClientID").value;

    $.ajax({
        type: "POST",
        url: "Seguimiento.aspx/GetSessionValues",
        data: JSON.stringify({ constanciaId: constanciaId }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            function toggleClave() {
                var textbox = ASPxClientControl.GetControlCollection().GetByName("txtClave");
                var input = textbox.GetInputElement();

                if (input.type === "password") {
                    input.type = "text";
                } else {
                    input.type = "password";
                }
            }
            var values = response.d.split("|");
            var constanciaId = values[0];
            var estado = values[1];
            var fechaCreacion = values[2];
            var otrosDatos = values[3];

            var tableHtml = `
<div style="background:#fff; padding:24px; max-width:600px; margin:0 auto; font-family:'Segoe UI', sans-serif; border-radius:10px; box-shadow:0 5px 15px rgba(0,0,0,0.1); color:#333;">
    <h3 style="text-align:center; color:#1F497D; margin-bottom:20px;">📄 Estado de la Solicitud</h3>
    <table style="width:100%; border-collapse:collapse;">
        <tr>
            <td style="padding:8px; font-weight:bold;">Número de Solicitud:</td>
            <td style="padding:8px;">${constanciaId}</td>
        </tr>
        <tr>
            <td style="padding:8px; font-weight:bold;">Estado:</td>
            <td style="padding:8px; color:${estado === 'Aprobada' ? '#28a745' : '#d62d20'}; font-weight:bold;">${estado}</td>
        </tr>
        <tr>
            <td style="padding:8px; font-weight:bold;">Fecha y Hora de Creación:</td>
            <td style="padding:8px;">${fechaCreacion}</td>
        </tr>
        <tr>
            <td style="padding:8px; font-weight:bold;">Otros Datos:</td>
            <td style="padding:8px;">${otrosDatos}</td>
        </tr>
    </table>
    <div style="text-align:center; margin-top:30px;">
        <a href="https://www.tsc.gob.hn/" style="background:#1F497D; color:#fff; padding:10px 20px; border-radius:6px; text-decoration:none; display:inline-block;">Salir</a>
    </div>
</div>`;
            document.getElementById("popupContent").innerHTML = tableHtml;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error al obtener valores de la constancia: " + thrownError);
        }
    });
}


/*Función para el formulario de seguimiento */
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

            var tableHtml = `
<div style="background:#fff; padding:24px; max-width:600px; margin:0 auto; font-family:'Segoe UI', sans-serif; border-radius:12px; box-shadow:0 5px 20px rgba(0,0,0,0.1); color:#333;">
    <h4 style="text-align:center; color:#1F497D; margin-bottom:24px;">📄 Estado de la Solicitud</h4>

    <table style="width:100%; border-collapse:collapse; margin-bottom:20px;">
        <tr>
            <td style="padding:10px; font-weight:bold; width:40%;">Número de Solicitud:</td>
            <td style="padding:10px;">${constanciaId}</td>
        </tr>
        <tr>
            <td style="padding:10px; font-weight:bold;">Estado:</td>
            <td style="padding:10px; color:${estado.toLowerCase() === 'aprobada' || estado.toLowerCase() === 'finalizada' ? '#28a745' : '#292929'}; font-weight:bold;">${estado}</td>
        </tr>
        <tr>
            <td style="padding:10px; font-weight:bold;">Fecha y hora de Creación:</td>
            <td style="padding:10px;">${fechaCreacion}</td>
        </tr>
        <tr>
            <td style="padding:10px; font-weight:bold;">Observación:</td>
            <td style="padding:10px;">${otrosDatos}</td>
        </tr>
    </table>`;

            if (estado.toLowerCase() === "finalizada" && archivoconstanciaBase64) {
                tableHtml += `
    <div style="text-align:center; margin-bottom:20px;">
        <button id="btnDescargarConstancia" class="btn btn-success" style="background-color:#28a745; border:none; padding:10px 20px; border-radius:6px; color:#fff; font-weight:500; cursor:pointer;">
            📥 Descargar Constancia
        </button>
    </div>`;
            }

            // Manejador para el botón de descarga
            if (estado.toLowerCase() === "finalizada" && archivoconstanciaBase64) {
                setTimeout(function () {
                    
                    document.getElementById("btnDescargarConstancia").addEventListener("click", function (e) {
                        e.preventDefault(); // <--- Previene recarga o cierre del popup
                        e.stopPropagation(); // <--- Evita que eventos se propaguen y afecten al popup

                        downloadBase64File(archivoconstanciaBase64, "Constancia_" + constanciaId + ".pdf");
                    });
                }, 100);
            }


            document.getElementById("popupContent").innerHTML = tableHtml;

            // Mostrar el popup
            setTimeout(function () { popupSeguimiento.Show(); }, 500);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error al obtener valores de la constancia: " + thrownError);
        }
    });
}


setTimeout(function () {
    var lbl = document.getElementById('<%= lblMensaje.ClientID %>');
    if (lbl) lbl.innerText = '';
}, 7000); // Oculta después de 7 segundos
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
                text: "Por favor seleccione una fila para generar la constancia.",
                icon: "warning",
                confirmButtonColor: "#1F497D"
            });
        }
    }
}
