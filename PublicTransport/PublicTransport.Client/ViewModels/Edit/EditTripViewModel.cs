using System;
using System.Collections.Generic;
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
    public class EditTripViewModel : ReactiveObject, IDetailViewModel
    {
        private readonly RouteService _routeService;
        private Trip _trip;
        private Route _selectedRoute;
        private RouteFilter _routeFilter;
        private readonly EditCalendarViewModel _calendarViewModel;

        public EditTripViewModel(IScreen screen, Trip trip = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            RouteSuggestions = new ReactiveList<Route>();
            _routeService = new RouteService();
            _routeFilter = new RouteFilter();
            var tripService = new TripService();
            var tripServiceMethod = trip == null ? new Func<Trip, Trip>(tripService.Create) : tripService.Update;
            var calendarService = new CalendarService();
            var calendarServiceMethod = trip == null
                ? new Func<Calendar, Calendar>(calendarService.Create)
                : calendarService.Update;
            _trip = trip ?? new Trip();
            _trip.Service = _trip.Service ?? new Calendar();
            _calendarViewModel = new EditCalendarViewModel(screen, _trip.Service);
            _selectedRoute = _trip?.Route;
            _routeFilter.ShortNameFilter = _trip?.ShortName ?? "";

            #endregion

            var agencySelected = this.WhenAnyValue(vm => vm.SelectedRoute).Select(s => s != null);
            
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
                Trip.Route = SelectedRoute;
                Trip.Service = schedule;
                return result;
            });
            // On exceptions: Display error.
            SaveTrip.ThrownExceptions.Subscribe(
                ex =>
                    UserError.Throw(
                        "The currently edited trip cannot be saved to the database. Please contact the system administrator.",
                        ex));

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions =
                ReactiveCommand.CreateAsyncTask(
                    async _ => await Task.Run(() => _routeService.FilterRoutes(RouteFilter)));
            UpdateSuggestions.Subscribe(results =>
            {
                RouteSuggestions.Clear();
                RouteSuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions
                .Subscribe(
                    ex =>
                        UserError.Throw(
                            "Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.RouteFilter.ShortNameFilter)
                .Where(s => (s != SelectedRoute?.ShortName) && RouteFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            #endregion

            #region Navigating to calendar view

            NavigateToCalendar =
                ReactiveCommand.CreateAsyncObservable(
                    _ => HostScreen.Router.Navigate.ExecuteAsync(_calendarViewModel));

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        public ReactiveList<Route> RouteSuggestions { get; protected set; }
        public ReactiveCommand<List<Route>> UpdateSuggestions { get; protected set; }
        public ReactiveCommand<Trip> SaveTrip { get; protected set; }
        public ReactiveCommand<Unit> Close { get; protected set; }
        public ReactiveCommand<object> NavigateToCalendar { get; protected set; }

        public Trip Trip
        {
            get { return _trip; }
            set { this.RaiseAndSetIfChanged(ref _trip, value); }
        }

        public Route SelectedRoute
        {
            get { return _selectedRoute; }
            set { this.RaiseAndSetIfChanged(ref _selectedRoute, value); }
        }

        public RouteFilter RouteFilter
        {
            get { return _routeFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeFilter, value); }
        }

        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public MenuOption AssociatedMenuOption => MenuOption.Trip;
    }
}