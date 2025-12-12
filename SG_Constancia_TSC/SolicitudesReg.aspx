<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="SolicitudesReg.aspx.cs" Inherits="SG_Constancia_TSC.SolicitudesReg" Async="true" %>

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
        <link href="Content/css/normalize.css" rel="stylesheet" />
        <script src="/Content/Sweetalert/js/sweetalert2@11.js"></script>
        <link href="/Content/Sweetalert/css/sweetalert2.min.css" rel="stylesheet" />


        <script type="text/javascript">
            if (window.history.replaceState) {
                window.history.replaceState(null, null, window.location.href);
            }

            function OnRowFocused(s, e) {
                var rowIndex = s.GetFocusedRowIndex();
                if (rowIndex !== -1) {
                    var key = s.GetRowKey(rowIndex);
                    console.log("ID seleccionado:", key);

                }
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


            function btnPopupUpdate_Click() {
                var campos = [cmbStatus.GetText(), txtObs.GetText()];
                var camposVacios = campos.some(function (valor) {
                    return !valor || valor.trim() === '';
                });

                if (camposVacios) {
                    Swal.fire({
                        title: "¡Alerta!",
                        text: "Debe completar los campos obligatorios",
                        icon: "warning",
                        confirmButtonColor: "#1F497D"
                    }).then(function () {
                        popupUpdateStatus.Show();
                    });
                } else {
                    ASPxCallback_PopupUpdate.PerformCallback(); // Llamada al servidor
                    popupUpdateStatus.Hide();
                }
            }
        </script>
        <div>
            <br />

            <h1 class="titulo-proceso">
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" />
                &nbsp;SOLICITUDES EN PROCESO
            </h1>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                <dx:ASPxGridView ID="GV_PreUsuarios" runat="server" AutoGenerateColumns="False" KeyFieldName="Id" ClientInstanceName="GV_PreUsuarios"
                    DataSourceID="SqlDataUsers" Width="100%" CssClass="responsive-grid" OnDetailRowExpandedChanged="GV_PreUsuarios_DetailRowExpandedChanged" EnableTheming="True" Theme="Office365">
                    <%--<SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />--%>
                    <SettingsPager AlwaysShowPager="True" PageSize="10">
                        <PageSizeItemSettings Items="10, 30, 50" ShowAllItem="True" Visible="True">
                        </PageSizeItemSettings>
                    </SettingsPager>

                    <Settings ShowFilterRow="True" ShowHeaderFilterButton="False" ShowFooter="True" />
                    <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" FileName="Solicitudes_En_Proceso" />
                    <SettingsPopup>
                        <FilterControl AutoUpdatePosition="False"></FilterControl>
                    </SettingsPopup>
                    <SettingsSearchPanel Visible="True"></SettingsSearchPanel>

                    <SettingsBehavior AllowSelectByRowClick="False" AllowFocusedRow="false" AllowSelectSingleRowOnly="True" AllowSort="False" />
                    <Toolbars>
                        <dx:GridViewToolbar>
                            <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                            <Items>

                                <dx:GridViewToolbarItem Command="Custom">
                                    <Template>
                                        <dx:ASPxButton ID="btnUpdateStatus" runat="server" Text="Actualizar Estado" AutoPostBack="False" CssClass="btn-accion">
                                            <ClientSideEvents Click="function(s, e) { ShowPopup(); }" />
                                        </dx:ASPxButton>
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

                    <SettingsDetail ShowDetailRow="true" />
                    <Templates>
                        <DetailRow>
                            <dx:ASPxGridView ID="GV_Detalle" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="DetailId" DataSourceID="SqlDataDetalle" CssClass="custom-grid">
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

                        <dx:GridViewCommandColumn SelectAllCheckboxMode="Page" ShowSelectCheckbox="True" ShowClearFilterButton="false"
                            Caption="✔" VisibleIndex="0">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" Font-Size="Small" />
                            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
                            <FooterCellStyle Font-Overline="False" />
                        </dx:GridViewCommandColumn>

                        <dx:GridViewDataTextColumn FieldName="Identidad" Name="Identidad" Caption="DNI" VisibleIndex="1" Visible="true">

                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="FirstName" Name="FirstName" Caption="NOMBRE" VisibleIndex="3" Visible="true">

                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="LastName" Name="LastName" Caption="APELLIDO" VisibleIndex="4" Visible="true">

                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
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
                        <dx:GridViewDataTextColumn Caption="RTN" FieldName="NumRtn" Name="NumRtn" Visible="true" VisibleIndex="2">

                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="EMPRESA/INSTITUCIÓN" Name="NomInstitucion" FieldName="NomInstitucion"
                            VisibleIndex="6" Visible="true">

                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataDateColumn FieldName="FechaIngreso" Caption="FECHA SOLICITUD" VisibleIndex="9">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataTextColumn Caption="OBSERVACIÓN" FieldName="Observaciones" VisibleIndex="10">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn Caption="ESTADO" FieldName="Descripcion_Estado" ShowInCustomizationForm="True" VisibleIndex="8">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />
                            <CellStyle HorizontalAlign="Center">
                            </CellStyle>

                            <PropertiesComboBox DataSourceID="SqlDataSourcEstados" TextField="Descripcion_Estado" ValueField="Id_Estado"></PropertiesComboBox>
                        </dx:GridViewDataComboBoxColumn>


                        <dx:GridViewDataTextColumn FieldName="Id_Estado" Visible="False">
                        </dx:GridViewDataTextColumn>


                    </Columns>

                    <Styles>
                        <SelectedRow BackColor="#CCCCCC" ForeColor="#003366" Font-Bold="True" />
                        <FocusedRow BackColor="#E5E5E5" />
                    </Styles>
                </dx:ASPxGridView>
                <dx:ASPxPopupControl ID="PopupArchivo" runat="server" ShowHeader="True"
                    HeaderText="Vista Previa de Archivo" Width="800px" Height="600px"
                    Modal="True" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ClientInstanceName="PopupArchivo">
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
                <asp:SqlDataSource ID="SqlDataUsers" runat="server"
                    ConnectionString="<%$ ConnectionStrings:connString %>"
                    SelectCommand="
                  SELECT DISTINCT 
        Id, Identidad, FirstName, LastName, Email,
        NumRtn, NomInstitucion, TipoSolicitud,
        Descripcion_Estado, FechaIngreso, Id_Estado,
        IdRole, Observaciones,Phone
      FROM V_Solicitudes
      WHERE Id_Estado IN (1,2)
        AND TipoSolicitud = @TipoSolicitud
      ORDER BY Id DESC">
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
                <br />
                <asp:SqlDataSource ID="SqlDataSourcEstados" runat="server" ConnectionString="<%$ ConnectionStrings:connString %>" SelectCommand="SELECT * FROM [Estados] where Id_Estado in (1,2)"></asp:SqlDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

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
                        <dx:ASPxMemo ID="txtObs" runat="server" Width="100%" Height="100px" NullText="Ingrese una Observación"
                            ClientInstanceName="txtObs" Caption="Ingrese una Observación" CaptionSettings-Position="Top">
                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" RequiredField-IsRequired="true"
                                SetFocusOnError="True">
                                <RequiredField ErrorText="La Observación es requerida." IsRequired="true" />
                            </ValidationSettings>
                        </dx:ASPxMemo>
                        <br />
                        <dx:ASPxButton ID="btnPopupUpdate" runat="server" Text="Actualizar" AutoPostBack="False" UseSubmitBehavior="false" ClientInstanceName="btnPopupUpdate" CssClass="btn-accion">
                            <ClientSideEvents Click="btnPopupUpdate_Click" />
                        </dx:ASPxButton>
                        <dx:ASPxCallback ID="ASPxCallback_PopupUpdate" runat="server" ClientInstanceName="ASPxCallback_PopupUpdate" OnCallback="ASPxCallback_PopupUpdate_Callback">
                        </dx:ASPxCallback>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <script>

       
        function ShowPopup() {
            var grid = ASPxClientControl.GetControlCollection().GetByName('GV_PreUsuarios');
            if (!grid) {
                alert('Grid no encontrado.');
                return;
            }

            // 1) Verifica selección única
            var keys = grid.GetSelectedKeysOnPage();
            if (keys.length !== 1) {
                Swal.fire({
                    title: "¡Alerta!",
                    text: "Por favor seleccione una fila para actualizar el estado.",
                    icon: "warning",
                    confirmButtonColor: "#1F497D"
                });
                return;
            }


            //    (en semicolon-delimited string)
            grid.GetSelectedFieldValues(
                'Descripcion_Estado;Observaciones',
                function (rows) {
                    if (!rows || rows.length === 0) return;

                    var data = rows[0];          // solo hay una fila seleccionada
                    var estadoId = data[0];   // primer campo: Id_Estado
                    var observacion = data[1];  // segundo campo: Observaciones

                    // 3) Carga valores en los controles del popup
                    var cmb = ASPxClientControl.GetControlCollection().GetByName('cmbStatus');
                    var txt = ASPxClientControl.GetControlCollection().GetByName('txtObs');

                    if (cmb) cmb.SetValue(estadoId);
                    if (txt) txt.SetText(observacion || '');

                    // 4) Muestra el popup
                    popupUpdateStatus.Show();
                }
            );
        }



    </script>
</asp:Content>
