using System;
using System.Reactive.Linq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Search;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels.Browse
{
    /// <summary>
    ///     View model for searching <see cref="Route"/> objects.
    /// </summary>
    public class SearchRouteViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly ISearchService _searchService;

        /// <summary>
        ///     The origin <see cref="StopDto" /> currently selected by the user.
        /// </summary>
        private StopDto _selectedOriginStop;

        /// <summary>
        ///     The destination <see cref="StopDto" /> currently selected by the user.
        /// </summary>
        private StopDto _selectedDestinationStop;

        /// <summary>
        ///     Filter used to make queries about <see cref="Route" /> objects.
        /// </summary>
        private RouteSearchReactiveFilter _routeSearchReactiveFilter;

        /// <summary>
        ///     Filter used to make queries about <see cref="Stop" /> objects.
        /// </summary>
        private StopReactiveFilter _originStopFilter;

        /// <summary>
        ///     Filter used to make queries about <see cref="Stop" /> objects.
        /// </summary>
        private StopReactiveFilter _destinationStopFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="searchService">Service exposing methods necessary to manage data.</param>
        public SearchRouteViewModel(IScreen screen, ISearchService searchService = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _searchService = searchService ?? Locator.Current.GetService<ISearchService>();
            _routeSearchReactiveFilter = new RouteSearchReactiveFilter();
            _originStopFilter = new StopReactiveFilter();
            _destinationStopFilter = new StopReactiveFilter();
            OriginStopSuggestions = new ReactiveList<StopDto>();
            DestinationStopSuggestions = new ReactiveList<StopDto>();
            Routes = new ReactiveList<RouteDto>();

            #endregion
            
            this.WhenAnyValue(vm => vm.SelectedOriginStop).Select(o => o != null)
                .Where(b => b).Subscribe(_ => RouteSearchReactiveFilter.OriginStopIdFilter = SelectedOriginStop.Id);
            this.WhenAnyValue(vm => vm.SelectedDestinationStop).Select(d => d != null)
                .Where(b => b).Subscribe(_ => RouteSearchReactiveFilter.DestinationStopIdFilter = SelectedDestinationStop.Id);

            var allSelected = this.WhenAnyValue(vm => vm.SelectedOriginStop, vm => vm.SelectedDestinationStop, (o, d) => o != null && d != null);

            #region FindRoutes command

            FindRoutes = ReactiveCommand.CreateAsyncTask(allSelected, async _ => await _searchService.FindRoutesAsync(RouteSearchReactiveFilter.Convert()));
            FindRoutes.Subscribe(result =>
            {
                Routes.Clear();
                Routes.AddRange(result);
            });
            FindRoutes.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch route data from the database. Please contact the system administrator.", e));

            #endregion

            #region UpdateSuggestions commands

            UpdateOriginStopSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await _searchService.FilterStopsAsync(OriginStopFilter.Convert()));
            UpdateOriginStopSuggestions.Subscribe(results =>
            {
                OriginStopSuggestions.Clear();
                OriginStopSuggestions.AddRange(results);
            });
            UpdateOriginStopSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            UpdateDestinationStopSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
                await _searchService.FilterStopsAsync(DestinationStopFilter.Convert()));
            UpdateDestinationStopSuggestions.Subscribe(results =>
            {
                DestinationStopSuggestions.Clear();
                DestinationStopSuggestions.AddRange(results);
            });
            UpdateDestinationStopSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions
            
            this.WhenAnyValue(vm => vm.OriginStopFilter.StopNameFilter)
                .Where(z => (z != SelectedOriginStop?.Name) && OriginStopFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateOriginStopSuggestions);

            this.WhenAnyValue(vm => vm.DestinationStopFilter.StopNameFilter)
                .Where(ps => (ps != SelectedDestinationStop?.Name) && OriginStopFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateDestinationStopSuggestions);

            #endregion
        }

        /// <summary>
        ///     The list of <see cref="RouteDto" /> objects currently displayed to the user.
        /// </summary>
        public ReactiveList<RouteDto> Routes { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="RouteDto" /> objects from the database, using the <see cref="RouteSearchReactiveFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<RouteDto[]> FindRoutes { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="StopDto" />s based on user input.
        /// </summary>
        public ReactiveList<StopDto> OriginStopSuggestions { get; protected set; }

        /// <summary>
        ///     List containing the suggested <see cref="StopDto" />s based on user input.
        /// </summary>
        public ReactiveList<StopDto> DestinationStopSuggestions { get; protected set; }
        
        /// <summary>
        ///     Command responsible for updating the origin stop suggestions.
        /// </summary>
        public ReactiveCommand<StopDto[]> UpdateOriginStopSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the destination stop suggestions.
        /// </summary>
        public ReactiveCommand<StopDto[]> UpdateDestinationStopSuggestions { get; protected set; }

        /// <summary>
        ///     The origin <see cref="StopDto" /> currently selected by the user.
        /// </summary>
        public StopDto SelectedOriginStop
        {
            get { return _selectedOriginStop; }
            set { this.RaiseAndSetIfChanged(ref _selectedOriginStop, value); }
        }

        /// <summary>
        ///     The destination <see cref="StopDto" /> currently selected by the user.
        /// </summary>
        public StopDto SelectedDestinationStop
        {
            get { return _selectedDestinationStop; }
            set { this.RaiseAndSetIfChanged(ref _selectedDestinationStop, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Route" /> objects.
        /// </summary>
        public RouteSearchReactiveFilter RouteSearchReactiveFilter
        {
            get { return _routeSearchReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _routeSearchReactiveFilter, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Stop" /> objects.
        /// </summary>
        public StopReactiveFilter OriginStopFilter
        {
            get { return _originStopFilter; }
            set { this.RaiseAndSetIfChanged(ref _originStopFilter, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Stop" /> objects.
        /// </summary>
        public StopReactiveFilter DestinationStopFilter
        {
            get { return _destinationStopFilter; }
            set { this.RaiseAndSetIfChanged(ref _destinationStopFilter, value); }
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
