<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerificarConstancia.aspx.cs" Inherits="SG_Constancia_TSC.VerificarConstancia" %>

<!DOCTYPE html>
<html lang="es">
<head>

    <!-- basic -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0" />
    <!-- mobile metas -->
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />

    <title>Verificación de Constancias TSC</title>
    <meta name="keywords" content="" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <!-- bootstrap css -->
    <link rel="icon" href="favicon.ico" type="image/x-icon" />
    <link href="Content/Form.css" rel="stylesheet" />
    <link href="Content/Denuncia.css" rel="stylesheet" />
    <link href="Content/css/responsive.css" rel="stylesheet" />
    <link href="Content/css/owl.carousel.min.css" rel="stylesheet" />
    <link href="Content/Fontawesome/css/all.css" rel="stylesheet" />
    <link href="Content/Fontawesome/css/fontawesome.css" rel="stylesheet" />

    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/CustomStyles.css" rel="stylesheet" />
    <link href="Content/Form1.css" rel="stylesheet" />
    <link href="Content/custom.css" rel="stylesheet" />
    <link rel="stylesheet" href="Content/bootstrap.css" />
    <link href="Content/css/responsive.css" rel="stylesheet" />
    <link rel="stylesheet" href="Content/Fontawesome/css/all.min.css" />
    <link rel="stylesheet" href="Content/css/bootstrap.min.css">
    <script src="Content/Bootstrap/js/jquery-3.6.0.min.js"></script>
    <script src="Content/bootstrap-5.3.7-dist/bootstrap-5.3.7-dist/js/bootstrap.bundle.min.js"></script>
    <script src="/Content/Sweetalert/js/sweetalert2@11.js"></script>
    <script src="https://www.google.com/recaptcha/api.js" async defer></script>

</head>
<body class="main-layout">
    <header class="header-area">
        <div class="container">
            <div class="row d_flex">
                <div class=" col-md-3">
                    <div class="logo">
                        <a href="https://www.tsc.gob.hn/">
                            <img src="Content/Images/LOGO_TSC_2024_Logo.png" alt="#" />T<span>SC</span></a>
                    </div>
                </div>
                <div class="col-md-9 col-sm-12">
                    <div class="navbar-area">
                        <nav class="site-navbar">
                            <ul>
                                <li><a class="active" href="https://www.tsc.gob.hn/" target="_blank"><i class="fa fa-home"></i>Inicio</a></li>
                                <li><a href="Content/Manual/Manual_de_Usuario_Modulo_Solicitante.pdf" target="_blank"><i class="fa fa-book"></i>Manual de Usuario</a></li>
                            </ul>
                            <button class="nav-toggler">
                                <span></span>
                            </button>
                        </nav>
                    </div>
                </div>
            </div>
        </div>
    </header>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-12">
                    <div class="titlepage text_align_left">
                        <h3 class="text-center titulo-formu">VERIFICACIÓN DE CONSTANCIA </h3>
                        <h3 class="text-center titulo-formu">DE NO TENER CUENTAS PENDIENTES CON EL ESTADO DE HONDURAS</h3>
                        <h3 class="text-center titulo-formulario">SISTEMA DE SOLICITUD DE CONSTANCIAS EN LÍNEA </h3>
                    </div>
                </div>
            </div>
            <br />

            <dx:ASPxFormLayout runat="server" ID="formSeguimientoConstancia" CssClass="formLayout">
                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
                <Items>
                    <dx:LayoutItem ShowCaption="False" HorizontalAlign="Left">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer>
                                <table>
                                    <tr>
                                        <td>
                                            <strong>Este formulario le permite verificar la autenticidad de la constancia emitida por el Tribunal Superior de Cuentas.
                                                 Antes de ingresar los datos solicitados, por favor revise las siguientes instrucciones.
                                            </strong>
                                            <br />
                                            <a href="javascript:void(0)" onclick="abrirInstruccionesVerificacion()" style="color: #1F497D; font-weight: bold; text-decoration: underline;">Ver instrucciones
                                            </a>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>


                    <dx:LayoutGroup Caption="Paso 1: Verificación por Correo Electrónico" ColCount="2">
                        <Items>
                            <dx:LayoutItem Caption="Correo Electrónico:">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtCorreo" runat="server" ClientInstanceName="txtCorreo" AutoPostBack="false" NullText="Correo Electrónico" ToolTip="Correo para enviar código">
                                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom">

                                                <RegularExpression ErrorText="Formato de correo no válido."
                                                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Código de Verificación:">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtVerficacion" runat="server" ClientInstanceName="txtVerficacion" NullText="Código de Verificación"
                                            AutoPostBack="false" ToolTip="Código recibido en su correo">
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right" ColSpan="2">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButton ID="btnEnviarCodigo" CssClass="btn-moderno" runat="server" Text="Enviar Código" OnClick="btnEnviarCodigo_Click" />
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>

                    <dx:LayoutGroup Caption="Paso 2: Verificación de Constancia" ColCount="2">
                        <Items>
                            <dx:LayoutItem Caption="Código Seguro de Verificación:">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="txtCodigo" runat="server" ClientInstanceName="txtCodigo" AutoPostBack="false" NullText="Código Seguro de Verificación" ToolTip="Ingrese el Código Seguro de Verificación de su Constancia.">
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right" ColSpan="2">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButton ID="btnVerificar" CssClass="btn-moderno" runat="server" Text="Verificar" OnClick="btnVerificar_Click" />
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem ShowCaption="False" ColSpan="2">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxLabel ID="lblResultado" runat="server" CssClass="text-success font-weight-bold" />
                                        <dx:ASPxLabel ID="lblMensaje" runat="server" CssClass="text-danger" />
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>

                </Items>
            </dx:ASPxFormLayout>
            <!-- Modal de resultado -->
            <div id="popupResultado" class="modal fade" tabindex="-1" aria-labelledby="popupResultadoLabel" aria-hidden="true"
                popuphorizontalalign="WindowCenter" popupverticalalign="WindowCenter" closeaction="CloseButton" allowdragging="true"
                closeanimationtype="None" allowresize="False" settingsadaptivity-mode="OnWindowInnerWidth" closeonescape="true"
                enableviewstate="False" autoupdateposition="true"
                settingsadaptivity-verticalalign="WindowCenter" popupanimationtype="Fade">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content p-4" style="border-radius: 12px;">
                        <div class="modal-header custom-modal-header">
                            <h5 class="modal-title" id="popupResultadoLabel">Resultado de Verificación de Constancia</h5>
                            <button type="button" class="custom-close-btn" data-bs-dismiss="modal" aria-label="Cerrar">
                                <i class="fas fa-times"></i>
                            </button>
                        </div>
                        <div class="modal-body" id="popupBodyContent">
                            <!-- Aquí se muestra el contenido desde el servidor -->
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
    <br />
    <footer>
        <div class="footer">
            <div class="container">
                <div class="row">
                    <div class="col-12 col-md-12">
                        <ul class="conta">
                            <li><span>Dirección</span> Tegucigalpa, M.D.C. Centro Cívico Gubernamental, Bulevar Fuerzas Armadas
                              <br />
                                Honduras, C.A </li>
                            <li><span>Correo Eléctronico</span> <a href="mailto:tsc@tsc.gob.hn">tsc@tsc.gob.hn</a> </li>
                            <li><span>Contacto</span> <a href="Javascript:void(0)">Tel(+504) 2230-3646 / 2228-3512 / 2228-7913
                              <br />
                                2230-4152 / 2230-8242 / 2230-3732 </a></li>
                        </ul>
                    </div>
                    <div class="col-12 col-md-12">
                        <div class="Informa">
                            <ul class="social_icon text_align_center">
                                <li><a href="https://www.tsc.gob.hn/"><i class="fa fa-solid fa-globe"></i></a></li>
                                <li><a href="http://www.facebook.com/tschonduras"><i class="fa-brands fa-facebook"></i></a></li>
                                <li><a href="http://www.twitter.com/tschonduras"><i class="fa-brands fa-x-twitter"></i></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="copyright text_align_center">
                <div class="container">
                    <div class="row">
                        <div class="col-md-12">
                            <p>&copy; Gerencia de Informática 2025 | Tribunal Superior de Cuentas.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </footer>
