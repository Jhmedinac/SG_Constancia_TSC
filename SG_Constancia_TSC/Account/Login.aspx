<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Login.aspx.cs" Inherits="SG_Constancia_TSC.SignInModule" Title="Login" %>


<asp:Content runat="server" ContentPlaceHolderID="Head">
    <link rel="stylesheet" href="../Content/css/responsive.css"/>
    <link href="../Content/SignInRegister.css" rel="stylesheet" />
     <style type="text/css">
         @media(max-width:824px) {
             .control-area {
                 max-width: 824px !important;
             }
         }

         .logo-container {
             text-align: center;
             margin-top: 10px;
             margin-bottom: 10px; /* <-- Asegura poco espacio debajo del logo */
         }

             .logo-container img {
                 max-width: 100%;
                 height: auto;
                 width: 500px; /* o el tamaño deseado */
             }

         .formLayout-container {
             padding-top: 10px !important; /* <-- Reduce espacio vertical */
         }
     </style>
     <%--<script src='https://www.google.com/recaptcha/api.js'></script> 
     <script src="https://www.google.com/recaptcha/api.js?onload=renderRecaptcha&render=explicit"></script>
     <script  src="https://code.jquery.com/jquery-3.2.1.min.js"></script> --%>
    <script src="../Content/SignInRegister.js"></script>
       
    <script type = "text/javascript">
        function disableBackButton() {
            window.history.forward();
        }
        setTimeout("disableBackButton()", 0);
    </script>

</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    <div class="formLayout-verticalAlign">
        <div class="formLayout-container">
           
            <div class="logo-container" style="margin-bottom:10px;" >
            <dx:ASPxImage ID="ASPxImage1"  runat="server" ShowLoadingImage="true"  ImageUrl="~/Content/TSC/LogoCons_transparente1.png"
                
              ></dx:ASPxImage>    
         </div>
            <div style="text-align:center" >
         </div> 
                        <dx:ASPxFormLayout runat="server" ID="FormLayout" ClientInstanceName="formLayout" UseDefaultPaddings="false">
                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" />
                            <SettingsItemCaptions Location="Top" />
                            <Styles LayoutGroup-Cell-Paddings-Padding="0" LayoutItem-Paddings-PaddingBottom="8" >
                            <LayoutItem>
                            <Paddings PaddingBottom="8px"></Paddings>
                            </LayoutItem>
                     <LayoutGroup>
                        <Cell>
                     <Paddings Padding="0px"></Paddings>
                             </Cell>
                    </LayoutGroup>
                            </Styles>
                            <Items>
                            <dx:LayoutItem ShowCaption="False" ColSpan="1" HorizontalAlign="Center">
                              <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer runat="server">
                                  <table  text-align: center;">
                                    <tr>
                                      <td >
                                        <strong></strong>
                                      </td>
                                    </tr>
                                  </table>
                                </dx:LayoutItemNestedControlContainer>
                              </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Paddings-Padding="16">
                                    <Paddings Padding="16px"></Paddings>
                                    <Items>
                                        <dx:LayoutItem Caption="Usuario" >
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxTextBox ID="UserNameTextBox" runat="server" Width="100%" NullText="Ingrese su Usuario" CssClass="Texbox">
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                            <RequiredField IsRequired="true" ErrorText="Usuario es Requerido" />
                                                        </ValidationSettings>
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Contraseña">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxButtonEdit ID="PasswordButtonEdit" runat="server" Width="100%" NullText="Ingrese su Contraseña" Password="true" ClearButton-DisplayMode="Never" CssClass="Texbox">
                                                      <ClearButton DisplayMode="Never"></ClearButton>
                                                        <ButtonStyle Border-BorderWidth="0" Width="6" CssClass="eye-button" HoverStyle-BackColor="Transparent" PressedStyle-BackColor="Transparent">
                                                        <PressedStyle BackColor="Transparent"></PressedStyle>
                                                        <HoverStyle BackColor="Transparent"></HoverStyle>
                                                        <Border BorderWidth="0px"></Border>
                                                        </ButtonStyle>
                                                        <ButtonTemplate>
                                                            <div></div>
                                                        </ButtonTemplate>
                                                        <Buttons>
                                                            <dx:EditButton>
                                                            </dx:EditButton>
                                                        </Buttons>
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                            <RequiredField IsRequired="true" ErrorText="Contraseña es Requerida" />
                                                        </ValidationSettings>
                                                        <ClientSideEvents ButtonClick="onPasswordButtonEditButtonClick" />
                                                    </dx:ASPxButtonEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem ShowCaption="False" Name="GeneralError">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <div id="GeneralErrorDiv" runat="server" class="formLayout-generalErrorText"></div>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                             <dx:LayoutItem Colspan="1" ShowCaption="False" HorizontalAlign="Center">
                                          <LayoutItemNestedControlCollection>
                                       <dx:LayoutItemNestedControlContainer>
                                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                        <h4 style="color:#FA5858; text-align:center">
                                            <asp:Literal runat="server" ID="FailureText" />
                                        </h4>
                                        </asp:PlaceHolder>
                                    </dx:LayoutItemNestedControlContainer>
                                         </LayoutItemNestedControlCollection>
                                                 </dx:LayoutItem>
                                      <dx:LayoutItem Colspan="1" ShowCaption="False" HorizontalAlign="Center">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                              <div style="width: 200px;margin:auto">
                               </div>
                               </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                                <dx:LayoutGroup GroupBoxDecoration="HeadingLine" ShowCaption="False">
                                    <Paddings PaddingTop="13" PaddingBottom="13" />
                                    <GroupBoxStyle CssClass="formLayout-groupBox" />
                                    <Items>
                                        <dx:LayoutItem ShowCaption="False" HorizontalAlign="Center" Paddings-Padding="0">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer>
                                                    <dx:ASPxButton ID="SignInButton" runat="server" Width="200" Text="Iniciar Sesión" CssClass="btn" OnClick="SignInButton_Click" UseSubmitBehavior="true">

                                                    </dx:ASPxButton>

                                                    <div style="text-align:center">     
                                                        <br />
                                                    <dx:ASPxHyperLink runat="server" NavigateUrl="~/Account/ForgotPassword.aspx" Text="¿Olvidó su contraseña?"  />
                                                    </div>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                           <Paddings Padding="0px"></Paddings>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>    
        </div>
    </div>
</asp:Content>