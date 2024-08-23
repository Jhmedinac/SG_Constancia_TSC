using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SG_Constancia_TSC.Models;


namespace SG_Constancia_TSC {
    public partial class Register : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e)
        {

            Form.Attributes.Add("autocomplete", "off");
            if (!IsPostBack)
            {
                Panel_Content.Visible = true;
            }
            else
            {
                if (IsPostBack)
                {
                    if (Session["Name_user"] == null)
                    {
                        Response.RedirectLocation = "../TimeOutPage.aspx";
                    }
                    if (!User.Identity.IsAuthenticated)
                    {
                        Response.RedirectLocation = "/Account/Login.aspx";
                        //Response.Redirect("~/Account/Login.aspx");
                    }
                }
            }

        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            // DXCOMMENT: Your Registration logic 
            int año = DateTime.Now.Year;
            string Password = "SSC" + año + "*tsc";

            //if (HttpContext.Current.GetOwinContext() != null)
            //{
            //    // Lógica relacionada con OWIN
            //    Debug.WriteLine("OWIN disponible");
            //}
            //else
            //{
            //    Debug.WriteLine("OWIN no disponible");
            //    // Manejo de la situación cuando OWIN no está disponible
            //}

            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = RegisterUserNameTextBox.Text.ToUpper(), Email = EmailTextBox.Text, Nombre_Usuario = NameuserTextBox.Text };
            IdentityResult result = manager.Create(user, Password);
            if (result.Succeeded)
            {
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                string code = manager.GenerateEmailConfirmationToken(user.Id);
                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);

                try
                {
                    string rutaplantilla = Server.MapPath("~/Mensaje.html");
                    StringBuilder plantilla = new StringBuilder();
                    plantilla.Append(llama_plantilla(rutaplantilla));
                    plantilla.Replace("$Nombre$", Convert.ToString(NameuserTextBox.Text).ToUpper());
                    plantilla.Replace("$Usuario$", Convert.ToString(RegisterUserNameTextBox.Text).ToUpper());
                    plantilla.Replace("$Password$", Password);
                    plantilla.Replace("$CallbackUrl$", callbackUrl);

                    manager.SendEmail(user.Id, "Confirmar Cuenta", plantilla.ToString());
                }
                catch (SmtpException smtpEx)
                {
                    // Problemas específicos con SMTP
                    this.ErrorMessage.Text = $"Error SMTP: {smtpEx.Message}";
                }
                catch (Exception ex)
                {
                    this.ErrorMessage.Text = ex.Message;
                    // If there is an error sending the email, delete the user and show an error message
                    manager.Delete(user);
                    ErrorMessage.Text = "Se produjo un error al enviar el correo electrónico de confirmación. Por favor, inténtelo de nuevo más tarde.";
                    return; // Exit the function to prevent further processing
                }


                if (user.EmailConfirmed)
                {
                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    ErrorMessage.Text = "Se ha enviado un correo electrónico al usuario. Por favor, Asígne el Rol.";
                }
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }

        protected static string llama_plantilla(string mi_plantilla)
        {
            System.Text.ASCIIEncoding v_codigoAscii = new System.Text.ASCIIEncoding();
            System.Net.WebClient v_recibeDato = new System.Net.WebClient();
            byte[] v_recibe;
            string v_recibecadena;

            try
            {
                v_recibe = v_recibeDato.DownloadData(mi_plantilla);
                v_recibecadena = v_codigoAscii.GetString(v_recibe);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString() + ex.ToString());
            }

            return v_recibecadena;

        }

        
    }
}