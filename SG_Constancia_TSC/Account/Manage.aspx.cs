using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using SG_Constancia_TSC.Models;

namespace SG_Constancia_TSC.Account
{
    public partial class Manage : System.Web.UI.Page
    {


        protected string SuccessMessage
        {
            get;
            private set;
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(User.Identity.GetUserId());
        }

        public bool HasPhoneNumber { get; private set; }

        public bool TwoFactorEnabled { get; private set; }

        public bool TwoFactorBrowserRemembered { get; private set; }

        public int LoginsCount { get; set; }

        protected void Page_Load()
        {
            if (IsPostBack)
            {
                if (Session["Name_user"] == null)
                {
                    Response.RedirectLocation = "/Account/Login.aspx";
                }
                if (!User.Identity.IsAuthenticated)
                {
                    Response.RedirectLocation = "/Account/Login.aspx";

                }
            }
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            HasPhoneNumber = String.IsNullOrEmpty(manager.GetPhoneNumber(User.Identity.GetUserId()));

            //// Enable this after setting up two-factor authentientication
            ////PhoneNumber.Text = manager.GetPhoneNumber(User.Identity.GetUserId()) ?? String.Empty;

            TwoFactorEnabled = manager.GetTwoFactorEnabled(User.Identity.GetUserId());

            LoginsCount = manager.GetLogins(User.Identity.GetUserId()).Count;

            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;

            if (!IsPostBack)
            {
                // Determine the sections to render
                if (HasPassword(manager))
                {
                    ChangePassword.Visible = true;
                }
                else
                {
                    CreatePassword.Visible = true;
                    ChangePassword.Visible = false;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Limpia el querystring de la acción
                    Form.Action = ResolveUrl("~/Account/Manage.aspx");

                    string texto =
                        message == "ChangePwdSuccess" ? "Tu contraseña ha sido cambiada."
                        : message == "SetPwdSuccess" ? "Su contraseña ha sido establecida."
                        : message == "RemoveLoginSuccess" ? "La cuenta fue eliminada."
                        : message == "AddPhoneNumberSuccess" ? "Se agregó el número de teléfono."
                        : message == "RemovePhoneNumberSuccess" ? "Se quitó el número de teléfono."
                        : string.Empty;

                    if (!string.IsNullOrEmpty(texto))
                    {
                        
                        var t = System.Web.HttpUtility.JavaScriptStringEncode("¡Éxito!");
                        var m = System.Web.HttpUtility.JavaScriptStringEncode(texto);
                        var script = $"Swal.fire({{ icon: 'success', title: '{t}', text: '{m}' }});";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "swalManageMsg", script, true);
                    }

                    // Evita que también intente mostrar el SuccessMessage anterior
                    SuccessMessage = string.Empty;
                }
            }
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // Remove phonenumber from user
        protected void RemovePhone_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var result = manager.SetPhoneNumber(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return;
            }
            var user = manager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                Response.Redirect("/Account/Manage.aspx?m=RemovePhoneNumberSuccess");
            }
        }

        // DisableTwoFactorAuthentication
        protected void TwoFactorDisable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), false);

            Response.Redirect("/Account/Manage.aspx");
        }

        //EnableTwoFactorAuthentication 
        protected void TwoFactorEnable_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), true);

            Response.Redirect("/Account/Manage.aspx");
        }


    }
}