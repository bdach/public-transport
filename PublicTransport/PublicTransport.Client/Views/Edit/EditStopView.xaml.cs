using System;
using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditStopView.xaml
    /// </summary>
    public partial class EditStopView : UserControl, IViewFor<EditStopViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditStopViewModel), typeof(EditStopView), new PropertyMetadata(default(EditStopViewModel)));

        public EditStopView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.StreetSuggestions, v => v.StreetComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ZoneSuggestions, v => v.ZoneComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ParentStationSuggestions, v => v.ParentStationComboBox.ItemsSource);
            this.Bind(ViewModel, vm => vm.StreetFilter.StreetNameFilter, v => v.StreetComboBox.Text);
            this.Bind(ViewModel, vm => vm.ZoneFilter, v => v.ZoneComboBox.Text);
            this.Bind(ViewModel, vm => vm.ParentStationFilter.StopNameFilter, v => v.ParentStationComboBox.Text);

            this.Bind(ViewModel, vm => vm.Stop.Name, v => v.NameTextBox.Text);
            this.Bind(ViewModel, vm => vm.Stop.Street, v => v.StreetComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.Stop.Zone, v => v.ZoneComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.Stop.ParentStation, v => v.ParentStationComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.Stop.IsStation, v => v.IsStationCheckBox.IsChecked);

            this.Bind(ViewModel, vm => vm.SelectedStreet, v => v.StreetComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedZone, v => v.ZoneComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedParentStation, v => v.ParentStationComboBox.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            this.BindCommand(ViewModel, vm => vm.DisplayStreetView, v => v.ToStreetButton);
            this.BindCommand(ViewModel, vm => vm.SaveStop, v => v.SaveButton);
        }

        public EditStopViewModel ViewModel
        {
            get { return (EditStopViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (EditStopViewModel)value;
                ViewModel.StreetSuggestions.ItemsAdded.Subscribe(_ => StreetComboBox.IsDropDownOpen = true);
                ViewModel.ZoneSuggestions.ItemsAdded.Subscribe(_ => ZoneComboBox.IsDropDownOpen = true);
                ViewModel.ParentStationSuggestions.ItemsAdded.Subscribe(_ => ParentStationComboBox.IsDropDownOpen = true);
            }
        }
    }
}
