using System.Windows;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for AgencyView.xaml
    /// </summary>
    public partial class AgencyView : IViewFor<Agency>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(Agency), typeof(AgencyView), new PropertyMetadata(default(Agency)));

        public Agency ViewModel
        {
            get { return (Agency) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public AgencyView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (Agency) value; }
        }
    }
}
