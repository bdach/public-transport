using System.Windows;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for FareAttributeView.xaml
    /// </summary>
    public partial class FareAttributeView : IViewFor<FareAttributeDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FareAttributeDto), typeof(FareAttributeView), new PropertyMetadata(default(FareAttributeDto)));

        public FareAttributeView()
        {
            InitializeComponent();
        }

        public FareAttributeDto ViewModel
        {
            get { return (FareAttributeDto)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FareAttributeDto)value; }
        }
    }
}
