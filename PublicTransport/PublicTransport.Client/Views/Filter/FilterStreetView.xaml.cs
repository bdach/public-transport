using System.Windows;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Views.Filter
{
    /// <summary>
    /// Interaction logic for FilterStreetView.xaml
    /// </summary>
    public partial class FilterStreetView : IViewFor<FilterStreetViewModel>
    {
        public static readonly DependencyProperty FilterStreetViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterStreetViewModel), typeof(FilterStreetView), new PropertyMetadata(default(FilterStreetViewModel)));

        public FilterStreetViewModel ViewModel
        {
            get { return (FilterStreetViewModel) GetValue(FilterStreetViewModelProperty); }
            set { SetValue(FilterStreetViewModelProperty, value); }
        }

        public FilterStreetView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Streets, v => v.StreetsListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.StreetFilter.StreetNameFilter, v => v.StreetNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.StreetFilter.CityNameFilter, v => v.CityNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedStreet, v => v.StreetsListView.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.AddStreet, v => v.AddStreetButton);
            this.BindCommand(ViewModel, vm => vm.EditStreet, v => v.EditStreetButton);
            this.BindCommand(ViewModel, vm => vm.DeleteStreet, v => v.DeleteStreetButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterStreetViewModel) value; }
        }
    }
}
