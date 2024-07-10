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