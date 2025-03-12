<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seguimiento.aspx.cs" Inherits="SG_Constancia_TSC.Seguimiento" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seguimiento de Constancias</title>
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/custom.css" rel="stylesheet" />
    
    <style>
        /* Diseño mejorado */
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }
        
        #form1 {
            max-width: 600px;
            width: 100%;
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }

        #lblMensaje {
            margin-top: 10px;
            color: red;
        }

        /* Resaltar estado */
        .estado-pendiente { color: orange; font-weight: bold; }
        .estado-aprobado { color: green; font-weight: bold; }
        .estado-rechazado { color: red; font-weight: bold; }
        
        /* Estilos para botón de ver clave */
        .password-container {
            position: relative;
            width: 100%;
        }

        .password-container input {
            width: 100%;
            padding-right: 40px;
        }

        .toggle-password {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            cursor: pointer;
        }
    </style>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.0.0/jquery.min.js"></script>
    <script src="Content/js/bootstrap.min.js"></script>
    <script src="Content/Seguimiento.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblConstanciaId" runat="server" Text="Número de Constancia:" CssClass="form-label" />
            <asp:TextBox ID="txtConstanciaId" runat="server" CssClass="form-control" />

            <asp:Label ID="lblClave" runat="server" Text="Clave:" CssClass="form-label" />
            <div class="password-container">
                <asp:TextBox ID="txtClave" runat="server" TextMode="Password" CssClass="form-control" />
                <span class="toggle-password" onclick="toggleClave()">👁️</span>
            </div>

            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btn btn-primary" />
            <asp:Label ID="lblMensaje" runat="server" Text="" CssClass="form-text text-danger" />
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

<%--                        <asp:Button ID="btnDescargar" runat="server" Text="Descargar Constancia"
                            CssClass="btn btn-success" Visible="false" OnClick="btnDescargar_Click" />--%>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </form>

    <script type="text/javascript">
        function toggleClave() {
            var claveInput = document.getElementById('<%= txtClave.ClientID %>');
            if (claveInput.type === "password") {
                claveInput.type = "text";
            } else {
                claveInput.type = "password";
            }
        }
    </script>
</body>
</html>
