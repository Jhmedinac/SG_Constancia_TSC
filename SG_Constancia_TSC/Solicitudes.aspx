<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="Solicitudes.aspx.cs" Inherits="SG_Constancia_TSC.Solicitudes" %>

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
          //ASPxCallback_PopupUpdate.CallbackComplete.AddHandler(function (s, e) {
          //    Swal.fire({
          //        title: "¡Éxito!",
          //        text: e.result,
          //        icon: "success",
          //        confirmButtonColor: "#1F497D"
          //    }).then(function () {
          //        GV_PreUsuarios.PerformCallback(); // Refrescar la cuadrícula
          //        popupUpdateStatus.Hide(); // Ocultar el popup
          //    });
          //});

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
                      text: "Debe completar los campos obligatorios",
                      icon: "warning",
                      confirmButtonColor: "#1F497D",
                  }).then(function () {
                      popupUpdateStatus.Show();  // Mantener el popup visible
                  });
              } else {
                  ASPxCallback_PopupUpdate.PerformCallback(); // Invoca el servidor
              }
          }
          //function btnPopupUpdate_Click(s, e) {
          //    //e.preventDefault();  // Prevenir el comportamiento por defecto del botón

          //    var campos = [
          //        cmbStatus.GetText(),
          //        txtObs.GetText()
          //    ];

          //    var camposVacios = campos.some(function (valor) {
          //        return valor === '' || valor === null;
          //    });

          //    if (camposVacios) {
          //        Swal.fire({
          //            title: "¡Alerta!",
          //            text: "Debe completar los campos Obligatorios",
          //            icon: "warning",
          //            confirmButtonColor: "#1F497D",
          //        }).then(function () {
          //            popupUpdateStatus.Show();  // Asegurar que el popup permanezca visible
          //        });
          //    } else {
          //        ASPxCallback_PopupUpdate.PerformCallback();
          //        GV_PreUsuarios.PerformCallback();
          //        popupUpdateStatus.Hide();
          //    }
          //}



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
            <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
            <%--<link href="Content/css/sweetalert2.min.css" rel="stylesheet" />
            <script src="Content/js/sweetalert2.all.min.js"></script>--%>
            <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11.15.10/dist/sweetalert2.all.min.js
            ">

            </script>
            <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11.15.10/dist/sweetalert2.min.css
            " rel="stylesheet">

            <script type="text/javascript">
                if (window.history.replaceState) {
                    window.history.replaceState(null, null, window.location.href);
                  }

                  //function ShowPopup() {
                  //    popupUpdateStatus.Show();
                  //}


          </script>
                <div>
                    <br />
            <h1 style= "color: #006699; background-color: #FFFFFF; text-align:center ">
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true">
                    <EmptyImage IconID="">
                    </EmptyImage>
                    </dx:ASPxImage>
                &nbsp;SOLICITUDES EN PROCESO 

            </h1>
                    </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">        
             <ContentTemplate>
    <dx:ASPxGridView ID="GV_PreUsuarios" runat="server" AutoGenerateColumns="False" Style="margin: auto;"
    ShowEditButton="True" KeyFieldName="Id" DataSourceID="SqlDataUsers" 
    OnBeforeExport="GV_PreUsuarios_BeforeExport" OnCustomCallback="GV_PreUsuarios_CustomCallback"
    Width="100%" CssClass="responsive-grid">
    
   
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true" />

    <SettingsPager AlwaysShowPager="True" PageSize="10">
        <PageSizeItemSettings Items="10, 30, 50" ShowAllItem="True" Visible="True" />
    </SettingsPager>


    <Settings ShowFilterRow="True" ShowHeaderFilterButton="True" ShowFooter="True" />
    <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" />
    <SettingsPopup>
        <FilterControl AutoUpdatePosition="False" />
    </SettingsPopup>
    <SettingsSearchPanel Visible="True" />
    <SettingsBehavior AllowFocusedRow="False" AllowSelectByRowClick="False" />


    <Toolbars>
        <dx:GridViewToolbar>
            <SettingsAdaptivity Enabled="true" EnableCollapseRootItemsToIcons="true" />
            <Items>
                <dx:GridViewToolbarItem Command="Custom">
                    <Template>
                        <dx:ASPxButton ID="btnUpdateStatus" runat="server" Text="Actualizar Estado" AutoPostBack="False">
                            <ClientSideEvents Click="function(s, e) { ShowPopup(); }" />
                        </dx:ASPxButton>
                    </Template>
                </dx:GridViewToolbarItem>
                <dx:GridViewToolbarItem Command="ExportToCsv" />
                <dx:GridViewToolbarItem Command="Refresh" />
            </Items>
        </dx:GridViewToolbar>
    </Toolbars>


    <Columns>
        <dx:GridViewCommandColumn ShowEditButton="True" VisibleIndex="0">
            <HeaderStyle HorizontalAlign="Center" />
            <CellStyle HorizontalAlign="Center" />
        </dx:GridViewCommandColumn>
        <dx:GridViewCommandColumn ShowSelectCheckbox="True" SelectAllCheckboxMode="AllPages" ShowClearFilterButton="True" VisibleIndex="1">
            <HeaderStyle HorizontalAlign="Center" Font-Bold="True" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewCommandColumn>
        <dx:GridViewDataTextColumn FieldName="FirstName" Caption="NOMBRE" VisibleIndex="2">
            <PropertiesTextEdit>
                <Style Font-Size="Small" />
            </PropertiesTextEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="LastName" Caption="APELLIDO" VisibleIndex="3">
            <PropertiesTextEdit>
                <Style Font-Size="Small" />
            </PropertiesTextEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Identidad" Caption="DNI" VisibleIndex="4">
            <PropertiesTextEdit>
                <Style Font-Size="Small" />
            </PropertiesTextEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Descripcion_Estado" Caption="ESTADO" VisibleIndex="5">
            <PropertiesTextEdit>
                <Style Font-Size="Small" />
            </PropertiesTextEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="FechaIngreso" Caption="FECHA LABORAL" VisibleIndex="6">
            <PropertiesDateEdit>
                <Style Font-Size="Small" />
            </PropertiesDateEdit>
            <HeaderStyle HorizontalAlign="Center" BackColor="#1F497D" Font-Bold="True" ForeColor="White" Font-Size="Small" />
            <CellStyle HorizontalAlign="Center" Font-Size="Small" />
        </dx:GridViewDataDateColumn>
    </Columns>


    <Styles>
        <SelectedRow BackColor="#CCCCCC" ForeColor="#1F497D" Font-Bold="True" />
        <FocusedRow BackColor="White" />
    </Styles>
</dx:ASPxGridView>

               

    </ContentTemplate>
        </asp:UpdatePanel>

          <asp:SqlDataSource runat="server" ID="SqlDataUsers" ConnectionString='<%$ ConnectionStrings:connString %>' SelectCommand="SELECT Id,Identidad,FirstName,LastName,
                email,
                Descripcion_Estado
                ,[FechaIngreso]
                ,[Id_Estado]
                ,[IdRole]
                FROM [V_Solicitudes] where id_Estado in (1,2,3) ORDER BY Id DESC"></asp:SqlDataSource>
   


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

