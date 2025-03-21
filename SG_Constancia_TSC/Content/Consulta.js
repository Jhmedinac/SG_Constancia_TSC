var globalIdConstancia;
var globalIdClave;
function Guardar_Datos_Complete(s, e) {
    var respuestaJSON = e.result;
    var respuesta = JSON.parse(respuestaJSON);
    var Retorno = respuesta.codeResult;
    var Mens = respuesta.message;

    //console.log('Respuesta:', respuesta); // Debug: Imprimir la respuesta en consola

    if (Retorno == 0) {
       /* Enviar.Hide();*/
        popupResumen.Hide(); 
        Relacionado.Show();
        //ckPolitica.SetVisible(false); // Ocultar el checkbox después de mostrar el comprobante
        //ckPolitica.SetChecked(false);
        btnEnviarCodigo.SetVisible(true); // Hacer visible el botón después de mostrar el comprobante
        
      /*  SetCampos();*/
    }
}

function SetCampos() {
    tbNombre.SetText(''),
        tbApellido.SetText(''),
        tbCorreo.SetText(''),
        tbConfirmCorreo.SetText(''),
        tbDireccion.SetText(''),
        fileUpload.value = '';   // Corregido
        fileUpload1.value = '';  // Corregido
        fileUpload2.value = '';  // Corregido
        tbTelefono.SetText(''),
        tbIdentidad.SetText(''),
      /*  ckPolitica.SetChecked(false);*/
    // Usando innerHTML para las etiquetas (Labels)
    
        lblUploadStatus.innerHTML = '';
        lblError.innerHTML = '';
        lblUploadStatus1.innerHTML = '';
        lblError1.innerHTML = '';
        lblUploadStatus2.innerHTML = '';
        lblError2.innerHTML = '';
        lblSize.innerHTML = '';
        lblSize1.innerHTML = '';
        lblSize2.innerHTML = '';
        lblSelected.innerHTML = '';
        lblSelected1.innerHTML = '';
        lblSelected2.innerHTML = '';
        
}
       
function btnEnviar_ClientClick(s, e) {
    // Realizar validaciones adicionales si es necesario
    e.processOnServer = true; // Permitir el postback para ejecutar la lógica del servidor
}
function toUpperCase(id) {
    var textbox = document.getElementById(id);
    textbox.value = textbox.value.toUpperCase();
}

function Terminos(s, e) {
/*    var ckPolitica = ASPxClientControl.GetControlCollection().GetByName("ckPolitica");*/
    var btnEnviarCodigo = ASPxClientControl.GetControlCollection().GetByName("btnEnviarCodigo");

    if (s.GetChecked()) {
        btnEnviarCodigo.SetEnabled(true);
        Enviar.Show();
    } else {
        btnEnviarCodigo.SetEnabled(false);
    }
}

function popup_Shown_comprobante(s, e) {
    var ckPolitica = ASPxClientControl.GetControlCollection().GetByName("ckPolitica");
    var btnEnviarCodigo = ASPxClientControl.GetControlCollection().GetByName("btnEnviarCodigo");

    if (ckPolitica.GetChecked()) {
        btnEnviarCodigo.SetEnabled(true);
    } else {
        btnEnviarCodigo.SetEnabled(false);
    }
}

function ClosePopupRelacionado(s, e) {
    var ckPolitica = ASPxClientControl.GetControlCollection().GetByName("ckPolitica");
    var btnEnviarCodigo = ASPxClientControl.GetControlCollection().GetByName("btnEnviarCodigo");
    Relacionado.Hide();
    if (!ckPolitica.GetChecked()) {
        btnEnviarCodigo.SetEnabled(false);
    }
}

function showConfirmationMessage1() {
    var email = tbCorreo.GetText();

    $.ajax({
        type: "POST",
        url: "Solicitud.aspx/GetSessionValues",
        data: JSON.stringify({
            email: email,
            constanciaId: globalIdConstancia,
            randPassword: globalIdClave
        }), // Correctly closed JSON object
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var values = response.d.split("|");
            var qrCodeImageUrl = values[0]; // Nueva URL de la imagen del código QR
            //var constanciaId = values[1];
            //var randPassword = values[2];

            var tableHtml = "<table border='0' width='100%'>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2' align='center'><strong><font size='+2'>Solicitud recibida!</font></strong></td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'></td></tr>" +
                "<tr><td colspan='2'>Copia de su solicitud ha sido enviada a su correo electrónico.</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>Si desea más adelante monitorear su solicitud, siga las siguientes instrucciones:</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>1) Guarde el número de su solicitud y la clave que se le indican en esta página.</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>2) Cuando desee monitorear su solicitud de constancia, ingrese al portal <font color='#d62d20'>https://www.tsc.gob.hn/</font>.</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>3) Posteriormente, ingrese a la viñeta de solicitud de constancias.</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>4) Ingrese al enlace Seguimiento de solicitud de constancias e ingrese los datos solicitados.</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>Su número de solicitud es: <font color='#d62d20'>" + globalIdConstancia + "</font>.</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>Su clave para seguimiento de Constancia es: <font color='#d62d20'>" + globalIdClave + "</font>.</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2' align='center'><img src='" + qrCodeImageUrl + "' alt='Código QR'></td></tr>" + // Añadir la imagen del código QR
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>" +
                "</table>";

            document.getElementById("popupContent").innerHTML = tableHtml;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error al obtener valores de sesión: " + thrownError);
        }
    });
    SetCampos();
}


