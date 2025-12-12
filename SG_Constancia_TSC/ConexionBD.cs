using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace SG_Constancia_TSC
{
    //public class ConexionBD
    public class ConexionBD : IDisposable
    {
        public string error;
      
        string cadena = (ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
        public SqlConnection conexion = new SqlConnection();

        public ConexionBD()
        {
            conexion.ConnectionString = cadena;
        }
        public void AbrirConexion()
        {
            try
            {
                if (conexion.State == ConnectionState.Closed)
                {
                    conexion.Open();
                }
            }
            catch (Exception e)
            {
                error = e.Message;
            }
        }

        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }

        public SqlConnection ObtenerConexion()
        {
            return conexion;
        }

        public void Dispose()
        {
            CerrarConexion();
            if (conexion != null)
            {
                conexion.Dispose();
            }
        }
    }

}
