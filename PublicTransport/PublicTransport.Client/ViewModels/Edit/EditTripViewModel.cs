﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Routes;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model responsible for editing trips.
    /// </summary>
    public class EditTripViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IRouteService _routeService;

        /// <summary>
        ///     Used for filtering <see cref="Route" /> objects.
        /// </summary>
        private RouteReactiveFilter _routeReactiveFilter;

        /// <summary>
        ///     Route currently selected by the user.
        /// </summary>
        private RouteDto _selectedRoute;

        /// <summary>
        ///     <see cref="StopTime" /> currently selected by the user.
        /// </summary>
        private EditStopTimeViewModel _selectedStopTime;

        /// <summary>
        ///     Currently edited <see cref="TripDto" />.
        /// </summary>
        private TripDto _trip;

        /// <summary>
        ///     Stores service schedule data for the currently edited trip.
        /// </summary>
        private CalendarDto _serviceCalendar;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display to.</param>
        /// <param name="routeService">Service used in the view model to access the database.</param>
        private EditTripViewModel(IScreen screen, IRouteService routeService)
        {
            HostScreen = screen;
            RouteSuggestions = new ReactiveList<RouteDto>();
            _routeService = routeService;
            _routeReactiveFilter = new RouteReactiveFilter();
            StopTimes = new ReactiveList<EditStopTimeViewModel>();
        }

        /// <summary>
        ///     Constructor. Used when a trip is being edited.
        /// </summary>
        /// <param name="screen">Screen to display on.</param>
        /// <param name="routeService">Service used in the view model to access the database.</param>
        /// <param name="trip">Trip to edit.</param>
        public EditTripViewModel(IScreen screen, IRouteService routeService, TripDto trip) : this(screen, routeService)
        {
            Trip = trip;
            var toAdd = _routeService.GetTripStops(trip);
            StopTimes.AddRange(toAdd.Select(s => new EditStopTimeViewModel(_routeService, s)));
            SetUp(false);
        }

        /// <summary>
        ///     Constructor. Used for adding a trip of an existing route.
        /// </summary>
        /// <param name="screen">Screen to display on.</param>
        /// <param name="routeService">Service used in the view model to access the database.</param>
        /// <param name="route">Route to add to.</param>
        /// <param name="stops">List of stops to initialize the stop list with.</param>
        public EditTripViewModel(IScreen screen, IRouteService routeService, RouteDto route, IEnumerable<StopDto> stops) : this(screen, routeService)
        {
            Trip = new TripDto
            {
                Route = route
            };
            var toAdd = stops.Select(s => new StopTimeDto
            {
                Trip = Trip,
                Stop = s
            }).ToList();
            StopTimes.AddRange(toAdd.Select(s => new EditStopTimeViewModel(_routeService, s)));
            SetUp(true);
        }

        /// <summary>
        ///     List of <see cref="RouteDto" /> suggestions.
        /// </summary>
        public ReactiveList<RouteDto> RouteSuggestions { get; protected set; }

        /// <summary>
        ///     List of stop times.
        /// </summary>
        public ReactiveList<EditStopTimeViewModel> StopTimes { get; protected set; }

        /// <summary>
        ///     Command responsible for updating <see cref="RouteDto" /> suggestions.
        /// </summary>
        public ReactiveCommand<RouteDto[]> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="TripDto" />.
        /// </summary>
        public ReactiveCommand<TripDto> SaveTrip { get; protected set; }

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
        ///     Currently edited <see cref="TripDto" />.
        /// </summary>
        public TripDto Trip
        {
            get { return _trip; }
            set { this.RaiseAndSetIfChanged(ref _trip, value); }
        }

        /// <summary>
        ///     Currently selected <see cref="RouteDto" />.
        /// </summary>
        public RouteDto SelectedRoute
        {
            get { return _selectedRoute; }
            set { this.RaiseAndSetIfChanged(ref _selectedRoute, value); }
        }

        /// <summary>
        ///     Used for filtering routes.
        /// </summary>
        public RouteReactiveFilter RouteReactiveFilter
        {
            get { return _routeReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeReactiveFilter, value); }
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
        ///     Stores service schedule data for the currently edited trip.
        /// </summary>
        public CalendarDto ServiceCalendar
        {
            get { return _serviceCalendar; }
            set { this.RaiseAndSetIfChanged(ref _serviceCalendar, value); }
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

            var calendarServiceMethod = isNew
                ? new Func<CalendarDto, Task<CalendarDto>>(_routeService.CreateCalendarAsync)
                : _routeService.UpdateCalendarAsync;
            var tripServiceMethod = isNew ? new Func<TripDto, Task<TripDto>>(_routeService.CreateTripAsync) : _routeService.UpdateTripAsync;
            SelectedRoute = _trip.Route;
            RouteSuggestions.Add(SelectedRoute);
            _routeReactiveFilter.ShortNameFilter = _trip.ShortName ?? "";
            ServiceCalendar = _trip.Service;

            #endregion

            var routeSelected = this.WhenAnyValue(vm => vm.SelectedRoute).Select(r => r != null);
            routeSelected.Where(b => b).Subscribe(_ => Trip.Route = SelectedRoute);

            var canSave = this.WhenAnyValue(vm => vm.SelectedRoute, vm => vm.ServiceCalendar)
                .Select(t => t.Item1 != null && t.Item2 != null);

            #region SaveTrip command

            SaveTrip = ReactiveCommand.CreateAsyncTask(canSave, async _ =>
            {
                Trip.Service = ServiceCalendar = await calendarServiceMethod(ServiceCalendar);
                var result = await tripServiceMethod(Trip);
                Trip = result;
                var stopTimes = StopTimes.Select(vm => vm.StopTime).ToList();
                await _routeService.UpdateStopsAsync(Trip.Id, stopTimes.ToArray());
                return result;
            });
            // On exceptions: Display error.
            SaveTrip.ThrownExceptions
                .Where(ex => !(ex is FaultException<ValidationFault>))
                .Subscribe(ex =>
                    UserError.Throw("Cannot connect to the server. Please try again later.", ex));
            SaveTrip.ThrownExceptions
                .Where(ex => ex is FaultException<ValidationFault>)
                .Select(ex => ex as FaultException<ValidationFault>)
                .Subscribe(ex => UserError.Throw(string.Join("\n", ex.Detail.Errors), ex));

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await _routeService.FilterRoutesAsync(RouteReactiveFilter.Convert()));
            UpdateSuggestions.Subscribe(results =>
            {
                RouteSuggestions.Clear();
                RouteSuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.RouteReactiveFilter.ShortNameFilter)
                .Where(s => (s != SelectedRoute?.ShortName) && RouteReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            #endregion

            #region Navigating to calendar view

            NavigateToCalendar = ReactiveCommand.Create();
            NavigateToCalendar.Subscribe(_ =>
            {
                ServiceCalendar = ServiceCalendar ?? new CalendarDto();
                HostScreen.Router.Navigate.Execute(new EditCalendarViewModel(HostScreen, ServiceCalendar));
            });

            #endregion

            #region Adding a new stop

            AddStop = ReactiveCommand.Create();
            AddStop.Subscribe(_ => StopTimes.Add(new EditStopTimeViewModel(_routeService)));

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