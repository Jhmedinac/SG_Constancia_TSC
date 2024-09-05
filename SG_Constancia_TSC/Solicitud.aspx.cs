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
//using Aspose.Email.Clients.ActiveSync.TransportLayer;
using System.Web.UI;
using System.Web.SessionState;
//using SG_Constancia_TSC.App_Start;
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

            // Procesa el uploadResponse aquí si es necesario

            // Devuelve el resultado como una cadena JSON
            e.Result = uploadResponse; // O cualquier procesamiento adicional que desees hacer

        }


        protected void ASPxCallback_EnviarToken_Callback(object source, CallbackEventArgs e)
        {
            string email = e.Parameter;
            string token = GenerateToken();
            //Debug.WriteLine("previo Token enviado");
            Util_g.SetToken(token, DateTime.Now);

            SampleUtil.SendToken(email, token);

            e.Result = "Token enviado";
            //Debug.WriteLine("Token enviado");
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
                //System.Diagnostics.Debug.WriteLine($"Token age in minutes: {elapsedMinutes}");

                if (elapsedMinutes <= 15)
                {
                    Util_g.SetToken(storedToken, tokenTimestamp.Value);

                    if (inputToken == storedToken)
                    {
                        e.Result = "success";
                        //System.Diagnostics.Debug.WriteLine("Token verification successful.");
                    }
                    else
                    {
                        e.Result = "incorrect";
                        //System.Diagnostics.Debug.WriteLine("Token is incorrect.");
                    }
                }
                else
                {
                    e.Result = "expired";
                    //System.Diagnostics.Debug.WriteLine("Token has expired.");
                    HttpContext.Current.Session.Remove("VerificationToken");
                    HttpContext.Current.Session.Remove("TokenTimestamp");
                }
            }
            else
            {
                e.Result = "failure";
                //System.Diagnostics.Debug.WriteLine("Token or timestamp missing.");
            }
        }

        /*validaciones campos*/

        protected void tbDNI_Validation(object sender, DevExpress.Web.ValidationEventArgs e)
        {
            string id = e.Value.ToString();
            if (!IsValidID(id))
            {
                e.IsValid = false;
                e.ErrorText = "El Número de identificación es incorrecto.";
            }
        }

        private bool IsValidID(string id)
        {
            // Validación para el Documento Nacional de Identificación (DNI)
            if (id.Length == 13 && IsNumeric(id))
            {
                return true;
            }
            // Validación para el pasaporte
            else if (id.Length >= 6 && id.Length <= 9 && IsAlphanumeric(id))
            {
                return true;
            }
            // Validación para el carnet de residencia
            else if (id.Length == 9 && IsAlphanumeric(id))
            {
                return true;
            }
            return false;
        }

        private bool IsNumeric(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private bool IsAlphanumeric(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
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
            // Obtener valores de sesión
            //string constanciaId = HttpContext.Current.Session["Id"] != null ? HttpContext.Current.Session["Id"].ToString() : "0";
            //string randPassword = HttpContext.Current.Session["Clave"] != null ? HttpContext.Current.Session["Clave"].ToString() : string.Empty;

            // Generar código QR
            string qrCodeImageUrl = GenerateQRCode(constanciaId); // Aquí llamas al método para generar el QR code

            // Enviar el correo electrónico
            SendEmail(constanciaId, randPassword, qrCodeImageUrl, email);

            //return $"{constanciaId}|{randPassword}|{qrCodeImageUrl}";
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
            string emailBody = "<table border='0' width='100%'>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2' align='center'><strong><font size='+2'>Solicitud recibida!</font></strong></td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2'></td></tr>" +
                               "<tr><td colspan='2'>Copia de su solicitud ha sido enviada a su correo electrónico.</td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2'>Si desea más adelante monitorear su solicitud, siga las siguientes instrucciones:</td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2'>1) Guarde el número de su solicitud y la clave que se le indican en esta página.</td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2'>2) Cuando desee monitorear su solicitud de constancia, ingrese al portal <font color='#d62d20'>https://www.tsc.gob.hn/</font>.</td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2'>3) Posteriormente, ingrese a la viñeta de solicitud de constancias.</td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2'>4) Ingrese al enlace Seguimiento de solicitud de constancias e ingrese los datos solicitados.</td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2'>Su número de solicitud es: <font color='#d62d20'>" + constanciaId + "</font>.</td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2'>Su clave para seguimiento de Constancia es: <font color='#d62d20'>" + randPassword + "</font>.</td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2' align='center'><img src='" + qrCodeImageUrl + "' alt='Código QR'></td></tr>" +
                               "<tr><td colspan='2'>&nbsp;</td></tr>" +
                               "<tr><td colspan='2' align='center'><a href='https://www.tsc.gob.hn/' class='Letrapagina'>Salir</a></td></tr>" +
                               "</table>";

            string subject = "Solicitud de Constancia se Registró Exitosamente";
            string ccEmail = null;

            // Aquí se llama a la función para enviar el correo electrónico
            SampleUtil.EnviarCorreo1("", subject, adressEmail, emailBody);

        }
    }
}