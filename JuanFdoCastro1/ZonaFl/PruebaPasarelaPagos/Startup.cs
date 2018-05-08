using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PruebaPasarelaPagos.Startup))]
namespace PruebaPasarelaPagos
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
