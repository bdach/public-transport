using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for StopTimeView.xaml
    /// </summary>
    public partial class StopTimeView : UserControl, IViewFor<StopTime>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(StopTime), typeof(StopTimeView), new PropertyMetadata(default(StopTime)));

        public StopTime ViewModel
        {
            get { return (StopTime) GetValue(ViewModelProperty); }
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
            set { ViewModel = (StopTime) value; }
        }
    }
}
