using System.Reactive;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;
using System;

namespace PublicTransport.Client.ViewModels.Edit
{
    public class EditCalendarViewModel : ReactiveObject, IDetailViewModel
    {
        private Calendar _calendar;

        public EditCalendarViewModel(IScreen screen, Calendar calendar)
        {
            #region Field/property initialization

            HostScreen = screen;
            _calendar = calendar;

            #endregion
            
            #region Close command

            Close = ReactiveCommand.CreateAsyncObservable(_ =>
            {
                return HostScreen.Router.NavigateBack.ExecuteAsync();
            });

            #endregion
        }
        
        public ReactiveCommand<Unit> Close { get; protected set; }

        public Calendar Calendar
        {
            get { return _calendar; }
            set { this.RaiseAndSetIfChanged(ref _calendar, value); }
        }

        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public MenuOption AssociatedMenuOption => MenuOption.Trip;
    }
}