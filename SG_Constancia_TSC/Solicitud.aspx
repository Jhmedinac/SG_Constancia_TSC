<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Solicitud.aspx.cs" Inherits="SG_Constancia_TSC.Solicitud" Async="true" %>

<%@ Import Namespace="SG_Constancia_TSC.UtilClass" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <!-- basic -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0" />
    <!-- mobile metas -->

    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="viewport" content="initial-scale=1, maximum-scale=1" />
    <!-- site metas -->
    <title>Solicitud de Constancias TSC</title>
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
    <link href="Content/Stepper.css" rel="stylesheet" />
    <link rel="stylesheet" href="Content/bootstrap.css" />
    <script src="Content/Denuncia.js"></script>
    <script src="Content/Consulta.js"></script>
    <link href="~/Content/Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Fontawesome/css/all.min.css" rel="stylesheet" />
    <script src="Content/Bootstrap/js/jquery-3.6.0.min.js"></script>
    <link href="~/Content/Bootstrap/css/bootstrap-icons.min.css" rel="stylesheet" />
    <script src="/Content/Sweetalert/js/sweetalert2@11.js"></script>
    <script src="/Content/Bootstrap/js/popper.min.js"></script>
    <script src="/Content/Bootstrap/js/bootstrap.min.js"></script>
    <script src="/Content/Bootstrap/js/bootstrap.bundle.min.js"></script>

</head>
<script type="text/javascript">

    function onTipoSolicitanteChange(s) {
        var tipo = s.GetValue();
        var esNatural = tipo === "Natural";

        // Mostrar/Ocultar campos
        formDenuncia.GetItemByName("itemIdentidad").SetVisible(esNatural);
        formDenuncia.GetItemByName("itemNombre").SetVisible(esNatural);
        formDenuncia.GetItemByName("itemApellido").SetVisible(esNatural);

        formDenuncia.GetItemByName("itemRTN").SetVisible(!esNatural);
        formDenuncia.GetItemByName("itemInstitucion").SetVisible(!esNatural);

        // Limpiar comunes
        limpiarCampo(tbCorreo);
        limpiarCampo(tbConfirmCorreo);
        limpiarCampo(tbTelefono);

        // Limpiar específicos
        if (esNatural) {
            limpiarCampo(tbRTN);
            limpiarCampo(tbInstitucion);
        } else {
            limpiarCampo(tbIdentidad);
            limpiarCampo(tbNombre);
            limpiarCampo(tbApellido);
        }
    }

    function limpiarCampo(ctrl) {
        if (ctrl && ctrl.SetValue) {
            ctrl.SetValue("");
        }
    }

