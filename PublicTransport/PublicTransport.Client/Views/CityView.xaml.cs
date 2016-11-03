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
        public static readonly DependencyProperty CityProperty = DependencyProperty.Register(
            "City", typeof(City), typeof(CityView), new PropertyMetadata(default(City)));

        public CityView()
        {
            InitializeComponent();
            this.Bind(City, c => c.Name, v => v.Name.Text);
        }

        public City City
        {
            get { return (City) GetValue(CityProperty); }
            set { SetValue(CityProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (City) value; }
        }

        public City ViewModel { get; set; }
    }
}