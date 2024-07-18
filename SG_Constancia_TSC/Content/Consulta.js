function Guardar_Datos_Complete(s, e) {
    var respuestaJSON = e.result;
    var respuesta = JSON.parse(respuestaJSON);
    var Retorno = respuesta.Retorno;
    var Mens = respuesta.Mensaje;

    //console.log('Respuesta:', respuesta); // Debug: Imprimir la respuesta en consola

    if (Retorno == 1) {
        Enviar.Hide();
        popupResumen.Hide(); 
        Relacionado.Show();
        ckPolitica.SetVisible(false); // Ocultar el checkbox después de mostrar el comprobante
        ckPolitica.SetChecked(false);
        btnEnviarCodigo.SetVisible(true); // Hacer visible el botón después de mostrar el comprobante
        
        SetCampos();
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
    $.ajax({
        type: "POST",
        url: "SolicitudTSC1.aspx/GetSessionValues",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var values = response.d.split("|");
            var constanciaId = values[0];
            var randPassword = values[1];
            var qrCodeImageUrl = values[2]; // Nueva URL de la imagen del código QR

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
                "<tr><td colspan='2'>Su número de solicitud es: <font color='#d62d20'>" + constanciaId + "</font>.</td></tr>" +
                "<tr><td colspan='2'>&nbsp;</td></tr>" +
                "<tr><td colspan='2'>Su clave para seguimiento de Constancia es: <font color='#d62d20'>" + randPassword + "</font>.</td></tr>" +
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

//function showConfirmationMessage1() {
//    $.ajax({
//        type: "POST",
//        url: "SolicitudTSC1.aspx/GetSessionValues",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        success: function (response) {
//            var values = response.d.split("|");
//            var constanciaId = values[0];
//            var randPassword = values[1];

//            var tableHtml = "<table border='0' width='100%'>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2' align='center'><strong><font size='+2'>Solicitud recibida!</font></strong></td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2'></td></tr>" +
//                "<tr><td colspan='2'>Copia de su solicitud ha sido enviada a su correo electrónico.</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2'>Si desea más adelante monitorear su solicitud, siga las siguientes instrucciones:</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2'>1) Guarde el número de su solicitud y la clave que se le indican en esta página.</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2'>2) Cuando desee monitorear su solicitud de constancia, ingrese al portal <font color='#d62d20'>https://www.tsc.gob.hn/</font>.</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2'>3) Posteriormente, ingrese a la viñeta de solicitud de constancias.</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2'>4) Ingrese al enlace Seguimiento de solicitud de constancias e ingrese los datos solicitados.</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2'>Su número de solicitud es: <font color='#d62d20'>" + constanciaId + "</font>.</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2'>Su clave para seguimiento de Constancia es: <font color='#d62d20'>" + randPassword + "</font>.</td></tr>" +
//                "<tr><td colspan='2'>&nbsp;</td></tr>" +
//                "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>" +
//                "</table>";

//            document.getElementById("popupContent").innerHTML = tableHtml;
//        },
//        error: function (xhr, ajaxOptions, thrownError) {
//            console.log("Error al obtener valores de sesión: " + thrownError);
//        }
//    });
//}
//function showConfirmationMessage(constanciaId, randPassword) {


//    var tableHtml = "<table border=\"0\" width=\"100%\">" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\" align=\"center\"><strong><font size=\"+2\">Solicitud recibida!</font></strong></td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\"></td></tr>" +
//        "<tr><td colspan=\"2\">Copia de su solicitud ha sido enviada a su correo electrónico.</td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\">Si desea más adelante monitorear su solicitud, siga las siguientes instrucciones:</td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\">1) Guarde el número de su solicitud y la clave que se le indican en esta página.</td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\">2) Cuando desee monitorear su solicitud de constancia, ingrese al portal <font color='#d62d20'>https://www.tsc.gob.hn/</font>.</td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\">3) Posteriormente, ingrese a la viñeta de solicitud de constancias.</td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\">4) Ingrese al enlace Seguimiento de solicitud de constancias e ingrese los datos solicitados.</td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\">Su número de solicitud es: <font color='#d62d20\">" + constanciaId + "</font>.</td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\">Su clave para seguimiento de Constancia es: <font color='#d62d20\">" + randPassword + "</font>.</td></tr>" +
//        "<tr><td colspan=\"2\">&nbsp;</td></tr>" +
//        "<tr><td colspan=\"2\" align=\"center\"><a href=\"https://www.tsc.gob.hn/\" class=\"Letrapagina\">Salir</a></td></tr>" +
//        "</table>";

//    document.getElementById("popupContent").innerHTML = tableHtml;
//}
//function showConfirmationMessage() {
//    alert("Gracias por su solicitud. Su registro ha sido recibido con éxito. Nos pondremos en contacto con usted pronto vía email para proporcionarle más detalles.");
//}

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
        ckPolitica.SetVisible(false); // Hacer visible el checkbox después de la verificación exitosa
        //ckPolitica.SetChecked(false);
        btnEnviarCodigo.SetVisible(false); // Ocultar el botón después de la verificación exitosa
        tbToken.SetText('');
        ASPxCallback_Guardar_Datos.PerformCallback();
    } else if (result === "incorrect") {
        alert('Código de verificación incorrecto. Por favor, inténtelo de nuevo.');
        tbToken.SetText('');
        // No cerramos el popupToken si el código es incorrecto
    } else if (result === "expired") {
        alert('El código de verificación ha expirado. Por favor, solicite un nuevo código.');
        popupToken.Hide(); // Cerrar el popupToken si el código ha expirado
        tbToken.SetText('');
    } else {
        alert('Error en la verificación del código. Por favor, inténtelo de nuevo.');
        popupToken.Hide(); // Cerrar el popupToken en caso de error general
        tbToken.SetText('');
    }

  

    
}

function mostrarResumen() {
    var resumenContent = document.getElementById("resumenContent");

    // Obtener referencias a los controles ASPxTextBox
    var tbIdentidad = ASPxClientControl.GetControlCollection().GetByName("tbIdentidad");
    var tbNombre = ASPxClientControl.GetControlCollection().GetByName("tbNombre");
    var tbApellido = ASPxClientControl.GetControlCollection().GetByName("tbApellido");
    var tbCorreo = ASPxClientControl.GetControlCollection().GetByName("tbCorreo");
    var tbTelefono = ASPxClientControl.GetControlCollection().GetByName("tbTelefono");

    // Construir el contenido del resumen
    resumenContent.innerHTML = `
        <p><strong>Identidad:</strong> ${tbIdentidad.GetValue()}</p>
        <p><strong>Nombre:</strong> ${tbNombre.GetValue()}</p>
        <p><strong>Apellido:</strong> ${tbApellido.GetValue()}</p>
        <p><strong>Email:</strong> ${tbCorreo.GetValue()}</p>
        <p><strong>Teléfono:</strong> ${tbTelefono.GetValue()}</p>
    `;

    // Mostrar el popup de resumen
    popupResumen.Show();
}