using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Projeto_Lab4.Startup))]
namespace Projeto_Lab4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
