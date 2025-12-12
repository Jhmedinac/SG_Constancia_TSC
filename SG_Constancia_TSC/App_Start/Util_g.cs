using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace SG_Constancia_TSC.App_Start
{
    public class Util_g
    {
        public static void Bitacora(String Activ, string name_user, string Den, string Etapa, string Estado, string Observaciones)
        {

            int retVal = 0;
            
            string constr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

            string obs1 = Observaciones; 
            using (SqlConnection Sqlcon = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    Sqlcon.Open();
                    cmd.Connection = Sqlcon;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "SP_Bitacora";

                    cmd.Parameters.Add(new SqlParameter("@pvchAction", SqlDbType.VarChar, 100));
                    cmd.Parameters.Add(new SqlParameter("@empname", SqlDbType.VarChar, 100));
                    cmd.Parameters.Add(new SqlParameter("@obs", SqlDbType.VarChar, 150));
                    cmd.Parameters.Add(new SqlParameter("@Actividad", SqlDbType.VarChar, 100));
                    cmd.Parameters.Add(new SqlParameter("@Etapa", SqlDbType.VarChar, 100));
                    cmd.Parameters.Add(new SqlParameter("@estado", SqlDbType.VarChar, 100));
                    cmd.Parameters.Add(new SqlParameter("@No_Denuncia", SqlDbType.VarChar, 100));
                    cmd.Parameters.Add("@pIntErrDescOut", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@pId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters["@pvchAction"].Value = "insert";
                    cmd.Parameters["@empname"].Value = name_user;
                    cmd.Parameters["@obs"].Value = obs1;             /*obs*/
                    cmd.Parameters["@Actividad"].Value = Activ;
                    cmd.Parameters["@Etapa"].Value = Etapa;/* etapa*/
                    cmd.Parameters["@estado"].Value = Estado;
                    cmd.Parameters["@No_Denuncia"].Value = Den;/*Denuncia*/
                    cmd.ExecuteNonQuery();
                    retVal = Convert.ToInt32(cmd.Parameters["@pIntErrDescOut"].Value);


                    if ((retVal == 2))
                    {
                    }
                    
                }
            }

        }

        

        public static string Encrypt(string input)
        {
            var plainTextBytes = Encoding.Unicode.GetBytes(input);
            var base64EncodedBytes = Convert.ToBase64String(plainTextBytes);
            return base64EncodedBytes.Replace("+", "-").Replace("/", "_").Replace("=", ",");
        }

        public static string Decrypt(string input)
        {
            var base64EncodedBytes = input.Replace("-", "+").Replace("_", "/").Replace(",", "=");
            var base64DecodedBytes = Convert.FromBase64String(base64EncodedBytes);
            return Encoding.Unicode.GetString(base64DecodedBytes);
        }

        public static void SetSessionValue(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static object GetSessionValue(string key)
        {
            return HttpContext.Current.Session[key];
        }

        public static void SetToken(string token, DateTime timestamp)
        {
            SetSessionValue("VerificationToken", token);
            SetSessionValue("TokenTimestamp", timestamp);
        }

        public static (string token, DateTime? timestamp) GetToken()
        {
            var token = GetSessionValue("VerificationToken")?.ToString();
            var timestamp = GetSessionValue("TokenTimestamp") as DateTime?;
            return (token, timestamp);
        }


    }
}