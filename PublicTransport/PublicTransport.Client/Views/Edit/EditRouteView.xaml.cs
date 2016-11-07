using System;
using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditRouteView.xaml
    /// </summary>
    public partial class EditRouteView : UserControl, IViewFor<EditRouteViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditRouteViewModel), typeof(EditRouteView), new PropertyMetadata(default(EditRouteViewModel)));

        public EditRouteViewModel ViewModel
        {
            get { return (EditRouteViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditRouteView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.AgencySuggestions, v => v.AgencyComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.RouteTypes, v => v.RouteTypesComboBox.ItemsSource);
            this.Bind(ViewModel, vm => vm.AgencyFilter.AgencyNameFilter, v => v.AgencyComboBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedAgency, v => v.AgencyComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.Route.ShortName, v => v.ShortNameTextBox.Text);
            this.Bind(ViewModel, vm => vm.Route.LongName, v => v.LongNameTextBox.Text);
            this.Bind(ViewModel, vm => vm.Route.RouteType, v => v.RouteTypesComboBox.SelectedItem);
            // TODO: Commented out due to not working properly as-is; I don't want to include the whole object hierarchy
            //this.BindCommand(ViewModel, vm => vm.DisplayAgencyView, v => v.ToAgencyButton);
            this.BindCommand(ViewModel, vm => vm.SaveRoute, v => v.SaveButton);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            this.WhenAnyObservable(v => v.ViewModel.UpdateSuggestions)
                .Subscribe(_ => AgencyComboBox.IsDropDownOpen = true);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditRouteViewModel) value; }
        }
    }
}
