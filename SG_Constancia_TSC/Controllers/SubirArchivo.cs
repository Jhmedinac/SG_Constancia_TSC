using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SG_Constancia_TSC.UtilClass;


namespace SG_Constancia_TSC.Controllers
{
    public class SubirArchivo_D
    {
        [Display(Name = "FlexfieldKey")]
        public string FlexfieldKey { get; set; }

        [Display(Name = "FlexfieldValue")]
        public string FlexfieldValue { get; set; }
    }

    public class SubirArchivo : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]


        public static async Task<CustomJsonResult> SubirArchivo_t(int idFile, HttpPostedFile File, string flexFields = null, string connectionString= null)
        {
            CustomJsonResult response = new CustomJsonResult();
            try
            {
                HttpClient httpClient = Util.Util.getGoFilesUtlHeaders();
                var fileContent = new StreamContent(File.InputStream);
                var formContent = new MultipartFormDataContent();

                HttpContent idFileParam = new StringContent(idFile.ToString());
                HttpContent flexFieldsParam = new StringContent(flexFields);

                formContent.Add(idFileParam, "IdFile");
                formContent.Add(flexFieldsParam, "Flexfields");
                var hackedFileName = new string(Encoding.UTF8.GetBytes(File.FileName).Select(b => (char)b).ToArray());

                fileContent.Headers.Add("Content-Type", "application/octet-stream");
                fileContent.Headers.Add("Content-Disposition", "form-data; name=\"File\"; filename=\"" + hackedFileName + "\"");
                formContent.Add(fileContent);

                // Construir la URL con el parámetro connectionString
                string baseUrl = Util.Util.GetFinalGoFilesUtlUrl(Util.Util.subirAchivos);
                string urlWithQuery = $"{baseUrl}?constring={Uri.EscapeDataString(connectionString)}";

                
                HttpResponseMessage httpResponse = await httpClient.PostAsync(urlWithQuery, formContent);
                //HttpResponseMessage httpResponse = await httpClient.PostAsync(Util.Util.GetFinalGoFilesUtlUrl(Util.Util.subirAchivos), formContent);
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                response.typeResult = UtilClass.UtilClass.codigoExitoso;
                response = JsonConvert.DeserializeObject<CustomJsonResult>(responseContent);

            }
            catch (Exception e)
            {
                response.typeResult = UtilClass.UtilClass.codigoError;
                response.message = $"Error: {e.Message}, Fuente: {e.Source}";
            }
            return response;
        }


        public static async Task<CustomJsonResult> SubirArchivos_m(int idFile, List<HttpPostedFile> archivos, string flexFields = null, string connectionString = null)
        {
            CustomJsonResult response = new CustomJsonResult();
            try
            {
                HttpClient httpClient = Util.Util.getGoFilesUtlHeaders();
                var formContent = new MultipartFormDataContent();

                // Añadir parámetros de texto
                formContent.Add(new StringContent(idFile.ToString()), "IdFile");
                formContent.Add(new StringContent(flexFields), "Flexfields");

                // Añadir cada archivo
                foreach (var archivo in archivos)
                {
                    var fileContent = new StreamContent(archivo.InputStream);
                    var hackedFileName = new string(Encoding.UTF8.GetBytes(archivo.FileName).Select(b => (char)b).ToArray());

                    fileContent.Headers.Add("Content-Type", "application/octet-stream");
                    fileContent.Headers.Add("Content-Disposition", $"form-data; name=\"File\"; filename=\"{hackedFileName}\"");
                    formContent.Add(fileContent, "File", hackedFileName);
                }

                // Construir la URL con el parámetro connectionString
                string baseUrl = Util.Util.GetFinalGoFilesUtlUrl(Util.Util.subirAchivos);
                string urlWithQuery = $"{baseUrl}?constring={Uri.EscapeDataString(connectionString)}";

                // Enviar la solicitud POST
                HttpResponseMessage httpResponse = await httpClient.PostAsync(urlWithQuery, formContent);

                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<CustomJsonResult>(responseContent);

            }
            catch (Exception e)
            {
                response.typeResult = UtilClass.UtilClass.codigoError;
                response.message = $"Error: {e.Message}, Fuente: {e.Source}";
            }
            return response;
        }
    }
}