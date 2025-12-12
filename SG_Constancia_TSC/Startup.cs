using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SG_Constancia_TSC.Startup), "SG_Constancia_TSC")]


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