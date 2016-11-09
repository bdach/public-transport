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
    ///     View model responsible for filtering fares.
    /// </summary>
    public class FilterFareViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used to fetch <see cref="FareAttribute" /> data from the database.
        /// </summary>
        private readonly FareAttributeService _fareAttributeService;

        /// <summary>
        ///     Service used to fetch <see cref="FareRule" /> data from the database.
        /// </summary>
        private readonly FareRuleService _fareRuleService;

        /// <summary>
        ///     <see cref="DataTransfer.FareFilter" /> object used to send query data to the service layer.
        /// </summary>
        private FareFilter _fareFilter;

        /// <summary>
        ///     The <see cref="FareAttribute" /> currently selected by the user.
        /// </summary>
        private FareAttribute _selectedFare;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen"></param>
        public FilterFareViewModel(IScreen screen)
        {
            #region Field/property initialization

            HostScreen = screen;
            _fareAttributeService = new FareAttributeService();
            _fareRuleService = new FareRuleService();
            _fareFilter = new FareFilter();
            FareAttributes = new ReactiveList<FareAttribute>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedFare).Select(s => s != null);

            #region Stop filtering command

            FilterFares = ReactiveCommand.CreateAsyncTask(async _ => await Task.Run(() => _fareAttributeService.FilterFares(FareFilter)));
            FilterFares.Subscribe(result =>
            {
                FareAttributes.Clear();
                FareAttributes.AddRange(result);
            });
            FilterFares.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch fare data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered stops upon filter change

            this.WhenAnyValue(
                    vm => vm.FareFilter.RouteNameFilter,
                    vm => vm.FareFilter.OriginZoneNameFilter,
                    vm => vm.FareFilter.DestinationZoneNameFilter)
                .Where(_ => FareFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterFares);

            #endregion

            #region Delete stop command

            DeleteFare = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await Task.Run(() => _fareRuleService.Delete(SelectedFare.FareRule));
                //await Task.Run(() => _fareAttributeService.Delete(SelectedFare)); // commented because of cascade delete
                return Unit.Default;
            });
            DeleteFare.Subscribe(_ => SelectedFare = null);
            DeleteFare.InvokeCommand(FilterFares);
            DeleteFare.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected fare. Please contact the system administrator.", e));

            #endregion

            #region Add/edit fare commands

            AddFare = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditFareViewModel(HostScreen)));
            EditFare = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditFareViewModel(HostScreen, SelectedFare)));

            #endregion

            #region Updating the list of stops upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && FareFilter.IsValid)
                .InvokeCommand(FilterFares);

            #endregion
        }

        /// <summary>
        ///     The <see cref="Stop" /> currently selected by the user.
        /// </summary>
        public FareAttribute SelectedFare
        {
            get { return _selectedFare; }
            set { this.RaiseAndSetIfChanged(ref _selectedFare, value); }
        }

        /// <summary>
        ///     <see cref="DataTransfer.FareFilter" /> object used to send query data to the service layer.
        /// </summary>
        public FareFilter FareFilter
        {
            get { return _fareFilter; }
            set { this.RaiseAndSetIfChanged(ref _fareFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="FareAttributes" /> objects currently displayed by the user.
        /// </summary>
        public ReactiveList<FareAttribute> FareAttributes { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="FareAttribute" /> objects from the database, using the <see cref="FareFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<List<FareAttribute>> FilterFares { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for adding a new <see cref="FareAttribute" /> and <see cref="FareRule"/> to the database.
        /// </summary>
        public ReactiveCommand<object> AddFare { get; protected set; }

        /// <summary>
        ///     Opens a view responsible for editing a <see cref="FareAttribute" /> and <see cref="FareRule"/>.
        /// </summary>
        public ReactiveCommand<object> EditFare { get; protected set; }

        /// <summary>
        ///     Deletes the currently selected <see cref="FareAttribute" /> and <see cref="FareRule"/>.
        /// </summary>
        public ReactiveCommand<Unit> DeleteFare { get; protected set; }

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
        public MenuOption AssociatedMenuOption => MenuOption.Fare;
    }
}
