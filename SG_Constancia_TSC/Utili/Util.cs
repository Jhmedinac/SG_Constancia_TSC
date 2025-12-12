using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


using System.Web;
using System.Configuration;
using System.IO;


namespace SG_Constancia_TSC.Util
{
    public class Util
    {
        #region servicios TSC FILES UTL
        public static string obtenerDataAchivos = "Data/";
        public static string obtenerAchivos = "Download_File/";
        public static string subirAchivos = "Upload";
        #endregion
        #region métodos reutilizables para el llamado de servicios
        public static HttpClient getGoFilesUtlHeaders()
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Username", ConfigurationManager.AppSettings["GoFilesUtlUser"].ToString());
            _httpClient.DefaultRequestHeaders.Add("Password", ConfigurationManager.AppSettings["GoFilesUtlPassword"].ToString());
            _httpClient.DefaultRequestHeaders.Add("File_Path", ConfigurationManager.AppSettings["TSCFilesUtlPath"].ToString());

          
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
            return ConfigurationManager.AppSettings["UrlGoFilesUtlTSC"].ToString() + url;
        }
        #endregion
    }
}

