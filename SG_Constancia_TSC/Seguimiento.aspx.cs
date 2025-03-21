using SG_Constancia_TSC.App_Start;
using SG_Constancia_TSC.Reportes;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace SG_Constancia_TSC
{
    public partial class Seguimiento : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string GenerateToken()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        protected void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            try
            {
                string numeroConstancia = HttpUtility.HtmlEncode(txtConstanciaId.Text.Trim());
                string clave = HttpUtility.HtmlEncode(txtClave.Text.Trim());
                string email = HttpUtility.HtmlEncode(txtCorreo.Text.Trim());

                if (string.IsNullOrWhiteSpace(email))
                {
                    lblMensaje.Text = "Por favor, ingrese un correo electrónico válido.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(numeroConstancia) || string.IsNullOrWhiteSpace(clave))
                {
                    lblMensaje.Text = "Debe ingresar el número de constancia y la clave.";
                    return;
                }

                // Generar código aleatorio
                string token = GenerateToken();
                Session["CodigoVerificacion"] = token;
                Session.Timeout = 10; // Expira en 10 minutos

                if (SendVerificationEmail(email, token))
                {
                    lblMensaje.Text = "Código de verificación enviado correctamente a su correo.";
                    //divVerificacion.Style["display"] = "block"; // Mostrar la caja de verificación
                }
                else
                {
                    lblMensaje.Text = "Error al enviar el código. Intente de nuevo.";
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error inesperado: " + ex.Message;
            }
        }


        private bool SendVerificationEmail(string email, string token)
        {
            try
            {
                SampleUtil.SendToken(email, token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //string codigoIngresado1 = HttpUtility.HtmlEncode(txtCodigoVerificacion.Text.Trim());
            string codigoIngresado = HttpUtility.HtmlEncode(txtVerficacion.Text.Trim());
            string constanciaId = HttpUtility.HtmlEncode(txtConstanciaId.Text.Trim());
            string clave = HttpUtility.HtmlEncode(txtClave.Text.Trim());
            //string codigoIngresado = txtCodigoVerificacion.Text.Trim();
            string codigoGuardado = Session["CodigoVerificacion"] as string;

            //codigoIngresado == codigoGuardado;

            if (string.IsNullOrEmpty(codigoGuardado) || codigoIngresado != codigoGuardado)
            {
                lblMensaje.Text = "Código de verificación incorrecto.";
                return;
            }

            // Aquí se agregará la lógica para consultar la constancia en la base de datos.
            bool isValid = ValidateConstancia(constanciaId, clave, out string estado);

            if (isValid)
            {
                // Mostrar el popup con la información si la constancia es válida
                //string script = "setTimeout(function() {popupSeguimiento.Show(); }, 500);";
                //ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", script, true);
                // Llama a la función JavaScript para mostrar los datos en el popup
                string script = $"setTimeout(function() {{ showConfirmationMessage2('{constanciaId}'); }}, 500);";
                ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", script, true);
            }
            else
            {
                lblMensaje.Text = "Número de constancia o clave incorrectos.";
            }

            //lblMensaje.Text = "Consulta realizada correctamente.";
            Session.Remove("CodigoVerificacion"); // Eliminar código tras su uso
        }

        protected void txtCodigoVerificacion_TextChanged(object sender, EventArgs e)
        {
            string codigoIngresado = txtVerficacion.Text.Trim();
            string codigoGuardado = Session["CodigoVerificacion"] as string;

            if (!string.IsNullOrEmpty(codigoGuardado) && codigoIngresado == codigoGuardado)
            {
                lblMensaje.Text = "Código verificado correctamente.";
                lblMensaje.CssClass = "text-success";

                // Habilitar el botón de búsqueda
                btnBuscar.Enabled = true;
            }
            else
            {
                lblMensaje.Text = "Código incorrecto. Intente nuevamente.";
                lblMensaje.CssClass = "text-danger";
                btnBuscar.Enabled = false;
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
        public static bool VerificarCodigo(string codigoIngresado)
        {
            string codigoGuardado = System.Web.HttpContext.Current.Session["CodigoVerificacion"] as string;
            return codigoIngresado == codigoGuardado;
        }

        [WebMethod]
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
        //public static string GetSessionValues(string constanciaId)
        //{
        //    try
        //    {
        //        string estado = string.Empty;
        //        string fechaCreacion = string.Empty;
        //        string otrosDatos = string.Empty;
        //        string enlaceDescarga = "#"; // Por defecto, no se muestra enlace de descarga

        //        string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
        //        string query = @"
        //                SELECT c.FechaCreacion, 
        //                       c.Observaciones, 
        //                       e.Descripcion_Estado, 
        //                       c.Archivoconstancia
        //                FROM Constancias c
        //                INNER JOIN Estados e ON c.Estado = e.Id_Estado
        //                WHERE c.SolicitudId = @ConstanciaId";

        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            SqlCommand command = new SqlCommand(query, connection);
        //            command.Parameters.AddWithValue("@ConstanciaId", constanciaId);

        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                estado = reader["Descripcion_Estado"].ToString();
        //                fechaCreacion = reader["FechaCreacion"].ToString();
        //                otrosDatos = reader["Observaciones"].ToString();

        //                // Si la constancia está lista, se agrega el enlace de descarga
        //                if (estado.ToLower() == "finalizada")
        //                {
        //                    enlaceDescarga = reader["Archivoconstancia"].ToString();
        //                }
        //            }
        //        }

        //        return $"{constanciaId}|{estado}|{fechaCreacion}|{otrosDatos}|{enlaceDescarga}";
        //    }
        //    catch (Exception ex)
        //    {
        //        return $"Error: {ex.Message}";
        //    }
        //}
    }

}


