using System.Windows;
using System.Windows.Controls;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for StreetView.xaml
    /// </summary>
    public partial class StreetView : UserControl, IViewFor<Street>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(Street), typeof(StreetView), new PropertyMetadata(default(Street)));

        public Street ViewModel
        {
            get { return (Street) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public StreetView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (Street) value; }
        }
    }
}
