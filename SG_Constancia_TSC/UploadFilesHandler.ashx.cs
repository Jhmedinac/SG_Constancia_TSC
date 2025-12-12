using DevExpress.DataAccess.Native.Web;
using Newtonsoft.Json;
using SG_Constancia_TSC.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SG_Constancia_TSC

{
    /// <summary>
    /// Descripción breve de UploadFilesHandler
    /// </summary>
    public class UploadFilesHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // Obtener los parámetros del request
                var files = context.Request.Files;
                var fileIdKey = context.Request.Form["fileIdKey"]; //id del archivo a subir
                var flexFieldKey = context.Request.Form["flexFieldKey"]; //llave del campo flexible
                var flexFieldValue = context.Request.Form["flexFieldValue"]; // id        

                // Extraer todos los valores para las claves 'flexFieldKey' y 'fileIdKey'
                var flexFieldKeysArray = context.Request.Form.GetValues("flexFieldKey");
                var fileIdKeysArray = context.Request.Form.GetValues("fileIdKey");

                // Verificar si hay archivos y si las claves corresponden en número a los archivos
                if (files.Count > 0 && flexFieldKeysArray != null && fileIdKeysArray != null &&
                    flexFieldKeysArray.Length == files.Count && fileIdKeysArray.Length == files.Count)
                {
                    List<HttpPostedFile> fileList = new List<HttpPostedFile>();
                    List<string> flexFieldKeys = new List<string>();
                    List<string> fileIdKeys = new List<string>();

                    // Recopilar todos los archivos y sus metadatos
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        if (file != null && file.ContentLength > 0)
                        {
                            fileList.Add(file);
                            flexFieldKeys.Add(flexFieldKeysArray[i]);
                            fileIdKeys.Add(fileIdKeysArray[i]);
                        }
                    }



                    // Llamar al método para subir archivos
                    var result = UploadFiles(fileList, fileIdKeys, flexFieldKeys, flexFieldValue).GetAwaiter().GetResult();


                    // Configurar la respuesta
                    context.Response.ContentType = "application/json";
                    context.Response.Write(result);
                }
            }

            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonConvert.SerializeObject(new { error = ex.Message }));
            }

        }
        private async Task<string> UploadFiles(List<HttpPostedFile> files, List<string> fileIdKeys, List<string> flexFieldKeys, string flexFieldValue)
        {
            // Obtener el contexto HTTP actual
            var context = HttpContext.Current;
            var results = new List<string>();
            CustomJsonResult response = new CustomJsonResult();

            // Obtener el ID de usuario actual, si existe sesión
            int codigoUsuario = 0;
            try
            {
                //codigoUsuario = Convert.ToInt32(context.Session["User_id"] ?? 0);
                if (context.Session != null && context.Session["User_id"] != null)
                {
                    codigoUsuario = Convert.ToInt32(context.Session["User_id"]);

                }
            }
            catch
            {
                codigoUsuario = 0;
            }

            for (int i = 0; i < files.Count; i++)
            {
                try

                {
                    var file = files[i];
                    var fileIdKey = fileIdKeys[i];
                    var flexFieldKey = flexFieldKeys[i];

                    if (!int.TryParse(fileIdKey, out int idFile))
                    {
                        results.Add($"Invalid File ID for file {file.FileName}.");
                        continue; // Continua con el siguiente archivo
                    }

                    // Crear el objeto de parámetros para el archivo
                    SubirArchivo_D subirArchivo = new SubirArchivo_D
                    {
                        FlexfieldKey = flexFieldKey,
                        FlexfieldValue = flexFieldValue
                    };

                    // Obtener la cadena de conexión desde el archivo de configuración
                    string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
                    var flexFieldString = JsonConvert.SerializeObject(subirArchivo);

                    // Verificar si la cadena de conexión es válida
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        results.Add("Connection String cannot be empty.");
                        break; // Termina la iteración si hay un problema con la cadena de conexión
                    }

                    // Llamar al método para subir el archivo
                    CustomJsonResult result = await SubirArchivo_m(idFile, file, flexFieldString, connectionString).ConfigureAwait(false);

                    // Verificar el resultado de la operación de carga
                    if (result.typeResult == UtilClass.UtilClass.codigoExitoso)
                    {

                        result.typeResult = UtilClass.UtilClass.codigoExitoso;



                        response = result;
                    }
                    else
                    {
                        results.Add($"File upload failed for {file.FileName}: {result.message}");
                        string userIP = context?.Request?.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (string.IsNullOrEmpty(userIP))
                            userIP = context?.Request?.ServerVariables["REMOTE_ADDR"];

                        // Registrar el error
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO [gral].[Bitacora_Errores] 
                        (codigo_usuario, fecha_hora, componente, descripcion)
                        VALUES (@codigo_usuario, @fecha_hora, @componente, @descripcion)", conn))
                            {
                                cmd.Parameters.AddWithValue("@codigo_usuario", codigoUsuario);
                                cmd.Parameters.AddWithValue("@fecha_hora", DateTime.Now);
                                cmd.Parameters.AddWithValue("@componente", "UploadFiles");
                                cmd.Parameters.AddWithValue("@descripcion", $"IP: {userIP ?? "Desconocida"} | File upload failed for {file.FileName}: {result.message}");
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    string fileName = files[i]?.FileName ?? "(unknown file)";
                    string errorMsg = $"Error uploading file {fileName}: {ex.Message}";
                    results.Add(errorMsg);

                    // 🔹 Registrar el error en la tabla de Bitácora
                    try
                    {
                        // Obtener IP del usuario
                        string userIP = context?.Request?.ServerVariables["HTTP_X_FORWARDED_FOR"];
                        if (string.IsNullOrEmpty(userIP))
                            userIP = context?.Request?.ServerVariables["REMOTE_ADDR"];

                        // Registrar el error
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO [gral].[Bitacora_Errores] 
                        (codigo_usuario, fecha_hora, componente, descripcion)
                        VALUES (@codigo_usuario, @fecha_hora, @componente, @descripcion)", conn))
                            {
                                cmd.Parameters.AddWithValue("@codigo_usuario", codigoUsuario);
                                cmd.Parameters.AddWithValue("@fecha_hora", DateTime.Now);
                                cmd.Parameters.AddWithValue("@componente", "UploadFiles");
                                cmd.Parameters.AddWithValue("@descripcion", $"IP: {userIP ?? "Desconocida"} | {ex}");
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception logEx)
                    {
                        // Evitar que un error de bitácora interrumpa el proceso
                        results.Add($"Failed to log error: {logEx.Message}");
                    }
                }
            }

            // Si hubo errores, incluirlos en la respuesta
            if (results.Count > 0)
            {
                response.typeResult = UtilClass.UtilClass.codigoError;
                response.message = string.Join("; ", results);
            }
            // Devolver los resultados de la subida de archivos

            return JsonConvert.SerializeObject(response);


        }
        private async Task<string> UploadFile(HttpPostedFile file, string fileIdKey, string flexFieldKey, string flexFieldValue)
        {
            // Verificar si el archivo es válido
            if (file == null || file.ContentLength <= 0)
            {
                return "Please select a file.";
            }

            // Convertir el fileIdKey a un entero
            if (!int.TryParse(fileIdKey, out int idFile))
            {
                return "Invalid File ID.";
            }

            // Crear el objeto de parámetros para el archivo
            SubirArchivo_D subirArchivo = new SubirArchivo_D
            {
                FlexfieldKey = flexFieldKey,
                FlexfieldValue = flexFieldValue
            };

            // Obtener la cadena de conexión desde el archivo de configuración
            string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
            var flexFieldString = JsonConvert.SerializeObject(subirArchivo);

            // Verificar si la cadena de conexión es válida
            if (string.IsNullOrEmpty(connectionString))
            {
                return "Connection String cannot be empty.";
            }

            try
            {
                // Llamar al método para subir el archivo
                var result = await SubirArchivo.SubirArchivo_t(idFile, file, flexFieldString, connectionString);
                //var result = await SubirArchivo.SubirArchivos_m(idFile, file, flexFieldString, connectionString);

                // Verificar el resultado de la operación de carga
                if (result.typeResult == UtilClass.UtilClass.codigoExitoso)
                {
                    return "File uploaded successfully!";
                }
                else
                {
                    return $"File upload failed: {result.message}";
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public static async Task<CustomJsonResult> SubirArchivo_m(int idFile, HttpPostedFile File, string flexFields = null, string connectionString = null)
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

                httpClient.Timeout = TimeSpan.FromSeconds(30); // 30 segundos de timeout
                HttpResponseMessage httpResponse = await httpClient.PostAsync(urlWithQuery, formContent).ConfigureAwait(false);


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



    }

    public class SubirArchivo_D
    {
        public string FlexfieldKey { get; set; }
        public string FlexfieldValue { get; set; }
    }
}