using SG_Constancia_TSC.App_Start;
using SG_Constancia_TSC.Reportes;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Services;
using System.Web.UI;

namespace SG_Constancia_TSC
{
    public partial class Seguimiento : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LimpiarCampos();
            }

        }

        private void LimpiarCampos()
        {
            txtCorreo.Text = string.Empty;
            txtVerficacion.Text = string.Empty;
            txtConstanciaId.Text = string.Empty;
            txtClave.Text = string.Empty;
        }

        private string GenerateToken()
        {
            return new Random().Next(100000, 999999).ToString();
        }




        protected void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = ""; // Limpiar mensaje por si viene de otro botón

            try
            {

                string numeroConstancia = HttpUtility.HtmlEncode(txtConstanciaId.Text.Trim());
                string clave = HttpUtility.HtmlEncode(txtClave.Text.Trim());
                string email = HttpUtility.HtmlEncode(txtCorreo.Text.Trim());

                // Validar si el campo está vacío
                if (string.IsNullOrWhiteSpace(email))
                {
                    MostrarAlertaSwal("¡Alerta!", "Por favor,ingrese un correo electrónico antes de enviar el código.", "warning");
                    return;
                }



                // Generar código aleatorio
                string token = GenerateToken();
                Session["CodigoVerificacion"] = token;
                Session.Timeout = 10; // Expira en 10 minutos

                if (SendVerificationEmail(email, token))
                {
                    MostrarAlertaSwal("¡Éxito!", "Por favor ingrese el Código de verificación enviado a su correo electrónico.", "success");
                }
                else
                {
                    MostrarAlertaSwal("Error", "No se pudo enviar el código. Intente nuevamente.", "error");
                }



            }
            catch (Exception ex)
            {
                MostrarAlertaSwal("Error inesperado", ex.Message, "error");
            }
        }

        private void MostrarAlertaSwal(string titulo, string mensaje, string icono)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SwalMensaje", $@"
        Swal.fire({{
            title: '{titulo}',
            text: '{mensaje}',
            icon: '{icono}',
            confirmButtonColor: '#1F497D'
        }});
    ", true);
        }




        private void RegistrarScriptOcultarMensaje()
        {
            string script = @"
        setTimeout(function () {
            var label = document.getElementById('" + lblMensaje.ClientID + @"');
            if (label) { label.innerText = ''; }
        }, 20000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "ocultarLblMensaje", script, true);
        }


        private bool CorreoExisteEnBD(string correo)
        {
            string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT COUNT(1) FROM Solicitudes WHERE Email = @Correo";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
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
            lblMensaje.Text = ""; // Limpieza inicial

            string solicitud = txtConstanciaId.Text.Trim();
            string Sclave = txtClave.Text.Trim();

            // Validar si los campos están vacíos
            if (string.IsNullOrWhiteSpace(solicitud) || string.IsNullOrWhiteSpace(Sclave))
            {
                MostrarAlertaSwal("¡Alerta!", " Debe completar el paso 1 y luego ingresar el número de solicitud y la clave.", "warning");
                return;
            }

            string codigoIngresado = HttpUtility.HtmlEncode(txtVerficacion.Text.Trim());
            string constanciaId = HttpUtility.HtmlEncode(txtConstanciaId.Text.Trim());
            string clave = HttpUtility.HtmlEncode(txtClave.Text.Trim());

            string codigoGuardado = Session["CodigoVerificacion"] as string;

            if (string.IsNullOrEmpty(codigoGuardado) || !string.Equals(codigoIngresado, codigoGuardado, StringComparison.Ordinal))
            {
                MostrarAlertaSwal("¡Error!", "Código de verificación incorrecto. Por favor, inténtelo de nuevo.", "error");
                RegistrarScriptOcultarMensaje();
                return; 
            }

            bool isValid = ValidateConstancia(constanciaId, clave, out string estado);

            if (isValid)
            {
                string script = $@"
            setTimeout(function() {{
                showConfirmationMessage2('{constanciaId}');
                txtConstanciaId.SetText('');
                txtClave.SetText('');
                txtVerficacion.SetText('');
                txtCorreo.SetText('');
            }}, 500);";

                ClientScript.RegisterStartupScript(this.GetType(), "ShowPopup", script, true);

                // Remover solo en éxito
                Session.Remove("CodigoVerificacion");
            }
            else
            {
                MostrarAlertaSwal("¡Error!", "Número de solicitud o clave de seguimiento incorrectos.", "error");
                RegistrarScriptOcultarMensaje();
               
            }
        }


        protected void txtCodigoVerificacion_TextChanged(object sender, EventArgs e)
        {
            string codigoIngresado = txtVerficacion.Text.Trim();
            string codigoGuardado = Session["CodigoVerificacion"] as string;

            if (!string.IsNullOrEmpty(codigoGuardado) && codigoIngresado == codigoGuardado)
            {
                MostrarAlertaSwal("¡Éxito!", "Código verificado correctamente.", "success");


                // Habilitar el botón de búsqueda
                btnBuscar.Enabled = true;
            }
            else
            {
                MostrarAlertaSwal("¡Error!", "Código incorrecto. Intente nuevamente.", "error");
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
                   c.Archivoconstancia,
                   c.ArchivoConstanciaSecretaria
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
                        if (reader["ArchivoConstanciaSecretaria"] != DBNull.Value)
                        {
                            byte[] archivoconstancia = (byte[])reader["ArchivoConstanciaSecretaria"];
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


    }

}


