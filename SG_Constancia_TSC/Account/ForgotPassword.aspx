<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="ForgotPassword.aspx.cs" Inherits="SG_Constancia_TSC.ForgotPassword" %>
 
<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">   
 
      <style>
       .btn{
          -webkit-border-radius: 35px;
            -moz-border-radius: 35px;
          border-radius: 35px;
          }
      </style>
  

    <div style="text-align:center; color:darkslategray; font-family:sans-serif;">
        <br />
         <br />
    <h2 style="color:darkslategray">¿No recuerda su contraseña?</h2>
            <hr style="color:aquamarine" />
        </div>
        <br />
     <div style="width: 25%;  margin: 0px auto;">
     <asp:PlaceHolder id="loginForm" runat="server">
    <h6>Ingrese su correo electrónico</h6>
    <p style="color:red"><asp:Literal runat="server" ID="FailureText" /></p>
    <dx:ASPxTextBox runat="server" ID="Email" Caption="Correo electrónico"  NullText="Correo electrónico" Width="280px" >
        <ValidationSettings Display="Dynamic"  ErrorTextPosition="Bottom">
            <RequiredField IsRequired="true" ErrorText="El campo de correo electrónico es requerido" />
        </ValidationSettings>
    </dx:ASPxTextBox><br />
      <div style="text-align: center;">
         <dx:ASPxButton ID="btnSubmit" runat="server" Text="Enviar a correo electrónico" CssClass="btn"  OnClick="Forgot"  />
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="DisplayEmail" Visible="false">
    <p style="text-align:center">Por favor revise su correo electrónico para restablecer su contraseña.</p>
</asp:PlaceHolder>
            
   </div>
  
</asp:content>
