using System;
using System.Reactive;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for adding <see cref="Domain.Entities.Zone" /> objects to the database.
    /// </summary>
    public class EditZoneViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     The <see cref="Zone" /> object being edited in the window.
        /// </summary>
        private Zone _zone;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="zoneUnitOfWork">Unit of work exposing methods necessary to manage data.</param>
        /// <param name="zone">Zone to be edited. If a zone is to be added, this parameter is null (can be left out).</param>
        public EditZoneViewModel(IScreen screen, IZoneUnitOfWork zoneUnitOfWork, Zone zone = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            var serviceMethod = zone == null ? new Func<Zone, Zone>(zoneUnitOfWork.CreateZone) : zoneUnitOfWork.UpdateZone;
            _zone = zone ?? new Zone();

            #endregion

            #region SaveZone command

            SaveZone = ReactiveCommand.CreateAsyncTask(async _ => { return await Task.Run(() => serviceMethod(Zone)); });
            SaveZone.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("The currently edited zone cannot be saved to the database. Please check the required fields and try again later.", ex));

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     Property for the <see cref="Domain.Entities.Zone" /> being edited in the window.
        /// </summary>
        public Zone Zone
        {
            get { return _zone; }
            set { this.RaiseAndSetIfChanged(ref _zone, value); }
        }

        /// <summary>
        ///     Command adding the <see cref="Zone" /> to the database.
        /// </summary>
        public ReactiveCommand<Zone> SaveZone { get; }

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
