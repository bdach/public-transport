using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Views.Filter
{
    /// <summary>
    /// Interaction logic for FilterAgencyView.xaml
    /// </summary>
    public partial class FilterAgencyView : UserControl, IViewFor<FilterAgencyViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterAgencyViewModel), typeof(FilterAgencyView), new PropertyMetadata(default(FilterAgencyViewModel)));

        public FilterAgencyViewModel ViewModel
        {
            get { return (FilterAgencyViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public FilterAgencyView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Agencies, v => v.AgenciesListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.AgencyFilter.AgencyNameFilter, v => v.AgencyNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.AgencyFilter.CityNameFilter, v => v.CityNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.AgencyFilter.StreetNameFilter, v => v.StreetNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedAgency, v => v.AgenciesListView.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.AddAgency, v => v.AddAgencyButton);
            this.BindCommand(ViewModel, vm => vm.EditAgency, v => v.EditAgencyButton);
            this.BindCommand(ViewModel, vm => vm.DeleteAgency, v => v.DeleteAgencyButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterAgencyViewModel) value; }
        }
    }
}
