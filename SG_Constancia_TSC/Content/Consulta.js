function btnEnviar_ClientClick(s, e) {
    // Realizar validaciones adicionales si es necesario
    e.processOnServer = true; // Permitir el postback para ejecutar la lógica del servidor
}
function toUpperCase(id) {
    var textbox = document.getElementById(id);
    textbox.value = textbox.value.toUpperCase();
}
function Terminos(s, e) {
    if (!s.GetChecked()) {
        btnEnviar.SetEnabled(false);
        btnEnviarCodigo.SetVisible(false);
        return;
    }
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
        ckPolitica.SetVisible(true); // Hacer visible el checkbox después de la verificación exitosa
        ckPolitica.SetChecked(false);
        btnEnviarCodigo.SetVisible(false); // Ocultar el botón después de la verificación exitosa
        tbToken.SetText('');
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