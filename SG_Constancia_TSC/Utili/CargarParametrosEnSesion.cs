using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;

namespace SG_Constancia_TSC.Utili
{
    public class Parametro
    {
        public string Valor { get; set; }
        public string Descripcion { get; set; }
    }

    public static class ParametrosHelper
    {
        /// <summary>
        /// Carga los parámetros desde la base de datos y los almacena en la sesión.
        /// </summary>
        public static void CargarParametrosEnSesion()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connString"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión no está configurada.");
            }

            var parametros = new Dictionary<string, Parametro>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT nombre_parametro, valor, descripcion FROM [gral].[Parametros]";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nombreParametro = reader["nombre_parametro"].ToString();
                                string valor = reader["valor"].ToString();
                                string descripcion = reader["descripcion"].ToString();

                                parametros[nombreParametro] = new Parametro
                                {
                                    Valor = valor,
                                    Descripcion = descripcion
                                };
                            }
                        }
                    }
                }

                // Almacenar el diccionario en la sesión
                HttpContext.Current.Session["Parametros"] = parametros;
            }
            catch (SqlException sqlEx)
            {
                // Manejo de excepciones de SQL
                throw new Exception("Error al cargar parámetros en la base de datos: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones generales
                throw new Exception("Error al cargar parámetros: " + ex.Message, ex);
            }
        }
    }
}