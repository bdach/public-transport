using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Edit
{
    /// <summary>
    ///     View model for editing <see cref="Domain.Entities.Agency" /> objects.
    /// </summary>
    public class EditAgencyViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used to fetch <see cref="Street" /> data from the database.
        /// </summary>
        private readonly StreetService _streetService;

        /// <summary>
        ///     The <see cref="Domain.Entities.Agency" /> object being edited in the window.
        /// </summary>
        private Agency _agency;

        /// <summary>
        ///     The <see cref="Street" /> currently selected by the user.
        /// </summary>
        private Street _selectedStreet;

        /// <summary>
        ///     Filter used to make queries about <see cref="Street" /> objects.
        /// </summary>
        private StreetFilter _streetFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">The screen the view model should appear on.</param>
        /// <param name="agency">Agency to be edited. If an agency is to be added, this parameter should be left null.</param>
        public EditAgencyViewModel(IScreen screen, Agency agency = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            StreetSuggestions = new ReactiveList<Street>();
            _streetService = new StreetService();
            _streetFilter = new StreetFilter();
            var agencyService = new AgencyService();
            var serviceMethod = agency == null ? new Func<Agency, Agency>(agencyService.Create) : agencyService.Update;
            _agency = agency ?? new Agency();
            _selectedStreet = _agency.Street;
            _streetFilter.StreetNameFilter = _agency.Street?.Name ?? "";

            #endregion

            var streetSelected = this.WhenAnyValue(vm => vm.SelectedStreet).Select(s => s != null);

            #region DisplayStreetView command

            DisplayStreetView = ReactiveCommand.CreateAsyncObservable(streetSelected, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditStreetViewModel(screen, SelectedStreet)));

            #endregion

            #region SaveAgency command

            SaveAgency = ReactiveCommand.CreateAsyncTask(streetSelected, async _ =>
            {
                // TODO: Fix this when refactoring services.
                Agency.Street = null;
                Agency.StreetId = SelectedStreet.Id;
                var result = await Task.Run(() => serviceMethod(Agency));
                Agency.Street = SelectedStreet;
                return result;
            });
            SaveAgency.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("The currently edited agency cannot be saved to the database. Please contact the system administrator.", ex));

            #endregion

            #region UpdateSuggestions command

            UpdateSuggestions = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _streetService.FilterStreets(StreetFilter)));
            UpdateSuggestions.Subscribe(results =>
            {
                StreetSuggestions.Clear();
                StreetSuggestions.AddRange(results);
            });
            UpdateSuggestions.ThrownExceptions.Subscribe(ex =>
                UserError.Throw("Cannot fetch suggestions from the database. Please contact the system administrator.", ex));

            #endregion

            #region Querying DB for suggestions

            this.WhenAnyValue(vm => vm.StreetFilter.StreetNameFilter)
                .Where(s => (s != SelectedStreet?.Name) && StreetFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
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
        public ReactiveList<Street> StreetSuggestions { get; protected set; }

        /// <summary>
        ///     Command allowing to edit the <see cref="SelectedStreet" /> object.
        /// </summary>
        public ReactiveCommand<object> DisplayStreetView { get; protected set; }

        /// <summary>
        ///     Command responsible for updating the street suggestions.
        /// </summary>
        public ReactiveCommand<List<Street>> UpdateSuggestions { get; protected set; }

        /// <summary>
        ///     Command responsible for saving the currently edited <see cref="PublicTransport.Domain.Entities.Agency" /> object.
        /// </summary>
        public ReactiveCommand<Agency> SaveAgency { get; protected set; }

        /// <summary>
        ///     Command responsible for closing the window.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; protected set; }

        /// <summary>
        ///     The <see cref="Domain.Entities.Agency" /> object being edited in the window.
        /// </summary>
        public Agency Agency
        {
            get { return _agency; }
            set { this.RaiseAndSetIfChanged(ref _agency, value); }
        }

        /// <summary>
        ///     The <see cref="Street" /> currently selected by the user.
        /// </summary>
        public Street SelectedStreet
        {
            get { return _selectedStreet; }
            set { this.RaiseAndSetIfChanged(ref _selectedStreet, value); }
        }

        /// <summary>
        ///     Filter used to make queries about <see cref="Street" /> objects.
        /// </summary>
        public StreetFilter StreetFilter
        {
            get { return _streetFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetFilter, value); }
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