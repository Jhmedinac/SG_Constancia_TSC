﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Solicitud.aspx.cs" Inherits="SG_Constancia_TSC.Solicitud"  Async="true" %>

<%@ Import Namespace="SG_Constancia_TSC.UtilClass" %>


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


    <!-- jQuery (versión completa y recomendada) -->
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<!-- Bootstrap CSS -->
<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />

<!-- Bootstrap Icons -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

<!-- SweetAlert2 (una sola vez) -->
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

<!-- Popper.js (necesario para Bootstrap) -->
<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.4/dist/umd/popper.min.js"></script>

<!-- Bootstrap JS (una sola vez) -->
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>


    <link rel="icon" href="Content/favicon.ico" type="image/x-icon" />
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/CustomStyles.css" rel="stylesheet" />

    <link href="Content/Form1.css" rel="stylesheet" />
    <link href="Content/Denuncia.css" rel="stylesheet" />
   <link href="Content/Stepper.css" rel="stylesheet" />
<%--    <link href="Content/css/responsive.css" rel="stylesheet" />--%>
<%--    <link href="Content/css/owl.carousel.min.css" rel="stylesheet" />
    <link href="Content/Fontawesome/css/all.css" rel="stylesheet" />
    <link href="Content/Fontawesome/css/fontawesome.css" rel="stylesheet" />--%>

    <script src="Content/Denuncia.js"></script>
    <script src="Content/Consulta.js"></script>

    <style>
        .required-asterisk::after {
            content: ' *';
            color: red;
        }
    </style>

</head>
     <!-- body -->
<body class="main-layout">
    <header class="header-area">
        <div class="container">
            <div class="row d_flex">
                <div class="col-md-3">
                    <div class="logo">
                        <a href="https://www.tsc.gob.hn/index.php/denuncia-ciudadana/">
                            <img src="Content/Images/LOGO_TSC_2024_Logo.png" alt="#"/>T<span>SC</span></a>
                       
                    </div>
                </div>
                <div class="col-md-9 col-sm-12">
                    <div class="navbar-area">
                        <nav class="site-navbar">
                            <ul>
                                <%--<li>--%>
                                    <a href="Default.aspx" target="_blank">
                                        <i class="fa fa-home"></i> Inicio
                                    </a>
                                <%--</li>--%>
                                <br />
                                <%--<li>--%>
                                    <a href="Content/Manuales/Manual de Usuario.pdf" target="_blank">
                                        <i class="fa fa-book"></i> Manual de Usuario
                                    </a>
                                <%--</li>--%>
                            </ul>
<%--                            <button class="nav-toggler">
                                <span></span>
                            </button>--%>
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
                            <h2 class="text-center">FORMULARIO SOLICITUD DE CONSTANCIA de no tener cuentas pendientes con el estado</h2>
                            <h3 class="text-center">SISTEMA DE CONSULTAS SECRETARIA GENERAL TSC</h3>
                        </div>
                    </div>
                </div>
            </div>

            <ol class="stepper">
                <li class="active" id="step1-tab">Datos del Solicitante</li>
<%--                <li id="step2-tab">Información Adicional</li>--%>
                <li id="step2-tab">Carga de Archivos</li>
                <li id="step3-tab">Confirmación</li>
            </ol>

            <div class="tab-content" id="stepper-content">
                <!-- Step 1 -->
                <div class="tab-pane fade show active step-content dynamic-height-step1" id="step1" style="padding: 1rem;">
                   <%-- <h4>Step 1: Datos del Solicitante</h4>--%>
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
<%--                                            <ClientSideEvents KeyUp="function(s, e) { toUpperCase(s.GetInputElement().id); }" />
                                            <ClientSideEvents />--%>
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
                        </Items>
                    </dx:ASPxFormLayout>
                </div>

                <!-- Step 2 -->
                <%--<div class="tab-pane fade step-content text-center" id="step2" role="tabpanel" aria-labelledby="pills-step2-tab" style="min-height: 30vh; max-height: 40vh; height: auto; padding: 1rem;">--%>
                    <%--<h4>Step 2: Información Adicional</h4>--%>
                <%--<div class="tab-pane fade step-content text-center" id="step2" role="tabpanel">
                    <dx:ASPxFormLayout runat="server" ID="formDenuncia2" CssClass="formLayout">
                        <Items>--%>

