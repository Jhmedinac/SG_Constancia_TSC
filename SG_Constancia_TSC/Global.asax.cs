using SG_Constancia_TSC.App_Start;
using System;

namespace SG_Constancia_TSC {
    public class Global_asax : System.Web.HttpApplication {
        void Application_Start(object sender, EventArgs e) {
            DevExpress.Web.ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            DevExpress.Security.Resources.AccessSettings.DataResources.SetRules(
                DevExpress.Security.Resources.DirectoryAccessRule.Allow(Server.MapPath("~/Content")),
                DevExpress.Security.Resources.UrlAccessRule.Allow()
            );
            DevExpress.XtraReports.Web.ASPxWebDocumentViewer.StaticInitialize();
        }

        void Application_End(object sender, EventArgs e) {
            // Code that runs on application shutdown
            Session.Abandon();
        }
    
        void Application_Error(object sender, EventArgs e) {
            // Code that runs when an unhandled error occurs
            string ErrorMessage;

            ErrorMessage = "La descripción del error es la siguiente : " + Server.GetLastError();
            string toemail = "jhmedina@tsc.gob.hn";
            string StrSubject = "Error en el sitio Consulta Secretaria";
            string strBody = ErrorMessage;

            SampleUtil.EnviarCorreo1("", StrSubject, toemail, strBody);
        }
    
        void Session_Start(object sender, EventArgs e) {
            // Code that runs when a new session is started
        }
    
        void Session_End(object sender, EventArgs e) {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.
        }
    }
}