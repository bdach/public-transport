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

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing <see cref="Domain.Entities.Route" /> objects.
    /// </summary>
    public class EditRouteViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     <see cref="AgencyService" /> used for filtering agencies.
        /// </summary>
        private readonly AgencyService _agencyService;

        /// <summary>
        ///     Filter used to make queries about <see cref="Agency" /> objects.
        /// </summary>
        private AgencyFilter _agencyFilter;

        /// <summary>
        ///     The <see cref="Domain.Entities.Route" /> object being edited in the window.
        /// </summary>
        private Route _route;

        /// <summary>
        ///     The <see cref="Agency" /> currently selected by the user.
        /// </summary>
        private Agency _selectedAgency;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="route">Route to be edited. If a route is to be added, this parameter should be left null.</param>
        public EditRouteViewModel(IScreen screen, Route route = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            AgencySuggestions = new ReactiveList<Agency>();
            RouteTypes = new ReactiveList<RouteType>(Enum.GetValues(typeof(RouteType)).Cast<RouteType>());
            _agencyService = new AgencyService();
            _agencyFilter = new AgencyFilter();
            var routeService = new RouteService();
            var serviceMethod = route == null ? new Func<Route, Route>(routeService.Create) : routeService.Update;
            _route = route ?? new Route();
            _selectedAgency = _route?.Agency;
            _agencyFilter.AgencyNameFilter = _route?.Agency?.Name ?? "";

            #endregion

            var agencySelected = this.WhenAnyValue(vm => vm.SelectedAgency).Select(s => s != null);

            #region DisplayAgencyView command

            DisplayAgencyView = ReactiveCommand.CreateAsyncObservable(agencySelected,
                _ => HostScreen.Router.Navigate.ExecuteAsync(new EditAgencyViewModel(screen, SelectedAgency)));

            #endregion

            #region SaveRoute command

            SaveRoute = ReactiveCommand.CreateAsyncTask(agencySelected, async _ =>
            {
                // TODO: Fix this when refactoring services.
                Route.Agency = null;
                Route.AgencyId = SelectedAgency.Id;
                var result = await Task.Run(() => serviceMethod(Route));
                Route.Agency = SelectedAgency;
                return result;
            });
            // On exceptions: Display error.
            SaveRoute.ThrownExceptions.Subscribe(
                ex =>
                    UserError.Throw(
                        "The currently edited route cannot be saved to the database. Please contact the system administrator.",
                        ex));

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions =
                ReactiveCommand.CreateAsyncTask(
                    async _ => await Task.Run(() => _agencyService.FilterAgencies(AgencyFilter)));
            UpdateSuggestions.Subscribe(results =>
            {
                AgencySuggestions.Clear();
                AgencySuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions
                .Subscribe(
                    ex =>
                        UserError.Throw(
                            "Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.AgencyFilter.AgencyNameFilter)
                .Where(s => (s != SelectedAgency?.Name) && AgencyFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     List containing the suggested <see cref="Agency" /> objects based on user input.
        /// </summary>
        public ReactiveList<Agency> AgencySuggestions { get; protected set; }

        /// <summary>
        ///     The list of <see cref="RouteType" /> enumeration values.
        /// </summary>
        public ReactiveList<RouteType> RouteTypes { get; protected set; }

        /// <summary>
        ///     Command allowing to edit the <see cref="SelectedAgency" /> object.
        /// </summary>
        public ReactiveCommand<object> DisplayAgencyView { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the agency suggestions.
        /// </summary>
        public ReactiveCommand<List<Agency>> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="Route" /> object.
        /// </summary>
        public ReactiveCommand<Route> SaveRoute { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The <see cref="Domain.Entities.Route" /> object being edited in the window.
        /// </summary>
        public Route Route
        {
            get { return _route; }
            set { this.RaiseAndSetIfChanged(ref _route, value); }
        }

        /// <summary>
        ///     The <see cref="Agency" /> currently selected by the user.
        /// </summary>
        public Agency SelectedAgency
        {
            get { return _selectedAgency; }
            set { this.RaiseAndSetIfChanged(ref _selectedAgency, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Agency" /> objects.
        /// </summary>
        public AgencyFilter AgencyFilter
        {
            get { return _agencyFilter; }
            set { this.RaiseAndSetIfChanged(ref _agencyFilter, value); }
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