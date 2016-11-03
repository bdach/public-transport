using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    /// Interaction logic for FilterCityView.xaml
    /// </summary>
    public partial class FilterCityView : UserControl, IViewFor<FilterCityViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterCityViewModel), typeof(FilterCityView), new PropertyMetadata(default(FilterCityViewModel)));

        public FilterCityViewModel ViewModel
        {
            get { return (FilterCityViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public FilterCityView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Cities, v => v.Cities.ItemsSource);
            this.Bind(ViewModel, vm => vm.NameFilter, v => v.NameFilter.Text);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterCityViewModel) value; }
        }
    }
}
