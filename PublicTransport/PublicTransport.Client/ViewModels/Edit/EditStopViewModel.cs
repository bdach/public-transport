using System;
using System.Reactive;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Stops;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing <see cref="Domain.Entities.Stop" /> objects.
    /// </summary>
    public class EditStopViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IStopService _stopService;

        /// <summary>
        ///     The <see cref="AgencyDto" /> object being edited in the window.
        /// </summary>
        private StopDto _stop;

        /// <summary>
        ///     The <see cref="StreetDto" /> currently selected by the user.
        /// </summary>
        private StreetDto _selectedStreet;

        /// <summary>
        ///     The <see cref="ZoneDto" /> currently selected by the user.
        /// </summary>
        private ZoneDto _selectedZone;

        /// <summary>
        ///     The <see cref="StopDto" /> parent station currently selected by the user.
        /// </summary>
        private StopDto _selectedParentStation;

        /// <summary>
        ///     Filter used to make queries about <see cref="Street" /> objects.
        /// </summary>
        private StreetReactiveFilter _streetReactiveFilter;

        /// <summary>
        ///     Filter used to make queries about <see cref="Zone" /> objects.
        /// </summary>
        private string _zoneFilter;

        /// <summary>
        ///     Filter used to make queries about <see cref="Domain.Entities.Stop" /> objects.
        /// </summary>
        private StopReactiveFilter _parentStationReactiveFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="stopService">Service exposing methods necessary to manage data.</param>
        /// <param name="stop">Stop to be edited. If a stop is to be added, this parameter should be left null.</param>
        public EditStopViewModel(IScreen screen, IStopService stopService, StopDto stop = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            StreetSuggestions = new ReactiveList<StreetDto>();
            ZoneSuggestions = new ReactiveList<ZoneDto>();
            ParentStationSuggestions = new ReactiveList<StopDto>();
            _stopService = stopService;

            _streetReactiveFilter = new StreetReactiveFilter();
            _parentStationReactiveFilter = new StopReactiveFilter { OnlyStations = true };

            var serviceMethod = stop == null ? new Func<StopDto, Task<StopDto>>(_stopService.CreateStopAsync) : _stopService.UpdateStopAsync;
            _stop = stop ?? new StopDto();
            _selectedStreet = _stop.Street;
            _selectedZone = _stop.Zone;
            _selectedParentStation = _stop.ParentStation;

            _streetReactiveFilter.StreetNameFilter = _stop.Street?.Name ?? "";
            _zoneFilter = _stop.Zone?.Name ?? "";
            _parentStationReactiveFilter.StopNameFilter = _stop.ParentStation?.Name ?? "";

            #endregion

            this.WhenAnyValue(vm => vm.SelectedZone).Select(z => z != null)
                .Where(b => b).Subscribe(_ => Stop.Zone = SelectedZone);
            this.WhenAnyValue(vm => vm.SelectedParentStation).Select(ps => ps != null)
                .Where(b => b).Subscribe(_ => Stop.ParentStation = SelectedParentStation);

            var streetSelected = this.WhenAnyValue(vm => vm.SelectedStreet).Select(s => s != null);
            streetSelected.Where(b => b).Subscribe(_ => Stop.Street = SelectedStreet);

            #region SaveStop command

            SaveStop = ReactiveCommand.CreateAsyncTask(streetSelected, async _ => await serviceMethod(Stop));
            SaveStop.ThrownExceptions
                .Where(ex => !(ex is FaultException<ValidationFault>))
                .Subscribe(ex =>
                    UserError.Throw("Cannot connect to the server. Please try again later.", ex));
            SaveStop.ThrownExceptions
                .Where(ex => ex is FaultException<ValidationFault>)
                .Select(ex => ex as FaultException<ValidationFault>)
                .Subscribe(ex => UserError.Throw(string.Join("\n", ex.Detail.Errors), ex));

            #endregion

            #region UpdateSuggestions commands

            UpdateStreetSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await _stopService.FilterStreetsAsync(StreetReactiveFilter.Convert()));
            UpdateStreetSuggestions.Subscribe(results =>
            {
                StreetSuggestions.Clear();
                StreetSuggestions.AddRange(results);
            });
            UpdateStreetSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateZoneSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await _stopService.FilterZonesAsync(ZoneFilter));
            UpdateZoneSuggestions.Subscribe(results =>
            {
                ZoneSuggestions.Clear();
                ZoneSuggestions.AddRange(results);
            });
            UpdateZoneSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateParentStationSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await _stopService.FilterStopsAsync(ParentStationReactiveFilter.Convert()));
            UpdateParentStationSuggestions.Subscribe(results =>
            {
                ParentStationSuggestions.Clear();
                ParentStationSuggestions.AddRange(results);
            });
            UpdateParentStationSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.StreetReactiveFilter.StreetNameFilter)
                .Where(s => (s != SelectedStreet?.Name) && StreetReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateStreetSuggestions);

            this.WhenAnyValue(vm => vm.ZoneFilter)
                .Where(z => (z != SelectedZone?.Name) && !string.IsNullOrWhiteSpace(ZoneFilter))
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateZoneSuggestions);

            this.WhenAnyValue(vm => vm.ParentStationReactiveFilter.StopNameFilter)
                .Where(ps => (ps != SelectedParentStation?.Name) && ParentStationReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateParentStationSuggestions);

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     List containing the suggested <see cref="Street" />s based on user input.
        /// </summary>
        public ReactiveList<StreetDto> StreetSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="Zone" />s based on user input.
        /// </summary>
        public ReactiveList<ZoneDto> ZoneSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="Zone" />s based on user input.
        /// </summary>
        public ReactiveList<StopDto> ParentStationSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the street suggestions.
        /// </summary>
        public ReactiveCommand<StreetDto[]> UpdateStreetSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the zone suggestions.
        /// </summary>
        public ReactiveCommand<ZoneDto[]> UpdateZoneSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the parent station suggestions.
        /// </summary>
        public ReactiveCommand<StopDto[]> UpdateParentStationSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="Domain.Entities.Stop" /> object.
        /// </summary>
        public ReactiveCommand<StopDto> SaveStop { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The <see cref="StopDto" /> object being edited in the window.
        /// </summary>
        public StopDto Stop
        {
            get { return _stop; }
            set { this.RaiseAndSetIfChanged(ref _stop, value); }
        }

        /// <summary>
        ///     The <see cref="StreetDto" /> currently selected by the user.
        /// </summary>
        public StreetDto SelectedStreet
        {
            get { return _selectedStreet; }
            set { this.RaiseAndSetIfChanged(ref _selectedStreet, value); }
        }

        /// <summary>
        ///     The <see cref="ZoneDto" /> currently selected by the user.
        /// </summary>
        public ZoneDto SelectedZone
        {
            get { return _selectedZone; }
            set { this.RaiseAndSetIfChanged(ref _selectedZone, value); }
        }

        /// <summary>
        ///     The <see cref="StopDto" /> parent station currently selected by the user.
        /// </summary>
        public StopDto SelectedParentStation
        {
            get { return _selectedParentStation; }
            set { this.RaiseAndSetIfChanged(ref _selectedParentStation, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Street" /> objects.
        /// </summary>
        public StreetReactiveFilter StreetReactiveFilter
        {
            get { return _streetReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetReactiveFilter, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Zone" /> objects.
        /// </summary>
        public string ZoneFilter
        {
            get { return _zoneFilter; }
            set { this.RaiseAndSetIfChanged(ref _zoneFilter, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Domain.Entities.Stop" /> parent station objects.
        /// </summary>
        public StopReactiveFilter ParentStationReactiveFilter
        {
            get { return _parentStationReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _parentStationReactiveFilter, value); }
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
        public MenuOption AssociatedMenuOption => MenuOption.Stop;
    }
}
