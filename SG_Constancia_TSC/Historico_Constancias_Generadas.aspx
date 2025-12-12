<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Historico_Constancias_Generadas.aspx.cs" Inherits="SG_Constancia_TSC.Historico_Constancias_Generadas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

     <style>
        .swal2-container {
            z-index: 20000 !important;
        }

        .dx-popup {
            z-index: 10000 !important;
        }
    </style>


</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">
    <asp:Panel ID="Panel_Content" runat="server">
        <link href="Content/css/style.css" rel="stylesheet" />
        <script src="Content/Seguimiento.js"></script>
        <script src="/Content/Sweetalert/js/sweetalert2@11.js"></script>

        <script type="text/javascript">
            if (window.history.replaceState) {
                window.history.replaceState(null, null, window.location.href);
            }

            function ShowPopup() {
                var grid = ASPxClientControl.GetControlCollection().GetByName('GV_PreUsuarios_H');
                if (grid) {
                    var selectedRowKeys = grid.GetSelectedKeysOnPage();
                    if (selectedRowKeys.length > 0) {
                        popupUpdateStatus_Historico.Show();
                    } else {
                        Swal.fire({
                            title: "¡Alerta!",
                            text: "Por favor seleccione una fila para guardar la constancia.",
                            icon: "warning",
                            confirmButtonColor: "#1F497D"
                        });

                    }
                } else {
                    alert('Grid not found.');
                }
            }

            function closePopupAndClearFields() {
                // Cerrar el popup
                popupUpdateStatus_Historico.Hide();

                // Limpiar el ComboBox y el Memo
                var cmbStatus_Historico = ASPxClientControl.GetControlCollection().GetByName('cmbStatus_Historico');
                var txtObs = ASPxClientControl.GetControlCollection().GetByName('txtObs');

                if (cmbStatus_Historico) {
                    cmbStatus_Historico.SetSelectedIndex(-1); // Deseleccionar cualquier valor
                    cmbStatus_Historico.SetValue(null); // Limpiar valor seleccionado
                }

                if (txtObs) {
                    txtObs.SetText(''); // Limpiar texto
                }
            }

            function OnRowFocused(s, e) {
                var rowIndex = s.GetFocusedRowIndex();
                if (rowIndex !== -1) {
                    var key = s.GetRowKey(rowIndex);

                }
            }
            function btnPopupUpdate_Click(s, e) {


                var campos = [
                    cmbStatus_Historico.GetText(),
                    txtObs.GetText()
                ];

                var camposVacios = campos.some(function (valor) {
                    return valor === '' || valor === null;
                });

                if (camposVacios) {
                    Swal.fire({
                        title: "¡Alerta!",
                        text: "Debe completar los campos Obligatorios",
                        icon: "warning",
                        confirmButtonColor: "#1F497D",
                    }).then(function () {
                        popupUpdateStatus_Historico.Show();  // Asegurar que el popup permanezca visible
                    });
                } else {
                    ASPxCallback_PopupUpdate_Historico.PerformCallback();
                    GV_PreUsuarios_H.PerformCallback();
                    popupUpdateStatus_Histoico.Hide();
                }
            }


            function SetCampos() {
                cmbStatus_Historico.SetText('');

            }


            function OnFocusedRowChanged(s, e) {
                var index = s.GetFocusedRowIndex();
                if (index >= 0) {
                    s.GetRowValues(index, 'Id', function (value) {
                        // Llama al servidor para recargar el reporte
                        ASPxCallbackPanel_Report_Historico.PerformCallback(value.toString());
                    });
                }
            }

            /*  FUNCIONES PARA ACTIVAR Y DESACTIVAR EL BOTÓN DE CERRAR LA CONSTANCIA*/
            function onReportCallbackComplete() {
                // Se completó el callback, mostraremos el iframe si ya se le puso src en el servidor
                var iframe = document.getElementById('<%= iframePDF_H.ClientID %>');
                var btn = document.getElementById('btnCerrarConstancia');
                if (iframe) {
                    // Si tiene src válido, lo mostramos y activamos el botón
                    if (iframe.src && !iframe.src.endsWith('about:blank')) {
                        iframe.style.display = 'block';
                        if (btn) btn.style.display = 'inline-block';
                    }
                }
            }

            function cerrarConstancia() {
                var iframe = document.getElementById('<%= iframePDF_H.ClientID %>');
                var btn = document.getElementById('btnCerrarConstancia');
                if (iframe) {
                    iframe.style.display = 'none';
                    iframe.src = 'about:blank';
                }
                if (btn) {
                    btn.style.display = 'none';
                }
            }
        </script>
        <div>
            <br />
            <h1 style="color: #006699; background-color: #FFFFFF; text-align: center; font-size: 30px;">
                <dx:ASPxImage ID="ASPxImage_Historico" runat="server" ShowLoadingImage="true">
                    <EmptyImage IconID="">
                    </EmptyImage>
                </dx:ASPxImage>
                &nbsp;HISTORICO DE CONSTANCIAS GENERADAS </h1>
        </div>

        <asp:ScriptManager ID="ScriptManager_Historico" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <contenttemplate>
            <div class="toolbar-filter">
                <span class="filter-label">Tipo de Solicitud:</span>
                <dx:ASPxComboBox ID="cmbTipoFiltro_Historico" runat="server"
                    Width="180px" AutoPostBack="True"
                    OnSelectedIndexChanged="cmbTipoFiltro_SelectedIndexChanged_Historico">
                    <Items>
                        <dx:ListEditItem Text="Natural" Value="0" />
                        <dx:ListEditItem Text="Jurídica" Value="1" />
                    </Items>
                </dx:ASPxComboBox>

                <div class="toolbar-filter" style="margin-top: 10px;">
                    <span class="filter-label">Mes:</span>
                    <dx:ASPxComboBox ID="cmbMes" runat="server" Width="140px" ClientInstanceName="cmbMes"
                        DropDownStyle="DropDownList" ValueType="System.Int32">
                        <Items>
                            <dx:ListEditItem Text="Enero" Value="1" />
                            <dx:ListEditItem Text="Febrero" Value="2" />
                            <dx:ListEditItem Text="Marzo" Value="3" />
                            <dx:ListEditItem Text="Abril" Value="4" />
                            <dx:ListEditItem Text="Mayo" Value="5" />
                            <dx:ListEditItem Text="Junio" Value="6" />
                            <dx:ListEditItem Text="Julio" Value="7" />
                            <dx:ListEditItem Text="Agosto" Value="8" />
                            <dx:ListEditItem Text="Septiembre" Value="9" />
                            <dx:ListEditItem Text="Octubre" Value="10" />
                            <dx:ListEditItem Text="Noviembre" Value="11" />
                            <dx:ListEditItem Text="Diciembre" Value="12" />
                        </Items>
                    </dx:ASPxComboBox>

                    <span class="filter-label" style="margin-left: 10px;">Año:</span>

                    <dx:ASPxComboBox ID="cmbAnio" runat="server" Width="100px"
                        ClientInstanceName="cmbAnio"
                        DropDownStyle="DropDownList"
                        ValueType="System.Int32"
                        DataSourceID="dsAnios"
                        TextField="Anio"
                        ValueField="Anio"
                        OnDataBound="cmbAnio_DataBound">
                    </dx:ASPxComboBox>
                    <dx:ASPxButton ID="btnBuscarMesAnio" runat="server" Text="Buscar" AutoPostBack="true"
                        OnClick="btnBuscarMesAnio_Click" CssClass="btn-neutro" Style="margin-left: 10px;" />
                </div>
            </div>

            <dx:ASPxGridView ID="GV_PreUsuarios_H" runat="server" AutoGenerateColumns="False" Style="margin-left: auto; margin-right: auto;"
                KeyFieldName="Id" DataSourceID="SqlDataUsers_Historico" OnBeforeExport="GV_PreUsuarios_BeforeExport_Historico" ClientInstanceName="GV_PreUsuarios"
                OnCustomCallback="GV_PreUsuarios_CustomCallback_Historico"
                Width="100%" CssClass="responsive-grid">
                <ClientSideEvents FocusedRowChanged="OnFocusedRowChanged" />
            
                <SettingsPager AlwaysShowPager="True" PageSize="10">
                    <PageSizeItemSettings Items="10, 30, 50" ShowAllItem="True" Visible="True">
                    </PageSizeItemSettings>
                </SettingsPager>
                <Settings ShowFilterRow="True" ShowHeaderFilterButton="False" ShowFooter="True" />
                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" FileName="Constancias_Generadas" />
                <SettingsPopup>
                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                </SettingsPopup>
                <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
                <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="False" AllowSort="False" />

                <Toolbars>
                    <dx:GridViewToolbar>
                        <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                        <Items>
                            <dx:GridViewToolbarItem Command="Custom">
                                <Template>
                                </Template>
                            </dx:GridViewToolbarItem>
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="Exportar a Excel">
                                <ItemStyle CssClass="btn-neutro" />

                            </dx:GridViewToolbarItem>

                        </Items>
                    </dx:GridViewToolbar>
                </Toolbars>


                <SettingsSearchPanel Visible="True" />
                <Columns>

                    <dx:GridViewCommandColumn
                        ShowClearFilterButton="True"
                        VisibleIndex="0" Caption="✔">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" Font-Size="Small" />
                        <CellStyle HorizontalAlign="Center" Font-Size="Small" />
                    </dx:GridViewCommandColumn>

                    <dx:GridViewDataTextColumn FieldName="FirstName" Name="FirstName" Visible="true" ShowInCustomizationForm="True" VisibleIndex="3" Caption="NOMBRE">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small" />
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
                        <CellStyle HorizontalAlign="Center" Font-Size="Small" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="LastName" Name="LastName" Visible="true" ShowInCustomizationForm="True" VisibleIndex="4" Caption="APELLIDO">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small" />
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
                        <CellStyle HorizontalAlign="Center" Font-Size="Small" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Identidad" Name="Identidad" Visible="true" ShowInCustomizationForm="True" VisibleIndex="2" Caption="DNI">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small" />
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
                        <CellStyle HorizontalAlign="Center" Font-Size="Small" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="Email" ShowInCustomizationForm="True" VisibleIndex="5" Caption="CORREO ELECTRÓNICO">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small"></Style>
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center" Font-Size="Small"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="Phone" ShowInCustomizationForm="True" VisibleIndex="7" Caption="CELULAR/TÉLEFONO">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small"></Style>
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center" Font-Size="Small"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="IdRole" VisibleIndex="1" Visible="False" />
                    <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="13" Visible="False" />

                    <dx:GridViewDataTextColumn Caption="RTN" FieldName="NumRtn" Name="NumRtn" Visible="true" VisibleIndex="2">

                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="EMPRESA/INSTITUCIÓN" Name="NomInstitucion" FieldName="NomInstitucion"
                        VisibleIndex="5" Visible="true">

                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="Descripcion_Estado" Caption="ESTADO" VisibleIndex="11">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small" />
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
                        <CellStyle HorizontalAlign="Center" Font-Size="Small" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataDateColumn Caption="FECHA SOLICITUD" FieldName="FechaIngreso" Name="FechaIngreso" VisibleIndex="12">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />

                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataDateColumn FieldName="FechaTermino" Name="FechaTermino" Visible="true" Caption="FECHA TERMINO" VisibleIndex="13">
                        <PropertiesDateEdit>
                            <Style Font-Size="Small">
                             </Style>
                        </PropertiesDateEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center" Font-Size="Small"></CellStyle>

                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn Caption="OBSERVACIÓN" FieldName="Observaciones" VisibleIndex="15">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Styles>
                    <SelectedRow BackColor="#CCCCCC" ForeColor="#003366" Font-Bold="True" />
                    <FocusedRow BackColor="#CCCCCC" ForeColor="#003366" Font-Bold="True" />
                </Styles>
            </dx:ASPxGridView>
            <dx:ASPxPopupControl ID="PopupArchivo_Historico" runat="server" ShowHeader="True"
                HeaderText="Vista Previa de Archivo" Width="800px" Height="600px"
                Modal="True" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ClientInstanceName="PopupArchivo_Historico">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">

                        <!-- Visor de PDF con iframe -->
                        <iframe id="pdfViewer" style="width: 100%; height: 100%; border: none;" hidden></iframe>

                        <!-- Visor de Imágenes -->
                        <dx:ASPxImage ID="ImgPreview_Historico" runat="server" Width="100%" Height="100%"
                            ClientInstanceName="ImgPreview_Historico"
                            ImageUrl='<%# String.Format("~/FileDownload.ashx?Upload_Id={0}", Eval("Upload_Id")) %>' />

                        <!-- Mensaje si no hay vista previa -->
                        <dx:ASPxLabel ID="LblNoPreview_Historico" runat="server" Text="No hay vista previa disponible"
                            ClientInstanceName="lblNoPreview_Historico" Visible="false" />
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </contenttemplate>

  <asp:SqlDataSource ID="SqlDataUsers_Historico" runat="server"
    ConnectionString="<%$ ConnectionStrings:connString %>"
    SelectCommand="
        SELECT DISTINCT
            Id, Identidad, FirstName, LastName, Email, NumRtn, NomInstitucion, TipoSolicitud,
            Descripcion_Estado, FechaIngreso, FechaTermino, FechaAprobacion, Id_Estado, Phone, IdRole, Observaciones
        FROM V_Solicitudes
        WHERE Id_Estado = 6
          AND TipoSolicitud = @TipoSolicitud
          AND (
               @Mes IS NULL OR @Mes = 0 
               OR @Anio IS NULL OR @Anio = 0
               OR (
                   MONTH(FechaIngreso) = @Mes
                   AND YEAR(FechaIngreso) = @Anio
                  )
              )
        ORDER BY Id DESC">
    <SelectParameters>
        <asp:Parameter Name="TipoSolicitud" Type="Boolean" DefaultValue="false" />
        <asp:ControlParameter Name="Mes"  ControlID="cmbMes"  PropertyName="Value" Type="Int32" ConvertEmptyStringToNull="true" />
        <asp:ControlParameter Name="Anio" ControlID="cmbAnio" PropertyName="Value" Type="Int32" ConvertEmptyStringToNull="true" />
    </SelectParameters>
