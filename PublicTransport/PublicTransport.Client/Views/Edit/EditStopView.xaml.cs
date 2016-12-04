using System;
using System.Reactive.Linq;
using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditStopView.xaml
    /// </summary>
    public partial class EditStopView : IViewFor<EditStopViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditStopViewModel), typeof(EditStopView), new PropertyMetadata(default(EditStopViewModel)));

        public EditStopView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.StreetSuggestions, v => v.StreetComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ZoneSuggestions, v => v.ZoneComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.ParentStationSuggestions, v => v.ParentStationComboBox.ItemsSource);
            this.Bind(ViewModel, vm => vm.StreetReactiveFilter.StreetNameFilter, v => v.StreetComboBox.Text);
            this.Bind(ViewModel, vm => vm.ZoneFilter, v => v.ZoneComboBox.Text);
            this.Bind(ViewModel, vm => vm.ParentStationReactiveFilter.StopNameFilter, v => v.ParentStationComboBox.Text);

            this.Bind(ViewModel, vm => vm.Stop.Name, v => v.NameTextBox.Text);
            this.Bind(ViewModel, vm => vm.Stop.Street, v => v.StreetComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.Stop.Zone, v => v.ZoneComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.Stop.ParentStation, v => v.ParentStationComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.Stop.IsStation, v => v.IsStationCheckBox.IsChecked);

            this.Bind(ViewModel, vm => vm.SelectedStreet, v => v.StreetComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedZone, v => v.ZoneComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedParentStation, v => v.ParentStationComboBox.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            this.BindCommand(ViewModel, vm => vm.SaveStop, v => v.SaveButton);

            this.WhenAnyValue(v => v.IsStationCheckBox.IsChecked)
                .Where(b => b.HasValue)
                .Select(b => b.Value)
                .Subscribe(b =>
                {
                    ParentStationComboBox.IsEnabled = !b;
                    ParentStationComboBox.SelectedItem = null;
                });

            this.WhenAnyObservable(v => v.ViewModel.UpdateStreetSuggestions)
                .Subscribe(_ => StreetComboBox.IsDropDownOpen = true);
            this.WhenAnyObservable(v => v.ViewModel.UpdateZoneSuggestions)
                .Subscribe(_ => ZoneComboBox.IsDropDownOpen = true);
            this.WhenAnyObservable(v => v.ViewModel.UpdateParentStationSuggestions)
                .Subscribe(_ => ParentStationComboBox.IsDropDownOpen = true);
        }

        public EditStopViewModel ViewModel
        {
            get { return (EditStopViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditStopViewModel)value; }
        }
    }
}
