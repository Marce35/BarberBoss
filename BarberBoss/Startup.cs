using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BarberBoss.Startup))]
namespace BarberBoss
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
