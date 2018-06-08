using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CMS_Web.Startup))]
namespace CMS_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
