using System;
using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditAgencyView.xaml
    /// </summary>
    public partial class EditAgencyView : IViewFor<EditAgencyViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditAgencyViewModel), typeof(EditAgencyView), new PropertyMetadata(default(EditAgencyViewModel)));

        public EditAgencyViewModel ViewModel
        {
            get { return (EditAgencyViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditAgencyView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.StreetSuggestions, v => v.StreetComboBox.ItemsSource);
            this.Bind(ViewModel, vm => vm.StreetFilter.StreetNameFilter, v => v.StreetComboBox.Text);
            this.Bind(ViewModel, vm => vm.Agency.Name, v => v.NameTextBox.Text);
            this.Bind(ViewModel, vm => vm.Agency.Phone, v => v.PhoneNumberTextBox.Text);
            this.Bind(ViewModel, vm => vm.Agency.Regon, v => v.RegonTextBox.Text);
            this.Bind(ViewModel, vm => vm.Agency.StreetNumber, v => v.StreetNumberTextBox.Text);
            this.Bind(ViewModel, vm => vm.Agency.Url, v => v.UrlTextBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedStreet, v => v.StreetComboBox.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            this.BindCommand(ViewModel, vm => vm.SaveAgency, v => v.SaveButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set {
                ViewModel = (EditAgencyViewModel) value;
                ViewModel.StreetSuggestions.ItemsAdded.Subscribe(_ => StreetComboBox.IsDropDownOpen = true);
            }
        }
    }
}
