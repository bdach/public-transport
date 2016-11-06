using System;
using System.Collections.Generic;
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
        ///     <see cref="City" /> selected by the user in the drop-down menu.
        /// </summary>
        private City _selectedCity;

        /// <summary>
        ///     <see cref="Domain.Entities.Street" /> objects to be saved to the database.
        /// </summary>
        private Street _street;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="street">Street to be edited. If a steet is to be added, this parameter is null (can be left out).</param>
        public EditStreetViewModel(IScreen screen, Street street = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            Suggestions = new ReactiveList<City>();
            _cityService = new CityService();
            _streetService = new StreetService();
            var serviceMethod = street == null ? new Func<Street, Street>(_streetService.Create) : _streetService.Update;
            _street = street ?? new Street();
            _selectedCity = street?.City;
            _cityName = _street?.City?.Name;

            #endregion

            #region DisplayCityView command

            DisplayCityView = ReactiveCommand.Create();
            DisplayCityView.Subscribe(_ => { HostScreen.Router.Navigate.Execute(new EditCityViewModel(screen)); });

            #endregion

            #region SaveStreet command

            var canSaveStreet = this.WhenAnyValue(vm => vm.SelectedCity).Select(c => c != null);
            SaveStreet = ReactiveCommand.CreateAsyncTask(canSaveStreet, async _ =>
            {
                // TODO: This is needed because of different contexts in this and CityService. Maybe consider grouping services into super-services and injecting context by ctor?
                Street.City = null;
                Street.CityId = SelectedCity.Id;
                var result = await Task.Run(() => serviceMethod(Street));
                Street.City = SelectedCity;
                return result;
            });
            // On exceptions: Display error.
            SaveStreet.ThrownExceptions.Subscribe(
                ex =>
                    UserError.Throw(
                        "The currently edited street cannot be saved to the database. Please contact the system administrator.",
                        ex));

            #endregion

            #region UpdateSuggestions command

            var canUpdateSuggestions = this.WhenAnyValue(vm => vm.CityName,
                s => !string.IsNullOrWhiteSpace(s));
            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(canUpdateSuggestions, async _ =>
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
                .Subscribe(ex => UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying database for suggestions

            this.WhenAnyValue(vm => vm.CityName)
                .Where(e => e != SelectedCity?.Name)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

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
        public ReactiveCommand<Street> SaveStreet { get; protected set; }

        /// <summary>
        ///     Command closing the current detail view model.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; }

        /// <summary>
        ///     Property for the <see cref="Domain.Entities.Street" /> being edited in the window.
        /// </summary>
        public Street Street
        {
            get { return _street; }
            set { this.RaiseAndSetIfChanged(ref _street, value); }
        }

        /// <summary>
        ///     <see cref="City" /> selected by the user in the drop-down menu.
        /// </summary>
        public City SelectedCity
        {
            get { return _selectedCity; }
            set { this.RaiseAndSetIfChanged(ref _selectedCity, value); }
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
        public string UrlPathSegment => "EditStreet";

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