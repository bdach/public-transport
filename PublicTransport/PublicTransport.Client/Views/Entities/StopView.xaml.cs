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
            this.Bind(ViewModel, vm => vm.Name, v => v.StopNameTextBlock.Text);
            this.Bind(ViewModel, vm => vm.Street.Name, v => v.StreetNameTextBlock.Text);
            this.Bind(ViewModel, vm => vm.Street.City.Name, v => v.CityNameTextBlock.Text);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (StopDto)value; }
        }
    }
}