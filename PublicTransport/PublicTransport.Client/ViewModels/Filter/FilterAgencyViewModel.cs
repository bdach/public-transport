using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Agencies;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering agencies.
    /// </summary>
    public class FilterAgencyViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IAgencyService _agencyService;

        /// <summary>
        ///     <see cref="DataTransfer.AgencyReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        private AgencyReactiveFilter _agencyReactiveFilter;

        /// <summary>
        ///     The <see cref="Agency" /> currently selected by the user.
        /// </summary>
        private AgencyDto _selectedAgency;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display the view model on.</param>
        /// <param name="agencyService">Service exposing methods necessary to manage data.</param>
        public FilterAgencyViewModel(IScreen screen, IAgencyService agencyService = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _agencyService = agencyService ?? Locator.Current.GetService<IAgencyService>();
            _agencyReactiveFilter = new AgencyReactiveFilter();
            Agencies = new ReactiveList<AgencyDto>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedAgency).Select(a => a != null);

            #region Agency filtering command

            FilterAgencies = ReactiveCommand.CreateAsyncTask(async _ => await _agencyService.FilterAgenciesAsync(AgencyReactiveFilter.Convert()));
            FilterAgencies.Subscribe(result =>
            {
                Agencies.Clear();
                Agencies.AddRange(result);
            });
            FilterAgencies.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch agency data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered agencies upon filter change

            this.WhenAnyValue(vm => vm.AgencyReactiveFilter.AgencyNameFilter, vm => vm.AgencyReactiveFilter.CityNameFilter,
                    vm => vm.AgencyReactiveFilter.StreetNameFilter)
                .Where(_ => AgencyReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5), RxApp.MainThreadScheduler)
                .InvokeCommand(this, vm => vm.FilterAgencies);

            #endregion

            #region Delete agency command

            DeleteAgency = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await _agencyService.DeleteAgencyAsync(SelectedAgency);
                return Unit.Default;
            });
            DeleteAgency.Subscribe(_ => SelectedAgency = null);
            DeleteAgency.InvokeCommand(FilterAgencies);
            DeleteAgency.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected agency. Please contact the system administrator.", e));

            #endregion

            #region Add/edit agency commands

            AddAgency = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditAgencyViewModel(HostScreen, _agencyService)));
            EditAgency = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditAgencyViewModel(HostScreen, _agencyService, SelectedAgency)));

            #endregion

            #region Updating the list of agencies upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && AgencyReactiveFilter.IsValid)
                .InvokeCommand(FilterAgencies);

            #endregion
        }

        /// <summary>
        ///     The <see cref="Agency" /> currently selected by the user.
        /// </summary>
        public AgencyDto SelectedAgency
        {
            get { return _selectedAgency; }
            set { this.RaiseAndSetIfChanged(ref _selectedAgency, value); }
        }

        /// <summary>
        ///     <see cref="DataTransfer.AgencyReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        public AgencyReactiveFilter AgencyReactiveFilter
        {
            get { return _agencyReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _agencyReactiveFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="Agency" /> objects currently displayed to the user.
        /// </summary>
        public ReactiveList<AgencyDto> Agencies { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="Agency" /> objects from the database, using the <see cref="AgencyReactiveFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<AgencyDto[]> FilterAgencies { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for adding a new <see cref="Agency" /> to the database.
        /// </summary>
        public ReactiveCommand<object> AddAgency { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for editing an <see cref="Agency" />.
        /// </summary>
        public ReactiveCommand<object> EditAgency { get; protected set; }

        /// <summary>
        ///     Deletes the currently selected <see cref="Agency" />
        /// </summary>
        public ReactiveCommand<Unit> DeleteAgency { get; protected set; }

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