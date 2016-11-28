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
    ///     View model for editing <see cref="Domain.Entities.FareRule" /> and <see cref="Domain.Entities.FareAttribute"/> objects.
    /// </summary>
    public class EditFareViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Unit of work used in the view model to access the database.
        /// </summary>
        private readonly IFareService _fareService;

        /// <summary>
        ///     The <see cref="Domain.Entities.FareAttribute" /> object being edited in the window.
        /// </summary>
        private FareAttribute _fareAttribute;

        /// <summary>
        ///     The <see cref="Domain.Entities.FareRule" /> object being edited in the window.
        /// </summary>
        private FareRule _fareRule;

        /// <summary>
        ///     The <see cref="Route" /> currently selected by the user.
        /// </summary>
        private Route _selectedRoute;

        /// <summary>
        ///     The <see cref="Zone" /> currently selected by the user.
        /// </summary>
        private Zone _selectedOriginZone;

        /// <summary>
        ///     The <see cref="Zone" /> currently selected by the user.
        /// </summary>
        private Zone _selectedDestinationZone;

        /// <summary>
        ///     Filter used to make queries about <see cref="Route" /> objects.
        /// </summary>
        private RouteFilter _routeFilter;

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
        /// <param name="fareService">Unit of work exposing methods necessary to manage data.</param>
        /// <param name="fareAttribute">Fare to be edited. If a fare is to be added, this parameter should be left null.</param>
        public EditFareViewModel(IScreen screen, IFareService fareService, FareAttribute fareAttribute = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _fareService = fareService;
            _routeFilter = new RouteFilter();
            RouteSuggestions = new ReactiveList<Route>();
            OriginZoneSuggestions = new ReactiveList<Zone>();
            DestinationZoneSuggestions = new ReactiveList<Zone>();
            TransferCounts = new ReactiveList<TransferCount>(Enum.GetValues(typeof(TransferCount)).Cast<TransferCount>());

            var fareServiceMethod = fareAttribute == null ? new Func<FareAttribute, FareAttribute>(_fareService.CreateFareAttribute) : _fareService.UpdateFareAttribute;
            var ruleServiceMethod = fareAttribute == null ? new Func<FareRule, FareRule>(_fareService.CreateFareRule) : _fareService.UpdateFareRule;
            _fareAttribute = fareAttribute ?? new FareAttribute();
            _fareRule = _fareAttribute.FareRule ?? new FareRule();
            _selectedRoute = _fareRule.Route;
            _selectedOriginZone = _fareRule.Origin;
            _selectedDestinationZone = _fareRule.Destination;

            _routeFilter.ShortNameFilter = _fareRule.Route?.ShortName ?? "";
            _originZoneFilter = _fareRule.Origin?.Name ?? "";
            _destinationZoneFilter = _fareRule.Destination?.Name ?? "";

            #endregion

            this.WhenAnyValue(vm => vm.SelectedRoute).Select(r => r != null)
                .Where(b => b).Subscribe(_ => FareRule.RouteId = SelectedRoute.Id);
            this.WhenAnyValue(vm => vm.SelectedOriginZone).Select(o => o != null)
                .Where(b => b).Subscribe(_ => FareRule.OriginId = SelectedOriginZone.Id);
            this.WhenAnyValue(vm => vm.SelectedDestinationZone).Select(d => d != null)
                .Where(b => b).Subscribe(_ => FareRule.DestinationId = SelectedDestinationZone.Id);

            var allSelected = this.WhenAnyValue(vm => vm.SelectedRoute, vm => vm.SelectedOriginZone,
                vm => vm.SelectedDestinationZone, (r, o, d) => r != null && o != null && d != null);

            #region SaveFare command

            SaveFare = ReactiveCommand.CreateAsyncTask(allSelected, async _ =>
            {
                var result = await Task.Run(() => ruleServiceMethod(FareRule));
                FareAttribute.FareRule = result;
                FareAttribute.FareRuleId = result.Id;
                return await Task.Run(() => fareServiceMethod(FareAttribute));
            });
            SaveFare.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("The currently edited fare cannot be saved to the database. Please check the required fields and try again later.", ex));

            #endregion

            #region UpdateSuggestions commands

            UpdateRouteSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await Task.Run(() => _fareService.FilterRoutes(RouteFilter)));
            UpdateRouteSuggestions.Subscribe(results =>
            {
                RouteSuggestions.Clear();
                RouteSuggestions.AddRange(results);
            });
            UpdateRouteSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateOriginZoneSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await Task.Run(() => _fareService.FilterZones(OriginZoneFilter)));
            UpdateOriginZoneSuggestions.Subscribe(results =>
            {
                OriginZoneSuggestions.Clear();
                OriginZoneSuggestions.AddRange(results);
            });
            UpdateOriginZoneSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateDestinationZoneSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await Task.Run(() => _fareService.FilterZones(DestinationZoneFilter)));
            UpdateDestinationZoneSuggestions.Subscribe(results =>
            {
                DestinationZoneSuggestions.Clear();
                DestinationZoneSuggestions.AddRange(results);
            });
            UpdateDestinationZoneSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.RouteFilter.ShortNameFilter)
                .Where(s => (s != SelectedRoute?.ShortName) && RouteFilter.IsValid)
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
        ///     List containing the suggested <see cref="Route" />s based on user input.
        /// </summary>
        public ReactiveList<Route> RouteSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="Zone" />s based on user input.
        /// </summary>
        public ReactiveList<Zone> OriginZoneSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="Zone" />s based on user input.
        /// </summary>
        public ReactiveList<Zone> DestinationZoneSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the route suggestions.
        /// </summary>
        public ReactiveCommand<List<Route>> UpdateRouteSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the origin zone suggestions.
        /// </summary>
        public ReactiveCommand<List<Zone>> UpdateOriginZoneSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the destination zone suggestions.
        /// </summary>
        public ReactiveCommand<List<Zone>> UpdateDestinationZoneSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="Domain.Entities.FareRule"/> and <see cref="Domain.Entities.FareAttribute"/> objects.
        /// </summary>
        public ReactiveCommand<FareAttribute> SaveFare { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The <see cref="Domain.Entities.FareAttribute" /> object being edited in the window.
        /// </summary>
        public FareAttribute FareAttribute
        {
            get { return _fareAttribute; }
            set { this.RaiseAndSetIfChanged(ref _fareAttribute, value); }
        }

        /// <summary>
        ///     The <see cref="Domain.Entities.FareRule" /> object being edited in the window.
        /// </summary>
        public FareRule FareRule
        {
            get { return _fareRule; }
            set { this.RaiseAndSetIfChanged(ref _fareRule, value); }
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
        ///     The origin <see cref="Zone" /> currently selected by the user.
        /// </summary>
        public Zone SelectedOriginZone
        {
            get { return _selectedOriginZone; }
            set { this.RaiseAndSetIfChanged(ref _selectedOriginZone, value); }
        }

        /// <summary>
        ///     The destination <see cref="Zone" /> currently selected by the user.
        /// </summary>
        public Zone SelectedDestinationZone
        {
            get { return _selectedDestinationZone; }
            set { this.RaiseAndSetIfChanged(ref _selectedDestinationZone, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Route" /> objects.
        /// </summary>
        public RouteFilter RouteFilter
        {
            get { return _routeFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeFilter, value); }
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
