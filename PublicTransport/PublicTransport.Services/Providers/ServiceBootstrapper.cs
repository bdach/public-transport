using Splat;

namespace PublicTransport.Services.Providers
{
    public class ServiceBootstrapper
    {
        public void RegisterServices()
        {
            Locator.CurrentMutable.Register(() => new LoginService(), typeof(ILoginService));

            Locator.CurrentMutable.Register(() => new AgencyService(), typeof(IAgencyService));
            Locator.CurrentMutable.Register(() => new ZoneService(), typeof(IZoneService));
            Locator.CurrentMutable.Register(() => new UserService(), typeof(IUserService));
            Locator.CurrentMutable.Register(() => new StopService(), typeof(IStopService));
            Locator.CurrentMutable.Register(() => new FareService(), typeof(IFareService));
            Locator.CurrentMutable.Register(() => new RouteService(), typeof(IRouteService));
        }
    }
}
