<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudTSC1.aspx.cs" Inherits="SG_Constancia_TSC.SolicitudTSC1" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, user-scalable=no, maximum-scale=1.0, minimum-scale=1.0" />
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
                                        <dx:ASPxTextBox ID="tbEmail" runat="server" NullText="Correo Electrónico" ToolTip="Ingrese su Correo Electrónico" ClientInstanceName="tbEmail">
                                            <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                <RequiredField ErrorText="El Correo Electrónico es requerido." IsRequired="true" />
                                                <RegularExpression ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" ErrorText="Ingrese un Correo Electrónico válido." />
                                            </ValidationSettings>
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
                <div class="tab-pane fade step-content" id="step4">
                    <h4>Step 4: Confirmación</h4>
                    <p>Revise los datos ingresados antes de enviar su solicitud.</p>
                    <dx:ASPxButton ID="btnEnviar" runat="server" Text="Enviar Solicitud" OnClick="btnEnviar_Click" />
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>

            <div class="pager">
                <ul class="pager">
                    <li class="previous"><a href="#" class="btn btn-primary">Anterior</a></li>
                    <li class="next"><a href="#" class="btn btn-primary">Siguiente</a></li>
                </ul>
            </div>
        </div>
    </form>

    <script src="Content/js/jquery-3.0.0.min.js"></script>
    <script src="Content/js/bootstrap.min.js"></script>
    <script>
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
</script>
</body>
</html>