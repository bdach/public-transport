using System.Windows;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.Views.Entities
{
    /// <summary>
    ///     Interaction logic for CityView.xaml
    /// </summary>
    public partial class CityView : IViewFor<CityDto>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(CityDto), typeof(CityView), new PropertyMetadata(default(CityDto)));

        public CityView()
        {
            InitializeComponent();
        }

        public CityDto ViewModel
        {
            get { return (CityDto) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (CityDto) value; }
        }
    }
}