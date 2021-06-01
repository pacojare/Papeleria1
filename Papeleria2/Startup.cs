using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Papeleria2.Startup))]
namespace Papeleria2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
