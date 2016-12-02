using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Zones;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering zones.
    /// </summary>
    public class FilterZoneViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IZoneService _zoneService;

        /// <summary>
        ///     String containing the zone name filter.
        /// </summary>
        private string _nameFilter;

        /// <summary>
        ///     <see cref="Zone" /> object currently selected in the view.
        /// </summary>
        private ZoneDto _selectedZone;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display view model on.</param>
        /// <param name="zoneService">Service exposing methods necessary to manage data.</param>
        public FilterZoneViewModel(IScreen screen, IZoneService zoneService = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _zoneService = zoneService ?? Locator.Current.GetService<IZoneService>();
            Zones = new ReactiveList<ZoneDto>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedZone).Select(z => z != null);

            #region Zone filtering command

            FilterZones = ReactiveCommand.CreateAsyncTask(async _ => await _zoneService.FilterZonesAsync(NameFilter));
            FilterZones.Subscribe(result =>
            {
                Zones.Clear();
                Zones.AddRange(result);
            });
            FilterZones.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch zone data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered zones upon filter string change

            this.WhenAnyValue(vm => vm.NameFilter)
                .Where(s => !string.IsNullOrEmpty(s))
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterZones);

            #endregion

            #region Delete zone command

            // TODO: Maybe prompt for confirmation?
            DeleteZone = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await _zoneService.DeleteZoneAsync(SelectedZone);
                return Unit.Default;
            });
            DeleteZone.Subscribe(_ => SelectedZone = null);
            DeleteZone.InvokeCommand(FilterZones);
            DeleteZone.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected zone. Please contact the system administrator.", e));

            #endregion

            #region Add/edit zone commands

            AddZone = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditZoneViewModel(screen, _zoneService)));
            EditZone = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditZoneViewModel(screen, _zoneService, SelectedZone)));

            #endregion

            #region Updating the list of zones upon navigating back to this view model

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this)
                .InvokeCommand(FilterZones);

            #endregion
        }

        /// <summary>
        ///     Reactive list containing the filtered <see cref="Zone" /> objects.
        /// </summary>
        public ReactiveList<ZoneDto> Zones { get; protected set; }

        /// <summary>
        ///     Command responsible for filtering out zones in accordance with the <see cref="NameFilter" />.
        /// </summary>
        public ReactiveCommand<ZoneDto[]> FilterZones { get; protected set; }

        /// <summary>
        ///     Command responsible for launching the zone editing view.
        /// </summary>
        public ReactiveCommand<object> EditZone { get; protected set; }

        /// <summary>
        ///     Command responsible for deleting the zone.
        /// </summary>
        public ReactiveCommand<Unit> DeleteZone { get; protected set; }

        /// <summary>
        ///     Command responsible for launching the zone adding view.
        /// </summary>
        public ReactiveCommand<object> AddZone { get; protected set; }

        /// <summary>
        ///     Property containing the name filter.
        /// </summary>
        public string NameFilter
        {
            get { return _nameFilter; }
            set { this.RaiseAndSetIfChanged(ref _nameFilter, value); }
        }

        /// <summary>
        ///     Property exposing the currently selected <see cref="Zone" />.
        /// </summary>
        public ZoneDto SelectedZone
        {
            get { return _selectedZone; }
            set { this.RaiseAndSetIfChanged(ref _selectedZone, value); }
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
        public MenuOption AssociatedMenuOption => MenuOption.Zone;
    }
}
