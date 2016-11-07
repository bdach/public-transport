using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Views.Filter
{
    /// <summary>
    /// Interaction logic for FilterFareView.xaml
    /// </summary>
    public partial class FilterFareView : UserControl, IViewFor<FilterFareViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterFareViewModel), typeof(FilterFareView), new PropertyMetadata(default(FilterFareViewModel)));
        
        public FilterFareView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.FareAttributes, v => v.FaresListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.FareFilter.RouteNameFilter, v => v.RouteNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.FareFilter.OriginZoneNameFilter, v => v.OriginZoneNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.FareFilter.DestinationZoneNameFilter, v => v.DestinationZoneNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedFare, v => v.FaresListView.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.AddFare, v => v.AddFareButton);
            this.BindCommand(ViewModel, vm => vm.EditFare, v => v.EditFareButton);
            this.BindCommand(ViewModel, vm => vm.DeleteFare, v => v.DeleteFareButton);
        }

        public FilterFareViewModel ViewModel
        {
            get { return (FilterFareViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterFareViewModel)value; }
        }
    }
}
