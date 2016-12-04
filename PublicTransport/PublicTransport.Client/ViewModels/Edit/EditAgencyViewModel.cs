using System;
using System.Reactive;
using System.Reactive.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Agencies;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using PublicTransport.Services.Exceptions;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing <see cref="Domain.Entities.Agency" /> objects.
    /// </summary>
    public class EditAgencyViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IAgencyService _agencyService;

        /// <summary>
        ///     The <see cref="Domain.Entities.Agency" /> object being edited in the window.
        /// </summary>
        private AgencyDto _agency;

        /// <summary>
        ///     The <see cref="Street" /> currently selected by the user.
        /// </summary>
        private StreetDto _selectedStreet;

        /// <summary>
        ///     Filter used to make queries about <see cref="Street" /> objects.
        /// </summary>
        private StreetReactiveFilter _streetReactiveFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="agencyService">Service exposing methods necessary to manage data.</param>
        /// <param name="agency">Agency to be edited. If an agency is to be added, this parameter should be left null.</param>
        public EditAgencyViewModel(IScreen screen, IAgencyService agencyService, AgencyDto agency = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            StreetSuggestions = new ReactiveList<StreetDto>();
            _agencyService = agencyService;
            var serviceMethod = agency == null ? new Func<AgencyDto, Task<AgencyDto>>(_agencyService.CreateAgencyAsync) : _agencyService.UpdateAgencyAsync;
            _agency = agency ?? new AgencyDto();
            _selectedStreet = _agency.Street;
            _streetReactiveFilter = new StreetReactiveFilter { StreetNameFilter = _agency.Street?.Name ?? "" };

            #endregion

            var streetSelected = this.WhenAnyValue(vm => vm.SelectedStreet).Select(s => s != null);
            streetSelected.Where(b => b).Subscribe(_ => Agency.Street = SelectedStreet);

            #region SaveAgency command

            SaveAgency = ReactiveCommand.CreateAsyncTask(streetSelected, async _ => await serviceMethod(Agency));
            SaveAgency.ThrownExceptions
                .Where(ex => !(ex is FaultException<ValidationFault>))
                .Subscribe(ex =>
                    UserError.Throw("Cannot connect to the server. Please try again later.", ex));
            SaveAgency.ThrownExceptions
                .Where(ex => ex is FaultException<ValidationFault>)
                .Select(ex => ex as FaultException<ValidationFault>)
                .Subscribe(ex => UserError.Throw(string.Join("\n", ex.Detail.Errors), ex));

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await _agencyService.FilterStreetsAsync(StreetReactiveFilter.Convert()));
            UpdateSuggestions.Subscribe(results =>
            {
                StreetSuggestions.Clear();
                StreetSuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.StreetReactiveFilter.StreetNameFilter)
                .Where(s => (s != SelectedStreet?.Name) && StreetReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.UpdateSuggestions);

            #endregion

            #region Close command

            // On activation, go back one step in the navigation stack.
            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     List containing the suggested <see cref="Street" />s based on user input.
        /// </summary>
        public ReactiveList<StreetDto> StreetSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the street suggestions.
        /// </summary>
        public ReactiveCommand<StreetDto[]> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="PublicTransport.Domain.Entities.Agency" /> object.
        /// </summary>
        public ReactiveCommand<AgencyDto> SaveAgency { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The <see cref="Domain.Entities.Agency" /> object being edited in the window.
        /// </summary>
        public AgencyDto Agency
        {
            get { return _agency; }
            set { this.RaiseAndSetIfChanged(ref _agency, value); }
        }

        /// <summary>
        ///     The <see cref="Street" /> currently selected by the user.
        /// </summary>
        public StreetDto SelectedStreet
        {
            get { return _selectedStreet; }
            set { this.RaiseAndSetIfChanged(ref _selectedStreet, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Street" /> objects.
        /// </summary>
        public StreetReactiveFilter StreetReactiveFilter
        {
            get { return _streetReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetReactiveFilter, value); }
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
        public MenuOption AssociatedMenuOption => MenuOption.Agency;
    }
}