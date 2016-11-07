using System.Windows;
using System.Windows.Controls;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for FareAttributeView.xaml
    /// </summary>
    public partial class FareAttributeView : UserControl, IViewFor<FareAttribute>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FareAttribute), typeof(FareAttributeView), new PropertyMetadata(default(FareAttribute)));

        public FareAttributeView()
        {
            InitializeComponent();
        }

        public FareAttribute ViewModel
        {
            get { return (FareAttribute)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FareAttribute)value; }
        }
    }
}
