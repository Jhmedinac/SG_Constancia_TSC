using DevExpress.DataAccess.Native.Web;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SG_Constancia_TSC.UtilClass;
using DevExpress.CodeParser;
using SG_Constancia_TSC;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;


namespace SG_Constancia_TSC
{
    public class FileDownload : IHttpAsyncHandler
                    
    {
        // Implementación de IHttpAsyncHandler
        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            // Crear una tarea que ejecute ProcessRequestAsync
            var task = ProcessRequestAsync(context);

            // Configurar el callback al completarse la tarea
            var asyncResult = new TaskWrapperAsyncResult(task, extraData);
            if (cb != null)
            {
                task.ContinueWith(t => cb(asyncResult));
            }
            return asyncResult;
        }

        public async Task ProcessRequestAsync(HttpContext context)
        {
            string uploadId = context.Request.QueryString["Upload_Id"];
            string mode = context.Request.QueryString["mode"]; // Nuevo parámetro para definir vista o descarga

            if (string.IsNullOrEmpty(uploadId))
            {
                context.Response.StatusCode = 400;
                context.Response.Write("El parámetro 'Upload_Id' es obligatorio.");
                return;
            }

            try
            {
                int codigoArchivo = Convert.ToInt32(uploadId);
                FileResultWithExtension response = await ObtenerArchivo(codigoArchivo);

                if (response.FileResult.typeResult != UtilClass.UtilClass.codigoExitoso)
                {
                    context.Response.StatusCode = 500;
                    context.Response.Write("Error al obtener el archivo desde la API.");
                    return;
                }

                // Convertir base64 a bytes
                string base64 = response.FileResult.result?.ToString();
                byte[] fileBytes = Convert.FromBase64String(base64);

                if (fileBytes == null || fileBytes.Length == 0)
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("El archivo solicitado no se encontró o está vacío.");
                    return;
                }

                // Validar extensión y MIME
                string extension = response.Extension;
                string mimeType = UtilClass.UtilClass.ObtenerContentType(extension);

                if (!UtilClass.UtilClass.EsExtensionValida(extension))
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write($"La extensión '{extension}' no está permitida.");
                    return;
                }

                // Configurar encabezados de respuesta
                context.Response.Clear();
                context.Response.ContentType = mimeType;

                // Definir si el archivo se muestra o se descarga
                string dispositionType = mode == "view" ? "inline" : "attachment";
                context.Response.AddHeader("Content-Disposition", $"{dispositionType}; filename=archivo_{uploadId}.{extension}");

