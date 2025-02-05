using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using SG_Constancia_TSC.Models;

namespace SG_Constancia_TSC
{
        public partial class Confirm : Page
        {
        //protected string StatusMessage
        //{
        //    get;
        //    private set;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            string userId = IdentityHelper.GetUserIdFromRequest(Request);
            string code = IdentityHelper.GetCodeFromRequest(Request);

            if (code != null && userId != null)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var result = manager.ConfirmEmail(userId, code);

                if (result.Succeeded)
                {
                    successPanel.Visible = true;
                    errorPanel.Visible = false; // Ocultar el panel de error si la confirmación tiene éxito.
                }
                else
                {
                    // Manejar el error y mostrar un mensaje de error adecuado al usuario.
                    errorPanel.Visible = true;
                    //errorMessageLabel.Text = "Ha ocurrido un error al confirmar el correo electrónico.";
                }
            }
            else
            {
                successPanel.Visible = false;
                errorPanel.Visible = true;
                //errorMessageLabel.Text = "El código o el usuario son nulos, no se puede realizar la confirmación.";
            }
        }

        }
}