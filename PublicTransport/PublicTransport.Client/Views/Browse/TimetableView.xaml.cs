using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels.Browse;
using ReactiveUI;

namespace PublicTransport.Client.Views.Browse
{
    /// <summary>
    /// Interaction logic for TimetableView.xaml
    /// </summary>
    public partial class TimetableView : UserControl, IViewFor<TimetableViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(TimetableViewModel), typeof(TimetableView), new PropertyMetadata(default(TimetableViewModel)));

        public TimetableViewModel ViewModel
        {
            get { return (TimetableViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public TimetableView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Route, v => v.RouteName.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Stops, v => v.StopsListView.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.StopTimes, v => v.StopTimesListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedStop, v => v.StopsListView.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedStopTime, v => v.StopTimesListView.SelectedItem);
            this.Bind(ViewModel, vm => vm.StopTimeFilter.Date, v => v.SearchDatePicker.SelectedDate);
            this.Bind(ViewModel, vm => vm.StopTimeFilter.Time, v => v.SearchTimePicker.Text);
            this.BindCommand(ViewModel, vm => vm.AddTrip, v => v.AddTripButton);
            this.BindCommand(ViewModel, vm => vm.DeleteTrip, v => v.DeleteTripButton);
            this.BindCommand(ViewModel, vm => vm.EditTrip, v => v.EditTripButton);
            this.WhenAnyValue(x => x.ViewModel.GetStops)
                .SelectMany(x => x.ExecuteAsync())
                .Subscribe();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (TimetableViewModel) value;
            }
        }
    }
}
