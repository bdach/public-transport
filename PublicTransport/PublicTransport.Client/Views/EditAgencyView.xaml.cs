using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    /// Interaction logic for EditAgencyView.xaml
    /// </summary>
    public partial class EditAgencyView : UserControl, IViewFor<EditAgencyViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditAgencyViewModel), typeof(EditAgencyView), new PropertyMetadata(default(EditAgencyViewModel)));

        public EditAgencyViewModel ViewModel
        {
            get { return (EditAgencyViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditAgencyView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditAgencyViewModel) value; }
        }
    }
}
