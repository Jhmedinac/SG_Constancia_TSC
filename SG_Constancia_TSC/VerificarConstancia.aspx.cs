using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SG_Constancia_TSC
{
    using DevExpress.CodeParser;
    using DevExpress.Web;
    using SG_Constancia_TSC.App_Start;
    using System;
    using System.Configuration;
    using System.Data;
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
                    MostrarAlertaSwal("¡Alerta!", "Por favor,ingrese un correo electrónico antes de enviar el código.", "warning");
                    return;
                }



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
            string codigo = (txtCodigo.Text ?? "").Trim();
            string codigoIngresado = (txtVerficacion.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(codigo))
            {
                MostrarAlertaSwal("¡Alerta!", "Debe completar el paso 1 y luego ingresar el código de verificación de su constancia.", "warning");
                return;
            }

            // Valida el token enviado por correo (Paso 1)
            string codigoGuardado = Session["CodigoVerificacion"] as string;
            if (string.IsNullOrEmpty(codigoGuardado) || !string.Equals(codigoIngresado, codigoGuardado, StringComparison.Ordinal))
            {
                MostrarAlertaSwal("¡Error!", "Código de verificación incorrecto. Por favor, inténtelo de nuevo.", "error");
                RegistrarScriptOcultarMensaje();
                return;
            }

            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["connString"]?.ConnectionString;

            //  Ya no filtra por fecha, se calcula por “vigencia” en el SELECT
            string query = @"
SELECT TOP 1
    Identidad,
    FirstName + ' ' + LastName AS NombreCompleto,
    NumRtn,
    NomInstitucion,
    Descripcion_Estado,
    FechaAprobacion,
    Observaciones,
    FechaTermino,
    CASE 
      WHEN FechaAprobacion IS NULL THEN 0
      WHEN FechaAprobacion >= DATEADD(MONTH, -6, GETDATE()) THEN 1
      ELSE 0
    END AS Vigente
FROM V_Solicitudes
WHERE CodigoVerificacion = @Codigo;";

            using (var conn = new SqlConnection(cs))
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@Codigo", SqlDbType.NVarChar, 100).Value = codigo;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    string popupContent;

                    if (!reader.Read())
                    {
                        // ❌ No existe
                        popupContent = @"
<div class='alert alert-danger text-center p-3'>
  <p class='mb-1'>❌ <strong>Código Seguro de Verificación no válido</strong></p>
  <p class='mb-0'>La constancia no existe.</p>
</div>";
                    }
                    else
                    {
                        bool esVigente = reader["Vigente"] != DBNull.Value && Convert.ToInt32(reader["Vigente"]) == 1;

                        if (!esVigente)
                        {
                            //  Existe pero expiró
                            popupContent = @"
<div class='alert alert-warning text-center p-3'>
  <p class='mb-1'>⚠️ <strong>Constancia expirada</strong></p>
  <p class='mb-0'>La constancia excede los 6 meses de vigencia.</p>
</div>";
                        }
                        else
                        {
                            // Vigente → arma el contenido según natural/jurídica
                            bool esJuridica = !reader.IsDBNull(reader.GetOrdinal("NumRtn")) &&
                                              !string.IsNullOrWhiteSpace(reader["NumRtn"].ToString());

                            if (esJuridica)
                            {
                                string numRtn = reader["NumRtn"]?.ToString() ?? "N/A";
                                string nomInstitucion = reader["NomInstitucion"]?.ToString() ?? "Desconocido";

                                popupContent = $@"
<div class='alert alert-success p-3'>
  <h4><i class='fa-solid fa-circle-check text-success'></i> Constancia Válida</h4>
  <p><i class='fa-solid fa-id-card'></i> <strong>Número de RTN:</strong> {numRtn}</p>
  <p><i class='fa-solid fa-building'></i> <strong>Empresa/Institución:</strong> {nomInstitucion}</p>
  <p><i class='fa-solid fa-file-circle-check'></i> <strong>Estado:</strong> {reader["Descripcion_Estado"]}</p>
  <p><i class='fa-solid fa-calendar-day'></i> <strong>Fecha y hora de Aprobación:</strong> {reader["FechaAprobacion"]}</p>
  <p><i class='fa-solid fa-comment-dots'></i> <strong>Observación:</strong> {reader["Observaciones"]}</p>
</div>";
                            }
                            else
                            {
                                string identidad = reader["Identidad"]?.ToString() ?? "N/A";
                                string nombre = reader["NombreCompleto"]?.ToString() ?? "Desconocido";

                                popupContent = $@"
<div class='alert alert-success p-3'>
  <h4><i class='fa-solid fa-circle-check text-success'></i> Constancia Válida</h4>
  <p><i class='fa-solid fa-id-card'></i> <strong>Número de Identidad:</strong> {identidad}</p>
  <p><i class='fa-solid fa-user'></i> <strong>Nombre Completo:</strong> {nombre}</p>
  <p><i class='fa-solid fa-file-circle-check'></i> <strong>Estado:</strong> {reader["Descripcion_Estado"]}</p>
  <p><i class='fa-solid fa-calendar-day'></i> <strong>Fecha y hora de Aprobación:</strong> {reader["FechaAprobacion"]}</p>
  <p><i class='fa-solid fa-comment-dots'></i> <strong>Observación:</strong> {reader["Observaciones"]}</p>
</div>";
                            }
                        }
                    }

                    // Mostrar modal y limpiar inputs
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

