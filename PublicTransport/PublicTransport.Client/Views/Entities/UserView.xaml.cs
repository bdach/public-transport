using System.Windows;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : IViewFor<UserDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(UserDto), typeof(UserView), new PropertyMetadata(default(UserDto)));

        public UserDto ViewModel
        {
            get { return (UserDto) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public UserView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (UserDto) value; }
        }
    }
}
