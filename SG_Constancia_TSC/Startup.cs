using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SG_Constancia_TSC.Startup), "SG_Constancia_TSC")]

// Files related to ASP.NET Identity duplicate the Microsoft ASP.NET Identity file structure and contain initial Microsoft comments.

namespace SG_Constancia_TSC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}