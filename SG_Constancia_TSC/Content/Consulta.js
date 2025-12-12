var globalIdConstancia;
var globalIdClave;

/*Validaciones de campos del paso uno */

function CustomValidateRTN(s, e) {
    var id = (e.value || "").trim(); // Evita null o undefined

    // Si el campo está vacío, que DevExpress maneje el RequiredField
    if (id === "") {
        e.isValid = true;
        return;
    }

    // Verifica que solo contenga números
    if (!/^\d+$/.test(id)) {
        e.isValid = false;
        e.errorText = "Solo se permiten números.";
        return;
    }

    // Verifica la longitud exacta de 14 dígitos
    if (id.length !== 14) {
        e.isValid = false;
        e.errorText = "El número de RTN debe tener 14 dígitos.";
        return;
    }

    // Si pasa todas las validaciones
    e.isValid = true;
}



function CustomValidateDNI(s, e) {
    var id = (e.value || "").trim(); // Evita null o undefined

    // Si el campo está vacío, que DevExpress maneje el RequiredField
    if (id === "") {
        e.isValid = true;
        return;
    }

    // Verifica que solo contenga números
    if (!/^\d+$/.test(id)) {
        e.isValid = false;
        e.errorText = "Solo se permiten números.";
        return;
    }

    // Verifica la longitud exacta de 13 dígitos
    if (id.length !== 13) {
        e.isValid = false;
        e.errorText = "El Número de Identidad debe tener 13 dígitos.";
        return;
    }

    // Si pasa todas las validaciones
    e.isValid = true;
}
function isValidID(id) {
    if (!id) return false;
    if (hasSpecialCharacters(id)) return false;

    // Limita la longitud máxima a 13 (DNI)
    if (id.length > 13) {
        return false;
    }

    // DNI: 13 dígitos numéricos
    if (id.length === 13 && isNumeric(id)) {
        return true;
    }

    // Pasaporte: entre 6 y 9 caracteres alfanuméricos
    if (id.length >= 6 && id.length <= 9 && isAlphanumeric(id)) {
        return true;
    }

    // Carnet de residencia: 8 caracteres alfanuméricos
    if (id.length === 8 && isAlphanumeric(id)) {
        return true;
    }

    return false;
}


// Verifica si es numérico
function isNumeric(str) {
    return /^\d+$/.test(str);
}

// Verifica si es alfanumérico
function isAlphanumeric(str) {
    return /^[A-Za-z0-9]+$/.test(str);
}

// Verifica si contiene caracteres no permitidos
function hasSpecialCharacters(str) {
    return /[^a-zA-Z0-9]/.test(str);
}

function solonumeros(e) {
    var key;

    if (window.event) { // IE
        key = e.keyCode;
    } else if (e.which) { // Netscape/Firefox/Opera
        key = e.which;
    }

    if (key < 48 || key > 57) {
        return false;
    }

    return true;
}

