using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    /// <summary>
    ///     View model for adding <see cref="Domain.Entities.City" /> objects to the database.
    /// </summary>
    public class EditCityViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     <see cref="CityService" /> used for persisting the object.
        /// </summary>
        private readonly CityService _cityService;

        /// <summary>
        ///     The <see cref="City" /> object being edited in the window.
        /// </summary>
        private City _city;

        /// <summary>
        ///     The status of the operation.
        ///     TODO: Move this to a Notification view model.
        /// </summary>
        private string _status;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        public EditCityViewModel(IScreen screen)
        {
            #region Field/property initialization

            HostScreen = screen;
            _city = new City();
            _cityService = new CityService();

            #endregion

            #region AddCity command

            // Action: Use the service to save to the database.
            AddCity = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var city = await Task.Run(() => _cityService.Create(City));
                return city;
            });
            // On completion: Display message.
            AddCity.Subscribe(city => Status = $"City {city.Name} saved to database.");
            // On exceptions: Display error.
            // TODO: This should be handled somehow.
            AddCity.ThrownExceptions.Subscribe(ex => UserError.Throw("Cannot connect to database", ex));

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion

            #region Notification hiding

            // Upon displaying the status, wait three seconds and clear it.
            this.WhenAnyValue(vm => vm.Status)
                .Where(s => s.Length > 0)
                .Throttle(TimeSpan.FromSeconds(3), RxApp.MainThreadScheduler)
                .Subscribe(s => Status = "");

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
        ///     Property for the status field.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        /// <summary>
        ///     Command adding the <see cref="City" /> to the database.
        /// </summary>
        public ReactiveCommand<City> AddCity { get; }

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