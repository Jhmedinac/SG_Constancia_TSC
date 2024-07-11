using DevExpress.Xpo.Logger;
using Newtonsoft.Json;
using SG_Constancia_TSC.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SG_Constancia_TSC.App_Start;

using SG_Constancia_TSC;

using DevExpress.DataAccess.Native.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DevExpress.Web.Internal.XmlProcessor;

using System.Data;
using DevExpress.Web;
using DevExpress.Export.Xl;

namespace SG_Constancia_TSC
{
    public partial class SolicitudTSC1 : System.Web.UI.Page
    {
        readonly ConexionBD conex = new ConexionBD();


        protected async Task<string> UploadSolicitudAsync(string idString)
        {
            lblMessage.Text = ""; // Inicializar lblMessage para asegurarse de que no es null

            if (fileUpload1.HasFile && fileUpload1.PostedFile.ContentLength > 0)
            {
                if (!int.TryParse(UtilClass.UtilClass.FileId_solicitud, out int idFile))
                {
                    lblMessage.Text = "Invalid File ID.";
                    //return "Invalid File ID.";
                    return lblMessage.Text;
                }

                SubirArchivo_D subirArchivo1 = new SubirArchivo_D();
                string FlexfieldKey = UtilClass.UtilClass.flexFieldKeySOLICITUD; // CargaDocumento
                                                                                 //subirArchivo.FlexfieldValue = modeloDocumento.codigo_ticket.ToString();
                subirArchivo1.FlexfieldKey = FlexfieldKey;
                subirArchivo1.FlexfieldValue = idString; /* llave registro creado*/
                //var flexFieldString = JsonConvert.SerializeObject(subirArchivo);

                //string flexFieldsValue = "0";//txtFlexFields.Text;
                string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;// txtConnectionString.Text;
                HttpPostedFile file1 = fileUpload1.PostedFile;
                var flexFieldString = JsonConvert.SerializeObject(subirArchivo1);

                string flexFields = flexFieldString; /*txtFlexFields.Text;*/
                //string connectionString = txtConnectionString.Text;

                if (string.IsNullOrEmpty(connectionString))
                {
                    lblMessage.Text = "Connection String cannot be empty.";
                    //return "Connection String cannot be empty.";
                    return lblMessage.Text;
                }

                //HttpPostedFile file = fileUpload.PostedFile;

                try
                {
                    var result = await SubirArchivo.SubirArchivo_t(idFile, file1, flexFields, connectionString);

                    if (result.typeResult == UtilClass.UtilClass.codigoExitoso)
                    {
                        lblMessage.Text = "File uploaded successfully!";
                    }
                    else
                    {
                        lblMessage.Text = $"File upload failed: {result.message}";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                lblMessage.Text = "Please select a file.";
            }
            return lblMessage.Text;
        }

        protected async Task<string> UploadReciboAsync(string idString)
        {
            lblMessage.Text = ""; // Inicializar lblMessage para asegurarse de que no es null

            if (fileUpload2.HasFile && fileUpload2.PostedFile.ContentLength > 0)
            {
                if (!int.TryParse(UtilClass.UtilClass.FileId_recibo, out int idFile))
                {
                    lblMessage.Text = "Invalid File ID.";
                    return lblMessage.Text;
                }

                SubirArchivo_D subirArchivo2 = new SubirArchivo_D();
                string FlexfieldKey = UtilClass.UtilClass.flexFieldKeyRECIBO; // CargaDocumento
                                                                              //subirArchivo.FlexfieldValue = modeloDocumento.codigo_ticket.ToString();
                subirArchivo2.FlexfieldKey = FlexfieldKey;
                subirArchivo2.FlexfieldValue = idString;
                //var flexFieldString = JsonConvert.SerializeObject(subirArchivo);

                //string flexFieldsValue = "0";//txtFlexFields.Text;
                string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;// txtConnectionString.Text;
                HttpPostedFile file2 = fileUpload2.PostedFile;
                var flexFieldString = JsonConvert.SerializeObject(subirArchivo2);

                string flexFields = flexFieldString; /*txtFlexFields.Text;*/
                //string connectionString = txtConnectionString.Text;

                if (string.IsNullOrEmpty(connectionString))
                {
                    lblMessage.Text = "Connection String cannot be empty.";
                    return lblMessage.Text;
                }


                //HttpPostedFile file = fileUpload.PostedFile;

                try
                {
                    var result = await SubirArchivo.SubirArchivo_t(idFile, file2, flexFields, connectionString);

                    if (result.typeResult == UtilClass.UtilClass.codigoExitoso)
                    {
                        lblMessage.Text = "File uploaded successfully!";
                    }
                    else
                    {
                        lblMessage.Text = $"File upload failed: {result.message}";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                lblMessage.Text = "Please select a file.";
            }
            return lblMessage.Text;
           
        }


        protected async Task<string> UploadIdentidadAsync(string idString)
        {

            lblMessage.Text = ""; // Inicializar lblMessage para asegurarse de que no es null

            if (fileUpload.HasFile && fileUpload.PostedFile.ContentLength > 0)
            {
                if (!int.TryParse(UtilClass.UtilClass.FileId_ident, out int idFile))
                {
                    lblMessage.Text = "Invalid File ID.";
                    return "Invalid File ID.";
                }

                SubirArchivo_D subirArchivo = new SubirArchivo_D();
                string FlexfieldKey = UtilClass.UtilClass.flexFieldKeyIDENTIDAD; // CargaDocumento
                                                                                 //subirArchivo.FlexfieldValue = modeloDocumento.codigo_ticket.ToString();
                subirArchivo.FlexfieldKey = FlexfieldKey;
                subirArchivo.FlexfieldValue = idString;
                //var flexFieldString = JsonConvert.SerializeObject(subirArchivo);

                //string flexFieldsValue = "0";//txtFlexFields.Text;
                string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;// txtConnectionString.Text;
                HttpPostedFile file = fileUpload.PostedFile;
                var flexFieldString = JsonConvert.SerializeObject(subirArchivo);

                string flexFields = flexFieldString; /*txtFlexFields.Text;*/
                //string connectionString = txtConnectionString.Text;

                if (string.IsNullOrEmpty(connectionString))
                {
                    lblMessage.Text = "Connection String cannot be empty.";
                    return "Connection String cannot be empty.";
                }

                //HttpPostedFile file = fileUpload.PostedFile;

                try
                {
                    var result = await SubirArchivo.SubirArchivo_t(idFile, file, flexFields, connectionString);

                    if (result.typeResult == UtilClass.UtilClass.codigoExitoso)
                    {
                        lblMessage.Text = "File uploaded successfully!";
                    }
                    else
                    {
                        lblMessage.Text = $"File upload failed: {result.message}";
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                lblMessage.Text = "Please select a file.";
            }
            return lblMessage.Text;
        }

        protected async void btnEnviar_Click(object sender, EventArgs e)
        {          
            try
            {
                string result = "";

                //crear solicitud en base de datos con los datos del formulario y obtener el id de la solicitud
                //int idSolicitud = CrearSolicitud();

                result  = CrearSolicitud();
                string idString = Session["Id"] != null ? Session["Id"].ToString() : string.Empty;

                result = await UploadIdentidadAsync(idString);
                result = await UploadSolicitudAsync(idString);
                result = await UploadReciboAsync(idString);
                // Utiliza el resultado como sea necesario
                lblMessage.Text = "Resultado: " + result;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                lblMessage.Text = "Error: " + ex.Message;
            }

        }
             
        private string CrearSolicitud()
        {
            // Crear solicitud en base de datos
            // Devolver el ID de la solicitud creada
            try
            {
                SqlCommand cmd = SetupCommand();
                PersonParameters(cmd);
                ExecuteDatabaseCommand(cmd);
                //PrepareResponse(cmd);
                var result = PrepareResponse(cmd);
                //if (CheckForSuccess(cmd))
                //{
                //    ManageReportCreationAndEmailing(cmd);
                //}
                return result;
            }

            catch (Exception ex)
            {
                lblMessage.Text = "Error: " + ex.Message;
                return ex.Message;

            }          

        }

        private void ExecuteDatabaseCommand(SqlCommand cmd)
        {
            try
            {
                conex.abrirConexion();
                cmd.ExecuteNonQuery();
                string Mens = cmd.Parameters["@MENSAGE"].Value.ToString();
                int Retorno = Convert.ToInt32(cmd.Parameters["@RETORNO"].Value);
                Session["Id"] = Convert.ToInt32(cmd.Parameters["@Id"].Value);
            }
            finally
            {
                conex.cerrarConexion();
            }
        }

        private SqlCommand SetupCommand()
        {
            var cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[gral].[sp_Registro_Solicitud]",
                Connection = conex.conexion
            };
            return cmd;
        }

        private void PersonParameters(SqlCommand cmd)
        {
            string DNI = tbIdentidad.Text;
            string FirstName = tbNombre.Text;
            string LastName = tbApellido.Text;
            string email = tbCorreo.Text;
            string Phone = tbTelefono.Text;
            string Address = tbDireccion.Text;


            //string Company = CmbCountry.Text;
            //string TypeDeclaration = CmbTipoDeclaracion.Text;
            //bool Policy = ckPolitica.Checked;

            cmd.Parameters.Add("@pcEmail", SqlDbType.NVarChar, 500).Value = email;
            cmd.Parameters.Add("@pnDNI", SqlDbType.NVarChar, 500).Value = DNI;
            cmd.Parameters.Add("@pcFirstName", SqlDbType.NVarChar, 500).Value = FirstName;
            cmd.Parameters.Add("@pcLastName", SqlDbType.NVarChar, 500).Value = LastName;
            cmd.Parameters.Add("@pnAddress", SqlDbType.NVarChar, 500).Value = Address;
            //cmd.Parameters.Add("@pcCompany", SqlDbType.NVarChar, 500).Value = Company;
            cmd.Parameters.Add("@pnPhone", SqlDbType.NVarChar).Value = Phone;
            //cmd.Parameters.Add("@pcTypeDeclaration", SqlDbType.NVarChar, 500).Value = TypeDeclaration;
            //cmd.Parameters.Add("@pbPolitica", SqlDbType.Bit).Value = Policy;
            cmd.Parameters.Add("@MENSAGE", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@RETORNO", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

        }

        private string PrepareResponse( SqlCommand cmd)
        {
            var responseObj = new
            {
                Retorno = Convert.ToInt32(cmd.Parameters["@RETORNO"].Value),
                Mensaje = cmd.Parameters["@MENSAGE"].Value.ToString(),
            };
            return JsonConvert.SerializeObject(responseObj);
        }
        /* referente al token */

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
            var (storedToken, tokenTimestamp) =Util_g.GetToken();

            if (storedToken != null && tokenTimestamp != null)
            {
                double elapsedMinutes = (DateTime.Now - tokenTimestamp.Value).TotalMinutes;
                System.Diagnostics.Debug.WriteLine($"Token age in minutes: {elapsedMinutes}");

                if (elapsedMinutes <= 15)
                {
                    Util_g.SetToken(storedToken, tokenTimestamp.Value);

                    if (inputToken == storedToken)
                    {
                        e.Result = "success";
                        System.Diagnostics.Debug.WriteLine("Token verification successful.");
                    }
                    else
                    {
                        e.Result = "incorrect";
                        System.Diagnostics.Debug.WriteLine("Token is incorrect.");
                    }
                }
                else
                {
                    e.Result = "expired";
                    System.Diagnostics.Debug.WriteLine("Token has expired.");
                    HttpContext.Current.Session.Remove("VerificationToken");
                    HttpContext.Current.Session.Remove("TokenTimestamp");
                }
            }
            else
            {
                e.Result = "failure";
                System.Diagnostics.Debug.WriteLine("Token or timestamp missing.");
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

        protected void callbackpane_comprobante_callback(object sender, CallbackEventArgsBase e)
        {
            // generar el identificador encriptado una sola vez y pasarlo a otros métodos
            //string encryptedid = SG_Constancia_TSC.App_Start.Util_g.Encrypt(Session["id"].ToString());

            //Reportes.Comprobante report = GenerarReporte(encryptedid);

            //// agregar el código qr al reporte
            //AgregarCodigoQRAlReporte(report, encryptedid);

            //// mostrar el reporte en el visor
            //ASPxWebDocumentViewer1.OpenReport(report);
        }
    }
}