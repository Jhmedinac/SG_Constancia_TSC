<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Solicitudes_finalizadas.aspx.cs" Inherits="SG_Constancia_TSC.Solicitudes_finalizadas" %>


<%@ Register Assembly="DevExpress.XtraReports.v21.2.Web.WebForms, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

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

            function OnRowFocused(s, e) {
                var rowIndex = s.GetFocusedRowIndex();
                if (rowIndex !== -1) {
                    var key = s.GetRowKey(rowIndex);
                    console.log("ID seleccionado:", key);

                }
            }


            function ShowPopup() {
                var grid = ASPxClientControl.GetControlCollection().GetByName('GV_PreUsuarios');
                if (!grid) { alert('Grid not found.'); return; }

                var focusedRowIndex = grid.GetFocusedRowIndex();
                if (focusedRowIndex !== -1) {
                    
                    if (grid.SetFocusedRowIndex) grid.SetFocusedRowIndex(focusedRowIndex);

                    var key = grid.GetRowKey(focusedRowIndex);
                    console.log("ID seleccionado:", key);
                    popupUpdateStatus.Show();
                } else {
                    Swal.fire({
                        title: "¡Alerta!",
                        text: "Por favor genere la Constancia Previa antes de actualizar el estado.",
                        icon: "warning",
                        confirmButtonColor: "#1F497D"
                    });
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

            function SetCampos() {
                cmbStatus.SetText('');


            }


            function onPopupUpdateCallbackComplete(s, e) {
                if (!e.result) return;
                var parts = e.result.split('|');
                var status = parts[0]; // "OK" o "ERROR"
                var message = parts[1] || '';
                var flag = parts[2] || '';

                if (status === 'OK') {
                    Swal.fire({
                        icon: 'success',
                        title: '¡Éxito!',
                        text: message,
                        confirmButtonColor: '#1F497D'
                    });

                    /*  limpia/oculta el viewer*/
                    if (flag === 'CLEAR') {

                        var viewer = ASPxWebDocumentViewer1; // instancia cliente
                        // Oculta visualmente
                        var cont = document.getElementById(ASPxWebDocumentViewer1.GetMainElement().id);
                        if (cont) cont.style.display = 'none';


                    }
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: message || 'Ocurrió un error.',
                        confirmButtonColor: '#1F497D'
                    });
                }
            }

        </script>
        <div>
            <br />
            <h1 style="color: #006699; background-color: #FFFFFF; text-align: center; font-size: 30px;">
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true">
                    <EmptyImage IconID="">
                    </EmptyImage>
                </dx:ASPxImage>
                &nbsp;SOLICITUDES APROBADAS </h1>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <contenttemplate>

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

            <dx:ASPxGridView ID="GV_PreUsuarios" runat="server" AutoGenerateColumns="False" Style="margin-left: auto; margin-right: auto;"
                KeyFieldName="Id" DataSourceID="SqlDataUsers" ClientInstanceName="GV_PreUsuarios"
                OnCustomCallback="GV_PreUsuarios_CustomCallback"
                Width="100%" CssClass="responsive-grid">
                <SettingsPager AlwaysShowPager="True" PageSize="10">
                    <PageSizeItemSettings Items="10, 30, 50" ShowAllItem="True" Visible="True">
                    </PageSizeItemSettings>
                </SettingsPager>
                <Settings ShowFilterRow="True" ShowHeaderFilterButton="False" ShowFooter="True" />
                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" FileName="Solicitudes_Aprobadas" />
                <SettingsPopup>
                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                </SettingsPopup>
                <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
                <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="False" AllowSort="False"/>
                <Toolbars>
                    <dx:GridViewToolbar>
                        <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                        <Items>
                            <dx:GridViewToolbarItem Command="Custom">
                                <Template>
                                    <dx:ASPxButton ID="ASPxReportCosntancia" runat="server" Text="Generar Constancia Previa" AutoPostBack="False"
                                        CssClass="btn-accion" OnClick="ASPxReportCosntancia_Click">
                                    </dx:ASPxButton>

                                    <dx:ASPxButton ID="btnGuardaConstancia" Visible="false" runat="server" Text="Modificar Constancia"
                                        CssClass="btn-accion" AutoPostBack="False" OnClick="btnGuardaConstancia_Click">
                                    </dx:ASPxButton>


                                </Template>
                            </dx:GridViewToolbarItem>
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
                    <dx:GridViewDataTextColumn FieldName="Identidad" Name="Identidad" Visible="True" ShowInCustomizationForm="True" VisibleIndex="2" Caption="DNI">
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
                    <dx:GridViewDataTextColumn FieldName="Descripcion_Estado" Name="Descripcion_Estado" Caption="ESTADO" VisibleIndex="11">
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

        </contenttemplate>

        <asp:SqlDataSource runat="server" ID="SqlDataUsers" ConnectionString='<%$ ConnectionStrings:connString %>' SelectCommand="SELECT DISTINCT Id, Identidad, FirstName, LastName, NumRtn, NomInstitucion, TipoSolicitud, Email, Descripcion_Estado, Phone,
            FechaIngreso, FechaTermino, Id_Estado, IdRole, Observaciones FROM V_Solicitudes WHERE (Id_Estado IN (3)) 
            AND TipoSolicitud = @TipoSolicitud ORDER BY Id DESC">
            <SelectParameters>
                <asp:Parameter Name="TipoSolicitud" Type="Boolean" DefaultValue="false" />
            </SelectParameters>
        </asp:SqlDataSource>
        <br />
        <fieldset>
            <legend style="margin-left: 5px;">FIRMA ADJUNTA</legend>
            <dx:ASPxRadioButtonList ID="ASPxRblAdjunto" runat="server" CssClass="margen-izquierda" ValueType="System.String">
                <Items>
                </Items>
            </dx:ASPxRadioButtonList>

        </fieldset>

        <dx:PanelContent runat="server">
            <dx:ASPxWebDocumentViewer
                ID="ASPxWebDocumentViewer1"
                runat="server"
                ClientInstanceName="ASPxWebDocumentViewer1"
                Height="750px"
                RightToLeft="false"
                DisableHttpHandlerValidation="False">
            </dx:ASPxWebDocumentViewer>
        </dx:PanelContent>

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
                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom"
                                SetFocusOnError="True">
                                <RequiredField ErrorText="El Estado es requerido." />
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                        <br />
                        <dx:ASPxMemo ID="txtObs" runat="server" Width="100%" Height="100px" NullText="Ingrese una Observación"
                            ClientInstanceName="txtObs" Caption="Ingrese una Observación" CaptionSettings-Position="Top" CaptionSettings-RequiredMarkDisplayMode="Required" Text="Pendiente de Firma.">
                            <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom"
                                SetFocusOnError="True">
                                <RequiredField ErrorText="La Observación es requerida." />
                            </ValidationSettings>
                        </dx:ASPxMemo>
                        <br />
                        <dx:ASPxButton ID="btnPopupUpdate" runat="server" Text="Actualizar" AutoPostBack="False" UseSubmitBehavior="false" ClientInstanceName="btnPopupUpdate" CssClass="btn-accion">
                            <ClientSideEvents Click="btnPopupUpdate_Click" />
                        </dx:ASPxButton>
                        <dx:ASPxCallback ID="ASPxCallback_PopupUpdate" runat="server" ClientInstanceName="ASPxCallback_PopupUpdate" OnCallback="ASPxCallback_PopupUpdate_Callback">
                            <ClientSideEvents CallbackComplete="onPopupUpdateCallbackComplete" />
                        </dx:ASPxCallback>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
