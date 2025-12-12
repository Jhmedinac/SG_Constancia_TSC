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

        public static string Desencripta(string Cadena)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(Cadena);
            result = Encoding.Unicode.GetString(decryted);
            return result;
        }


        public static string ObtenerContentType(string extension)
        {
                    var mimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "pdf", "application/pdf" },
                { "png", "image/png" },
                { "jpg", "image/jpeg" },
                { "jpeg", "image/jpeg" },
                { "gif", "image/gif" },
                { "txt", "text/plain" },
                { "doc", "application/msword" },
                { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { "xls", "application/vnd.ms-excel" },
                { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { "zip", "application/zip" },
                { "rar", "application/x-rar-compressed" },
              
            };

                return mimeTypes.TryGetValue(extension, out string mimeType) ? mimeType : "application/octet-stream";
        }

        public static  bool EsExtensionValida(string extension)
        {
            var extensionesPermitidas = new[] { "pdf", "png", "jpg", "jpeg", "gif", "txt", "doc", "docx", "xls", "xlsx", "zip", "rar" };
            return extensionesPermitidas.Contains(extension.ToLower());
        }

    }

}

