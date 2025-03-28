<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerificarConstancia.aspx.cs" Inherits="SG_Constancia_TSC.VerificarConstancia" %>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="utf-8" />
    <title>Verificación de Constancia</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css">
    <script src="https://www.google.com/recaptcha/api.js" async defer></script>


    <style>
        .container {
/*            max-width: 600px;
            margin-top: 50px;*/
        }

        .alert-success {
            background-color: #e8f5e9; /* Verde más suave */
            border: 1px solid #2e7d32; /* Verde oscuro */
            border-radius: 12px; /* Bordes más redondeados */
            padding: 15px;
            box-shadow: 3px 3px 10px rgba(0, 0, 0, 0.1);
        }

        h4 {
            display: flex;
            align-items: center;
            font-size: 20px;
            margin-bottom: 10px;
        }

        h4 img {
            width: 24px; 
            height: 24px;
            margin-right: 8px; /* Espacio entre el icono y el texto */
        }

        p {
            display: flex;
            align-items: center;
            font-size: 16px;
            margin: 5px 0;
        }

        p img {
            width: 20px;
            height: 20px;
            margin-right: 8px; /* Espacio entre el icono y el texto */
        }

        .text-bold {
            font-weight: bold;
        }

        body {
            font-family: Arial, sans-serif;
            background-color: #f8f9fa;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }
        .container {
            background-color: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
            max-width: 500px;
            width: 100%;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h3 class="text-center text-primary"><i class="fa-solid fa-magnifying-glass"></i> Seguimiento de Constancia</h3>
            <hr />

            <div class="form-group">
                <label for="txtCorreo"><i class="fa-solid fa-envelope"></i> Correo Electrónico:</label>
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Ingrese su correo" />
            </div>
            <div class="form-group">
                <label for="txtVerficacion"><i class="fa-solid fa-envelope"></i> Codigo Verificacion:</label>
                <asp:TextBox ID="txtVerficacion" runat="server" CssClass="form-control" placeholder="Codigo Verificacion" />
            </div>

            <div class="text-center">
                <asp:Button ID="btnEnviarCodigo" runat="server" Text="Enviar Código" OnClick="btnEnviarCodigo_Click" CssClass="btn btn-secondary btn-block" />
            </div>
        </div>
        <div class="container">
            <h2 class="text-center">Verificación de Constancia</h2>
            <div class="card">
                <div class="card-body">
                    <div class="form-group">
                        <label for="txtCodigo">Código de Verificación:</label>
                        <asp:TextBox ID="txtCodigo" CssClass="form-control" runat="server" />
                    </div>
                    <div class="text-center">
                        <asp:Button ID="btnVerificar" CssClass="btn btn-primary" runat="server" Text="Verificar" OnClick="btnVerificar_Click" />
                    </div>
                    <br />
                    <div>
                        <asp:Label ID="lblResultado" runat="server" CssClass="text-center d-block"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="text-center">
                <asp:Label ID="lblMensaje" runat="server" Text="" CssClass="form-text text-danger" />
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</body>
</html>
