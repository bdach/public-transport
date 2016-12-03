using System.Windows;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    /// Interaction logic for AgencyView.xaml
    /// </summary>
    public partial class AgencyView : IViewFor<AgencyDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(AgencyDto), typeof(AgencyView), new PropertyMetadata(default(AgencyDto)));

        public AgencyDto ViewModel
        {
            get { return (AgencyDto) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public AgencyView()
        {
            InitializeComponent();
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (AgencyDto) value; }
        }
    }
}
