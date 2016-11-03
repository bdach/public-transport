using System;
using System.Collections.Generic;
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
    ///     View model for saving <see cref="Domain.Entities.Street" /> objects to the database.
    /// </summary>
    public class EditStreetViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     <see cref="CityService" /> used to fetch suggestions.
        /// </summary>
        private readonly CityService _cityService;

        /// <summary>
        ///     <see cref="StreetService" /> used for saving the <see cref="Street" /> object.
        /// </summary>
        private readonly StreetService _streetService;

        /// <summary>
        ///     City name supplied by the user. This field is used to supply suggestions.
        /// </summary>
        private string _cityName;

        /// <summary>
        ///     <see cref="Domain.Entities.Street" /> objects to be saved to the database.
        /// </summary>
        private Street _street;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        public EditStreetViewModel(IScreen screen)
        {
            // This is added to fix problems with ReactiveList.AddRange (multiple notifications firing)
            RxApp.SupportsRangeNotifications = false;

            #region Field/property initialization

            HostScreen = screen;
            Suggestions = new ReactiveList<City>();
            _cityService = new CityService();
            _streetService = new StreetService();
            _street = new Street();

            #endregion

            #region DisplayCityView command

            DisplayCityView = ReactiveCommand.Create();
            DisplayCityView.Subscribe(_ => { HostScreen.Router.Navigate.Execute(new EditCityViewModel(screen)); });

            #endregion

            #region AddStreet command

            AddStreet = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                Street.CityId = Street.City?.Id ?? 0;
                Street.City = null;
                return await Task.Run(() => _streetService.Create(Street));
            });

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var suggestions = await Task.Run(() => _cityService.GetCitiesContainingString(CityName));
                return suggestions;
            });
            UpdateSuggestions.Subscribe(results =>
            {
                Suggestions.Clear();
                Suggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions
                .Subscribe(ex => UserError.Throw("Cannot connect to database"));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.CityName)
                .Where(e => e != Street?.City?.Name && !string.IsNullOrEmpty(_cityName))
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            #endregion
        }

        /// <summary>
        ///     List of suggested cities.
        /// </summary>
        public ReactiveList<City> Suggestions { get; set; }

        /// <summary>
        ///     Command responsible for displaying the <see cref="EditCityViewModel" /> view model.
        /// </summary>
        public ReactiveCommand<object> DisplayCityView { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the <see cref="Suggestions" /> collection.
        /// </summary>
        public ReactiveCommand<List<City>> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the <see cref="Street" /> object to the database.
        /// </summary>
        public ReactiveCommand<Street> AddStreet { get; protected set; }

        /// <summary>
        ///     Property for the <see cref="Domain.Entities.Street" /> being edited in the window.
        /// </summary>
        public Street Street
        {
            get { return _street; }
            set { this.RaiseAndSetIfChanged(ref _street, value); }
        }

        /// <summary>
        ///     Property for the city name supplied by the user, used for displaying suggestions.
        /// </summary>
        public string CityName
        {
            get { return _cityName; }
            set { this.RaiseAndSetIfChanged(ref _cityName, value); }
        }

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
        public MenuOption AssociatedMenuOption => MenuOption.Street;
    }
}