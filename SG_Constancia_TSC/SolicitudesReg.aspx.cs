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
using DevExpress.Office.Internal;
using DevExpress.XtraPrinting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SG_Constancia_TSC
{
    public partial class SolicitudesReg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Attributes.Add("autocomplete", "off");

            if (!IsPostBack)
            {
                LoadStatuses();
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


        private void LoadStatuses()
        {
            string connectionString = (ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            string query = "SELECT Id_Estado, Descripcion_Estado FROM Estados WHERE Id_Estado IN (2,5,6)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                cmbStatus.DataSource = dataTable;
                cmbStatus.DataBind();
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
       

        protected void GV_PreUsuarios_DetailRowExpandedChanged(object sender, ASPxGridViewDetailRowEventArgs e)
        {
            if (e.Expanded)
            {
                ASPxGridView detailGrid = (ASPxGridView)GV_PreUsuarios.FindDetailRowTemplateControl(e.VisibleIndex, "GV_Detalle");
                if (detailGrid != null)
                {
                    int id = (int)GV_PreUsuarios.GetRowValues(e.VisibleIndex, "Id");
                    SqlDataDetalle.SelectParameters["Id"].DefaultValue = id.ToString();
                    detailGrid.DataBind();
                }
            }
        }

        protected void ASPxCallback_PopupUpdate_Callback(object source, CallbackEventArgs e)
        {
            try
            {
                // Validar en el servidor
                //if (string.IsNullOrEmpty(cmbStatus.Text) || string.IsNullOrEmpty(txtObs.Text))
                if (string.IsNullOrEmpty(cmbStatus.Text))
                {
                    e.Result = "Error: Complete todos los campos obligatorios.";
                    return;
                }

                // Lógica de actualización
                List<string> selectedIDs = new List<string>();
                foreach (var key in GV_PreUsuarios.GetSelectedFieldValues("Id"))
                {
                    selectedIDs.Add(key.ToString());
                }

                if (selectedIDs.Count != 1)
                {
                    e.Result = "Error: Seleccione un solo usuario.";
                    return;
                }

                string selectedID = selectedIDs[0];
                string estado = cmbStatus.SelectedItem.Value.ToString();
                string observacion = txtObs.Text;

                // Conexión y ejecución
                string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("[gral].[sp_gestion_estado]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IDUser", Convert.ToInt32(Session["User_id"]));
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

                        string mensaje = mensParam.Value.ToString();
                        int retorno = Convert.ToInt32(retornoParam.Value);

                        if (retorno == 1)
                        {
                            e.Result = "Estado actualizado correctamente.";
                        }
                        else
                        {
                            e.Result = mensaje;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = "Error: " + ex.Message;
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

        protected void GV_PreUsuarios_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GV_PreUsuarios.DataBind();
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




    }
}