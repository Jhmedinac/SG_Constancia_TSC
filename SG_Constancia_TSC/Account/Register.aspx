<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Register.aspx.cs" Inherits="SG_Constancia_TSC.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="../Content/SignInRegister%20-%20Copia.css" rel="stylesheet" />
    <%--<link rel="stylesheet" type="text/css" href='<%# ResolveUrl("~/Content/SignInRegister - Copia.css") %>' />--%>
    <style>

        .formLayout-container {
            background-color: white;
            width: 395px;
            box-shadow: 0 0 15px 10px #E1E1E1;
            border: 2px solid #E1E1E1;
            border-radius: 20px;
            padding: 15px;
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 395px;
        }
    </style>
  <%--  <script type="text/javascript" src='<%# ResolveUrl("~/Content/SignInRegister - Copia.js") %>'></script>--%>
    <script src="../Content/SignInRegister%20-%20Copia.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageContent" runat="server">
    <asp:Panel ID="Panel_Content" runat="server">
   <div class="formLayout-verticalAlign">
        <div class="formLayout-container">
            <br />
            <div style="text-align:center" >
            <dx:ASPxImage ID="ASPxImage1"  runat="server" ShowLoadingImage="true"  ImageUrl="~/Content/TSC/LOGO_TSC_2024V1.png" ></dx:ASPxImage>    
         </div>
            <div style="text-align:center" >
         </div> 
            <dx:ASPxFormLayout runat="server" ID="FormLayout" ClientInstanceName="formLayout" CssClass="formLayout" UseDefaultPaddings="false">
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
                    <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Paddings-Padding="16">
<Paddings Padding="16px"></Paddings>
                        <Items>
                           <dx:LayoutItem ShowCaption="False" ColSpan="1" HorizontalAlign="Center"><LayoutItemNestedControlCollection>
                            <dx:LayoutItemNestedControlContainer runat="server">
                            <table>
                             <tr>
                               <td style="font-size:18px;">
                                <strong> Registrar Nuevo Usuario </strong>
                               </td>
                             </tr>
                            </table>
                        </dx:LayoutItemNestedControlContainer>
                    </LayoutItemNestedControlCollection>
                  </dx:LayoutItem>
                            <dx:LayoutItem Caption="Usuario">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="RegisterUserNameTextBox" runat="server" Width="100%" CssClass="Texbox" NullText="Usuario">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="Usuario es Requerido" />
                                            </ValidationSettings>
                                            <ClientSideEvents Init="function(s, e){ s.Focus(); }" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Nombre del Usuario" CssClass="Texbox">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="NameuserTextBox" runat="server" Width="100%" CssClass="Texbox" NullText="Nombre Completo del Usuario">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="Nombre de Usuario es Requerido" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                          

                  

                            <dx:LayoutItem Caption="Correo Eléctronico">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="EmailTextBox" runat="server" Width="100%" CssClass="Texbox" NullText="Correo Eléctronico">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                <RegularExpression ErrorText="Invalid e-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                <RequiredField IsRequired="true" ErrorText="Correo Elétronico es Requerido" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
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
                                       <asp:Literal runat="server" ID="ErrorMessage" />
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>

<Paddings Padding="0px"></Paddings>
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
                                        <dx:ASPxButton ID="RegisterButton" runat="server" Width="200" Text="Crear Usuario" CssClass="btn" OnClick="RegisterButton_Click"></dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>

<Paddings Padding="0px"></Paddings>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                </Items>
                <Items>

                </Items>
            </dx:ASPxFormLayout>
        </div>
    </div>
        </asp:Panel>
</asp:Content>
