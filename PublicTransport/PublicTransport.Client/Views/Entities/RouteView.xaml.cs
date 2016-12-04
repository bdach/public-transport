using System.Windows;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for RouteView.xaml
    /// </summary>
    public partial class RouteView : IViewFor<RouteDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(RouteDto), typeof(RouteView), new PropertyMetadata(default(RouteDto)));

        public RouteDto ViewModel
        {
            get { return (RouteDto) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public RouteView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.ShortName, v => v.ShortNameTextBlock.Text);
            this.Bind(ViewModel, vm => vm.LongName, v => v.LongNameTextBlock.Text);
            this.Bind(ViewModel, vm => vm.Agency.Name, v => v.AgencyNameTextBlock.Text);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (RouteDto) value; }
        }
    }
}
