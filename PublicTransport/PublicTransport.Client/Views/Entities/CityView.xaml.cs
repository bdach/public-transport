using System.Windows;
using System.Windows.Controls;
using PublicTransport.Domain.Entities;
using ReactiveUI;

namespace PublicTransport.Client.Views
{
    /// <summary>
    ///     Interaction logic for CityView.xaml
    /// </summary>
    public partial class CityView : UserControl, IViewFor<City>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(City), typeof(CityView), new PropertyMetadata(default(City)));

        public CityView()
        {
            InitializeComponent();
        }

        public City ViewModel
        {
            get { return (City) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (City) value; }
        }
    }
}