<%--                            <dx:LayoutItem Caption="Dirección">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbDireccion" runat="server" NullText="Dirección" ToolTip="Ingrese su Dirección" ClientInstanceName="tbDireccion">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="La Dirección es requerida." IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>--%>
<%--                        </Items>
                    </dx:ASPxFormLayout>
                </div>--%>

                <!-- Step 3 -->
                <div class="tab-pane fade step-content text-center" id="pills-step3" role="tabpanel" aria-labelledby="pills-step3-tab" style="min-height: 30vh; max-height: 40vh; height: auto; padding: 1rem;">
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
                                                    <asp:FileUpload ID="fileUpload" runat="server" ClientIDMode="Static" accept=".jpeg" onchange="Filevalidation()"      />
                                                    <asp:Label ID="lblUploadStatus" runat="server" Text=""></asp:Label>
                                                    <hr />
                                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" />
                                                    <hr />
                                                    <asp:Label ID="lblSelected" runat="server" ForeColor="Green" />
                                                    <br />
                                                    <asp:Label ID="lblSize" runat="server" ForeColor="Green" />
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
                                                    <asp:FileUpload ID="fileUpload1" runat="server" ClientIDMode="Static"  accept=".pdf" onchange="Filevalidation2()" />
                                                    <asp:Label ID="lblUploadStatus1" runat="server" Text=""></asp:Label>
                                                    <hr />
                                                    <asp:Label ID="lblError1" runat="server" ForeColor="Red" />
                                                    <hr />
                                                    <asp:Label ID="lblSelected1" runat="server" ForeColor="Green" />
                                                    <br />
                                                    <asp:Label ID="lblSize1" runat="server" ForeColor="Green" />
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
                                                    <asp:FileUpload ID="fileUpload2" runat="server" ClientIDMode="Static"  accept=".jpeg"  onchange="Filevalidation1()" />
                                                    <asp:Label ID="lblUploadStatus2" runat="server" Text=""></asp:Label>
                                                    <hr />
                                                    <asp:Label ID="lblError2" runat="server" ForeColor="Red" />
                                                    <hr />
                                                    <asp:Label ID="lblSelected2" runat="server" ForeColor="Green" />
                                                    <br />
                                                    <asp:Label ID="lblSize2" runat="server" ForeColor="Green" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hfFileUpload" runat="server" />
                                    <asp:HiddenField ID="hfFileUpload1" runat="server" />
                                    <asp:HiddenField ID="hfFileUpload2" runat="server" />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:ASPxFormLayout>
                </div>

                <!-- Step 4 -->
             
                <%--<div class="tab-pane fade step-content d-flex align-items-center justify-content-center flex-column" id="step4" style="height: 50vh; padding: 1rem;">--%>
                <div class="tab-pane fade step-content d-flex align-items-center justify-content-center flex-column" id="step4" style="min-height: 30vh; max-height: 50vh; height: auto; padding: 1rem;">
                    <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout2" CssClass="formLayout mb-4">
                        <Items>
                            <dx:LayoutItem ShowCaption="False" ColSpan="1" HorizontalAlign="Center">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Revisar Solicitud" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn" HorizontalAlign="Right" Enabled="true" ClientVisible="True" ClientInstanceName="btnEnviarCodigo">
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
                    <!-- Popup de Resumen -->
                    <dx:ASPxPopupControl ID="popupResumen" runat="server" ClientInstanceName="popupResumen" HeaderText="Resumen de la Solicitud" CloseAction="CloseButton" CloseOnEscape="true" CssClass="popup"
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" MinWidth="500px" MinHeight="300px" AllowDragging="True" EnableViewState="False" AutoUpdatePosition="true">
                        <HeaderStyle CssClass="headerpopup" />
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <table class="dx-justification">
                                    <tr>
                                        <td id="resumenContent" class="dx-ac" style="text-align:left;">
                                            <!-- Contenido dinámico del resumen -->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:center;">
                                            <dx:ASPxButton ID="btnConfirmar" runat="server" Text="Confirmar y Enviar" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn" ClientInstanceName="btnConfirmar">
                                                <ClientSideEvents Click="btnEnviarCodigo_Click" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
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
                    <dx:ASPxPopupControl ID="Relacionado" runat="server" ClientInstanceName="Relacionado" CssClass="popup"
                        AllowDragging="true" HeaderText="Registro Solicitud"
                        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseOnEscape="true"
                        EnableViewState="False" AutoUpdatePosition="true" SettingsAdaptivity-Mode="OnWindowInnerWidth" CloseAnimationType="None" AllowResize="False"
                        SettingsAdaptivity-VerticalAlign="WindowCenter" PopupAnimationType="Fade" Width="900px" Height="700px">
                        <HeaderStyle CssClass="headerpopup" />
                        <ClientSideEvents Shown="function() { showConfirmationMessage1(); }" CloseUp="ClosePopupRelacionado" />
                        <ContentCollection>
                            <dx:PopupControlContentControl runat="server">
                                <div id="popupContent" style="width: 100%; height: 100%; overflow: auto;"></div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>


                </div>
                <div class="pager d-flex justify-content-center my-1">
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
                         <p> &copy; Copyright 2025 Informática | Tribunal Superior de Cuentas. <%--Design by <a href="https://html.design/"> <%: DateTime.Now.Year %> Free Html Template</a>--%></p>
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

            let currentStep = 0;

            // Función para actualizar la visibilidad de los botones según el paso actual
            function updateButtonVisibility() {
                const btnNext = document.querySelector('.pager .next a');
                const btnPrevious = document.querySelector('.pager .previous a');

                if (currentStep === 0) {
                    // Primer paso: ocultar botón "Anterior"
                    btnPrevious.style.display = 'none';
                } else {
                    btnPrevious.style.display = 'inline-block';
                }

                if (currentStep === steps.length - 1) {
                    // Último paso: ocultar botón "Siguiente"
                    btnNext.style.display = 'none';
                } else {
                    btnNext.style.display = 'inline-block';
                }
            }

            // Evento click para los pasos
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

                    currentStep = index;
                    updateButtonVisibility();
                });
            });

            // Configura los botones siguiente y anterior
            const btnNext = document.querySelector('.pager .next a');
            const btnPrevious = document.querySelector('.pager .previous a');

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

                    updateButtonVisibility();
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

                    updateButtonVisibility();
                }
            });

            // Configuración inicial de visibilidad
            updateButtonVisibility();

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





        function validarFormatoCorreo(correo) {
            var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
            return regex.test(correo);
        }

        function validarFormatoIDN(valor) {
            var regex = /^\d{13}$/;
            return regex.test(valor);
        }

        var fileIdIdent = <%= UtilClass.FileId_ident %>;
        var fileIdsolicitud = <%= UtilClass.FileId_solicitud %>;
        var fileIdrecib = <%= UtilClass.FileId_recibo %>;

        function Filevalidation() {
            var lblFile = document.getElementById("<%=lblSelected.ClientID %>");
            lblFile.innerHTML = "";
            var lblError = document.getElementById("<%=lblError.ClientID %>");
            lblError.innerHTML = "";
            var fileUpload = document.getElementById("<%=fileUpload.ClientID %>");
            var lblSize = document.getElementById("<%=lblSize.ClientID %>");
            lblSize.innerHTML = "";
            var allowedFiles = [".jpeg", ".jpg", ".png"];
            var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
            if (!regex.test(fileUpload.value.toLowerCase())) {
                lblError.innerHTML = "Please upload files having extensions: <b>" + allowedFiles.join(', ') + "</b> only.";
                return false;
            } else {
            if (fileUpload.files.length > 0) {
                for (var i = 0; i <= fileUpload.files.length - 1; i++) {
                    var fsize = fileUpload.files.item(i).size;
                    var file = Math.round((fsize / 1024));
                    // The size of the file.
                    if (file >= 5120) {
                        lblError.innerHTML = "";
                        lblSize.innerHTML = "";
                        fileUpload.value  = "";
                        alert("File too Big, please select a file less than 5mb");
                    } else {
                             document.getElementById('<%=lblSize.ClientID %>').innerHTML = '<b>' + file + '</b> KB Size File Selected ';
                         }
                     }
                 }
            lblFile.innerHTML = fileUpload.files.item(0).name;
           /* lblError.innerHTML = "";*/

             }
         }


        function Filevalidation1() {
            var lblFile2 = document.getElementById("<%=lblSelected2.ClientID %>");
            lblFile2.innerHTML = "";
            var lblError2 = document.getElementById("<%=lblError2.ClientID %>");
            lblError2.innerHTML = "";
            var fileUpload2 = document.getElementById("<%=fileUpload2.ClientID %>");
            var lblSize2 = document.getElementById("<%=lblSize2.ClientID %>");
            lblSize2.innerHTML = "";
            var allowedFiles = [".jpeg", ".jpg", ".png"];
            var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
            if (!regex.test(fileUpload2.value.toLowerCase())) {
                lblError2.innerHTML = "Please upload files having extensions: <b>" + allowedFiles.join(', ') + "</b> only.";
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
                     fileUpload2.value  = "";
                     alert("File too Big, please select a file less than 5mb");
                 } else {
                                 document.getElementById('<%=lblSize2.ClientID %>').innerHTML = '<b>' + file + '</b> KB Size File Selected ';
                             }
                         }
                     }
                     lblFile2.innerHTML = fileUpload2.files.item(0).name;
                     /* lblError.innerHTML = "";*/

                 }
        }

        function Filevalidation2() {
            var lblFile1 = document.getElementById("<%=lblSelected1.ClientID %>");
            lblFile1.innerHTML = "";
            var lblError1 = document.getElementById("<%=lblError1.ClientID %>");
            lblError1.innerHTML = "";
            var fileUpload1 = document.getElementById("<%=fileUpload1.ClientID %>");
            var lblSize1 = document.getElementById("<%=lblSize1.ClientID %>");
            lblSize1.innerHTML = "";
            var allowedFiles = [".pdf"];
            var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(" + allowedFiles.join('|') + ")$");
            if (!regex.test(fileUpload1.value.toLowerCase())) {
                lblError1.innerHTML = "Please upload files having extensions: <b>" + allowedFiles.join(', ') + "</b> only.";
                return false;
            } else {
            if (fileUpload1.files.length > 0) {
      for (var i = 0; i <= fileUpload1.files.length - 1; i++) {
          var fsize = fileUpload1.files.item(i).size;
          var file = Math.round((fsize / 1024));
          // The size of the file.
          if (file >= 5120) {
              lblError1.innerHTML = "";
              lblSize1.innerHTML = "";
              fileUpload1.value  = "";
              alert("File too Big, please select a file less than 5mb");
          } else {
                             document.getElementById('<%=lblSize1.ClientID %>').innerHTML = '<b>' + file + '</b> KB Size File Selected ';
                         }
                     }
                 }
                 lblFile1.innerHTML = fileUpload1.files.item(0).name;
                 /* lblError.innerHTML = "";*/

             }
         }
    </script>


   </body>
</html>

