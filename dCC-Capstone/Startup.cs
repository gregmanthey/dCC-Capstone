using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(dCC_Capstone.Startup))]
namespace dCC_Capstone
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
