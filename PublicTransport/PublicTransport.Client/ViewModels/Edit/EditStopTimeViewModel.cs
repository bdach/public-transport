using System;
using System.Reactive.Linq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Services.Routes;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model responsible for editing stop times.
    /// </summary>
    public class EditStopTimeViewModel : ReactiveObject
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IRouteService _routeService;

        /// <summary>
        ///     <see cref="StopReactiveFilter" /> used for filtering stop suggestions.
        /// </summary>
        private StopReactiveFilter _stopReactiveFilter;
        
        /// <summary>
        ///     <see cref="StopTimeDto" /> object storing the currently edited data.
        /// </summary>
        private StopTimeDto _stopTime;

        /// <summary>
        ///     Currently selected <see cref="StopDto"/> object.
        /// </summary>
        private StopDto _selectedStop;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="routeService">Service used in the view model to access the database.</param>
        /// <param name="stopTime">Stop time to edit; null if a stop time is to be added.</param>
        public EditStopTimeViewModel(IRouteService routeService, StopTimeDto stopTime = null)
        {
            #region Property/field initialization

            StopSuggestions = new ReactiveList<StopDto>();
            _routeService = routeService;
            _stopReactiveFilter = new StopReactiveFilter { StopNameFilter = stopTime?.Stop?.Name ?? "" };
            SelectedStop = stopTime?.Stop;
            _stopTime = stopTime ?? new StopTimeDto();

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await _routeService.FilterStopsAsync(StopReactiveFilter.Convert()));
            UpdateSuggestions.Subscribe(results =>
            {
                StopSuggestions.Clear();
                StopSuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            this.WhenAnyValue(vm => vm.StopReactiveFilter.StopNameFilter)
                .Where(s => (s != SelectedStop?.Name) && StopReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            this.WhenAnyValue(vm => vm.SelectedStop)
                .Where(s => s != null)
                .Subscribe(_ => StopTime.Stop = SelectedStop);
        }

        /// <summary>
        ///     List containing suggestions for <see cref="StopDto" /> names.
        /// </summary>
        public ReactiveList<StopDto> StopSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the contents of <see cref="StopSuggestions" />.
        /// </summary>
        public ReactiveCommand<StopDto[]> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     The <see cref="StopDto" /> currently selected by the user.
        /// </summary>
        public StopDto SelectedStop
        {
            get { return _selectedStop; }
            set { this.RaiseAndSetIfChanged(ref _selectedStop, value); }
        }

        /// <summary>
        ///     <see cref="StopReactiveFilter" /> used for filtering stop suggestions.
        /// </summary>
        public StopReactiveFilter StopReactiveFilter
        {
            get { return _stopReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _stopReactiveFilter, value); }
        }

        /// <summary>
        ///     Currently edited stop time.
        /// </summary>
        public StopTimeDto StopTime
        {
            get { return _stopTime; }
            set { this.RaiseAndSetIfChanged(ref _stopTime, value); }
        }
    }
}