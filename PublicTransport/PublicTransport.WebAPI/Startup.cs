using System.Web.Http;
using Microsoft.Owin;
using Owin;
using PublicTransport.WebAPI;

[assembly: OwinStartup(typeof(Startup))]
namespace PublicTransport.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}