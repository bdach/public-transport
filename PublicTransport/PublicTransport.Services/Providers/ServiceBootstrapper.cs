using PublicTransport.Services.UnitsOfWork;
using Splat;

namespace PublicTransport.Services.Providers
{
    public class ServiceBootstrapper
    {
        public void RegisterServices()
        {
            Locator.CurrentMutable.Register(() => new LoginService(), typeof(ILoginService));

            Locator.CurrentMutable.Register(() => new CityUnitOfWork(), typeof(ICityUnitOfWork));
        }
    }
}
