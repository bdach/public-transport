using System;
using System.Reactive.Linq;
using System.Windows;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    ///     Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : IViewFor<MenuViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(MenuViewModel), typeof(MenuView), new PropertyMetadata(default(MenuViewModel)));

        public MenuView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Menu, v => v.SidebarMenu.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedOption, v => v.SidebarMenu.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.LogOut, v => v.LogOutButton);
            this.WhenAnyValue(v => v.ViewModel.UserInfo)
                .Select(ui => ui == null)
                .Subscribe(b => Visibility = b ? Visibility.Collapsed : Visibility.Visible);
        }

        public MenuViewModel ViewModel
        {
            get { return (MenuViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MenuViewModel) value; }
        }
    }
}