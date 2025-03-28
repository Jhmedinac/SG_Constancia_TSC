using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SG_Constancia_TSC
{
    using SG_Constancia_TSC.App_Start;
    // Página ASP.NET para verificar constancias
    using System;
    using System.Data.SqlClient;
    using System.Web.UI;

    public partial class VerificarConstancia : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
            try
            {
                //string numeroConstancia = HttpUtility.HtmlEncode(txtConstanciaId.Text.Trim());
                //string clave = HttpUtility.HtmlEncode(txtClave.Text.Trim());
                string email = HttpUtility.HtmlEncode(txtCorreo.Text.Trim());

                if (string.IsNullOrWhiteSpace(email))
                {
                    lblMensaje.Text = "Por favor, ingrese un correo electrónico válido.";
                    return;
                }

                //if (string.IsNullOrWhiteSpace(numeroConstancia) || string.IsNullOrWhiteSpace(clave))
                //{
                //    lblMensaje.Text = "Debe ingresar el número de constancia y la clave.";
                //    return;
                //}

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
            protected void btnVerificar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"]?.ConnectionString;

            string query = @"
        SELECT 
            Identidad, 
            FirstName + ' ' + LastName AS NombreCompleto, 
            Descripcion_Estado, 
            FechaAprobacion, 
            Observaciones 
        FROM V_Solicitudes 
        WHERE CodigoVerificacion = @Codigo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Codigo", codigo);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblResultado.Text = $@"
                    <div class='alert alert-success p-3'>
                        <h4><i class=""fa-solid fa-circle-check"" style=""color: #2e7d32;""></i> Constancia Válida</h4>
                        <p><i class=""fa-solid fa-id-card""></i> <span class='text-bold'>Identidad:</span> {reader["Identidad"]}</p>
                        <p><i class=""fa-solid fa-user""></i> <span class='text-bold'>Nombre Completo:</span> {reader["NombreCompleto"]}</p>
                        <p><i class=""fa-solid fa-file-circle-check""></i> <span class='text-bold'>Estado:</span> {reader["Descripcion_Estado"]}</p>
                        <p><i class=""fa-solid fa-calendar-day""></i> <span class='text-bold'>Fecha de Aprobación:</span> {reader["FechaAprobacion"]}</p>
                        <p><i class=""fa-solid fa-comment-dots""></i> <span class='text-bold'>Observaciones:</span> {reader["Observaciones"]}</p>
                    </div>
                ";
                    }
                    else
                    {
                        lblResultado.Text = @"
                    <div class='alert alert-danger text-center p-3'>
                        ❌ <strong>Código no válido.</strong> La constancia no existe o ha expirado.
                    </div>";
                    }
                }
            }
        }



    }
}
