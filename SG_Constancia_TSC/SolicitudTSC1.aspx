<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudTSC1.aspx.cs" Inherits="SG_Constancia_TSC.SolicitudTSC1" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <!-- basic -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0" />
    <!-- mobile metas -->
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <meta name="viewport" content="initial-scale=1, maximum-scale=1"/>
    <!-- site metas -->
    
    <meta name="keywords" content=""/>
    <meta name="description" content=""/>
    <meta name="author" content=""/>

    <title>Solicitud de Constancias TSC</title>
    <link rel="icon" href="Content/favicon.ico" type="image/x-icon" />
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Form.css" rel="stylesheet" />
    <link href="Content/Denuncia.css" rel="stylesheet" />
    <link href="Content/Stepper.css" rel="stylesheet" />
    <link href="Content/css/responsive.css" rel="stylesheet" />
    <link href="Content/css/owl.carousel.min.css" rel="stylesheet" />
    <link href="Content/Fontawesome/css/all.css" rel="stylesheet" />
    <link href="Content/Fontawesome/css/fontawesome.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <script src="Content/Denuncia.js"></script>
    <script src="Content/Consulta.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        .required-asterisk::after {
            content: ' *';
            color: red;
        }
    </style>
        <!-- Cargar jQuery primero -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.0.0/jquery.min.js"></script>
    <!-- Cargar otros scripts que dependen de jQuery -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="Content/js/bootstrap.min.js"></script>
    <script src="Content/Denuncia.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.0.0/jquery.min.js"></script>
    <script src="Content/js/jquery-3.0.0.min.js"></script>
    <script src="Content/js/bootstrap.min.js"></script>


</head>

