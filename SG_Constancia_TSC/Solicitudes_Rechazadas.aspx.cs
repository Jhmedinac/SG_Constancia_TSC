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

namespace SG_Constancia_TSC
{
    public partial class Solicitudes_Rechazadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Attributes.Add("autocomplete", "off");

            if (!IsPostBack)
            {
                LoadStatuses();
                
                cmbTipoFiltro.Value = "0";

                 GV_PreUsuarios.FilterExpression = "TipoSolicitud = false";
                

                
                GV_PreUsuarios.Columns["Identidad"].Visible = true;
                GV_PreUsuarios.Columns["FirstName"].Visible = true;
                GV_PreUsuarios.Columns["LastName"].Visible = true;
                // Jurídicas: NumRtn/NomInstitucion ocultas
                GV_PreUsuarios.Columns["NumRtn"].Visible = false;
                GV_PreUsuarios.Columns["NomInstitucion"].Visible = false;

                // 4) Finalmente, renderiza el grid
                GV_PreUsuarios.DataBind();

            }

        }
        private void LoadStatuses()
        {
            string connectionString = (ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
            string query = "SELECT Id_Estado, Descripcion_Estado FROM Estados WHERE Id_Estado IN (2)";

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

        

        protected void GV_PreUsuarios_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            GV_PreUsuarios.DataBind();
        }

        protected void cmbTipoFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {

            string val = cmbTipoFiltro.Value as string;

            bool isNatural = (val == "0");

            // 1) Filtrar filas según selección
            GV_PreUsuarios.FilterExpression = isNatural
                ? "TipoSolicitud = false"
                : "TipoSolicitud = true";

            // 2) Columnas según tipo
            GV_PreUsuarios.Columns["Identidad"].Visible = isNatural;
            GV_PreUsuarios.Columns["FirstName"].Visible = isNatural;
            GV_PreUsuarios.Columns["LastName"].Visible = isNatural;

            GV_PreUsuarios.Columns["NumRtn"].Visible = !isNatural;
            GV_PreUsuarios.Columns["NomInstitucion"].Visible = !isNatural;

            // 3) Refrescar grid con los nuevos ajustes
            GV_PreUsuarios.DataBind();


        }
    }
}