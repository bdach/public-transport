using Splat;

namespace PublicTransport.Services.Providers
{
    public class ServiceBootstrapper
    {
        public void RegisterServices()
        {
            Locator.CurrentMutable.Register(() => new LoginService(), typeof(ILoginService));
        }
    }
}