<body class="main-layout">
    <header class="header-area">
        <div class="container">
            <div class="row d_flex">
                <div class="col-md-3">
                    <div class="logo">
                        <a href="https://www.tsc.gob.hn/index.php/denuncia-ciudadana/">
                            <img src="Content/Images/TSCLogo.png" alt="TSC Logo" /> T<span>SC</span>
                        </a>
                    </div>
                </div>
                <div class="col-md-9 col-sm-12">
                    <div class="navbar-area">
                        <nav class="site-navbar">
                            <ul>
                                <li>
                                    <a href="https://www.tsc.gob.hn/index.php/sistema-para-la-declaracion-jurada-en-linea/" target="_blank">
                                        <i class="fa fa-home"></i> Inicio
                                    </a>
                                </li>
                                <li>
                                    <a href="Content/Manuales/Manual de Usuario.pdf" target="_blank">
                                        <i class="fa fa-book"></i> Manual de Usuario
                                    </a>
                                </li>
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
            <asp:scriptmanager id="ScriptManager1" runat="server" />
        <div class="services">
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="titlepage text_align_left">
                            <h2 class="text-center">FORMULARIO SOLICITUD DE CONSTANCIA</h2>
                            <h3 class="text-center">SISTEMA DE CONSULTAS SECRETARIA GENERAL TSC</h3>
                        </div>
                    </div>
                </div>
            </div>

            <ol class="stepper">
                <li class="active" id="step1-tab">Step 1</li>
                <li id="step2-tab">Step 2</li>
                <li id="step3-tab">Step 3</li>
                <li id="step4-tab">Step 4</li>
            </ol>

            <div class="tab-content" id="stepper-content">
                <!-- Step 1 -->
                <div class="tab-pane fade show active step-content" id="step1">
                    <h4>Step 1: Datos del Solicitante</h4>
                    <dx:ASPxFormLayout runat="server" ID="formDenuncia1" CssClass="formLayout">
                        <Items>
                            <dx:LayoutItem Caption="Identidad del Solicitante">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbIdentidad" runat="server" NullText="Número de identificación" ClientInstanceName="tbIdentidad" 
                                            MaskSettings-Mask="0000000000000" MaskSettings-ErrorText="El Documento Nacional de Identificación es Requerido."
                                            ToolTip="Ingesar el número de identificación nacional, numero de pasaporte o carnet de residencia según aplique">
                                                <MaskSettings Mask="0000000000000" ErrorText="El Número de identificación incorrecto" />
                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" />
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" EnableCustomValidation="True" 
                                                            ErrorDisplayMode="Text" CausesValidation="True">
                                                  <RequiredField ErrorText="El Número de identificación es requerida" IsRequired="true"/>
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Nombres del Solicitante">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbNombre" runat="server" NullText="Nombres del Solicitante" ToolTip="Ingrese sus Nombres" ClientInstanceName="tbNombre" CaptionSettings-RequiredMarkDisplayMode="Hidden">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="Los Nombres son requerido." IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Apellidos del Solicitante">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbApellido" runat="server" NullText="Apellidos del Solicitante" ToolTip="Ingrese sus Apellidos" ClientInstanceName="tbApellido">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="Los Apellidos son requeridos." IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Correo Electrónico">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbCorreo" runat="server" NullText="Correo Electrónico Personal" ClientInstanceName="tbCorreo" ToolTip="Ingresar su correo electrónico.">
                                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" EnableCustomValidation="True" ErrorDisplayMode="ImageWithText" CausesValidation="True">
                                                <RegularExpression ErrorText="El Correo Electrónico no es válido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                <RequiredField ErrorText="El Correo Electrónico es requerida" IsRequired="true" />
                                            </ValidationSettings>
                                            <ClientSideEvents KeyUp="function(s, e) { toUpperCase(s.GetInputElement().id); }" />
                                            <ClientSideEvents />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Confirmar Correo Electrónico Personal" ColSpan="1">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbConfirmCorreo" runat="server" NullText="Confirmar Correo Electrónico Personal" ClientInstanceName="tbConfirmCorreo" ToolTip="Confirmar su correo electrónico.">
                                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" EnableCustomValidation="True" ErrorDisplayMode="Text" CausesValidation="True">
                                                <RegularExpression ErrorText="El Correo Electrónico no es válido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                <RequiredField ErrorText="Se requiere confirmación Correo Electrónico Personal" IsRequired="true" />
                                            </ValidationSettings>
                                            <ClientSideEvents Validation="function(s, e) {
                                                var originalCorreo = tbCorreo.GetText();
                                                var confirmCorreo = s.GetText();
                                                e.isValid = (originalCorreo == confirmCorreo);
                                                e.errorText = 'Correo Electrónico Personal deben coincidir.';
                                            }" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                </div>

                <!-- Step 2 -->
                <div class="tab-pane fade step-content" id="step2">
                    <h4>Step 2: Información Adicional</h4>
                    <dx:ASPxFormLayout runat="server" ID="formDenuncia2" CssClass="formLayout">
                        <Items>
                            <dx:LayoutItem Caption="Teléfono">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbTelefono" runat="server" NullText="Teléfono" ToolTip="Ingrese su Teléfono" ClientInstanceName="tbTelefono">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="El Teléfono es requerido." IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Dirección">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbDireccion" runat="server" NullText="Dirección" ToolTip="Ingrese su Dirección" ClientInstanceName="tbDireccion">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="La Dirección es requerida." IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                </div>

                <!-- Step 3 -->
                <div class="tab-pane fade step-content text-center" id="pills-step3" role="tabpanel" aria-labelledby="pills-step3-tab">
                <h4 style="font-size: 1.5em;">Step 3: Carga de Archivos</h4>
                <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout1" CssClass="formLayout">
                    <Items>
                        <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <table class="table table-centered" style="width: 80%; margin: 0 auto;">
                                        <tr>
                                            <td>
                                                <div class="form-group">
                                                    <label for="fileUpload" class="required-asterisk">Seleccione los archivos de su solicitud</label>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="form-group">
                                                    <asp:FileUpload ID="fileUpload" runat="server" AllowMultiple="true" />
                                                    <asp:Label ID="lblUploadStatus" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="form-group">
                                                    <label for="fileUpload1" class="required-asterisk">Seleccione el Archivo de su Identidad</label>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="form-group">
                                                    <asp:FileUpload ID="fileUpload1" runat="server" AllowMultiple="true" />
                                                    <asp:Label ID="lblUploadStatus1" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="form-group">
                                                    <label for="fileUpload2" class="required-asterisk">Seleccione el Archivo de su Recibo</label>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="form-group">
                                                    <asp:FileUpload ID="fileUpload2" runat="server" AllowMultiple="true" />
                                                    <asp:Label ID="lblUploadStatus2" runat="server" Text=""></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>

