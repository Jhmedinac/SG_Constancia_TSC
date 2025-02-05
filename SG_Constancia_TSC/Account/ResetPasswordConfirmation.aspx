<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Light.master" CodeBehind="ResetPasswordConfirmation.aspx.cs" Inherits="SG_Constancia_TSC.ResetPasswordConfirmation" %>

<asp:content id="ClientArea" contentplaceholderid="MainContent" runat="server">
     
<div class="accountHeader" style="text-align:center; color:darkslategray; font-family:sans-serif;">
    <h2>Contraseña Cambiada</h2>
</div>
    <div class="accountHeader" style="text-align:center; color:black; font-family:sans-serif;">
<p >Se ha cambiado su contraseña. Haga Click
    <dx:ASPxHyperLink ID="login" runat="server" NavigateUrl="Login.aspx" Text="Aquí" /> para iniciar sesión </p>
</div>
</asp:content>