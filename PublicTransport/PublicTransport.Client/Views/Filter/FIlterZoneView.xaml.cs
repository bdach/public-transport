using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Views.Filter
{
    /// <summary>
    /// Interaction logic for FilterZoneView.xaml
    /// </summary>
    public partial class FilterZoneView : UserControl, IViewFor<FilterZoneViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterZoneViewModel), typeof(FilterZoneView), new PropertyMetadata(default(FilterZoneViewModel)));

        public FilterZoneViewModel ViewModel
        {
            get { return (FilterZoneViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public FilterZoneView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Zones, v => v.ZonesListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.NameFilter, v => v.NameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedZone, v => v.ZonesListView.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.AddZone, v => v.AddZoneButton);
            this.BindCommand(ViewModel, vm => vm.EditZone, v => v.EditZoneButton);
            this.BindCommand(ViewModel, vm => vm.DeleteZone, v => v.DeleteZoneButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterZoneViewModel)value; }
        }
    }
}