</body>
</html>
<script type="text/javascript">

    function abrirInstruccionesVerificacion() {
        const contenido = `
      <div style="text-align:left; font-size:14px; line-height:1.55">
        <h4 style="margin:0 0 8px 0;">Paso 1: Verificación por Correo Electrónico <span style="color:#1F497D;"></span></h4>
        <ul style="margin-top:6px; padding-left:18px;">
          <li>✅ Ingrese su <b>correo electrónico</b> en el campo indicado.</li>
          <li>✅  Haga clic en <b>“Enviar Código”</b>.</li>
          <li>📩 Revise su bandeja de entrada y copie el <b>código de verificación</b> recibido a su correo electrónico.</li>
         
        </ul>
        <p style="margin:10px 0 16px 0;color:#1C4271;font-weight:bold;">
          Nota: Este código se envía cada vez que desee verificar la autenticidad de la constancia; deberá repetir este paso en cada consulta.
        </p>

        <h4 style="margin:0 0 8px 0;">Paso 2: Verificación de Constancia</h4>
        <ul style="margin-top:6px; padding-left:18px;">
          <li>📄  Ingrese el <b>Código Seguro de Verificación generado en su constancia</b>.</li>
          <li>✅ Haga clic en <b>“Verificar”</b> para visualizar la autenticidad de la constancia.</li>
        </ul>
        
      </div>
    `;

        Swal.fire({
            title: 'Instrucciones de Verificación de Constancia',
            html: contenido,
            width: 720,
            confirmButtonText: 'Cerrar',
            confirmButtonColor: '#1F497D',
            showCloseButton: true,
            focusConfirm: false,
            scrollbarPadding: false,
            didOpen: (popup) => {
                const htmlContainer = popup.querySelector('.swal2-html-container');
                if (htmlContainer) {
                    htmlContainer.style.maxHeight = '60vh';
                    htmlContainer.style.overflowY = 'auto';
                    htmlContainer.style.textAlign = 'left';
                }
            }
        });
    }

</script>