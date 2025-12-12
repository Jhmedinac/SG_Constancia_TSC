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
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Attributes.Add("autocomplete", "off");

            if (!IsPostBack)
            {
                bool isSuperAdmin = Context.User.IsInRole("SuperAdmin");

                // Si NO es SuperAdmin, ocultar el rol SuperAdmin del combo
                if (!isSuperAdmin)
                {
                    SqlDataSource_Rol_Usuario.SelectCommand = @"
                SELECT Id, Name 
                FROM dbo.AspNetRoles 
                WHERE Name <> 'SuperAdmin'
                ORDER BY Name";
                }


            }



        }


        protected void btnGuarda_Rol_Usuario_Click(object sender, EventArgs e)
        {
            try
            {

                string constr = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);

                con.Open();
                string sql = @"INSERT INTO AspNetUserRoles VALUES ('" + Convert.ToString(Cbx_Usuario.Value) + "','" + Convert.ToString(Cbx_Rol.Value) + "')";
                SqlCommand cmdd = new SqlCommand(sql, con);

                cmdd.ExecuteNonQuery();
                Usuario.DataBind();

                Roles_Usuario.ShowOnPageLoad = false;
                Response.Redirect("~/Mantenimiento/Usuarios.aspx");
            }

            catch (Exception)
            {
                if (IsPostBack)
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                           @"<script type=""text/javascript"">alert('ERROR AL AGREGAR EL ROL AL USUARIO');</script>");

            }

        }


        protected void SqlDataSource_Codigo_roles_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            // Si el usuario No es SuperAdmin, se excluye el rol
            if (!Context.User.IsInRole("SuperAdmin"))
            {
                e.Command.CommandText = "SELECT Id, Name FROM dbo.AspNetRoles WHERE Name <> 'SuperAdmin' ORDER BY Name";
            }
            else
            {
                e.Command.CommandText = "SELECT Id, Name FROM dbo.AspNetRoles ORDER BY Name";
            }
        }

        protected void Usuario_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            bool isSuperAdmin = Context.User.IsInRole("SuperAdmin");
            if (!isSuperAdmin)
            {
                SqlDataSource_Codigo_roles.SelectCommand = @"
            SELECT Id, Name
            FROM dbo.AspNetRoles
            WHERE Id IN ('1','2')
            ORDER BY Name";
            }
            else
            {
                SqlDataSource_Codigo_roles.SelectCommand = @"
            SELECT Id, Name
            FROM dbo.AspNetRoles
            ORDER BY Name";
            }
            SqlDataSource_Codigo_roles.DataBind();
            Usuario.DataBind(); // forzar recarga del editor/data
                                // Luego tu validación original:
            if (Usuario.GetRowValuesByKeyValue(e.EditingKeyValue, "RoleId").ToString() == "")
            {
                ((ASPxGridView)sender).JSProperties["cpUpdateMessageUser"] = "Debe de agregar un rol al usuario,para actualizar los datos.";
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }






        protected void Usuario_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                ((ASPxGridView)sender).JSProperties["cpUpdateMessageUser"] = "Datos actualizados correctamente.";
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpUpdateMessageUser"] = "Se produjo un error al intentar actualizar los datos del usuario.";
                e.ExceptionHandled = true;

            }

        }


        protected void GV_Cargos_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e)
        {
            if (e.Exception == null)
            {
                ((ASPxGridView)sender).JSProperties["cpUpdateMessageCargo"] = "Datos actualizados correctamente.";
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpUpdateMessageCargo"] = "Se produjo un error al intentar actualizar los datos del cargo.";
                e.ExceptionHandled = true;

            }
        }

        protected void GV_Cargos_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e)
        {
            if (e.Exception == null)
            {
                ((ASPxGridView)sender).JSProperties["cpUpdateMessageCargo"] = "Datos agregados correctamente.";
            }
            else
            {
                ((ASPxGridView)sender).JSProperties["cpUpdateMessageCargo"] = "Se produjo un error al intentar agregar los datos del cargo.";
                e.ExceptionHandled = true;

            }
        }

    }
}