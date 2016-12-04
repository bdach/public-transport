using Splat;

namespace PublicTransport.Services.Providers
{
    public class ServiceBootstrapper
    {
        public void RegisterServices()
        {
            Locator.CurrentMutable.Register(() => new StopService(), typeof(IStopService));
            Locator.CurrentMutable.Register(() => new FareService(), typeof(IFareService));
        }
    }
}
