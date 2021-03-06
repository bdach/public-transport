﻿using System.Windows;
using PublicTransport.Client.ViewModels.Filter;
using ReactiveUI;

namespace PublicTransport.Client.Views.Filter
{
    /// <summary>
    /// Interaction logic for FilterCityView.xaml
    /// </summary>
    public partial class FilterCityView : IViewFor<FilterCityViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(FilterCityViewModel), typeof(FilterCityView), new PropertyMetadata(default(FilterCityViewModel)));

        public FilterCityViewModel ViewModel
        {
            get { return (FilterCityViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public FilterCityView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.Cities, v => v.CitiesListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.NameFilter, v => v.NameFilterTextBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedCity, v => v.CitiesListView.SelectedItem);
            this.BindCommand(ViewModel, vm => vm.AddCity, v => v.AddCityButton);
            this.BindCommand(ViewModel, vm => vm.EditCity, v => v.EditCityButton);
            this.BindCommand(ViewModel, vm => vm.DeleteCity, v => v.DeleteCityButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (FilterCityViewModel) value; }
        }
    }
}
