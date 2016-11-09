﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model responsible for editing trips.
    /// </summary>
    public class EditTripViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used to fetch <see cref="Route" /> data from the database.
        /// </summary>
        private readonly RouteService _routeService;

        /// <summary>
        ///     Service used to fetch <see cref="Stop" /> data from the database.
        /// </summary>
        private readonly StopService _stopService;

        /// <summary>
        ///     Service used to fetch and saving <see cref="Domain.Entities.Trip" /> data from the database.
        /// </summary>
        private readonly TripService _tripService;

        /// <summary>
        ///     Used for filtering <see cref="Route" /> objects.
        /// </summary>
        private RouteFilter _routeFilter;

        /// <summary>
        ///     Route currently selected by the user.
        /// </summary>
        private Route _selectedRoute;

        /// <summary>
        ///     <see cref="StopTime" /> currently selected by the user.
        /// </summary>
        private EditStopTimeViewModel _selectedStopTime;

        /// <summary>
        ///     Currently edited <see cref="Domain.Entities.Trip" />.
        /// </summary>
        private Trip _trip;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display to.</param>
        public EditTripViewModel(IScreen screen)
        {
            HostScreen = screen;
            RouteSuggestions = new ReactiveList<Route>();
            _tripService = new TripService();
            _routeService = new RouteService();
            _routeFilter = new RouteFilter();
            _stopService = new StopService();
            StopTimes = new ReactiveList<EditStopTimeViewModel>();
        }

        /// <summary>
        ///     Constructor. Used when a trip is being edited.
        /// </summary>
        /// <param name="screen">Screen to display on.</param>
        /// <param name="trip">Trip to edit.</param>
        public EditTripViewModel(IScreen screen, Trip trip) : this(screen)
        {
            Trip = trip;
            var toAdd = _tripService.GetTripStops(trip);
            StopTimes.AddRange(toAdd.Select(s => new EditStopTimeViewModel(_stopService, s)));
            SetUp(false);
        }

        /// <summary>
        ///     Constructor. Used for adding a trip of an existing route.
        /// </summary>
        /// <param name="screen">Screen to display on.</param>
        /// <param name="route">Route to add to.</param>
        /// <param name="stops">List of stops to initialize the stop list with.</param>
        public EditTripViewModel(IScreen screen, Route route, IEnumerable<Stop> stops) : this(screen)
        {
            Trip = new Trip
            {
                Route = route,
                RouteId = route.Id
            };
            var toAdd = stops.Select(s => new StopTime
            {
                TripId = Trip.Id,
                StopId = s.Id,
                Stop = s
            }).ToList();
            StopTimes.AddRange(toAdd.Select(s => new EditStopTimeViewModel(_stopService, s)));
            SetUp(true);
        }

        /// <summary>
        ///     List of <see cref="Route" /> suggestions.
        /// </summary>
        public ReactiveList<Route> RouteSuggestions { get; protected set; }

        /// <summary>
        ///     List of stop times.
        /// </summary>
        public ReactiveList<EditStopTimeViewModel> StopTimes { get; protected set; }

        /// <summary>
        ///     Command responsible for updating <see cref="Route" /> suggestions.
        /// </summary>
        public ReactiveCommand<List<Route>> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="Trip" />.
        /// </summary>
        public ReactiveCommand<Trip> SaveTrip { get; protected set; }

        /// <summary>
        ///     Closes the view.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     Adds a stop to the list.
        /// </summary>
        public ReactiveCommand<object> AddStop { get; protected set; }

        /// <summary>
        ///     Removes a stop from the stop list.
        /// </summary>
        public ReactiveCommand<object> DeleteStop { get; protected set; }

        /// <summary>
        ///     Navigates to the view allowing the user to edit the trip schedule.
        /// </summary>
        public ReactiveCommand<object> NavigateToCalendar { get; protected set; }

        /// <summary>
        ///     Fetches the stops associated with the provided trip.
        /// </summary>
        public ReactiveCommand<List<StopTime>> FetchStops { get; protected set; }

        /// <summary>
        ///     Currently edited <see cref="Domain.Entities.Trip" />.
        /// </summary>
        public Trip Trip
        {
            get { return _trip; }
            set { this.RaiseAndSetIfChanged(ref _trip, value); }
        }

        /// <summary>
        ///     Currently selected <see cref="Domain.Entities.Route" />.
        /// </summary>
        public Route SelectedRoute
        {
            get { return _selectedRoute; }
            set { this.RaiseAndSetIfChanged(ref _selectedRoute, value); }
        }

        /// <summary>
        ///     Used for filtering routes.
        /// </summary>
        public RouteFilter RouteFilter
        {
            get { return _routeFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeFilter, value); }
        }

        /// <summary>
        ///     Currently selected <see cref="StopTime" />.
        /// </summary>
        public EditStopTimeViewModel SelectedStopTime
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
        public MenuOption AssociatedMenuOption => MenuOption.Trip;

        /// <summary>
        ///     Sets up commands and observables necessary for the view model's functioning.
        /// </summary>
        /// <param name="isNew">Indicates whether the currently edited object is new or not.</param>
        private void SetUp(bool isNew)
        {
            #region Field/property initialization

            var tripServiceMethod = isNew ? new Func<Trip, Trip>(_tripService.Create) : _tripService.Update;
            var calendarService = new CalendarService();
            var calendarServiceMethod = isNew ? new Func<Calendar, Calendar>(calendarService.Create) : calendarService.Update;
            SelectedRoute = _trip.Route;
            RouteSuggestions.Add(SelectedRoute);
            _routeFilter.ShortNameFilter = _trip.ShortName ?? "";

            #endregion

            var agencySelected = this.WhenAnyValue(vm => vm.SelectedRoute).Select(r => r != null);

            #region SaveTrip command

            SaveTrip = ReactiveCommand.CreateAsyncTask(agencySelected, async _ =>
            {
                // TODO: Fix this when refactoring services.
                var schedule = await Task.Run(() => calendarServiceMethod(Trip.Service));
                Trip.Route = null;
                Trip.Service = null;
                Trip.RouteId = SelectedRoute.Id;
                Trip.ServiceId = schedule.Id;
                var result = await Task.Run(() => tripServiceMethod(Trip));
                var stopTimes = StopTimes.Select(vm => vm.StopTime).ToList();
                await Task.Run(() => _tripService.UpdateStops(Trip.Id, stopTimes));
                Trip.Route = SelectedRoute;
                Trip.Service = schedule;
                return result;
            });
            // On exceptions: Display error.
            SaveTrip.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("The currently edited trip cannot be saved to the database. Please contact the system administrator.", ex));

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _routeService.FilterRoutes(RouteFilter)));
            UpdateSuggestions.Subscribe(results =>
            {
                RouteSuggestions.Clear();
                RouteSuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.RouteFilter.ShortNameFilter)
                .Where(s => (s != SelectedRoute?.ShortName) && RouteFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            #endregion

            #region Navigating to calendar view

            NavigateToCalendar = ReactiveCommand.Create();
            NavigateToCalendar.Subscribe(_ =>
            {
                Trip.Service = Trip.Service ?? new Calendar();
                HostScreen.Router.Navigate.Execute(new EditCalendarViewModel(HostScreen, Trip.Service));
            });

            #endregion

            #region Adding a new stop

            AddStop = ReactiveCommand.Create();
            AddStop.Subscribe(_ => StopTimes.Add(new EditStopTimeViewModel(_stopService)));

            #endregion

            #region Deleting a stop

            DeleteStop = ReactiveCommand.Create();
            DeleteStop.Subscribe(_ => StopTimes.Remove(SelectedStopTime));

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }
    }
}