<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeOutPage.aspx.cs" Inherits="PREDJL.TimeOutPage" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <script type = "text/javascript" >
        
         if (window.history.replaceState) {
             window.history.replaceState(null, null, window.location.href);
         }
         


         window.location.hash = "no-back-button";
         window.location.hash = "Again-No-back-button";//esta linea es necesaria para chrome
         window.onhashchange = function () { window.location.hash = "no-back-button"; }
</script>
</head>
   <body>
    <form id="form1" runat="server">
  
         <div style="width: 495px;
	border: 1px solid #E1E1E1;
	border-radius: 4px;
	padding: 15px;
	position: absolute;
	top: 35%;
	left: 50%;
	transform: translate(-50%, -50%);">
        <div style="text-align:center; height: 531px;">
        <asp:Image ID="Image1" runat="server" ImageUrl="/Content/TSC/LOGO_TSC_2025.png" Height="357px" Width="456px"/>
        <h2 style="color:#566573">
        &nbsp;&nbsp;&nbsp;&nbsp;
        Sistema de Solicitud de Constancias en Línea
        </h2>
    <h2 style="color:#566573">
        &nbsp;&nbsp;&nbsp;
        Su sesión ha expirado
        </h2>
    <p style=" color:#424949; height: 46px;"> 
    Por favor vuelve a <a href="Account/Login.aspx"> Iniciar Sesión </a>para acceder al Sistema.</p>
    <br />
              </div>
    </div>


    </form>
</body>
</html>
