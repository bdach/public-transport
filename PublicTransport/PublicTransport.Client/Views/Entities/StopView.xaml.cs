using System.Windows;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for StopView.xaml
    /// </summary>
    public partial class StopView : IViewFor<Stop>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(Stop), typeof(StopView), new PropertyMetadata(default(Stop)));

        public Stop ViewModel
        {
            get { return (Stop)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public StopView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (Stop)value; }
        }
    }
}