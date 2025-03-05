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

using DevExpress.XtraPrinting;
using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.CodeParser;
//using Org.BouncyCastle.Bcpg;

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
                GV_PreUsuarios.FocusedRowIndex = -1; // Evita que se enfoque una fila al cargar la página
            }

        }
        private void LoadStatuses()
        {
            string connectionString = (ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            string query = "SELECT Id_Estado, Descripcion_Estado FROM Estados WHERE Id_Estado IN (2, 3)";

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
            List<string> selectedIDs = new List<string>();
            foreach (var key in GV_PreUsuarios.GetSelectedFieldValues("Id"))
            {
                selectedIDs.Add(key.ToString());
            }

            if (selectedIDs.Count != 1)
            {
                return;
            }

            string selectedID = selectedIDs[0]; // Solo tomamos el primer (y único) ID
            string estado = cmbStatus.SelectedItem.Value.ToString();
            //string observacion = txtObs.Text;

            // Ejecuta el procedimiento almacenado para actualizar el estado y la observación
            string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("[gral].[sp_gestion_estado_user]", connection))
                {
                    int UserID = Convert.ToInt16(Session["User_id"]);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IDUser", SqlDbType.Int).Value = UserID;
                    command.Parameters.AddWithValue("@IDs", selectedID);
                    command.Parameters.AddWithValue("@Estado", estado);
                    //command.Parameters.AddWithValue("@Observacion", observacion);
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
                        //ScriptManager.RegisterStartupScript(this, GetType(), "closePopup", "closePopupAndClearFields();", true);
                    }
                    else
                    {
                        // Error
                        GV_PreUsuarios.DataBind();
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
                    string query = "SELECT * FROM solicitudes "; // Ajusta esta consulta a tus necesidades
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

        private void ExportToCSV(List<string> selectedIDs)
        {
            StringBuilder csvContent = new StringBuilder();
            DataTable updatedData = new DataTable();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                string query = "SELECT * FROM Users WHERE Id IN ({0})";
                var parameters = new List<string>();
                var commandParameters = new List<SqlParameter>();
                for (int i = 0; i < selectedIDs.Count; i++)
                {
                    string parameterName = "@id" + i;
                    parameters.Add(parameterName);
                    commandParameters.Add(new SqlParameter(parameterName, selectedIDs[i]));
                }
                query = string.Format(query, string.Join(",", parameters));

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddRange(commandParameters.ToArray());
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(updatedData);
                    }
                }
            }

            // Columns to export
            List<string> selectedColumns = new List<string> { "FirstName", "LastName", "Email", "EmployeeId", "UserId", "Address" };

            // Filter columns
            var filteredData = new DataTable();
            foreach (var col in selectedColumns)
            {
                filteredData.Columns.Add(col, updatedData.Columns[col].DataType);
            }

            foreach (DataRow row in updatedData.Rows)
            {
                var newRow = filteredData.NewRow();
                foreach (var col in selectedColumns)
                {
                    newRow[col] = row[col];
                }
                filteredData.Rows.Add(newRow);
            }

            csvContent.AppendLine(string.Join(",", filteredData.Columns.Cast<DataColumn>().Select(col => "\"" + col.ColumnName.Replace("\"", "\"\"") + "\"")));

            foreach (DataRow row in filteredData.Rows)
            {
                List<string> fields = new List<string>();
                foreach (var item in row.ItemArray)
                {
                    fields.Add("\"" + item.ToString().Replace("\"", "\"\"") + "\"");
                }
                csvContent.AppendLine(string.Join(",", fields));
            }

            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment;filename=DatosActualizados.csv");
            Response.ContentEncoding = Encoding.UTF8;

            byte[] byteArray = Encoding.UTF8.GetBytes(csvContent.ToString());
            using (MemoryStream memoryStream = new MemoryStream(byteArray))
            {
                memoryStream.WriteTo(Response.OutputStream);
            }

            Response.End();
        }


        //GV_PreUsuarios_BeforeExport
        protected void GV_PreUsuarios_BeforeExport(object sender, ASPxGridBeforeExportEventArgs e)
        {
            List<string> selectedIDs = new List<string>();
            foreach (var key in GV_PreUsuarios.GetSelectedFieldValues("Id"))
            {
                selectedIDs.Add(key.ToString());
            }

            if (!(GV_PreUsuarios.Selection.Count > 0))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertMessage", "alert('Por favor, seleccione las filas a exportar.');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            {
                try
                {
                    int UserID = Convert.ToInt16(Session["User_id"]);
                    using (SqlCommand cmd = new SqlCommand("[gral].[sp_gestion_exportado_user]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IDUser", UserID));
                        cmd.Parameters.Add(new SqlParameter("@IDs", string.Join(",", selectedIDs)));

                        SqlParameter correoParam = new SqlParameter("@CorreoElectronico", SqlDbType.NVarChar, -1); // Use -1 for NVARCHAR(MAX)
                        correoParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(correoParam);

                        SqlParameter retornoParam = new SqlParameter("@RETORNO", SqlDbType.Int);
                        retornoParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(retornoParam);

                        SqlParameter mensajeEstadoParam = new SqlParameter("@MENS", SqlDbType.NVarChar, 255);
                        mensajeEstadoParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(mensajeEstadoParam);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        string emailList = correoParam.Value != DBNull.Value ? correoParam.Value.ToString() : string.Empty;
                        string mensajeEstado = mensajeEstadoParam.Value != DBNull.Value ? mensajeEstadoParam.Value.ToString() : string.Empty;
                        int retorno = retornoParam.Value != DBNull.Value ? (int)retornoParam.Value : 0;

                        if (retorno == 1)
                        {
                            SampleUtil.SendEmails(emailList);
                            ExportToCSV(selectedIDs);

                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + mensajeEstado + "');", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error al ejecutar el procedimiento almacenado: " + ex.Message + "');", true);
                    GV_PreUsuarios.Selection.UnselectAll();
                    GV_PreUsuarios.DataBind();
                }
                finally
                {
                    con.Close();
                    GV_PreUsuarios.DataBind();
                }
            }
        }

        private DataTable GetFilteredDataFromGridView()
        {
            // Crear y definir el DataTable
            DataTable dt = new DataTable();
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("Identidad", typeof(string));
            dt.Columns.Add("nombre", typeof(string)); // Columna para el nombre completo

            // Obtener el índice de la fila enfocada (seleccionada)
            int selectedRowIndex = GV_PreUsuarios.FocusedRowIndex;
            if (selectedRowIndex >= 0)
            {
                // Obtener los valores de la fila seleccionada
                object[] values = GV_PreUsuarios.GetRowValues(
                    selectedRowIndex,
                    new string[] { "FirstName", "LastName", "Identidad" }
                ) as object[];

                if (values != null)
                {
                    DataRow dr = dt.NewRow();
                    dr["FirstName"] = values[0];
                    dr["LastName"] = values[1];
                    dr["Identidad"] = values[2];
                    // Combina FirstName y LastName, agregando un espacio entre ellos
                    dr["nombre"] = values[0].ToString() + " " + values[1].ToString();
                    dt.Rows.Add(dr);
                }
            }
            // Si no hay fila seleccionada, el DataTable quedará vacío
            return dt;
        }


        //protected void ASPxReportCosntancia_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = GetFilteredDataFromGridView();

        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        var parametros = HttpContext.Current.Session["Parametros"] as Dictionary<string, Parametro>;

        //        if (parametros == null)
        //        {
        //            throw new InvalidOperationException("Los parámetros no están disponibles en la sesión.");
        //        }

        //        // Función local para crear y abrir el reporte
        //        void OpenReport(string nombreParametro)
        //        {
        //            if (parametros.TryGetValue(nombreParametro, out Parametro parametro))
        //            {
        //                string valor = parametro.Valor;
        //                string descripcion = parametro.Descripcion;

        //                if (string.IsNullOrEmpty(valor) || string.IsNullOrEmpty(descripcion))
        //                {
        //                    valor = "Valor por defecto";
        //                    descripcion = "Descripción por defecto";
        //                }
        //                // CORRECCIÓN: Acceder a la fila 0 del DataTable
        //                string nombre = dt.Rows[0]["nombre"].ToString();
        //                string documento = dt.Rows[0]["Identidad"].ToString();


        //                if descripcion == "Firma_Secretario_adjunto")
        //                {
        //                    string textoFormateado =
        //                    "Por este medio<b>HACE CONSTAR:</b>: Que, de acuerdo a revisión efectuada en los  <b>" +
        //                    "<archivos de esta Secretaría, así como en el Sistema de Pliegos enviados, archivo  " +
        //                    "de Pliegos de Responsabilidad Notificados 2008-2024 y Carpeta de Resoluciones, " +
        //                    "la ciudadana <b> " + nombre + "</b> , con Documento Nacional de <br><br>" +
        //                    "Identificación N°<b>" + documento + "</b> , no tiene a la fecha ningún tipo de "  +
        //                    "responsabilidad firme, ni existe ninguna intervención por presunción de " +
        //                    "enriquecimiento ilícito.";
        //                }
        //                else if (descripcion == "Firma_Secretaria_general")
        //                {
        //                    string textoFormateado =
        //                    "El Infrascrito Secretario General Adjunto del Tribunal Superior de Cuentas, <b>HACE CONSTAR:</b> Que la ciudadana <b>" +
        //                    nombre + "</b>, con Documento Nacional de Identificación N°<b>" + documento + "</b>, no tiene a la fecha ningún tipo de " +
        //                    "responsabilidad firme, ni existe ninguna intervención por presunción de enriquecimiento ilícito en igual situación que le impida el desempeño de un cargo público.<br><br>" +
        //                    "La presente constancia no constituye Solvencia con el Estado de Honduras, ni finiquito a favor del solicitante, quedando sujeto a investigaciones futuras.<br><br>" +
        //                    "La presente constancia tiene validez por el término de seis meses.";
        //                }



        //                var report = new Reportes.Constancia(textoFormateado);
        //                report.Parameters["Firma"].Value = valor;
        //                report.Parameters["Descripcion"].Value = descripcion;

        //                ASPxWebDocumentViewer1.OpenReport(report);
        //                ASPxWebDocumentViewer1.Visible = true;
        //            }
        //            else
        //            {
        //                throw new InvalidOperationException($"El parámetro '{nombreParametro}' no se encontró.");
        //            }
        //        }

        //        bool isAdjunto = false;
        //        if (ASPxChkAdjunto.Value != null)
        //        {
        //            isAdjunto = true;
        //        }

        //        if (isAdjunto) // Si se marca "adjunto"
        //        {
        //            OpenReport("Firma_Secretario_adjunto");
        //        }
        //        else
        //        {
        //            OpenReport("Firma_Secretaria_general");
        //        }
        //    }
        //    else
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No hay datos para imprimir.');", true);
        //    }
        //}

        protected void ASPxReportCosntancia_Click(object sender, EventArgs e)
        {
            DataTable dt = GetFilteredDataFromGridView();

            if (dt != null && dt.Rows.Count > 0)
            {
                var parametros = HttpContext.Current.Session["Parametros"] as Dictionary<string, Parametro>;

                if (parametros == null)
                {
                    throw new InvalidOperationException("Los parámetros no están disponibles en la sesión.");
                }

                // Obtener datos de la primera fila
                string nombre = dt.Columns.Contains("nombre") ? dt.Rows[0]["nombre"].ToString() : "Desconocido";
                string documento = dt.Columns.Contains("Identidad") ? dt.Rows[0]["Identidad"].ToString() : "N/A";

                // Función local para generar el reporte
                void OpenReport(string nombreParametro)
                {
                    if (parametros.TryGetValue(nombreParametro, out Parametro parametro))
                    {
                        string valor = parametro.Valor ?? "Valor por defecto";
                        string descripcion = parametro.Descripcion ?? "Descripción por defecto";

                        string textoFormateado = string.Empty;

                        if (nombreParametro == "Firma_Secretario_adjunto" && nombreParametro == "Firma_Secretaria_general")
                        {
                            textoFormateado =
                                 "El Infrascrito Secretario General Adjunto del Tribunal Superior de Cuentas, <b>HACE CONSTAR:</b> Que la ciudadana <b>" +
                                 nombre + "</b>, con Documento Nacional de Identificación N°<b>" + documento + "</b>, no tiene a la fecha ningún tipo de " +
                                 "responsabilidad firme, ni existe ninguna intervención por presunción de enriquecimiento ilícito en igual situación que le impida el desempeño de un cargo público.<br><br>" +
                                 "La presente constancia no constituye Solvencia con el Estado de Honduras, ni finiquito a favor del solicitante, quedando sujeto a investigaciones futuras.<br><br>" +
                                 "La presente constancia tiene validez por el término de seis meses.";
                        }
                        else 
                        {
                            textoFormateado =
                               "Por este medio <b>HACE CONSTAR:</b> Que, de acuerdo a revisión efectuada en los " +
                               "<b>archivos de esta Secretaría</b>, así como en el Sistema de Pliegos enviados, archivo " +
                               "de Pliegos de Responsabilidad Notificados 2008-2024 y Carpeta de Resoluciones, " +
                               "la ciudadana <b>" + nombre + "</b>, con Documento Nacional de " +
                               "Identificación N°<b>" + documento + "</b>, no tiene a la fecha ningún tipo de " +
                               "responsabilidad firme, ni existe ninguna intervención por presunción de " +
                               "enriquecimiento ilícito.";

                            
                        }

                        var report = new Reportes.Constancia(textoFormateado);
                        report.Parameters["Firma"].Value = valor;
                        report.Parameters["Descripcion"].Value = descripcion;

                        ASPxWebDocumentViewer1.OpenReport(report);
                        ASPxWebDocumentViewer1.Visible = true;
                    }
                    else
                    {
                        throw new InvalidOperationException($"El parámetro '{nombreParametro}' no se encontró.");
                    }
                }
                //bool isAdjunto = false;
                if (ASPxChkAdjunto.Value == null)
                {
                    ASPxChkAdjunto.Value = "Firma_Secretaria_general";
                }

                string valorAdjunto = ASPxChkAdjunto.Value?.ToString(); // Convertir a string de forma segura

                if (valorAdjunto == "Firma Secretario(a) Adjunto")
                {
                    OpenReport("Firma_Secretario_adjunto");
                }
                else if (valorAdjunto == "Firma_Secretaria_general")
                {
                    OpenReport("Firma_Secretaria_general");
                }
                else if (valorAdjunto == "Firma Encargado de estadística")
                {
                    OpenReport("Firma_Encargado_de_estadistica");
                }
                else
                {
                    OpenReport("Firma_Secretaria_general"); // Valor por defecto
                }

            }
            else
            {
                // Si hay un UpdatePanel, usar ScriptManager en lugar de ClientScript
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('No hay datos para imprimir.');", true);
            }
        }

        protected void GV_PreUsuarios_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GV_PreUsuarios.DataBind();
        }
    }
}