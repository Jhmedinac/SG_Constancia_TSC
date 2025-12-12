

<%@ Page Title="" Language="C#" MasterPageFile="~/Light.master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="SG_Constancia_TSC.Account.ResetPassword" %>
<asp:Content ID="Content" ContentPlaceHolderID="Head" runat="server">
    <link href="../Content/SignInRegister%20-%20Copia.css" rel="stylesheet" />
    <link href="../Content/SignInRegister.css" rel="stylesheet" />
    <script src="../Content/SignInRegister.js"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div  style="       background-color: white;
                        width: 495px;
	                    box-shadow: 0 0 15px 10px #E1E1E1;
                        border: 2px solid #E1E1E1;
                        border-radius: 20px;
                        padding: 15px;
	                    position: absolute;
	                    top: 50%;
	                    left: 50%;
	                    transform: translate(-50%, -50%);
                        background-color: white;"">
                  <div style="text-align:center">
      <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" ImageUrl="~/Content/TSC/LOGO_TSC_2025.png"></dx:ASPxImage>
             </div>
    <div class="accountHeader" style="text-align:center; color:darkslategray; font-family:sans-serif;">

    <h3>Restablecer contraseña</h3>

<h4 style="color:#FA5858"><asp:Literal runat="server" ID="ErrorMessage" /></h4>

        </div>
      <div style="margin-right:auto;  margin-left: 69px;" >
      <dx:ASPxTextBox runat="server" ID="Email" Caption="Correo electrónico" Width="350px" Theme="iOS" CssClass="Texbox" NullText="Ingrese su Correo Electrónico">
    <CaptionSettings Position="Top" />
    <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
        <RequiredField IsRequired="true" ErrorText="El correo electrónico es requerido." />
    </ValidationSettings>
        </dx:ASPxTextBox>

                 <dx:ASPxButtonEdit ID="Password" Caption="Contraseña" runat="server" Width="350" Password="true" ClientInstanceName="Password" ClearButton-DisplayMode="Never" CssClass="Texbox" 
                     NullText="Ingrese su Contraseña" OnTextChanged="Password_TextChanged">
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
                    <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
                    <RequiredField IsRequired="true" ErrorText="La Contraseña es Requerida" />
                    </ValidationSettings>
                    <ClientSideEvents ButtonClick="onPasswordButtonEditButtonClick" />
                    </dx:ASPxButtonEdit>

                   <dx:ASPxButtonEdit ID="ConfirmPassword" Caption="Confirmar contraseña" runat="server" Width="350" Password="true"  ClientInstanceName="ConfirmPassword" ClearButton-DisplayMode="Never" CssClass="Texbox" NullText="Ingrese de Nuevo su Contraseña">
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
              <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="Text">
               <RequiredField IsRequired="true" ErrorText="La Contraseña es Requerida" />
                </ValidationSettings>
                    <ClientSideEvents ButtonClick="onPasswordButtonEditButtonClick" />
               <ClientSideEvents 

                   Validation="function(s, e) {
        var originalPasswd = Password.GetText();
        var currentPasswd = s.GetText();
        e.isValid = (originalPasswd  == currentPasswd );
        e.errorText = 'Las contraseñas no coinciden,intentelo de nuevo.';
    }"   
                   />

        </dx:ASPxButtonEdit>
       </div><br />
     <div style="text-align:center"> 
  <dx:ASPxButton runat="server" ID="Reset" Text="Restablecer" OnClick="Reset_Click" Height="34px" CssClass="btn">
     <PressedStyle>
        <BorderBottom BorderColor="#CCCCCC" />
    </PressedStyle>
    <HoverStyle BackColor="#069BFF" Border-BorderColor="#0066FF">
    </HoverStyle>
   </dx:ASPxButton>
    </div>
    </div>


</asp:Content>

