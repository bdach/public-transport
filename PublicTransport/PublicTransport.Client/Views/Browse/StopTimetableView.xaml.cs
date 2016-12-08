using System;
using System.Reactive.Linq;
using System.Windows;
using PublicTransport.Client.ViewModels.Browse;
using ReactiveUI;

namespace PublicTransport.Client.Views.Browse
{
    /// <summary>
    /// Interaction logic for StopTimetableView.xaml
    /// </summary>
    public partial class StopTimetableView : IViewFor<StopTimetableViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(StopTimetableViewModel), typeof(StopTimetableView), new PropertyMetadata(default(StopTimetableViewModel)));

        public StopTimetableViewModel ViewModel
        {
            get { return (StopTimetableViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public StopTimetableView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Stop, v => v.StopName.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.Routes, v => v.RoutesListView.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.StopTimes, v => v.StopTimesListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedRoute, v => v.RoutesListView.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            this.WhenAnyValue(v => v.ViewModel.GetTimetable)
                .SelectMany(o => o.ExecuteAsync())
                .Subscribe();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (StopTimetableViewModel) value; }
        }
    }
}
