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


namespace SG_Constancia_TSC
{
    public partial class Solicitud : System.Web.UI.Page
    {
        readonly ConexionBD conex = new ConexionBD();
        private const string SessionKey = "UploadedFile";

        public string MyDir { get; private set; }
        protected void Page_Load(object sender, EventArgs e)
        {

            Form.Attributes.Add("autocomplete", "off");

            //phDenunciado.Visible = true;


        }

        protected void FillCityCombo(string country)
        {
            if (string.IsNullOrEmpty(country)) return;

        }

        protected void CmbCity2_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            FillCityCombo2(e.Parameter);
        }

        protected void FillCityCombo2(string country)
        {
            if (string.IsNullOrEmpty(country)) return;

        }

        protected void CmbCity_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            FillCityCombo(e.Parameter);
        }

        protected void Callback_Callback(object source, CallbackEventArgs e)
        {
            Session.Remove(SessionKey);
            e.Result = "OK";
        }

        protected static string llama_plantilla(string mi_plantilla)
        {
            System.Text.ASCIIEncoding v_codigoAscii = new System.Text.ASCIIEncoding();
            System.Net.WebClient v_recibeDato = new System.Net.WebClient();
            byte[] v_recibe;
            string v_recibecadena;

            try
            {
                v_recibe = v_recibeDato.DownloadData(mi_plantilla);
                v_recibecadena = v_codigoAscii.GetString(v_recibe);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + ex.ToString());
            }

            return v_recibecadena;

        }

        protected void OK_Click(object sender, EventArgs e)
        {

            Response.Redirect("http://localhost:63127/AutoenrolamientoDJL.aspx");
        }

        public class UploadedFile
        {
            public string FileId { get; set; }
            public string FileName { get; set; }
            public byte[] FileBytes { get; set; }
            // Puedes agregar más propiedades según sea necesario
        }
        protected void UploadControl_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            int maxAllowedFiles = 5; // Cambia este valor según tus necesidades

            if (e.IsValid)
            {
                // Verifica la cantidad de archivos ya cargados
                List<UploadedFile> uploadedFilesList = (List<UploadedFile>)Session["UploadedFilesList"] ?? new List<UploadedFile>();

                if (uploadedFilesList.Count <= maxAllowedFiles)
                {
                    // Aún no se ha alcanzado el límite, por lo que puedes procesar el archivo
                    UploadedFile uploadedFile = new UploadedFile
                    {
                        FileId = Guid.NewGuid().ToString(),
                        FileName = e.UploadedFile.FileName,
                        FileBytes = e.UploadedFile.FileBytes
                        // Puedes agregar más propiedades según sea necesario
                    };

                    // Agrega el archivo a la lista temporal
                    uploadedFilesList.Add(uploadedFile);
                    Session["UploadedFilesList"] = uploadedFilesList;

                    // Construye una cadena con la lista de archivos y asigna al CallbackData
                    string fileList = string.Join("|", uploadedFilesList.Select(file => file.FileName));
                    e.CallbackData = fileList;

                }
                else
                {
                    // Se ha alcanzado el límite, así que marca el archivo como inválido
                    e.ErrorText = "No se permiten más de " + maxAllowedFiles + " archivos simultáneamente.";
                    e.IsValid = false;
                }


            }

        }

        protected void ASPxCallback_EnviarToken_Callback(object source, CallbackEventArgs e)
        {
            string email = e.Parameter;
            string token = GenerateToken();

            SG_Constancia_TSC.App_Start.Util_g.SetToken(token, DateTime.Now);

            SampleUtil.SendToken(email, token);

            e.Result = "Token enviado";
        }

        protected void ASPxCallback_VerificarToken_Callback(object source, CallbackEventArgs e)
        {
            string inputToken = e.Parameter;
            var (storedToken, tokenTimestamp) = SG_Constancia_TSC.App_Start.Util_g.GetToken();

            if (storedToken != null && tokenTimestamp != null)
            {
                double elapsedMinutes = (DateTime.Now - tokenTimestamp.Value).TotalMinutes;
                //System.Diagnostics.Debug.WriteLine($"Token age in minutes: {elapsedMinutes}");

                if (elapsedMinutes <= 15)
                {
                    SG_Constancia_TSC.App_Start.Util_g.SetToken(storedToken, tokenTimestamp.Value);

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

        private string GenerateToken()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        protected void ASPxCallback_Guardar_Datos_Callback(object source, CallbackEventArgs e)
        {
            try
            {
                SqlCommand cmd = SetupCommand();
                PopulateParameters(cmd);
                ExecuteDatabaseCommand(cmd);
                PrepareResponse(e, cmd);
                if (CheckForSuccess(cmd))
                {
                    ManageReportCreationAndEmailing(cmd);
                }
            }
            catch (Exception ex)
            {
                HandleException(e, ex);
            }
        }

        private void PrepareResponse(CallbackEventArgs e, SqlCommand cmd)
        {
            var responseObj = new
            {
                Retorno = Convert.ToInt32(cmd.Parameters["@RETORNO"].Value),
                Mensaje = cmd.Parameters["@MENS"].Value.ToString(),
            };
            e.Result = JsonConvert.SerializeObject(responseObj);
        }
        private SqlCommand SetupCommand()
        {
            var cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[gral].[sp_gestion_PreRegistro]",
                Connection = conex.conexion
            };
            return cmd;
        }
        private void PopulateParameters(SqlCommand cmd)
        {
            string email = tbCorreo.Text;
        //string DNI = tbDNI.Text;
            string FirstName = tbNombre.Text;
            string LastName = tbApellido.Text;
        //string Address = tbDependencia.Text;
            //string Company = CmbCountry.Text;
        //string Phone = tbCelular.Text;
            //string TypeDeclaration = CmbTipoDeclaracion.Text;
            bool Policy = ckPolitica.Checked;
            cmd.Parameters.Add("@pcEmail", SqlDbType.NVarChar, 500).Value = email;
            //cmd.Parameters.Add("@pnDNI", SqlDbType.NVarChar, 500).Value = DNI;
            cmd.Parameters.Add("@pcFirstName", SqlDbType.NVarChar, 500).Value = FirstName;
            cmd.Parameters.Add("@pcLastName", SqlDbType.NVarChar, 500).Value = LastName;
            //cmd.Parameters.Add("@pnAddress", SqlDbType.NVarChar, 500).Value = Address;
            //cmd.Parameters.Add("@pcCompany", SqlDbType.NVarChar, 500).Value = Company;
            //cmd.Parameters.Add("@pnPhone", SqlDbType.NVarChar).Value = Phone;
            //cmd.Parameters.Add("@pcTypeDeclaration", SqlDbType.NVarChar, 500).Value = TypeDeclaration;
            cmd.Parameters.Add("@pbPolitica", SqlDbType.Bit).Value = Policy;
            cmd.Parameters.Add("@MENS", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@RETORNO", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

        }

        private void ExecuteDatabaseCommand(SqlCommand cmd)
        {
            try
            {
                conex.AbrirConexion();
                cmd.ExecuteNonQuery();
                string Mens = cmd.Parameters["@MENS"].Value.ToString();
                int Retorno = Convert.ToInt32(cmd.Parameters["@RETORNO"].Value);
                Session["Id"] = Convert.ToInt32(cmd.Parameters["@Id"].Value);
            }
            finally
            {
                conex.CerrarConexion();
            }
        }

        private bool CheckForSuccess(SqlCommand cmd)
        {
            int retorno = Convert.ToInt32(cmd.Parameters["@RETORNO"].Value);
            return retorno == 1;
        }


        private void ManageReportCreationAndEmailing(SqlCommand cmd)
        {
            string encryptedId = SG_Constancia_TSC.App_Start.Util_g.Encrypt(Session["Id"].ToString());
            Reportes.Comprobante report = GenerarReporte(encryptedId);
            AgregarCodigoQRAlReporte(report, encryptedId);

            byte[] pdfDocument = ExportReportToPDF(report);
            if (pdfDocument != null && pdfDocument.Length > 0)
            {
                SavePDFToDatabase(pdfDocument);
                SendEmailWithPDFAttachment(pdfDocument);
            }
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
        private byte[] ExportReportToPDF(Reportes.Comprobante report)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                report.ExportToPdf(stream);
                return stream.ToArray(); // Convierte el stream a un arreglo byte directamente.
            }
        }

        private int SavePDFToDatabase(byte[] pdfDocument)
        {
            using (SqlCommand cmd = new SqlCommand("[gral].[sp_Guardar_PDF]", conex.conexion))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Session["Id"];
                cmd.Parameters.Add("@pdfDocument", SqlDbType.VarBinary).Value = pdfDocument;
                cmd.Parameters.Add("@MENS", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@RETORNO", SqlDbType.Int).Direction = ParameterDirection.Output;

                conex.AbrirConexion();
                cmd.ExecuteNonQuery();
                conex.CerrarConexion();

                return Convert.ToInt32(cmd.Parameters["@RETORNO"].Value);
            }
        }


        private void SendEmailWithPDFAttachment(byte[] pdfDocument)
        {
            string emailTemplatePath = Server.MapPath("~/Mensaje_Registro.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            string subject = "Solicitud de Constancia se Registro Exitosamente";
            string ccEmail = "jhmedina@tsc.gob.hn";

            using (var pdfStream = new MemoryStream(pdfDocument))
            {
                SampleUtil.EnviarCorreo(pdfStream, subject, tbCorreo.Text, emailBody, ccEmail);
            }
        }

        protected void callbackpane_comprobante_callback(object sender, CallbackEventArgsBase e)
        {
            // generar el identificador encriptado una sola vez y pasarlo a otros métodos
            string encryptedid = SG_Constancia_TSC.App_Start.Util_g.Encrypt(Session["id"].ToString());
            
            Reportes.Comprobante report = GenerarReporte(encryptedid);

            // agregar el código qr al reporte
            AgregarCodigoQRAlReporte(report, encryptedid);

            // mostrar el reporte en el visor
            ASPxWebDocumentViewer1.OpenReport(report);
        }

        protected Reportes.Comprobante GenerarReporte(string encryptedId)
        {
            Reportes.Comprobante report = new Reportes.Comprobante();
            report.Parameters["Id"].Value = SG_Constancia_TSC.App_Start.Util_g.Decrypt(encryptedId);
            return report;
        }

        protected void AgregarCodigoQRAlReporte(Reportes.Comprobante report, string encryptedId)
        {
            XRBarCode xrBarCode = new XRBarCode
            {
                Symbology = new QRCodeGenerator
                {
                    CompactionMode = QRCodeCompactionMode.Byte,
                    ErrorCorrectionLevel = QRCodeErrorCorrectionLevel.H,
                    Version = QRCodeVersion.Version4
                },
                Text = "https://presdjl.tsc.gob.hn/Doc.aspx?id=" + encryptedId,
                ShowText = false,
                AutoModule = true,
                WidthF = 176.46f,
                HeightF = 179.58f,
                LocationF = new PointF(235.42f, 19.79f),
                Module = 2,
                Padding = new PaddingInfo(10, 10, 10, 10)
            };
            report.Bands["GroupFooter1"].Controls.Add(xrBarCode);
        }

        //protected async Task btnUpload_ClickAsync(object sender, EventArgs e)
        //{

        //}
        protected async Task MyUploadFileAsync()
        {
             if (fileUpload.HasFile && fileUpload.PostedFile.ContentLength > 0)
            {
                int idFile = Convert.ToInt32(UtilClass.UtilClass.FileId_ident);
                    //if (!int.TryParse(UtilClass.UtilClass.fileId_ident, out idFile))
                    //{
                    //    lblMessage.Text = "Invalid File ID.";
                    //    return;
                    //}

                    SubirArchivo_D subirArchivo = new SubirArchivo_D();
                    string FlexfieldKey = UtilClass.UtilClass.flexFieldKeyIDENTIDAD; // CargaDocumento
                                                                                     //subirArchivo.FlexfieldValue = modeloDocumento.codigo_ticket.ToString();
                    subirArchivo.FlexfieldKey = FlexfieldKey;
                    subirArchivo.FlexfieldValue = "1";
                    //var flexFieldString = JsonConvert.SerializeObject(subirArchivo);

                    //string flexFieldsValue = "0";//txtFlexFields.Text;
                    string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;// txtConnectionString.Text;
                    HttpPostedFile file = fileUpload.PostedFile;
                    var flexFieldString = JsonConvert.SerializeObject(subirArchivo);


                    //response = await services.CargaDocumento.SubirArchivo(UtilClass.fileIdDocumentoSeguimiento, evidencia, flexFieldString);
                    //if (string.IsNullOrEmpty(response.result.ToString()) || response.typeResult.Equals(UtilClass.codigoError))
                    //{
                    //    response.result = new { };
                    //    response.message = "Ocurrió un error inesperado al intentar cargar el archivo.";
                    //    return Content(JsonConvert.SerializeObject(response), "application/json");
                    //}
                    //codReferencia = Convert.ToInt32(response.result);
                    //modeloDocumento.referenciaDocumento = codReferencia;

                    var result = await SubirArchivo.SubirArchivo_t(idFile, file, flexFieldString, connectionString);

                            if (result.typeResult == UtilClass.UtilClass.codigoExitoso)
                            {
                                lblMessage.Text = "File uploaded successfully!";
                            }
                            else
                            {
                                lblMessage.Text = $"File upload failed: {result.message}";
                            }
                        }
                        else
            {
                lblMessage.Text = "Please select a file.";
            }
        }   



        protected void btnUpload_Click(object sender, EventArgs e)
        {
            _ = MyUploadFileAsync();
        }
    }

    // 28 junio 2024
    // carga de archivos

    //protected async void btnUpload_Click(object sender, EventArgs e)
    //{
    //    if (fileUpload.HasFile && fileUpload.PostedFile.ContentLength > 0)
    //    {
    //        int idFile;
    //        if (!int.TryParse(txtIdFile.Text, out idFile))
    //        {
    //            lblMessage.Text = "Invalid File ID.";
    //            return;
    //        }

    //        string flexFields = txtFlexFields.Text;
    //        string connectionString = txtConnectionString.Text;
    //        HttpPostedFile file = fileUpload.PostedFile;

    //        var result = await SubirArchivo(idFile, file, connectionString, flexFields);

    //        if (result.typeResult == UtilClass.codigoExitoso)
    //        {
    //            lblMessage.Text = "File uploaded successfully!";
    //        }
    //        else
    //        {
    //            lblMessage.Text = $"File upload failed: {result.message}";
    //        }
    //    }
    //    else
    //    {
    //        lblMessage.Text = "Please select a file.";
    //    }
    //}
}