function btnVerificarToken_Click(s, e) {
    // Verificar el token ingresado
    var inputToken = tbToken.GetText();
    ASPxCallback_VerificarToken.PerformCallback(inputToken);
}

function btnEnviarCodigo_Click(s, e) {
    var campos = [
        tbNombre.GetText(),
        tbApellido.GetText(),
        tbCorreo.GetText(),
        tbConfirmCorreo.GetText(),
        tbTelefono.GetText(),
        tbIdentidad.GetText(),

    ];

    var camposVacios = campos.some(function (valor) {
        return valor === '' || valor === null;
    });

    if (camposVacios) {
        Swal.fire({
            title: "¡Alerta!",
            text: "Debe llenar los datos requeridos del formulario para hacer el envio del código de verificación",
            icon: "warning",
            confirmButtonColor: "#1F497D",
        });
    } else {
        // Enviar el token al correo
        var email = tbCorreo.GetText();
        if (validarFormatoCorreo(email)) {
            // Llamar al servidor para enviar el token
            ASPxCallback_EnviarToken.PerformCallback(email);
            popupToken.Show();
        } else {
            Swal.fire({
                title: "¡Alerta!",
                text: "Por favor ingrese un correo electrónico válido.",
                icon: "warning",
                confirmButtonColor: "#1F497D",
            });
            //alert('Por favor ingrese un correo electrónico válido.');
        }
    }
}
function TokenVerificationComplete(result) {
    if (result === "success") {
        popupToken.Hide();
        //ckPolitica.SetVisible(false);
        btnEnviarCodigo.SetVisible(false);
        tbToken.SetText('');

        var tbIdentidad = ASPxClientControl.GetControlCollection().GetByName("tbIdentidad");
        var tbNombre = ASPxClientControl.GetControlCollection().GetByName("tbNombre");
        var tbApellido = ASPxClientControl.GetControlCollection().GetByName("tbApellido");
        var tbCorreo = ASPxClientControl.GetControlCollection().GetByName("tbCorreo");
        var tbTelefono = ASPxClientControl.GetControlCollection().GetByName("tbTelefono");
        /*var tbDireccion = ASPxClientControl.GetControlCollection().GetByName("tbDireccion");*/
        var tbDireccion = "";

        if (tbIdentidad && tbNombre && tbApellido && tbCorreo && tbTelefono ) {
            $.ajax({
                type: "POST",
                url: "CreateSolicitudHandler.ashx",
                data: {
                    tbIdentidad: tbIdentidad.GetValue(),
                    tbNombre: tbNombre.GetValue(),
                    tbApellido: tbApellido.GetValue(),
                    tbCorreo: tbCorreo.GetValue(),
                    tbTelefono: tbTelefono.GetValue(),
                    /*tbDireccion: tbDireccion.GetValue()*/
                      tbDireccion: ""
                },
                dataType: "json",
                success: function (response) {
                    try {
                        var idConstancia = response.Idconstancia;
                        globalIdConstancia = idConstancia;
                        var clave = response.Clave;
                        globalIdClave = clave;

                        // Crear un solo objeto FormData para todos los archivos
                        var formData = new FormData();

                        var fileUpload = document.getElementById('fileUpload');
                        var fileUpload1 = document.getElementById('fileUpload1');
                        var fileUpload2 = document.getElementById('fileUpload2');

                        // Tamaño máximo permitido en bytes (5 MB)
                        var maxSize = 5 * 1024 * 1024;
                        //var maxSize = 5 * 1024;

                        // Añadir archivos y sus respectivos metadatos con validación de tamaño
                        if (fileUpload.files.length > 0) {
                            if (fileUpload.files[0].size <= maxSize) {
                                formData.append("fileIdentidad", fileUpload.files[0]);
                                formData.append("flexFieldKey", "CODIGO_IDENTIDAD");
                                formData.append("fileIdKey", fileIdIdent);
                            } else {
                                alert("El archivo de identidad supera el tamaño máximo permitido de 5 MB.");
                                return;
                            }
                        }
                        if (fileUpload1.files.length > 0) {
                            if (fileUpload1.files[0].size <= maxSize) {
                                formData.append("fileSolicitud", fileUpload1.files[0]);
                                formData.append("flexFieldKey", "CODIGO_SOLICITUD");
                                formData.append("fileIdKey", fileIdsolicitud);
                            } else {
                                alert("El archivo de solicitud supera el tamaño máximo permitido de 5 MB.");
                                return;
                            }
                        }
                        if (fileUpload2.files.length > 0) {
                            if (fileUpload2.files[0].size <= maxSize) {
                                formData.append("fileRecibo", fileUpload2.files[0]);
                                formData.append("flexFieldKey", "CODIGO_RECIBO");
                                formData.append("fileIdKey", fileIdrecib);
                            } else {
                                alert("El archivo de recibo supera el tamaño máximo permitido de 5 MB.");
                                return;
                            }
                        }

                        // Incluir el idConstancia en el formData
                        formData.append("flexFieldValue", idConstancia);

                        // Realizar la solicitud AJAX para subir los archivos
                        $.ajax({
                            url: "UploadFilesHandler.ashx",
                            type: "POST",
                            data: formData,
                            processData: false,
                            contentType: false,
                            success: function (uploadResponse) {
                                alert("Archivos subidos con éxito.");
                                ASPxCallback_Guardar_Datos.PerformCallback(JSON.stringify(uploadResponse));
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                console.error("Error al subir los archivos: ", errorThrown);
                                alert("Error al subir los archivos.");
                            }
                        });
                    } catch (e) {
                        console.error("Error parsing JSON response: ", e);
                        console.error("Response received: ", response);
                        alert("Error en la respuesta del servidor.");
                    }
                },
                error: function () {
                    alert("Error al crear la solicitud.");
                }
            });
        } else {
            alert("Uno o más elementos del formulario no se encontraron.");
        }
    } else if (result === "incorrect") {
        alert('Código de verificación incorrecto. Por favor, inténtelo de nuevo.');
        tbToken.SetText('');
    } else if (result === "expired") {
        alert('El código de verificación ha expirado. Por favor, solicite un nuevo código.');
        popupToken.Hide();
        tbToken.SetText('');
    } else {
        alert('Error en la verificación del código. Por favor, inténtelo de nuevo.');
        popupToken.Hide();
        tbToken.SetText('');
    }
}






