using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dev14_Net46_Mvc5.Startup))]
namespace Dev14_Net46_Mvc5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
