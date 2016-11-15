using System.Windows;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for RouteView.xaml
    /// </summary>
    public partial class RouteView : IViewFor<Route>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(Route), typeof(RouteView), new PropertyMetadata(default(Route)));

        public Route ViewModel
        {
            get { return (Route) GetValue(ViewModelProperty); }
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
            set { ViewModel = (Route) value; }
        }
    }
}
