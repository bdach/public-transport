using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    /// Interaction logic for CityView.xaml
    /// </summary>
    public partial class CityView : UserControl, IViewFor<CityViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(CityViewModel), typeof(CityView), new PropertyMetadata(default(CityViewModel)));

        public CityViewModel ViewModel
        {
            get { return (CityViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public CityView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.City.Name, v => v.Name.Text);
            this.Bind(ViewModel, vm => vm.Status, v => v.Status.Text);
            this.BindCommand(ViewModel, vm => vm.AddCity, v => v.Save);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (CityViewModel)value; }
        }
    }
}
