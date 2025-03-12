<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/Main.master" CodeBehind="Solicitudes_finalizadas.aspx.cs" Inherits="SG_Constancia_TSC.Solicitudes_finalizadas" %>


<%@ Register Assembly="DevExpress.XtraReports.v21.2.Web.WebForms, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
<style>

   .swal2-container {
          z-index: 20000 !important; /* Asegúrate de que sea mayor que el z-index del popup */
      }
      .dx-popup {
          z-index: 10000 !important; /* Ajusta esto según el valor adecuado para tu popup */
      }
</style>
      <script>
          function OnRowFocused(s, e) {
              var rowIndex = s.GetFocusedRowIndex();
              if (rowIndex !== -1) {
                  var key = s.GetRowKey(rowIndex);
                 /* console.log("ID seleccionado:", key);*/
                  // Aquí puedes realizar acciones adicionales, como actualizar otro control
              }
          }
          function btnPopupUpdate_Click(s, e) {
              //e.preventDefault();  // Prevenir el comportamiento por defecto del botón

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
                 /* txtObs.SetText('');*/


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
                      //alert('Please select a row to update the status.');
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
          <link href="Content/css/sweetalert2.min.css" rel="stylesheet" />
           <script src="Content/js/sweetalert2.all.min.js"></script>
          <script type="text/javascript">
            if (window.history.replaceState) {
                window.history.replaceState(null, null, window.location.href);
              }



          </script>
                <div>
                    <br />
            <h1 style= "color: #006699; background-color: #FFFFFF; text-align:center ">
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true">
                    <EmptyImage IconID="">
                    </EmptyImage>
                </dx:ASPxImage>
             &nbsp;CONSTANCIAS ENTEGADAS </h1>
                    </div>
          <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <ContentTemplate>

<%--    <dx:ASPxGridView ID="GV_PreUsuarios" runat="server" AutoGenerateColumns="False" Style="margin-left: auto; margin-right: auto;"
    KeyFieldName="Id" DataSourceID="SqlDataUsers" OnBeforeExport="GV_PreUsuarios_BeforeExport" ClientInstanceName="GV_PreUsuarios" 
        OnCustomCallback="GV_PreUsuarios_CustomCallback" 
    Width="100%" CssClass="responsive-grid">
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
          <SettingsPager AlwaysShowPager="True" PageSize="10">
     <PageSizeItemSettings Items="10, 30, 50" ShowAllItem="True" Visible="True">
         </PageSizeItemSettings>
</SettingsPager>
    <Settings ShowFilterRow="false" ShowHeaderFilterButton="True" ShowFooter="True" />
    <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" />
    <SettingsPopup>
        <FilterControl AutoUpdatePosition="False"></FilterControl>
    </SettingsPopup>
    <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
        <SettingsBehavior AllowFocusedRow="true"  AllowSelectByRowClick="true" />
                  
<SettingsPopup>
<FilterControl AutoUpdatePosition="False"></FilterControl>
</SettingsPopup>--%>

    <dx:ASPxGridView ID="GV_PreUsuarios" runat="server" AutoGenerateColumns="False" Style="margin-left: auto; margin-right: auto;"
    KeyFieldName="Id" DataSourceID="SqlDataUsers" OnBeforeExport="GV_PreUsuarios_BeforeExport" ClientInstanceName="GV_PreUsuarios" 
        OnCustomCallback="GV_PreUsuarios_CustomCallback" 
    Width="100%" CssClass="responsive-grid">
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />
          <SettingsPager AlwaysShowPager="True" PageSize="10">
     <PageSizeItemSettings Items="10, 30, 50" ShowAllItem="True" Visible="True">
         </PageSizeItemSettings>
</SettingsPager>
    <Settings ShowFilterRow="True" ShowHeaderFilterButton="True" ShowFooter="True" />
    <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" />
    <SettingsPopup>
        <FilterControl AutoUpdatePosition="False"></FilterControl>
    </SettingsPopup>
    <SettingsSearchPanel Visible="True"></SettingsSearchPanel>
        <SettingsBehavior AllowFocusedRow="true" AllowSelectByRowClick="False" />
                  
<SettingsPopup>
<FilterControl AutoUpdatePosition="False"></FilterControl>
</SettingsPopup>
    
    <Toolbars>
        <dx:GridViewToolbar>
            <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
            <Items>
                <dx:GridViewToolbarItem Command="Custom">
                    <Template>
                        <dx:ASPxButton ID="ASPxReportCosntancia" runat="server" Text="Constancia" AutoPostBack="False" OnClick="ASPxReportCosntancia_Click" />
                        <dx:ASPxButton ID="ASPxInforme" runat="server" Text="Informe" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) { Showinforme(); }" />
                        </dx:ASPxButton>
                    </Template>
                </dx:GridViewToolbarItem>
                <dx:GridViewToolbarItem Command="ExportToCsv" />
                <dx:GridViewToolbarItem Command="Refresh" />
            </Items>
        </dx:GridViewToolbar>
    </Toolbars>
    <SettingsSearchPanel Visible="True" />
    <Columns>

        <dx:GridViewCommandColumn 
            ShowSelectCheckbox="True" 
            SelectAllCheckboxMode="None" 
            ShowClearFilterButton="True" 
            VisibleIndex="0">
            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewCommandColumn>


        <dx:GridViewDataTextColumn FieldName="FirstName" ShowInCustomizationForm="True" VisibleIndex="3" Caption="NOMBRE">
            <PropertiesTextEdit>
                <Style Font-Size="Small" />
            </PropertiesTextEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="LastName" ShowInCustomizationForm="True" VisibleIndex="4" Caption="APELLIDO">
            <PropertiesTextEdit>
                <Style Font-Size="Small" />
            </PropertiesTextEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Identidad" ShowInCustomizationForm="True" VisibleIndex="2" Caption="DNI">
            <PropertiesTextEdit>
                <Style Font-Size="Small" />
            </PropertiesTextEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="IdRole" VisibleIndex="1" Visible="False" />
        <dx:GridViewDataTextColumn FieldName="Id" VisibleIndex="14" Visible="False" />
        <dx:GridViewDataTextColumn FieldName="Descripcion_Estado" Caption="ESTADO" VisibleIndex="12">
            <PropertiesTextEdit>
                <Style Font-Size="Small" />
            </PropertiesTextEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="FechaIngreso" Caption="FECHA TERMINO" VisibleIndex="15">
            <PropertiesDateEdit>
                <Style Font-Size="Small" />
            </PropertiesDateEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataDateColumn>
    </Columns>
    <Styles>
        <SelectedRow BackColor="#CCCCCC" ForeColor="#1F497D" Font-Bold="True" />
        <%--<FocusedRow BackColor="" />--%>
    </Styles>
