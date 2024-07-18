<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Seguimiento.aspx.cs" Inherits="SG_Constancia_TSC.Seguimiento" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudTSC1.aspx.cs" Inherits="TuProyecto.SolicitudTSC1" %>--%>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seguimiento de Constancias</title>
    <style>
        .popup {
            width: 600px !important;
            height: 400px !important;
        }
    </style>

        <script src="Content/Seguimiento.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblConstanciaId" runat="server" Text="Número de Constancia:" />
            <asp:TextBox ID="txtConstanciaId" runat="server" />
            <asp:Label ID="lblClave" runat="server" Text="Clave:" />
            <asp:TextBox ID="txtClave" runat="server" TextMode="Password" />
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            <asp:Label ID="lblMensaje" runat="server" Text="" ForeColor="Red" />
        </div>

        <dx:ASPxPopupControl ID="Relacionado" runat="server" ClientInstanceName="Relacionado" CssClass="popup"
            AllowDragging="true" HeaderText="Registro Solicitud" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseOnEscape="true" EnableViewState="False" AutoUpdatePosition="true"
            SettingsAdaptivity-Mode="OnWindowInnerWidth" CloseAnimationType="None" AllowResize="False" 
            SettingsAdaptivity-VerticalAlign="WindowCenter" PopupAnimationType="Fade">
            <HeaderStyle CssClass="headerpopup" />
            <ClientSideEvents Shown="function() { showConfirmationMessage1(); }" CloseUp="ClosePopupRelacionado" />
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <div id="popupContent"></div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </form>

    <script type="text/javascript">
        function showConfirmationMessage1() {
            // Aquí va tu lógica AJAX para obtener y mostrar los datos
        }
    </script>
</body>
</html>
