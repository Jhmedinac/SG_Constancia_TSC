<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Solicitudes_Rechazadas.aspx.cs" Inherits="SG_Constancia_TSC.Solicitudes_Rechazadas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style>
        .swal2-container {
            z-index: 20000 !important;
        }

        .dx-popup {
            z-index: 10000 !important;
        }
    </style>
    <script>

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

        function btnBeforeExport(s, e) {
            var grid = ASPxClientControl.GetControlCollection().GetByName('GV_PreUsuarios');
            if (grid) {
                var selectedRowKeys = grid.GetSelectedKeysOnPage();
                if (selectedRowKeys.length > 0) {
                    ASPxCallback_BeforeExport.PerformCallback();
                    GV_PreUsuarios.PerformCallback();
                } else {
                    Swal.fire({
                        title: "¡Alerta!",
                        text: "Por favor seleccione una fila para Exportar usuario.",
                        icon: "warning",
                        confirmButtonColor: "#1F497D"
                    });

                }
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftPanelContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Content" runat="server">


    <asp:Panel ID="Panel_Content" runat="server">
        <link href="Content/css/style.css" rel="stylesheet" />
        <script src="/Content/Sweetalert/js/sweetalert2@11.js"></script>

        <script type="text/javascript">
            if (window.history.replaceState) {
                window.history.replaceState(null, null, window.location.href);
            }

        </script>
        <div>
            <br />
            <h1 style="color: #006699; background-color: #FFFFFF; text-align: center; font-size: 30px;">
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true">
                    <EmptyImage IconID="">
                    </EmptyImage>
                </dx:ASPxImage>
                &nbsp;SOLICITUDES RECHAZADAS </h1>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
                <%--<SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />--%>
                <SettingsPager AlwaysShowPager="True" PageSize="10">
                    <PageSizeItemSettings Items="10, 30, 50" ShowAllItem="True" Visible="True">
                    </PageSizeItemSettings>
                </SettingsPager>
                <Settings ShowFilterRow="True" ShowHeaderFilterButton="False" ShowFooter="True" />
                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" FileName="Solicitudes_Rechazadas" />
                <SettingsPopup>
                    <FilterControl AutoUpdatePosition="False"></FilterControl>
                </SettingsPopup>
                <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
                <SettingsBehavior AllowSelectByRowClick="true" AllowFocusedRow="false" AllowSort="False" />
                <Toolbars>
                    <dx:GridViewToolbar>
                        <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
                        <Items>

                            <dx:GridViewToolbarItem Command="ExportToXls" Text="Exportar a Excel">
                                <ItemStyle CssClass="btn-neutro" />

                            </dx:GridViewToolbarItem>

                        </Items>
                    </dx:GridViewToolbar>
                </Toolbars>
                <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="FirstName" Name="FirstName" Visible="true" ShowInCustomizationForm="True" VisibleIndex="2" Caption="NOMBRE">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small"></Style>
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center" Font-Size="Small"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="LastName" Name="LastName" Visible="true" ShowInCustomizationForm="True" VisibleIndex="4" Caption="APELLIDO">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small"></Style>
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center" Font-Size="Small"></CellStyle>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="Identidad" Name="Identidad" Visible="true" ShowInCustomizationForm="True" VisibleIndex="1" Caption="DNI">
                        <PropertiesTextEdit>
                            <Style Font-Size="Small"></Style>
                        </PropertiesTextEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center" Font-Size="Small"></CellStyle>
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

                    <dx:GridViewDataTextColumn FieldName="IdRole" VisibleIndex="0" Visible="False"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="11" Visible="False"></dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="RTN" FieldName="NumRtn" Name="NumRtn" Visible="true" VisibleIndex="3">

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

                    <dx:GridViewDataTextColumn FieldName="Descripcion_Estado" Name="Descripcion_Estado" Caption="ESTADO" VisibleIndex="11">
      <PropertiesTextEdit>
          <Style Font-Size="Small" />
      </PropertiesTextEdit>
      <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
      <CellStyle HorizontalAlign="Center" Font-Size="Small" />
  </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="FechaTermino" Name="FechaTermino" Visible="true" Caption="FECHA TERMINO" VisibleIndex="13">
                        <PropertiesDateEdit>
                            <Style Font-Size="Small">
                           </Style>
                        </PropertiesDateEdit>
                        <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center" Font-Size="Small"></CellStyle>

                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn Caption="OBSERVACIÓN" Name="Observaciones" Visible="true" FieldName="Observaciones" VisibleIndex="14">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />

                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="FECHA SOLICITUD" FieldName="FechaIngreso" Name="FechaIngreso" VisibleIndex="12">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="True" BackColor="#1F497D" ForeColor="White" />

                        <CellStyle HorizontalAlign="Center">
                        </CellStyle>
                    </dx:GridViewDataDateColumn>
                </Columns>
                <Styles>
                    <SelectedRow BackColor="#CCCCCC" ForeColor="#003366" Font-Bold="True" />
                    <FocusedRow BackColor="#E5E5E5" />
                </Styles>
            </dx:ASPxGridView>

        </contenttemplate>

        <asp:SqlDataSource runat="server" ID="SqlDataUsers" ConnectionString='<%$ ConnectionStrings:connString %>' 
            SelectCommand="SELECT DISTINCT Id, Identidad, FirstName, LastName, NumRtn, NomInstitucion, Email, 
            Descripcion_Estado, FechaIngreso,FechaTermino, Id_Estado, IdRole, Observaciones, TipoSolicitud,FechaTermino,Phone
            FROM V_Solicitudes WHERE (Id_Estado IN (5)) ORDER BY Id DESC"></asp:SqlDataSource>
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
                                <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" RequiredField-IsRequired="true" SetFocusOnError="True">
                                    <RequiredField ErrorText="El Estado es requerido." IsRequired="true" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                            <br />

                            <br />
                            <dx:ASPxButton ID="btnPopupUpdate" runat="server" Text="Actualizar" AutoPostBack="False" UseSubmitBehavior="false" CssClass="btn-accion" ClientInstanceName="btnPopupUpdate">
                                <ClientSideEvents Click="btnPopupUpdate_Click" />
                            </dx:ASPxButton>
                            <dx:ASPxCallback ID="ASPxCallback_PopupUpdate" runat="server" ClientInstanceName="ASPxCallback_PopupUpdate" OnCallback="ASPxCallback_PopupUpdate_Callback">
                            </dx:ASPxCallback>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>



        <script type="text/javascript">


            function ShowPopup() {
                var grid = ASPxClientControl.GetControlCollection().GetByName('GV_PreUsuarios');
                if (!grid) {
                    return alert('Grid not found.');
                }

                var selectedKeys = grid.GetSelectedKeysOnPage();
                if (selectedKeys.length !== 1) {
                    return Swal.fire({
                        title: "¡Alerta!",
                        text: "Por favor seleccione exactamente una fila para actualizar.",
                        icon: "warning",
                        confirmButtonColor: "#1F497D"
                    });
                }

                var key = selectedKeys[0];
                // Obtiene el índice de la fila por su key
                var rowIndex = grid.GetRowIndexByKey(key);

                // Recupera el estado actual y la observación de esa fila
                grid.GetRowValues(rowIndex,
                    'Id_Estado;Observaciones',   // usa el FieldName exacto que tengas en tu data source
                    function (values) {
                        var estadoValue = values[0]; // Id_Estado
                        var observacionValue = values[1]; // Observaciones

                        // Pre‑setea el combo y el memo en el popup
                        var cmbStatus = ASPxClientControl.GetControlCollection().GetByName('cmbStatus');
                        var txtObs = ASPxClientControl.GetControlCollection().GetByName('txtObs');

                        if (cmbStatus) {
                            cmbStatus.SetValue(estadoValue);
                        }
                        if (txtObs) {
                            txtObs.SetText(observacionValue || '');
                        }

                        // Finalmente muestra el popup
                        popupUpdateStatus.Show();
                    }
                );
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

        </script>

    </asp:Panel>
</asp:Content>
