using PublicTransport.Client.ViewModels;
using PublicTransport.Client.Views;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.Providers
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        public AppBootstrapper()
        {
            Router = new RoutingState();
            Locator.CurrentMutable.RegisterLazySingleton(() => this, typeof(IScreen));
            Locator.CurrentMutable.Register(() => new RoutableViewModelFactory(), typeof(RoutableViewModelFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellViewModel(
                Locator.Current.GetService<IScreen>(),
                Locator.Current.GetService<RoutableViewModelFactory>()
            ), typeof(ShellViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellView(Locator.Current.GetService<ShellViewModel>()), typeof(IViewFor<ShellViewModel>));
            Locator.CurrentMutable.Register(() => new MenuView(), typeof(IViewFor<MenuViewModel>));
            Locator.CurrentMutable.Register(() => new CityView(), typeof(IViewFor<CityViewModel>));
            Locator.CurrentMutable.Register(() => new StreetView(), typeof(IViewFor<StreetViewModel>));
            Locator.CurrentMutable.Register(() => new PlaceholderView(), typeof(IViewFor<PlaceholderViewModel>));
        }
    }
}