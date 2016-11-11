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
    ///     View model for adding <see cref="Domain.Entities.City" /> objects to the database.
    /// </summary>
    public class EditCityViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     The <see cref="Domain.Entities.City" /> object being edited in the window.
        /// </summary>
        private City _city;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="cityUnitOfWork">Unit of work exposing methods necessary to manage data.</param>
        /// <param name="city">City to be edited. If a city is to be added, this parameter is null (can be left out).</param>
        public EditCityViewModel(IScreen screen, ICityUnitOfWork cityUnitOfWork, City city = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            var serviceMethod = city == null ? new Func<City, City>(cityUnitOfWork.CreateCity) : cityUnitOfWork.UpdateCity;
            _city = city ?? new City();

            #endregion

            #region SaveCity command

            SaveCity = ReactiveCommand.CreateAsyncTask(async _ => { return await Task.Run(() => serviceMethod(City)); });
            SaveCity.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("The currently edited city cannot be saved to the database. Please contact the administrator.", ex));

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     Property for the <see cref="Domain.Entities.City" /> being edited in the window.
        /// </summary>
        public City City
        {
            get { return _city; }
            set { this.RaiseAndSetIfChanged(ref _city, value); }
        }

        /// <summary>
        ///     Command adding the <see cref="Domain.Entities.City" /> to the database.
        /// </summary>
        public ReactiveCommand<City> SaveCity { get; }

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
        public MenuOption AssociatedMenuOption => MenuOption.City;
    }
}