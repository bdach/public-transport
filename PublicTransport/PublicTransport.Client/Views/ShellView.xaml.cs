using System.Windows;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    ///     Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window, IViewFor<ShellViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(ShellViewModel), typeof(ShellView), new PropertyMetadata(default(ShellViewModel)));

        public ShellView(ShellViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            this.OneWayBind(ViewModel, vm => vm.MenuViewModel, v => v.Menu.ViewModel);
            this.OneWayBind(ViewModel, vm => vm.NotificationViewModel, v => v.NotificationPanel.ViewModel);
            this.Bind(ViewModel, vm => vm.HostScreen.Router, v => v.ContentView.Router);
        }

        public ShellViewModel ViewModel
        {
            get { return (ShellViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ShellViewModel) value; }
        }
    }
}