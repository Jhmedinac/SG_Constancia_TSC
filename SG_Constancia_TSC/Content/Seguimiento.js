function showConfirmationMessage1() {
    var constanciaId = '<%= txtConstanciaId.Text %>';

    $.ajax({
        type: "POST",
        url: "SolicitudTSC1.aspx/GetSessionValues",
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
                "<tr><td>Otros Datos:</td><td>" + otrosDatos + "</td></tr>" +
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