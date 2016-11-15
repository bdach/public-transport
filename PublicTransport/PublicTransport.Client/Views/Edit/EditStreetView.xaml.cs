using System;
using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditStreetView.xaml
    /// </summary>
    public partial class EditStreetView : IViewFor<EditStreetViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditStreetViewModel), typeof(EditStreetView), new PropertyMetadata(default(EditStreetViewModel)));

        public EditStreetViewModel ViewModel
        {
            get { return (EditStreetViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditStreetView()
        {
            InitializeComponent();
            this.BindCommand(ViewModel, vm => vm.SaveStreet, v => v.Save);
            this.Bind(ViewModel, vm => vm.Street.Name, v => v.StreetName.Text);
            this.Bind(ViewModel, vm => vm.CityName, v => v.City.Text);
            this.OneWayBind(ViewModel, vm => vm.Suggestions, v => v.City.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedCity, v => v.City.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.Close);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (EditStreetViewModel)value;
                ViewModel.Suggestions.ItemsAdded.Subscribe(_ => City.IsDropDownOpen = true);
            }
        }
    }
}
