using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SG_Constancia_TSC.Mantenimiento
{
    public partial class Parametros : System.Web.UI.Page
    {
        protected void Page_Load1(object sender, EventArgs e)
        {

            Form.Attributes.Add("autocomplete", "off");



        }
        private int GetNextParametroId()
        {
            
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            using (var cmd = new SqlCommand(@"
                 /* Bloquea la tabla durante el cálculo para reducir carreras */
                SELECT ISNULL(MAX(codigo_parametro),0) + 1
               FROM gral.Parametros WITH (TABLOCKX);", con))
            {
                con.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        protected void ParFirAdjunt_RowInserted1(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {

            var grid = (ASPxGridView)sender;
            if (e.Exception == null)
            {
                grid.JSProperties["cpUpdatedMessage"] = "Su registro se ha agregado correctamente.";
                grid.CancelEdit();
                grid.DataBind();
            }
            else
            {
                grid.JSProperties["cpUpdatedMessage"] = "Se produjo un error al agregar el parámetro: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }

        protected void ParFirAdjunt_RowUpdated1(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {

            var grid = (ASPxGridView)sender;
            if (e.Exception == null)
            {
                grid.JSProperties["cpUpdatedMessage"] = "Su registro se ha actualizado correctamente.";
            }
            else
            {
                grid.JSProperties["cpUpdatedMessage"] = "Se produjo un error al actualizar: " + e.Exception.Message;
                e.ExceptionHandled = true;
            }

        }


        private string ObtenerCodigoIdUsuarioActual()
        {

            var userKey = User.Identity.Name;
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString))
            using (var cmd = new SqlCommand("SELECT CodigoId FROM AspNetUsers WHERE UserName=@p ", cn))
            {
                cmd.Parameters.AddWithValue("@p", userKey);
                cn.Open();
                var val = cmd.ExecuteScalar();
                return val == null ? "" : val.ToString();
            }
        }

       
        protected void ParFirAdjunt_RowUpdating1(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["UserId"] = ObtenerCodigoIdUsuarioActual();
        }

        protected void ParFirAdjunt1_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            ParFirAdjunt1.SettingsText.PopupEditFormCaption = "CREAR PARÁMETRO";
        }

        protected void ParFirAdjunt1_RowInserting1(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {


            // PK obligatoria (int)
            e.NewValues["codigo_parametro"] = GetNextParametroId();

            // Validaciones mínimas
            string nombre = (e.NewValues["nombre_parametro"] ?? "").ToString().Trim();
            string valor = (e.NewValues["valor"] ?? "").ToString().Trim();
            string desc = (e.NewValues["descripcion"] ?? "").ToString().Trim();
            if (string.IsNullOrEmpty(nombre)) throw new ApplicationException("Debe ingresar el NOMBRE del parámetro.");
            if (string.IsNullOrEmpty(valor)) throw new ApplicationException("Debe ingresar el VALOR del parámetro.");
            if (string.IsNullOrEmpty(desc)) throw new ApplicationException("Debe ingresar la DESCRIPCIÓN del parámetro.");

            // UserId es NULLable en BD; intenta obtener el actual como int
            int userIdInt;
            object userIdFromEditor = e.NewValues["UserId"]; // por si el editor lo envió
            if (!int.TryParse(userIdFromEditor?.ToString(), out userIdInt))
            {
                // reintenta con tu helper actual
                if (!int.TryParse(ObtenerCodigoIdUsuarioActual()?.ToString(), out userIdInt))
                    e.NewValues["UserId"] = DBNull.Value; // deja NULL si no se puede
                else
                    e.NewValues["UserId"] = userIdInt;
            }
            else
            {
                e.NewValues["UserId"] = userIdInt;
            }




        }

        
    }
}