using System.Windows;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for ZoneView.xaml
    /// </summary>
    public partial class ZoneView : IViewFor<ZoneDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(ZoneDto), typeof(ZoneView), new PropertyMetadata(default(ZoneDto)));

        public ZoneView()
        {
            InitializeComponent();
        }

        public ZoneDto ViewModel
        {
            get { return (ZoneDto)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ZoneDto)value; }
        }
    }
}
