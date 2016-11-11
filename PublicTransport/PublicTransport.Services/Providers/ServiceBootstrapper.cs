using PublicTransport.Services.UnitsOfWork;
using Splat;

namespace PublicTransport.Services.Providers
{
    public class ServiceBootstrapper
    {
        public void RegisterServices()
        {
            Locator.CurrentMutable.Register(() => new LoginService(), typeof(ILoginService));

            Locator.CurrentMutable.Register(() => new AgencyUnitOfWork(), typeof(IAgencyUnitOfWork));
            Locator.CurrentMutable.Register(() => new CityUnitOfWork(), typeof(ICityUnitOfWork));
            Locator.CurrentMutable.Register(() => new StreetUnitOfWork(), typeof(IStreetUnitOfWork));
            Locator.CurrentMutable.Register(() => new ZoneUnitOfWork(), typeof(IZoneUnitOfWork));
            Locator.CurrentMutable.Register(() => new UserUnitOfWork(), typeof(IUserUnitOfWork));
            Locator.CurrentMutable.Register(() => new StopUnitOfWork(), typeof(IStopUnitOfWork));
            Locator.CurrentMutable.Register(() => new FareUnitOfWork(), typeof(IFareUnitOfWork));
            Locator.CurrentMutable.Register(() => new RouteUnitOfWork(), typeof(IRouteUnitOfWork));
        }
    }
}
