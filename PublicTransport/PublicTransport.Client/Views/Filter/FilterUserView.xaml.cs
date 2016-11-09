using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Views.Filter
{
    /// <summary>
    /// Interaction logic for FilterUserView.xaml
    /// </summary>
    public partial class FilterUserView : UserControl, IViewFor<FilterUserViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterUserViewModel), typeof(FilterUserView), new PropertyMetadata(default(FilterUserViewModel)));

        public FilterUserViewModel ViewModel
        {
            get { return (FilterUserViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public FilterUserView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Users, v => v.UsersListView.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.Roles, v => v.RoleTypesComboBox.ItemsSource);
            this.Bind(ViewModel, vm => vm.UserFilter.UserNameFilter, v => v.UserNameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.UserFilter.RoleNameFilter, v => v.RoleTypesComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.SelectedUser, v => v.UsersListView.SelectedItem);

            this.BindCommand(ViewModel, vm => vm.AddUser, v => v.AddUserButton);
            this.BindCommand(ViewModel, vm => vm.EditUser, v => v.EditUserButton);
            this.BindCommand(ViewModel, vm => vm.DeleteUser, v => v.DeleteUserButton);
            this.BindCommand(ViewModel, vm => vm.ClearRoleTypeChoice, v => v.ClearRoleTypeButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterUserViewModel) value; }
        }
    }
}
