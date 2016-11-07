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
    public class EditStopTimeViewModel : ReactiveObject
    {
        private StopTime _stopTime;
        private StopService _stopService;

        public EditStopTimeViewModel(StopService stopService, int sequenceNo, StopTime stopTime = null)
        {
            _stopService = stopService;
            _stopTime = stopTime ?? new StopTime();
            _stopTime.StopSequence = sequenceNo;
            _stopFilter = new StopFilter();
            StopSuggestions = new ReactiveList<Stop>();
            IsNew = stopTime == null;
            _stopFilter.StopNameFilter = stopTime?.Stop?.Name ?? "";

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
        
            this.WhenAnyValue(vm => vm.StopFilter.StopNameFilter)
                .Where(s => (s != SelectedStop?.Name) && StopFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            this.WhenAnyValue(vm => vm.SelectedStop)
                .Where(s => s != null)
                .Subscribe(_ =>
                {
                    StopTime.StopId = SelectedStop.Id;
                });
        }

        public ReactiveList<Stop> StopSuggestions { get; protected set; }
        public ReactiveCommand<List<Stop>> UpdateSuggestions { get; protected set; }

        public Stop SelectedStop
        {
            get { return _selectedStop; }
            set { this.RaiseAndSetIfChanged(ref _selectedStop, value); }
        }

        public StopFilter StopFilter
        {
            get { return _stopFilter; }
            set { this.RaiseAndSetIfChanged(ref _stopFilter, value); }
        }

        public StopTime StopTime
        {
            get { return _stopTime; }
            set { this.RaiseAndSetIfChanged(ref _stopTime, value); }
        }
        public bool IsNew;
        private StopFilter _stopFilter;
        private Stop _selectedStop;
    }
}