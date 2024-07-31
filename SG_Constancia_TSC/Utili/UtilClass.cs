using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SG_Constancia_TSC.UtilClass
{
    public static class UtilClass
    {
        public static String alertNotification = "AlertNotification";

        public static string estado = "0";
        public static int codigoError = 2;
        public static int codigoExitoso = 0;

        public static int tipoOperacionCrear = 1;
        public static int tipoOperacionModificar = 2;
        public static int tipoOperacionEliminar = 3;

        public static int tipoEmpleadoPermanente = 1;
        public static int tipoEmpleadoContrato = 2;
        public static int tipoEmpleadoPrueba = 3;
        public static int tipoEmpleadoInterino = 4;
        public static int tipoEmpleadoPractica = 5;
        public static int tipoEmpleadoExterno = 6;

        public static string tipoSentenciaInsert = "INSERT";
        public static string tipoSentenciaUpdate = "UPDATE";
        public static string tipoSentenciaDelete = "DELETE";

        public static string flexFieldKeyIDENTIDAD = "CODIGO_IDENTIDAD";
        public static string flexFieldKeySOLICITUD = "CODIGO_SOLICITUD";
        public static string flexFieldKeyRECIBO = "CODIGO_RECIBO";
        
        public static string FileId_ident = "2"; /*IDENTIDAD*/
        public static string FileId_solicitud = "3"; /*SOLICITUD*/
        public static string FileId_recibo = "4";   /*RECIBO*/


        public static int Idconstancia { get;  set; }
        public static string Clave { get; set; }
        
        //public static string GetSiteName()
        //{
        //    string SiteName = "";

        //    if (!HttpContext.Current.Request.Url.Host.Contains("localhost"))
        //    {
        //        SiteName = HttpContext.Current.Request.ApplicationPath;
        //    }
        //    return SiteName;
        //}

        //public static string GetFinalUrl(string url, int codigoUsuario)
        //{
        //    string urlFinal = Database.Environment.GetFinalUrl(url, codigoUsuario);
        //    return urlFinal;
        //}

        //public static UsuarioSesion GetUsuarioSesion()
        //{
        //    return (UsuarioSesion)HttpContext.Current.Session["USUARIO_SESION"];
        //}

        public static int maximoVarchar = 32500;
        public static bool isEstadoActivo = true;
        public static int EstadoActivo = 1;
        public static int maximoInt = 32;
        private static readonly Encoding encoding = Encoding.UTF8;


        ///<summary>
        /// Este método sirve encriptar las credenciales
        ///</summary>
        ///<remarks>
        ///Autor: by lgarcia 20230911
        ///</remarks>
        ///<returns>
        /// string
        /// </returns>
        public static string EncriptarPassword(string str)
        {
            string Key = "@PlTyvh4536#567*{KLa123AA!]o)_?K";
            string encryptedText;
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.ECB;

                aes.Key = encoding.GetBytes(Key);
                aes.GenerateIV();

                ICryptoTransform AESEncrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] buffer = encoding.GetBytes(str);

                encryptedText = Convert.ToBase64String(AESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e.Message}, Fuente: {e.Source}");
            }
            return encryptedText;
        }

    }

}

