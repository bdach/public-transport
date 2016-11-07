using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering agencies.
    /// </summary>
    public class FilterAgencyViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     <see cref="AgencyService" /> used to fetch data from database.
        /// </summary>
        private readonly AgencyService _agencyService;

        /// <summary>
        ///     <see cref="DataTransfer.AgencyFilter" /> object used to send query data to the service layer.
        /// </summary>
        private AgencyFilter _agencyFilter;

        /// <summary>
        ///     The <see cref="Agency" /> currently selected by the user.
        /// </summary>
        private Agency _selectedAgency;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display the view model on.</param>
        public FilterAgencyViewModel(IScreen screen)
        {
            #region Field/property initialization

            HostScreen = screen;
            _agencyService = new AgencyService();
            _agencyFilter = new AgencyFilter();
            Agencies = new ReactiveList<Agency>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedAgency).Select(a => a != null);

            #region Agency filtering command

            FilterAgencies =
                ReactiveCommand.CreateAsyncTask(
                    async _ => await Task.Run(() => _agencyService.FilterAgencies(AgencyFilter)));
            FilterAgencies.Subscribe(result =>
            {
                Agencies.Clear();
                Agencies.AddRange(result);
            });
            FilterAgencies.ThrownExceptions.Subscribe(
                e =>
                    UserError.Throw(
                        "Cannot fetch agency data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered agencies upon filter change

            this.WhenAnyValue(vm => vm.AgencyFilter.AgencyNameFilter, vm => vm.AgencyFilter.CityNameFilter,
                    vm => vm.AgencyFilter.StreetNameFilter)
                .Where(_ => AgencyFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterAgencies);

            #endregion

            #region Delete agency command

            DeleteAgency = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem,
                async _ =>
                {
                    await Task.Run(() => _agencyService.Delete(SelectedAgency));
                    return Unit.Default;
                });
            DeleteAgency.Subscribe(_ => SelectedAgency = null);
            DeleteAgency.InvokeCommand(FilterAgencies);
            DeleteAgency.ThrownExceptions.Subscribe(
                e => UserError.Throw("Cannot delete the selected agency. Please contact the system administrator.", e));

            #endregion

            AddAgency =
                ReactiveCommand.CreateAsyncObservable(
                    _ => HostScreen.Router.Navigate.ExecuteAsync(new EditAgencyViewModel(HostScreen)));
            EditAgency = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem,
                _ => HostScreen.Router.Navigate.ExecuteAsync(new EditAgencyViewModel(HostScreen, SelectedAgency)));

            #region Updating the list of agencies upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && AgencyFilter.IsValid)
                .InvokeCommand(FilterAgencies);

            #endregion
        }

        /// <summary>
        ///     The <see cref="Agency" /> currently selected by the user.
        /// </summary>
        public Agency SelectedAgency
        {
            get { return _selectedAgency; }
            set { this.RaiseAndSetIfChanged(ref _selectedAgency, value); }
        }

        /// <summary>
        ///     <see cref="DataTransfer.AgencyFilter" /> object used to send query data to the service layer.
        /// </summary>
        public AgencyFilter AgencyFilter
        {
            get { return _agencyFilter; }
            set { this.RaiseAndSetIfChanged(ref _agencyFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="Agency" /> objects currently displayed to the user.
        /// </summary>
        public ReactiveList<Agency> Agencies { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="Agency" /> objects from the database, using the <see cref="AgencyFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<List<Agency>> FilterAgencies { get; protected set; }

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