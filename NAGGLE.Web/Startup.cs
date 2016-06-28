using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NAGGLE.Web.Startup))]
namespace NAGGLE.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
