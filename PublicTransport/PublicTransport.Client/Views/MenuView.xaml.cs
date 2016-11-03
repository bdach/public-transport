using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    ///     Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl, IViewFor<MenuViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(MenuViewModel), typeof(MenuView), new PropertyMetadata(default(MenuViewModel)));

        public MenuView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Menu, v => v.SidebarMenu.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedOption, v => v.SidebarMenu.SelectedValue);
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