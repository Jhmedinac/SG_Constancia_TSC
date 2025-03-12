//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.NetworkInformation;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;

//using System.Data.SqlClient;
//using System.Web.Services;
//using System.Configuration;



//namespace SG_Constancia_TSC
//{
//    public partial class Seguimiento : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, EventArgs e)
//        {
//        }

//        protected void btnBuscar_Click(object sender, EventArgs e)
//        {
//            string constanciaId = txtConstanciaId.Text.Trim();
//            string clave = txtClave.Text.Trim();

//            if (string.IsNullOrEmpty(constanciaId) || string.IsNullOrEmpty(clave))
//            {
//                lblMensaje.Text = "Por favor, ingrese el número de constancia y la clave.";
//                return;
//            }

//            bool isValid = ValidateConstancia(constanciaId, clave);

//            if (isValid)
//            {
//                // Mostrar el popup con la información
//                string script = "setTimeout(function() { Relacionado1.Show(); }, 500);";
//                ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", script, true);
//            }
//            else
//            {
//                lblMensaje.Text = "Número de constancia o clave incorrectos.";
//            }
//        }

//        private bool ValidateConstancia(string constanciaId, string clave)
//        {
//            bool isValid = false;

//            // Aquí va la lógica para validar la constancia y la clave con la base de datos
//            string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;// txtConnectionString.Text;";
//            string query = "SELECT COUNT(*) FROM Constancias WHERE solicitudid = @ConstanciaId AND Clave = @Clave";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                SqlCommand command = new SqlCommand(query, connection);
//                command.Parameters.AddWithValue("@ConstanciaId", constanciaId);
//                command.Parameters.AddWithValue("@Clave", clave);

//                connection.Open();
//                int count = Convert.ToInt32(command.ExecuteScalar());
//                isValid = count > 0;
//            }

//            return isValid;
//        }

//        [WebMethod]
//        public static string GetSessionValues(string constanciaId)
//        {
//            try
//            {
//                // Obtener valores de la constancia de la base de datos
//                string estado = string.Empty;
//                string fechaCreacion = string.Empty;
//                string otrosDatos = string.Empty;

//                string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
//                string query = @"
//                        SELECT Constancias.FechaCreacion, 
//                                Constancias.Observaciones, 
//                                Estados.Descripcion_Estado 
//                        FROM Constancias 
//                        INNER JOIN Estados ON Constancias.Estado = Estados.Id_Estado 
//                        WHERE Constancias.SolicitudId = @ConstanciaId";

//                using (SqlConnection connection = new SqlConnection(connectionString))
//                {
//                    SqlCommand command = new SqlCommand(query, connection);
//                    command.Parameters.AddWithValue("@ConstanciaId", constanciaId);

//                    connection.Open();
//                    SqlDataReader reader = command.ExecuteReader();
//                    if (reader.Read())
//                    {
//                        estado = reader["Descripcion_Estado"].ToString();
//                        fechaCreacion = reader["FechaCreacion"].ToString();
//                        otrosDatos = reader["Observaciones"].ToString();
//                    }
//                }

//                return $"{constanciaId}|{estado}|{fechaCreacion}|{otrosDatos}";
//            }
//            catch (Exception ex)
//            {
//                // Log the exception (logging mechanism to be implemented as needed)
//                // For simplicity, we are returning the error message as part of the response
//                // In a real application, consider logging the error to a file, database, or monitoring service
//                return $"Error: {ex.Message}";
//            }
//        }
//    }
//}

using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

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

            bool isValid = ValidateConstancia(constanciaId, clave, out string estado);

            if (isValid)
            {
                // Mostrar el popup con la información si la constancia es válida
                string script = "setTimeout(function() {popupSeguimiento.Show(); }, 500);";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", script, true);
            }
            else
            {
                lblMensaje.Text = "Número de constancia o clave incorrectos.";
            }
        }

        private bool ValidateConstancia(string constanciaId, string clave, out string estado)
        {
            estado = string.Empty;
            bool isValid = false;

            string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
            string query = "SELECT Estado FROM Constancias WHERE SolicitudId = @ConstanciaId AND Clave = @Clave";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ConstanciaId", constanciaId);
                    command.Parameters.AddWithValue("@Clave", clave);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        estado = result.ToString();
                        isValid = true;
                    }
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = "Error al validar la constancia: " + ex.Message;
                }
            }

            return isValid;
        }

        [WebMethod]
        public static string GetSessionValues(string constanciaId)
        {
            try
            {
                string estado = string.Empty;
                string fechaCreacion = string.Empty;
                string otrosDatos = string.Empty;
                string enlaceDescarga = "#"; // Por defecto, no se muestra enlace de descarga

                string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
                string query = @"
                        SELECT c.FechaCreacion, 
                               c.Observaciones, 
                               e.Descripcion_Estado, 
                               c.EnlaceArchivo
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

                        // Si la constancia está lista, se agrega el enlace de descarga
                        if (estado.ToLower() == "lista")
                        {
                            enlaceDescarga = reader["EnlaceArchivo"].ToString();
                        }
                    }
                }

                return $"{constanciaId}|{estado}|{fechaCreacion}|{otrosDatos}|{enlaceDescarga}";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
