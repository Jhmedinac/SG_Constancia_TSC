using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SG_Constancia_TSC
{
    public partial class MainMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            TimeoutControl1.TimeOutUrl = "TimeOutPage.aspx";
            

            var user = HttpContext.Current.User;

            if (user.IsInRole("SuperAdmin"))
            {
                
                foreach (DevExpress.Web.NavBarGroup g in nbMain.Groups)
                    g.Visible = true;
            }
            else if (user.IsInRole("Admin"))
            {
                
                nbMain.Groups[0].Visible = true;
                nbMain.Groups[1].Visible = true;
                nbMain.Groups[2].Visible = true;
                nbMain.Groups[3].Items[0].Visible = true;
                nbMain.Groups[3].Items[1].Visible = true;
                nbMain.Groups[3].Items[2].Visible = true; 
                nbMain.Groups[3].Items[3].Visible = false; 
            }
            else if (user.IsInRole("Revisor"))
            {
                nbMain.Groups[0].Visible = true;
                nbMain.Groups[1].Visible = true;
                nbMain.Groups[2].Items[3].Visible = false; 
                nbMain.Groups[3].Visible = false;
            }
            else
            {
                nbMain.Groups[0].Visible = true;
                nbMain.Groups[1].Visible = false;
                nbMain.Groups[2].Visible = false;
                nbMain.Groups[3].Visible = false;
            }

        }
    }
}