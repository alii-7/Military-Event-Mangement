using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tarbya.Startup))]
namespace Tarbya
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
