<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="ForgotPassword.aspx.cs" Inherits="SG_Constancia_TSC.ForgotPassword" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">

    <style>
        .btn {
            -webkit-border-radius: 35px;
            -moz-border-radius: 35px;
            border-radius: 35px;
        }
    </style>

    <div style="background-color: white; width: 495px; box-shadow: 0 0 15px 10px #E1E1E1; border: 2px solid #E1E1E1; border-radius: 20px; padding: 15px; position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); background-color: white;">

        <div style="text-align: center">
            <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" ImageUrl="~/Content/TSC/LOGO_TSC_2025.png"></dx:ASPxImage>
        </div>

        <h4 style="color: darkslategray; text-align: center">Contraseña Olvidada</h4>
        <br />
        <asp:PlaceHolder ID="loginForm" runat="server">

            <h6>Ingrese su correo electrónico registrado para restablecer su contraseña de forma segura.</h6>
            <br />
            <p style="color: red">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
            <dx:ASPxTextBox runat="server" ID="Email" Caption="Correo electrónico" NullText="Correo electrónico" Width="280px">
                <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom">
                    <RequiredField IsRequired="true" ErrorText="El campo de correo electrónico es requerido" />
                </ValidationSettings>
            </dx:ASPxTextBox>
            <br />
            <div style="text-align: center;">
                <dx:ASPxButton ID="btnSubmit" runat="server" Text="Enviar a correo electrónico" CssClass="btn" OnClick="Forgot" />
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="DisplayEmail" Visible="false">
            <p style="text-align: center">Por favor revise su correo electrónico para restablecer su contraseña.</p>
        </asp:PlaceHolder>

    </div>

</asp:Content>
