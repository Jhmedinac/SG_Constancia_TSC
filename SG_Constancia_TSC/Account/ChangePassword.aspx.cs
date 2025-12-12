using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;



namespace SG_Constancia_TSC.Account 
{

    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Form.Attributes.Add("autocomplete", "off");
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (!IsPostBack && !manager.HasPassword(User.Identity.GetUserId()))
            {
                PageHeader.InnerText = "Establecer contraseña";
                btnChangePassword.Visible = false;
                btnSetPassword.Visible = true;
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {     ErrorMessage.Text = string.Empty;
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), tbCurrentPassword.Text, tbPassword.Text);
            if (result.Succeeded)
            {
                var user = manager.FindById(User.Identity.GetUserId());
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("~/Account/Manage.aspx?m=ChangePwdSuccess");
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }

        protected void btnSetPassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Create the local login info and link the local account to the user
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                IdentityResult result = manager.AddPassword(User.Identity.GetUserId(), tbPassword.Text);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage.aspx?m=SetPwdSuccess");
                }
                else
                {
                    ErrorMessage.Text = result.Errors.FirstOrDefault();
                }
            }
       
        
        
        }


        protected void Password_TextChanged(object sender, EventArgs e)
        {

        }
    }

}