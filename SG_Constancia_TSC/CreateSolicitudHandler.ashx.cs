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
                // Obtener tipo de solicitante del request ("Natural" o "Juridica")
                string tipoSolicitante = context.Request.Form["tipoSolicitante"];

                // Campos comunes
                string email = context.Request.Form["tbCorreo"];
                string confirmEmail = context.Request.Form["tbConfirmCorreo"];
                string phone = context.Request.Form["tbTelefono"];

                string result = string.Empty;

                // Validación básica
                if (string.IsNullOrEmpty(tipoSolicitante))
                    throw new Exception("No se especificó el tipo de solicitante.");

                if (email != confirmEmail)
                    throw new Exception("El correo y la confirmación no coinciden.");

                // Persona Natural
                if (tipoSolicitante == "Natural")
                {
                    string dni = context.Request.Form["tbIdentidad"];
                    string firstName = context.Request.Form["tbNombre"];
                    string lastName = context.Request.Form["tbApellido"];

                    if (string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                        string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
                    {
                        throw new Exception("Faltan campos requeridos para persona natural.");
                    }

                    result = CrearSolicitud_Natural(dni, firstName, lastName, email, confirmEmail, phone, 0); // 0 = Natural
                }
                // Persona Jurídica
                else if (tipoSolicitante == "Juridica")
                {
                    string rtn = context.Request.Form["tbRTN"];
                    string institucion = context.Request.Form["tbInstitucion"];

                    if (string.IsNullOrEmpty(rtn) || string.IsNullOrEmpty(institucion) ||
                        string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
                    {
                        throw new Exception("Faltan campos requeridos para persona jurídica.");
                    }

                    result = CrearSolicitud_Juridica(rtn, institucion, email, confirmEmail, phone, 1); // 1 = Jurídica
                }
                else
                {
                    throw new Exception("Tipo de solicitante no válido.");
                }

                // Enviar respuesta
                context.Response.ContentType = "application/json";
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(JsonConvert.SerializeObject(new { error = ex.Message }));
            }

        }

        private string CrearSolicitud_Natural(string dni, string firstName, string lastName, string email, string confirmEmail, string phone, int tipoSolicitante)
        {
            using (var conexBD = new ConexionBD())
            {
                var conex = conexBD.ObtenerConexion();

                using (var cmd = new SqlCommand("gral.sp_Registro_Solicitud", conex))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pcEmail", email);
                    cmd.Parameters.AddWithValue("@pnDNI", dni);
                    cmd.Parameters.AddWithValue("@pcFirstName", firstName);
                    cmd.Parameters.AddWithValue("@pcLastName", lastName);
                    cmd.Parameters.AddWithValue("@pnPhone", phone ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pcRTN", DBNull.Value);
                    cmd.Parameters.AddWithValue("@pcInstitucion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@pbTipoSolicitud", tipoSolicitante); // 0 = natural

                    // Parámetros de salida
                    cmd.Parameters.Add("@MENSAGE", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@RETORNO", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                    try
                    {
                        conexBD.AbrirConexion();
                        cmd.ExecuteNonQuery();

                        return PrepareResponse(cmd);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error al ejecutar la solicitud natural.", ex);
                    }
                }
            }
        }


        private string CrearSolicitud_Juridica(string rtn, string institucion, string email, string confirmEmail, string phone, int tipoSolicitante)
        {
            using (var conexBD = new ConexionBD())
            {
                var conex = conexBD.ObtenerConexion();

                using (var cmd = new SqlCommand("gral.sp_Registro_Solicitud", conex))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@pcEmail", email);
                    cmd.Parameters.AddWithValue("@pnDNI", DBNull.Value);
                    cmd.Parameters.AddWithValue("@pcFirstName", DBNull.Value);
                    cmd.Parameters.AddWithValue("@pcLastName", DBNull.Value);
                    cmd.Parameters.AddWithValue("@pnPhone", phone ?? string.Empty);
                    cmd.Parameters.AddWithValue("@pcRTN", rtn);
                    cmd.Parameters.AddWithValue("@pcInstitucion", institucion);
                    cmd.Parameters.AddWithValue("@pbTipoSolicitud", tipoSolicitante); // 1 = jurídica

                    // Parámetros de salida
                    cmd.Parameters.Add("@MENSAGE", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@RETORNO", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Clave", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

                    try
                    {
                        conexBD.AbrirConexion();
                        cmd.ExecuteNonQuery();

                        return PrepareResponse(cmd);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error al ejecutar la solicitud jurídica.", ex);
                    }
                }
            }
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