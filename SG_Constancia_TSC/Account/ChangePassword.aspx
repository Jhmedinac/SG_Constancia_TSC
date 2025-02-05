<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="ChangePassword.aspx.cs" Inherits="SG_Constancia_TSC.Account.ChangePassword" %>

<asp:Content ID="Content" ContentPlaceHolderID="Head" runat="server">
    <%--<link href="../Content/SignInRegister%20-%20Copia.css" rel="stylesheet" />--%>
    <link href="../Content/SignInRegister.css" rel="stylesheet" />
    <script src="../Content/SignInRegister.js"></script>
   
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <div style="background-color: white; width: 495px; box-shadow: 0 0 15px 10px #E1E1E1; border: 2px solid #E1E1E1; border-radius: 20px; padding: 15px; position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); background-color: white;">

        <div style="text-align: center">
            <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" ImageUrl="~/Content/TSC/LOGO_TSC_2024V1.png"></dx:ASPxImage>
        </div>
        <div class="accountHeader" style="text-align: center; color: darkslategray; font-family: sans-serif;">
            <br />
            <br />
            <h2 runat="server" id="PageHeader">Cambiar Contraseña</h2>
            <br />
            <h4 style="color: #FA5858">
                <asp:Literal runat="server" ID="ErrorMessage" />
            </h4>
        </div>
        <div style="margin-right: auto; margin-left: 69px;">
            <dx:ASPxButtonEdit ID="tbCurrentPassword" runat="server" Caption="Contraseña actual" NullText="Contraseña actual" Password="true" ClearButton-DisplayMode="Never" Width="350px" Theme="iOS"
                CssClass="Texbox" OnTextChanged="Password_TextChanged">
                <CaptionSettings Position="Top" />
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
                <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                    <RequiredField ErrorText="Se requiere la contraseña actual." IsRequired="true" />
                </ValidationSettings>
                <ClientSideEvents ButtonClick="onPasswordButtonEditButtonClick" />
            </dx:ASPxButtonEdit>

            <dx:ASPxButtonEdit ID="tbPassword" ClientInstanceName="Password" Caption="Nueva contraseña" NullText="Nueva contraseña" Password="true" ClearButton-DisplayMode="Never" 
                runat="server" CssClass="Texbox"
                Width="350px" OnTextChanged="Password_TextChanged" Theme="iOS">
                <CaptionSettings Position="Top" />
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
                <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                    <RequiredField ErrorText="Se requiere la contraseña nueva." IsRequired="true" />
                </ValidationSettings>
                <ClientSideEvents ButtonClick="onPasswordButtonEditButtonClick" />
            </dx:ASPxButtonEdit>

            <dx:ASPxButtonEdit ID="tbConfirmPassword" Password="true" Caption="Confirmar la nueva contraseña" NullText="Confirmar la nueva contraseña" runat="server" ClearButton-DisplayMode="Never"
                Width="350px" Theme="iOS" CssClass="Texbox">
                <CaptionSettings Position="Top" />
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
                <ValidationSettings ValidationGroup="ChangeUserPasswordValidationGroup" Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                    <RequiredField ErrorText="Se requiere confirmación de la contraseña nueva." IsRequired="true" />
                </ValidationSettings>
                 <ClientSideEvents ButtonClick="onPasswordButtonEditButtonClick" />
                <ClientSideEvents Validation="function(s, e) {
        var originalPasswd = Password.GetText();
        var currentPasswd = s.GetText();
        e.isValid = (originalPasswd  == currentPasswd );
        e.errorText = 'Las contraseñas no coinciden,intentelo de nuevo.';
    }" />
            </dx:ASPxButtonEdit>
            <br />
        </div>
        <div style="text-align: center">
            <dx:ASPxButton ID="btnChangePassword" runat="server" Text="Cambiar contraseña" ValidationGroup="ChangeUserPasswordValidationGroup" CssClass="btn"
                OnClick="btnChangePassword_Click" Theme="iOS" Height="34px">
                <ClientSideEvents Click="function(s, e){ 
          e.processOnServer = confirm('¿Esta seguro de cambiar su contraseña?');
            }" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnSetPassword" runat="server" Text="Set Password" Visible="false" ValidationGroup="ChangeUserPasswordValidationGroup" CssClass="btn"
                OnClick="btnSetPassword_Click">
                <PressedStyle>
                    <BorderBottom BorderColor="#CCCCCC" />
                </PressedStyle>
                <HoverStyle BackColor="#069BFF" Border-BorderColor="#0066FF">
                </HoverStyle>
            </dx:ASPxButton>
        </div>
        <br />
        <br />
    </div>
</asp:Content>
