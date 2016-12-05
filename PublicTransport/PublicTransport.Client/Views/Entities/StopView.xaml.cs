using System.Windows;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for StopView.xaml
    /// </summary>
    public partial class StopView : IViewFor<StopDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(StopDto), typeof(StopView), new PropertyMetadata(default(StopDto)));

        public StopDto ViewModel
        {
            get { return (StopDto)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public StopView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (StopDto)value; }
        }
    }
}