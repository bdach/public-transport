using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    /// Interaction logic for StreetView.xaml
    /// </summary>
    public partial class StreetView : UserControl, IViewFor<StreetViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(StreetViewModel), typeof(StreetView), new PropertyMetadata(default(StreetViewModel)));

        public StreetViewModel ViewModel
        {
            get { return (StreetViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public StreetView()
        {
            InitializeComponent();
            this.BindCommand(ViewModel, vm => vm.DisplayCityView, v => v.ToCity);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (StreetViewModel) value; }
        }
    }
}
