<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Parametros.aspx.cs" Inherits="SG_Constancia_TSC.Mantenimiento.Parametros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

</asp:Content>


<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <script type="text/javascript">
        function OnUploadComplete(s, e) {
            if (e.callbackData) {
             
                if (typeof imgPreview !== "undefined" && imgPreview != null) {
                    imgPreview.SetImageUrl(e.callbackData);
                }
            }
        }
    </script>
   
    <div>
        <h1 style="color: #006699; background-color: #FFFFFF; text-align: center">
            <dx:ASPxImage ID="ASPxImage1" runat="server" ShowLoadingImage="true">
            </dx:ASPxImage>
            <br />
            &nbsp;Parámetros de Firmas Adjuntas </h1>
        <br />
        <div>
            <dx:ASPxGridView ID="ParFirAdjunt" runat="server" KeyFieldName="codigo_parametro" AutoGenerateColumns="False" DataSourceID="SqlDataSourceParametro"
                Style="margin-left: auto; margin-right: auto;" OnRowInserted="ParFirAdjunt_RowInserted" 
                OnRowUpdated="ParFirAdjunt_RowUpdated" OnRowUpdating="ParFirAdjunt_RowUpdating" Theme="Office365" ClientInstanceName ="ParFirAdjunt">

                <SettingsAdaptivity>
                    <AdaptiveDetailLayoutProperties ColCount="1"></AdaptiveDetailLayoutProperties>
                </SettingsAdaptivity>
             
                <SettingsPager PageSize="5">
                </SettingsPager>

                <SettingsPager PageSize="10" />

                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" FileName="Parámetros_Firmas_Adjuntas" />

                 <ClientSideEvents EndCallback="function(s, e) {
                   if (s.cpUpdatedMessage) {
                   alert(s.cpUpdatedMessage);
                    delete s.cpUpdatedMessage;
                       }
                      }" />

                <SettingsEditing Mode="PopupEditForm">
                </SettingsEditing>
                <Settings ShowFilterRow="true" />

                <SettingsBehavior ConfirmDelete="True" EnableRowHotTrack="True" />
                <SettingsCommandButton>
                    <NewButton>
                        <Image IconID="actions_add_16x16">
                        </Image>
                    </NewButton>
                    <UpdateButton ButtonType="Button" RenderMode="Button">
                        <Image IconID="actions_apply_16x16">
                        </Image>
                        <Styles>
                            <Style BackColor="White" ForeColor="Black" Width="30px">
                                
                            </Style>
                        </Styles>
                    </UpdateButton>
                    <CancelButton ButtonType="Button" RenderMode="Button">
                        <Image IconID="actions_cancel_16x16">
                        </Image>
                        <Styles>
                            <Style BackColor="White" ForeColor="Black" Width="30px">
                           
                            </Style>
                        </Styles>
                    </CancelButton>
                    <EditButton>
                        <Image IconID="actions_editname_16x16">
                        </Image>
                    </EditButton>
                    <DeleteButton>
                        <Image IconID="actions_clear_16x16">
                        </Image>
                    </DeleteButton>
                </SettingsCommandButton>
                
                <SettingsDataSecurity AllowDelete="False" AllowInsert="True" /> 
                <SettingsPopup>
                    <EditForm Modal="True" HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter">
                    </EditForm>
                    <CustomizationWindow HorizontalAlign="WindowCenter" VerticalAlign="WindowCenter" />
                    <FilterControl AutoUpdatePosition="False">
                    </FilterControl>
                </SettingsPopup>

                <SettingsSearchPanel Visible="True" />

                <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" />

                <SettingsText PopupEditFormCaption="EDITAR PARÁMETROS" />
                <EditFormLayoutProperties ColCount="2">
                    <Items>
                        <dx:GridViewColumnLayoutItem ColumnName="valor" Caption="VALOR DEL PARÁMETRO" ColSpan="2" />
                        <dx:GridViewColumnLayoutItem ColumnName="descripcion" Caption="DESCRIPCIÓN" ColSpan="2" />
                        <dx:GridViewColumnLayoutItem ColumnName="UserId" Caption="NOMBRE DEL USUARIO" ColSpan="2" />


                        <dx:GridViewColumnLayoutItem Caption="FIRMA" ColSpan="2">
                            <Template>
                                <table>
                                    <tr>
                                        
                                        <td>
                                            <dx:ASPxUploadControl ID="UploadFirma" runat="server" 
                                                UploadMode="Auto"
                                                ShowProgressPanel="True"
                                                AutoStartUpload="true"
                                                ClientInstanceName="ucFirma"
                                                OnFileUploadComplete="UploadFirma_FileUploadComplete">
                                                <ValidationSettings AllowedFileExtensions=".jpg,.jpeg,.png" MaxFileSize="2097152" />
                                                <ClientSideEvents FileUploadComplete="OnUploadComplete" />
                                            </dx:ASPxUploadControl>

                                            <dx:ASPxImage ID="imgPreview" runat="server" Width="200px" Height="80px"  ClientInstanceName="imgPreview"/>
                                            <asp:HiddenField ID="hfFirmaBase64" runat="server"  />
                                        </td>
                                    </tr>
                                </table>
                            </Template>
                        </dx:GridViewColumnLayoutItem>
                        <dx:EditModeCommandLayoutItem ColSpan="2" HorizontalAlign="Center" VerticalAlign="Middle" />
                    </Items>
                </EditFormLayoutProperties>

                
                <Columns>
                    <dx:GridViewCommandColumn ShowEditButton="True" ShowClearFilterButton="True" ButtonRenderMode="Image"
                        ButtonType="Image" ShowInCustomizationForm="True" Caption="ACCIÓN" VisibleIndex="0">
                        <HeaderStyle BackColor="#1F497D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center" />
                    </dx:GridViewCommandColumn>

                    <dx:GridViewDataTextColumn FieldName="codigo_parametro" Caption="Código" VisibleIndex="1" Visible="False" ReadOnly="True">
                        <HeaderStyle BackColor="#1F497D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="nombre_parametro" VisibleIndex="2" Caption="NOMBRE DEL PARÁMETRO" Visible="False">
                        <HeaderStyle BackColor="#1F497D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="nombre_modulo" VisibleIndex="3" Visible="False" />

                    <dx:GridViewDataTextColumn FieldName="valor" VisibleIndex="4" Caption="VALOR DEL PARÁMETRO" PropertiesTextEdit-MaxLength="100">
                        <PropertiesTextEdit MaxLength="100" >
                            <ValidationSettings>
                                <RequiredField ErrorText="Por favor ingrese el valor del parámetro." IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <HeaderStyle BackColor="#1F497D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn FieldName="descripcion" VisibleIndex="5" Caption="DESCRIPCIÓN DEL PARÁMETRO">
                        <PropertiesTextEdit>
                            <ValidationSettings>
                                <RequiredField ErrorText="Por favor ingresar la descripción del parámetro." IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <HeaderStyle BackColor="#1F497D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataComboBoxColumn FieldName="UserId" Caption="USUARIO" Visible="False">
                        <EditFormSettings Visible="True" />
                        <PropertiesComboBox
                            DataSourceID="SqlUsuarios"
                            TextField="Nombre_Usuario"
                            ValueField="CodigoId"
                            ValueType="System.String"
                            DropDownStyle="DropDownList"
                            IncrementalFilteringMode="Contains"
                            NullText="Seleccione nombre del usuario">
                            <ValidationSettings>
                                <RequiredField ErrorText="" />
                            </ValidationSettings>
                        </PropertiesComboBox>
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataTextColumn FieldName="NombreUsuario" Caption="NOMBRE DE USUARIO" VisibleIndex="6" ReadOnly="True">
                        <HeaderStyle BackColor="#1F497D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <CellStyle HorizontalAlign="Center" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataBinaryImageColumn FieldName="firma" Caption="FIRMA" VisibleIndex="7">
                        <HeaderStyle BackColor="#1F497D" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                         <CellStyle HorizontalAlign="Center" />
                        <PropertiesBinaryImage ImageHeight="50px" ImageWidth="150px" />
                    </dx:GridViewDataBinaryImageColumn>

                </Columns>

                <Toolbars>
                    <dx:GridViewToolbar Enabled="true">
                        <Items>
                            <dx:GridViewToolbarItem Command="ExportToXls" Text="EXPORTAR A EXCEL">
                            </dx:GridViewToolbarItem>
                        </Items>
                    </dx:GridViewToolbar>
                </Toolbars>
                <Styles>
                    <RowHotTrack BackColor="#def7ff">
                    </RowHotTrack>

                    <AlternatingRow Enabled="True"></AlternatingRow>

                    <EditFormDisplayRow BackColor="#def7ff"></EditFormDisplayRow>

                </Styles>

                <Border BorderColor="Aqua" />
            </dx:ASPxGridView>

        </div>
        <asp:SqlDataSource ID="SqlUsuarios" runat="server"
            ConnectionString="<%$ ConnectionStrings:connString %>"
            SelectCommand="SELECT CodigoId, Nombre_Usuario FROM AspNetUsers ORDER BY Nombre_Usuario" />
        <asp:SqlDataSource ID="SqlDataSourceParametro" runat="server" 
            ConnectionString="<%$ ConnectionStrings:connString %>"
            OnUpdating="SqlDataSourceParametro_Updating"
            OnUpdated="SqlDataSourceParametro_Updated"
            SelectCommand="
    SELECT p.codigo_parametro,
           p.nombre_parametro,
           p.nombre_modulo,
           p.valor, 
           p.descripcion,
           p.UserId,
          u.Nombre_Usuario AS NombreUsuario,
        p.firma
    FROM gral.Parametros p
    LEFT JOIN AspNetUsers u ON u.CodigoId = p.UserId"
            UpdateCommand="
            UPDATE gral.Parametros
            SET valor = @valor,
                descripcion = @descripcion,
                firma = @firma,
                UserId = @UserId
            WHERE codigo_parametro = @codigo_parametro"
            InsertCommand="
    INSERT INTO gral.Parametros (codigo_parametro, nombre_parametro, nombre_modulo, valor, descripcion, UserId)
    VALUES (@codigo_parametro, @nombre_parametro, @nombre_modulo, @valor, @descripcion, @UserId)"
            DeleteCommand="
    DELETE FROM gral.Parametros
    WHERE codigo_parametro = @codigo_parametro">


            <InsertParameters>
                <asp:Parameter Name="codigo_parametro" Type="String" />
                <asp:Parameter Name="nombre_parametro" Type="String" />
                <asp:Parameter Name="nombre_modulo" Type="String" DefaultValue="FirmasEstadistica" />
                <asp:Parameter Name="valor" Type="String" />
                <asp:Parameter Name="descripcion" Type="String" />
                <asp:Parameter Name="UserId" Type="String" />
                <asp:Parameter Name="firma" Type="Object" />
            </InsertParameters>


            <UpdateParameters>
                <asp:Parameter Name="valor" Type="String" />
                <asp:Parameter Name="descripcion" Type="String" />
                <asp:Parameter Name="UserId" Type="String" />
                <asp:Parameter Name="codigo_parametro" Type="String" />
                <asp:Parameter Name="firma" Type="Object" />
            </UpdateParameters>

            <DeleteParameters>
                <asp:Parameter Name="codigo_parametro" Type="String" />
            </DeleteParameters>
        </asp:SqlDataSource>

    </div>

   
</asp:Content>
