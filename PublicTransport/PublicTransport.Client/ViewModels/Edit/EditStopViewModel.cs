using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing <see cref="Domain.Entities.Stop" /> objects.
    /// </summary>
    public class EditStopViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Unit of work used in the view model to access the database.
        /// </summary>
        private readonly IStopUnitOfWork _stopUnitOfWork;

        /// <summary>
        ///     The <see cref="Agency" /> object being edited in the window.
        /// </summary>
        private Stop _stop;

        /// <summary>
        ///     The <see cref="Street" /> currently selected by the user.
        /// </summary>
        private Street _selectedStreet;

        /// <summary>
        ///     The <see cref="Zone" /> currently selected by the user.
        /// </summary>
        private Zone _selectedZone;

        /// <summary>
        ///     The <see cref="Zone" /> currently selected by the user.
        /// </summary>
        private Stop _selectedParentStation;

        /// <summary>
        ///     Filter used to make queries about <see cref="Street" /> objects.
        /// </summary>
        private StreetFilter _streetFilter;

        /// <summary>
        ///     Filter used to make queries about <see cref="Zone" /> objects.
        /// </summary>
        private string _zoneFilter;

        /// <summary>
        ///     Filter used to make queries about <see cref="Domain.Entities.Stop" /> objects.
        /// </summary>
        private StopFilter _parentStationFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="stopUnitOfWork">Unit of work exposing methods necessary to manage data.</param>
        /// <param name="stop">Stop to be edited. If a stop is to be added, this parameter should be left null.</param>
        public EditStopViewModel(IScreen screen, IStopUnitOfWork stopUnitOfWork, Stop stop = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            StreetSuggestions = new ReactiveList<Street>();
            ZoneSuggestions = new ReactiveList<Zone>();
            ParentStationSuggestions = new ReactiveList<Stop>();
            _stopUnitOfWork = stopUnitOfWork;

            _streetFilter = new StreetFilter();
            _parentStationFilter = new StopFilter { OnlyStations = true };

            var serviceMethod = stop == null ? new Func<Stop, Stop>(_stopUnitOfWork.CreateStop) : _stopUnitOfWork.UpdateStop;
            _stop = stop ?? new Stop();
            _selectedStreet = _stop.Street;
            _selectedZone = _stop.Zone;
            _selectedParentStation = _stop.ParentStation;

            _streetFilter.StreetNameFilter = _stop.Street?.Name ?? "";
            _zoneFilter = _stop.Zone?.Name ?? "";
            _parentStationFilter.StopNameFilter = _stop.ParentStation?.Name ?? "";

            #endregion

            this.WhenAnyValue(vm => vm.SelectedZone).Select(z => z != null)
                .Where(b => b).Subscribe(_ => Stop.ZoneId = SelectedZone.Id);
            this.WhenAnyValue(vm => vm.SelectedParentStation).Select(ps => ps != null)
                .Where(b => b).Subscribe(_ => Stop.ParentStationId = SelectedParentStation.Id);

            var streetSelected = this.WhenAnyValue(vm => vm.SelectedStreet).Select(s => s != null);
            streetSelected.Where(b => b).Subscribe(_ => Stop.StreetId = SelectedStreet.Id);

            #region SaveStop command

            SaveStop = ReactiveCommand.CreateAsyncTask(streetSelected, async _ => await Task.Run(() => serviceMethod(Stop)));
            SaveStop.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("The currently edited stop cannot be saved to the database. Please contact the system administrator.", ex));

            #endregion

            #region UpdateSuggestions commands

            UpdateStreetSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _stopUnitOfWork.FilterStreets(StreetFilter)));
            UpdateStreetSuggestions.Subscribe(results =>
            {
                StreetSuggestions.Clear();
                StreetSuggestions.AddRange(results);
            });
            UpdateStreetSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateZoneSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _stopUnitOfWork.FilterZones(ZoneFilter)));
            UpdateZoneSuggestions.Subscribe(results =>
            {
                ZoneSuggestions.Clear();
                ZoneSuggestions.AddRange(results);
            });
            UpdateZoneSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateParentStationSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _stopUnitOfWork.FilterStops(ParentStationFilter)));
            UpdateParentStationSuggestions.Subscribe(results =>
            {
                ParentStationSuggestions.Clear();
                ParentStationSuggestions.AddRange(results);
            });
            UpdateParentStationSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.StreetFilter.StreetNameFilter)
                .Where(s => (s != SelectedStreet?.Name) && StreetFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateStreetSuggestions);

            this.WhenAnyValue(vm => vm.ZoneFilter)
                .Where(z => (z != SelectedZone?.Name) && !string.IsNullOrWhiteSpace(ZoneFilter))
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateZoneSuggestions);

            this.WhenAnyValue(vm => vm.ParentStationFilter.StopNameFilter)
                .Where(ps => (ps != SelectedParentStation?.Name) && ParentStationFilter.IsValid)
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
        public ReactiveList<Street> StreetSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="Zone" />s based on user input.
        /// </summary>
        public ReactiveList<Zone> ZoneSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="Zone" />s based on user input.
        /// </summary>
        public ReactiveList<Stop> ParentStationSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the street suggestions.
        /// </summary>
        public ReactiveCommand<List<Street>> UpdateStreetSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the zone suggestions.
        /// </summary>
        public ReactiveCommand<List<Zone>> UpdateZoneSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the parent station suggestions.
        /// </summary>
        public ReactiveCommand<List<Stop>> UpdateParentStationSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="Domain.Entities.Stop" /> object.
        /// </summary>
        public ReactiveCommand<Stop> SaveStop { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The <see cref="Domain.Entities.Stop" /> object being edited in the window.
        /// </summary>
        public Stop Stop
        {
            get { return _stop; }
            set { this.RaiseAndSetIfChanged(ref _stop, value); }
        }

        /// <summary>
        ///     The <see cref="Street" /> currently selected by the user.
        /// </summary>
        public Street SelectedStreet
        {
            get { return _selectedStreet; }
            set { this.RaiseAndSetIfChanged(ref _selectedStreet, value); }
        }

        /// <summary>
        ///     The <see cref="Zone" /> currently selected by the user.
        /// </summary>
        public Zone SelectedZone
        {
            get { return _selectedZone; }
            set { this.RaiseAndSetIfChanged(ref _selectedZone, value); }
        }

        /// <summary>
        ///     The <see cref="Domain.Entities.Stop" /> parent station currently selected by the user.
        /// </summary>
        public Stop SelectedParentStation
        {
            get { return _selectedParentStation; }
            set { this.RaiseAndSetIfChanged(ref _selectedParentStation, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Street" /> objects.
        /// </summary>
        public StreetFilter StreetFilter
        {
            get { return _streetFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetFilter, value); }
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
        public StopFilter ParentStationFilter
        {
            get { return _parentStationFilter; }
            set { this.RaiseAndSetIfChanged(ref _parentStationFilter, value); }
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
