using System.Windows;
using System.Windows.Controls;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : UserControl, IViewFor<User>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(User), typeof(UserView), new PropertyMetadata(default(User)));

        public User ViewModel
        {
            get { return (User) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public UserView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (User) value; }
        }
    }
}
