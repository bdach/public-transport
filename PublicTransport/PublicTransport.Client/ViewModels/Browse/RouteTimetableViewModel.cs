using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Routes;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Browse
{
    public class RouteTimetableViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IRouteService _routeService;

        /// <summary>
        ///     The <see cref="RouteDto" /> whose timetable is displayed in the view.
        /// </summary>
        private RouteDto _route;

        /// <summary>
        ///     <see cref="StopDto" /> object currently selected in the view.
        /// </summary>
        private StopDto _selectedStop;

        /// <summary>
        ///     <see cref="StopTimeDto" /> object currently selected by the user.
        /// </summary>
        private StopTimeDto _selectedStopTime;

        /// <summary>
        ///     Object used for querying the database for stop times.
        /// </summary>
        private StopTimeReactiveFilter _stopTimeReactiveFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display view model on.</param>
        /// <param name="routeService">Service used in the view model to access the database.</param>
        /// <param name="route">The <see cref="Domain.Entities.Route" /> whose timetable is displayed on the view.</param>
        public RouteTimetableViewModel(IScreen screen, IRouteService routeService, RouteDto route)
        {
            #region Field/property initialization

            HostScreen = screen;
            Route = route;
            Stops = new ReactiveList<StopDto>();
            StopTimes = new ReactiveList<StopTimeDto>();
            StopTimeReactiveFilter = new StopTimeReactiveFilter { RouteId = route.Id };
            _routeService = routeService;

            #endregion

            var stopTimeSelected = this.WhenAnyValue(vm => vm.SelectedStopTime).Select(st => st != null);

            #region Getting stops

            GetStops = ReactiveCommand.CreateAsyncTask(async _ => await _routeService.GetStopsByRouteIdAsync(Route.Id));
            GetStops.Subscribe(results =>
            {
                Stops.Clear();
                Stops.AddRange(results);
            });
            GetStops.ThrownExceptions.Subscribe(e =>
            {
                HostScreen.Router.NavigateBack.ExecuteAsync();
                UserError.Throw("Cannot fetch stops. Please contact the system administrator.", e);
            });

            #endregion

            #region Updating stop times

            UpdateStopTimes = ReactiveCommand.CreateAsyncTask(async _ => await _routeService.GetRouteTimetableByStopIdAsync(StopTimeReactiveFilter.Convert()));
            UpdateStopTimes.Subscribe(results =>
            {
                StopTimes.Clear();
                StopTimes.AddRange(results);
            });
            UpdateStopTimes.ThrownExceptions.Subscribe(e => UserError.Throw("Cannot fetch timetable. Please contact the system administrator.", e));

            #endregion

            #region Delete trip command

            DeleteTrip = ReactiveCommand.CreateAsyncTask(stopTimeSelected, async _ =>
            {
                // should cascade delete all stop times
                await _routeService.DeleteTripAsync(SelectedStopTime.Trip);
                return Unit.Default;
            });
            DeleteTrip.Subscribe(_ => SelectedStopTime = null);
            DeleteTrip.InvokeCommand(GetStops);
            GetStops.InvokeCommand(UpdateStopTimes);
            DeleteTrip.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected trip. Please contact the system administrator.", e));

            #endregion

            #region Add/edit trip commands

            AddTrip = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditTripViewModel(screen, _routeService, Route, Stops)));
            EditTrip = ReactiveCommand.CreateAsyncObservable(stopTimeSelected, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditTripViewModel(screen, _routeService, SelectedStopTime.Trip)));

            #endregion

            #region Updating the list of stop times upon navigating back to this view model

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this)
                .InvokeCommand(UpdateStopTimes);

            #endregion

            this.WhenAnyValue(vm => vm.SelectedStop, vm => vm.StopTimeReactiveFilter.Date, vm => vm.StopTimeReactiveFilter.Time)
                .Select(s => s.Item1?.Id)
                .Where(id => id.HasValue)
                .Select(s => StopTimeReactiveFilter.StopId = s.Value)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(UpdateStopTimes);

            #region Closing the view

            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     Reactive list containing the filtered <see cref="StopDto" /> objects.
        /// </summary>
        public ReactiveList<StopDto> Stops { get; protected set; }

        /// <summary>
        ///     Reactive list containing the filtered <see cref="StopTimeDto" /> objects.
        /// </summary>
        public ReactiveList<StopTimeDto> StopTimes { get; protected set; }

        /// <summary>
        ///     Command responsible for fetching the list of stops.
        /// </summary>
        public ReactiveCommand<StopDto[]> GetStops { get; protected set; }

        /// <summary>
        ///     Command responsible for updating stop times.
        /// </summary>
        public ReactiveCommand<StopTimeDto[]> UpdateStopTimes { get; protected set; }

        /// <summary>
        ///     Command responsible for launching the trip adding view.
        /// </summary>
        public ReactiveCommand<object> AddTrip { get; protected set; }

        /// <summary>
        ///     Command responsible for launching the trip editing view.
        /// </summary>
        public ReactiveCommand<object> EditTrip { get; protected set; }

        /// <summary>
        ///     Command responsible for deleting the trip.
        /// </summary>
        public ReactiveCommand<Unit> DeleteTrip { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the view.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; set; }

        /// <summary>
        ///     The <see cref="Domain.Entities.Route"/> whose timetable is displayed in the view.
        /// </summary>
        public RouteDto Route
        {
            get { return _route; }
            set { this.RaiseAndSetIfChanged(ref _route, value); }
        }

        /// <summary>
        ///     Property exposing the currently selected <see cref="StopDto" />.
        /// </summary>
        public StopDto SelectedStop
        {
            get { return _selectedStop; }
            set { this.RaiseAndSetIfChanged(ref _selectedStop, value); }
        }

        /// <summary>
        ///     Property exposing the currently selected <see cref="StopTimeDto" />.
        /// </summary>
        public StopTimeDto SelectedStopTime
        {
            get { return _selectedStopTime; }
            set { this.RaiseAndSetIfChanged(ref _selectedStopTime, value); }
        }

        /// <summary>
        ///     Object user for querying database for stop times.
        /// </summary>
        public StopTimeReactiveFilter StopTimeReactiveFilter
        {
            get { return _stopTimeReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _stopTimeReactiveFilter, value); }
        }

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