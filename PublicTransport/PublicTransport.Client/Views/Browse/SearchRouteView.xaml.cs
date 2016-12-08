using System;
using System.Windows;
using PublicTransport.Client.ViewModels.Browse;
using ReactiveUI;

namespace PublicTransport.Client.Views.Browse
{
    /// <summary>
    /// Interaction logic for SearchRouteView.xaml
    /// </summary>
    public partial class SearchRouteView : IViewFor<SearchRouteViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
       "ViewModel", typeof(SearchRouteViewModel), typeof(SearchRouteView), new PropertyMetadata(default(SearchRouteViewModel)));

        public SearchRouteViewModel ViewModel
        {
            get { return (SearchRouteViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public SearchRouteView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Routes, v => v.RoutesListView.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.OriginStopSuggestions, v => v.OriginStopComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.DestinationStopSuggestions, v => v.DestinationStopComboBox.ItemsSource);

            this.Bind(ViewModel, vm => vm.OriginStopFilter.StopNameFilter, v => v.OriginStopComboBox.Text);
            this.Bind(ViewModel, vm => vm.DestinationStopFilter.StopNameFilter, v => v.DestinationStopComboBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedOriginStop, v => v.OriginStopComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedDestinationStop, v => v.DestinationStopComboBox.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.FindRoutes, v => v.FindRoutesButton);

            this.WhenAnyObservable(v => v.ViewModel.UpdateOriginStopSuggestions)
                .Subscribe(_ => OriginStopComboBox.IsDropDownOpen = true);
            this.WhenAnyObservable(v => v.ViewModel.UpdateDestinationStopSuggestions)
                .Subscribe(_ => DestinationStopComboBox.IsDropDownOpen = true);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (SearchRouteViewModel)value;
            }
        }
    }
}
