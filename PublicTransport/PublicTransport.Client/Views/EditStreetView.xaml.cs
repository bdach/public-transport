using System;
using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    /// Interaction logic for EditStreetView.xaml
    /// </summary>
    public partial class EditStreetView : UserControl, IViewFor<EditStreetViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditStreetViewModel), typeof(EditStreetView), new PropertyMetadata(default(EditStreetViewModel)));

        public EditStreetViewModel ViewModel
        {
            get { return (EditStreetViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditStreetView()
        {
            InitializeComponent();
            this.BindCommand(ViewModel, vm => vm.DisplayCityView, v => v.ToCity);
            this.BindCommand(ViewModel, vm => vm.AddStreet, v => v.Save);
            this.Bind(ViewModel, vm => vm.Street.Name, v => v.Name.Text);
            this.Bind(ViewModel, vm => vm.CityName, v => v.City.Text);
            this.OneWayBind(ViewModel, vm => vm.Suggestions, v => v.City.ItemsSource);
            this.Bind(ViewModel, vm => vm.Street.City, v => v.City.SelectedItem);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set {
                ViewModel = (EditStreetViewModel) value;
                ViewModel.Suggestions.ItemsAdded.Subscribe(_ => City.IsDropDownOpen = true);
            }
        }
    }
}
