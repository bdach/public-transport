using System.Windows;
using System.Windows.Controls;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for RouteView.xaml
    /// </summary>
    public partial class RouteView : UserControl, IViewFor<Route>
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
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (Route) value; }
        }
    }
}
