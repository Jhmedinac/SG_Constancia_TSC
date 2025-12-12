using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace SG_Constancia_TSC
{
    public class CreateSolicitudHandler : IHttpHandler, IRequiresSessionState
   
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

                // ✅ Obtener el identificador del usuario (ID o IP)
                int userID = 0;
                string userIdentifier = string.Empty;

                if (context.Session["User_id"] != null)
                {
                    userID = Convert.ToInt32(context.Session["User_id"]);
                    userIdentifier = userID.ToString();
                }
                else
                {
                    // Obtener IP del cliente si no hay sesión
                    string userIP = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (string.IsNullOrEmpty(userIP))
                        userIP = context.Request.ServerVariables["REMOTE_ADDR"];

                    userIdentifier = userIP;
                }

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

                    // 🔹 Si tu procedimiento solo acepta int, envía userID (0 si no hay sesión)
                    // 🔹 Si ya lo adaptaste para aceptar texto, usa userIdentifier
                    result = CrearSolicitud_Natural(dni, firstName, lastName, email, confirmEmail, phone, 0, userID, userIdentifier);
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

                    result = CrearSolicitud_Juridica(rtn, institucion, email, confirmEmail, phone, 1, userID, userIdentifier);
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

        private string CrearSolicitud_Natural(string dni, string firstName, string lastName, string email, string confirmEmail, string phone, int tipoSolicitante, int UserID, string ipclient )
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
                    cmd.Parameters.AddWithValue("@pcUsuarioCreacionId", UserID); // 
                    cmd.Parameters.AddWithValue("@ipclient", ipclient); // 

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
                        // Obtener usuario actual
                        int codigoUsuario = UserID;

                        // Registrar error en la base de datos
                        using (SqlCommand cmdError = new SqlCommand(@"
                            INSERT INTO [gral].[Bitacora_Errores] 
                            (codigo_usuario, fecha_hora, componente, descripcion)
                            VALUES (@codigo_usuario, @fecha_hora, @componente, @descripcion)", conex))
                        {
                            cmdError.Parameters.AddWithValue("@codigo_usuario", codigoUsuario);
                            cmdError.Parameters.AddWithValue("@fecha_hora", DateTime.Now);
                            cmdError.Parameters.AddWithValue("@componente", "CreateSolicitudHandler");
                            cmdError.Parameters.AddWithValue("@descripcion", ex.ToString());
                            cmdError.ExecuteNonQuery();
                        }

                        // Opcional: cerrar conexión
                        conexBD.CerrarConexion();

                        // Retornar respuesta controlada como JSON
                        var errorResponse = new
                        {
                            success = false,
                            message = "Ocurrió un error al procesar la solicitud. El equipo técnico ha sido notificado."
                        };

                        return JsonConvert.SerializeObject(errorResponse);
                    }
                }
            }
        }


        private string CrearSolicitud_Juridica(string rtn, string institucion, string email, string confirmEmail, string phone, int tipoSolicitante, int UserID, string ipclient)
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
                    cmd.Parameters.AddWithValue("@pcUsuarioCreacionId", UserID);
                    cmd.Parameters.AddWithValue("@ipclient", ipclient); // 
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
                        // Obtener usuario actual
                        int codigoUsuario = UserID;

                        // Registrar error en la base de datos
                        using (SqlCommand cmdError = new SqlCommand(@"
                            INSERT INTO [gral].[Bitacora_Errores] 
                            (codigo_usuario, fecha_hora, componente, descripcion)
                            VALUES (@codigo_usuario, @fecha_hora, @componente, @descripcion)", conex))
                        {
                            cmdError.Parameters.AddWithValue("@codigo_usuario", codigoUsuario);
                            cmdError.Parameters.AddWithValue("@fecha_hora", DateTime.Now);
                            cmdError.Parameters.AddWithValue("@componente", "CreateSolicitudHandler");
                            cmdError.Parameters.AddWithValue("@descripcion", ex.ToString());
                            cmdError.ExecuteNonQuery();
                        }

                        // Opcional: cerrar conexión
                        conexBD.CerrarConexion();

                        // Retornar respuesta controlada como JSON
                        var errorResponse = new
                        {
                            success = false,
                            message = "Ocurrió un error al procesar la solicitud. El equipo técnico ha sido notificado."
                        };

                        return JsonConvert.SerializeObject(errorResponse);
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