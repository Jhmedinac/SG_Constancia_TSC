<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Default.aspx.cs" Inherits="SG_Constancia_TSC._Default" %>


<%@ Register Assembly="DevExpress.XtraCharts.v21.2.Web, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web.Designer" TagPrefix="dxchartdesigner" %>

<%@ Register Assembly="DevExpress.XtraCharts.v21.2.Web, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dx" %>

<%@ Register TagPrefix="dx" Namespace="DevExpress.XtraCharts" Assembly="DevExpress.XtraCharts.v21.2, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<asp:Content runat="server" ContentPlaceHolderID="Head">
       <style>
        #chartContainer {
            width: 60%;
            height: 50%;
            margin: auto;
            text-align: center;
            display: flex;
            flex-direction: column;
            align-items: center;
            /*justify-content: center;*/
            height: 100vh;
        }

            .filters {
            margin-bottom: 20px;
        }
    </style>

</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">

    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://cdn3.devexpress.com/jslib/20.2.6/js/dx.all.js"></script>
    <link rel="stylesheet" href="https://cdn3.devexpress.com/jslib/20.2.6/css/dx.light.css">
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <div class="container" style="text-align: center; padding-top: 10px;">
       <%-- <h1 style="color: #1F497D;">SISTEMA DE PRE-REGISTRO DECLARACIÓN JURADA EN LÍNEA</h1>--%>

   <%--       <div class="filters">
        <label for="startDate">Fecha Inicio:</label>
        <dx:ASPxDateEdit ID="startDate" runat="server" ClientInstanceName="startDate"></dx:ASPxDateEdit>
        
        <label for="endDate">Fecha Final:</label>
        <dx:ASPxDateEdit ID="endDate" runat="server" ClientInstanceName="endDate"></dx:ASPxDateEdit>
        
        <label for="company">Compañía:</label>
        <dx:ASPxComboBox ID="company" runat="server" ValueType="System.String"></dx:ASPxComboBox>
        
        <button type="button" onclick="fetchAndRenderChart()">Actualizar Gráfico</button>
    </div>--%>
        <div id="chartContainer" >
            <canvas id="barChart"></canvas>
        </div>
    </div>

    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>

    <script type="text/javascript">
        function renderChart(data) {
            const estadoColors = {
                "Pre-Registrado": "gray",
                "Procesando": "#FAC21A",
                "Registrado": "green",
                "Requiere Documentación": "orangered",
                "Solicitud Rechazada": "red"
            };

            var ctx = document.getElementById('barChart').getContext('2d');
            var chart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: data.map(item => item.Descripcion_Estado),
                    datasets: [{
                        label: 'Cantidad',
                        data: data.map(item => item.Cantidad),
                        backgroundColor: data.map(item => estadoColors[item.Descripcion_Estado] || 'blue'),
                        borderColor: data.map(item => estadoColors[item.Descripcion_Estado] || 'blue'),
                        borderWidth: 1
                    }]
                },
                options: {
                    plugins: {
                        legend: {
                            position: 'right',
                            labels: {
                                usePointStyle: true
                            }
                        },
                        title: {
                            display: true,
                            text: 'Estado de Solicitudes',
                            font: {
                                size: 24  // Ajusta este valor para cambiar el tamaño del título
                            }
                        }
                    },
                    layout: {
                        padding: {
                            top: 20
                        }
                    }
                }
            });
        }

    </script>
   <%-- <script type="text/javascript">
        function renderChart(data) {
            const estadoColors = {
                "Pre-Registrado": "gray",
                "Procesando": "#FAC21A",
                "Registrado": "green",
                "Requiere Documentación": "orangered",
                "Solicitud Rechazada": "red"
            };

            // Calculate totals for each state
            const totals = data.reduce((acc, item) => {
                acc[item.Descripcion_Estado] = (acc[item.Descripcion_Estado] || 0) + item.Cantidad;
                return acc;
            }, {});

            var ctx = document.getElementById('barChart').getContext('2d');
            var chart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: data.map(item => item.Descripcion_Estado),
                    datasets: [{
                        label: 'Cantidad',
                        data: data.map(item => item.Cantidad),
                        backgroundColor: data.map(item => estadoColors[item.Descripcion_Estado] || 'blue'),
                        borderColor: data.map(item => estadoColors[item.Descripcion_Estado] || 'blue'),
                        borderWidth: 1
                    }]
                },
                options: {
                    plugins: {
                        legend: {
                            position: 'right',
                            labels: {
                                usePointStyle: true,
                                generateLabels: function (chart) {
                                    const labels = chart.data.labels;
                                    const dataset = chart.data.datasets[0];

                                    return labels.map((label, index) => ({
                                        text: `${label} (${totals[label]})`,
                                        fillStyle: dataset.backgroundColor[index],
                                        strokeStyle: dataset.borderColor[index],
                                        lineWidth: dataset.borderWidth,
                                        hidden: false,
                                        index: index
                                    }));
                                }
                            }
                        },
                        title: {
                            display: true,
                            text: 'Estado de Solicitudes',
                            font: {
                                size: 24  // Ajusta este valor para cambiar el tamaño del título
                            }
                        }
                    },
                    layout: {
                        padding: {
                            top: 20
                        }
                    }
                }
            });
        }
    </script>--%>

</asp:Content>