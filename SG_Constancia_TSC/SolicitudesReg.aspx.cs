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
        // 0) Vuelve a cargar los ítems del combo
        LoadStatuses();

        // 0.1) Valida que el usuario haya seleccionado un estado
        if (string.IsNullOrEmpty(cmbStatus.Text))
        {
            e.Result = "Error: Complete todos los campos obligatorios.";
            return;
        }

        // 1) Validación: debe haber un SelectedItem (ya garantizado por el Text)
        if (cmbStatus.SelectedItem == null)
        {
            e.Result = "Error: Debe seleccionar un Estado.";
            return;
        }

        // 2) Recoge el ID de la fila seleccionada
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

        // 3) Lee estado y observación con seguridad
        string estado      = cmbStatus.SelectedItem.Value.ToString();
        string observacion = txtObs.Text.Trim();

        // 4) Llama al SP para actualizar estado y observación
        string cs = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        using (var conn = new SqlConnection(cs))
        using (var cmd  = new SqlCommand("[gral].[sp_gestion_estado]", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IDUser", Convert.ToInt32(Session["User_id"]));
            cmd.Parameters.AddWithValue("@IDs", Convert.ToInt32(selectedID));
            cmd.Parameters.AddWithValue("@Estado", Convert.ToInt32(estado));
            cmd.Parameters.AddWithValue("@Obs", observacion);

            var mensParam    = new SqlParameter("@MENS", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
            var retornoParam = new SqlParameter("@RETORNO", SqlDbType.Int)      { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(mensParam);
            cmd.Parameters.Add(retornoParam);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            int retorno = Convert.ToInt32(retornoParam.Value);
            e.Result = (retorno == 1)
                ? "Estado actualizado correctamente."
                : mensParam.Value.ToString();
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



