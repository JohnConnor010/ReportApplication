using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReportApplication.Startup))]
namespace ReportApplication
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
