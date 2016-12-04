using System;
using System.Reactive;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Streets;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for saving <see cref="Domain.Entities.Street" /> objects to the database.
    /// </summary>
    public class EditStreetViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IStreetService _streetService;

        /// <summary>
        ///     City name supplied by the user. This field is used to supply suggestions.
        /// </summary>
        private string _cityName;

        /// <summary>
        ///     <see cref="City" /> object currently selected by the user.
        /// </summary>
        private CityDto _selectedCity;

        /// <summary>
        ///     <see cref="Domain.Entities.Street" /> objects to be saved to the database.
        /// </summary>
        private StreetDto _street;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="streetService">Service exposing methods necessary to manage data.</param>
        /// <param name="street">Street to be edited. If a steet is to be added, this parameter is null (can be left out).</param>
        public EditStreetViewModel(IScreen screen, IStreetService streetService, StreetDto street = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            Suggestions = new ReactiveList<CityDto>();
            _streetService = streetService;
            var serviceMethod = street == null ? new Func<StreetDto, Task<StreetDto>>(_streetService.CreateStreetAsync) : _streetService.UpdateStreetAsync;
            _street = street ?? new StreetDto();
            _selectedCity = _street.City;
            _cityName = _street.City?.Name;

            #endregion

            var citySelected = this.WhenAnyValue(vm => vm.SelectedCity).Select(c => c != null);
            citySelected.Where(b => b).Subscribe(_ => Street.City = SelectedCity);

            #region SaveStreet command

            SaveStreet = ReactiveCommand.CreateAsyncTask(citySelected, async _ => await serviceMethod(Street));
            SaveStreet.ThrownExceptions
                .Where(ex => !(ex is FaultException<ValidationFault>))
                .Subscribe(ex =>
                    UserError.Throw("Cannot connect to the server. Please try again later.", ex));
            SaveStreet.ThrownExceptions
                .Where(ex => ex is FaultException<ValidationFault>)
                .Select(ex => ex as FaultException<ValidationFault>)
                .Subscribe(ex => UserError.Throw(string.Join("\n", ex.Detail.Errors), ex));

            #endregion

            var canUpdateSuggestions = this.WhenAnyValue(vm => vm.CityName, s => !string.IsNullOrWhiteSpace(s));

            #region UpdateSuggestions command

            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(canUpdateSuggestions, async _ => await _streetService.FilterCitiesAsync(CityName));
            UpdateSuggestions.Subscribe(results =>
            {
                Suggestions.Clear();
                Suggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

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
        ///     List of suggested <see cref="City"/> objects.
        /// </summary>
        public ReactiveList<CityDto> Suggestions { get; set; }

        /// <summary>
        ///     Command responsible for updating the <see cref="Suggestions" /> collection.
        /// </summary>
        public ReactiveCommand<CityDto[]> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the <see cref="Street" /> object to the database.
        /// </summary>
        public ReactiveCommand<StreetDto> SaveStreet { get; protected set; }

        /// <summary>
        ///     Command closing the current detail view model.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; }

        /// <summary>
        ///     Property for the <see cref="Domain.Entities.Street" /> being edited in the window.
        /// </summary>
        public StreetDto Street
        {
            get { return _street; }
            set { this.RaiseAndSetIfChanged(ref _street, value); }
        }

        /// <summary>
        ///     <see cref="City" /> selected by the user in the drop-down menu.
        /// </summary>
        public CityDto SelectedCity
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