<%--     <button type="button" class="btn btn-secondary" data-toggle="pill" href="#pills-step2">Previous</button>
     <button type="button" class="btn btn-primary" data-toggle="pill" href="#pills-step4">Next</button>--%>
                </div>

                <!-- Step 4 -->
                <%--<div class="tab-pane fade step-content d-flex align-items-center justify-content-center flex-column" id="step4" style="height: 50vh; padding: 1rem;">
                    <h4 class="mb-4">Step 4: Confirmación</h4>
                    <p class="mb-4">Revise los datos ingresados antes de enviar su solicitud.</p>
                    <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout2" CssClass="formLayout mb-4">
                        <Items>
                            <dx:LayoutItem ShowCaption="False" ColSpan="1" HorizontalAlign="Center">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxButton ID="btnEnviarCodigo" runat="server" Text="Enviar Código" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn" ClientInstanceName="btnEnviarCodigo">
                                            <ClientSideEvents Click="btnEnviarCodigo_Click" />
                                        </dx:ASPxButton>
                                        <dx:ASPxCallback ID="ASPxCallback_EnviarToken" runat="server" ClientInstanceName="ASPxCallback_EnviarToken" OnCallback="ASPxCallback_EnviarToken_Callback"></dx:ASPxCallback>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem ColSpan="1" ShowCaption="False" HorizontalAlign="Center">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxCheckBox ID="ckPolitica" runat="server" EncodeHtml="false" ClientInstanceName="ckPolitica" ClientVisible="False"
                                            Text="Acepto los términos y politicas del Tribunal Superior de Cuentas" ValidationSettings-CausesValidation="true"> 
                                            <ClientSideEvents CheckedChanged="Terminos" />
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                    <dx:ASPxPopupControl ID="popupToken" runat="server" ClientInstanceName="popupToken" HeaderText="Verificación de Código" CloseAction="CloseButton" CloseOnEscape="true" CssClass="popup"
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="310px" MinHeight="214px" Width="400px" Height="200px"
                        AllowDragging="True" EnableViewState="False" AutoUpdatePosition="true">
                        <HeaderStyle CssClass="headerpopup" />
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <table class="dx-justification">
                                    <tr>
                                        <td style="text-align:center;">
                                            <dx:ASPxLabel ID="lblTokenPrompt" runat="server" Text="Ingrese el código enviado a su correo:" />
                                            <br /><br />
                                            <dx:ASPxTextBox ID="tbToken" runat="server" NullText="Código de Verificación" ClientInstanceName="tbToken" Width="100%" />
                                            <br /><br />
                                            <dx:ASPxButton ID="btnVerificarToken" runat="server" Text="Verificar Código" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn" ClientInstanceName="btnVerificarToken">
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

                    <dx:ASPxButton ID="btnEnviar" runat="server" OnCallback="Guardar_Datos_Callback" Text="Enviar Solicitud" OnClick="btnEnviar_Click" CssClass="rounded-button mt-4" />
                    <asp:Label ID="lblMessage" runat="server" Text="" class="mt-4"></asp:Label>
                </div>--%>
                <div class="tab-pane fade step-content d-flex align-items-center justify-content-center flex-column" id="step4" style="height: 50vh; padding: 1rem;">
                    <h4 class="mb-4">Step 4: Confirmación</h4>
                    <p class="mb-4">Revise los datos ingresados antes de enviar su solicitud.</p>
                    <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout2" CssClass="formLayout mb-4">
                        <Items>
                            <dx:LayoutItem ShowCaption="False" ColSpan="1" HorizontalAlign="Center">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxButton ID="btnEnviarCodigo" runat="server" Text="Enviar Código" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn" ClientInstanceName="btnEnviarCodigo">
                                            <ClientSideEvents Click="btnEnviarCodigo_Click" />
                                        </dx:ASPxButton>
                                        <dx:ASPxCallback ID="ASPxCallback_EnviarToken" runat="server" ClientInstanceName="ASPxCallback_EnviarToken" OnCallback="ASPxCallback_EnviarToken_Callback"></dx:ASPxCallback>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem ColSpan="1" ShowCaption="False" HorizontalAlign="Center">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxCheckBox ID="ckPolitica" runat="server" EncodeHtml="false" ClientInstanceName="ckPolitica" ClientVisible="False"
                                            Text="Acepto los términos y politicas del Tribunal Superior de Cuentas" ValidationSettings-CausesValidation="true"> 
                                            <ClientSideEvents CheckedChanged="Terminos" />
                                        </dx:ASPxCheckBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>

                    <dx:ASPxPopupControl ID="popupToken" runat="server" ClientInstanceName="popupToken" HeaderText="Verificación de Código" CloseAction="CloseButton" CloseOnEscape="true" CssClass="popup"
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="310px" MinHeight="214px" Width="400px" Height="200px"
                        AllowDragging="True" EnableViewState="False" AutoUpdatePosition="true" ClientVisible="false"  >
                        <HeaderStyle CssClass="headerpopup" />
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <table class="dx-justification">
                                    <tr>
                                        <td style="text-align:center;">
                                            <dx:ASPxLabel ID="lblTokenPrompt" runat="server" Text="Ingrese el código enviado a su correo:" />
                                            <br /><br />
                                            <dx:ASPxTextBox ID="tbToken" runat="server" NullText="Código de Verificación" ClientInstanceName="tbToken" Width="100%" />
                                            <br /><br />
                                            <dx:ASPxButton ID="btnVerificarToken" runat="server" Text="Verificar Código" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn" ClientInstanceName="btnVerificarToken">
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
                    <dx:ASPxButton ID="btnEnviar" runat="server" Visible ="false"  Text="Enviar Solicitud" CssClass="rounded-button mt-4" ClientInstanceName="btnEnviar" OnClick="btnEnviar_Click">
                        <ClientSideEvents Click="btnEnviar_ClientClick" />
                    </dx:ASPxButton>
