using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WaiterRestaurantApplication.Startup))]
namespace WaiterRestaurantApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
