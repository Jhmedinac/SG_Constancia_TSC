using DevExpress.Web;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace SG_Constancia_TSC.Mantenimiento
{
    public partial class Parametros : System.Web.UI.Page
    {
        private const string FirmaSessionKey = "FirmaBytesTemporal";
        protected void SqlDataSourceParametro_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {
            // Buscamos el parámetro específico para la firma.
            if (e.Command.Parameters.Contains("@firma"))
            {
                // Convertimos (cast) el parámetro genérico DbParameter al tipo específico SqlParameter.
                SqlParameter firmaParam = e.Command.Parameters["@firma"] as SqlParameter;

                // Si la conversión es exitosa (no es nulo), le asignamos el tipo de dato correcto de SQL Server.
                if (firmaParam != null)
                {
                    firmaParam.SqlDbType = System.Data.SqlDbType.VarBinary;
                }
            }
        }

        protected void SqlDataSourceParametro_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            
            if (e.Exception != null)
            {
                
                string mensajeError = e.Exception.Message;
                System.Diagnostics.Debug.WriteLine("Error de BD: " + mensajeError);

                
                e.ExceptionHandled = true;
            }
            else
            {
                
                int filasActualizadas = e.AffectedRows;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Limpiar la sesión al cargar la página por primera vez
                Session.Remove(FirmaSessionKey);
            }
            Form.Attributes.Add("autocomplete", "off");
        }

        // Evento que se dispara ANTES de que el grid intente actualizar la base de datos
        protected void ParFirAdjunt_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            // Verificamos si hay una nueva firma en la sesión
            if (Session[FirmaSessionKey] is byte[] firmaBytes)
            {
                // CASO 1: El usuario subió una nueva firma. La usamos.
                e.NewValues["firma"] = firmaBytes;
            }
            else
            {
                // CASO 2: El usuario NO subió una firma nueva.
                // Mantenemos la firma original que ya estaba en la base de datos para no perderla.
                // De esta forma, el parámetro @firma siempre tiene un valor.
                e.NewValues["firma"] = e.OldValues["firma"];
            }

            
        }

        // Evento que se dispara DESPUÉS de una actualización exitosa o fallida
        protected void ParFirAdjunt_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                ((ASPxGridView)sender).JSProperties["cpUpdatedMessage"] = "Su registro se ha actualizado correctamente.";
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpUpdatedMessage"] = "Se produjo un error al intentar actualizar los datos.";
                e.ExceptionHandled = true;
            }

            // Limpiamos la sesión después de la operación (exitosa o no)
            Session.Remove(FirmaSessionKey);
            Utili.ParametrosHelper.CargarParametrosEnSesion();
        }

        // Evento que se dispara DESPUÉS de una inserción exitosa o fallida
        protected void ParFirAdjunt_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            if (e.Exception == null)
                ((ASPxGridView)sender).JSProperties["cpUpdatedMessage"] = "Su registro se ha agregado correctamente.";
            else
            {
                ((ASPxGridView)sender).JSProperties["cpUpdatedMessage"] = "Se produjo un error al intentar agregar los datos.";
                e.ExceptionHandled = true;
            }

            // Limpiamos la sesión también después de insertar
            Session.Remove(FirmaSessionKey);
        }


        // Evento que se dispara cuando el formulario de edición se va a mostrar
        protected void ParFirAdjunt_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            // Limpiamos la sesión al empezar a editar para evitar usar una firma anterior
            Session.Remove(FirmaSessionKey);
        }

        // Evento que se dispara cuando se cancela la edición
        protected void ParFirAdjunt_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            // Limpiamos la sesión si el usuario cancela
            Session.Remove(FirmaSessionKey);
        }


       
        protected void UploadFirma_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                byte[] fileBytes = e.UploadedFile.FileBytes;

                // Guardamos los bytes de la imagen en la sesión del servidor
                Session[FirmaSessionKey] = fileBytes;

                string base64 = Convert.ToBase64String(fileBytes);
               
                // Enviamos la imagen en formato base64 al cliente para la vista previa
                e.CallbackData = "data:image/png;base64," + base64;
            }
        }


    }


}