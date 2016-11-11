using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Browse
{
    public class TimetableViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Unit of work used in the view model to access the database.
        /// </summary>
        private readonly RouteUnitOfWork _routeUnitOfWork;

        /// <summary>
        ///     The <see cref="Domain.Entities.Route" /> whose timetable is displayed in the view.
        /// </summary>
        private Route _route;

        /// <summary>
        ///     <see cref="Stop" /> object currently selected in the view.
        /// </summary>
        private Stop _selectedStop;

        /// <summary>
        ///     <see cref="StopTime" /> object currently selected by the user.
        /// </summary>
        private StopTime _selectedStopTime;

        /// <summary>
        ///     Object used for querying the database for stop times.
        /// </summary>
        private StopTimeFilter _stopTimeFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display view model on.</param>
        /// <param name="routeUnitOfWork">Unit of work used in the view model to access the database.</param>
        /// <param name="route">The <see cref="Domain.Entities.Route" /> whose timetable is displayed on the view.</param>
        public TimetableViewModel(IScreen screen, RouteUnitOfWork routeUnitOfWork, Route route)
        {
            #region Field/property initialization

            HostScreen = screen;
            Route = route;
            Stops = new ReactiveList<Stop>();
            StopTimes = new ReactiveList<StopTime>();
            StopTimeFilter = new StopTimeFilter { RouteId = route.Id };
            _routeUnitOfWork = routeUnitOfWork;

            #endregion

            var stopTimeSelected = this.WhenAnyValue(vm => vm.SelectedStopTime).Select(st => st != null);

            #region Getting stops

            GetStops = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _routeUnitOfWork.GetStopsByRouteId(Route.Id)));
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

            UpdateStopTimes = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _routeUnitOfWork.GetRouteTimetableByStopId(StopTimeFilter)));
            UpdateStopTimes.Subscribe(results =>
            {
                StopTimes.Clear();
                StopTimes.AddRange(results);
            });
            UpdateStopTimes.ThrownExceptions.Subscribe(e => UserError.Throw("Cannot fetch timetable. Please contact the system administrator.", e));

            #endregion

            #region Delete trip command

            // TODO: Maybe prompt for confirmation?
            DeleteTrip = ReactiveCommand.CreateAsyncTask(stopTimeSelected, async _ =>
            {
                // should cascade delete all stop times
                await Task.Run(() => _routeUnitOfWork.DeleteTrip(SelectedStopTime.Trip));
                return Unit.Default;
            });
            DeleteTrip.Subscribe(_ => SelectedStopTime = null);
            DeleteTrip.InvokeCommand(GetStops);
            GetStops.InvokeCommand(UpdateStopTimes);
            DeleteTrip.ThrownExceptions.Subscribe(e => UserError.Throw("Cannot delete the selected trip. Please contact the system administrator.", e));

            #endregion

            #region Add/edit trip commands

            AddTrip = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditTripViewModel(screen, _routeUnitOfWork, Route, Stops)));
            EditTrip = ReactiveCommand.CreateAsyncObservable(stopTimeSelected, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditTripViewModel(screen, _routeUnitOfWork, SelectedStopTime.Trip)));

            #endregion

            #region Updating the list of stop times upon navigating back to this view model

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this)
                .InvokeCommand(UpdateStopTimes);

            #endregion

            this.WhenAnyValue(vm => vm.SelectedStop, vm => vm.StopTimeFilter.Date, vm => vm.StopTimeFilter.Time)
                .Select(s => s.Item1?.Id)
                .Where(id => id.HasValue)
                .Select(s => StopTimeFilter.StopId = s.Value)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(UpdateStopTimes);
        }

        /// <summary>
        ///     Reactive list containing the filtered <see cref="Stop" /> objects.
        /// </summary>
        public ReactiveList<Stop> Stops { get; protected set; }

        /// <summary>
        ///     Reactive list containing the filtered <see cref="StopTime" /> objects.
        /// </summary>
        public ReactiveList<StopTime> StopTimes { get; protected set; }

        /// <summary>
        ///     Command responsible for fetching the list of stops.
        /// </summary>
        public ReactiveCommand<List<Stop>> GetStops { get; protected set; }

        /// <summary>
        ///     Command responsible for updating stop times.
        /// </summary>
        public ReactiveCommand<List<StopTime>> UpdateStopTimes { get; protected set; }

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
        ///     The <see cref="Domain.Entities.Route"/> whose timetable is displayed in the view.
        /// </summary>
        public Route Route
        {
            get { return _route; }
            set { this.RaiseAndSetIfChanged(ref _route, value); }
        }

        /// <summary>
        ///     Property exposing the currently selected <see cref="Stop" />.
        /// </summary>
        public Stop SelectedStop
        {
            get { return _selectedStop; }
            set { this.RaiseAndSetIfChanged(ref _selectedStop, value); }
        }

        /// <summary>
        ///     Property exposing the currently selected <see cref="StopTime" />.
        /// </summary>
        public StopTime SelectedStopTime
        {
            get { return _selectedStopTime; }
            set { this.RaiseAndSetIfChanged(ref _selectedStopTime, value); }
        }

        /// <summary>
        ///     Object user for querying database for stop times.
        /// </summary>
        public StopTimeFilter StopTimeFilter
        {
            get { return _stopTimeFilter; }
            set { this.RaiseAndSetIfChanged(ref _stopTimeFilter, value); }
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