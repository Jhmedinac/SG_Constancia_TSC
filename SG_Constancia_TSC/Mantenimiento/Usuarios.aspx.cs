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
                //Roles_Usuario.ShowOnPageLoad = false;
            }

        }

        protected void Usuario_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            if (Usuario.GetRowValuesByKeyValue(e.EditingKeyValue, "RoleId").ToString() == "")
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "<script type=\"text/javascript\">alert('Debe de agregar un perfil al usuario, para actualizar los datos.');</script>");
                ((ASPxGridView)sender).JSProperties["cpUpdateMessageUser"] = "Debe de agregar un perfil al usuario, para actualizar los datos.";
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