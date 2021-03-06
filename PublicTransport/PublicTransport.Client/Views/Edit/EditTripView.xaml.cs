﻿using System;
using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditTripView.xaml
    /// </summary>
    public partial class EditTripView : IViewFor<EditTripViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditTripViewModel), typeof(EditTripView), new PropertyMetadata(default(EditTripViewModel)));

        public EditTripViewModel ViewModel
        {
            get { return (EditTripViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditTripView()
        {
            InitializeComponent();
            this.OneWayBind(ViewModel, vm => vm.RouteSuggestions, v => v.RouteComboBox.ItemsSource);
            this.OneWayBind(ViewModel, vm => vm.StopTimes, v => v.StopsListView.ItemsSource);
            this.Bind(ViewModel, vm => vm.SelectedRoute, v => v.RouteComboBox.SelectedItem);
            this.Bind(ViewModel, vm => vm.RouteReactiveFilter.ShortNameFilter, v => v.RouteComboBox.Text);
            this.Bind(ViewModel, vm => vm.Trip.ShortName, v => v.ShortNameTextBox.Text);
            this.Bind(ViewModel, vm => vm.Trip.Headsign, v => v.HeadsignTextBox.Text);
            this.Bind(ViewModel, vm => vm.SelectedStopTime, v => v.StopsListView.SelectedItem);
            // TODO: Saving trip without arrival/departure times causes no saving and no notification about error.
            this.BindCommand(ViewModel, vm => vm.SaveTrip, v => v.SaveButton);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
            this.BindCommand(ViewModel, vm => vm.NavigateToCalendar, v => v.EditScheduleButton);
            this.BindCommand(ViewModel, vm => vm.AddStop, v => v.AddStopButton);
            this.BindCommand(ViewModel, vm => vm.DeleteStop, v => v.DeleteStopButton);

            this.WhenAnyObservable(v => v.ViewModel.UpdateSuggestions)
                .Subscribe(_ => RouteComboBox.IsDropDownOpen = true);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditTripViewModel) value; }
        }
    }
}
