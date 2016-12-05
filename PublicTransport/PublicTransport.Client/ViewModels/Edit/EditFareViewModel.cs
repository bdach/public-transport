using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Fares;
using PublicTransport.Domain.Entities;
using PublicTransport.Domain.Enums;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing <see cref="Domain.Entities.FareRule" /> and <see cref="Domain.Entities.FareAttribute"/> objects.
    /// </summary>
    public class EditFareViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IFareService _fareService;

        /// <summary>
        ///     The <see cref="FareAttributeDto" /> object being edited in the window.
        /// </summary>
        private FareAttributeDto _fareAttribute;

        /// <summary>
        ///     The <see cref="FareRuleDto" /> object being edited in the window.
        /// </summary>
        private FareRuleDto _fareRule;

        /// <summary>
        ///     The <see cref="RouteDto" /> currently selected by the user.
        /// </summary>
        private RouteDto _selectedRoute;

        /// <summary>
        ///     The origin <see cref="ZoneDto" /> currently selected by the user.
        /// </summary>
        private ZoneDto _selectedOriginZone;

        /// <summary>
        ///     The destination <see cref="ZoneDto" /> currently selected by the user.
        /// </summary>
        private ZoneDto _selectedDestinationZone;

        /// <summary>
        ///     Filter used to make queries about <see cref="Route" /> objects.
        /// </summary>
        private RouteReactiveFilter _routeReactiveFilter;

        /// <summary>
        ///     Filter used to make queries about <see cref="Zone" /> objects.
        /// </summary>
        private string _originZoneFilter;

        /// <summary>
        ///     Filter used to make queries about <see cref="Zone" /> objects.
        /// </summary>
        private string _destinationZoneFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="fareService">Service exposing methods necessary to manage data.</param>
        /// <param name="fareAttribute">Fare to be edited. If a fare is to be added, this parameter should be left null.</param>
        public EditFareViewModel(IScreen screen, IFareService fareService, FareAttributeDto fareAttribute = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _fareService = fareService;
            _routeReactiveFilter = new RouteReactiveFilter();
            RouteSuggestions = new ReactiveList<RouteDto>();
            OriginZoneSuggestions = new ReactiveList<ZoneDto>();
            DestinationZoneSuggestions = new ReactiveList<ZoneDto>();
            TransferCounts = new ReactiveList<TransferCount>(Enum.GetValues(typeof(TransferCount)).Cast<TransferCount>());

            var fareServiceMethod = fareAttribute == null ? new Func<FareAttributeDto, Task<FareAttributeDto>>(_fareService.CreateFareAttributeAsync) : _fareService.UpdateFareAttributeAsync;
            var ruleServiceMethod = fareAttribute == null ? new Func<FareRuleDto, Task<FareRuleDto>>(_fareService.CreateFareRuleAsync) : _fareService.UpdateFareRuleAsync;
            _fareAttribute = fareAttribute ?? new FareAttributeDto();
            _fareRule = _fareAttribute.FareRule ?? new FareRuleDto();
            _selectedRoute = _fareRule.Route;
            _selectedOriginZone = _fareRule.Origin;
            _selectedDestinationZone = _fareRule.Destination;

            _routeReactiveFilter.ShortNameFilter = _fareRule.Route?.ShortName ?? "";
            _originZoneFilter = _fareRule.Origin?.Name ?? "";
            _destinationZoneFilter = _fareRule.Destination?.Name ?? "";

            #endregion

            this.WhenAnyValue(vm => vm.SelectedRoute).Select(r => r != null)
                .Where(b => b).Subscribe(_ => FareRule.Route = SelectedRoute);
            this.WhenAnyValue(vm => vm.SelectedOriginZone).Select(o => o != null)
                .Where(b => b).Subscribe(_ => FareRule.Origin = SelectedOriginZone);
            this.WhenAnyValue(vm => vm.SelectedDestinationZone).Select(d => d != null)
                .Where(b => b).Subscribe(_ => FareRule.Destination = SelectedDestinationZone);

            var allSelected = this.WhenAnyValue(vm => vm.SelectedRoute, vm => vm.SelectedOriginZone,
                vm => vm.SelectedDestinationZone, (r, o, d) => r != null && o != null && d != null);

            #region SaveFare command

            SaveFare = ReactiveCommand.CreateAsyncTask(allSelected, async _ =>
            {
                var result = await ruleServiceMethod(FareRule);
                FareAttribute.FareRule = result;
                return await fareServiceMethod(FareAttribute);
            });
            SaveFare.ThrownExceptions
                .Where(ex => !(ex is FaultException<ValidationFault>))
                .Subscribe(ex =>
                    UserError.Throw("Cannot connect to the server. Please try again later.", ex));
            SaveFare.ThrownExceptions
                .Where(ex => ex is FaultException<ValidationFault>)
                .Select(ex => ex as FaultException<ValidationFault>)
                .Subscribe(ex => UserError.Throw(string.Join("\n", ex.Detail.Errors), ex));

            #endregion

            #region UpdateSuggestions commands

            UpdateRouteSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await _fareService.FilterRoutesAsync(RouteReactiveFilter.Convert()));
            UpdateRouteSuggestions.Subscribe(results =>
            {
                RouteSuggestions.Clear();
                RouteSuggestions.AddRange(results);
            });
            UpdateRouteSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateOriginZoneSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await _fareService.FilterZonesAsync(OriginZoneFilter));
            UpdateOriginZoneSuggestions.Subscribe(results =>
            {
                OriginZoneSuggestions.Clear();
                OriginZoneSuggestions.AddRange(results);
            });
            UpdateOriginZoneSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateDestinationZoneSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await _fareService.FilterZonesAsync(DestinationZoneFilter));
            UpdateDestinationZoneSuggestions.Subscribe(results =>
            {
                DestinationZoneSuggestions.Clear();
                DestinationZoneSuggestions.AddRange(results);
            });
            UpdateDestinationZoneSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.RouteReactiveFilter.ShortNameFilter)
                .Where(s => (s != SelectedRoute?.ShortName) && RouteReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateRouteSuggestions);

            this.WhenAnyValue(vm => vm.OriginZoneFilter)
                .Where(z => (z != SelectedOriginZone?.Name) && !string.IsNullOrEmpty(OriginZoneFilter))
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateOriginZoneSuggestions);

            this.WhenAnyValue(vm => vm.DestinationZoneFilter)
                .Where(ps => (ps != SelectedDestinationZone?.Name) && !string.IsNullOrEmpty(DestinationZoneFilter))
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateDestinationZoneSuggestions);

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     The list of <see cref="TransferCount" /> enumeration values.
        /// </summary>
        public ReactiveList<TransferCount> TransferCounts { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="RouteDto" />s based on user input.
        /// </summary>
        public ReactiveList<RouteDto> RouteSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="ZoneDto" />s based on user input.
        /// </summary>
        public ReactiveList<ZoneDto> OriginZoneSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="ZoneDto" />s based on user input.
        /// </summary>
        public ReactiveList<ZoneDto> DestinationZoneSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the route suggestions.
        /// </summary>
        public ReactiveCommand<RouteDto[]> UpdateRouteSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the origin zone suggestions.
        /// </summary>
        public ReactiveCommand<ZoneDto[]> UpdateOriginZoneSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the destination zone suggestions.
        /// </summary>
        public ReactiveCommand<ZoneDto[]> UpdateDestinationZoneSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="FareRuleDto"/> and <see cref="FareAttributeDto"/> objects.
        /// </summary>
        public ReactiveCommand<FareAttributeDto> SaveFare { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The <see cref="FareAttributeDto" /> object being edited in the window.
        /// </summary>
        public FareAttributeDto FareAttribute
        {
            get { return _fareAttribute; }
            set { this.RaiseAndSetIfChanged(ref _fareAttribute, value); }
        }

        /// <summary>
        ///     The <see cref="FareRuleDto" /> object being edited in the window.
        /// </summary>
        public FareRuleDto FareRule
        {
            get { return _fareRule; }
            set { this.RaiseAndSetIfChanged(ref _fareRule, value); }
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
        ///     The origin <see cref="ZoneDto" /> currently selected by the user.
        /// </summary>
        public ZoneDto SelectedOriginZone
        {
            get { return _selectedOriginZone; }
            set { this.RaiseAndSetIfChanged(ref _selectedOriginZone, value); }
        }

        /// <summary>
        ///     The destination <see cref="ZoneDto" /> currently selected by the user.
        /// </summary>
        public ZoneDto SelectedDestinationZone
        {
            get { return _selectedDestinationZone; }
            set { this.RaiseAndSetIfChanged(ref _selectedDestinationZone, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Route" /> objects.
        /// </summary>
        public RouteReactiveFilter RouteReactiveFilter
        {
            get { return _routeReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeReactiveFilter, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Zone" /> objects.
        /// </summary>
        public string OriginZoneFilter
        {
            get { return _originZoneFilter; }
            set { this.RaiseAndSetIfChanged(ref _originZoneFilter, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Zone" /> objects.
        /// </summary>
        public string DestinationZoneFilter
        {
            get { return _destinationZoneFilter; }
            set { this.RaiseAndSetIfChanged(ref _destinationZoneFilter, value); }
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
        public MenuOption AssociatedMenuOption => MenuOption.Fare;
    }
}
