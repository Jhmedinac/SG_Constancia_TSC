using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SG_Constancia_TSC;
using DevExpress.Xpo.Logger;
using SG_Constancia_TSC.Controllers;
using System.Configuration;
using DevExpress.DataAccess.Native.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SG_Constancia_TSC
{
    public partial class Upload : System.Web.UI.Page
    {
        protected async void btnUpload_Click(object sender, EventArgs e)
        {
            lblMessage.Text = ""; // Inicializar lblMessage para asegurarse de que no es null

            if (fileUpload.HasFile && fileUpload.PostedFile.ContentLength > 0)
            {
                if (!int.TryParse(txtIdFile.Text, out int idFile))
                {
                    lblMessage.Text = "Invalid File ID.";
                    return;
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

                string flexFields = txtFlexFields.Text;
                //string connectionString = txtConnectionString.Text;

                if (string.IsNullOrEmpty(connectionString))
                {
                    lblMessage.Text = "Connection String cannot be empty.";
                    return;
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
        }
    }
}