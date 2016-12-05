using System.Windows;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Views.Filter
{
    /// <summary>
    /// Interaction logic for FilterRouteView.xaml
    /// </summary>
    public partial class FilterRouteView : IViewFor<FilterRouteViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterRouteViewModel), typeof(FilterRouteView), new PropertyMetadata(default(FilterRouteViewModel)));

        public FilterRouteViewModel ViewModel
        {
            get { return (FilterRouteViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public FilterRouteView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Routes, v => v.RoutesListView.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.RouteTypes, v => v.RouteTypesComboBox.ItemsSource);
            this.Bind(ViewModel, vm => vm.RouteReactiveFilter.AgencyNameFilter, v => v.AgencyNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.RouteReactiveFilter.ShortNameFilter, v => v.ShortRouteNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.RouteReactiveFilter.LongNameFilter, v => v.LongRouteNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.RouteReactiveFilter.RouteTypeFilter, v => v.RouteTypesComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedRoute, v => v.RoutesListView.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.AddRoute, v => v.AddRouteButton);
            this.BindCommand(ViewModel, vm => vm.EditRoute, v => v.EditRouteButton);
            this.BindCommand(ViewModel, vm => vm.DeleteRoute, v => v.DeleteRouteButton);
            this.BindCommand(ViewModel, vm => vm.ShowTimetable, v => v.ShowTimetableButton);
            this.BindCommand(ViewModel, vm => vm.ClearRouteTypeChoice, v => v.ClearRouteTypeButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterRouteViewModel) value; }
        }
    }
}
