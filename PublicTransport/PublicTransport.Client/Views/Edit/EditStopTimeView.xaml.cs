﻿using System;
using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditStopTimeView.xaml
    /// </summary>
    public partial class EditStopTimeView : IViewFor<EditStopTimeViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditStopTimeViewModel), typeof(EditStopTimeView), new PropertyMetadata(default(EditStopTimeViewModel)));

        public EditStopTimeViewModel ViewModel
        {
            get { return (EditStopTimeViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditStopTimeView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.StopSuggestions, v => v.StopComboBox.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedStop, v => v.StopComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.StopReactiveFilter.StopNameFilter, v => v.StopComboBox.Text);
            this.Bind(ViewModel, vm => vm.StopTime.ArrivalTime, v => v.ArrivalTimePicker.SelectedTime);
            this.Bind(ViewModel, vm => vm.StopTime.DepartureTime, v => v.DepartureTimePicker.SelectedTime);

            this.WhenAnyObservable(v => v.ViewModel.UpdateSuggestions)
                .Subscribe(_ => StopComboBox.IsDropDownOpen = true);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditStopTimeViewModel) value; }
        }
    }
}
