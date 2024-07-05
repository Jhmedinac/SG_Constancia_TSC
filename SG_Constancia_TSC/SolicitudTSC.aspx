<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudTSC.aspx.cs" Inherits="SG_Constancia_TSC.SolicitudTSC" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0" />
    <title>Solicitud de Constancias TSC</title>
    <link rel="icon" href="favicon.ico" type="image/x-icon" />
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
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        .required-asterisk::after {
            content: ' *';
            color: red;
        }
    </style>
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
                                    <a href="Content/Manuales/Manual de Usuario Ciudadano.pdf" target="_blank">
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

           
             <ul class="nav nav-pills mb-5" id="pills-tab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="pills-step1-tab" data-toggle="pill" href="#pills-step1" role="tab" aria-controls="pills-step1" aria-selected="true">Step 1</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-step2-tab" data-toggle="pill" href="#pills-step2" role="tab" aria-controls="pills-step2" aria-selected="false">Step 2</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-step3-tab" data-toggle="pill" href="#pills-step3" role="tab" aria-controls="pills-step3" aria-selected="false">Step 3</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="pills-step4-tab" data-toggle="pill" href="#pills-step4" role="tab" aria-controls="pills-step4" aria-selected="false">Step 4</a>
                </li>
            </ul>

            <!-- Stepper Content -->
            <div class="tab-content" id="pills-tabContent">
                <!-- Step 1 -->
                <div class="tab-pane fade show active step-content" id="pills-step1" role="tabpanel" aria-labelledby="pills-step1-tab">
                    <h4>Step 1: Datos del Solicitante</h4>
                    <dx:ASPxFormLayout runat="server" ID="formDenuncia1" CssClass="formLayout">
                        <Items>
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
                                                <RequiredField ErrorText="Los Apellidos son requerido." IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Correo Electrónico Personal">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbCorreo" runat="server" NullText="Correo Electrónico Personal" ClientInstanceName="tbCorreo" ToolTip="Ingresar su correo electrónico.">
                                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" EnableCustomValidation="True" ErrorDisplayMode="Text" CausesValidation="true">
                                                <RegularExpression ErrorText="El Correo Electrónico no es válido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                <RequiredField ErrorText="El Correo Electrónico es requerida" IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Número de Teléfono o Celular">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxTextBox ID="tbTelefono" runat="server" NullText="Número de Teléfono o Celular" ClientInstanceName="tbTelefono" ToolTip="Ingrese su número de teléfono o celular.">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="El número de teléfono o celular es requerido." IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                    <button type="button" class="btn btn-primary" data-toggle="pill" href="#pills-step2">Next</button>
                </div>

                <!-- Step 2 -->
                <div class="tab-pane fade step-content" id="pills-step2" role="tabpanel" aria-labelledby="pills-step2-tab">
                    <h4 style="text-align: center;">Step 2: Datos de la Constancia</h4>
                    <dx:ASPxFormLayout runat="server" ID="formDenuncia2" CssClass="formLayout">
                        <Items>
                            <dx:LayoutItem Caption="Seleccione el tipo de constancia a solicitar">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxComboBox runat="server" ID="cbTipoConstancia" ClientInstanceName="cbTipoConstancia">
                                            <Items>
                                                <dx:ListEditItem Text="Seleccione el tipo de constancia" Value="" />
                                                <dx:ListEditItem Text="Laboral" Value="1" />
                                                <dx:ListEditItem Text="No Laboral" Value="2" />
                                            </Items>
                                        </dx:ASPxComboBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Descripción de la constancia">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxMemo ID="tbDescripcion" runat="server" ClientInstanceName="tbDescripcion" Width="100%" Height="120px">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="La descripción es requerida" IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxMemo>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem Caption="Fecha de Solicitud">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer runat="server">
                                        <dx:ASPxDateEdit ID="deFechaSolicitud" runat="server" ClientInstanceName="deFechaSolicitud">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="La fecha es requerida" IsRequired="true" />
                                            </ValidationSettings>
                                        </dx:ASPxDateEdit>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:ASPxFormLayout>
                    <button type="button" class="btn btn-secondary" data-toggle="pill" href="#pills-step1">Previous</button>
                    <button type="button" class="btn btn-primary" data-toggle="pill" href="#pills-step3">Next</button>
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
                                                    <label for="fileUpload" class="required-asterisk">Seleccione los archivos a cargar</label>
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

                    <button type="button" class="btn btn-secondary" data-toggle="pill" href="#pills-step2">Previous</button>
                    <button type="button" class="btn btn-primary" data-toggle="pill" href="#pills-step4">Next</button>
                </div>
                    

                <!-- Step 4 -->
                <div class="tab-pane fade step-content" id="pills-step4" role="tabpanel" aria-labelledby="pills-step4-tab">
                    <h4>Step 4: Confirmación</h4>
                    <div class="form-group">
                        <label>Confirmación de Datos</label>
                        <div id="confirmationDetails">
                            <!-- Aquí se puede mostrar un resumen de los datos ingresados en los pasos anteriores -->
                        </div>
                    </div>
                    <button type="button" class="btn btn-secondary" data-toggle="pill" href="#pills-step3">Previous</button>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/js/bootstrap.bundle.min.js"></script>
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
                         <p> &copy; Copyright 2024 IT | Tribunal Superior de Cuentas. <%--Design by <a href="https://html.design/"> <%: DateTime.Now.Year %> Free Html Template</a>--%></p>
                     </div>
                  </div>
               </div>
            </div>
         </div>
      </footer>

      <script src="Content/js/jquery.min.js"></script>
      <script src="Content/js/bootstrap.bundle.min.js"></script>
      <script src="Content/js/jquery-3.0.0.min.js"></script>
      <script src="Content/js/owl.carousel.min.js"></script>
      <script src="Content/js/custom.js"></script>
  
   </body>
</html>




