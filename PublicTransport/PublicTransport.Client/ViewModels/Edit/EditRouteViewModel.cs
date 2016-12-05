using System;
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
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing route objects.
    /// </summary>
    public class EditRouteViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IRouteService _routeService;

        /// <summary>
        ///     Filter used to make queries about <see cref="Agency" /> objects.
        /// </summary>
        private AgencyReactiveFilter _agencyReactiveFilter;

        /// <summary>
        ///     The <see cref="RouteDto" /> object being edited in the window.
        /// </summary>
        private RouteDto _route;

        /// <summary>
        ///     The <see cref="AgencyDto" /> currently selected by the user.
        /// </summary>
        private AgencyDto _selectedAgency;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="routeService">Service exposing methods necessary to manage data.</param>
        /// <param name="route">Route to be edited. If a route is to be added, this parameter should be left null.</param>
        public EditRouteViewModel(IScreen screen, IRouteService routeService, RouteDto route = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            AgencySuggestions = new ReactiveList<AgencyDto>();
            RouteTypes = new ReactiveList<RouteType>(Enum.GetValues(typeof(RouteType)).Cast<RouteType>());
            _agencyReactiveFilter = new AgencyReactiveFilter();
            _routeService = routeService;
            var serviceMethod = route == null ? new Func<RouteDto, Task<RouteDto>>(routeService.CreateRouteAsync) : routeService.UpdateRouteAsync;
            _route = route ?? new RouteDto();
            _selectedAgency = _route.Agency;
            _agencyReactiveFilter.AgencyNameFilter = _route.Agency?.Name ?? "";

            #endregion

            var agencySelected = this.WhenAnyValue(vm => vm.SelectedAgency).Select(s => s != null);
            agencySelected.Where(b => b).Subscribe(_ => Route.Agency = SelectedAgency);

            #region SaveRoute command

            SaveRoute = ReactiveCommand.CreateAsyncTask(agencySelected, async _ => await serviceMethod(Route));
            SaveRoute.ThrownExceptions
                .Where(ex => !(ex is FaultException<ValidationFault>))
                .Subscribe(ex =>
                    UserError.Throw("Cannot connect to the server. Please try again later.", ex));
            SaveRoute.ThrownExceptions
                .Where(ex => ex is FaultException<ValidationFault>)
                .Select(ex => ex as FaultException<ValidationFault>)
                .Subscribe(ex => UserError.Throw(string.Join("\n", ex.Detail.Errors), ex));

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await Task.Run(() => _routeService.FilterAgencies(AgencyReactiveFilter.Convert())));
            UpdateSuggestions.Subscribe(results =>
            {
                AgencySuggestions.Clear();
                AgencySuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.AgencyReactiveFilter.AgencyNameFilter)
                .Where(s => (s != SelectedAgency?.Name) && AgencyReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     List containing the suggested <see cref="AgencyDto" /> objects based on user input.
        /// </summary>
        public ReactiveList<AgencyDto> AgencySuggestions { get; protected set; }

        /// <summary>
        ///     The list of <see cref="RouteType" /> enumeration values.
        /// </summary>
        public ReactiveList<RouteType> RouteTypes { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the agency suggestions.
        /// </summary>
        public ReactiveCommand<AgencyDto[]> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="Domain.Entities.Route" /> object.
        /// </summary>
        public ReactiveCommand<RouteDto> SaveRoute { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The <see cref="RouteDto" /> object being edited in the window.
        /// </summary>
        public RouteDto Route
        {
            get { return _route; }
            set { this.RaiseAndSetIfChanged(ref _route, value); }
        }

        /// <summary>
        ///     The <see cref="AgencyDto" /> currently selected by the user.
        /// </summary>
        public AgencyDto SelectedAgency
        {
            get { return _selectedAgency; }
            set { this.RaiseAndSetIfChanged(ref _selectedAgency, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Agency" /> objects.
        /// </summary>
        public AgencyReactiveFilter AgencyReactiveFilter
        {
            get { return _agencyReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _agencyReactiveFilter, value); }
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