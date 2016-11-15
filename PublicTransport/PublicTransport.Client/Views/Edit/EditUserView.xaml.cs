using System;
using System.Reactive.Linq;
using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditUserView.xaml
    /// </summary>
    public partial class EditUserView : IViewFor<EditUserViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditUserViewModel), typeof(EditUserView), new PropertyMetadata(default(EditUserViewModel)));

        public EditUserViewModel ViewModel
        {
            get { return (EditUserViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditUserView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.User.UserName, v => v.UserNameTextBox.Text);
            this.Bind(ViewModel, vm => vm.User.Password, v => v.PasswordTextBox.Text);
            this.OneWayBind(ViewModel, vm => vm.RoleViewModels, v => v.RolesListBox.ItemsSource);

            this.BindCommand(ViewModel, vm => vm.SaveUser, v => v.SaveButton);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);

            this.WhenAnyValue(x => x.ViewModel.GetRoles)
                .SelectMany(x => x.ExecuteAsync())
                .Subscribe();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditUserViewModel)value; }
        }
    }
}
