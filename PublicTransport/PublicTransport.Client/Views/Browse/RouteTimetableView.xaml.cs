using System;
using System.Reactive.Linq;
using System.Windows;
using PublicTransport.Client.ViewModels.Browse;
using ReactiveUI;

namespace PublicTransport.Client.Views.Browse
{
    /// <summary>
    /// Interaction logic for RouteTimetableView.xaml
    /// </summary>
    public partial class RouteTimetableView : IViewFor<RouteTimetableViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(RouteTimetableViewModel), typeof(RouteTimetableView), new PropertyMetadata(default(RouteTimetableViewModel)));

        public RouteTimetableViewModel ViewModel
        {
            get { return (RouteTimetableViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public RouteTimetableView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Route, v => v.RouteName.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Stops, v => v.StopsListView.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.StopTimes, v => v.StopTimesListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedStop, v => v.StopsListView.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedStopTime, v => v.StopTimesListView.SelectedItem);
            this.Bind(ViewModel, vm => vm.StopTimeReactiveFilter.Date, v => v.SearchDatePicker.SelectedDate);
            this.Bind(ViewModel, vm => vm.StopTimeReactiveFilter.Time, v => v.SearchTimePicker.SelectedTime);
            this.BindCommand(ViewModel, vm => vm.AddTrip, v => v.AddTripButton);
            this.BindCommand(ViewModel, vm => vm.DeleteTrip, v => v.DeleteTripButton);
            this.BindCommand(ViewModel, vm => vm.EditTrip, v => v.EditTripButton);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            this.WhenAnyValue(x => x.ViewModel.GetStops)
                .SelectMany(x => x.ExecuteAsync())
                .Subscribe();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (RouteTimetableViewModel) value;
            }
        }
    }
}
