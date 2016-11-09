using System.Windows;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : IViewFor<LoginViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(LoginViewModel), typeof(LoginView), new PropertyMetadata(default(LoginViewModel)));

        public LoginViewModel ViewModel
        {
            get { return (LoginViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public LoginView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.Username, v => v.UserNameTextBox.Text);
            this.BindCommand(ViewModel, vm => vm.SendLoginRequest, v => v.LoginButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set
            {
                ViewModel = (LoginViewModel) value;
                ViewModel.PasswordBox = PasswordBox;
            }
        }
    }
}
