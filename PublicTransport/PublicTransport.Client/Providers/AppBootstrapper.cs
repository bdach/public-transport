using PublicTransport.Client.Services.Agencies;
using PublicTransport.Client.Services.Cities;
using PublicTransport.Client.Services.Login;
using PublicTransport.Client.Services.Streets;
using PublicTransport.Client.Services.Users;
using PublicTransport.Client.Services.Zones;
using PublicTransport.Client.ViewModels;
using PublicTransport.Client.ViewModels.Browse;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Client.ViewModels.Entities;
using PublicTransport.Client.ViewModels.Filter;
using PublicTransport.Client.Views;
using PublicTransport.Client.Views.Browse;
using PublicTransport.Client.Views.Edit;
using PublicTransport.Client.Views.Entities;
using PublicTransport.Client.Views.Filter;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Providers;
using ReactiveUI;
using Splat;
using IAgencyService = PublicTransport.Client.Services.Agencies.IAgencyService;
using ICityService = PublicTransport.Client.Services.Cities.ICityService;
using IStreetService = PublicTransport.Client.Services.Streets.IStreetService;
using IZoneService = PublicTransport.Client.Services.Zones.IZoneService;
using IUserService = PublicTransport.Client.Services.Users.IUserService;

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
            var serviceBootstrapper = new ServiceBootstrapper();
            serviceBootstrapper.RegisterServices();

            // Startup objects.
            Locator.CurrentMutable.RegisterLazySingleton(() => this, typeof(IScreen));
            Locator.CurrentMutable.Register(() => new LoginView(), typeof(IViewFor<LoginViewModel>));
            Locator.CurrentMutable.Register(() => new DetailViewModelFactory(), typeof(IDetailViewModelFactory));
            Locator.CurrentMutable.RegisterLazySingleton(() =>
                new ShellViewModel(Locator.Current.GetService<IScreen>(), Locator.Current.GetService<IDetailViewModelFactory>()), typeof(ShellViewModel));
            Locator.CurrentMutable.RegisterLazySingleton(() => new ShellView(Locator.Current.GetService<ShellViewModel>()), typeof(IViewFor<ShellViewModel>));
            Locator.CurrentMutable.Register(() => new MenuView(), typeof(IViewFor<MenuViewModel>));
            Locator.CurrentMutable.Register(() => new NotificationView(), typeof(IViewFor<NotificationViewModel>));
            Locator.CurrentMutable.Register(() => new PlaceholderView(), typeof(IViewFor<PlaceholderViewModel>));

            // View model views.
            Locator.CurrentMutable.Register(() => new EditAgencyView(), typeof(IViewFor<EditAgencyViewModel>));
            Locator.CurrentMutable.Register(() => new EditCalendarView(), typeof(IViewFor<EditCalendarViewModel>));
            Locator.CurrentMutable.Register(() => new EditCityView(), typeof(IViewFor<EditCityViewModel>));
            Locator.CurrentMutable.Register(() => new EditFareView(), typeof(IViewFor<EditFareViewModel>));
            Locator.CurrentMutable.Register(() => new EditRouteView(), typeof(IViewFor<EditRouteViewModel>));
            Locator.CurrentMutable.Register(() => new EditStopTimeView(), typeof(IViewFor<EditStopTimeViewModel>));
            Locator.CurrentMutable.Register(() => new EditStopView(), typeof(IViewFor<EditStopViewModel>));
            Locator.CurrentMutable.Register(() => new EditStreetView(), typeof(IViewFor<EditStreetViewModel>));
            Locator.CurrentMutable.Register(() => new EditTripView(), typeof(IViewFor<EditTripViewModel>));
            Locator.CurrentMutable.Register(() => new EditUserView(), typeof(IViewFor<EditUserViewModel>));
            Locator.CurrentMutable.Register(() => new EditZoneView(), typeof(IViewFor<EditZoneViewModel>));
            Locator.CurrentMutable.Register(() => new FilterAgencyView(), typeof(IViewFor<FilterAgencyViewModel>));
            Locator.CurrentMutable.Register(() => new FilterCityView(), typeof(IViewFor<FilterCityViewModel>));
            Locator.CurrentMutable.Register(() => new FilterFareView(), typeof(IViewFor<FilterFareViewModel>));
            Locator.CurrentMutable.Register(() => new FilterRouteView(), typeof(IViewFor<FilterRouteViewModel>));
            Locator.CurrentMutable.Register(() => new FilterStopView(), typeof(IViewFor<FilterStopViewModel>));
            Locator.CurrentMutable.Register(() => new FilterStreetView(), typeof(IViewFor<FilterStreetViewModel>));
            Locator.CurrentMutable.Register(() => new FilterUserView(), typeof(IViewFor<FilterUserViewModel>));
            Locator.CurrentMutable.Register(() => new FilterZoneView(), typeof(IViewFor<FilterZoneViewModel>));
            Locator.CurrentMutable.Register(() => new TimetableView(), typeof(IViewFor<TimetableViewModel>));

            // Entity views.
            Locator.CurrentMutable.Register(() => new CityView(), typeof(IViewFor<CityDto>));
            Locator.CurrentMutable.Register(() => new StreetView(), typeof(IViewFor<StreetDto>));
            Locator.CurrentMutable.Register(() => new ZoneView(), typeof(IViewFor<ZoneDto>));
            Locator.CurrentMutable.Register(() => new AgencyView(), typeof(IViewFor<AgencyDto>));
            Locator.CurrentMutable.Register(() => new UserView(), typeof(IViewFor<UserDto>));

            Locator.CurrentMutable.Register(() => new RouteView(), typeof(IViewFor<Route>));
            Locator.CurrentMutable.Register(() => new StopView(), typeof(IViewFor<Stop>));
            Locator.CurrentMutable.Register(() => new StopTimeView(), typeof(IViewFor<StopTime>));
            Locator.CurrentMutable.Register(() => new FareAttributeView(), typeof(IViewFor<FareAttribute>));
            Locator.CurrentMutable.Register(() => new RoleView(), typeof(IViewFor<RoleViewModel>));

            // WCF services.
            Locator.CurrentMutable.Register(() => new CityServiceClient(), typeof(ICityService));
            Locator.CurrentMutable.Register(() => new StreetServiceClient(), typeof(IStreetService));
            Locator.CurrentMutable.Register(() => new ZoneServiceClient(), typeof(IZoneService));
            Locator.CurrentMutable.Register(() => new AgencyServiceClient(), typeof(IAgencyService));
            Locator.CurrentMutable.Register(() => new UserServiceClient(), typeof(IUserService));
            Locator.CurrentMutable.Register(() => new LoginServiceClient(), typeof(ILoginService));
        }

        /// <summary>
        ///     The router instance used throughout the lifetime of the application.
        /// </summary>
        public RoutingState Router { get; }
    }
}