function SoloLetras(s, e) {
    var key = e.htmlEvent.keyCode || e.htmlEvent.which;
    var char = String.fromCharCode(key);

    // Solo letras, espacios y apóstrofe
    var regex = /^[A-Za-zÁÉÍÓÚáéíóúÑñ\s']$/;

    if (!regex.test(char)) {
        e.htmlEvent.preventDefault();
    }
}


function CustomValidateTelefono(s, e) {
    var value = e.value;

    if (!value) {
        e.isValid = false;
        e.errorText = "El número es requerido";
        return;
    }

    // Solo números
    var regex = /^\d+$/;
    if (!regex.test(value)) {
        e.isValid = false;
        e.errorText = "Solo se permiten números.";
        return;
    }

    // Longitud mínima/máxima
    if (value.length < 8 || value.length > 8) {
        e.isValid = false;
        e.errorText = "Debe tener exactamente 8 dígitos.";
    }
}


//function CustomValidateNombreApellido(s, e) {
//    var value = e.value;

//    // Solo letras + acentos + espacios
//    var regex = /^[A-Za-zÁÉÍÓÚáéíóúÑñ\s']+$/;

//    if (!value || !regex.test(value)) {
//        e.isValid = false;
//        return;
//    }

//    e.isValid = true;
//}



function CustomValidateNombreEmpresa(s, e) {
    var value = e.value;


    var regex = /^[A-ZÁÉÍÓÚÑÜ\s().]+$/;

    if (!value || !regex.test(value)) {
        e.isValid = false;
        e.errorText = "No se permiten números, solo letras, espacios, paréntesis y puntos.";
        return;
    }

    // No permitir más de un punto seguido o espacios dobles
    if (value.includes("..") || value.includes("  ")) {
        e.isValid = false;
        e.errorText = "Formato inválido: verifique espacios y puntos.";
        return;
    }

    e.isValid = true;
}
function OnKeyUpUpper(s, e) {
    s.SetText(s.GetText().toUpperCase());
    s.Validate(); // fuerza la validación después de cambiar el texto
}




function CustomValidateNombreApellido(s, e) {
    var value = e.value;

    // Acepta solo letras (incluye acentos y ñ), espacios y apóstrofes
    var regex = /^[A-Za-zÁÉÍÓÚáéíóúÑñ\s']+$/;

    if (!value || !regex.test(value)) {
        e.isValid = false;
        e.errorText = "Solo se permiten letras y espacios.";
    }
}



function validarFormatoCorreo(correo) {
    var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    return regex.test(correo);
}

function validarFormatoIDN(valor) {
    // DNI: 13 dígitos numéricos
    if (/^\d{13}$/.test(valor)) {
        return true;
    }
    // Pasaporte o carnet de residencia: 9 caracteres alfanuméricos
    if (/^[a-zA-Z0-9]{6,9}$/.test(valor)) {
        return true;
    }
    return false;
}

function isValidString(value) {
    // Expresión regular que permite solo letras, "-" y "/"
    const regex = /^[a-zA-Z\-/]+$/;
    return regex.test(value);
}


function Guardar_Datos_Complete(s, e) {
    var respuestaJSON = e.result;
    var respuesta = JSON.parse(respuestaJSON);
    var Retorno = respuesta.codeResult;
    var Mens = respuesta.message;



    if (Retorno == 0) {
        /* Enviar.Hide();*/
        popupResumen.Hide();
        Relacionado.Show();

        btnEnviarCodigo.SetVisible(true); // Hacer visible el botón después de mostrar el comprobante


    }
}

function SetCampos() {
    // Limpiar campos comunes
    tbCorreo.SetText('');
    tbConfirmCorreo.SetText('');
    tbTelefono.SetText('');

    // Limpiar campos de persona natural
    tbIdentidad.SetText('');
    tbNombre.SetText('');
    tbApellido.SetText('');

    // Limpiar campos de persona jurídica
    tbRTN.SetText('');
    tbInstitucion.SetText('');

    // Limpiar uploads
    fileUpload.value = '';
    fileUpload1.value = '';
    fileUpload2.value = '';

    // Limpiar mensajes
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

    // Mostrar/ocultar campos según tipo de solicitante
    var tipoSolicitante = rbTipoSolicitante.GetValue();

    if (tipoSolicitante === "Natural") {
        // Mostrar campos de persona natural
        formDenuncia.GetItemByName("itemNombre").SetVisible(true);
        formDenuncia.GetItemByName("itemApellido").SetVisible(true);
        formDenuncia.GetItemByName("itemIdentidad").SetVisible(true);

        // Ocultar campos de persona jurídica
        formDenuncia.GetItemByName("itemRTN").SetVisible(false);
        formDenuncia.GetItemByName("itemInstitucion").SetVisible(false);
    } else if (tipoSolicitante === "Juridica") {
        // Ocultar campos de persona natural
        formDenuncia.GetItemByName("itemNombre").SetVisible(false);
        formDenuncia.GetItemByName("itemApellido").SetVisible(false);
        formDenuncia.GetItemByName("itemIdentidad").SetVisible(false);

        // Mostrar campos de persona jurídica
        formDenuncia.GetItemByName("itemRTN").SetVisible(true);
        formDenuncia.GetItemByName("itemInstitucion").SetVisible(true);
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


function ClosePopupRelacionado(s, e) {
    var ckPolitica = ASPxClientControl.GetControlCollection().GetByName("ckPolitica");
    var btnEnviarCodigo = ASPxClientControl.GetControlCollection().GetByName("btnEnviarCodigo");
    Relacionado.Hide();

}

/**SOLICITUD RECIBIDA*/

function showConfirmationMessage1() {
    var email = tbCorreo.GetText();
    var tipoSolicitante = rbTipoSolicitante.GetValue(); // "Natural" o "Juridica"

    // URL del logo que quieres usar en el PDF (no en el popup)
    var logoUrl = "https://dcioxh.stripocdn.email/content/guids/CABINET_b93ec2a38389d475174431c45f61c597b12b8d21784bda70816c8c0373b4ae6a/images/logo_tsc_2024_n0C.png?v=" + new Date().getTime();

    $.ajax({
        type: "POST",
        url: "Solicitud.aspx/GetSessionValues",
        data: JSON.stringify({
            email: email,
            constanciaId: globalIdConstancia,
            randPassword: globalIdClave
        }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var values = response.d.split("|");
            var qrCodeImageUrl = values[0]; // no usado aquí

            var tipoTexto = (tipoSolicitante === "Natural")
                ? "<p><strong>Tipo de solicitante:</strong> Persona Natural</p>"
                : "<p><strong>Tipo de solicitante:</strong> Persona Jurídica</p>";


            var tableHtml = `
<div style="background:#fff; padding:24px; max-width:600px; margin:0 auto; font-family:'Segoe UI', sans-serif; color:#333;">
   <h4 style="text-align:center; color:#313131; margin-bottom:16px;">✅  ¡Confirmación de Solicitud!</h4>
   <p>Una copia de su solicitud ha sido enviada a su correo electrónico.</p>
    ${tipoTexto}
    <hr style="margin:16px 0;">
    <p><strong>¿Desea dar seguimiento a su solicitud?</strong> Siga los siguientes pasos:</p>
   <ol style="padding-left:20px;">
       <li>Guarde el número de su solicitud y la clave de seguimiento.</li>
       <li>Ingrese a <a href="https://www.tsc.gob.hn/" target="_blank">www.tsc.gob.hn</a>.</li>
       <li>Vaya a la sección <em>Seguimiento de solicitud de constancias</em>.</li>
       <li>Seleccione <strong>Seguimiento de solicitud de constancias</strong> e ingrese los datos solicitados.</li>
   </ol>
   <div style="background:#f9f9f9; border-left:4px solid #1F497D; padding:16px; margin:20px 0; border-radius:8px;">
        <p><strong>Número de solicitud:</strong> <span style="color:#000000; font-weight:bold;">📄 ${globalIdConstancia}</span></p>
       <p><strong>Clave de seguimiento:</strong> <span style="color:#000000; font-weight:bold;">🔑 ${globalIdClave}</span></p>
    </div>
      <div style="text-align:center; margin-top:10px;">
    <!-- Botón de descarga -->
    <button id="btnDescargarPdf" style="background:#1F497D; color:#fff; padding:10px 18px; border-radius:6px; border:none; cursor:pointer; margin-right:16px;">
        Descargar 
    </button>
    <!-- Un solo enlace "Volver al Inicio" con el mismo estilo que el botón -->
    <a id="btnVolverInicio" href="Solicitud.aspx" style="background:#1F497D; color:#fff; padding:10px 18px; border-radius:6px; text-decoration:none; display:inline-block; margin-left:16px;">
        Volver al Inicio
    </a>
</div>
   
</div>
            `;

            document.getElementById("popupContent").innerHTML = tableHtml;

            // Datos planos para el PDF/TXT
            var tipoTextoPlano = (tipoSolicitante === "Natural") ? "Persona Natural" : "Persona Jurídica";
            var fechaGeneracion = new Date().toLocaleString();
            function generarTxtFallback() {
                var contenido = `Tipo de solicitante: ${tipoTextoPlano}\nNúmero de solicitud: ${globalIdConstancia}\nClave de seguimiento: ${globalIdClave}\nFecha: ${fechaGeneracion}\n`;
                var blob = new Blob([contenido], { type: "text/plain;charset=utf-8" });
                var url = URL.createObjectURL(blob);
                var link = document.createElement("a");
                link.href = url;
                link.download = `Seguimiento_${globalIdConstancia}.txt`;
                link.style.display = "none";
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                setTimeout(function () { URL.revokeObjectURL(url); }, 1000);
            }


            function loadImageAsDataUrl(url) {
                return new Promise(function (resolve, reject) {
                    try {
                        var img = new Image();
                        img.crossOrigin = "Anonymous";
                        img.onload = function () {
                            try {
                                var canvas = document.createElement("canvas");
                                canvas.width = img.width;
                                canvas.height = img.height;
                                var ctx = canvas.getContext("2d");
                                ctx.drawImage(img, 0, 0);
                                var dataURL = canvas.toDataURL("image/png");
                                resolve({ dataUrl: dataURL, width: img.width, height: img.height });
                            } catch (err) {

                                reject(err);
                            }
                        };
                        img.onerror = function (e) {
                            reject(e);
                        };
                        img.src = url;
                        if (img.complete && img.naturalWidth !== 0) {
                            img.onload();
                        }
                    } catch (ex) {
                        reject(ex);
                    }
                });
            }

            function generarPdfConJsPDFConLogo(logoData) {
                try {
                    var doc = new jsPDF({ unit: "pt", format: "a4" });
                    var left = 40;
                    var y = 60;
                    var pageWidth = doc.internal.pageSize.getWidth();

                    if (logoData && logoData.dataUrl) {
                        var targetWidthPt = 50;
                        var imgW = logoData.width;
                        var imgH = logoData.height;
                        var scale = targetWidthPt / imgW;
                        var wPt = imgW * scale;
                        var hPt = imgH * scale;
                        var x = pageWidth - left - wPt;
                        var yImg = 30;
                        try {
                            doc.addImage(logoData.dataUrl, "PNG", x, yImg, wPt, hPt);
                        } catch (e) {
                            console.warn("No se pudo agregar la imagen al PDF:", e);
                        }
                    }

                    doc.setFontSize(18);
                    doc.text("Confirmación de solicitud de constancia", pageWidth / 2, y, { align: "center" });

                    y += 20;
                    doc.text("de no tener cuentas pendientes con el Estado ", pageWidth / 2, y, { align: "center" });

                    y += 28;
                    doc.setFontSize(11);
                    doc.text(`Fecha y hora de solicitud: ${fechaGeneracion}`, left, y);
                    y += 20;

                    doc.setFontSize(12);
                    doc.text("Tipo de solicitante:", left, y);
                    doc.setFont("helvetica", "bold");
                    doc.text(tipoTextoPlano, left + 140, y);
                    doc.setFont("helvetica", "normal");
                    y += 20;

                    doc.text("Número de solicitud:", left, y);
                    doc.setFont("helvetica", "bold");
                    doc.text(String(globalIdConstancia), left + 140, y);
                    doc.setFont("helvetica", "normal");
                    y += 20;

                    doc.text("Clave de seguimiento:", left, y);
                    doc.setFont("helvetica", "bold");
                    doc.text(String(globalIdClave), left + 140, y);
                    doc.setFont("helvetica", "normal");
                    y += 30;

                    doc.setDrawColor(200);
                    doc.setLineWidth(0.5);
                    doc.line(left, y, pageWidth - left, y);

                    var filename = `Seguimiento_${globalIdConstancia}.pdf`;


                    try {

                        var blob = doc.output && typeof doc.output === "function" ? doc.output("blob") : null;
                        if (!blob) {

                            doc.save(filename);
                            return;
                        }
                        var url = URL.createObjectURL(blob);
                        var a = document.createElement("a");
                        a.href = url;
                        a.download = filename;
                        // asegurar que esté fuera de pantalla (no visible)
                        a.style.display = "none";
                        document.body.appendChild(a);
                        a.click();
                        document.body.removeChild(a);
                        // liberar memoria
                        setTimeout(function () { URL.revokeObjectURL(url); }, 1000);
                    } catch (ex) {
                        // Si algo falla, usar el comportamiento clásico
                        console.warn("Error al forzar descarga como blob, usando doc.save():", ex);
                        try { doc.save(filename); } catch (e) { generarTxtFallback(); }
                    }
                } catch (e) {
                    console.error("Error generando PDF con jsPDF:", e);
                    generarTxtFallback();
                }
            }

            function generarPdfConJsPDF() {

                loadImageAsDataUrl(logoUrl).then(function (logoData) {
                    generarPdfConJsPDFConLogo(logoData);
                }).catch(function (err) {

                    console.warn("No se pudo cargar el logo para el PDF (se generará sin él):", err);
                    generarPdfConJsPDFConLogo(null);
                });
            }

            function cargarJsPdfYGenerar() {

                if (typeof jsPDF !== "undefined") {
                    generarPdfConJsPDF();
                    return;
                }


                var scriptId = "jsPDF-cdn-script";
                if (document.getElementById(scriptId)) {
                    setTimeout(function () {
                        if (typeof jsPDF !== "undefined") generarPdfConJsPDF(); else generarTxtFallback();
                    }, 800);
                    return;
                }

                var script = document.createElement("script");
                script.id = scriptId;
                script.src = "https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js";
                script.onload = function () {
                    if (window.jspdf && window.jspdf.jsPDF) {
                        window.jsPDF = window.jspdf.jsPDF;
                    }
                    if (typeof jsPDF !== "undefined") {
                        generarPdfConJsPDF();
                    } else {
                        try {
                            if (window.jspdf && window.jspdf.default && window.jspdf.default.jsPDF) {
                                window.jsPDF = window.jspdf.default.jsPDF;
                                generarPdfConJsPDF();
                                return;
                            }
                        } catch (e) { }
                        generarTxtFallback();
                    }
                };
                script.onerror = function () {
                    console.warn("No se pudo cargar jsPDF desde CDN. Usando fallback TXT.");
                    generarTxtFallback();
                };
                document.head.appendChild(script);
            }

            // Asociar evento al botón de descarga
            var btn = document.getElementById("btnDescargarPdf");
            if (btn) {
                btn.addEventListener("click", function (e) {
                    e.preventDefault();
                    var originalText = btn.innerText;
                    btn.innerText = "Generando...";
                    btn.disabled = true;
                    setTimeout(function () {
                        cargarJsPdfYGenerar();
                        setTimeout(function () {
                            btn.innerText = originalText;
                            btn.disabled = false;
                        }, 1000);
                    }, 100);
                });
            }
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


/**ENVIAR EL TOKEN DE VERIFICACIÓN AL CORREO */
function btnEnviarCodigo_Click(s, e) {
    var tipoSolicitante = rbTipoSolicitante.GetValue(); // "Natural" o "Juridica"

    // Campos comunes
    var correo = tbCorreo.GetText();
    var confirmarCorreo = tbConfirmCorreo.GetText();
    var telefono = tbTelefono.GetText();

    var campos = [];

    if (tipoSolicitante === "Natural") {
        var nombre = tbNombre.GetText();
        var apellido = tbApellido.GetText();
        var identidad = tbIdentidad.GetText();

        campos = [nombre, apellido, identidad, correo, confirmarCorreo, telefono];

    } else if (tipoSolicitante === "Juridica") {
        var rtn = tbRTN.GetText();
        var institucion = tbInstitucion.GetText();

        campos = [rtn, institucion, correo, confirmarCorreo, telefono];
    }

    // Verificar si hay algún campo vacío
    var camposVacios = campos.some(function (valor) {
        return valor === '' || valor === null;
    });

    if (camposVacios) {
        Swal.fire({
            title: "¡Alerta!",
            text: "Debe llenar los datos requeridos del formulario para hacer el envío del código de verificación.",
            icon: "warning",
            confirmButtonColor: "#1F497D",
        });
        return;
    }

    // Validar formato de correo
    if (!validarFormatoCorreo(correo)) {
        Swal.fire({
            title: "¡Alerta!",
            text: "Por favor ingrese un correo electrónico válido.",
            icon: "warning",
            confirmButtonColor: "#1F497D",
        });
        return;
    }

    // Enviar el token al correo
    ASPxCallback_EnviarToken.PerformCallback(correo);
    popupToken.Show();
}


/**SOLICITUD COMPLETA */
function TokenVerificationComplete(result) {
    if (result === "success") {
        popupToken.Hide();
        btnEnviarCodigo.SetVisible(false);
        tbToken.SetText('');

        var tipoSolicitante = rbTipoSolicitante.GetValue();

        // Campos comunes
        var tbCorreo = ASPxClientControl.GetControlCollection().GetByName("tbCorreo");
        var tbConfirmCorreo = ASPxClientControl.GetControlCollection().GetByName("tbConfirmCorreo");
        var tbTelefono = ASPxClientControl.GetControlCollection().GetByName("tbTelefono");

        var formDataSolicitud = {};

        if (tipoSolicitante === "Natural") {
            var tbIdentidad = ASPxClientControl.GetControlCollection().GetByName("tbIdentidad");
            var tbNombre = ASPxClientControl.GetControlCollection().GetByName("tbNombre");
            var tbApellido = ASPxClientControl.GetControlCollection().GetByName("tbApellido");

            if (tbIdentidad && tbNombre && tbApellido && tbCorreo && tbConfirmCorreo && tbTelefono) {
                formDataSolicitud = {
                    tipoSolicitante: "Natural",
                    tbIdentidad: tbIdentidad.GetValue(),
                    tbNombre: tbNombre.GetValue(),
                    tbApellido: tbApellido.GetValue(),
                    tbCorreo: tbCorreo.GetValue(),
                    tbConfirmCorreo: tbConfirmCorreo.GetValue(),
                    tbTelefono: tbTelefono.GetValue()
                };
            } else {
                alert("Faltan campos para persona natural.");
                return;
            }
        } else if (tipoSolicitante === "Juridica") {
            var tbRTN = ASPxClientControl.GetControlCollection().GetByName("tbRTN");
            var tbInstitucion = ASPxClientControl.GetControlCollection().GetByName("tbInstitucion");

            if (tbRTN && tbInstitucion && tbCorreo && tbConfirmCorreo && tbTelefono) {
                formDataSolicitud = {
                    tipoSolicitante: "Juridica",
                    tbRTN: tbRTN.GetValue(),
                    tbInstitucion: tbInstitucion.GetValue(),
                    tbCorreo: tbCorreo.GetValue(),
                    tbConfirmCorreo: tbConfirmCorreo.GetValue(),
                    tbTelefono: tbTelefono.GetValue()
                };
            } else {
                alert("Faltan campos para persona jurídica.");
                return;
            }
        }

        // Enviar datos al backend
        $.ajax({
            type: "POST",
            url: "CreateSolicitudHandler.ashx",
            data: formDataSolicitud,
            dataType: "json",
            success: function (response) {
                try {
                    var idConstancia = response.Idconstancia;
                    if (typeof idConstancia === "undefined") {
                        // No continuar si es undefined
                        return;
                    }
                    globalIdConstancia = idConstancia;
                    var clave = response.Clave;
                    globalIdClave = clave;

                    // Subida de archivos
                    var formData = new FormData();
                    var fileUpload = document.getElementById('fileUpload');
                    var fileUpload1 = document.getElementById('fileUpload1');
                    var fileUpload2 = document.getElementById('fileUpload2');
                    var maxSize = 5 * 1024 * 1024;

                    if (fileUpload1.files.length > 0 && fileUpload1.files[0].size <= maxSize) {
                        formData.append("fileIdentidad", fileUpload1.files[0]);
                        formData.append("flexFieldKey", "CODIGO_IDENTIDAD");
                        formData.append("fileIdKey", fileIdIdent);
                    } else if (fileUpload1.files.length > 0) {
                        alert("El archivo de identidad supera el tamaño máximo permitido de 5 MB.");
                        return;
                    }

                    if (fileUpload.files.length > 0 && fileUpload.files[0].size <= maxSize) {
                        formData.append("fileSolicitud", fileUpload.files[0]);
                        formData.append("flexFieldKey", "CODIGO_SOLICITUD");
                        formData.append("fileIdKey", fileIdsolicitud);
                    } else if (fileUpload.files.length > 0) {
                        alert("El archivo de solicitud supera el tamaño máximo permitido de 5 MB.");
                        return;
                    }

                    if (fileUpload2.files.length > 0 && fileUpload2.files[0].size <= maxSize) {
                        formData.append("fileRecibo", fileUpload2.files[0]);
                        formData.append("flexFieldKey", "CODIGO_RECIBO");
                        formData.append("fileIdKey", fileIdrecib);
                    } else if (fileUpload2.files.length > 0) {
                        alert("El archivo de recibo supera el tamaño máximo permitido de 5 MB.");
                        return;
                    }

                    formData.append("flexFieldValue", idConstancia);

                    $.ajax({
                        url: "UploadFilesHandler.ashx",
                        type: "POST",
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (uploadResponse) {
                            ASPxCallback_Guardar_Datos.PerformCallback(JSON.stringify(uploadResponse));

                            // 2) Limpias los file‑inputs
                            ['fileUpload', 'fileUpload1', 'fileUpload2'].forEach(function (id) {
                                var f = document.getElementById(id);
                                if (f) f.value = '';
                            });

                            // 3) Limpias los labels de “Seleccionado” y “Tamaño”
                            ['lblSelected', 'lblSize',
                                'lblSelected1', 'lblSize1',
                                'lblSelected2', 'lblSize2'
                            ].forEach(function (id) {
                                var lbl = document.getElementById(id);
                                if (lbl) lbl.innerText = '';
                            });

                            // 4) Limpiar mensajes de error si los usas
                            ['lblError', 'lblError1', 'lblError2'].forEach(function (id) {
                                var err = document.getElementById(id);
                                if (err) err.innerText = '';
                            })


                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.error("Error al subir los archivos: ", errorThrown);
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

    } else if (result === "incorrect") {
        Swal.fire({
            title: "¡Alerta!",
            text: "Código de verificación incorrecto. Por favor, inténtelo de nuevo.",
            icon: "warning",
            confirmButtonColor: "#1F497D",
        });
        tbToken.SetText('');
    } else if (result === "expired") {
        Swal.fire({
            title: "¡Alerta!",
            text: "El código de verificación ha expirado. Por favor, solicite un nuevo código.",
            icon: "warning",
            confirmButtonColor: "#1F497D",
        });
        popupToken.Hide();
        tbToken.SetText('');
    } else {
        Swal.fire({
            title: "¡Alerta!",
            text: "Error en la verificación del código. Por favor, inténtelo de nuevo.",
            icon: "warning",
            confirmButtonColor: "#1F497D",
        });
        popupToken.Hide();
        tbToken.SetText('');
    }
}

/**RESUMEN DE LA SILICITUD*/
function mostrarResumen() {
    var resumenContent = document.getElementById("resumenContent");


    var tipoSolicitante = rbTipoSolicitante.GetValue(); // "Natural" o "Juridica"


    var tbCorreo = ASPxClientControl.GetControlCollection().GetByName("tbCorreo");
    var tbTelefono = ASPxClientControl.GetControlCollection().GetByName("tbTelefono");


    var fileUpload = document.getElementById('fileUpload');
    var fileUpload1 = document.getElementById('fileUpload1');
    var fileUpload2 = document.getElementById('fileUpload2');

    var strfileNames = fileUpload ? Array.from(fileUpload.files).map(file => file.name).join(', ') : '';
    var strfileNames1 = fileUpload1 ? Array.from(fileUpload1.files).map(file => file.name).join(', ') : '';
    var strfileNames2 = fileUpload2 ? Array.from(fileUpload2.files).map(file => file.name).join(', ') : '';

    var datosSolicitanteHTML = "";

    if (tipoSolicitante === "Natural") {
        var tbIdentidad = ASPxClientControl.GetControlCollection().GetByName("tbIdentidad");
        var tbNombre = ASPxClientControl.GetControlCollection().GetByName("tbNombre");
        var tbApellido = ASPxClientControl.GetControlCollection().GetByName("tbApellido");

        datosSolicitanteHTML = `
      <div><span>Número de Identidad:</span> ${tbIdentidad.GetText() || ""}</div>
      <div><span>Nombres del Solicitante:</span>     ${tbNombre.GetText() || ""}</div>
      <div><span>Apellidos del Solicitante:</span>   ${tbApellido.GetText() || ""}</div>
    `;

    } else if (tipoSolicitante === "Juridica") {
        var tbRTN = ASPxClientControl.GetControlCollection().GetByName("tbRTN");
        var tbInstitucion = ASPxClientControl.GetControlCollection().GetByName("tbInstitucion");

        datosSolicitanteHTML = `
            <div><span>Número de RTN:</span> ${tbRTN.GetValue() || ""}</div>
            <div><span>Nombre de la Empresa/Institución:</span> ${tbInstitucion.GetValue() || ""}</div>
        `;
    }

    resumenContent.innerHTML = `
    <div class="resumen-popup">
        <div class="resumen-info">
            ${datosSolicitanteHTML}
            <div><span>Correo Electrónico:</span> ${tbCorreo.GetValue() || ""}</div>
            <div><span>Celular/Teléfono:</span> ${tbTelefono.GetValue() || ""}</div>
        </div>

        <div class="resumen-archivos">
            <h5><i class="fas fa-paperclip"></i> Archivos Adjuntos</h5>
            <div class="archivo-item">
                <span class="archivo-label">Archivo de su Solicitud:</span>
                <span class="archivo-nombre">${strfileNames}</span>
            </div>
            <div class="archivo-item">
                <span class="archivo-label">Archivo de Identidad/RTN:</span>
                <span class="archivo-nombre">${strfileNames1}</span>
            </div>
            <div class="archivo-item">
                <span class="archivo-label">Archivo de su Recibo TGR-1 y comprobante de pago:</span>
                <span class="archivo-nombre">${strfileNames2}</span>
            </div>
        </div>
    </div>
    `;

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

