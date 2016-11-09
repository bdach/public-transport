using System;
using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditFareView.xaml
    /// </summary>
    public partial class EditFareView : IViewFor<EditFareViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditFareViewModel), typeof(EditFareView), new PropertyMetadata(default(EditFareViewModel)));

        public EditFareView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.RouteSuggestions, v => v.RouteComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.OriginZoneSuggestions, v => v.OriginZoneComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.DestinationZoneSuggestions, v => v.DestinationZoneComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.TransferCounts, v => v.TransferCountComboBox.ItemsSource);

            this.Bind(ViewModel, vm => vm.RouteFilter.ShortNameFilter, v => v.RouteComboBox.Text);
            this.Bind(ViewModel, vm => vm.OriginZoneFilter, v => v.OriginZoneComboBox.Text);
            this.Bind(ViewModel, vm => vm.DestinationZoneFilter, v => v.DestinationZoneComboBox.Text);

            this.Bind(ViewModel, vm => vm.FareRule.Route, v => v.RouteComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.FareRule.Origin, v => v.OriginZoneComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.FareRule.Destination, v => v.DestinationZoneComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.FareAttribute.Price, v => v.PriceTextBox.Text);
            this.Bind(ViewModel, vm => vm.FareAttribute.Transfers, v => v.TransferCountComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.FareAttribute.TransferDuration, v => v.TransferDurationTextBox.Text);

            this.Bind(ViewModel, vm => vm.SelectedRoute, v => v.RouteComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedOriginZone, v => v.OriginZoneComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedDestinationZone, v => v.DestinationZoneComboBox.SelectedItem);

            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            this.BindCommand(ViewModel, vm => vm.SaveFare, v => v.SaveButton);

            this.WhenAnyObservable(v => v.ViewModel.UpdateRouteSuggestions)
                .Subscribe(_ => RouteComboBox.IsDropDownOpen = true);
            this.WhenAnyObservable(v => v.ViewModel.UpdateOriginZoneSuggestions)
                .Subscribe(_ => OriginZoneComboBox.IsDropDownOpen = true);
            this.WhenAnyObservable(v => v.ViewModel.UpdateDestinationZoneSuggestions)
                .Subscribe(_ => DestinationZoneComboBox.IsDropDownOpen = true);
        }

        public EditFareViewModel ViewModel
        {
            get { return (EditFareViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditFareViewModel)value; }
        }
    }
}
