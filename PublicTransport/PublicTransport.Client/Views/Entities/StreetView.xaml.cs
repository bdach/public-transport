using System.Windows;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for StreetView.xaml
    /// </summary>
    public partial class StreetView : IViewFor<StreetDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(StreetDto), typeof(StreetView), new PropertyMetadata(default(StreetDto)));

        public StreetDto ViewModel
        {
            get { return (StreetDto) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public StreetView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (StreetDto) value; }
        }
    }
}
