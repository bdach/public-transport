using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditZoneView.xaml
    /// </summary>
    public partial class EditZoneView : IViewFor<EditZoneViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditZoneViewModel), typeof(EditZoneView), new PropertyMetadata(default(EditZoneViewModel)));

        public EditZoneViewModel ViewModel
        {
            get { return (EditZoneViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditZoneView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.Zone.Name, v => v.NameTextBox.Text);
            this.BindCommand(ViewModel, vm => vm.SaveZone, v => v.SaveButton);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditZoneViewModel)value; }
        }
    }
}
