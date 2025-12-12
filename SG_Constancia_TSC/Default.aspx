<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master"
    CodeBehind="Default.aspx.cs" Inherits="SG_Constancia_TSC._Default" %>

<%@ Register Assembly="DevExpress.Web.v21.2" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.Bootstrap.v21.2" Namespace="DevExpress.Web.Bootstrap" TagPrefix="dx" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="Head" runat="server">

    <style>
        .wrap {
            max-width: 1100px;
            margin: 12px auto;
            padding: 8px
        }

        .card {
            border: 1px solid #e3e3e3;
            border-radius: 10px;
            padding: 10px
        }

        h3 {
            margin: 10px 0 20px;
            font-family: Arial
        }

        .filters .field-tipotipo {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        .card {
            overflow: visible;
        }


        .dxbs-edit .dxbs-ddm,
        .dxbs-dropdown-menu {
            min-width: 100% !important;
            box-sizing: border-box;
        }


        .dxbs-combobox .input-group {
            position: relative;
            display: table;
            border-collapse: separate;
            width: 100%;
        }

        .dxbs-combobox .form-control {
            display: table-cell;
            float: none;
            width: 100%;
            height: 34px;
            line-height: 1.42857143;
            padding-right: 2.25rem;
            box-sizing: border-box;
        }


        .dxbs-combobox .input-group-btn {
            position: relative;
            display: table-cell;
            width: 1%;
            white-space: nowrap;
            vertical-align: middle;
        }

            /* Botón/caret alineado y con la misma altura del input */
            .dxbs-combobox .input-group-btn > .btn {
                height: 34px;
                line-height: 1.42857143;
                border-left: 0;
                box-sizing: border-box;
            }

        .dxbs-combobox .dxbs-ddb {
            height: 34px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
        }


        .dxbs-dropdown-menu,
        .dxbs-edit .dxbs-ddm {
            min-width: 100% !important;
            box-sizing: border-box;
        }


        .filters {
            display: flex;
            flex-wrap: wrap;
            gap: 16px 24px;
            align-items: flex-end;
        }


        .filter-field {
            display: flex;
            flex-direction: column;
            gap: 6px;
            width: 260px;
            max-width: 100%;
        }

        .filter-label {
            font-weight: 600;
        }


        .filter-field .editor-wrap {
            width: 100%;
        }


        .filter-field .dxbs-dropdown-menu,
        .filter-field .dxbs-edit .dxbs-ddm {
            min-width: 100% !important;
            box-sizing: border-box;
        }


        .filter-field .dxeButtonEditSys,
        .filter-field .dxeEditAreaSys,
        .filter-field .dxic {
            width: 100% !important;
            box-sizing: border-box;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="Content" runat="server">

    <p class="auto-style1" style="text-align: center; color: #1F497D">&nbsp;</p>
    <h1 class="auto-style1" style="text-align: center; color: #1F497D">SISTEMA DE SOLICITUD DE CONSTANCIAS EN LÍNEA</h1>
    <p class="auto-style1" style="text-align: center; color: #1F497D">&nbsp;</p>
    <div class="wrap">

        <div class="filters">

            <div class="filter-field">
                <asp:Label ID="lblTipo" runat="server"
                    CssClass="filter-label"
                    AssociatedControlID="cmbTipo"
                    Text="Tipo de solicitud" />
                <div class="editor-wrap">
                    <dx:BootstrapComboBox ID="cmbTipo" runat="server"
                        Width="100%" NullText="Seleccione…"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="cmbTipo_SelectedIndexChanged"
                        ValueType="System.Int32"
                        ClientInstanceName="cmbTipo">
                        <Items>
                            <dx:BootstrapListEditItem Text="Natural" Value="0" />
                            <dx:BootstrapListEditItem Text="Jurídica" Value="1" />
                        </Items>
                    </dx:BootstrapComboBox>
                </div>
            </div>

            <div class="filter-field">
                <asp:Label ID="lblDesde" runat="server"
                    CssClass="filter-label"
                    AssociatedControlID="cmbFecInicio"
                    Text="Fecha Inicio" />
                <div class="editor-wrap">
                    <dx:BootstrapDateEdit ID="cmbFecInicio" runat="server"
                        Width="100%"
                        NullText="Seleccione una fecha"
                        EditFormat="Date"
                        DisplayFormatString="dd/MM/yyyy"
                        AutoPostBack="true"
                        OnDateChanged="Fechas_Changed">
                    </dx:BootstrapDateEdit>
                </div>
            </div>
            <div class="filter-field">
                <asp:Label ID="lblHasta" runat="server"
                    CssClass="filter-label"
                    AssociatedControlID="cmbFecFin"
                    Text="Fecha Fin" />
                <div class="editor-wrap">
                    <dx:BootstrapDateEdit ID="cmbFecFin" runat="server"
                        Width="100%"
                        NullText="Seleccione una fecha"
                        EditFormat="Date"
                        DisplayFormatString="dd/MM/yyyy"
                        AutoPostBack="true"
                        OnDateChanged="Fechas_Changed">
                    </dx:BootstrapDateEdit>
                </div>
            </div>
        </div>
        <br />
        <br />

        <div class="card">
            <h3>
                <asp:Literal ID="litTitulo" runat="server" Text="Estado de las Solicitudes" /></h3>

            <dx:BootstrapPieChart ID="ChartEstado" runat="server" DataSourceID="DsEstadosPorTipo" Palette="SoftPastel">
                <SettingsLegend HorizontalAlignment="Right" VerticalAlignment="Top" />
                <SettingsToolTip Enabled="true" />
                <SeriesCollection>

                    <dx:BootstrapPieChartSeries ArgumentField="EstadoTexto" ValueField="Cantidad">
                    </dx:BootstrapPieChartSeries>
                </SeriesCollection>
            </dx:BootstrapPieChart>

            <asp:SqlDataSource ID="DsEstadosPorTipo" runat="server"
                ConnectionString="<%$ ConnectionStrings:connString %>"
                CancelSelectOnNullParameter="false"
                SelectCommand="SELECT e.Descripcion_Estado AS Estado, COUNT(s.Id) AS Cantidad, (e.Descripcion_Estado + ': ' + CONVERT(varchar(12), COUNT(s.Id))) AS EstadoTexto FROM dbo.Estados e LEFT JOIN dbo.Solicitudes s ON s.Estado = e.Id_Estado AND s.TipoSolicitud = @Tipo AND (@Desde IS NULL OR CONVERT(date, s.CreatedDate) >= @Desde) AND (@Hasta IS NULL OR CONVERT(date, s.CreatedDate) <= @Hasta) GROUP BY e.Descripcion_Estado, e.Id_Estado ORDER BY e.Id_Estado;">
                <SelectParameters>
                    <asp:ControlParameter Name="Tipo" ControlID="cmbTipo" PropertyName="Value"
                        Type="Int32" DefaultValue="0" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter Name="Desde" ControlID="cmbFecInicio" PropertyName="Value"
                        Type="DateTime" ConvertEmptyStringToNull="true" />
                    <asp:ControlParameter Name="Hasta" ControlID="cmbFecFin" PropertyName="Value"
                        Type="DateTime" ConvertEmptyStringToNull="true" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>

</asp:Content>


