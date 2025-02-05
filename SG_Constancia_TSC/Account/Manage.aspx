<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Manage.aspx.cs" Inherits="SG_Constancia_TSC.Account.Manage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="LeftPanelContent" runat="server">
</asp:Content>

<asp:content id="Content1" contentplaceholderid="Content" runat="server">
    <p style="color:blue; text-align:center"><%: SuccessMessage %></p>
    <div style="text-align:center; color:darkslategray; font-family:sans-serif;">
    <h2>Administrar cuenta</h2>
    <h4>Cambiar la contraseña de su cuenta</h4>
    <hr style="color:aquamarine" />
</div>
    <div class="accountHeader" style="text-align:center; color:black; font-family:sans-serif;">
    <ul>
    <li style="list-style:none">
        Contraseña:
 
        <dx:ASPxHyperLink NavigateUrl="~/Account/ChangePassword.aspx" Text="[Cambiar]" Visible="false" ID="ChangePassword" runat="server" />
        <dx:ASPxHyperLink NavigateUrl="~/Account/ChangePassword.aspx" Text="[Create]" Visible="false" ID="CreatePassword" runat="server" />
 
    </li>
<!-- <li>
        Inicios de sesión externos: <%: LoginsCount %> 
 
        <dx:ASPxHyperLink NavigateUrl="/Account/ManageLogins.aspx" Text="[Manage]" runat="server" />
 
    </li>-->
    <%-- 
        Phone Numbers can used as a second factor of verification in a two-factor authentication system.
        See <a href="http://go.microsoft.com/fwlink/?LinkId=403804">this article</a>
        for details on setting up this ASP.NET application to support two-factor authentication using SMS.
        Uncomment the following blocks after you have set up two-factor authentication
    --%> 
    <%-- 
    <li>
        Phone Number:
 
    <% if (HasPhoneNumber) { %> 
        <dx:ASPxHyperLink NavigateUrl="/Account/AddPhoneNumber.aspx" runat="server" Text="[Add]" />
    <% } else { %> 
        <dx:ASPxLabel Text="" ID="PhoneNumber" runat="server" />
        <dx:ASPxHyperLink NavigateUrl="/Account/AddPhoneNumber.aspx" runat="server" Text="[Change]" /> &nbsp;|&nbsp;
        <dx:ASPxButton runat="server" ID="RemovePhone" RenderMode="Link" Text="[Remove]" OnClick="RemovePhone_Click" />
    <% } %> 
 
    </li> 
    --%>
<!--   <li>
        Two-Factor Authentication:
        There are no two-factor authentication providers configured. See <a href="http://go.microsoft.com/fwlink/?LinkId=403804">this article</a>
        for details on setting up this ASP.NET application to support two-factor authentication.
 
        <% if (TwoFactorEnabled) { %> 
        <%--
        Enabled
        <dx:ASPxButton runat="server" id="TwoFactorDisable" RenderMode="Link" Text="[Disable]" CommandArgument="false" OnClick="TwoFactorDisable_Click" />
        --%>
        <% } else { %>          
        <%--
        Disabled
        <dx:ASPxButton runat="server" id="TwoFactorEnable" RenderMode="Link" Text="[Enable]" CommandArgument="true" OnClick="TwoFactorEnable_Click" />
        --%>
        <% } %> 
    </li>-->
</ul>
        </div>
</asp:Content>