</script>
<!-- body -->
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
                                <li><a href="Content/Manual/Manual_Usuario.pdf" target="_blank"><i class="fa fa-book"></i>Manual de Usuario</a></li>
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

    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="services">
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="titlepage text_align_left">
                            <h3 class="text-center titulo-formulario"> SOLICITUD DE CONSTANCIA </h3>
                            <h3 class="text-center titulo-formulario">DE NO TENER CUENTAS PENDIENTES CON EL ESTADO DE HONDURAS</h3>
                          <h3 class="text-center titulo-formulario">SISTEMA DE SOLICITUD DE CONSTANCIAS EN LÍNEA </h3>
                        </div>
                    </div>
                </div>
            </div>

            <ol class="stepper">
                <li class="active" id="step1-tab">Datos del Solicitante</li>
                <li id="step2-tab">Carga de Archivos</li>
                <li id="step3-tab">Confirmación</li>
            </ol>
            <br />
            <div id="step1" class="step-content">
                <dx:ASPxFormLayout runat="server" ID="formDenuncia" CssClass="formLayout" ClientInstanceName="formDenuncia">

                    <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />

                    <Items>
                        <dx:LayoutItem ShowCaption="False" HorizontalAlign="Left">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <table>
                                        <tr>
                                            <td><strong>Bienvenido al formulario de solicitud de constancia de no tener cuentas pendientes con el Estado de Honduras, por favor llenar los siguientes campos para la solicitud de su constancia.
                                                <br />
                                                <br />
                                            </strong>Los campos marcados con <span class="auto-style4">*</span> son obligatorios.
                                            </td>
                                        </tr>
                                    </table>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Tipo de Solicitante">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxRadioButtonList ID="rbTipoSolicitante" runat="server" ClientInstanceName="rbTipoSolicitante" RepeatDirection="Horizontal">
                                        <Items>
                                            <dx:ListEditItem Text="Persona Natural" Value="Natural" Selected="True" />
                                            <dx:ListEditItem Text="Persona Jurídica" Value="Juridica" />
                                        </Items>
                                        <ClientSideEvents
                                            Init="function(s,e){ setTimeout(function(){ onTipoSolicitanteChange(rbTipoSolicitante); }, 100); }"
                                            ValueChanged="onTipoSolicitanteChange" />
                                    </dx:ASPxRadioButtonList>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Número de Identidad (sin guiones)" Name="itemIdentidad">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">

                                    <dx:ASPxTextBox ID="tbIdentidad" ClientInstanceName="tbIdentidad" runat="server" NullText="Número de Identidad (sin guiones)"
                                        ToolTip="Ingresar el Número de Identidad (sin guiones)."
                                        onkeypress="javascript:return solonumeros(event)" MaxLength="13">
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic"
                                            ErrorTextPosition="Bottom" SetFocusOnError="True" EnableCustomValidation="True"
                                            ErrorDisplayMode="Text" CausesValidation="True">
                                            <RequiredField ErrorText="El Número de Identidad es obligatorio" IsRequired="true" />
                                        </ValidationSettings>
                                        <ClientSideEvents Validation="function(s, e) { CustomValidateDNI(s, e); }" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Nombres del Solicitante" Name="itemNombre">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbNombre" ClientInstanceName="tbNombre" runat="server" NullText="Nombres del Solicitante" ToolTip="Ingrese sus Nombres" Validation="tbNombre_Validation">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                            <RequiredField ErrorText="Los Nombres son obligatorios." IsRequired="true" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyUp="OnKeyUpUpper"
                                            Validation="CustomValidateNombreApellido" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Apellidos del Solicitante" Name="itemApellido">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbApellido" ClientInstanceName="tbApellido" runat="server" NullText="Apellidos del Solicitante" ToolTip="Ingrese sus Apellidos" Validation="tbApellido_Validation">

                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                            <RequiredField ErrorText="Los Apellidos son obligatorios." IsRequired="true" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyUp="OnKeyUpUpper"
                                            Validation="CustomValidateNombreApellido" />
                                    </dx:ASPxTextBox>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>

                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Número de RTN (sin guiones)" Name="itemRTN">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbRTN" ClientInstanceName="tbRTN" runat="server" NullText="Número de RTN (sin guiones)" ToolTip="Ingrese el número de RTN sin guiones." MaxLength="14"
                                        onkeypress="javascript:return solonumeros(event)">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                            <RequiredField ErrorText="El RTN es obligatorio." IsRequired="true" />
                                        </ValidationSettings>
                                        <ClientSideEvents Validation="function(s, e) { CustomValidateRTN(s, e); }" />

                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Nombre de la Empresa/Institución" Name="itemInstitucion">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbInstitucion" ClientInstanceName="tbInstitucion" runat="server" NullText="Nombre de la Empresa/Institución" ToolTip="Ingrese el nombre de la Empresa o Institución.">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                            <RequiredField ErrorText="El nombre de la institución es obligatorio." IsRequired="true" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyUp="OnKeyUpUpper"
                                            Validation="CustomValidateNombreEmpresa" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Correo Electrónico" Name="itemCorreo">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbCorreo" ClientInstanceName="tbCorreo" runat="server" NullText="Correo Electrónico" ToolTip="Ingrese su correo electrónico.">
                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" EnableCustomValidation="True" ErrorDisplayMode="ImageWithText" CausesValidation="True">
                                            <RegularExpression ErrorText="El Correo Electrónico no es válido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                            <RequiredField ErrorText="El Correo Electrónico es obligatorio." IsRequired="true" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyUp="OnKeyUpUpper" />

                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Confirmar Correo Electrónico" Name="itemConfirmCorreo">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbConfirmCorreo" ClientInstanceName="tbConfirmCorreo" runat="server" NullText="Confirmar Correo Electrónico" ToolTip="Confirme su correo electrónico.">
                                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" EnableCustomValidation="True" ErrorDisplayMode="ImageWithText" CausesValidation="True">
                                            <RegularExpression ErrorText="El Correo Electrónico no es válido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                            <RequiredField ErrorText="Se requiere confirmación Correo Electrónico." IsRequired="true" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyUp="OnKeyUpUpper"
                                            Validation="function(s, e) {
                                          var originalCorreo = tbCorreo.GetText();
                                          var confirmCorreo = s.GetText();
                                          e.isValid = (originalCorreo == confirmCorreo);
                                          e.errorText = 'El correo electrónico no coincide.';
                                           }" />
                                    </dx:ASPxTextBox>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>

                        <dx:LayoutItem Caption="Celular/Teléfono(Sin guiones)" Name="itemTelefono">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxTextBox ID="tbTelefono" ClientInstanceName="tbTelefono" runat="server" NullText="Celular/Teléfono(Sin guiones)"
                                        ToolTip="Ingrese su número de Celular/Teléfono" Validation="tbTelefono_Validation" onkeypress="javascript:return solonumeros(event)" MaxLength="8" Size="8">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom"
                                            SetFocusOnError="True" ErrorDisplayMode="ImageWithText" EnableCustomValidation="True">
                                            <RequiredField ErrorText="El Celular/Teléfono es obligatorio." IsRequired="true" />
                                        </ValidationSettings>
                                        <ClientSideEvents Validation="function(s, e) { CustomValidateTelefono(s, e); }" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>

                        </dx:LayoutItem>

                    </Items>

                </dx:ASPxFormLayout>
                <asp:HiddenField ID="hfIdentidad" runat="server" />
                <asp:HiddenField ID="hfNombre" runat="server" />
                <asp:HiddenField ID="hfApellido" runat="server" />
                <asp:HiddenField ID="hfCorreo" runat="server" />
                <asp:HiddenField ID="hfConfirmCorreo" runat="server" />
                <asp:HiddenField ID="hfTelefono" runat="server" />
                <br />

                <div class="pager d-flex justify-content-center">
                    <ul class="pager list-inline">
                        <li class="list-inline-item next">
                            <a href="#" class="btn btn-primary" onclick="event.preventDefault(); goToStep(2);">Siguiente</a>

                        </li>
                    </ul>
                </div>

            </div>

            <!-- Step 2: Carga de Archivos -->

            <div id="step2" class="step-content" style="display: none;">
                <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout1" CssClass="formLayout">
                    <Items>
                        <dx:LayoutItem ShowCaption="False">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <div class="container">
                                        <div class="row">
                                            <!-- Archivo de Solicitud -->
                                            <div class="col-md-8 offset-md-2">
                                                <div class="form-group mb-4">
                                                    <label for="fileUpload" class="required-asterisk">Archivo de su Solicitud</label>
                                                    <i class="fas fa-info-circle text-primary" data-bs-toggle="tooltip" title="Formatos permitidos:.pdf, .jpeg, .jpg, .png "></i>
                                                    <asp:FileUpload ID="fileUpload" runat="server" ClientIDMode="Static" accept=".pdf,.jpeg,.jpg,.png" class="form-control"
                                                        onchange="Filevalidation()" />
                                                    <asp:Label ID="lblUploadStatus" runat="server"  ClientIDMode="Static" Text=""></asp:Label>
                                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" ClientIDMode="Static" class="text-danger" />
                                                    <asp:Label ID="lblSelected" runat="server" ForeColor="Green" ClientIDMode="Static" class="text-success" />
                                                    <asp:Label ID="lblSize" runat="server" ForeColor="Green" ClientIDMode="Static" class="text-success" />
                                                </div>
                                            </div>

                                            <!-- Archivo de Identidad -->
                                            <div class="col-md-8 offset-md-2">
                                                <div class="form-group mb-4">
                                                    <label for="fileUpload1" class="required-asterisk">Archivo de Identidad/RTN</label>
                                                    <i class="fas fa-info-circle text-primary" data-bs-toggle="tooltip" title="Formatos permitidos:.pdf,.jpeg,.jpg,.png "></i>
                                                    <asp:FileUpload ID="fileUpload1" runat="server" ClientIDMode="Static" accept=".pdf,.jpeg,.jpg,.png" class="form-control"
                                                        onchange="Filevalidation2()" />
                                                    <asp:Label ID="lblUploadStatus1" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                                    <asp:Label ID="lblError1" runat="server"  ClientIDMode="Static" ForeColor="Red" class="text-danger" />
                                                    <asp:Label ID="lblSelected1" runat="server" ClientIDMode="Static" ForeColor="Green" class="text-success" />
                                                    <asp:Label ID="lblSize1" runat="server" ClientIDMode="Static" ForeColor="Green" class="text-success" />
                                                </div>
                                            </div>

                                            <!-- Archivo de Recibo -->
                                            <div class="col-md-8 offset-md-2">
                                                <div class="form-group mb-4">
                                                    <label for="fileUpload2" class="required-asterisk">Archivo de su Recibo</label>
                                                    <i class="fas fa-info-circle text-primary" data-bs-toggle="tooltip" title="Formatos permitidos:.pdf, .jpeg, .jpg, .png "></i>
                                                    <asp:FileUpload ID="fileUpload2" runat="server" ClientIDMode="Static" accept=".jpeg,.pdf ,.jpg, .png" class="form-control"
                                                        onchange="Filevalidation1()" />
                                                    <asp:Label ID="lblUploadStatus2" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                                    <asp:Label ID="lblError2" runat="server" ClientIDMode="Static" ForeColor="Red" class="text-danger" />
                                                    <asp:Label ID="lblSelected2" runat="server" ClientIDMode="Static" ForeColor="Green" class="text-success" />
                                                    <asp:Label ID="lblSize2" runat="server" ClientIDMode="Static" ForeColor="Green" class="text-success" />
                                                </div>
                                            </div>
                                        </div>

                                        <!-- Hidden Fields -->
                                        <asp:HiddenField ID="hfFileUpload" runat="server" />
                                        <asp:HiddenField ID="hfFileUpload1" runat="server" />
                                        <asp:HiddenField ID="hfFileUpload2" runat="server" />
                                    </div>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                <div class="text-center mt-3">
                    <a href="#" class="btn btn-secondary me-2" onclick="goToStep(1)">Anterior</a>
                    <a href="#" class="btn btn-primary" onclick="goToStep(3)">Siguiente</a>
                </div>
            </div>

            <!-- Step 3 -->

            <div id="step3" class="step-content" style="display: none;">
                <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout2" CssClass="formLayout mb-4">
                    <Items>
                        <dx:LayoutItem ShowCaption="False" ColSpan="1" HorizontalAlign="Center">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="ASPxButton2" runat="server" Text="<i class='fas fa-search'></i> Revisar Solicitud"
                                        EncodeHtml="false" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn-revisar" HorizontalAlign="Right" Enabled="true" ClientVisible="True"
                                        ClientInstanceName="btnEnviarCodigo" ToolTip="Haz clic aquí para ver el resumen de su solicitud antes de enviarla">
                                        <ClientSideEvents Click="mostrarResumen" />
                                    </dx:ASPxButton>

                                    <dx:ASPxCallback ID="ASPxCallback_EnviarToken" runat="server" ClientInstanceName="ASPxCallback_EnviarToken" OnCallback="ASPxCallback_EnviarToken_Callback"></dx:ASPxCallback>
                                    <dx:ASPxCallback ID="ASPxCallback_Guardar_Datos" runat="server" OnCallback="ASPxCallback_Guardar_Datos_Callback" ClientInstanceName="ASPxCallback_Guardar_Datos">
                                        <ClientSideEvents CallbackComplete="Guardar_Datos_Complete" />
                                    </dx:ASPxCallback>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ColSpan="1" ShowCaption="False" HorizontalAlign="Center">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>

                <dx:ASPxPopupControl ID="popupResumen" runat="server" ClientInstanceName="popupResumen" HeaderText="Resumen de la Solicitud"
                    CloseAction="CloseButton" CloseOnEscape="true" CssClass="popup popup-responsive" CloseAnimationType="None" AllowResize="False"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" SettingsAdaptivity-Mode="OnWindowInnerWidth"
                    AllowDragging="True" EnableViewState="False" AutoUpdatePosition="true"
                    SettingsAdaptivity-VerticalAlign="WindowCenter" PopupAnimationType="Fade" Width="500px" Height="200px">
                    <HeaderStyle CssClass="headerpopup" />
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table class="dx-justification" style="width: 100%;">
                                <tr>
                                    <td id="resumenContent" class="dx-ac" style="text-align: left;">
                                        <!-- Contenido dinámico del resumen -->
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">
                                        <br />
                                        <dx:ASPxButton ID="btnConfirmar" runat="server" Text="Confirmar y Enviar" AutoPostBack="False" UseSubmitBehavior="false"
                                            CssClass="btn-confirmar" ClientInstanceName="btnConfirmar">
                                            <ClientSideEvents Click="btnEnviarCodigo_Click" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

                <dx:ASPxPopupControl ID="popupToken" runat="server" ClientInstanceName="popupToken" HeaderText="Verificación de Código" CloseAction="CloseButton" CloseOnEscape="true" CssClass="popup"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="310px" MinHeight="214px" Width="400px" Height="200px" SettingsAdaptivity-Mode="OnWindowInnerWidth" CloseAnimationType="None" AllowResize="False" AutoUpdatePosition="True" SettingsAdaptivity-VerticalAlign="WindowCenter"
                    AllowDragging="True" EnableViewState="False" ClientVisible="false">
                    <HeaderStyle CssClass="headerpopup" />
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table class="dx-justification">
                                <tr>
                                    <td style="text-align: center;">
                                        <dx:ASPxLabel ID="lblTokenPrompt" runat="server" Text="Favor ingrese el código enviado a su correo electrónico." />
                                        <br />
                                        <br />
                                        <dx:ASPxTextBox ID="tbToken" runat="server" NullText="Código de Verificación" ClientInstanceName="tbToken" Width="100%" />
                                        <br />
                                        <br />
                                        <dx:ASPxButton ID="btnVerificarToken" runat="server" Text="Verificar Código" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn-confirmar" ClientInstanceName="btnVerificarToken">
                                            <ClientSideEvents Click="btnVerificarToken_Click" />
                                        </dx:ASPxButton>
                                        <dx:ASPxCallback ID="ASPxCallback_VerificarToken" runat="server" ClientInstanceName="ASPxCallback_VerificarToken" OnCallback="ASPxCallback_VerificarToken_Callback">
                                            <ClientSideEvents CallbackComplete="function(s, e) { TokenVerificationComplete(e.result); }" />
                                        </dx:ASPxCallback>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

                <dx:ASPxPopupControl ID="Relacionado" runat="server" ClientInstanceName="Relacionado" CssClass="popup"
                    AllowDragging="true" HeaderText="Registro de Solicitud"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseOnEscape="true"
                    EnableViewState="False" AutoUpdatePosition="true" SettingsAdaptivity-Mode="OnWindowInnerWidth" CloseAnimationType="None" AllowResize="False"
                    SettingsAdaptivity-VerticalAlign="WindowCenter" PopupAnimationType="Fade" Width="700px" Height="750px">
                    <HeaderStyle CssClass="headerpopup" />
                    <ClientSideEvents Shown="function() { showConfirmationMessage1(); }" CloseUp="ClosePopupRelacionado" />
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <div id="popupContent" style="width: 100%; height: 100%; overflow: auto;"></div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

           
            </div>

            <div id="btnAnteriorSoloPaso3" class="text-center mt-3" style="display: none;">
                <a href="#" class="btn btn-secondary me-2" onclick="goToStep(2)">Anterior</a>
            </div>
        </div>
    </form>

    <!-- end innva -->
    <!-- footer -->
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


    /* Función para tooltips*** */
    document.addEventListener('DOMContentLoaded', function () {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    });


    //STEPER//

    var currentStep = 1;


    function validateStep(stepNum) {
        if (stepNum === 1) {
            var isValid = true;

            // Lista de controles obligatorios posibles (usa los ClientInstanceName)
            var requiredControls = [
                tbIdentidad,
                tbNombre,
                tbApellido,
                tbRTN,
                tbInstitucion,
                tbCorreo,
                tbConfirmCorreo,
                tbTelefono
            ];

            // Validar solo los visibles
            for (var i = 0; i < requiredControls.length; i++) {
                var ctrl = requiredControls[i];
                var elem = ctrl.GetMainElement ? ctrl.GetMainElement() : null;

                if (elem && elem.offsetParent !== null) {
                    var value = ctrl.GetValue();
                    if (value === null || value === undefined || value.toString().trim() === "") {
                        isValid = false;
                        break;
                    }
                }
            }

            // Validación de coincidencia de correo solo si están visibles
            var correoVisible = tbCorreo.GetMainElement().offsetParent !== null;
            var confirmCorreoVisible = tbConfirmCorreo.GetMainElement().offsetParent !== null;

            if (correoVisible && confirmCorreoVisible) {
                var correo = tbCorreo.GetValue();
                var confirmar = tbConfirmCorreo.GetValue();
                if (correo !== confirmar) {
                    Swal.fire({
                        icon: 'warning',
                        title: '¡Alerta!',
                        text: 'El correo y la confirmación de correo no coinciden.',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Aceptar'
                    });
                    return false;
                }
            }

            // Mostrar alerta general si hay campos vacíos
            if (!isValid) {
                Swal.fire({
                    icon: 'warning',
                    title: '¡Alerta!',
                    text: 'Por favor, complete los campos obligatorios para seguir a la seccion de "Carga de Archivos".',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'Aceptar'
                });
                return false;
            }

            return true;
        }

        // PASO 2: Validación de archivos
        if (stepNum === 2) {
            var fileSolicitud = document.getElementById("fileUpload");
            var fileId = document.getElementById("fileUpload1");
            var fileRecibo = document.getElementById("fileUpload2");

            var filesSelected = 0;
            if (fileSolicitud && fileSolicitud.value !== "") filesSelected++;
            if (fileId && fileId.value !== "") filesSelected++;
            if (fileRecibo && fileRecibo.value !== "") filesSelected++;

            if (filesSelected < 3) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Alerta',
                    text: 'Debe cargar los 3 archivos solicitados antes de continuar.',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'Aceptar'
                });
                return false;
            }

            return true;
        }

        return true; // Para otros pasos si los hay
    }


    function goToStep(stepNumber) {
        // Si el usuario está intentando avanzar (no retroceder)
        if (stepNumber > currentStep) {
            // Validar el paso actual antes de avanzar
            var esValido = validateStep(currentStep);
            if (!esValido) {
                return; // Detener avance si no pasa la validación
            }
        }

        // Ocultar todos los pasos
        document.querySelectorAll('.step-content').forEach(function (el) {
            el.style.display = 'none';
        });

        // Mostrar el paso deseado
        var paso = document.getElementById('step' + stepNumber);
        if (paso) {
            paso.style.display = 'block';
        }

        // Actualizar el estado visual del stepper (si lo estás usando)
        document.querySelectorAll('.stepper li').forEach(function (el, index) {
            el.classList.toggle('active', index + 1 === stepNumber);
        });

        // Mostrar u ocultar el botón específico del paso 3 (si aplica)
        const btnPaso3 = document.getElementById('btnAnteriorSoloPaso3');
        if (btnPaso3) {
            btnPaso3.style.display = (stepNumber === 3) ? 'block' : 'none';
        }

        // Actualizar el número de paso actual
        currentStep = stepNumber;
    }


    function resetStepper() {
        // Limpiar todos los campos de texto
        document.querySelectorAll("input").forEach(input => input.value = "");

        // Limpiar campos de DevExpress
        ASPxClientEdit.ClearEditorsInContainer();

        // Limpiar archivos
        ["fileUpload", "fileUpload1", "fileUpload2"].forEach(id => {
            let el = document.getElementById(id);
            if (el) el.value = "";
        });

        // Volver al paso 1
        goToStep(1);
    }


    function enviarFormulario() {
        // Aquí va la lógica de tu envío (AJAX o POST normal)

        // Luego mostrar mensaje y resetear
        Swal.fire({
            icon: 'success',
            title: '¡Solicitud enviada!',
            text: 'Gracias por completar el formulario.',
            confirmButtonText: 'Aceptar'
        }).then(() => {
            resetStepper();
        });
    }


    /*Adaptar step1 según los diferentes dospositivos */
    function setStep1Height() {

        const vh = window.innerHeight * 0.01;

        document.documentElement.style.setProperty('--vh', `${vh}px`);
    }

    window.addEventListener('load', setStep1Height);

    window.addEventListener('resize', setStep1Height);

    window.addEventListener('orientationchange', () => {

        setTimeout(setStep1Height, 200);
    });

    window.addEventListener('scroll', () => {
        setTimeout(setStep1Height, 100);
    });


    //********************************************************************************************* */


    var fileIdIdent = <%= UtilClass.FileId_ident %>;
    var fileIdsolicitud = <%= UtilClass.FileId_solicitud %>;
    var fileIdrecib = <%= UtilClass.FileId_recibo %>;


    /*ARCHIVO DE SOLICITUD*/
    function Filevalidation() {
        var lblFile = document.getElementById("<%=lblSelected.ClientID %>");
        lblFile.innerHTML = "";

        var lblError = document.getElementById("<%=lblError.ClientID %>");
        lblError.innerHTML = "";

        var fileUpload = document.getElementById("<%=fileUpload.ClientID %>");
        var lblSize = document.getElementById("<%=lblSize.ClientID %>");
        lblSize.innerHTML = "";

        var allowedFiles = [".jpeg", ".pdf", ".jpg", ".png"];
        var regex = new RegExp("([a-zA-Z0-9\\s_\\.\\-:])+(" + allowedFiles.join('|') + ")$");

        if (!regex.test(fileUpload.value.toLowerCase())) {
            lblError.innerHTML = "Por favor, cargue archivos con extensiones: <b>" + allowedFiles.join(', ') + "</b>.";
            return false;
        } else {
            if (fileUpload.files.length > 0) {
                for (var i = 0; i < fileUpload.files.length; i++) {
                    var fsize = fileUpload.files.item(i).size;
                    var file = Math.round(fsize / 1024); // KB

                    if (file >= 5120) {
                        lblError.innerHTML = "";
                        lblSize.innerHTML = "";
                        fileUpload.value = "";

                        // Swal alert personalizado
                        Swal.fire({
                            icon: 'error',
                            title: 'Archivo muy grande',
                            text: 'El archivo debe pesar menos de 5 MB.',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Aceptar'
                        });

                        return false;
                    } else {
                        lblSize.innerHTML = '<b>' + file + '</b> KB de tamaño seleccionado.';
                    }
                }
            }

            lblFile.innerHTML = fileUpload.files.item(0).name;
        }
    }

    /*ARCHIVO RECIBO*/
    function Filevalidation1() {
        var lblFile2 = document.getElementById("<%=lblSelected2.ClientID %>");
        lblFile2.innerHTML = "";
        var lblError2 = document.getElementById("<%=lblError2.ClientID %>");
        lblError2.innerHTML = "";
        var fileUpload2 = document.getElementById("<%=fileUpload2.ClientID %>");
        var lblSize2 = document.getElementById("<%=lblSize2.ClientID %>");
        lblSize2.innerHTML = "";
        var allowedFiles = [".jpeg", ".pdf", ".jpg", ".png"];
        var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
        if (!regex.test(fileUpload2.value.toLowerCase())) {
            lblError2.innerHTML = "Por favor, cargue archivos con extensiones: <b>" + allowedFiles.join(', ') + "</b> .";
            return false;
        } else {
            if (fileUpload2.files.length > 0) {
                for (var i = 0; i <= fileUpload2.files.length - 1; i++) {
                    var fsize = fileUpload2.files.item(i).size;
                    var file = Math.round((fsize / 1024));
                    // The size of the file.
                    if (file >= 5120) {
                        lblError2.innerHTML = "";
                        lblSize2.innerHTML = "";
                        fileUpload2.value = "";
                        // Swal alert personalizado
                        Swal.fire({
                            icon: 'error',
                            title: 'Archivo muy grande',
                            text: 'El archivo debe pesar menos de 5 MB.',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Entendido'
                        });
                        return false;

                    } else {

                        lblSize2.innerHTML = '<b>' + file + '</b> KB de tamaño seleccionado.';
                    }
                }
            }
            lblFile2.innerHTML = fileUpload2.files.item(0).name;


        }
    }


    /*ARCHIVO DE DNI*/
    function Filevalidation2() {
        var lblFile1 = document.getElementById("<%=lblSelected1.ClientID %>");
        lblFile1.innerHTML = "";
        var lblError1 = document.getElementById("<%=lblError1.ClientID %>");
        lblError1.innerHTML = "";
        var fileUpload1 = document.getElementById("<%=fileUpload1.ClientID %>");
        var lblSize1 = document.getElementById("<%=lblSize1.ClientID %>");
        lblSize1.innerHTML = "";
        var allowedFiles = [".jpeg", ".pdf", ".jpg", ".png"];
        //var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
        var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:\(\)])+(" + allowedFiles.join('|') + ")$");
        if (!regex.test(fileUpload1.value.toLowerCase())) {
            lblError1.innerHTML = "Por favor, cargue archivos con extensiones: <b>" + allowedFiles.join(', ') + "</b>.";
            return false;
        } else {
            if (fileUpload1.files.length > 0) {
                for (var i = 0; i <= fileUpload1.files.length - 1; i++) {
                    var fsize = fileUpload1.files.item(i).size;
                    var file = Math.round((fsize / 1024));
                    // The size of the file.
                    console.log("File size in KB: " + file); // Debugging line
                    if (file >= 5120) {
                        lblError1.innerHTML = "";
                        lblSize1.innerHTML = "";
                        fileUpload1.value = "";

                        // Swal alert personalizado
                        Swal.fire({
                            icon: 'error',
                            title: 'Archivo muy grande',
                            text: 'El archivo debe pesar menos de 5 MB.',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Entendido'
                        });
                        return false;

                    } else {

                        lblSize1.innerHTML = '<b>' + file + '</b> KB de tamaño seleccionado.';
                    }
                }
            }

            lblFile1.innerHTML = fileUpload1.files.item(0).name;


        }
    }

</script>