<%--                    <dx:ASPxButton ID="btnEnviar" runat="server" Text="Enviar Solicitud" CssClass="rounded-button mt-4" ClientInstanceName="btnEnviar" OnClick="btnEnviar_Click" >
                        <ClientSideEvents Click="btnEnviar_Click" />
                    </dx:ASPxButton>--%>
                    <asp:Label ID="lblMessage" runat="server" Text="" class="mt-4"></asp:Label>
                    <dx:ASPxPopupControl ID="Relacionado" runat="server" ClientInstanceName="Relacionado" 
                        AllowDragging="true" HeaderText="Pre-Registro" 
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseOnEscape="true" 
                        EnableViewState="False" AutoUpdatePosition="true" MinHeight="750px" SettingsAdaptivity-Mode="OnWindowInnerWidth" CloseAnimationType="None" AllowResize="False" SettingsAdaptivity-VerticalAlign="WindowCenter">
                        <ClientSideEvents Shown="popup_Shown_comprobante" />
                            <ContentCollection>
                                <dx:PopupControlContentControl runat="server">
                                    <dx:ASPxCallbackPanel runat="server" ID="callbackPane_comprobante" ClientInstanceName="callbackPane_comprobante"
                                        OnCallback="callbackPane_comprobante_Callback" RenderMode="Table" Width="100%" SettingsCollapsing-Modal="True">
                                        <PanelCollection> 
                                            <dx:PanelContent runat="server">
