using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for RoleView.xaml
    /// </summary>
    public partial class RoleView : UserControl, IViewFor<RoleViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(RoleViewModel), typeof(RoleView), new PropertyMetadata(default(RoleViewModel)));

        public RoleViewModel ViewModel
        {
            get { return (RoleViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public RoleView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.Selected, v => v.SelectedCheckBox.IsChecked);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (RoleViewModel) value; }
        }
    }
}
