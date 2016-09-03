using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CrossOverStock.Startup))]
namespace CrossOverStock
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
