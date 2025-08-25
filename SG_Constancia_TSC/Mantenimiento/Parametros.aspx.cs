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
            // La propiedad 'e.Exception' contiene el error si algo falló.
            if (e.Exception != null)
            {
                // ¡AQUÍ SE REGISTRA EL ERROR!
                // 1. Puedes guardar el mensaje de error en un archivo de log, en otra tabla, etc.
                string mensajeError = e.Exception.Message;
                System.Diagnostics.Debug.WriteLine("Error de BD: " + mensajeError);

                // 2. Puedes preparar un mensaje más amigable para el usuario.
                // (Opcional, porque el GridView también puede hacer esto).

                // 3. ¡MUY IMPORTANTE! Marcas la excepción como manejada para
                //    evitar que la aplicación se caiga y muestre la "pantalla amarilla de la muerte".
                e.ExceptionHandled = true;
            }
            else
            {
                // Si no hay excepción, la actualización fue exitosa.
                // e.AffectedRows te dirá cuántas filas fueron modificadas.
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

            // El resto de la lógica (como el UserId si es necesario) iría aquí.

            // El UserId no se obtiene aquí, ya que el SqlDataSource lo maneja con el parámetro @UserId
            // Sin embargo, si necesitas pasarlo explícitamente, este es el lugar.
             //e.NewValues["UserId"] = ObtenerCodigoIdUsuarioActual();

            // Verificar los parámetros que se están enviando
          
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

        // No se necesita el método ObtenerFirmaDesdeHiddenField()
        // No se necesita el método ParFirAdjunt_RowInserting() ya que el GridView se encarga
        // No se necesita el método ObtenerCodigoIdUsuarioActual() si se maneja por SqlDataSource
    }


}