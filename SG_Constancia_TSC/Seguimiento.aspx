<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seguimiento.aspx.cs" Inherits="SG_Constancia_TSC.Seguimiento" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seguimiento de Constancias</title>
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/custom.css" rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="Content/js/bootstrap.min.js"></script>
    <script src="Content/Consulta.js"></script>
    <script src="Content/Seguimiento.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" />

    <style>
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
                <label for="txtConstanciaId"><i class="fa-solid fa-file-alt"></i> Número de Constancia:</label>
                <asp:TextBox ID="txtConstanciaId" runat="server" CssClass="form-control" placeholder="Ingrese el número" />
            </div>

            <div class="form-group">
                <label for="txtClave"><i class="fa-solid fa-key"></i> Clave:</label>
                <div class="input-group">
                    <asp:TextBox ID="txtClave" runat="server" TextMode="Password" CssClass="form-control" placeholder="Ingrese la clave" />
                    <div class="input-group-append">
                        <button class="btn btn-outline-secondary" type="button" onclick="toggleClave()">
                            <i class="fa-solid fa-eye"></i>
                        </button>
                    </div>
                </div>
            </div>

            <div class="text-center">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                CssClass="btn btn-primary btn-block" Enabled="true"
                OnClick="btnBuscar_Click" />
            </div>

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



             <div class="text-center">
                <asp:Label ID="lblMensaje" runat="server" Text="" CssClass="form-text text-danger" />
            </div>
        </div>
        <dx:ASPxPopupControl ID="popupSeguimiento" runat="server" ClientInstanceName="popupSeguimiento"
            AllowDragging="true" HeaderText="Estado de la Constancia" Modal="True"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            CloseOnEscape="true" EnableViewState="False" AutoUpdatePosition="true">
            <HeaderStyle CssClass="headerpopup" />
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <div id="popupContent">
                        <asp:Label ID="lblEstado" runat="server" CssClass="estado-pendiente" />
                        <asp:Label ID="lblFechaCreacion" runat="server" />
                        <asp:Label ID="lblObservaciones" runat="server" />
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

    </form>
</body>
</html>
