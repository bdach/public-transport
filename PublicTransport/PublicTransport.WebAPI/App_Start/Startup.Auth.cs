using Microsoft.Owin.Security.OAuth;
using Owin;

namespace PublicTransport.WebAPI
{
    public partial class Startup
    {
        static Startup()
        {
            OAuthOptions = new OAuthAuthorizationServerOptions();
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; }

        public static void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}