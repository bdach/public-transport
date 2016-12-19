using System.Windows;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for StopTimeView.xaml
    /// </summary>
    public partial class StopTimeView : IViewFor<StopTimeDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(StopTimeDto), typeof(StopTimeView), new PropertyMetadata(default(StopTimeDto)));

        public StopTimeDto ViewModel
        {
            get { return (StopTimeDto) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public StopTimeView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.ArrivalTime, v => v.ArrivalTextBlock.Text);
            // condition for equal times
            this.OneWayBind(ViewModel, vm => vm.DepartureTime, v => v.DepartureTextBlock.Text);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (StopTimeDto) value; }
        }
    }
}
