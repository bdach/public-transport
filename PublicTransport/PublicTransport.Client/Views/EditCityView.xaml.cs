using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    ///     Interaction logic for EditCityView.xaml
    /// </summary>
    public partial class EditCityView : UserControl, IViewFor<EditCityViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditCityViewModel), typeof(EditCityView), new PropertyMetadata(default(EditCityViewModel)));

        public EditCityViewModel ViewModel
        {
            get { return (EditCityViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditCityView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.City.Name, v => v.NameTextBox.Text);
            this.BindCommand(ViewModel, vm => vm.SaveCity, v => v.SaveButton);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditCityViewModel)value; }
        }
    }
}