function updateFileNames() {
    // Obtener referencias a los controles ASPxFileUpload
    //var fileUpload = document.getElementById('fileUpload');
    //var fileUpload1 = document.getElementById('<%= fileUpload1.ClientID %>');
    //var fileUpload2 = document.getElementById('<%= fileUpload2.ClientID %>');

    //// Obtener nombres de archivos seleccionados
    //var fileNames = fileUpload ? Array.from(fileUpload.files).map(file => file.name).join(', ') : '';
    //var fileNames1 = fileUpload1 ? Array.from(fileUpload1.files).map(file => file.name).join(', ') : '';
    //var fileNames2 = fileUpload2 ? Array.from(fileUpload2.files).map(file => file.name).join(', ') : '';

    //// Mostrar nombres de archivos en las etiquetas
    //document.getElementById('<%= lblUploadStatus.ClientID %>').innerText = fileNames;
    //document.getElementById('<%= lblUploadStatus1.ClientID %>').innerText = fileNames1;
    //document.getElementById('<%= lblUploadStatus2.ClientID %>').innerText = fileNames2;
}

// Asegúrate de que el código se ejecute después de que el DOM esté cargado

function mostrarResumen() {
    var resumenContent = document.getElementById("resumenContent");

    // Obtener referencias a los controles ASPxTextBox
    var tbIdentidad = ASPxClientControl.GetControlCollection().GetByName("tbIdentidad");
    var tbNombre = ASPxClientControl.GetControlCollection().GetByName("tbNombre");
    var tbApellido = ASPxClientControl.GetControlCollection().GetByName("tbApellido");
    var tbCorreo = ASPxClientControl.GetControlCollection().GetByName("tbCorreo");
    var tbTelefono = ASPxClientControl.GetControlCollection().GetByName("tbTelefono");

       // Obtener nombres de archivos desde campos ocultos
    var fileUpload = document.getElementById('fileUpload');
    var fileUpload1 = document.getElementById('fileUpload1');
    var fileUpload2 = document.getElementById('fileUpload2');

    var strfileNames = fileUpload ? Array.from(fileUpload.files).map(file => file.name).join(', ') : '';
    var strfileNames1 = fileUpload1 ? Array.from(fileUpload1.files).map(file => file.name).join(', ') : '';
    var strfileNames2 = fileUpload2 ? Array.from(fileUpload2.files).map(file => file.name).join(', ') : '';
    // Construir el contenido del resumen
    resumenContent.innerHTML = `
        <p><strong>Identidad:</strong> ${tbIdentidad.GetValue()}</p>
        <p><strong>Nombre:</strong> ${tbNombre.GetValue()}</p>
        <p><strong>Apellido:</strong> ${tbApellido.GetValue()}</p>
        <p><strong>Email:</strong> ${tbCorreo.GetValue()}</p>
        <p><strong>Teléfono:</strong> ${tbTelefono.GetValue()}</p>
        <p><strong>Archivo(s) Seleccionado(s):</strong></p>
        <ul>
            <li><strong>Archivo de Solicitud:</strong> ${strfileNames}</li>
            <li><strong>Archivo de Identidad:</strong> ${strfileNames1}</li>
            <li><strong>Archivo de Recibo:</strong> ${strfileNames2}</li>
        </ul>
    `;

    // Mostrar el popup de resumen
    popupResumen.Show();
}

