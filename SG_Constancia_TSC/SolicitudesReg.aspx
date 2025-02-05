<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Main.master" CodeBehind="SolicitudesReg.aspx.cs" Inherits="SG_Constancia_TSC.SolicitudesReg"  Async="true" %>

<%@ Register assembly="DevExpress.XtraReports.v21.2.Web.WebForms, Version=21.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>

                 
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style>
        /* Manejo del z-index para evitar conflictos de superposición */
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
        <!-- SweetAlert2 CDN -->
        <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
        <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11.15.10/dist/sweetalert2.min.css" rel="stylesheet">

        <!-- Prevenir el reenvío de formularios al recargar la página -->
        <script type="text/javascript">
            if (window.history.replaceState) {
                window.history.replaceState(null, null, window.location.href);
            }

            function OnRowFocused(s, e) {
                var rowIndex = s.GetFocusedRowIndex();
                if (rowIndex !== -1) {
                    var key = s.GetRowKey(rowIndex);
                    console.log("ID seleccionado:", key);
                    // Aquí puedes realizar acciones adicionales, como actualizar otro control
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
                }
            }
        </script>

        <div>
            <h1 style="color: #006699; text-align: center;">
                <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true" />
                &nbsp;SOLICITUDES EN PROCESO
            </h1>
        </div>

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <dx:ASPxGridView ID="GV_PreUsuarios" runat="server" AutoGenerateColumns="False" KeyFieldName="Id"
                    DataSourceID="SqlDataUsers" Width="100%" CssClass="responsive-grid"  OnDetailRowExpandedChanged="GV_PreUsuarios_DetailRowExpandedChanged">
            
       
                    <SettingsBehavior AllowSelectByRowClick="true" AllowFocusedRow="true" />
                    <ClientSideEvents FocusedRowChanged="OnRowFocused" />

                 
                    <SettingsDetail ShowDetailRow="true" />
                    <Templates>
                        <DetailRow>
                            <dx:ASPxGridView ID="GV_Detalle" runat="server" AutoGenerateColumns="False" Width="100%" KeyFieldName="DetailId" DataSourceID="SqlDataDetalle" CssClass="custom-grid">
                                <SettingsPager PageSize="10" />
                                <SettingsBehavior AllowSort="true" AllowFocusedRow="true" />
                                <Styles>
                                    <Header CssClass="custom-grid-header" />
                                    <Row CssClass="custom-grid-row" />
                                    <SelectedRow CssClass="custom-grid-selected-row" />
                                </Styles>
                                <Columns>
                                     <dx:GridViewDataTextColumn FieldName="Upload_Id" Caption="Codigo" Visible ="false"  />
<%--                                    <dx:GridViewDataTextColumn FieldName="FirstName" Caption="Primer nombre" />
                                    <dx:GridViewDataTextColumn FieldName="LastName" Caption="Primer Apellido" />--%>
                                    <dx:GridViewDataTextColumn FieldName="File_Name" Caption="Nombre de archivo"/> 
                                    <dx:GridViewDataTextColumn FieldName="File_Name" Caption="Descargar o Ver"> 
                                        <DataItemTemplate>
                                            <dx:ASPxHyperLink 
                                                ID="hlDownload" 
                                                runat="server" 
                                                Text="Descargar"
                                                NavigateUrl='<%# String.Format("~/FileDownload.ashx?Upload_Id={0}&mode=download", Eval("Upload_Id")) %>' 
                                            />
                                            <dx:ASPxHyperLink 
                                                ID="hlView" 
                                                runat="server" 
                                                Text="Ver"
                                                Target="_blank"
                                                NavigateUrl='<%# String.Format("~/FileDownload.ashx?Upload_Id={0}&mode=view", Eval("Upload_Id")) %>' 
                                            />
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
<%--                                    <dx:GridViewDataTextColumn FieldName="File_Name" Caption="visualizar pantalla">
                                        <DataItemTemplate>
                                            <dx:ASPxHyperLink 
                                                ID="hlPreviewFile" 
                                                runat="server" 
                                                NavigateUrl="javascript:void(0);"
                                                Text='<%# Eval("File_Name") %>'
                                                OnInit="hlPreviewFile_Init" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn> --%>

                                </Columns>
                            </dx:ASPxGridView>
                        </DetailRow>
                    </Templates>

                   
                    <Columns>
                         
                        <dx:GridViewDataTextColumn FieldName="FirstName" Caption="NOMBRE" />
                        <dx:GridViewDataTextColumn FieldName="LastName" Caption="APELLIDO" />
                        <dx:GridViewDataTextColumn FieldName="Identidad" Caption="DNI" />
                        <dx:GridViewDataTextColumn FieldName="Descripcion_Estado" Caption="ESTADO" />
                        <dx:GridViewDataDateColumn FieldName="FechaIngreso" Caption="FECHA LABORAL" />
                    </Columns>
                </dx:ASPxGridView>
                <dx:ASPxPopupControl ID="PopupArchivo" runat="server" ShowHeader="True" 
                    HeaderText="Vista Previa de Archivo" Width="800px" Height="600px"
                    Modal="True" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ClientInstanceName="PopupArchivo">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <!-- Visor de Documentos -->
<%--                            <dx:ASPxWebDocumentViewer ID="DocViewer" runat="server" Width="100%" Height="100%" 
                                ClientInstanceName="docViewer" Visible="false" />--%>
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
                    ConnectionString='<%$ ConnectionStrings:connString %>' 
                    SelectCommand="SELECT DISTINCT Id, Identidad, FirstName, LastName, email, [Descripcion_Estado], 
                                    [FechaIngreso], [Id_Estado], [IdRole] FROM [dbo].[V_Solicitudes] 
                                    WHERE Id_Estado IN (1, 2, 3) ORDER BY Id DESC">
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="SqlDataDetalle" runat="server" 
                    ConnectionString='<%$ ConnectionStrings:connString %>' 
                    SelectCommand="SELECT Upload_Id, Id, Identidad, FirstName, LastName, [File_Name] FROM [dbo].[V_Solicitudes] WHERE Id = @Id">
                    <SelectParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
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

    <script>
        

        function btnBeforeExport() {
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