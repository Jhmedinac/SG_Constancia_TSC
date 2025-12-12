using DevExpress.Web;
using SG_Constancia_TSC.App_Start;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.BarCode;
using System.Web;
using System.Drawing;
using DevExpress.XtraPrinting;
using System.Web.UI;
using System.Web.SessionState;
using SG_Constancia_TSC;
using DevExpress.Xpo.Logger;
using System.Web.UI.WebControls;
using SG_Constancia_TSC.Controllers;
using System.Configuration;
using DevExpress.DataAccess.Native.Web;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Web.Services;
using ZXing.QrCode;
using ZXing;
using System.Diagnostics;


namespace SG_Constancia_TSC
{
    public partial class Solicitud : System.Web.UI.Page
    {
        readonly ConexionBD conex = new ConexionBD();
        UploadFilesHandler uploadFilesHandler = new UploadFilesHandler();

        protected async void ASPxCallback_Guardar_Datos_Callback(object source, CallbackEventArgs e)

        {
            string uploadResponse = e.Parameter;

            

            // Devuelve el resultado como una cadena JSON
            e.Result = uploadResponse; // O cualquier procesamiento adicional que se desee hacer

        }




        protected void ASPxCallback_EnviarToken_Callback(object source, CallbackEventArgs e)
        {
            string email = e.Parameter;
            string token = GenerateToken();
         
            Util_g.SetToken(token, DateTime.Now);

            SampleUtil.SendToken(email, token);

            e.Result = "Token enviado";
            
        }

        private string GenerateToken()
        {
            return new Random().Next(100000, 999999).ToString();
        }
        protected void ASPxCallback_VerificarToken_Callback(object source, CallbackEventArgs e)
        {
            string inputToken = e.Parameter;
            var (storedToken, tokenTimestamp) = Util_g.GetToken();

            if (storedToken != null && tokenTimestamp != null)
            {
                double elapsedMinutes = (DateTime.Now - tokenTimestamp.Value).TotalMinutes;
             

                if (elapsedMinutes <= 15)
                {
                    Util_g.SetToken(storedToken, tokenTimestamp.Value);

                    if (inputToken == storedToken)
                    {
                        e.Result = "success";
                       
                    }
                    else
                    {
                        e.Result = "incorrect";
                      
                    }
                }
                else
                {
                    e.Result = "expired";
                   
                    HttpContext.Current.Session.Remove("VerificationToken");
                    HttpContext.Current.Session.Remove("TokenTimestamp");
                }
            }
            else
            {
                e.Result = "failure";
                
            }
        }

        /*VALIDACIONES DE LOS CAMPOS DEL PASO 1*/
        /*Nombres*/
        private bool IsValidNombreApellido(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Sin guiones ni puntos
            return System.Text.RegularExpressions.Regex.IsMatch(
            value,
             @"^[\p{L}\s']+$", // Solo letras Unicode, espacios y apóstrofes
               System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }


        protected void tbNombre_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            string nombre = e.Value?.ToString() ?? string.Empty;

            if (!IsValidNombreApellido(nombre))
            {
                e.IsValid = false;
                e.ErrorText = "Los nombres solo pueden contener letras y espacios.";
            }
        }

        /*Apellidos*/
        protected void tbApellido_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            string apellido = e.Value?.ToString() ?? string.Empty;

            if (!IsValidNombreApellido(apellido))
            {
                e.IsValid = false;
                e.ErrorText = "Los apellidos solo pueden contener letras y espacios.";
            }
        }

        
        public bool IsValidID(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            if (HasSpecialCharacters(id))
                return false;

            // Rechazamos IDs con más de 13 caracteres
            if (id.Length > 13)
                return false;

            // DNI: 13 dígitos numéricos
            if (id.Length == 13 && IsNumeric(id))
                return true;

            // Pasaporte: entre 6 y 9 caracteres alfanuméricos
            if (id.Length >= 6 && id.Length <= 9 && IsAlphanumeric(id))
                return true;

            // Carnet de residencia: 8 caracteres alfanuméricos
            if (id.Length == 8 && IsAlphanumeric(id))
                return true;

            return false;
        }


        private bool IsValidTelefono(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Solo permite números
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            // Opcional: establece la cantidad mínima o máxima de dígitos
            int minLength = 8;   // Honduras: usualmente 8 dígitos
            int maxLength = 8;

            return value.Length >= minLength && value.Length <= maxLength;
        }


