using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DevExpress.Web;
using SG_Constancia_TSC.App_Start;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

using System.Text;

using SG_Constancia_TSC.Utili;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;

using DevExpress.Office.Internal;


using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.CodeParser;
using System.Web.Services;
using DevExpress.Pdf.Native;
using System.Drawing.Imaging;
using System.Drawing;
using ZXing.QrCode;
using ZXing;
using DevExpress.XtraPrinting.BarCode;
using SG_Constancia_TSC.Reportes;
using DevExpress.CodeParser.VB;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;
using System.Diagnostics;
using DevExpress.XtraReports.Web.WebDocumentViewer;
using DevExpress.XtraReports.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace SG_Constancia_TSC
{
    public partial class Autorizar_Constancia : System.Web.UI.Page
    {
        private bool IsBinding { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Attributes.Add("autocomplete", "off");

            if (!IsPostBack)
            {
                LoadStatuses();
                GV_PreUsuarios.FocusedRowIndex = -1; // Evita que se enfoque una fila al cargar la página

                cmbTipoFiltro.Value = "0";

                // 2) Ajusta el parámetro del SqlDataSource
                SqlDataUsers.SelectParameters["TipoSolicitud"].DefaultValue = "false";

                // 3) Rellena el grid
                GV_PreUsuarios.DataBind();

                // 4) Ajusta visibilidad de columnas
                GV_PreUsuarios.Columns["Identidad"].Visible = true;
                GV_PreUsuarios.Columns["FirstName"].Visible = true;
                GV_PreUsuarios.Columns["LastName"].Visible = true;
                GV_PreUsuarios.Columns["NumRtn"].Visible = false;
                GV_PreUsuarios.Columns["NomInstitucion"].Visible = false;

            }
        }


        protected void GV_PreUsuarios_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            if (e.Expanded)
            {
                ASPxGridView detailGrid = GV_PreUsuarios.FindDetailRowTemplateControl(e.VisibleIndex, "GV_Detalle") as ASPxGridView;
                if (detailGrid != null)
                {
                    int id = (int)GV_PreUsuarios.GetRowValues(e.VisibleIndex, "Id");
                    SqlDataDetalle.SelectParameters["Id"].DefaultValue = id.ToString();

                    // Desvincula temporalmente el evento DataBinding para evitar recursión
                    detailGrid.DataBinding -= GV_Detalle_DataBinding;
                    detailGrid.DataBind();
                    detailGrid.DataBinding += GV_Detalle_DataBinding;
                }
            }
        }

        protected void GV_Detalle_BeforePerformDataSelect(object sender, EventArgs e)
        {
            ASPxGridView detailGrid = sender as ASPxGridView;
            var container = detailGrid.NamingContainer as GridViewDetailRowTemplateContainer;

            if (container != null)
            {
                int id = (int)GV_PreUsuarios.GetRowValues(container.VisibleIndex, "Id");
                SqlDataDetalle.SelectParameters["Id"].DefaultValue = id.ToString();
            }
        }

        protected void GV_Detalle_DataBinding(object sender, EventArgs e)
        {
            ASPxGridView detailGrid = sender as ASPxGridView;
            var container = detailGrid.NamingContainer as GridViewDetailRowTemplateContainer;

            if (container != null && !IsBinding)
            {
                try
                {
                    IsBinding = true;
                    int id = (int)GV_PreUsuarios.GetRowValues(container.VisibleIndex, "Id");
                    SqlDataDetalle.SelectParameters["Id"].DefaultValue = id.ToString();
                    // Evita llamar a DataBind() directamente
                }
                finally
                {
                    IsBinding = false;
                }
            }
        }


        protected void SqlDataDetalle_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            if (GV_PreUsuarios.FocusedRowIndex >= 0)
            {
                // Obtén el valor del campo clave de la fila seleccionada
                object id = GV_PreUsuarios.GetRowValues(GV_PreUsuarios.FocusedRowIndex, "Id");
                e.Command.Parameters["@Id"].Value = id ?? DBNull.Value;
            }
            else
            {
                e.Command.Parameters["@Id"].Value = DBNull.Value; // Manejo de selección nula
            }
        }
        protected void hlPreviewFile_Init(object sender, EventArgs e)
        {
            ASPxHyperLink link = sender as ASPxHyperLink;
            GridViewDataItemTemplateContainer container = link.NamingContainer as GridViewDataItemTemplateContainer;

            if (container != null)
            {
                string uploadId = DataBinder.Eval(container.DataItem, "Upload_Id").ToString();
                string fileName = DataBinder.Eval(container.DataItem, "File_Name").ToString();

                // Asegurarse de que el nombre del archivo está bien escapado
                fileName = fileName.Replace("'", "\\'");

                // Definir el script correctamente
                string script = $"function(s, e) {{ ShowPreviewPopup({uploadId}, '{fileName}'); }}";

                // Asignar el evento de clic
                link.ClientSideEvents.Click = script;
            }
        }
        protected async void hyperLink_Init(object sender, EventArgs e)
        {
            var hyperLink = sender as DevExpress.Web.ASPxHyperLink;
            var container = hyperLink.NamingContainer as DevExpress.Web.GridViewDataItemTemplateContainer;

            string Upload_Id = container.Grid.GetRowValues(container.VisibleIndex, "Upload_Id")?.ToString();
            string url = await DownloadFile(Upload_Id); // Ahora sí se usa await

            hyperLink.Text = Upload_Id;
            hyperLink.NavigateUrl = !string.IsNullOrEmpty(url) ? url : "#";
            hyperLink.Enabled = !string.IsNullOrEmpty(url);
        }

        public static async Task<string> DownloadFile(string idFile)
        {
            try
            {
                using (var formContent = new MultipartFormDataContent())
                {
                    formContent.Add(new StringContent(idFile), "idFile");

                    using (HttpClient httpClient = Util.Util.getGoFilesUtlHeaders())
                    {
                        // Obtener la cadena de conexión desde el archivo de configuración
                        string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
                        string baseUrl = Util.Util.GetFinalGoFilesUtlUrl(Util.Util.obtenerAchivos);
                        string urlWithQuery = $"{baseUrl}?constring={Uri.EscapeDataString(connectionString)}";
                        httpClient.Timeout = TimeSpan.FromSeconds(30);

                        // Elimina el envío de la cadena de conexión por la URL
                        HttpResponseMessage httpResponse = await httpClient.PostAsync(baseUrl, formContent);

                        if (!httpResponse.IsSuccessStatusCode)
                            return null;

                        string responseContent = await httpResponse.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<CustomJsonResult>(responseContent);

                        // Asume que CustomJsonResult tiene una propiedad 'Url'
                        return (result?.typeResult == UtilClass.UtilClass.codigoExitoso) ? result.result?.ToString() : null;
                    }
                }
            }
            catch
            {
                return null; // Opcional: Loggear el error
            }
        }

        private void LoadStatuses()
        {
            string connectionString = (ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            string query = "SELECT Id_Estado, Descripcion_Estado FROM Estados WHERE Id_Estado IN (6)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                cmbStatus.DataSource = dataTable;
                cmbStatus.DataBind();
            }
        }
        protected void ASPxCallbackPanel_Report_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            int solicitudId;

            if (int.TryParse(e.Parameter, out solicitudId))
            {
                iframePDF.Attributes["src"] = "MostrarPdf.aspx?id=" + solicitudId;
                iframePDF.Visible = true;
            }


        }

        private string GetEmail(string solicitudId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            string getCodeQuery = "SELECT email FROM Solicitudes WHERE Id = @SolicitudId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(getCodeQuery, connection))
            {
                command.Parameters.AddWithValue("@SolicitudId", solicitudId);
                connection.Open();
                object result = command.ExecuteScalar();
                return result?.ToString();
            }
        }


        private void EnviarConstanciaAutorizada(string solicitudId, byte[] pdfBytes)
        {
            try
            {
                // Obtener correo del solicitante
                string adressEmail = GetEmail(solicitudId);

                string subject = "Constancia de no tener cuentas pendientes con el Estado de Honduras";

                string emailBody = @"
            <div style='font-family: Arial, text-align:center; Helvetica, sans-serif; font-size:14px; color:#333; line-height:1.6;'>
            <div style='max-width:600px; margin:0 auto; padding:20px; border:1px solid #ddd; border-radius:8px; background-color:#fafafa;'>
            
            <div style='text-align:center; margin-bottom:20px;'>
              <img src='https://dcioxh.stripocdn.email/content/guids/CABINET_b93ec2a38389d475174431c45f61c597b12b8d21784bda70816c8c0373b4ae6a/images/logo_tsc_2024_n0C.png' 
                   alt='Logo TSC' width='150' style='display:block; margin:auto;' />
            </div>
           <p style='text-align:center;'>Estimado(a) Usuario,</p>

          <p style='text-align:center;'>
          Le informamos que su solicitud de <b>Constancia de no tener cuentas pendientes con el Estado de Honduras</b> 
          ha sido <span style='color:black; font-weight:bold;'>finalizada</span>.
          </p>

        <p style='text-align:center;'>La constancia tiene validez por el término de seis meses.</p>
       <p style='text-align:center;'>Adjuntamos la constancia en formato <b>PDF</b>.</p>

       <p style='margin-top:20px; text-align:center;'>
      <b>Atentamente,</b><br/>
      Tribunal Superior de Cuentas<br/>
      Secretaría General
       </p>

      <hr style='margin:25px 0; border:none; border-top:1px solid #ccc;' />

        <p style='font-size:12px; color:#666; text-align:center;'>
         Este mensaje fue generado automáticamente por el Sistema de Solicitudes de Constancias en Línea.<br/>
          Por favor no responda este mensaje.
         </p>
         </div>
         </div>";

                using (MemoryStream ms = new MemoryStream(pdfBytes))
                {
                    SampleUtil.EnviarCorreo2(
                        ms,
                        Desde: "",  // opcional, se usa el del web.config
                        Subject: subject,
                        ToEmail: adressEmail,
                        Body: emailBody
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar constancia por correo", ex);
            }
        }


        private byte[] ObtenerFirma(int parametroId)
        {
            byte[] firmaBytes = null;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                try
                {

                    string query = "SELECT firma FROM gral.Parametros WHERE codigo_parametro = 1";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != DBNull.Value && result != null)
                        {
                            firmaBytes = (byte[])result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error al obtener la firma desde la base de datos", ex);
                }
            }

            return firmaBytes;
        }

        protected void ASPxReportCosntancia_Click(object sender, EventArgs e)
        {
            //natural
            int userID = Convert.ToInt32(Session["User_id"]);
            int tipo = Convert.ToInt32(cmbTipoFiltro.Value);
            if (cmbTipoFiltro.Value == null || cmbTipoFiltro.Value.ToString() == "0")
            {
                DataTable dt = GetFilteredDataFromGridView(tipo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var parametros = HttpContext.Current.Session["Parametros"] as Dictionary<string, Parametro>;

                    if (parametros == null)
                    {
                        throw new InvalidOperationException("Los parámetros no están disponibles en la sesión.");
                    }

                    string nombre = dt.Columns.Contains("nombre") ? dt.Rows[0]["nombre"].ToString() : "Desconocido";
                    string documento = dt.Columns.Contains("Identidad") ? dt.Rows[0]["Identidad"].ToString() : "N/A";
                    string solicitudId = dt.Rows[0]["Id"].ToString();

                    void OpenReport(string nombreParametro)
                    {
                        if (parametros.TryGetValue(nombreParametro, out Parametro parametro))
                        {
                            string valor = parametro.Valor ?? "Valor por defecto";
                            string descripcion = parametro.Descripcion ?? "Descripción por defecto";

                            
                            string textoFormateado = $@"
                             <div style='font-family:Arial, Helvetica, sans-serif; font-size:12pt;line-height:1.5;'>
                             <p style='margin:0 0 12pt 0; text-align:justify;'>
                             El Tribunal Superior de Cuentas a través de la Sección Constancias de no tener cuentas 
                             pendientes con el Estado, adscrita a la Secretaria General, por este medio <b>HACE
                             CONSTAR:</b> Que <b>{nombre}</b>, con Documento Nacional de Identificación <b>N°{documento}</b>, 
                             no tiene a la fecha ningún tipo de responsabilidad firme, ni existe ninguna intervención por presunción
                             de enriquecimiento ilícito en igual situación que le impida el desempeño de un cargo público.
                             </p>

                             <p style='margin:6pt 0 10pt 0; text-align:justify;'>
                             La presente constancia no constituye Solvencia con el Estado de Honduras, ni finiquito a favor del solicitante,
                             quedando sujeto a investigaciones futuras.
                             </p>

                              <p style='margin:0; text-align:center;'>
                              --La presente constancia tiene validez por el término de seis meses.--
                               </p>
                              </div>";

                            // 1. Obtener firma desde BD
                            int UserID = Convert.ToInt16(Session["User_id"]);
                            byte[] firmaBytes = ObtenerFirma(UserID); // parametro.Id = código_parametro en tu tabla

                            var report = new Reportes.Constancia(textoFormateado, solicitudId, firmaBytes);

                            report.Parameters["id"].Value = solicitudId;
                            report.Parameters["Firma"].Value = valor;
                            report.Parameters["Descripcion"].Value = descripcion;

                            string verificacionCode = GetCodigo_Verificacion(solicitudId);
                            report.Parameters["verificacion"].Value = verificacionCode ?? "Sin código";
                            report.Parameters["verificacion"].Visible = true;

                            byte[] pdfBytes;
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                report.ExportToPdf(memoryStream);
                                pdfBytes = memoryStream.ToArray();
                            }

                            GuardarConstanciaEnBD(solicitudId, pdfBytes, verificacionCode);
                            ActualizarEstado(solicitudId, 6, "Constancia generada y revisada.");
                            EnviarConstanciaAutorizada(solicitudId, pdfBytes);



                            string scriptBandeja = @"
                             Swal.fire({
                               icon: 'success',
                               title: '¡Éxito!',
                                   text: 'La constancia se ha enviado a la bandeja de Constancias Generadas.',
                                  confirmButtonColor: '#1F497D'
                                   });";
                            ScriptManager.RegisterStartupScript(this, GetType(), "bandejaGeneradas", scriptBandeja, true);
                            // Agregar el código QR al reporte
                            AgregarCodigoQRAlReporte(report, solicitudId);

                            ASPxWebDocumentViewer1.OpenReport(report);
                            ASPxWebDocumentViewer1.Visible = true;
                        }
                        else
                        {
                            throw new InvalidOperationException($"El parámetro '{nombreParametro}' no se encontró.");
                        }
                    }
                    string Userid = Session["User_id"]?.ToString();
                    OpenReport(Userid);
                    //string valorAdjunto = ASPxChkAdjunto.Value?.ToString();


                }
                else
                {
                    string script = @"
                    Swal.fire({
                        title: '¡Alerta!',
                        text: 'Por favor seleccione una fila para generar la constancia final.',
                        icon: 'warning',
                        confirmButtonColor: '#1F497D'
                    });";

                    ScriptManager.RegisterStartupScript(this, GetType(), "alertaSwal", script, true);
                }

            }

            else if (cmbTipoFiltro.Value.ToString() == "1") // Jurídica
            {
                DataTable dt = GetFilteredDataFromGridView(tipo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    var parametros = HttpContext.Current.Session["Parametros"] as Dictionary<string, Parametro>;

                    if (parametros == null)
                    {
                        throw new InvalidOperationException("Los parámetros no están disponibles en la sesión.");
                    }

                    string nombre = dt.Columns.Contains("NomInstitucion") ? dt.Rows[0]["NomInstitucion"].ToString() : "Desconocido";
                    string documento = dt.Columns.Contains("NumRtn") ? dt.Rows[0]["NumRtn"].ToString() : "N/A";
                    string solicitudId = dt.Rows[0]["Id"].ToString();

                    void OpenReport(string nombreParametro)
                    {
                        if (parametros.TryGetValue(nombreParametro, out Parametro parametro))
                        {
                            string valor = parametro.Valor ?? "Valor por defecto";
                            string descripcion = parametro.Descripcion ?? "Descripción por defecto";

                            
                            string textoFormateado = $@"
                            <div style='font-family:Arial, Helvetica, sans-serif; font-size:12pt;line-height:1.5;'>
                            <p style='margin:0 0 12pt 0; text-align:justify;'>
                            El Tribunal Superior de Cuentas a través de la Sección Constancias de no tener cuentas 
                            pendientes con el Estado, adscrita a la Secretaria General, por este medio <b>HACE
                            CONSTAR:</b> Que <b>{nombre}</b>, con RTN <b>N°{documento}</b>, no tiene a la 
                            fecha ningún tipo de responsabilidad civil o administrativa en carácter de firme, ante esta
                            institución.</p>
                           
                          <p style='margin:6pt 0 10pt 0; text-align:justify;'>
                          La presente constancia no constituye Solvencia con el Estado de Honduras, ni finiquito a favor del solicitante,
                          quedando sujeto a investigaciones futuras.
                          </p>

                          <p style='margin:0; text-align:center;'>
                                --La presente constancia tiene validez por el término de seis meses.--
                              </p>
                              </div>";


                            // 1. Obtener firma desde BD
                            int UserID = Convert.ToInt16(Session["User_id"]);
                            byte[] firmaBytes = ObtenerFirma(UserID); // parametro.Id = código_parametro en tu tabla
                            var report = new Reportes.Constancia(textoFormateado, solicitudId, firmaBytes); report.Parameters["id"].Value = solicitudId;

                            report.Parameters["Firma"].Value = valor;
                            report.Parameters["Descripcion"].Value = descripcion;

                            string verificacionCode = GetCodigo_Verificacion(solicitudId);
                            report.Parameters["verificacion"].Value = verificacionCode ?? "Sin código";
                            report.Parameters["verificacion"].Visible = true;

                            byte[] pdfBytes;
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                report.ExportToPdf(memoryStream);
                                pdfBytes = memoryStream.ToArray();
                            }

                            GuardarConstanciaEnBD(solicitudId, pdfBytes, verificacionCode);
                            ActualizarEstado(solicitudId, 6, "Constancia generada y revisada.");
                            EnviarConstanciaAutorizada(solicitudId, pdfBytes);

                            string scriptBandeja = @"
                             Swal.fire({
                               icon: 'success',
                               title: '¡Éxito!',
                                   text: 'La constancia se ha enviado a la bandeja de Constancias Generadas.',
                                  confirmButtonColor: '#1F497D'
                                   });";
                            ScriptManager.RegisterStartupScript(this, GetType(), "bandejaGeneradas", scriptBandeja, true);
                            // Agregar el código QR al reporte
                            AgregarCodigoQRAlReporte(report, solicitudId);

                            ASPxWebDocumentViewer1.OpenReport(report);
                            ASPxWebDocumentViewer1.Visible = true;
                        }
                        else
                        {
                            throw new InvalidOperationException($"El parámetro '{nombreParametro}' no se encontró.");
                        }
                    }


                    string Userid = Session["User_id"]?.ToString();
                    OpenReport(Userid);
                    //string valorAdjunto = ASPxChkAdjunto.Value?.ToString();


                }
                else
                {
                    string script = @"
                    Swal.fire({
                        title: '¡Alerta!',
                        text: 'Por favor seleccione una fila para generar la constancia final.',
                        icon: 'warning',
                        confirmButtonColor: '#1F497D'
                    });";

                    ScriptManager.RegisterStartupScript(this, GetType(), "alertaSwal", script, true);
                }
            }
        }
        protected void ActualizarEstado(string solicitudId, int estado, string observacion)
        {
            // Ejecuta el procedimiento almacenado para actualizar el estado y la observación
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            int userID = Convert.ToInt32(Session["User_id"]);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("[gral].[sp_gestion_estado]", connection))

                {
                    int UserID = Convert.ToInt16(Session["User_id"]);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IDUser", SqlDbType.Int).Value = UserID;
                    command.Parameters.AddWithValue("@IDs", solicitudId);
                    command.Parameters.AddWithValue("@Estado", estado);
                    command.Parameters.AddWithValue("@Obs", observacion);
                    command.Parameters.AddWithValue("@pcUsuarioModificaId", userID);
                    SqlParameter mensParam = new SqlParameter("@MENS", SqlDbType.NVarChar, -1)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter retornoParam = new SqlParameter("@RETORNO", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(mensParam);
                    command.Parameters.Add(retornoParam);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    string mensaje = mensParam.Value.ToString();
                    int retorno = Convert.ToInt32(retornoParam.Value);

                    // Opcional: Lógica para manejar el mensaje y retorno
                    if (retorno == 1)
                    {
                        // Éxito
                        GV_PreUsuarios.DataBind();

                    }
                    else
                    {
                        // Error
                        GV_PreUsuarios.DataBind();
                    }
                }
            }
        }
        protected void btnGuardaConstancia_Click(object sender, EventArgs e)
        {


        }

        private string GetCodigo_Verificacion(string solicitudId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            string getCodeQuery = "SELECT TOP 1 CodigoVerificacion FROM Constancias WHERE SolicitudId = @SolicitudId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(getCodeQuery, connection))
            {
                command.Parameters.AddWithValue("@SolicitudId", solicitudId);
                connection.Open();
                object result = command.ExecuteScalar();
                return result?.ToString();
            }
        }

        private void GuardarConstanciaEnBD(string solicitudId, byte[] archivoConstancia, string codigoVerificacion)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            int userID = Convert.ToInt32(Session["User_id"]);

            if (ConstanciaYaGenerada(solicitudId))
                return;

            string insertQuery = @"
        INSERT INTO ConstanciasEntregadas (
            Clave, Estado, FechaCreacion, Observaciones, SolicitudId, CodigoVerificacion, Archivoconstancia
        ) VALUES (
            @Clave, @Estado, @FechaCreacion, @Observaciones, @SolicitudId, @CodigoVerificacion, @Archivoconstancia
        )";

            string updateQuery = @"
        UPDATE Constancias
        SET ArchivoConstanciaSecretaria = @Archivoconstancia, Estado = 3, Observaciones = 'Constancia fue generada y revisada , proceda a descargar'
        WHERE SolicitudId = @SolicitudId";
            try
            {
                    using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.Add("@Clave", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                    insertCommand.Parameters.Add("@Estado", SqlDbType.NVarChar).Value = "Finalizada";
                    insertCommand.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = DateTime.Now;
                    insertCommand.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = "Constancia Final generada automáticamente";
                    insertCommand.Parameters.Add("@SolicitudId", SqlDbType.NVarChar).Value = solicitudId;
                    insertCommand.Parameters.Add("@CodigoVerificacion", SqlDbType.NVarChar).Value = codigoVerificacion ?? "N/A";
                    insertCommand.Parameters.Add("@Archivoconstancia", SqlDbType.VarBinary).Value = archivoConstancia;

                    insertCommand.ExecuteNonQuery();
                }

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.Add("@Archivoconstancia", SqlDbType.VarBinary).Value = archivoConstancia;
                    updateCommand.Parameters.Add("@SolicitudId", SqlDbType.NVarChar).Value = solicitudId;

                    updateCommand.ExecuteNonQuery();
                }
            }
            }
            catch (SqlException sqlEx)
            {
                RegistrarErrorEnBitacora(userID, "GuardarConstanciaEnBD", $"Error SQL: {sqlEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                RegistrarErrorEnBitacora(userID, "GuardarConstanciaEnBD", $"Error general: {ex.Message}");
                throw;
            }
        }

        private void RegistrarErrorEnBitacora(int userId, string componente, string descripcion)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                INSERT INTO [gral].[Bitacora_Errores] 
                (codigo_usuario, fecha_hora, componente, descripcion)
                VALUES (@codigo_usuario, @fecha_hora, @componente, @descripcion)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.Add("@codigo_usuario", SqlDbType.Int).Value = userId;
                        cmd.Parameters.Add("@fecha_hora", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@componente", SqlDbType.VarChar, 250).Value = componente;
                        cmd.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = descripcion;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                // ⚠️ Importante: nunca relanzar aquí para evitar bucles de error
                // Si falla el registro en bitácora, lo ignoramos
            }
        }
        public static string GenerateQRCode(string encryptedConstanciaId)
        {
            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 180,
                    Width = 180,
                    Margin = 1
                }
            };

            //var pixelData = qrWriter.Write("https://xxxxx/Solicitud.aspx?constanciaid=" + encryptedConstanciaId);
            var pixelData = qrWriter.Write("http://localhost:59458/Solicitud.aspx?constanciaid=" + encryptedConstanciaId);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppArgb))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                 ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                try
                {
                    // Transfer the image data to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                // Save to memory stream as PNG
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return "data:image/png;base64," + base64String;
            }
        }

        protected void GV_PreUsuarios_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void GV_PreUsuarios_CustomCallback1(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {

        }

        protected void GV_PreUsuarios_BeforeExport(object sender, ASPxGridBeforeExportEventArgs e)
        {

        }

        protected void ASPxCallback_PopupUpdate_Callback(object source, CallbackEventArgs e)
        {

        }


        // eventos propios
        private bool ConstanciaYaGenerada(string solicitudId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT 1 FROM ConstanciasEntregadas WHERE SolicitudId = @SolicitudId", conn))
            {
                cmd.Parameters.AddWithValue("@SolicitudId", solicitudId);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public static string GetSessionValues(string constanciaId)
        {
            try
            {
                string estado = string.Empty;
                string fechaCreacion = string.Empty;
                string otrosDatos = string.Empty;
                string archivoconstanciaBase64 = string.Empty; // Archivo en Base64

                string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
                string query = @"
            SELECT c.FechaCreacion, 
                   c.Observaciones, 
                   e.Descripcion_Estado,
                   c.Archivoconstancia
            FROM Constancias c
            INNER JOIN Estados e ON c.Estado = e.Id_Estado
            WHERE c.SolicitudId = @ConstanciaId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ConstanciaId", constanciaId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        estado = reader["Descripcion_Estado"].ToString();
                        fechaCreacion = reader["FechaCreacion"].ToString();
                        otrosDatos = reader["Observaciones"].ToString();

                        // Si hay un archivo de constancia, convertirlo a Base64
                        if (reader["Archivoconstancia"] != DBNull.Value)
                        {
                            byte[] archivoconstancia = (byte[])reader["Archivoconstancia"];
                            archivoconstanciaBase64 = Convert.ToBase64String(archivoconstancia);
                        }
                    }
                }

                return $"{constanciaId}|{estado}|{fechaCreacion}|{otrosDatos}|{archivoconstanciaBase64}";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private DataTable GetFilteredDataFromGridView(int tipo)
        {
            if (tipo == 0) // Natural
            {

                // Crear y definir el DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("FirstName", typeof(string));
                dt.Columns.Add("LastName", typeof(string));
                dt.Columns.Add("Identidad", typeof(string));
                dt.Columns.Add("nombre", typeof(string)); // Columna para el nombre completo
                dt.Columns.Add("Id", typeof(string)); // Columna para el nombre completo

                // Obtener el índice de la fila enfocada (seleccionada)
                int selectedRowIndex = GV_PreUsuarios.FocusedRowIndex;
                if (selectedRowIndex >= 0)
                {
                    // Obtener los valores de la fila seleccionada
                    object[] values = GV_PreUsuarios.GetRowValues(
                        selectedRowIndex,
                        new string[] { "FirstName", "LastName", "Identidad", "Id" }
                    ) as object[];

                    if (values != null)
                    {
                        DataRow dr = dt.NewRow();
                        dr["FirstName"] = values[0];
                        dr["LastName"] = values[1];
                        dr["Identidad"] = values[2];
                        // Combina FirstName y LastName, agregando un espacio entre ellos
                        dr["nombre"] = values[0].ToString() + " " + values[1].ToString();
                        dr["Id"] = values[3];
                        dt.Rows.Add(dr);
                    }
                }
                // Si no hay fila seleccionada, el DataTable quedará vacío
                return dt;


            }
            else if (tipo == 1) // Jurídica
            {
                // Crear y definir el DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("FirstName", typeof(string));
                dt.Columns.Add("LastName", typeof(string));
                dt.Columns.Add("NumRtn", typeof(string));
                dt.Columns.Add("NomInstitucion", typeof(string)); // Columna para el nombre completo
                dt.Columns.Add("Id", typeof(string)); // Columna para el nombre completo

                // Obtener el índice de la fila enfocada (seleccionada)
                int selectedRowIndex = GV_PreUsuarios.FocusedRowIndex;
                if (selectedRowIndex >= 0)
                {
                    // Obtener los valores de la fila seleccionada
                    object[] values = GV_PreUsuarios.GetRowValues(
                        selectedRowIndex,
                        new string[] { "FirstName", "LastName", "NumRtn", "Id", "NomInstitucion" }
                    ) as object[];

                    if (values != null)
                    {
                        DataRow dr = dt.NewRow();
                        dr["FirstName"] = values[0];
                        dr["LastName"] = values[1];
                        dr["NumRtn"] = values[2];
                        // Combina FirstName y LastName, agregando un espacio entre ellos
                        dr["NomInstitucion"] = values[4].ToString();
                        dr["Id"] = values[3];
                        dt.Rows.Add(dr);
                    }
                }
                // Si no hay fila seleccionada, el DataTable quedará vacío
                return dt;
            }
            else
            {
                throw new ArgumentException("Tipo de filtro no válido");
            }
        }

        protected void AgregarCodigoQRAlReporte(Reportes.Constancia report, string encryptedId)
        {
            XRBarCode xrBarCode = new XRBarCode
            {
                Symbology = new QRCodeGenerator
                {
                    CompactionMode = QRCodeCompactionMode.Byte,
                    ErrorCorrectionLevel = QRCodeErrorCorrectionLevel.H,
                    Version = QRCodeVersion.Version4
                },
                Text = "https://constancia.tsc.gob.hn/SolicitudUsuario.aspx?id=" + encryptedId,
                ShowText = false,
                AutoModule = true,
                WidthF = 176.46f,
                HeightF = 179.58f,
                LocationF = new PointF(235.42f, 19.79f),
                Module = 2,
                Padding = new PaddingInfo(10, 10, 10, 10)
            };
            report.Bands["GroupFooter1"].Controls.Add(xrBarCode);
        }

        protected void cmbTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {

            bool isNatural = (cmbTipoFiltro.Value as string) == "0";

            // 1) Ajusto el parámetro del SqlDataSource
            SqlDataUsers.SelectParameters["TipoSolicitud"].DefaultValue = isNatural
                ? "false"
                : "true";

            // 2) Re-bindeo el grid
            GV_PreUsuarios.DataBind();

            // 3) Muestro/oculto columnas
            GV_PreUsuarios.Columns["Identidad"].Visible = isNatural;
            GV_PreUsuarios.Columns["FirstName"].Visible = isNatural;
            GV_PreUsuarios.Columns["LastName"].Visible = isNatural;
            GV_PreUsuarios.Columns["NumRtn"].Visible = !isNatural;
            GV_PreUsuarios.Columns["NomInstitucion"].Visible = !isNatural;


        }
    }
}


