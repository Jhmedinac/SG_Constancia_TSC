<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Autorizar_Constancia.aspx.cs" Inherits="SG_Constancia_TSC.Autorizar_Constancia" %>

<%@ Register Assembly="DevExpress.XtraReports.v21.2.Web.WebForms, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style>
        .swal2-container {
            z-index: 20000 !important;
        }

        .dx-popup {
            z-index: 10000 !important;
        }

        .responsive-grid {
            max-width: 100%;
            overflow-x: auto;
        }

        .custom-grid-header {
            background-color: #1F497D;
            color: white;
            text-align: center;
            font-weight: bold;
        }

        .custom-grid-row:nth-child(odd) {
            background-color: #f8f8f8;
        }

        .custom-grid-selected-row {
            background-color: #d0e4f7;
            font-weight: bold;
        }

        .link-descargar {
            color: red;
            font-size: 16px;
            font-weight: bold;
            text-decoration: none;
        }

            .link-descargar:hover {
                color: darkred;
                text-decoration: underline;
            }

        .link-ver {
            color: blue;
            font-size: 16px;
            font-weight: bold;
            text-decoration: none;
        }

            .link-ver:hover {
                color: darkblue;
                text-decoration: underline;
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
                var grid = ASPxClientControl.GetControlCollection().GetByName('GV_PreUsuarios');
                if (grid) {
                    var selectedRowKeys = grid.GetSelectedKeysOnPage();
                    if (selectedRowKeys.length > 0) {
                        popupUpdateStatus.Show();
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
                popupUpdateStatus.Hide();

                // Limpiar el ComboBox y el Memo
                var cmbStatus = ASPxClientControl.GetControlCollection().GetByName('cmbStatus');
                var txtObs = ASPxClientControl.GetControlCollection().GetByName('txtObs');

                if (cmbStatus) {
                    cmbStatus.SetSelectedIndex(-1); // Deseleccionar cualquier valor
                    cmbStatus.SetValue(null); // Limpiar valor seleccionado
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
                    cmbStatus.GetText(),
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
                        popupUpdateStatus.Show();  // Asegurar que el popup permanezca visible
                    });
                } else {
                    ASPxCallback_PopupUpdate.PerformCallback();
                    GV_PreUsuarios.PerformCallback();
                    popupUpdateStatus.Hide();
                }
            }
            /*  FUNCIONES PARA ACTIVAR Y DESACTIVAR EL BOTÓN DE CERRAR LA CONSTANCIA*/
            function onReportCallbackComplete() {
                    // Se completó el callback, mostraremos el iframe si ya se le puso src en el servidor
                    var iframe = document.getElementById('<%= iframePDF.ClientID %>');
            var btn = document.getElementById('btnCerrarConstancia');
            if (iframe) {
                // Si tiene src válido, lo mostramos y activamos el botón
                if (iframe.src && !iframe.src.endsWith('about:blank')) {
                    iframe.style.display = 'block';
                    if (btn) btn.style.display = 'inline-block';
                }
            }
 }

            function SetCampos() {
                cmbStatus.SetText('');



            }

            function OnFocusedRowChanged(s, e) {
                var index = s.GetFocusedRowIndex();
                if (index >= 0) {
                    s.GetRowValues(index, 'Id', function (value) {
                        // Llama al servidor para recargar el reporte
                        ASPxCallbackPanel_Report.PerformCallback(value.toString());
                    });
                }
            }
            /*  FUNCION PARA BOTÓN DE CERRAR LA CONSTANCIA*/

            function cerrarConstancia() {
                // Solo afecta al WebDocumentViewer, no al iframe de previa
                var mainEl = ASPxWebDocumentViewer1.GetMainElement();
                if (mainEl) mainEl.style.display = 'none';
            }

            function ShowPreviewPopup(uploadId, fileName) {
                console.log("Mostrando vista previa del archivo:", uploadId, fileName);

                var popup = ASPxClientControl.GetControlCollection().GetByName("PopupArchivo");
                var imgPreview = ASPxClientControl.GetControlCollection().GetByName("ImgPreview");
                var lblNoPreview = ASPxClientControl.GetControlCollection().GetByName("LblNoPreview");
                var pdfViewer = document.getElementById("pdfViewer");

                // Ocultar todos los elementos antes de mostrar el correcto
                if (pdfViewer) pdfViewer.hidden = true;
                if (imgPreview) imgPreview.SetVisible(false);
                if (lblNoPreview) lblNoPreview.SetVisible(false);

                var extension = fileName.split('.').pop().toLowerCase();
                var docExtensions = ["pdf", "doc", "docx", "xls", "xlsx"];
                var imgExtensions = ["jpg", "jpeg", "png", "gif"];

                if (extension === "pdf") {
                    if (pdfViewer) {
                        pdfViewer.src = `/FileDownload.ashx?Upload_Id=${uploadId}&mode=view`;
                        pdfViewer.style.display = "block"; // Mostrar el visor
                        pdfViewer.hidden = false;

                    }
                } else if (imgExtensions.includes(extension)) {
                    if (imgPreview) {
                        imgPreview.SetVisible(true);
                        imgPreview.SetImageUrl("/FileDownload.ashx?Upload_Id=" + uploadId);
                    }
                } else {
                    if (lblNoPreview) lblNoPreview.SetVisible(true);
                }

                popup.Show();
            }



        </script>
        <div>
            <br />
            <h1 style="color: #006699; background-color: #FFFFFF; text-align: center; font-size: 30px;">
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true">
                    <EmptyImage IconID="">
                    </EmptyImage>
                </dx:ASPxImage>
                &nbsp; AUTORIZAR CONSTANCIAS  </h1>
        </div>

        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <ContentTemplate>
            <div class="toolbar-filter">
                <span class="filter-label">Tipo de Solicitud:</span>
                <dx:ASPxComboBox ID="cmbTipoFiltro" runat="server"
                    Width="180px" AutoPostBack="True"
                    OnSelectedIndexChanged="cmbTipoFiltro_SelectedIndexChanged">
                    <Items>
                        <dx:ListEditItem Text="Natural" Value="0" />
                        <dx:ListEditItem Text="Jurídica" Value="1" />
                    </Items>
                </dx:ASPxComboBox>
            </div>

            <dx:ASPxGridView ID="GV_PreUsuarios" runat="server" AutoGenerateColumns="False" KeyFieldName="Id" Style="margin-left: auto; margin-right: auto;"
                DataSourceID="SqlDataUsers" OnBeforeExport="GV_PreUsuarios_BeforeExport" ClientInstanceName="GV_PreUsuarios"
                OnCustomCallback="GV_PreUsuarios_CustomCallback" AllowOnlyOneAdaptiveDetailExpanded="true"
                Width="100%" CssClass="responsive-grid">
                <ClientSideEvents FocusedRowChanged="OnFocusedRowChanged" />
                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
                <SettingsPager AlwaysShowPager="True" PageSize="10">
                    <PageSizeItemSettings Items="10, 30, 50" ShowAllItem="True" Visible="True">
                    </PageSizeItemSettings>
                </SettingsPager>
                <Settings ShowFilterRow="True" ShowHeaderFilterButton="False" ShowFooter="True" />
                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" FileName="Constancias_Autorizadas" />
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
                                    <dx:ASPxButton ID="ASPxReportCosntancia" runat="server" Text="Generar Constancia Final" AutoPostBack="False" CssClass="btn-accion" OnClick="ASPxReportCosntancia_Click">
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="btnGuardaConstancia" Visible="false" runat="server" Text="Modificar Constancia" CssClass="btn-accion" AutoPostBack="False" OnClick="btnGuardaConstancia_Click" />

                                </Template>
                            </dx:GridViewToolbarItem>
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="Exportar a Excel">
                                <ItemStyle CssClass="btn-neutro" />
                            </dx:GridViewToolbarItem>

                            <dx:GridViewToolbarItem Command="Refresh" Text="Actualizar">
                                <ItemStyle CssClass="rtbtn-expoar" />
                            </dx:GridViewToolbarItem>

                        </Items>
                    </dx:GridViewToolbar>
                </Toolbars>

                <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="true" />

                <Templates>
                    <DetailRow>
                        <dx:ASPxGridView ID="GV_Detalle" runat="server" AutoGenerateColumns="False"   OnBeforePerformDataSelect="GV_Detalle_BeforePerformDataSelect"
                            Width="100%" KeyFieldName="DetailId" DataSourceID="SqlDataDetalle" CssClass="custom-grid">
                            <SettingsPager PageSize="10" />
                            <Styles>
                                <Header CssClass="custom-grid-header" />
                                <Row CssClass="custom-grid-row" />
                                <SelectedRow CssClass="custom-grid-selected-row" BackColor="#CCCCCC" ForeColor="#003366" />
                                <FocusedRow ForeColor="#E5E5E5">
                                </FocusedRow>
                            </Styles>
                            <SettingsPopup>
                                <FilterControl AutoUpdatePosition="False">
                                </FilterControl>
                            </SettingsPopup>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Codigo" FieldName="Upload_Id" Visible="false" />

                                <dx:GridViewDataTextColumn Caption="NOMBRE DE ARCHIVO" FieldName="File_Name">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="DESCARGAR O VER" FieldName="File_Name" Settings-AllowSort="False">
                                    
                                    <DataItemTemplate>

                                        <dx:ASPxHyperLink ID="hlDownload" runat="server"
                                            NavigateUrl='<%# String.Format("~/FileDownload.ashx?Upload_Id={0}&mode=download", Eval("Upload_Id")) %>'
                                            Text="Descargar" CssClass="link-descargar" />
                                        &nbsp;&nbsp;
                                         <dx:ASPxHyperLink ID="hlView" runat="server"
                                        NavigateUrl='<%# String.Format("~/FileDownload.ashx?Upload_Id={0}&mode=view", Eval("Upload_Id")) %>'
                                        Target="_blank" Text="Ver" CssClass="link-ver" />
                                    </DataItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                </dx:GridViewDataTextColumn>

                            </Columns>
                        </dx:ASPxGridView>


                    </DetailRow>
                </Templates>
                <SettingsPopup>
                    <FilterControl AutoUpdatePosition="False">
                    </FilterControl>
                </SettingsPopup>
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
                    <dx:GridViewDataTextColumn FieldName="Descripcion_Estado" Caption="ESTADO" VisibleIndex="11">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small" />
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
                        <CellStyle HorizontalAlign="Center" Font-Size="Small" />
                    </dx:GridViewDataTextColumn>
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
            <dx:ASPxPopupControl ID="PopupArchivo" runat="server" ShowHeader="True"
                HeaderText="Vista Previa de Archivo" Width="800px" Height="600px"
                Modal="True" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ClientInstanceName="PopupArchivo" >
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">

                        <!-- Visor de PDF con iframe -->
                        <iframe id="pdfViewer" style="width: 100%; height: 100%; border: none;" hidden></iframe>

                        <!-- Visor de Imágenes -->
                        <dx:ASPxImage ID="ImgPreview" runat="server" Width="100%" Height="100%"
                            ClientInstanceName="ImgPreview"
                            ImageUrl='<%# String.Format("~/FileDownload.ashx?Upload_Id={0}", Eval("Upload_Id")) %>' />

                        <!-- Mensaje si no hay vista previa -->
                        <dx:ASPxLabel ID="LblNoPreview" runat="server" Text="No hay vista previa disponible"
                            ClientInstanceName="lblNoPreview" Visible="false" />
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>

        </ContentTemplate>
        <asp:SqlDataSource runat="server" ID="SqlDataUsers" ConnectionString='<%$ ConnectionStrings:connString %>'
            SelectCommand="SELECT DISTINCT Id, Identidad, FirstName, LastName, NumRtn, NomInstitucion, Email, Descripcion_Estado,
             FechaIngreso,FechaTermino, Id_Estado, IdRole, Phone, Observaciones FROM V_Solicitudes
                 WHERE (Id_Estado IN (4)) AND TipoSolicitud = @TipoSolicitud ORDER BY Id DESC">
            <SelectParameters>
                <asp:Parameter Name="TipoSolicitud" Type="Boolean" DefaultValue="false" />
            </SelectParameters>

        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataDetalle" runat="server"
        ConnectionString='<%$ ConnectionStrings:connString %>'
        SelectCommand="SELECT Upload_Id, Id, Identidad, FirstName, LastName, File_Name FROM V_Solicitudes WHERE (Id = @Id)">
        <SelectParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
               
       
        <dx:ASPxPopupControl ID="popupUpdateStatus" runat="server" ClientInstanceName="popupUpdateStatus"
            ShowCloseButton="True" HeaderText="Actualizar Estado"
            Width="400px" Height="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" SettingsAdaptivity-Mode="OnWindowInnerWidth" Modal="True" CloseAnimationType="None" AllowResize="False" AutoUpdatePosition="False" SettingsAdaptivity-VerticalAlign="WindowCenter">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <div style="display: flex; align-items: center; justify-content: center; height: 100%;">
                        <div style="text-align: center;">
                            <dx:ASPxComboBox ID="cmbStatus" runat="server" ValueField="Id_Estado" TextField="Descripcion_Estado" ClientInstanceName="cmbStatus"
                                Width="300px" Height="30px" ValueType="System.Int32" Caption="Seleccionar el Estado"
                                NullText="Seleccionar Estado" CaptionSettings-Position="Top" ClearButton-DisplayMode="Always" CaptionSettings-RequiredMarkDisplayMode="Required">
                                <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" RequiredField-IsRequired="true"
                                    SetFocusOnError="True">
                                    <RequiredField ErrorText="El Estado es requerido." IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                            <br />
                            <br />
                            <dx:ASPxButton ID="btnPopupUpdate" runat="server" Text="Actualizar" AutoPostBack="False" UseSubmitBehavior="false" ClientInstanceName="btnPopupUpdate">
                                <ClientSideEvents Click="btnPopupUpdate_Click" />
                            </dx:ASPxButton>
                            <dx:ASPxCallback ID="ASPxCallback_PopupUpdate" runat="server" ClientInstanceName="ASPxCallback_PopupUpdate" OnCallback="ASPxCallback_PopupUpdate_Callback">
                            </dx:ASPxCallback>
                        </div>
                    </div>

                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>


        <dx:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" Width="100%">
            <TabPages>

                <dx:TabPage Name="Estadistica" Text=" Constancia Previa" Visible="true">

                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <dx:PanelContent runat="server">

                                <dx:ASPxCallbackPanel ID="ASPxCallbackPanel_Report" runat="server" Width="100%" OnCallback="ASPxCallbackPanel_Report_Callback" ClientInstanceName="ASPxCallbackPanel_Report">
                                    <ClientSideEvents EndCallback="function(s,e){ onReportCallbackComplete(); }" />
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">
                                            
                                            <iframe id="iframePDF" runat="server" style="width:100%; height:800px; display:none;"></iframe>

                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </dx:PanelContent>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
                <dx:TabPage Name="Constancia" Text="Constancia Final" Visible="true">

                    <ContentCollection>
                        <dx:ContentControl runat="server">
                            <div style="position: relative; margin-bottom: 16px; padding-top: 8px;">
                                <button id="btnCerrarConstancia" class="btn-neutro"
                                    style="position: absolute; right: 0; top: -8px; z-index: 10;"
                                    onclick="cerrarConstancia();">
                                    Cerrar Constancia 
                                </button>
                            </div>
                            <br />
                            <dx:ASPxWebDocumentViewer
                                ID="ASPxWebDocumentViewer1"
                                runat="server"
                                ClientInstanceName="ASPxWebDocumentViewer1"
                                Height="750px"
                                RightToLeft="false"
                                DisableHttpHandlerValidation="False">
                            </dx:ASPxWebDocumentViewer>
                        </dx:ContentControl>
                    </ContentCollection>
                </dx:TabPage>
            </TabPages>
        </dx:ASPxPageControl>

    </asp:Panel>

</asp:Content>
