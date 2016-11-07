using System.Windows;
using System.Windows.Controls;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for ZoneView.xaml
    /// </summary>
    public partial class ZoneView : UserControl, IViewFor<Zone>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(Zone), typeof(ZoneView), new PropertyMetadata(default(Zone)));

        public ZoneView()
        {
            InitializeComponent();
        }

        public Zone ViewModel
        {
            get { return (Zone)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (Zone)value; }
        }
    }
}
