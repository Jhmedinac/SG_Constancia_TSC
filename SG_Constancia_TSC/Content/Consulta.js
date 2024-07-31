var globalIdConstancia;
var globalIdClave;
function Guardar_Datos_Complete(s, e) {
    var respuestaJSON = e.result;
    var respuesta = JSON.parse(respuestaJSON);
    var Retorno = respuesta.codeResult;
    var Mens = respuesta.message;

    //console.log('Respuesta:', respuesta); // Debug: Imprimir la respuesta en consola

    if (Retorno == 0) {
        Enviar.Hide();
        popupResumen.Hide(); 
        Relacionado.Show();
        ckPolitica.SetVisible(false); // Ocultar el checkbox después de mostrar el comprobante
        ckPolitica.SetChecked(false);
        btnEnviarCodigo.SetVisible(true); // Hacer visible el botón después de mostrar el comprobante
        
      /*  SetCampos();*/
    }
}

function SetCampos() {
        tbNombre.SetText(''),
        tbApellido.SetText(''),
        tbCorreo.SetText(''),
        tbConfirmCorreo.SetText(''),
        //tbDependencia.SetText(''),
            //tbFechaIngreso.SetText(''),
        tbTelefono.SetText(''),
        //CmbCountry.SetText(''),
        //CmbTipoDeclaracion.SetText(''),
        tbIdentidad.SetText(''),
        ckPolitica.SetChecked(false);

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
    var ckPolitica = ASPxClientControl.GetControlCollection().GetByName("ckPolitica");
    var btnEnviarCodigo = ASPxClientControl.GetControlCollection().GetByName("btnEnviarCodigo");

    //var lblUploadStatus = document.getElementById('<%= lblUploadStatus.ClientID %>');
    //var fileUpload = document.getElementById('fileUpload');

    // Debugging to ensure elements are fetched correctly
   /* console.log(ckPolitica, btnEnviarCodigo, lblUploadStatus, fileUpload);*/

    //var fileUpload1 = document.getElementById('<%= fileUpload1.ClientID %>');
    //var fileUpload2 = document.getElementById('<%= fileUpload2.ClientID %>');

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
        url: "SolicitudTSC1.aspx/GetSessionValues",
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
        //tbDependencia.GetText(),
        //CmbCountry.GetText(),
        //CmbTipoDeclaracion.GetText(),
        /*tbIdentidad.GetText(),*/
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
        ckPolitica.SetVisible(false);
        btnEnviarCodigo.SetVisible(false);
        tbToken.SetText('');

        var tbIdentidad = ASPxClientControl.GetControlCollection().GetByName("tbIdentidad");
        var tbNombre = ASPxClientControl.GetControlCollection().GetByName("tbNombre");
        var tbApellido = ASPxClientControl.GetControlCollection().GetByName("tbApellido");
        var tbCorreo = ASPxClientControl.GetControlCollection().GetByName("tbCorreo");
        var tbTelefono = ASPxClientControl.GetControlCollection().GetByName("tbTelefono");
        var tbDireccion = ASPxClientControl.GetControlCollection().GetByName("tbDireccion");

        if (tbIdentidad && tbNombre && tbApellido && tbCorreo && tbTelefono && tbDireccion) {
            $.ajax({
                type: "POST",
                url: "CreateSolicitudHandler.ashx",
                data: {
                    tbIdentidad: tbIdentidad.GetValue(),
                    tbNombre: tbNombre.GetValue(),
                    tbApellido: tbApellido.GetValue(),
                    tbCorreo: tbCorreo.GetValue(),
                    tbTelefono: tbTelefono.GetValue(),
                    tbDireccion: tbDireccion.GetValue()
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

                        // Añadir archivos y sus respectivos metadatos
                        if (fileUpload.files.length > 0) {
                            formData.append("fileIdentidad", fileUpload.files[0]);
                            formData.append("flexFieldKey", "CODIGO_IDENTIDAD");
                            formData.append("fileIdKey", fileIdIdent);
                        }
                        if (fileUpload1.files.length > 0) {
                            formData.append("fileSolicitud", fileUpload1.files[0]);
                            formData.append("flexFieldKey", "CODIGO_SOLICITUD");
                            formData.append("fileIdKey", fileIdsolicitud);
                        }
                        if (fileUpload2.files.length > 0) {
                            formData.append("fileRecibo", fileUpload2.files[0]);
                            formData.append("flexFieldKey", "CODIGO_RECIBO");
                            formData.append("fileIdKey", fileIdrecib);
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
            //$.ajax({
            //    type: "POST",
            //    url: "CreateSolicitudHandler.ashx",
            //    data: {
            //        tbIdentidad: tbIdentidad.GetValue(),
            //        tbNombre: tbNombre.GetValue(),
            //        tbApellido: tbApellido.GetValue(),
            //        tbCorreo: tbCorreo.GetValue(),
            //        tbTelefono: tbTelefono.GetValue(),
            //        tbDireccion: tbDireccion.GetValue()
            //    },
            //    dataType: "json", // Asegúrate de que la respuesta sea manejada como JSON
            //    success: function (response) {
            //        try {
            //            /*var result = JSON.parse(response);*/
            //            var idConstancia = response.Idconstancia;
            //            var clave = response.Clave;

            //            // Subir los archivos una vez que se ha creado la solicitud
            //            var formData = new FormData();
            //            var formData1 = new FormData();
            //            var formData2 = new FormData();

            //            var fileUpload = document.getElementById('fileUpload');
            //            var fileUpload1 = document.getElementById('fileUpload1');
            //            var fileUpload2 = document.getElementById('fileUpload2');

            //            // Asignar FlexfieldKey y FileIdKey
            //            //var fileIdIdent = '<%= UtilClass.UtilClass.FileId_ident %>'; // Valor desde el servidor
            //            //var fileIdsolicitud = '<%= UtilClass.UtilClass.FileId_solicitud %>'; // Valor desde el servidor
            //            //var fileIdrecib = '<%= UtilClass.UtilClass.FileId_recibo %>'; // Valor desde el servidor

            //            if (fileUpload.files.length > 0) {
            //                formData.append("file", fileUpload.files[0]);
            //                formData.append("flexFieldKey", "CODIGO_IDENTIDAD"); // Asigna el FlexfieldKey IDENTIDAD correspondiente
            //                formData.append("fileIdKey", fileIdIdent); // Asigna el FlexfieldKey IDENTIDAD correspondiente
            //            }
            //            if (fileUpload1.files.length > 0) {
            //                formData1.append("file", fileUpload1.files[0]);
            //                formData1.append("flexFieldKey", "CODIGO_SOLICITUD"); // Asigna el FlexfieldKey SOLICITUD correspondiente
            //                formData1.append("fileIdKey", fileIdsolicitud); // Asigna el FlexfieldKey IDENTIDAD correspondiente
            //            }
            //            if (fileUpload2.files.length > 0) {
            //                formData2.append("file", fileUpload2.files[0]);
            //                formData2.append("flexFieldKey", "CODIGO_RECIBO"); // Otro archivo de tipo RECIBO si es necesario
            //                formData2.append("fileIdKey", fileIdrecib); // Asigna el FlexfieldKey IDENTIDAD correspondiente
            //            }

            //            // Incluir el idConstancia en los formData
            //            formData.append("flexFieldValue", idConstancia);
            //            formData1.append("flexFieldValue", idConstancia);
            //            formData2.append("flexFieldValue", idConstancia);

            //            var xhr = new XMLHttpRequest();
            //            var xhr1 = new XMLHttpRequest();
            //            var xhr2 = new XMLHttpRequest();

            //            xhr.open("POST", "UploadFilesHandler.ashx", true);
            //            xhr1.open("POST", "UploadFilesHandler.ashx", true);
            //            xhr2.open("POST", "UploadFilesHandler.ashx", true);

            //            xhr2.onload = function () {
            //                if (xhr1.status === 200) {
            //                    alert("Archivo de tipo RECIBO subido con éxito.");
            //                } else {
            //                    alert("Error al subir el archivo de tipo RECIBO.");
            //                }
            //            };
            //            xhr1.onload = function () {
            //                if (xhr1.status === 200) {
            //                    alert("Archivo de tipo SOLICITUD subido con éxito.");
            //                } else {
            //                    alert("Error al subir el archivo de tipo SOLICITUD.");
            //                }
            //            };

            //            xhr.onload = function () {
            //                if (xhr.status === 200) {
            //                    alert("Archivo de tipo IDENTIDAD subido con éxito.");
            //                    ASPxCallback_Guardar_Datos.PerformCallback();
            //                } else {
            //                    alert("Error al subir el archivo de tipo IDENTIDAD.");
            //                }
            //            };

            //            xhr.send(formData);
            //            xhr1.send(formData1);
            //            xhr2.send(formData2);
            //        } catch (e) {
            //            console.error("Error parsing JSON response: ", e);
            //            console.error("Response received: ", response);
            //            alert("Error en la respuesta del servidor.");
            //        }
            //    },

                        
                
            //    error: function () {
            //        alert("Error al crear la solicitud.");
            //    }
            //});
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

//function TokenVerificationComplete(result) {
//    if (result === "success") {
//        //popupToken.Hide();
//        //ckPolitica.SetVisible(false); // Hacer visible el checkbox después de la verificación exitosa
//        ////ckPolitica.SetChecked(false);
//        //btnEnviarCodigo.SetVisible(false); // Ocultar el botón después de la verificación exitosa
//        //tbToken.SetText('');
//        //ASPxCallback_Guardar_Datos.PerformCallback();
//        popupToken.Hide();
//        ckPolitica.SetVisible(false);
//        btnEnviarCodigo.SetVisible(false);
//        tbToken.SetText('');

//        var tbIdentidad = ASPxClientControl.GetControlCollection().GetByName("tbIdentidad")
//        var tbNombre = ASPxClientControl.GetControlCollection().GetByName("tbNombre");
//        var tbApellido = ASPxClientControl.GetControlCollection().GetByName("tbApellido");
//        var tbCorreo = ASPxClientControl.GetControlCollection().GetByName("tbCorreo");
//        var tbTelefono = ASPxClientControl.GetControlCollection().GetByName("tbTelefono");
//        var tbDireccion = ASPxClientControl.GetControlCollection().GetByName("tbDireccion");
//        //var nombre = document.getElementById('tbNombre');
//        //var identidad = document.getElementById('tbIdentidad');
//        //var nombre = document.getElementById('tbNombre');
//        //var apellido = document.getElementById('tbApellido');
//        //var correo = document.getElementById('tbCorreo');
//        //var telefono = document.getElementById('tbTelefono');
//        //var direccion = document.getElementById('tbDireccion');

//        if (tbIdentidad && tbNombre && tbApellido && tbCorreo && tbTelefono && tbDireccion) {
//        // Crear la solicitud primero y obtener el idString
//            $.ajax({
//                type: "POST",
//                url: "CreateSolicitudHandler.ashx",
//                data: {
//                    tbIdentidad: tbIdentidad.GetValue(),
//                    tbNombre: tbNombre.GetValue(),
//                    tbApellido: tbApellido.GetValue(),
//                    tbCorreo: tbCorreo.GetValue(),
//                    tbTelefono: tbTelefono.GetValue(),
//                    tbDireccion: tbDireccion.GetValue()
//                },
//                success: function (idString, Idconstancia) {
//                    // Subir los archivos una vez que se ha creado la solicitud
//                    var formData = new FormData();
//                    var formData1 = new FormData();
//                    var formData2 = new FormData();

//                    var fileUpload = document.getElementById('fileUpload');
//                    var fileUpload1 = document.getElementById('fileUpload1');
//                    var fileUpload2 = document.getElementById('fileUpload2');

//                    var Idconstancia = Idconstancia;

//                    // Asignar FlexfieldKey y FileIdKey
//                    var fileIdIdent = '<%= UtilClass.UtilClass.FileId_ident %>'; // Valor desde el servidor
//                    var fileIdsolicitud = '<%= UtilClass.UtilClass.FileId_solicitud %>'; // Valor desde el servidor
//                    var fileIdrecib = '<%= UtilClass.UtilClass.FileId_recibo %>'; // Valor desde el servidor

//                    if (fileUpload.files.length > 0) {
//                        formData.append("file", fileUpload.files[0]);
//                        formData.append("flexFieldKey", "IDENTIDAD"); // Asigna el FlexfieldKey IDENTIDAD correspondiente
//                        formData.append("fileIdKey", fileIdIdent); // Asigna el FlexfieldKey IDENTIDAD correspondiente
//                    }
//                    if (fileUpload1.files.length > 0) {
//                        formData1.append("file", fileUpload1.files[0]);
//                        formData1.append("flexFieldKey", "SOLICITUD"); // Asigna el FlexfieldKey SOLICITUD correspondiente
//                        formData1.append("fileIdKey", fileIdsolicitud); // Asigna el FlexfieldKey IDENTIDAD correspondiente
//                    }
//                    if (fileUpload2.files.length > 0) {
//                        formData2.append("file", fileUpload2.files[0]);
//                        formData2.append("flexFieldKey", "RECIBO"); // Otro archivo de tipo RECIBO si es necesario
//                        formData2.append("fileIdKey", fileIdrecib); // Asigna el FlexfieldKey IDENTIDAD correspondiente
//                    }

//                    // Incluir el idString en los formData
//                    formData.append("flexFieldValue", Idconstancia);
//                    formData1.append("flexFieldValue", Idconstancia);
//                    formData2.append("flexFieldValue", Idconstancia);

//                    var xhr = new XMLHttpRequest();
//                    var xhr1 = new XMLHttpRequest();
//                    var xhr2 = new XMLHttpRequest();

//                    xhr.open("POST", "UploadFilesHandler.ashx", true);
//                    xhr1.open("POST", "UploadFilesHandler.ashx", true);
//                    xhr2.open("POST", "UploadFilesHandler.ashx", true);

//                    xhr2.onload = function () {
//                        if (xhr1.status === 200) {
//                            alert("Archivo de tipo RECIBO subido con éxito.");
//                        } else {
//                            alert("Error al subir el archivo de tipo RECIBO.");
//                        }
//                    };
//                    xhr1.onload = function () {
//                        if (xhr1.status === 200) {
//                            alert("Archivo de tipo SOLICITUD subido con éxito.");
//                        } else {
//                            alert("Error al subir el archivo de tipo SOLICITUD.");
//                        }
//                    };


//                    xhr.onload = function () {
//                        if (xhr.status === 200) {
//                            alert("Archivo de tipo IDENTIDAD subido con éxito.");
//                            ASPxCallback_Guardar_Datos.PerformCallback();
//                        } else {
//                            alert("Error al subir el archivo de tipo IDENTIDAD.");
//                        }
//                    };

//                    xhr.send(formData);
//                },
//                error: function () {
//                    alert("Error al crear la solicitud.");
//                }
//            });
//        } else {
//            alert("Uno o más elementos del formulario no se encontraron.");
//        }


//    } else if (result === "incorrect") {
//        alert('Código de verificación incorrecto. Por favor, inténtelo de nuevo.');
//        tbToken.SetText('');
//        // No cerramos el popupToken si el código es incorrecto
//    } else if (result === "expired") {
//        alert('El código de verificación ha expirado. Por favor, solicite un nuevo código.');
//        popupToken.Hide(); // Cerrar el popupToken si el código ha expirado
//        tbToken.SetText('');
//    } else {
//        alert('Error en la verificación del código. Por favor, inténtelo de nuevo.');
//        popupToken.Hide(); // Cerrar el popupToken en caso de error general
//        tbToken.SetText('');
//    }

  

    
//}


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


    //var fileUpload = document.getElementById('<%= fileUpload.ClientID %>');
    //var fileUpload1 = document.getElementById('<%= fileUpload1.ClientID %>');
    //var fileUpload2 = document.getElementById('<%= fileUpload2.ClientID %>');

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
            <li>Archivo de Solicitud: ${strfileNames}</li>
            <li>Archivo de Identidad: ${strfileNames1}</li>
            <li>Archivo de Recibo: ${strfileNames2}</li>
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