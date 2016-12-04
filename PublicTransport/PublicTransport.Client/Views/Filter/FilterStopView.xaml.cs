using System.Windows;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Views.Filter
{
    /// <summary>
    /// Interaction logic for FilterStopView.xaml
    /// </summary>
    public partial class FilterStopView : IViewFor<FilterStopViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterStopViewModel), typeof(FilterStopView), new PropertyMetadata(default(FilterStopViewModel)));

        public FilterStopView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Stops, v => v.StopsListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.StopReactiveFilter.StopNameFilter, v => v.StopNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.StopReactiveFilter.CityNameFilter, v => v.CityNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.StopReactiveFilter.StreetNameFilter, v => v.StreetNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.StopReactiveFilter.ZoneNameFilter, v => v.ZoneNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.StopReactiveFilter.ParentStationNameFilter, v => v.ParentStationNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedStop, v => v.StopsListView.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.AddStop, v => v.AddStopButton);
            this.BindCommand(ViewModel, vm => vm.EditStop, v => v.EditStopButton);
            this.BindCommand(ViewModel, vm => vm.DeleteStop, v => v.DeleteStopButton);
        }

        public FilterStopViewModel ViewModel
        {
            get { return (FilterStopViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterStopViewModel)value; }
        }
    }
}