        protected void tbTelefono_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            string telefono = e.Value?.ToString() ?? string.Empty;

            if (!IsValidTelefono(telefono))
            {
                e.IsValid = false;
                e.ErrorText = "Solo se permiten números. Debe tener 8 dígitos.";
            }
        }


        private bool IsNumeric(string str)
        {
            return int.TryParse(str, out _);
        }

        private bool IsAlphanumeric(string str)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(str, "^[a-zA-Z0-9]+$");
        }

        private bool HasSpecialCharacters(string str)
        {
            return !System.Text.RegularExpressions.Regex.IsMatch(str, "^[a-zA-Z0-9]+$");
        }


        private void HandleException(CallbackEventArgs e, Exception ex)
        {
            var responseObj = new
            {
                Retorno = 0,
                Mensaje = "Error: " + ex.Message
            };
            e.Result = JsonConvert.SerializeObject(responseObj);
        }

        [WebMethod]
        public static string GetSessionValues(string email, string constanciaId, string randPassword)
        {
            // Generar código QR
            string qrCodeImageUrl = GenerateQRCode(constanciaId); // Aquí llamas al método para generar el QR code

            // Enviar el correo electrónico
            SendEmail(constanciaId, randPassword, qrCodeImageUrl, email);

          
            return $"{qrCodeImageUrl}";
        }

        public static string GenerateQRCode(string encryptedConstanciaId)
        {
            var qrWriter = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = 180,
                    Width = 180,
                    Margin = 1
                }
            };

            //var pixelData = qrWriter.Write("https://xxxxx/Solicitud.aspx?constanciaid=" + encryptedConstanciaId);
            var pixelData = qrWriter.Write("http://localhost:59458/Solicitud.aspx?constanciaid=" + encryptedConstanciaId);

            using (var bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppArgb))
            using (var ms = new MemoryStream())
            {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                 ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                try
                {
                    // Transfer the image data to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }

                // Save to memory stream as PNG
                bitmap.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return "data:image/png;base64," + base64String;
            }
        }

        private static void SendEmail(string constanciaId, string randPassword, string qrCodeImageUrl, string adressEmail)
        {
            string emailBody = @"
 <table style='width:100%; max-width:600px; margin:0 auto; font-family:Segoe UI, sans-serif; background-color:#f9f9f9; padding:20px; color:#333; border-radius:8px; box-shadow:0 2px 6px rgba(0,0,0,0.1);'>
    <tr><td colspan='2' style='text-align:center;'>
        <h2 style='color:#2e7d32; margin-bottom:10px;'>✅ ¡Confirmación de Solicitud!</h2>
    </td></tr>

    <tr><td colspan='2' style='padding:10px 0;'>Para dar seguimiento a su solicitud siga los siguientes pasos:</td></tr>

    <tr><td colspan='2' style='padding:5px 0;'>1️⃣ Guarde el <strong>número de solicitud</strong> y la <strong>clave</strong> de seguimiento.</td></tr>
    <tr><td colspan='2' style='padding:5px 0;'>2️⃣ Ingrese al portal: <a href='https://www.tsc.gob.hn/' style='color:#000000; text-decoration:none;'>www.tsc.gob.hn</a></td></tr>
    <tr><td colspan='2' style='padding:5px 0;'>3️⃣ Vaya a la sección de <strong>Seguimiento de solicitud de constancias</strong>.</td></tr>
    <tr><td colspan='2' style='padding:5px 0;'>4️⃣ Seleccione <strong>Seguimiento de solicitud de constancias</strong> e ingrese los datos solicitados.</td></tr>

    <tr><td colspan='2' style='padding:15px 0;'>
        <strong>Número de solicitud:</strong> <span style='color:#000000; font-size:16px;'>" + constanciaId + @"</span><br>
        <strong>Clave de seguimiento:</strong> <span style='color:#000000; font-size:16px;'>" + randPassword + @"</span>
    </td></tr>


    
</table>";
            string subject = "Confirmación de Solicitud de Constancia de no tener cuentas pendientes con el Estado de Honduras";
            string ccEmail = null;

            // Aquí se llama a la función para enviar el correo electrónico
            SampleUtil.EnviarCorreo1("", subject, adressEmail, emailBody);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
          
        }
    }

}