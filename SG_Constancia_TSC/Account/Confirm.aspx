<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Confirm.aspx.cs" Inherits="SG_Constancia_TSC.Confirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="PageContent" runat="server">
    <div class="accountHeader" style="text-align:center; color:darkslategray; font-family:sans-serif;">
        <h2>Confirmación de la cuenta</h2>
    </div>
     <div class="accountHeader" style="text-align:center; color:black; font-family:sans-serif;">
    <asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true">
        <p>
            Gracias por confirmar su cuenta. Haga click <dx:ASPxHyperLink ID="login" runat="server" NavigateUrl="~/Account/Login.aspx" Text="Aquí" /> para iniciar sesión
        </p>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="errorPanel" ViewStateMode="Disabled" Visible="false">
        <p style="color:red">Se ha producido un error.</p>
    </asp:PlaceHolder>
         </div>
</asp:Content>