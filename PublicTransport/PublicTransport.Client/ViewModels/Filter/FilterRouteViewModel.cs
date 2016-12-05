using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Routes;
using PublicTransport.Client.ViewModels.Browse;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering routes.
    /// </summary>
    public class FilterRouteViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IRouteService _routeService;

        /// <summary>
        ///     <see cref="RouteReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        private RouteReactiveFilter _routeReactiveFilter;

        /// <summary>
        ///     The <see cref="RouteDto" /> currently selected by the user.
        /// </summary>
        private RouteDto _selectedRoute;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display the view model on.</param>
        /// <param name="routeService">Service used in the view model to access the database.</param>
        public FilterRouteViewModel(IScreen screen, IRouteService routeService = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _routeService = routeService ?? Locator.Current.GetService<IRouteService>();
            _routeReactiveFilter = new RouteReactiveFilter();
            Routes = new ReactiveList<RouteDto>();
            RouteTypes = new ReactiveList<RouteType>(Enum.GetValues(typeof(RouteType)).Cast<RouteType>());

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedRoute).Select(a => a != null);

            #region Route filtering command

            FilterRoutes = ReactiveCommand.CreateAsyncTask(async _ => await _routeService.FilterRoutesAsync(RouteReactiveFilter.Convert()));
            FilterRoutes.Subscribe(result =>
            {
                Routes.Clear();
                Routes.AddRange(result);
            });
            FilterRoutes.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch route data from the database. Please contact the system administrator.", e));

            #endregion

            #region Automatically filtering routes

            this.WhenAnyValue(vm => vm.RouteReactiveFilter.AgencyNameFilter, vm => vm.RouteReactiveFilter.ShortNameFilter,
                    vm => vm.RouteReactiveFilter.LongNameFilter, vm => vm.RouteReactiveFilter.RouteTypeFilter)
                .Where(_ => RouteReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterRoutes);

            #endregion

            #region Delete route command

            DeleteRoute = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await _routeService.DeleteRouteAsync(SelectedRoute);
                return Unit.Default;
            });
            DeleteRoute.Subscribe(_ => SelectedRoute = null);
            DeleteRoute.InvokeCommand(FilterRoutes);
            DeleteRoute.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected route. Please contact the system administrator.", e));

            #endregion

            #region Button commands

            AddRoute = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditRouteViewModel(HostScreen, _routeService)));
            EditRoute = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditRouteViewModel(HostScreen, _routeService, SelectedRoute)));
            ShowTimetable = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new TimetableViewModel(HostScreen, _routeService, SelectedRoute)));

            #endregion

            #region Clearing enum choice

            ClearRouteTypeChoice = ReactiveCommand.Create();
            ClearRouteTypeChoice.Subscribe(_ => RouteReactiveFilter.RouteTypeFilter = null);

            #endregion

            #region Updating the list of agencies upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => (HostScreen.Router.NavigationStack.Last() == this) && RouteReactiveFilter.IsValid)
                .InvokeCommand(FilterRoutes);

            #endregion

            #region Disposing of contexts

            //HostScreen.Router.NavigateAndReset
            //    .Skip(1)
            //    .Subscribe(_ => _routeService.Dispose());

            #endregion
        }

        /// <summary>
        ///     The <see cref="RouteDto" /> currently selected by the user.
        /// </summary>
        public RouteDto SelectedRoute
        {
            get { return _selectedRoute; }
            set { this.RaiseAndSetIfChanged(ref _selectedRoute, value); }
        }

        /// <summary>
        ///     <see cref="RouteReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        public RouteReactiveFilter RouteReactiveFilter
        {
            get { return _routeReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeReactiveFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="AgencyDto" /> objects currently displayed to the user.
        /// </summary>
        public ReactiveList<RouteDto> Routes { get; protected set; }

        /// <summary>
        ///     The list of <see cref="RouteType" /> enumeration values.
        /// </summary>
        public ReactiveList<RouteType> RouteTypes { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="RouteDto" /> objects from the database, using the <see cref="RouteReactiveFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<RouteDto[]> FilterRoutes { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for adding a new <see cref="Route" /> to the database.
        /// </summary>
        public ReactiveCommand<object> AddRoute { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for editing a <see cref="Route" />.
        /// </summary>
        public ReactiveCommand<object> EditRoute { get; protected set; }

        /// <summary>
        ///     Deletes the currently selected <see cref="Route" />
        /// </summary>
        public ReactiveCommand<Unit> DeleteRoute { get; set; }

        /// <summary>
        ///     Clears the route type filter.
        /// </summary>
        public ReactiveCommand<object> ClearRouteTypeChoice { get; protected set; }

        /// <summary>
        ///     Shows the timetable for the selected route.
        /// </summary>
        public ReactiveCommand<object> ShowTimetable { get; protected set; }

        /// <summary>
        ///     String uniquely identifying the current view model.
        /// </summary>
        public string UrlPathSegment => AssociatedMenuOption.ToString();

        /// <summary>
        ///     Host screen to display on.
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        ///     Gets the <see cref="MenuOption" /> enum value that associates a menu item with the concrete view model.
        /// </summary>
        public MenuOption AssociatedMenuOption => MenuOption.Route;
    }
}