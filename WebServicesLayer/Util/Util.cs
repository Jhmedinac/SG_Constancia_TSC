using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Configuration;
using System.Net.Http;
namespace WebServicesLayer.Util
{
    public class Util
    {
        #region servicios TSC FILES UTL
        public static string obtenerDataAchivos = "Data/";
        public static string obtenerAchivos = "Download/";
        public static string subirAchivos = "Upload";
        #endregion
        #region métodos reutilizables para el llamado de servicios
        public static HttpClient getGoFilesUtlHeaders()
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Username", ConfigurationManager.AppSettings["GoFilesUtlUser"].ToString());
            _httpClient.DefaultRequestHeaders.Add("Password", ConfigurationManager.AppSettings["GoFilesUtlPassword"].ToString());
            _httpClient.DefaultRequestHeaders.Add("File_Path", ConfigurationManager.AppSettings["GoFilesUtlPath"].ToString());
            //_httpClient.DefaultRequestHeaders.Add("Path", ConfigurationManager.AppSettings["GoFilesUtlConnString"].ToString());
            return _httpClient;
        }
        public static HttpClient getServiceClient(string token)
        {
            HttpClient _httpClient = new HttpClient();


            _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            return _httpClient;
        }

        public static string GetFinalUrl(string url)
        {
            return ConfigurationManager.AppSettings["urlApiHis"].ToString() + url;
        }

        public static string GetFinalGoFilesUtlUrl(string url)
        {
            return ConfigurationManager.AppSettings["UrlGoFilesUtlSASI"].ToString() + url;
        }
        #endregion
    }
}