<%--                                                <dx:ASPxWebDocumentViewer    ID="ASPxWebDocumentViewer1"     runat="server"     ClientInstanceName="ASPxWebDocumentViewer1"     Height="750px" 
                                                    RightToLeft="True"     DisableHttpHandlerValidation="False">
                                                </dx:ASPxWebDocumentViewer>--%>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                     </dx:ASPxCallbackPanel>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        <ClientSideEvents CloseUp="ClosePopupRelacionado" />
                    </dx:ASPxPopupControl>
                </div>

            </div>

            <div class="pager d-flex justify-content-center my-3">
                <ul class="pager list-inline">
                    <li class="list-inline-item previous">
                        <a href="#" class="btn btn-secondary">Anterior</a>
                    </li>
                    <li class="list-inline-item next">
                        <a href="#" class="btn btn-success">Siguiente</a>
                    </li>
                </ul>
            </div>

        </div>
    </form>

          
      <!-- end innva -->
      <!-- footer -->
      <footer>
         <div class="footer">
            <div class="container">
               <div class="row">
                 <div class="col-md-12"> 
                    <ul class="conta">
                        <li> <span>Dirección</span> Centro civico Gubernamental, Bulevar Fuerzas Armadas <br /> Honduras, C.A </li>
                        <li> <span>Correo Eléctronico</span> <a href="mailto:tsc@tsc.gob.hn">tsc@tsc.gob.hn</a> </li>
                        <li> <span>Contacto</span> <a href="Javascript:void(0)">Tel(+504) 2230-3646 / 2228-3512 / 2228-7913 <br/> 2230-4152 / 2230-8242 / 2230-3732 </a> </li>
                      </ul>
                 </div>
          
                  <div class="col-md-12">
                    <div class="Informa">
                      <ul class="social_icon text_align_center">
                    
                        <li> <a href="https://www.tsc.gob.hn/"> <i class="fa fa-solid fa-globe"></i></a></li>
                       <li> <a href="http://www.facebook.com/tschonduras"> <i class="fa-brands fa-facebook"></i></a></li>
                        <li> <a href="http://www.twitter.com/tschonduras">  <i class="fa-brands fa-x-twitter"></i></a></li>
                      </ul>
                    </div>
                  </div>
               </div>
            </div>
            <div class="copyright text_align_center">
               <div class="container">
                  <div class="row">
                     <div class="col-md-12">
                         <p> &copy; Copyright 2024 Informática | Tribunal Superior de Cuentas. <%--Design by <a href="https://html.design/"> <%: DateTime.Now.Year %> Free Html Template</a>--%></p>
                     </div>
                  </div>
               </div>
            </div>
         </div>
      </footer>

    <script type="text/javascript" >

        document.addEventListener('DOMContentLoaded', function () {
            const steps = document.querySelectorAll('.stepper li');
            const contents = document.querySelectorAll('.step-content');

            steps.forEach((step, index) => {
                step.addEventListener('click', () => {
                    steps.forEach(s => s.classList.remove('active'));
                    contents.forEach(c => {
                        c.classList.remove('active');
                        c.classList.remove('show');
                    });

                    step.classList.add('active');
                    contents[index].classList.add('active');
                    contents[index].classList.add('show');
                });
            });

            // Configura los botones siguiente y anterior
            const btnNext = document.querySelector('.pager .next a');
            const btnPrevious = document.querySelector('.pager .previous a');
            let currentStep = 0;

            btnNext.addEventListener('click', (e) => {
                e.preventDefault();
                if (currentStep < steps.length - 1) {
                    steps[currentStep].classList.remove('active');
                    contents[currentStep].classList.remove('active');
                    contents[currentStep].classList.remove('show');
                    currentStep++;
                    steps[currentStep].classList.add('active');
                    contents[currentStep].classList.add('active');
                    contents[currentStep].classList.add('show');
                }
            });

            btnPrevious.addEventListener('click', (e) => {
                e.preventDefault();
                if (currentStep > 0) {
                    steps[currentStep].classList.remove('active');
                    contents[currentStep].classList.remove('active');
                    contents[currentStep].classList.remove('show');
                    currentStep--;
                    steps[currentStep].classList.add('active');
                    contents[currentStep].classList.add('active');
                    contents[currentStep].classList.add('show');
                }
            });
        });



        function CustomValidateID(s, e) {
            var id = e.value;
            if (!isValidID(id)) {
                e.isValid = false;
                if (hasSpecialCharacters(id)) {
                    e.errorText = "No debe contener caracteres especiales.";
                } else {
                    e.errorText = "El Número de identificación es incorrecto.";
                }
            }
        }


        function isValidID(id) {
            // Check if id is null or undefined
            if (!id) {
                return false;
            }

            // Verify if it has special characters
            if (hasSpecialCharacters(id)) {
                return false;
            }

            // Validate ID based on its length and type
            if (id.length === 13 && isNumeric(id)) {
                // Validation for Documento Nacional de Identificación (DNI)
                return true;
            } else if (id.length >= 6 && id.length <= 9 && isAlphanumeric(id)) {
                // Validation for passport
                return true;
            } else if (id.length === 8 && isAlphanumeric(id)) {
                // Validation for residency card
                return true;
            }

            // If none of the conditions match, return false
            return false;
        }

        function isNumeric(value) {
            return /^\d+$/.test(value);
        }

        function isAlphanumeric(value) {
            return /^[a-zA-Z0-9]+$/.test(value);
        }

        function hasSpecialCharacters(value) {
            var result = /[^a-zA-Z0-9]/.test(value);
            return result;
        }

              

        //function btnEnviar_Click(s, e) {
        //    popupToken.Show();
        //}



        //function TokenVerificationComplete(result) {
        //    try {
        //        var resultData = JSON.parse(result);
        //        var lblNombreUsuario = document.getElementById('lblNombreUsuario');
        //        var lblEstadoUsuario = document.getElementById('lblEstadoUsuario');
        //        var lblObs = document.getElementById('lblObs');
        //        var progressBarFill = document.getElementById('progress-bar-fill');
        //        var estadoColor = {
        //            "Pre-Registrado": "gray",
        //            "Procesado": "#FAC21A",
        //            "Registrado": "green",
        //            "Requiere Documentación": "orangered",
        //            "Rechazado": "red"
        //        };
        //        var estadoAvance = {
        //            "Pre-Registrado": "20%",
        //            "Requiere Documentación": "40%",
        //            "Procesado": "60%",
        //            "Registrado": "100%",
        //            "Rechazado": "100%"
        //        };

        //        if (resultData.success) {
        //            if (lblNombreUsuario) lblNombreUsuario.innerText = resultData.nombreUsuario;
        //            if (lblEstadoUsuario) {
        //                lblEstadoUsuario.innerText = resultData.estadoUsuario;
        //                if (resultData.estadoUsuario === "Requiere Documentación" || resultData.estadoUsuario === "Solicitud Rechazada") {
        //                    lblObs.style.display = "block";
        //                    lblObs.innerText = resultData.obs;
        //                } else if (resultData.estadoUsuario === "Pre-Registrado" || resultData.estadoUsuario === "Procesado" || resultData.estadoUsuario === "Registrado") {
        //                    lblObs.style.display = "none";
        //                }
        //                if (estadoColor[resultData.estadoUsuario]) {
        //                    progressBarFill.style.backgroundColor = estadoColor[resultData.estadoUsuario];
        //                    progressBarFill.style.width = estadoAvance[resultData.estadoUsuario];
        //                    progressBarFill.innerText = estadoAvance[resultData.estadoUsuario];
        //                }
        //            }

        //            tbIdentidad.SetText('');
        //            popupToken.Hide();
        //            popupUserStatus.Show();
        //        } else {
        //            Swal.fire({
        //                title: "¡Alerta!",
        //                text: resultData.message,
        //                icon: "error",
        //                confirmButtonColor: "#1F497D"
        //            });
        //            if (resultData.message === "Código de verificación incorrecto.") {
        //                tbToken.SetText('');
        //            } else if (resultData.message === "El código de verificación ha expirado.") {
        //                popupToken.Hide();
        //                tbToken.SetText('');
        //            } else {
        //                popupToken.Hide();
        //                tbToken.SetText('');
        //            }
        //        }
        //    } catch (e) {
        //        Swal.fire({
        //            title: "¡Error!",
        //            text: "Error en la verificación del código. Por favor, inténtelo de nuevo.",
        //            icon: "error",
        //            confirmButtonColor: "#1F497D"
        //        });
        //    }
        //}

        function validarFormatoCorreo(correo) {
            var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
            return regex.test(correo);
        }

        function validarFormatoIDN(valor) {
            var regex = /^\d{13}$/;
            return regex.test(valor);
        }




    </script>


   </body>
</html>