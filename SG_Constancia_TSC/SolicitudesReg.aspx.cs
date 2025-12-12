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
            if (!IsPostBack)
            {
                LoadStatuses();
                // 1) Pone el combo en Natural
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
            string query = "SELECT Id_Estado, Descripcion_Estado FROM Estados WHERE Id_Estado IN (2,3,5)";

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
                int userID = Convert.ToInt32(Session["User_id"]);

              
                object postedEstadoValue = null;
                try { postedEstadoValue = cmbStatus.Value; } catch { postedEstadoValue = null; }

                LoadStatuses();

                if (cmbStatus != null) cmbStatus.DataBind();

                if (postedEstadoValue != null)
                {
                    try { cmbStatus.Value = postedEstadoValue; } catch { /* defensiva */ }
                }

                // Valida que el usuario haya seleccionado un estado
                if (string.IsNullOrEmpty(cmbStatus.Text))
                {
                    e.Result = "Error: Complete todos los campos obligatorios.";
                    return;
                }

                // Validación: debe haber un SelectedItem (ya garantizado por el Text)
                if (cmbStatus.SelectedItem == null)
                {
                    e.Result = "Error: Debe seleccionar un Estado.";
                    return;
                }

                //  Recoge el ID de la fila seleccionada
                var selectedIDs = GV_PreUsuarios.GetSelectedFieldValues("Id")
                                                .Cast<object>()
                                                .Select(x => x.ToString())
                                                .ToList();
                if (selectedIDs.Count != 1)
                {
                    e.Result = "Error: Seleccione un solo registro.";
                    return;
                }
                string selectedID = selectedIDs[0];

                //  Lee estado y observación con seguridad
                string estado = cmbStatus.SelectedItem.Value.ToString();
                string observacion = txtObs.Text.Trim();
                string Desc_estado = cmbStatus.SelectedItem?.Text?.ToString();
                //  Llama al SP para actualizar estado y observación
                string cs = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                using (var conn = new SqlConnection(cs))
                using (var cmd = new SqlCommand("[gral].[sp_gestion_estado]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDUser", Convert.ToInt32(Session["User_id"]));
                    cmd.Parameters.AddWithValue("@IDs", Convert.ToInt32(selectedID));
                    cmd.Parameters.AddWithValue("@Estado", Convert.ToInt32(estado));
                    cmd.Parameters.AddWithValue("@Obs", observacion);
                    cmd.Parameters.AddWithValue("@pcUsuarioModificaId", userID);

                    var mensParam = new SqlParameter("@MENS", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    var retornoParam = new SqlParameter("@RETORNO", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(mensParam);
                    cmd.Parameters.Add(retornoParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    int retorno = Convert.ToInt32(retornoParam.Value);
                    e.Result = (retorno == 1)
                        ? "Estado actualizado correctamente."
                        : mensParam.Value.ToString();
                    string adressEmail = SampleUtil.GetEmail(selectedID);
                    string subject = "Actualización del estado de su solicitud de constancia";

                    string emailBody = @"
<!doctype html>
<html>
  <head>
    <meta charset='utf-8'>
    <title>Estado de solicitud</title>
  </head>
  <body style='margin:0;padding:0;background-color:#f5f5f5; font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#333; line-height:1.6;'>
    <div style='max-width:600px; margin:0 auto; padding:20px;'>
      <div style='border:1px solid #ddd; border-radius:8px; background-color:#fafafa; padding:20px;'>
        
        <div style='text-align:center; margin-bottom:20px;'>
          <img src='https://dcioxh.stripocdn.email/content/guids/CABINET_b93ec2a38389d475174431c45f61c597b12b8d21784bda70816c8c0373b4ae6a/images/logo_tsc_2024_n0C.png'
               alt='Logo TSC' width='150' style='display:block; margin:0 auto;' />
        </div>

        <p style='text-align:center; margin:0 0 12px 0;'>Estimado(a) Usuario,</p>

        <p style='text-align:center; margin:0 0 12px 0;'>
          Le informamos que el estado de su solicitud de
          <b>Constancia de no tener cuentas pendientes con el Estado de Honduras</b>
          ha sido actualizado a: <b>" + Desc_estado + @"</b>.
        </p>

       <p style=""text-align:center; margin:0 0 12px 0;"">
  Para más detalles, por favor ingrese al siguiente enlace:  
  <a href=""https://consta-sec-dev.tsc.gob.hn:8011/Seguimiento.aspx""
     target=""_blank"" 
     style=""color:#1F497D; font-weight:bold; text-decoration:underline;"">
     Seguimiento de Solicitud
  </a>  
  </b>.
</p>

        <p style='margin-top:20px; text-align:center;'>
          <b>Atentamente,</b><br/>
          Tribunal Superior de Cuentas<br/>
          Secretaría General
        </p>

        <hr style='margin:24px 0; border:none; border-top:1px solid #ccc;' />

        <p style='font-size:12px; color:#666; text-align:center; margin:0;'>
          Este mensaje fue generado automáticamente por el Sistema de Solicitudes de Constancias en Línea.<br/>
          Por favor, no responda este mensaje.
        </p>

      </div>
    </div>
  </body>
</html>";

                    SampleUtil.EnviarCorreo1("", subject, adressEmail, emailBody);
                }

                // 5) Vuelve a aplicar el filtro de TipoSolicitud
                bool isNatural = (cmbTipoFiltro.Value as string) == "0";
                SqlDataUsers.SelectParameters["TipoSolicitud"].DefaultValue = isNatural ? "false" : "true";

                // 6) Re-bind del grid
                GV_PreUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                e.Result = "Error inesperado: " + ex.Message;
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
            string url = await DownloadFile(Upload_Id);

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







