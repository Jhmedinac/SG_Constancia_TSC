using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;



namespace SG_Constancia_TSC.Services
{

public class CargaDocumento
    {
        ///<summary>
        /// Método para obtener la información del archivo subido
        ///</summary>
        ///<remarks>
        ///Autor: by JMedina 20230918
        ///</remarks>
        ///<returns>
        /// CustomJsonResult
        /// </returns>
        /// 
        public static async Task<CustomJsonResult> ObtenerDataArchivo(int idFile)
        {
            CustomJsonResult response = new CustomJsonResult();
            try
            {
                HttpClient httpClient = Util.Util.getGoFilesUtlHeaders();
                HttpResponseMessage httpResponse = await httpClient.GetAsync(Util.Util.GetFinalGoFilesUtlUrl(Util.Util.obtenerDataAchivos) + idFile);
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


        ///<summary>
        /// Método para obtener/descargar el archivo subido
        ///</summary>
        ///<remarks>
        ///Autor: by JMedina 20230918
        ///</remarks>
        ///<returns>
        /// CustomJsonResult
        /// </returns>



        ///<summary>
        /// Método para subir un archivo
        ///</summary>
        ///<remarks>
        ///Autor: by JMedina 20230918
        ///</remarks>
        ///<returns>
        /// CustomJsonResult
        /// </returns>
        public static async Task<CustomJsonResult> SubirArchivo(int idFile, IFormFile file, string connectionString, string flexFields = null)
        {
            CustomJsonResult response = new CustomJsonResult();
            try
            {
                HttpClient httpClient = Util.Util.getGoFilesUtlHeaders();
                var fileContent = new StreamContent(file.OpenReadStream());
                var formContent = new MultipartFormDataContent();

                HttpContent idFileParam = new StringContent(idFile.ToString());
                HttpContent flexFieldsParam = new StringContent(flexFields ?? string.Empty);

                formContent.Add(idFileParam, "IdFile");
                formContent.Add(flexFieldsParam, "Flexfields");

                var hackedFileName = new string(Encoding.UTF8.GetBytes(file.FileName).Select(b => (char)b).ToArray());

                fileContent.Headers.Add("Content-Type", "application/octet-stream");
                fileContent.Headers.Add("Content-Disposition", $"form-data; name=\"File\"; filename=\"{hackedFileName}\"");
                formContent.Add(fileContent);

                // Construir la URL con el parámetro connectionString
                string baseUrl = Util.Util.GetFinalGoFilesUtlUrl(Util.Util.subirAchivos);
                string urlWithQuery = $"{baseUrl}?connectionString={Uri.EscapeDataString(connectionString)}";

                HttpResponseMessage httpResponse = await httpClient.PostAsync(urlWithQuery, formContent);
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
        //public static async Task<CustomJsonResult> SubirArchivo(int idFile, HttpPostedFileBase File ,string flexFields = null)
        //{
        //    CustomJsonResult response = new CustomJsonResult();
        //    try
        //    {
        //        HttpClient httpClient = Util.Util.getGoFilesUtlHeaders();
        //        var fileContent = new StreamContent(File.InputStream);
        //        var formContent = new MultipartFormDataContent();

        //        HttpContent idFileParam = new StringContent(idFile.ToString());
        //        HttpContent flexFieldsParam = new StringContent(flexFields);

        //        formContent.Add(idFileParam, "IdFile");
        //        formContent.Add(flexFieldsParam, "Flexfields");
        //        var hackedFileName = new string(Encoding.UTF8.GetBytes(File.FileName).Select(b => (char)b).ToArray());

        //        fileContent.Headers.Add("Content-Type", "application/octet-stream");
        //        fileContent.Headers.Add("Content-Disposition", "form-data; name=\"File\"; filename=\"" + hackedFileName + "\"");
        //        formContent.Add(fileContent);

        //        HttpResponseMessage httpResponse = await httpClient.PostAsync(Util.Util.GetFinalGoFilesUtlUrl(Util.Util.subirAchivos), formContent);
        //        string responseContent = await httpResponse.Content.ReadAsStringAsync();
        //        response.typeResult = Util.UtilClass.codigoExitoso;
        //        response = JsonConvert.DeserializeObject<CustomJsonResult>(responseContent);

        //    }
        //    catch (Exception e)
        //    {
        //        response.typeResult = Util.UtilClass.codigoError;
        //        response.message = $"Error: {e.Message}, Fuente: {e.Source}";
        //    }
        //    return response;
        //}


        public static async Task<CustomJsonResult> ObtenerArchivo(int idFile)
        {
            CustomJsonResult response = new CustomJsonResult();
            try
            {
                HttpClient httpClient = Util.Util.getGoFilesUtlHeaders();
                HttpResponseMessage httpResponse = await httpClient.GetAsync(Util.Util.GetFinalGoFilesUtlUrl(Util.Util.obtenerAchivos) + idFile);
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseContent = await httpResponse.Content.ReadAsStreamAsync();
                    var resp = httpResponse.Content.Headers.ContentType.MediaType.ToString();
                    var fileName = httpResponse.Content.Headers?.ContentDisposition?.FileNameStar?.ToString();

                    //Se convierte la respuesta del servicio (Stream) a MemoryStream para luego convertirlo a base64 
                    byte[] bytes;
                    var memoryStream = new MemoryStream();
                    responseContent.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray();
                    string base64 = Convert.ToBase64String(bytes);
                    string _nombre = "";
                    string _extension = "";
                    int? indiceDelUltimoPunto = httpResponse.Content.Headers?.ContentDisposition?.FileNameStar?.LastIndexOf('.');
                    if (indiceDelUltimoPunto != null)
                    {
                        _nombre = fileName.Substring(0, (int)indiceDelUltimoPunto);
                        _extension = fileName.Substring((int)indiceDelUltimoPunto + 1, fileName.Length - (int)indiceDelUltimoPunto - 1);
                    }

                    var datosArchivo = new
                    {
                        nombre = _nombre,
                        extension = _extension,
                        mediaType = resp
                    };

                    response.result = base64;
                    response.message = JsonConvert.SerializeObject(datosArchivo);
                }
                else
                {
                    string responseContent = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<CustomJsonResult>(responseContent);
                }
            }
            catch (Exception e)
            {
                response.typeResult = UtilClass.UtilClass.codigoError;
                response.message = $"Error: {e.Message}, Fuente: {e.Source}";
            }
            return response;
        }

        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "tuToken");

            var formContent = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes("/ruta/al/archivo"));
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "File",
                FileName = "nombreDelArchivo"
            };
            formContent.Add(fileContent);
            formContent.Add(new StringContent("123"), "IdFile");
            formContent.Add(new StringContent("campo1, campo2"), "Flexfields");

            // Construir la URL con el parámetro connectionString
            string baseUrl = Util.Util.GetFinalGoFilesUtlUrl(Util.Util.subirAchivos);
            string connectionString = "tuCadenaDeConexion";
            string urlWithQuery = $"{baseUrl}?connectionString={Uri.EscapeDataString(connectionString)}";

            HttpResponseMessage httpResponse = await httpClient.PostAsync(urlWithQuery, formContent);
            var responseString = await httpResponse.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);
        }

    }
}







