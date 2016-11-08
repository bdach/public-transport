using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Browse
{
    public class TimetableViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     <see cref="StopService" /> used to fetch data from database.
        /// </summary>
        private readonly StopService _stopService;

        /// <summary>
        ///     Service used to fetch <see cref="StopTime" /> data.
        /// </summary>
        private readonly StopTimeService _stopTimeService;

        /// <summary>
        ///     Service used to fetch <see cref="Trip" /> data.
        /// </summary>
        private readonly TripService _tripService;

        /// <summary>
        ///     The <see cref="Domain.Entities.Route" /> whose timetable is displayed on the view.
        /// </summary>
        private Route _route;

        /// <summary>
        ///     <see cref="Stop" /> object currently selected in the view.
        /// </summary>
        private Stop _selectedStop;

        /// <summary>
        ///     <see cref="StopTime" /> currently selected by the user.
        /// </summary>
        private StopTime _selectedStopTime;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display view model on.</param>
        /// <param name="route">The <see cref="Domain.Entities.Route" /> whose timetable is displayed on the view.</param>
        public TimetableViewModel(IScreen screen, Route route)
        {
            #region Field/property initialization

            HostScreen = screen;
            _stopService = new StopService();
            _stopTimeService = new StopTimeService();
            _tripService = new TripService();
            Route = route;
            Stops = new ReactiveList<Stop>();
            StopTimes = new ReactiveList<StopTime>();

            #endregion

            var stopTimeSelected = this.WhenAnyValue(vm => vm.SelectedStopTime).Select(c => c != null);

            #region Getting stops

            GetStops =
                ReactiveCommand.CreateAsyncTask(
                    async _ => await Task.Run(() => _stopService.GetStopsByRouteId(Route.Id)));
            GetStops.Subscribe(results =>
            {
                Stops.Clear();
                Stops.AddRange(results);
            });

            #endregion

            #region Updating stop times

            UpdateStopTimes =
                ReactiveCommand.CreateAsyncTask(
                    async _ =>
                            await Task.Run(() => _stopTimeService.GetRouteTimetableByStopId(SelectedStop.Id, Route.Id)));
            UpdateStopTimes.Subscribe(results =>
            {
                StopTimes.Clear();
                StopTimes.AddRange(results);
            });

            #endregion

            #region Delete trip command

            // TODO: Maybe prompt for confirmation?
            DeleteTrip = ReactiveCommand.CreateAsyncTask(stopTimeSelected, async _ =>
            {
                // should cascade delete all stop times
                await Task.Run(() => _tripService.Delete(SelectedStopTime.Trip));
                return Unit.Default;
            });
            DeleteTrip.Subscribe(_ => SelectedStopTime = null);
            DeleteTrip.InvokeCommand(UpdateStopTimes);
            DeleteTrip.ThrownExceptions.Subscribe(
                e => UserError.Throw("Cannot delete the selected trip. Please contact the system administrator.", e));

            #endregion

            #region Add/edit commands

            AddTrip =
                ReactiveCommand.CreateAsyncObservable(
                    _ => HostScreen.Router.Navigate.ExecuteAsync(new EditTripViewModel(screen)));
            EditTrip = ReactiveCommand.CreateAsyncObservable(stopTimeSelected,
                _ => HostScreen.Router.Navigate.ExecuteAsync(new EditTripViewModel(screen, SelectedStopTime.Trip)));

            #endregion

            #region Updating the list of stop times upon navigating back to this view model

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this)
                .InvokeCommand(UpdateStopTimes);

            #endregion

            this.WhenAnyValue(vm => vm.SelectedStop)
                .Where(c => c != null)
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