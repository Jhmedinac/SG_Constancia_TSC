using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
//using WebServicesLayerSG.Util;
//using WebServicesLayerSG;

namespace SG_Constancia_TSC.Utili
{
    public static class Utila
    {
        public static HttpClient getGoFilesUtlHeaders()
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "tuToken");
        return client;
    }

    public static async Task<CustomJsonResult> SubirArchivo(int idFile, HttpPostedFileBase file, string connectionString, string flexFields = null)
    {
        CustomJsonResult response = new CustomJsonResult();
        try
        {
            HttpClient httpClient = getGoFilesUtlHeaders();
            var fileContent = new StreamContent(file.InputStream);
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
            string baseUrl = GetFinalGoFilesUtlUrl("subirAchivos");
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

    public static string GetFinalGoFilesUtlUrl(string endpoint)
    {
        // Implementar lógica para obtener la URL final
        return $"https://tu-dominio.com/api/{endpoint}";
    }
}
}