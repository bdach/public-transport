using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering stops.
    /// </summary>
    public class FilterStopViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IStopService _stopService;

        /// <summary>
        ///     <see cref="StopReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        private StopReactiveFilter _stopReactiveFilter;

        /// <summary>
        ///     The <see cref="Stop" /> currently selected by the user.
        /// </summary>
        private Stop _selectedStop;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display the view model on.</param>
        /// <param name="stopService">Service used in the view model to access the database.</param>
        public FilterStopViewModel(IScreen screen, IStopService stopService = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _stopService = stopService ?? Locator.Current.GetService<IStopService>();
            _stopReactiveFilter = new StopReactiveFilter();
            Stops = new ReactiveList<Stop>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedStop).Select(s => s != null);

            #region Stop filtering command

            FilterStops = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _stopService.FilterStops(StopReactiveFilter.Convert())));
            FilterStops.Subscribe(result =>
            {
                Stops.Clear();
                Stops.AddRange(result);
            });
            FilterStops.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch stop data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered stops upon filter change

            this.WhenAnyValue(
                    vm => vm.StopReactiveFilter.StopNameFilter,
                    vm => vm.StopReactiveFilter.CityNameFilter,
                    vm => vm.StopReactiveFilter.StreetNameFilter,
                    vm => vm.StopReactiveFilter.ZoneNameFilter,
                    vm => vm.StopReactiveFilter.ParentStationNameFilter)
                .Where(_ => StopReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterStops);

            #endregion

            #region Delete stop command

            DeleteStop = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await Task.Run(() => _stopService.DeleteStop(SelectedStop));
                return Unit.Default;
            });
            DeleteStop.Subscribe(_ => SelectedStop = null);
            DeleteStop.InvokeCommand(FilterStops);
            DeleteStop.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected stop. Please contact the system administrator.", e));

            #endregion

            #region Add/edit stop command

            AddStop = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditStopViewModel(HostScreen, _stopService)));
            EditStop = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditStopViewModel(HostScreen, _stopService, SelectedStop)));

            #endregion

            #region Updating the list of stops upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && StopReactiveFilter.IsValid)
                .InvokeCommand(FilterStops);

            #endregion

            #region Disposing of contexts

            HostScreen.Router.NavigateAndReset
                .Skip(1)
                .Subscribe(_ => _stopService.Dispose());

            #endregion
        }

        /// <summary>
        ///     The <see cref="Stop" /> currently selected by the user.
        /// </summary>
        public Stop SelectedStop
        {
            get { return _selectedStop; }
            set { this.RaiseAndSetIfChanged(ref _selectedStop, value); }
        }

        /// <summary>
        ///     <see cref="StopReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        public StopReactiveFilter StopReactiveFilter
        {
            get { return _stopReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _stopReactiveFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="Stop" /> objects currently displayed by the user.
        /// </summary>
        public ReactiveList<Stop> Stops { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="Stop" /> objects from the database, using the <see cref="StopReactiveFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<List<Stop>> FilterStops { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for adding a new <see cref="Stop" /> to the database.
        /// </summary>
        public ReactiveCommand<object> AddStop { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for editing a <see cref="Stop" />.
        /// </summary>
        public ReactiveCommand<object> EditStop { get; protected set; }

        /// <summary>
        ///     Deletes the currently selected <see cref="Stop" />.
        /// </summary>
        public ReactiveCommand<Unit> DeleteStop { get; protected set; }

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
