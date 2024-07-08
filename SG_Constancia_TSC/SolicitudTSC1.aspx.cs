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


using SG_Constancia_TSC;

using DevExpress.DataAccess.Native.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SG_Constancia_TSC
{
    public partial class SolicitudTSC1 : System.Web.UI.Page
    {



        protected async Task<string> UploadSolicitudAsync()
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
                subirArchivo1.FlexfieldValue = "1"; /* llave registro creado*/
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

        protected async Task<string> UploadReciboAsync()
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
                subirArchivo2.FlexfieldValue = "1";
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


        protected async Task<string> UploadIdentidadAsync()
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
                subirArchivo.FlexfieldValue = "1";
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
                result = await UploadIdentidadAsync();
                result = await UploadSolicitudAsync();
                result = await UploadReciboAsync();
                // Utiliza el resultado como sea necesario
                lblMessage.Text = "Resultado: " + result;
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                lblMessage.Text = "Error: " + ex.Message;
            }

        }

 
    }
}