using System.Reactive;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    public class EditCalendarViewModel : ReactiveObject, IDetailViewModel
    {
        private CalendarDto _calendar;

        public EditCalendarViewModel(IScreen screen, CalendarDto calendar)
        {
            #region Field/property initialization

            HostScreen = screen;
            _calendar = calendar;

            #endregion
            
            #region Close command

            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }
        
        public ReactiveCommand<Unit> Close { get; protected set; }

        public CalendarDto Calendar
        {
            get { return _calendar; }
            set { this.RaiseAndSetIfChanged(ref _calendar, value); }
        }

        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public MenuOption AssociatedMenuOption => MenuOption.Trip;
    }
}