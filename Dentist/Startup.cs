using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dentist.Startup))]
namespace Dentist
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
