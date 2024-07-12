function Guardar_Datos_Complete(s, e) {
    var respuestaJSON = e.result;
    var respuesta = JSON.parse(respuestaJSON);
    var Retorno = respuesta.Retorno;
    var Mens = respuesta.Mensaje;

    console.log('Respuesta:', respuesta); // Debug: Imprimir la respuesta en consola

    if (Retorno == 1) {
        Enviar.Hide();
        Relacionado.Show();
        ckPolitica.SetVisible(false); // Ocultar el checkbox después de mostrar el comprobante
        ckPolitica.SetChecked(false);
        btnEnviarCodigo.SetVisible(true); // Hacer visible el botón después de mostrar el comprobante
        SetCampos();
    }
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

    function Terminos(s, e) {
        // Desactivar el botón si el checkbox no está marcado
        if (!s.GetChecked()) {
            ASPxButton2.SetEnabled(false);
            return; // Detener la ejecución adicional de la función
        }

        // Lista de campos a validar
        var campos = [
            tbNombre.GetText(),
            tbApellido.GetText(),
            tbCorreo.GetText(),
            tbDependencia.GetText(),
            CmbCountry.GetText(),
            CmbTipoDeclaracion.GetText(),
            tbIdentidad.GetText(),
        ];

        // Revisa si alguno de los campos está vacío o es null
        var camposVacios = campos.some(function (valor) {
            return valor === '' || valor === null;
        });

        if (camposVacios) {
            Swal.fire({
                title: "¡Alerta!",
                text: "Debe de llenar los datos requeridos del formulario para enviar la solicitud del Pre-Registro",
                icon: "warning",
                //showCancelButton: true,
                confirmButtonColor: "#1F497D",
                /*cancelButtonColor: "#d33",*/

            });
            ckPolitica.SetChecked(false);
            //alert("¡Alerta! Debe de llenar los datos requeridos del formulario para enviar la solicitud del Pre-registro");
            console.log(campos); // Esto imprimirá los valores de los campos, podrías quitarlo si no es necesario
        }
        else {

            if (tbIdentidad.GetText() === '') {
                tbIdentidad.SetFocus();
                ckPolitica.SetChecked(false);
                return;
            }

            var valor = tbIdentidad.GetText(); // Suponiendo que tbIdentidad.GetText() obtiene el valor del campo de entrada de texto

            ////// Validar el formato del valor
            if (!validarFormatoIDN(valor)) {
                tbIdentidad.SetFocus();
                ckPolitica.SetChecked(false);
                return;
            }


            var correo = tbCorreo.GetText();
            if (!validarFormatoCorreo(correo)) {
                tbCorreo.SetFocus();// Enfocar el campo de correo electrónico
                ckPolitica.SetChecked(false);
                return;
            }


            ShowEnviar(); // Función para mostrar la opción de enviar o algo relacionado
            ASPxButton2.SetEnabled(true);
            console.log(campos);
        }

    }
}