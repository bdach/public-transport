using System;
using System.Reactive;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Zones;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for adding <see cref="Domain.Entities.Zone" /> objects to the database.
    /// </summary>
    public class EditZoneViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     The <see cref="ZoneDto" /> object being edited in the window.
        /// </summary>
        private ZoneDto _zone;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="zoneService">Service exposing methods necessary to manage data.</param>
        /// <param name="zone">Zone to be edited. If a zone is to be added, this parameter is null (can be left out).</param>
        public EditZoneViewModel(IScreen screen, IZoneService zoneService, ZoneDto zone = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            var serviceMethod = zone == null ? new Func<ZoneDto, Task<ZoneDto>>(zoneService.CreateZoneAsync) : zoneService.UpdateZoneAsync;
            _zone = zone ?? new ZoneDto();

            #endregion

            #region SaveZone command

            SaveZone = ReactiveCommand.CreateAsyncTask(async _ => await serviceMethod(Zone));
            SaveZone.ThrownExceptions
                .Where(ex => !(ex is FaultException<ValidationFault>))
                .Subscribe(ex =>
                    UserError.Throw("Cannot connect to the server. Please try again later.", ex));
            SaveZone.ThrownExceptions
                .Where(ex => ex is FaultException<ValidationFault>)
                .Select(ex => ex as FaultException<ValidationFault>)
                .Subscribe(ex => UserError.Throw(string.Join("\n", ex.Detail.Errors), ex));

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     Property for the <see cref="ZoneDto" /> being edited in the window.
        /// </summary>
        public ZoneDto Zone
        {
            get { return _zone; }
            set { this.RaiseAndSetIfChanged(ref _zone, value); }
        }

        /// <summary>
        ///     Command adding the <see cref="ZoneDto" /> to the database.
        /// </summary>
        public ReactiveCommand<ZoneDto> SaveZone { get; }

        /// <summary>
        ///     Command closing the current detail view model.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; }

        /// <summary>
        ///     String uniquely identifying the current view model.
        /// </summary>
        public string UrlPathSegment => AssociatedMenuOption.ToString();

        /// <summary>
        ///     Host screen to display on.
        /// </summary>
        public IScreen HostScreen { get; }

        /// <summary>
        ///     Gets the <see cref="MenuOption" /> enum value that associates a menu item with the concrete view model.
        /// </summary>
        public MenuOption AssociatedMenuOption => MenuOption.Zone;
    }
}
