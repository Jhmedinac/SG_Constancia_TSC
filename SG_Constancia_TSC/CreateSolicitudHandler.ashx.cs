using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Newtonsoft.Json;

namespace SG_Constancia_TSC
{
    public class CreateSolicitudHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // Obtener los datos del request
                string dni = context.Request.Form["tbIdentidad"];
                string firstName = context.Request.Form["tbNombre"];
                string lastName = context.Request.Form["tbApellido"];
                string email = context.Request.Form["tbCorreo"];
                string phone = context.Request.Form["tbTelefono"];
                string address = context.Request.Form["tbDireccion"];

                // Verificar que los valores se han obtenido
                if (string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                    string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
                {
                    throw new Exception("Uno o más parámetros están vacíos o no se han enviado.");
                }

                // Crear y ejecutar la solicitud
                string result = CrearSolicitud(dni, firstName, lastName, email, phone, address);

                // Configurar la respuesta
                context.Response.ContentType = "application/json";
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonConvert.SerializeObject(new { error = ex.Message }));
            }
        }

        private string CrearSolicitud(string dni, string firstName, string lastName, string email, string phone, string address)
        {
            using (var conexBD = new ConexionBD())
            {
                var conex = conexBD.ObtenerConexion();

                using (var cmd = SetupCommand(conex))
                {
                    PersonParameters(cmd, dni, firstName, lastName, email, phone, address);

                    try
                    {
                        conexBD.AbrirConexion();
                        ExecuteDatabaseCommand(cmd);

                        return PrepareResponse(cmd);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error al ejecutar el comando en la base de datos.", ex);
                    }
                }
            }
        }

        private SqlCommand SetupCommand(SqlConnection conex)
        {
            var cmd = new SqlCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "[gral].[sp_Registro_Solicitud]",
                Connection = conex
            };
            return cmd;
        }

        private void PersonParameters(SqlCommand cmd, string dni, string firstName, string lastName, string email, string phone, string address)
        {
            cmd.Parameters.Add("@pcEmail", SqlDbType.NVarChar, 500).Value = email;
            cmd.Parameters.Add("@pnDNI", SqlDbType.NVarChar, 500).Value = dni;
            cmd.Parameters.Add("@pcFirstName", SqlDbType.NVarChar, 500).Value = firstName;
            cmd.Parameters.Add("@pcLastName", SqlDbType.NVarChar, 500).Value = lastName;
            cmd.Parameters.Add("@pnAddress", SqlDbType.NVarChar, 500).Value = address;
            cmd.Parameters.Add("@pnPhone", SqlDbType.NVarChar, 500).Value = phone;
            cmd.Parameters.Add("@MENSAGE", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@RETORNO", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
        }

        private void ExecuteDatabaseCommand(SqlCommand cmd)
        {
            cmd.ExecuteNonQuery();
        }

        private string PrepareResponse(SqlCommand cmd)
        {
            var responseObj = new
            {
                Retorno = Convert.ToInt32(cmd.Parameters["@RETORNO"].Value),
                Mensaje = cmd.Parameters["@MENSAGE"].Value.ToString(),
                Idconstancia = Convert.ToInt32(cmd.Parameters["@Id"].Value),
                Clave = Convert.ToString(cmd.Parameters["@Clave"].Value)
            };

            return JsonConvert.SerializeObject(responseObj);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}