</dx:ASPxGridView>

               

    </ContentTemplate>

          <asp:SqlDataSource runat="server" ID="SqlDataUsers" ConnectionString='<%$ ConnectionStrings:connString %>' SelectCommand="SELECT DISTINCT Id, Identidad, FirstName, LastName, email, [Descripcion_Estado], 
                                    [FechaIngreso], [Id_Estado], [IdRole] FROM [dbo].[V_Solicitudes] 
                                    WHERE Id_Estado IN (6) ORDER BY Id DESC"></asp:SqlDataSource>
   


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
<%--                    <dx:ASPxMemo ID="txtObs" runat="server" Width="100%" Height="100px" NullText="Ingrese una Observación" 
                        ClientInstanceName="txtObs" Caption="Ingrese una Observación" CaptionSettings-Position="Top">
                        <ValidationSettings Display="Dynamic" ErrorTextPosition="Bottom" RequiredField-IsRequired="true" 
                            SetFocusOnError="True">
                            <RequiredField ErrorText="La Observación es requerida." IsRequired="true" />
                        </ValidationSettings>
                    </dx:ASPxMemo>--%>
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

    <fieldset>
        <legend style="margin-left: 30px;">FIRMAS ADJUNTAS</legend>
        <dx:ASPxCheckBoxList ID="ASPxChkAdjunto" runat="server" CssClass="margen-izquierda" ValueType="System.String">
        <Items>
        <dx:ListEditItem Text="Firma Secretario(a) Adjunto " Value="Firma Secretario(a) Adjunto" />
        <dx:ListEditItem Text="Firma Encargado de estadística" Value="Firma Encargado de estadística" />
        </Items>
        </dx:ASPxCheckBoxList>
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

<script type="text/javascript">
    //function ShowPopup() {
    //    popupUpdateStatus.Show();
    //}

    function ShowPopup() {
        var grid = ASPxClientControl.GetControlCollection().GetByName('GV_PreUsuarios');
        if (grid) {
            var selectedRowKeys = grid.GetSelectedKeysOnPage();
            if (selectedRowKeys.length > 0) {
                popupUpdateStatus.Show();
            } else {
                Swal.fire({
                    title: "¡Alerta!",
                    text: "Por favor seleccione una fila para Actualizar el Estado.",
                    icon: "warning",
                    confirmButtonColor: "#1F497D"
                });
                //alert('Please select a row to update the status.');
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

</script>
      
      </asp:Panel>
</asp:Content>
