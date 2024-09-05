<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seguimiento.aspx.cs" Inherits="SG_Constancia_TSC.Seguimiento" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seguimiento de Constancias</title>
    <link href="Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/css/custom.css" rel="stylesheet" />
    <style>
        /* Estilos para el popup */
        .popup {
            width: 600px !important;
            height: 400px !important;
            background-color: #f9f9f9;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
        }

        .headerpopup {
            background-color: #007bff;
            color: white;
            padding: 10px;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
        }

        /* Estilos para el contenido del popup */
        #popupContent {
            padding: 20px;
        }

        /* Estilos para la página de seguimiento */
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
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

        #form1 div {
            margin-bottom: 10px;
        }

        #form1 label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        #form1 input[type="text"], 
        #form1 input[type="password"] {
            width: 100%;
            padding: 8px;
            box-sizing: border-box;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        #form1 button {
            width: 100%;
            padding: 10px;
            background-color: #007bff;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }

        #form1 button:hover {
            background-color: #0056b3;
        }

        #lblMensaje {
            margin-top: 10px;
            color: red;
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
            <asp:TextBox ID="txtClave" runat="server" TextMode="Password" CssClass="form-control" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" CssClass="btn btn-primary" />
            <asp:Label ID="lblMensaje" runat="server" Text="" CssClass="form-text text-danger" />
        </div>

        <dx:ASPxPopupControl ID="Relacionado1" runat="server" ClientInstanceName="Relacionado1" CssClass="popup"
            AllowDragging="true" HeaderText="Registro Solicitud" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseOnEscape="true" EnableViewState="False" AutoUpdatePosition="true"
            SettingsAdaptivity-Mode="OnWindowInnerWidth" CloseAnimationType="None" AllowResize="False" 
            SettingsAdaptivity-VerticalAlign="WindowCenter" PopupAnimationType="Fade">
            <HeaderStyle CssClass="headerpopup" />
            <ClientSideEvents Shown="function() { showConfirmationMessage2(); }" CloseUp="ClosePopupRelacionado1" />
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <div id="popupContent"></div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </form>

    <script type="text/javascript">
        function showConfirmationMessage2() {
            var constanciaId = '<%= txtConstanciaId.Text %>';

            $.ajax({
                type: "POST",
                url: "Seguimiento.aspx/GetSessionValues",
                data: JSON.stringify({ constanciaId: constanciaId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var values = response.d.split("|");
                    var constanciaId = values[0];
                    var estado = values[1];
                    var fechaCreacion = values[2];
                    var otrosDatos = values[3];

                    var tableHtml = "<table border='0' width='100%'>" +
                        "<tr><td colspan='2'>&nbsp;</td></tr>" +
                        "<tr><td colspan='2' align='center'><strong><font size='+2'>Estado de la Constancia</font></strong></td></tr>" +
                        "<tr><td colspan='2'>&nbsp;</td></tr>" +
                        "<tr><td>Número de Constancia:</td><td>" + constanciaId + "</td></tr>" +
                        "<tr><td>Estado:</td><td>" + estado + "</td></tr>" +
                        "<tr><td>Fecha de Creación:</td><td>" + fechaCreacion + "</td></tr>" +
                        "<tr><td>Observaciones:</td><td>" + otrosDatos + "</td></tr>" +
                        "<tr><td colspan='2'>&nbsp;</td></tr>" +
                        "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>" +
                        "</table>";

                    document.getElementById("popupContent").innerHTML = tableHtml;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console.log("Error al obtener valores de la constancia: " + thrownError);
                }
            });
        }
    </script>
</body>
</html>