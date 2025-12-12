<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Light.master" CodeBehind="ResetPasswordConfirmation.aspx.cs" Inherits="SG_Constancia_TSC.ResetPasswordConfirmation" %>

<asp:content id="ClientArea" contentplaceholderid="MainContent" runat="server">
                 <br />
    <div style="text-align:center">
<dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" ImageUrl="~/Content/TSC/LOGO_TSC_2025.png"></dx:ASPxImage>
       </div>
<div class="accountHeader" style="text-align:center; color:darkslategray; font-family:sans-serif;">
    <br />
    <h2>Contraseña Cambiada</h2>
</div>
    <br />
    <div class="accountHeader" style="text-align:center; color:black; font-family:sans-serif;">
<p >Se ha cambiado su contraseña. Haga Click
    <dx:ASPxHyperLink ID="login" runat="server" NavigateUrl="Login.aspx" Text="Aquí" /> para iniciar sesión </p>
</div>
</asp:content>