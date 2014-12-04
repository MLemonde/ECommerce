using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MFMElectronique.Startup))]
namespace MFMElectronique
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
