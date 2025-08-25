using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SG_Constancia_TSC
{
    using DevExpress.Web;
    using SG_Constancia_TSC.App_Start;
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Web.UI;

    public partial class VerificarConstancia : Page
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
            txtCodigo.Text = string.Empty;
            txtVerficacion.Text = string.Empty;
            txtCorreo.Text = string.Empty;
        }

        private string GenerateToken()
        {
            return new Random().Next(100000, 999999).ToString();
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
        protected void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            lblMensaje.Text = ""; // Limpiar mensaje por si viene de otro botón

            try
            {
                string email = HttpUtility.HtmlEncode(txtCorreo.Text.Trim());

                // Validar si está vacío
                if (string.IsNullOrWhiteSpace(email))
                {
                    MostrarAlertaSwal("¡Alerta!", "Debe ingresar un correo electrónico antes de enviar el código.", "warning");
                    return;
                }

                // Validar si el correo existe en la base de datos
                //if (!CorreoExisteEnBD(email))
                //{
                //    MostrarAlertaSwal("Correo no encontrado", "El correo ingresado no está registrado.", "error");
                //    return;
                //}

                // Generar código y guardar en sesión
                string token = GenerateToken();
                Session["CodigoVerificacion"] = token;
                Session.Timeout = 10;

                // Enviar correo
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

        //private bool CorreoExisteEnBD(string correo)
        //{
        //    string connStr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(connStr))
        //    {
        //        string query = "SELECT COUNT(1) FROM Solicitudes WHERE Email = @Correo";
        //        using (SqlCommand cmd = new SqlCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@Correo", correo);
        //            conn.Open();
        //            int count = (int)cmd.ExecuteScalar();
        //            return count > 0;
        //        }
        //    }
        //}

        private void RegistrarScriptOcultarMensaje()
        {
            string script = @"
        setTimeout(function () {
            var label = document.getElementById('" + lblMensaje.ClientID + @"');
            if (label) { label.innerText = ''; }
        }, 20000);";

            ScriptManager.RegisterStartupScript(this, GetType(), "ocultarLblMensaje", script, true);
        }


        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            if (string.IsNullOrWhiteSpace(codigo))
            {
                MostrarAlertaSwal("¡Alerta!", "Debe completar el paso 1 y luego ingresar el código de verificación de su constancia.", "warning");
                return;
            }

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"]?.ConnectionString;

            string query = @"
                       SELECT 
                       Identidad, 
                       FirstName + ' ' + LastName AS NombreCompleto, 
                       NumRtn,
                       NomInstitucion,
                       Descripcion_Estado, 
                       FechaAprobacion, 
                       Observaciones,
                       FechaTermino
                       FROM V_Solicitudes 
                       WHERE CodigoVerificacion = @Codigo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Codigo", codigo);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    string popupContent;

                    if (reader.Read())
                    {
                        bool esJuridica = !reader.IsDBNull(reader.GetOrdinal("NumRtn")) && !string.IsNullOrWhiteSpace(reader["NumRtn"].ToString());

                        if (esJuridica)
                        {
                            // Persona jurídica
                            string numRtn = reader["NumRtn"]?.ToString() ?? "N/A";
                            string nomInstitucion = reader["NomInstitucion"]?.ToString() ?? "Desconocido";

                            popupContent = $@"
                           <div class='alert alert-success p-3'>
                          <h4><i class='fa-solid fa-circle-check text-success'></i> Constancia Válida</h4>
                          <p><i class='fa-solid fa-id-card'></i> <strong>Número de RTN:</strong> {numRtn}</p>
                          <p><i class='fa-solid fa-building'></i> <strong>Empresa/Institución:</strong> {nomInstitucion}</p>
                          <p><i class='fa-solid fa-file-circle-check'></i> <strong>Estado:</strong> {reader["Descripcion_Estado"]}</p>
                          <p><i class='fa-solid fa-calendar-day'></i> <strong>Fecha de Aprobación:</strong> {reader["FechaAprobacion"]}</p>
                           <p><i class='fa-solid fa-comment-dots'></i> <strong>Observación:</strong> {reader["Observaciones"]}</p>
                           </div>";
                        }
                        else
                        {
                            // Persona natural
                            string identidad = reader["Identidad"]?.ToString() ?? "N/A";
                            string nombre = reader["NombreCompleto"]?.ToString() ?? "Desconocido";

                            popupContent = $@"
                               <div class='alert alert-success p-3'>
                               <h4><i class='fa-solid fa-circle-check text-success'></i> Constancia Válida</h4>
                               <p><i class='fa-solid fa-id-card'></i> <strong>Número de Identidad:</strong> {identidad}</p>
                               <p><i class='fa-solid fa-user'></i> <strong>Nombre Completo:</strong> {nombre}</p>
                               <p><i class='fa-solid fa-file-circle-check'></i> <strong>Estado:</strong> {reader["Descripcion_Estado"]}</p>
                               <p><i class='fa-solid fa-calendar-day'></i> <strong>Fecha de Aprobación:</strong> {reader["FechaAprobacion"]}</p>
                               <p><i class='fa-solid fa-comment-dots'></i> <strong>Observación:</strong> {reader["Observaciones"]}</p>
                              </div>";
                        }
                    }
                    else
                    {
                        popupContent = @"
                   <div class='alert alert-danger text-center p-3'>
                ❌ <strong>Código no válido.</strong> La constancia no existe o ha expirado.
                          </div>";
                    }

                    // Mostrar modal y limpiar
                    string script = $@"
                            <script>
                         document.getElementById('popupBodyContent').innerHTML = `{popupContent.Replace("`", "\\`")}`;
                         var popupModal = new bootstrap.Modal(document.getElementById('popupResultado'));
                         popupModal.show();
                         document.getElementById('{txtCodigo.ClientID}').value = '';
                         document.getElementById('{txtVerficacion.ClientID}').value = '';
                         document.getElementById('{txtCorreo.ClientID}').value = '';
                         </script>";

                    ClientScript.RegisterStartupScript(this.GetType(), "MostrarResultado", script);

                    LimpiarCampos();
                }
            }
        }

    }
}
