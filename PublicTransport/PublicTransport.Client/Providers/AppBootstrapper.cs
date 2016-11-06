using PublicTransport.Client.ViewModels;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Client.Views;
using PublicTransport.Client.Views.Edit;
using PublicTransport.Client.Views.Filter;
using PublicTransport.Domain.Entities;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.Providers
{
    /// <summary>
    ///     Class responsible for application startup and dependency injection initialization.
    /// </summary>
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        /// <summary>
        ///     Constructor. Registers the appropiate classes in the container.
        /// </summary>
        public AppBootstrapper()
        {
            Router = new RoutingState();
            // Start-up objects.
            Locator.CurrentMutable.RegisterLazySingleton(() => this, typeof(IScreen));
            Locator.CurrentMutable.Register(() => new DetailViewModelFactory(), typeof(DetailViewModelFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellViewModel(
                Locator.Current.GetService<IScreen>(),
                Locator.Current.GetService<DetailViewModelFactory>()
            ), typeof(ShellViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(
                () => new ShellView(Locator.Current.GetService<ShellViewModel>()), typeof(IViewFor<ShellViewModel>));
            // View model views.
            Locator.CurrentMutable.Register(() => new MenuView(), typeof(IViewFor<MenuViewModel>));
            Locator.CurrentMutable.Register(() => new NotificationView(), typeof(IViewFor<NotificationViewModel>));
            Locator.CurrentMutable.Register(() => new PlaceholderView(), typeof(IViewFor<PlaceholderViewModel>));

            Locator.CurrentMutable.Register(() => new FilterCityView(), typeof(IViewFor<FilterCityViewModel>));
            Locator.CurrentMutable.Register(() => new EditCityView(), typeof(IViewFor<EditCityViewModel>));
            Locator.CurrentMutable.Register(() => new FilterStreetView(), typeof(IViewFor<FilterStreetViewModel>));
            Locator.CurrentMutable.Register(() => new EditStreetView(), typeof(IViewFor<EditStreetViewModel>));
            Locator.CurrentMutable.Register(() => new FilterAgencyView(), typeof(IViewFor<FilterAgencyViewModel>));
            Locator.CurrentMutable.Register(() => new EditAgencyView(), typeof(IViewFor<EditAgencyViewModel>));
            // Entity views.
            Locator.CurrentMutable.Register(() => new CityView(), typeof(IViewFor<City>));
            Locator.CurrentMutable.Register(() => new StreetView(), typeof(IViewFor<Street>));
            Locator.CurrentMutable.Register(() => new AgencyView(), typeof(IViewFor<Agency>));
        }

        /// <summary>
        ///     The router instance used throughout the lifetime of the application.
        /// </summary>
        public RoutingState Router { get; }
    }
}