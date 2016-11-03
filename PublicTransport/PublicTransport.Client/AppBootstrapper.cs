using PublicTransport.Client.ViewModels;
using PublicTransport.Client.Views;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        public AppBootstrapper()
        {
            Router = new RoutingState();
            Locator.CurrentMutable.RegisterLazySingleton(() => this, typeof(IScreen));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellViewModel(
                Locator.Current.GetService<IScreen>()), typeof(ShellViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellView(Locator.Current.GetService<ShellViewModel>()), typeof(IViewFor<ShellViewModel>));
            Locator.CurrentMutable.RegisterLazySingleton(() => new CityView(), typeof(IViewFor<CityViewModel>));
        }
    }
}