                context.Response.BinaryWrite(fileBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error interno: {ex.Message}");
                context.Response.StatusCode = 500;
                context.Response.Write("Ocurrió un error interno al procesar la solicitud.");
            }
            finally
            {
                context.Response.End();
            }
        }

        // Método requerido por IHttpHandler (pero no se usará)
        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException("Usar el método asíncrono ProcessRequestAsync");
        }

        public void EndProcessRequest(IAsyncResult result)
        {
            // No se requiere lógica adicional
        }

        // Clase auxiliar para envolver la tarea en IAsyncResult
        private class TaskWrapperAsyncResult : IAsyncResult
        {
            private readonly Task _task;
            private readonly object _state;

            public TaskWrapperAsyncResult(Task task, object state)
            {
                _task = task;
                _state = state;
            }

            public bool IsCompleted => _task.IsCompleted;
            public WaitHandle AsyncWaitHandle => ((IAsyncResult)_task).AsyncWaitHandle;
            public object AsyncState => _state;
            public bool CompletedSynchronously => false;
        }



        //public async Task ProcessRequestAsync(HttpContext context)
        //{
        //    string uploadId = context.Request.QueryString["Upload_Id"];
        //    if (string.IsNullOrEmpty(uploadId))
        //    {
        //        context.Response.StatusCode = 400;
        //        context.Response.Write("El parámetro 'Upload_Id' es obligatorio.");
        //        return;
        //    }

        //    try
        //    {
        //        // Convertir uploadId a código de archivo (asegúrate de declarar 'idf')
        //        int codigoArchivo = Convert.ToInt32(uploadId);

        //        // Obtener archivo desde la API
        //        FileResultWithExtension response = await ObtenerArchivo(codigoArchivo);

        //        // Verificar si la respuesta fue exitosa
        //        if (response.FileResult.typeResult != UtilClass.UtilClass.codigoExitoso)
        //        {
        //            context.Response.StatusCode = 500;
        //            context.Response.Write("Error al obtener el archivo desde la API.");
        //            return;
        //        }

        //        // Convertir base64 a bytes
        //        string base64 = response.FileResult.result?.ToString();
        //        byte[] fileBytes = Convert.FromBase64String(base64);

        //        // Validar archivo descargado
        //        if (fileBytes == null || fileBytes.Length == 0)
        //        {
        //            context.Response.StatusCode = 404;
        //            context.Response.Write("El archivo solicitado no se encontró o está vacío.");
        //            return;
        //        }

        //        // Validar extensión y MIME
        //        string extension = response.Extension;
        //        string mimeType = UtilClass.UtilClass.ObtenerContentType(extension);

        //        if (!UtilClass.UtilClass.EsExtensionValida(extension))
        //        {
        //            context.Response.StatusCode = 400;
        //            context.Response.Write($"La extensión '{extension}' no está permitida.");
        //            return;
        //        }

        //        // Configurar respuesta
        //        context.Response.Clear();
        //        context.Response.ContentType = mimeType;
        //        context.Response.AddHeader("Content-Disposition", $"attachment; filename=archivo_{uploadId}.{extension}");
        //        context.Response.BinaryWrite(fileBytes);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de errores
        //        Console.WriteLine($"Error interno: {ex.Message}");
        //        context.Response.StatusCode = 500;
        //        context.Response.Write("Ocurrió un error interno al procesar la solicitud.");
        //    }
        //    finally
        //    {
        //        context.Response.End();
        //    }
        //}



        public async Task<byte[]> GetFileBytesAsync(string idf)
        {
            //CustomJsonResult response = new CustomJsonResult();
            FileResultWithExtension response = new FileResultWithExtension();
            try
            {
                //int codigoArchivo = Convert.ToInt32(UtilClass.UtilClass.Desencripta(idf));
                int codigoArchivo = Convert.ToInt32(idf);
                response = await ObtenerArchivo(codigoArchivo);

                if (response.FileResult.typeResult != UtilClass.UtilClass.codigoExitoso)
                {
                    return null; // O lanzar una excepción
                }

                // Convertir base64 a bytes
                string base64 = response.FileResult.result?.ToString();
                return Convert.FromBase64String(base64);
            }
            catch (Exception e)
            {
                return null;
            }
        }

     
        


        public static async Task<FileResultWithExtension> ObtenerArchivo(int idFile)
        {
            CustomJsonResult response = new CustomJsonResult();
            FileResultWithExtension result = new FileResultWithExtension();

            try
            {
                // Obtener la cadena de conexión desde el archivo de configuración
                string connectionString = ConfigurationManager.ConnectionStrings["GoFilesUtlConnString"].ConnectionString;
                                
                    HttpClient httpClient = Util.Util.getGoFilesUtlHeaders();
                //HttpResponseMessage httpResponse = await httpClient.GetAsync(Util.Util.GetFinalGoFilesUtlUrl(Util.Util.obtenerAchivos) + idFile, connectionString);
                // Ejemplo: si necesitas agregar un token de conexión

                // Construir la URL con el parámetro connectionString
                string baseUrl = Util.Util.GetFinalGoFilesUtlUrl((Util.Util.obtenerAchivos) + idFile);
                string urlCompleta = $"{baseUrl}?constring={Uri.EscapeDataString(connectionString)}";

                HttpResponseMessage httpResponse = await httpClient.GetAsync(urlCompleta);



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
                        result.Extension = _extension; // Almacenar la extensión
                }
                    else
                    {
                        string responseContent = await httpResponse.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<CustomJsonResult>(responseContent);
                    }
              
            }
            catch (Exception e)
            {
                response.typeResult = SG_Constancia_TSC.UtilClass.UtilClass.codigoError;

                response.message = $"Error: {e.Message}, Fuente: {e.Source}";
            }
            result.FileResult = response;
            //return response;
            return result;
        }

        public bool IsReusable => false;
    }


}

public class FileResultWithExtension
{
    public CustomJsonResult FileResult { get; set; }
    public string Extension { get; set; }
}