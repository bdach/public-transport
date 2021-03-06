﻿using System.Windows;
using PublicTransport.Client.ViewModels.Edit;
using ReactiveUI;

namespace PublicTransport.Client.Views.Edit
{
    /// <summary>
    /// Interaction logic for EditCalendarView.xaml
    /// </summary>
    public partial class EditCalendarView : IViewFor<EditCalendarViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
            "ViewModel", typeof(EditCalendarViewModel), typeof(EditCalendarView), new PropertyMetadata(default(EditCalendarViewModel)));

        public EditCalendarViewModel ViewModel
        {
            get { return (EditCalendarViewModel) GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public EditCalendarView()
        {
            InitializeComponent();
            this.Bind(ViewModel, vm => vm.Calendar.Monday, v => v.MondayToggleButton.IsChecked);
            this.Bind(ViewModel, vm => vm.Calendar.Tuesday, v => v.TuesdayToggleButton.IsChecked);
            this.Bind(ViewModel, vm => vm.Calendar.Wednesday, v => v.WednesdayToggleButton.IsChecked);
            this.Bind(ViewModel, vm => vm.Calendar.Thursday, v => v.ThursdayToggleButton.IsChecked);
            this.Bind(ViewModel, vm => vm.Calendar.Friday, v => v.FridayToggleButton.IsChecked);
            this.Bind(ViewModel, vm => vm.Calendar.Saturday, v => v.SaturdayToggleButton.IsChecked);
            this.Bind(ViewModel, vm => vm.Calendar.Sunday, v => v.SundayToggleButton.IsChecked);
            this.Bind(ViewModel, vm => vm.Calendar.StartDate, v => v.StartDatePicker.SelectedDate);
            this.Bind(ViewModel, vm => vm.Calendar.EndDate, v => v.EndDatePicker.SelectedDate);
            this.BindCommand(ViewModel, vm => vm.Close, v => v.CloseButton);
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (EditCalendarViewModel) value; }
        }
    }
}
