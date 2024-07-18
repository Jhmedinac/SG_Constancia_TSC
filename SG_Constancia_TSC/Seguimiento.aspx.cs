using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;



namespace SG_Constancia_TSC
{
    public partial class Seguimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            string constanciaId = txtConstanciaId.Text.Trim();
            string clave = txtClave.Text.Trim();

            if (string.IsNullOrEmpty(constanciaId) || string.IsNullOrEmpty(clave))
            {
                lblMensaje.Text = "Por favor, ingrese el número de constancia y la clave.";
                return;
            }

            bool isValid = ValidateConstancia(constanciaId, clave);

            if (isValid)
            {
                // Mostrar el popup con la información
                string script = "Relacionado.Show();";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", script, true);
            }
            else
            {
                lblMensaje.Text = "Número de constancia o clave incorrectos.";
            }
        }

        private bool ValidateConstancia(string constanciaId, string clave)
        {
            bool isValid = false;

            // Aquí va la lógica para validar la constancia y la clave con la base de datos
            string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;// txtConnectionString.Text;";
            string query = "SELECT COUNT(*) FROM Constancias WHERE ConstanciaId = @ConstanciaId AND Clave = @Clave";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ConstanciaId", constanciaId);
                command.Parameters.AddWithValue("@Clave", clave);

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());
                isValid = count > 0;
            }

            return isValid;
        }

        [WebMethod]
        public static string GetSessionValues(string constanciaId)
        {
            // Obtener valores de la constancia de la base de datos
            string estado = string.Empty;
            string fechaCreacion = string.Empty;
            string otrosDatos = string.Empty;

            string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;// txtConnectionString.Text;";;
            string query = "SELECT Estado, FechaCreacion, Observaciones FROM Constancias WHERE ConstanciaId = @ConstanciaId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ConstanciaId", constanciaId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    estado = reader["Estado"].ToString();
                    fechaCreacion = reader["FechaCreacion"].ToString();
                    otrosDatos = reader["Observaciones"].ToString();
                }
            }

            return $"{constanciaId}|{estado}|{fechaCreacion}|{otrosDatos}";
        }
    }
}