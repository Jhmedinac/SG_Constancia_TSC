<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="SG_Constancia_TSC.Upload"  Async="true" %>


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
            <title>Solicitud de Constancias TSC</title>
            <meta name="keywords" content=""/>
            <meta name="description" content=""/>
            <meta name="author" content=""/>
            <!-- bootstrap css -->
            <link rel="icon" href="favicon.ico" type="image/x-icon"/>
            <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
            <link href="Content/Form.css" rel="stylesheet" />
            <link href="Content/Denuncia.css" rel="stylesheet" />
            <link href="Content/css/responsive.css" rel="stylesheet" />
            <link href="Content/css/owl.carousel.min.css" rel="stylesheet" />
            <link href="Content/Fontawesome/css/all.css" rel="stylesheet" />
            <link href="Content/Fontawesome/css/fontawesome.css" rel="stylesheet" />
            <link href="Content/css/bootstrap.css" rel="stylesheet" />
            <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css"/>
            <script src="Content/Denuncia.js"></script>
            <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>


        <style>
        .required-asterisk::after {
            content: ' *';
            color: red;
        }
        </style>

        <script type="text/javascript">
        </script>
   </head>



   <body class="main-layout">
       <div id="list"></div>
        <header class="header-area">
            <div class="container">
               <div class="row d_flex">
                  <div class=" col-md-3">
                     <div class="logo">
                        <%--<a href="https://www.tsc.gob.hn/index.php/denuncia-ciudadana/"> <img src="Content/Images/TSCLogo.png"  alt="#"/>T<span>SC</span></a>--%>
                     </div>
                  </div>
                  <div class="col-md-9 col-sm-12">
                    <div class="navbar-area">
                      <nav class="site-navbar">
                        <ul>
                            <%--<li><a href="https://www.tsc.gob.hn/index.php/sistema-para-la-declaracion-jurada-en-linea/" target="_blank"><i class="fa fa-home"></i> Inicio</a></li>--%>
                             <%--<li><a href="Content/Manuales/Manual de Usuario Ciudadano.pdf" target="_blank"><i class="fa fa-book"></i> Manual de Usuario</a></li>--%>
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
                   <h2 style="text-align:center">FORMULARIO SOLICITUD DE CONSTANCIA</h2>
                     <h3 style="text-align:center">SISTEMA DE CONSULTAS SECRETARIA GENERAL TSC</h3>
                 </div>
               </div>
             </div>
        </div>
<%--       <asp:scriptmanager id="ScriptManager1" runat="server" />--%>
       
        <asp:PlaceHolder ID="phDenunciado" runat="server">
        <dx:ASPxFormLayout runat="server" ID="formDenuncia" CssClass="formLayout">
              <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="700" />
            <Items>
                   <dx:LayoutItem ShowCaption="False" HorizontalAlign="Left">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <table>
                                        <tr>
                                           <td >
                                              Bienvenido al Sistema de Solicitud de Constacias en Linea, favor llenar los siguientes campos para la solicitud de usuario.
                                             <br />
                                             <br />
                                              Los campos marcados con * son obligatorios.
                                           </td>
                                        </tr>
                                    </table>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                  </dx:LayoutItem>
                <dx:LayoutGroup Caption="1. DATOS DEL SOLICITABTE *" ColCount="1" GroupBoxDecoration="HeadingLine" UseDefaultPaddings="false" Paddings-PaddingTop="10">
                   <GroupBoxStyle>
                        <Caption Font-Bold="true" Font-Size="15" />
                    </GroupBoxStyle>
                     <Items>
                        <dx:LayoutItem Caption="Nombres del Solicitante" ColSpan="1" >
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
                        <dx:LayoutItem ColSpan="1" Caption="Apellidos del Solicitante">
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

                    <dx:LayoutItem Caption="Correo Electrónico Personal" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <dx:ASPxTextBox ID="tbCorreo" runat="server" NullText="Correo Electrónico Personal" ClientInstanceName="tbCorreo" ToolTip="Ingresar su correo electrónico.">
                                    <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" SetFocusOnError="True" EnableCustomValidation="True" ErrorDisplayMode="Text" CausesValidation="True">
                                        <RegularExpression ErrorText="El Correo Electrónico no es válido" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                        <RequiredField ErrorText="El Correo Electrónico es requerida" IsRequired="true" />
                                    </ValidationSettings>
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

                    <dx:LayoutItem Caption="Subir Identidad" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <h2>Upload File</h2>
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                                <div>
                                    <label for="file">Select file:</label>
                                    <asp:FileUpload ID="fileUpload1" runat="server" />
                                </div>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>

                    <dx:LayoutItem Caption="Subir Identidad" ColSpan="1">
                        <LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                                <div>
                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Subir Id" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn" ClientInstanceName="btnUpload" OnClick="btnUpload_Click">
                                    </dx:ASPxButton>
                                </div>
                            </dx:LayoutItemNestedControlContainer>
                        </LayoutItemNestedControlCollection>
                    </dx:LayoutItem>



                        <dx:LayoutItem ShowCaption="False" ColSpan="1" HorizontalAlign="Center">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                    <dx:ASPxButton ID="btnEnviarCodigo" runat="server" Text="Enviar Código" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn" ClientInstanceName="btnEnviarCodigo">
               
                                        <%--<ClientSideEvents Click="btnEnviarCodigo_Click" />--%>
                                    </dx:ASPxButton>
                                   <%-- <dx:ASPxCallback ID="ASPxCallback_EnviarToken" runat="server" ClientInstanceName="ASPxCallback_EnviarToken" OnCallback="ASPxCallback_EnviarToken_Callback"></dx:ASPxCallback>--%>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ColSpan="1" ShowCaption="False" HorizontalAlign="Center">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                       <dx:ASPxCheckBox ID="ckPolitica" runat="server" EncodeHtml="false" ClientInstanceName="ckPolitica" ClientVisible="False"
                                             Text="Acepto los términos y politicas del Tribunal Superior de Cuentas" ValidationSettings-CausesValidation="true"> 
                                            <%--<ClientSideEvents CheckedChanged="Terminos" />--%>
                                        </dx:ASPxCheckBox>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                       <dx:LayoutItem ShowCaption="False" ColSpan="1" Width="70px" HorizontalAlign="Center" >
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <table class="dx-justification">
                                                    <tr>
                                                        <td style="text-align:center">                                                          
                                                        </td>
                                                    </tr>
                                                </table>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
            <Items>
      
            </Items>
                      
            </dx:ASPxFormLayout>
        </asp:PlaceHolder>
        <div>
            <h2>Upload File</h2>
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            <div>
                <label for="file">Select file:</label>
                <asp:FileUpload ID="fileUpload" runat="server" />
            </div>
            <div>
                <label for="idFile">File ID:</label>
                <asp:TextBox ID="txtIdFile" runat="server"></asp:TextBox>
            </div>
            <div>
                <label for="flexFields">Flex Fields:</label>
                <asp:TextBox ID="txtFlexFields" runat="server"></asp:TextBox>
            </div>
            <div>
                <label for="connectionString">Connection String:</label>
                <asp:TextBox ID="txtConnectionString" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
            </div>
        </div>
    </form>
    //<%--</asp:PlaceHolder>--%>
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
