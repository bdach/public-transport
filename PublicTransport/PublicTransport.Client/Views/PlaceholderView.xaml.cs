using System.Windows;
using System.Windows.Controls;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    ///     Interaction logic for PlaceholderView.xaml
    /// </summary>
    public partial class PlaceholderView : UserControl, IViewFor<PlaceholderViewModel>
    {
        public static readonly DependencyProperty PlaceholderViewModelProperty = DependencyProperty.Register(
            "PlaceholderViewModel", typeof(PlaceholderViewModel), typeof(PlaceholderView),
            new PropertyMetadata(default(PlaceholderViewModel)));

        public PlaceholderView()
        {
            InitializeComponent();
        }

        public PlaceholderViewModel PlaceholderViewModel
        {
            get { return (PlaceholderViewModel) GetValue(PlaceholderViewModelProperty); }
            set { SetValue(PlaceholderViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (PlaceholderViewModel) value; }
        }

        public PlaceholderViewModel ViewModel { get; set; }
    }
}