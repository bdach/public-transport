using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering routes.
    /// </summary>
    public class FilterRouteViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     <see cref="RouteService" /> used to fetch data from database.
        /// </summary>
        private readonly RouteService _routeService;

        /// <summary>
        ///     <see cref="DataTransfer.RouteFilter" /> object used to send query data to the service layer.
        /// </summary>
        private RouteFilter _routeFilter;

        /// <summary>
        ///     The <see cref="Route" /> currently selected by the user.
        /// </summary>
        private Route _selectedRoute;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display the view model on.</param>
        public FilterRouteViewModel(IScreen screen)
        {
            #region Field/property initialization

            HostScreen = screen;
            _routeService = new RouteService();
            _routeFilter = new RouteFilter();
            Routes = new ReactiveList<Route>();
            RouteTypes = new ReactiveList<RouteType>(Enum.GetValues(typeof(RouteType)).Cast<RouteType>());

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedRoute).Select(a => a != null);

            #region Route filtering command

            FilterRoutes =
                ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _routeService.FilterRoutes(RouteFilter)));
            FilterRoutes.Subscribe(result =>
            {
                Routes.Clear();
                Routes.AddRange(result);
            });
            FilterRoutes.ThrownExceptions.Subscribe(
                e =>
                    UserError.Throw(
                        "Cannot fetch route data from the database. Please contact the system administrator.", e));

            #endregion

            #region Automatically filtering routes

            this.WhenAnyValue(vm => vm.RouteFilter.AgencyNameFilter, vm => vm.RouteFilter.ShortNameFilter,
                    vm => vm.RouteFilter.LongNameFilter, vm => vm.RouteFilter.RouteTypeFilter)
                .Where(_ => RouteFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterRoutes);

            #endregion

            #region Delete agency command

            DeleteRoute = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem,
                async _ =>
                {
                    await Task.Run(() => _routeService.Delete(SelectedRoute));
                    return Unit.Default;
                });
            DeleteRoute.Subscribe(_ => SelectedRoute = null);
            DeleteRoute.InvokeCommand(FilterRoutes);
            DeleteRoute.ThrownExceptions.Subscribe(
                e => UserError.Throw("Cannot delete the selected route. Please contact the system administrator.", e));

            #endregion

            // TODO: add, edit

            #region Clearing enum choice

            ClearRouteTypeChoice = ReactiveCommand.Create();
            ClearRouteTypeChoice.Subscribe(_ => RouteFilter.RouteTypeFilter = null);

            #endregion

            #region Updating the list of agencies upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => (HostScreen.Router.NavigationStack.Last() == this) && RouteFilter.IsValid)
                .InvokeCommand(FilterRoutes);

            #endregion
        }

        /// <summary>
        ///     The <see cref="Route" /> currently selected by the user.
        /// </summary>
        public Route SelectedRoute
        {
            get { return _selectedRoute; }
            set { this.RaiseAndSetIfChanged(ref _selectedRoute, value); }
        }

        /// <summary>
        ///     <see cref="DataTransfer.RouteFilter" /> object used to send query data to the service layer.
        /// </summary>
        public RouteFilter RouteFilter
        {
            get { return _routeFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="Agency" /> objects currently displayed to the user.
        /// </summary>
        public ReactiveList<Route> Routes { get; protected set; }

        /// <summary>
        ///     The list of <see cref="RouteType" /> enumeration values.
        /// </summary>
        public ReactiveList<RouteType> RouteTypes { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="Route" /> objects from the database, using the <see cref="RouteFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<List<Route>> FilterRoutes { get; protected set; }

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