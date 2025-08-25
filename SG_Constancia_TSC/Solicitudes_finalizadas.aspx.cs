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
            string query = "SELECT Id_Estado, Descripcion_Estado FROM Estados WHERE Id_Estado IN (8)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                cmbStatus.DataSource = dataTable;
                cmbStatus.DataBind();
            }
        }
        protected void ASPxCallback_PopupUpdate_Callback(object source, CallbackEventArgs e)
        {
            int index = GV_PreUsuarios.FocusedRowIndex;
            if (index < 0) return;

            object id = GV_PreUsuarios.GetRowValues(index, "Id");
            if (id == null) return;

            string selectedID = id.ToString();  //  Usamos el ID directamente

            string estado = cmbStatus.SelectedItem?.Value?.ToString();
            if (string.IsNullOrEmpty(estado))
                return;


            string observacion = txtObs.Text;

            // Ejecuta el procedimiento almacenado para actualizar el estado y la observación
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("[gral].[sp_gestion_estado]", connection))

                {
                    int UserID = Convert.ToInt16(Session["User_id"]);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IDUser", SqlDbType.Int).Value = UserID;
                    command.Parameters.AddWithValue("@IDs", selectedID);
                    command.Parameters.AddWithValue("@Estado", estado);
                    command.Parameters.AddWithValue("@Obs", observacion);
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

                    //Lógica para manejar el mensaje y retorno
                    if (retorno == 1)
                    {
                        GV_PreUsuarios.DataBind();
                        // Indica éxito con una señal especial
                        e.Result = "OK|La solicitud se ha enviado a la bandeja de Autorizar Constancias.|CLEAR";
                    }
                    else
                    {
                        GV_PreUsuarios.DataBind();
                        e.Result = $"ERROR|{mensaje}";
                    }
                }
            }


            GV_PreUsuarios.DataBind();
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

                            // Solo declaramos una vez la variable
                            string textoFormateado = $@"
                       <div style='font-family:Times New Roman; font-size:12pt; text-align:justify ;'>
                       <p style='margin-bottom:24pt;'>El Tribunal Superior de Cuentas a través de la Sección Constancias de no tener cuentas 
                         pendientes con el Estado, adscrita a la Secretaria General, por este medio  <b>HACE
                         CONSTAR: </b> Que <b>{nombre}</b>, con Documento Nacional de  
                         Identificación <b>N° {documento}</b>, no tiene a la fecha ningún tipo de responsabilidad firme, 
                         ni existe ninguna intervención por presunción de enriquecimiento ilícito en igual situación      
                         que le impida el desempeño de un cargo público.</p>
                        <br/>
                     
                       <p style='margin-bottom:24pt;'>La presente constancia no constituye Solvencia con el Estado de Honduras, ni finiquito a favor del solicitante, quedando sujeto a investigaciones futuras.</p>
                        <br/>
                      <p style='margin-bottom:0pt;'>La presente constancia tiene validez por el término de seis meses.</p>
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

                    //switch (valorAdjunto)
                    //{

                    //    case "Firma Encargado de estadística":
                    //        OpenReport("Firma_Encargado_de_estadistica");
                    //        break;
                    //    case "Firma Encargado de estadística 1":
                    //        OpenReport("Firma_Encargado_de_estadistica_1");
                    //        break;
                    //    case "Firma Encargado de estadística 2":
                    //        OpenReport("Firma_Encargado_de_estadistica_2");
                    //        break;

                    //}
                }
                else
                {
                    string script = @"
                Swal.fire({
                    title: '¡Alerta!',
                    text: 'Por favor seleccione una fila para generar la constancia.',
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

                            // Solo declaramos una vez la variable
                            string textoFormateado = $@"
                           <div style='font-family:Times New Roman; font-size:12pt; text-align:justify ;'>
                           <p style='margin-bottom:24pt;'>El Tribunal Superior de Cuentas a través de la Sección Constancias de no tener cuentas 
                            pendientes con el Estado, adscrita a la Secretaria General, por este medio,<b>HACE 
                            CONSTAR:</b> Que la <b>{nombre}</b> con RTN <b>N° {documento}</b>, no tiene a la 
                            fecha ningún tipo de responsabilidad civil o administrativa en carácter de firme, ante esta
                            institución.</p>
                           
                        <br/>
                       <p style='margin-bottom:24pt;'>La presente constancia no constituye Solvencia con el Estado de Honduras, ni finiquito 
                                                      a favor del solicitante, quedando sujeto a investigaciones futuras.</p>
                        <br/>
                      <p style='margin-bottom:0pt;'>La presente constancia tiene validez por el término de seis meses.</p>
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

                    //switch (valorAdjunto)
                    //{

                    //    case "Firma Encargado de estadística":
                    //        OpenReport("Firma_Encargado_de_estadistica");
                    //        break;
                    //    case "Firma Encargado de estadística 1":
                    //        OpenReport("Firma_Encargado_de_estadistica_1");
                    //        break;
                    //    case "Firma Encargado de estadística 2":
                    //        OpenReport("Firma_Encargado_de_estadistica_2");
                    //        break;

                    //}
                }
                else
                {
                    string script = @"
                Swal.fire({
                    title: '¡Alerta!',
                    text: 'Por favor seleccione una fila para generar la constancia.',
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
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT 1 FROM ConstanciasGeneradas WHERE SolicitudId = @SolicitudId", conn))
            {
                cmd.Parameters.AddWithValue("@SolicitudId", solicitudId);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        private void GuardarConstanciaEnBD(string solicitudId, byte[] archivoConstancia, string codigoVerificacion)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

            if (ConstanciaYaGenerada(solicitudId))
                return;

            string insertQuery = @"
        INSERT INTO ConstanciasGeneradas (
            Clave, Estado, FechaCreacion, Observaciones, SolicitudId, CodigoVerificacion, FechaAprobacion, Archivoconstancia
        ) VALUES (
            @Clave, @Estado, @FechaCreacion, @Observaciones, @SolicitudId, @CodigoVerificacion, @FechaAprobacion, @Archivoconstancia
        )";

            string updateQuery = @"
        UPDATE Constancias
        SET Archivoconstancia = @Archivoconstancia, Estado = 8, Observaciones = 'Constancia fue generada'
        WHERE SolicitudId = @SolicitudId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.Add("@Clave", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                    insertCommand.Parameters.Add("@Estado", SqlDbType.NVarChar).Value = "Generada";
                    insertCommand.Parameters.Add("@FechaCreacion", SqlDbType.DateTime).Value = DateTime.Now;
                    insertCommand.Parameters.Add("@Observaciones", SqlDbType.NVarChar).Value = "Constancia generada automáticamente";
                    insertCommand.Parameters.Add("@SolicitudId", SqlDbType.NVarChar).Value = solicitudId;
                    insertCommand.Parameters.Add("@CodigoVerificacion", SqlDbType.NVarChar).Value = codigoVerificacion ?? "N/A";
                    insertCommand.Parameters.Add("@FechaAprobacion", SqlDbType.DateTime).Value = DBNull.Value;
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
                        command.Parameters.AddWithValue("@EnlaceArchivo", "ruta/a/la/constancia.pdf"); // Aquí debes proporcionar la ruta correcta

                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        //return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                //return false;
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