/*/ Función para obtener los nombres de los archivos seleccionados en un control FileUpload*/
function getFileNames(fileUpload) {
    var files = fileUpload.files;
    var fileNames = [];
    for (var i = 0; i < files.length; i++) {
        fileNames.push(files[i].name);
    }
    return fileNames.join(", ") || "No hay archivos seleccionados";
}

function showConfirmationMessage3() {
    var constanciaId = '<%= txtConstanciaId.Text %>';

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
            var enlaceDescarga = values[4];

            var tableHtml = "<table border='0' width='100%'>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2' align='center'><strong><font size='+2'>Estado de la Constancia</font></strong></td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td>Número de Constancia:</td><td>" + constanciaId + "</td></tr>" +
                "<tr><td>Estado:</td><td>" + estado + "</td></tr>" +
                "<tr><td>Fecha de Creación:</td><td>" + fechaCreacion + "</td></tr>" +
                "<tr><td>Observaciones:</td><td>" + otrosDatos + "</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>";

            // Agregar botón de descarga solo si la constancia está lista
            if (estado.toLowerCase() === "lista") {
                tableHtml += "<tr><td colspan='2' align='center'><a href='" + enlaceDescarga + "' class='btn btn-success' download>Descargar Constancia</a></td></tr>";
            }

            tableHtml += "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>";
            tableHtml += "</table>";

            document.getElementById("popupContent").innerHTML = tableHtml;
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error al obtener valores de la constancia: " + thrownError);
        }
    });
}


function showConfirmationMessage2() {
    /*var constanciaId = '<%= txtConstanciaId.Text %>';*/
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
            var enlaceDescarga = values[4];

            var tableHtml = "<table border='0' width='100%'>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2' align='center'><strong><font size='+2'>Estado de la Constancia</font></strong></td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td>Número de Constancia:</td><td>" + constanciaId + "</td></tr>" +
                "<tr><td>Estado:</td><td>" + estado + "</td></tr>" +
                "<tr><td>Fecha de Creación:</td><td>" + fechaCreacion + "</td></tr>" +
                "<tr><td>Observaciones:</td><td>" + otrosDatos + "</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>";

            if (estado.toLowerCase() === "lista") {
                tableHtml += "<tr><td colspan='2' align='center'><a href='" + enlaceDescarga + "' class='btn btn-success' download>Descargar Constancia</a></td></tr>";
            }

            tableHtml += "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>";
            tableHtml += "</table>";

            // Verificar si el popup existe
            var popupElement = document.getElementById("popupSeguimiento");
            if (!popupElement) {
                console.error("Error: No se encontró el elemento 'popupSeguimiento'.");
                return;
            }

            // Insertar contenido en el popup
            popupElement.innerHTML = tableHtml;

            // Mostrar el popup si es un DevExpress ASPxPopupControl
            if (typeof popupSeguimiento !== "undefined" && popupSeguimiento.Show) {
                popupSeguimiento.Show();
            } else {
                console.warn("popupSeguimiento no está definido como un control DevExpress.");
                popupElement.style.display = "block"; // Alternativa si es un div normal
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log("Error al obtener valores de la constancia: " + thrownError);
        }
    });
}

