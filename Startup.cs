using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RotatingChores.Startup))]
namespace RotatingChores
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
