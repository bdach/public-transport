using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model responsible for editing stop times.
    /// </summary>
    public class EditStopTimeViewModel : ReactiveObject
    {
        /// <summary>
        ///     <see cref="StopService" /> responsible for saving the entity to the database.
        /// </summary>
        private readonly StopService _stopService;

        /// <summary>
        ///     Currently edited stop time.
        /// </summary>
        private Stop _selectedStop;

        /// <summary>
        ///     <see cref="DataTransfer.StopFilter" /> used for filtering stop suggestions.
        /// </summary>
        private StopFilter _stopFilter;

        /// <summary>
        ///     <see cref="Domain.Entities.StopTime" /> object storing the currently edited data.
        /// </summary>
        private StopTime _stopTime;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="stopService">Stop service to use.</param>
        /// <param name="stopTime">Stop time to edit; null if a stop time is to be added.</param>
        public EditStopTimeViewModel(StopService stopService, StopTime stopTime = null)
        {
            #region Property/field initialization

            StopSuggestions = new ReactiveList<Stop>();
            _stopService = stopService;
            _stopFilter = new StopFilter();
            _stopFilter.StopNameFilter = stopTime?.Stop?.Name ?? "";
            SelectedStop = stopTime?.Stop;
            _stopTime = stopTime ?? new StopTime();
            _stopTime.Stop = null;

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions =
                ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _stopService.FilterStops(StopFilter)));
            UpdateSuggestions.Subscribe(results =>
            {
                StopSuggestions.Clear();
                StopSuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions
                .Subscribe(
                    ex =>
                        UserError.Throw(
                            "Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            this.WhenAnyValue(vm => vm.StopFilter.StopNameFilter)
                .Where(s => (s != SelectedStop?.Name) && StopFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            this.WhenAnyValue(vm => vm.SelectedStop)
                .Where(s => s != null)
                .Subscribe(_ => StopTime.StopId = SelectedStop.Id);
        }

        /// <summary>
        ///     List containing suggestions for <see cref="Stop" /> names.
        /// </summary>
        public ReactiveList<Stop> StopSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the contents of <see cref="StopSuggestions" />.
        /// </summary>
        public ReactiveCommand<List<Stop>> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     The <see cref="Stop" /> currently selected by the user.
        /// </summary>
        public Stop SelectedStop
        {
            get { return _selectedStop; }
            set { this.RaiseAndSetIfChanged(ref _selectedStop, value); }
        }

        /// <summary>
        ///     <see cref="DataTransfer.StopFilter" /> used for filtering stop suggestions.
        /// </summary>
        public StopFilter StopFilter
        {
            get { return _stopFilter; }
            set { this.RaiseAndSetIfChanged(ref _stopFilter, value); }
        }

        /// <summary>
        ///     Currently edited stop time.
        /// </summary>
        public StopTime StopTime
        {
            get { return _stopTime; }
            set { this.RaiseAndSetIfChanged(ref _stopTime, value); }
        }
    }
}