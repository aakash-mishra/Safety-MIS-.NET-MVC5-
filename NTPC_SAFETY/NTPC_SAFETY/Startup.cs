using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NTPC_SAFETY.Startup))]
namespace NTPC_SAFETY
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