</asp:SqlDataSource>


    <asp:SqlDataSource ID="dsAnios" runat="server"
    ConnectionString="<%$ ConnectionStrings:connString %>"
    SelectCommand="
        SELECT DISTINCT YEAR(FechaIngreso) AS Anio
        FROM V_Solicitudes
        WHERE Id_Estado = 6
        ORDER BY Anio DESC">
</asp:SqlDataSource>

        <dx:ASPxPopupControl ID="popupUpdateStatus_Historico" runat="server" ClientInstanceName="popupUpdateStatus_Historico"
            ShowCloseButton="True" HeaderText="Actualizar Estado"
            Width="400px" Height="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" SettingsAdaptivity-Mode="OnWindowInnerWidth" Modal="True" CloseAnimationType="None" AllowResize="False" AutoUpdatePosition="False" SettingsAdaptivity-VerticalAlign="WindowCenter">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <div style="display: flex; align-items: center; justify-content: center; height: 100%;">
                        <div style="text-align: center;">
                            <dx:ASPxComboBox ID="cmbStatus_Historico" runat="server" ValueField="Id_Estado" TextField="Descripcion_Estado" ClientInstanceName="cmbStatus_Historico"
                                Width="300px" Height="30px" ValueType="System.Int32" Caption="Seleccionar el Estado"
                                NullText="Seleccionar Estado" CaptionSettings-Position="Top" ClearButton-DisplayMode="Always" CaptionSettings-RequiredMarkDisplayMode="Required">
                                <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" RequiredField-IsRequired="true"
                                    SetFocusOnError="True">
                                    <RequiredField ErrorText="El Estado es requerido." IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                            <br />
                            <br />
                            <dx:ASPxButton ID="btnPopupUpdate_Historico" runat="server" Text="Actualizar" AutoPostBack="False" UseSubmitBehavior="false" ClientInstanceName="btnPopupUpdate">
                                <ClientSideEvents Click="btnPopupUpdate_Click" />
                            </dx:ASPxButton>
                            <dx:ASPxCallback ID="ASPxCallback_PopupUpdate_Historico" runat="server" ClientInstanceName="ASPxCallback_PopupUpdate_Historico" OnCallback="ASPxCallback_PopupUpdate_Callback_Historico">
                            </dx:ASPxCallback>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <dx:ASPxPageControl ID="ASPxPageControl_Historico" runat="server" ActiveTabIndex="0" Width="100%">
            <TabPages>

                <dx:TabPage Name="Estadistica" Text=" Constancia Final" Visible="true">
                  
                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <dx:PanelContent runat="server">
                                <div style="position: relative; margin-bottom: 16px; padding-top: 8px;">
                                    <button
                                        class="btn-neutro"
                                        type="button"
                                        id="btnCerrarConstancia"
                                        style="display: none; position: absolute; right: 0; top: -8px; padding: 6px 12px; cursor: pointer; z-index: 10;"
                                        onclick="cerrarConstancia();">
                                        Cerrar Constancia 
                                    </button>
                                </div>

                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel_Report_Historico" runat="server" Width="100%" OnCallback="ASPxCallbackPanel_Report_Callback_Historico" ClientInstanceName="ASPxCallbackPanel_Report_Historico">
                                    <ClientSideEvents EndCallback="function(s,e){ onReportCallbackComplete(); }" />
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">
                                            <iframe id="iframePDF_H" runat="server" width="100%" height="800px" visible="false"></iframe>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </dx:PanelContent>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
        </dx:ASPxPageControl>

    </asp:Panel>

</asp:Content>


