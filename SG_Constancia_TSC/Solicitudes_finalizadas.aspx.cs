using DevExpress.Web;
using SG_Constancia_TSC.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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
using DevExpress.Utils;
using SG_Constancia_TSC.Mantenimiento;

namespace SG_Constancia_TSC
{
    public partial class Solicitudes_finalizadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Attributes.Add("autocomplete", "off");

            if (!IsPostBack)
            {
                LoadStatuses();
                GV_PreUsuarios.FocusedRowIndex = -1;

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

                string userId = Session["Name_user"]?.ToString();
                if (!string.IsNullOrEmpty(userId))
                {
                    string base64UserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(userId));
                    ASPxRblAdjunto.Items.Add(userId, userId);
                }
            }

        }


        private void LoadStatuses()
        {
            string connectionString = (ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            string query = "SELECT Id_Estado, Descripcion_Estado FROM Estados WHERE Id_Estado IN (4)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                cmbStatus.DataSource = dataTable;
                cmbStatus.DataBind();
            }
        }



        protected void ASPxCallback_PopupUpdate_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            try
            {
                int userID = Convert.ToInt32(Session["User_id"] ?? "0");

                // 1) Id de la fila seleccionada
                int index = GV_PreUsuarios.FocusedRowIndex;
                if (index < 0) { e.Result = "ERROR|Por favor seleccione una fila antes de actualizar el estado.|"; return; }

                object idObj = GV_PreUsuarios.GetRowValues(index, "Id");
                if (idObj == null) { e.Result = "ERROR|No se pudo obtener el Id de la solicitud.|"; return; }
                string selectedID = idObj.ToString();

                // 2) Estado y observación
                var sel = cmbStatus.SelectedItem;
                if (sel == null) { e.Result = "ERROR|Seleccione el estado.|"; return; }

                if (!int.TryParse(Convert.ToString(sel.Value), out int estadoInt))
                {
                    e.Result = "ERROR|Seleccione un estado válido.|"; return;
                }
                string Desc_estado = sel.Text ?? "";
                string observacion = txtObs.Text ?? "";

                // 3) Existir constancia previa en la BD
                if (!ConstanciaYaGenerada(selectedID))
                {
                    e.Result = "ERROR|Debe generar la constancia previa antes de actualizar el estado.|";
                    return;
                }

                // 4) Ejecutar SP
                string cs = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(cs))
                using (SqlCommand command = new SqlCommand("[gral].[sp_gestion_estado]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                  
                    command.Parameters.Add("@IDUser", SqlDbType.Int).Value = userID;
                    command.Parameters.Add("@IDs", SqlDbType.NVarChar, 50).Value = selectedID; 
                    command.Parameters.Add("@Estado", SqlDbType.Int).Value = estadoInt;
                    command.Parameters.Add("@Obs", SqlDbType.NVarChar, -1).Value = (object)observacion ?? DBNull.Value;
                    command.Parameters.Add("@pcUsuarioModificaId", SqlDbType.Int).Value = userID;

                    var mensParam = new SqlParameter("@MENS", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    var retornoParam = new SqlParameter("@RETORNO", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(mensParam);
                    command.Parameters.Add(retornoParam);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    string mensaje = Convert.ToString(mensParam.Value);
                    int retorno = Convert.ToInt32(retornoParam.Value);

                    if (retorno == 1)
                    {
                        GV_PreUsuarios.DataBind();
                        e.Result = "OK|La solicitud se ha enviado a la bandeja de Autorizar Constancias.|CLEAR";

                        string adressEmail = SampleUtil.GetEmail(selectedID);
                        string subject = "Actualización del estado de su solicitud de constancia";
                        string emailBody = @"
                                <!doctype html>
                                  <html><head><meta charset='utf-8'><title>Estado de solicitud</title></head>
                                  <body style='margin:0;padding:0;background-color:#f5f5f5; font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#333; line-height:1.6;'>
                                  <div style='max-width:600px; margin:0 auto; padding:20px;'>
                                  <div style='border:1px solid #ddd; border-radius:8px; background-color:#fafafa; padding:20px;'>
                                  <div style='text-align:center; margin-bottom:20px;'>
                                  <img src='https://dcioxh.stripocdn.email/content/guids/CABINET_b93ec2a38389d475174431c45f61c597b12b8d21784bda70816c8c0373b4ae6a/images/logo_tsc_2024_n0C.png' alt='Logo TSC' width='150' style='display:block; margin:0 auto;' />
                                  </div>
                                  <p style='text-align:center; margin:0 0 12px 0;'>Estimado(a) Usuario,</p>
                                  <p style='text-align:center; margin:0 0 12px 0;'>
                                  Le informamos que el estado de su solicitud de
                                  <b>Constancia de no tener cuentas pendientes con el Estado de Honduras</b>
                                  ha sido actualizado a: <b>" + Desc_estado + @"</b>.
                                  </p>
                                  <p style='text-align:center; margin:0 0 12px 0;'>
                                  Para más detalles, por favor ingrese al siguiente enlace:
                                  <a href='https://consta-sec-dev.tsc.gob.hn:8011/Seguimiento.aspx' target='_blank' style='color:#1F497D; font-weight:bold; text-decoration:underline;'>Seguimiento de Solicitud</a>.
                                  </p>
                                  <p style='margin-top:20px; text-align:center;'><b>Atentamente,</b><br/>Tribunal Superior de Cuentas<br/>Secretaría General</p>
                                  <hr style='margin:24px 0; border:none; border-top:1px solid #ccc;' />
                                  <p style='font-size:12px; color:#666; text-align:center; margin:0;'>
                                  Este mensaje fue generado automáticamente por el Sistema de Solicitudes de Constancias en Línea.<br/>Por favor, no responda este mensaje.
                                  </p>
                                </div></div></body></html>";
                        SampleUtil.EnviarCorreo1("", subject, adressEmail, emailBody);
                    }
                    else
                    {
                        GV_PreUsuarios.DataBind();
                        e.Result = $"ERROR|{mensaje}";
                    }
                }
            }
            catch (Exception)
            {
                e.Result = "ERROR|No se pudo actualizar el estado. Inténtelo de nuevo.|";
            }
        }



        private DataTable GetData()
        {
            DataTable dataTable = new DataTable();


            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM solicitudes ";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                    Console.WriteLine("Error obteniendo datos: " + ex.Message);
                }
            }

            return dataTable;
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
        private byte[] ObtenerFirma(int parametroId)
        {
            byte[] firmaBytes = null;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(
                        "SELECT firma FROM gral.Parametros WHERE UserId = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", parametroId);
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
                    // Aquí puedes registrar el error o mostrarlo
                    throw new ApplicationException("Error al obtener la firma desde la base de datos", ex);
                }
            }

            return firmaBytes;
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


        protected void ASPxReportCosntancia_Click(object sender, EventArgs e)
        {


            //natural
            int tipo = Convert.ToInt32(cmbTipoFiltro.Value);
            if (cmbTipoFiltro.Value == null || cmbTipoFiltro.Value.ToString() == "0")
            {
                DataTable dt = GetFilteredDataFromGridView(tipo);
                if (dt != null && dt.Rows.Count > 0)
                {

                    var firmaSeleccion = ASPxRblAdjunto.Value as string;
                    if (string.IsNullOrWhiteSpace(firmaSeleccion))
                    {
                        ScriptManager.RegisterStartupScript(
                            this, this.GetType(), "swalFirmaRequeridaGen_N",
                            @"Swal.fire({
                        icon: 'warning',
                        title: 'Firma Adjunta Requerida',
                        text: 'Debe seleccionar la opción de firma adjunta antes de generar la constancia previa.',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Aceptar'
                    });",
                            true
                        );
                        return;
                    }

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
                    string valorAdjunto = ASPxRblAdjunto.Value?.ToString();


                }
                else
                {
                    string script = @"
                Swal.fire({
                    title: '¡Alerta!',
                    text: 'Por favor seleccione una fila para generar la constancia previa.',
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

                    var firmaSeleccion = ASPxRblAdjunto.Value as string;
                    if (string.IsNullOrWhiteSpace(firmaSeleccion))
                    {
                        ScriptManager.RegisterStartupScript(
                            this, this.GetType(), "swalFirmaRequeridaGen_J",
                            @"Swal.fire({
                        icon: 'warning',
                        title: 'Firma Adjunta Requerida',
                        text: 'Debe seleccionar la opción de firma adjunta antes de generar la constancia previa.',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Aceptar'
                    });",
                            true
                        );
                        return;
                    }

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

                            // Solo declaramos una vez la variable
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
                    string valorAdjunto = ASPxRblAdjunto.Value?.ToString();


                }
                else
                {
                    string script = @"
                Swal.fire({
                    title: '¡Alerta!',
                    text: 'Por favor seleccione una fila para generar la constancia previa.',
                    icon: 'warning',
                    confirmButtonColor: '#1F497D'
                });";

                    ScriptManager.RegisterStartupScript(this, GetType(), "alertaSwal", script, true);
                }
            }
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

        

        private bool ConstanciaYaGenerada(string solicitudId)
        {
            string cs = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(@"
        IF EXISTS (
            SELECT 1
            FROM dbo.ConstanciasGeneradas WITH (NOLOCK)
            WHERE SolicitudId = @SolicitudId
              AND Archivoconstancia IS NOT NULL
        ) SELECT 1 ELSE SELECT 0;", conn))
            {
                cmd.Parameters.Add("@SolicitudId", SqlDbType.NVarChar).Value = solicitudId;
                conn.Open();
                var r = cmd.ExecuteScalar();
                return r != null && Convert.ToInt32(r) == 1;
            }
        }

        private void GuardarConstanciaEnBD(string solicitudId, byte[] archivoConstancia, string codigoVerificacion)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            int userID = Convert.ToInt32(Session["User_id"]);

            if (string.IsNullOrEmpty(solicitudId))
                throw new ArgumentException("El parámetro solicitudId no puede ser nulo o vacío.");

            if (archivoConstancia == null || archivoConstancia.Length == 0)
                throw new ArgumentException("El archivoConstancia no puede estar vacío.");

            if (ConstanciaYaGenerada(solicitudId))
                return;

            string insertQuery = @"
        INSERT INTO ConstanciasGeneradas (
            Clave, Estado, FechaCreacion, Observaciones, SolicitudId, CodigoVerificacion, Archivoconstancia
        ) VALUES (
            @Clave, @Estado, @FechaCreacion, @Observaciones, @SolicitudId, @CodigoVerificacion, @Archivoconstancia
        )";

            string updateQuery = @"
        UPDATE Constancias
        SET Archivoconstancia = @Archivoconstancia, 
            Estado = 4, 
            Observaciones = 'Constancia fue generada',
            UsuarioModificacionId = @UsuarioModificacionId, 
            fechaModificacion = @fechaModificacion
        WHERE SolicitudId = @SolicitudId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Insertar en ConstanciasGeneradas
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.Add("@Clave", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                        insertCommand.Parameters.Add("@Estado", SqlDbType.NVarChar).Value = "Generada";
                        insertCommand.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = DateTime.Now;
                        insertCommand.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = "Constancia previa generada automáticamente";
                        insertCommand.Parameters.Add("@SolicitudId", SqlDbType.NVarChar).Value = solicitudId;
                        insertCommand.Parameters.Add("@CodigoVerificacion", SqlDbType.NVarChar).Value = codigoVerificacion ?? "N/A";
                        insertCommand.Parameters.Add("@Archivoconstancia", SqlDbType.VarBinary).Value = archivoConstancia;

                        insertCommand.ExecuteNonQuery();
                    }

                    // Actualizar Constancias
                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.Add("@Archivoconstancia", SqlDbType.VarBinary).Value = archivoConstancia;
                        updateCommand.Parameters.Add("@SolicitudId", SqlDbType.NVarChar).Value = solicitudId;
                        updateCommand.Parameters.Add("@UsuarioModificacionId", SqlDbType.Int).Value = userID;
                        updateCommand.Parameters.Add("@fechaModificacion", SqlDbType.DateTime).Value = DateTime.Now;

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

        protected void GV_PreUsuarios_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GV_PreUsuarios.DataBind();
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


        [WebMethod]
        public static bool GuardarConstancia(string constanciaId)
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                string query = "UPDATE Constancias SET EnlaceArchivo = @EnlaceArchivo WHERE SolicitudId = @ConstanciaId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ConstanciaId", constanciaId);
                    command.Parameters.AddWithValue("@EnlaceArchivo", "ruta/a/la/constancia.pdf"); // Aquí debes proporcionar la ruta correcta

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return false;
            }
        }

        protected void btnGuardaConstancia_Click(object sender, EventArgs e)
        {
            try
            {
                int tipo = Convert.ToInt32(cmbTipoFiltro.Value);
                DataTable dt = GetFilteredDataFromGridView(tipo);

                if (dt != null && dt.Rows.Count > 0)
                {

                    string solicitudId = dt.Rows[0]["Id"].ToString();


                    string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                    string query = "UPDATE Constancias SET EnlaceArchivo = @EnlaceArchivo WHERE SolicitudId = @solicitudId";


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@solicitudId", solicitudId);
                        command.Parameters.AddWithValue("@EnlaceArchivo", "ruta/a/la/constancia.pdf");

                        connection.Open();
                        int result = command.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {

            }
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

public static class VerificacionHelper
{
    private static string _verificacionCode = Guid.NewGuid().ToString();

    public static string GetVerificacionCode()
    {
        return _verificacionCode;
    }
}