using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Fares;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering fares.
    /// </summary>
    public class FilterFareViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IFareService _fareService;

        /// <summary>
        ///     <see cref="DataTransfer.FareReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        private FareReactiveFilter _fareReactiveFilter;

        /// <summary>
        ///     The <see cref="FareAttributeDto" /> currently selected by the user.
        /// </summary>
        private FareAttributeDto _selectedFare;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="fareService">Service used in the view model to access the database.</param>
        public FilterFareViewModel(IScreen screen, IFareService fareService = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _fareService = fareService ?? Locator.Current.GetService<IFareService>();
            _fareReactiveFilter = new FareReactiveFilter();
            FareAttributes = new ReactiveList<FareAttributeDto>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedFare).Select(s => s != null);

            #region Fare filtering command

            FilterFares = ReactiveCommand.CreateAsyncTask(async _ => await _fareService.FilterFaresAsync(FareReactiveFilter.Convert()));
            FilterFares.Subscribe(result =>
            {
                FareAttributes.Clear();
                FareAttributes.AddRange(result);
            });
            FilterFares.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch fare data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered fares upon filter change

            this.WhenAnyValue(
                    vm => vm.FareReactiveFilter.RouteNameFilter,
                    vm => vm.FareReactiveFilter.OriginZoneNameFilter,
                    vm => vm.FareReactiveFilter.DestinationZoneNameFilter)
                .Where(_ => FareReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterFares);

            #endregion

            #region Delete fare command

            DeleteFare = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await _fareService.DeleteFareRuleAsync(SelectedFare.FareRule);
                //await _fareService.DeleteFareAttributeAsync(SelectedFare); // commented because of cascade delete
                return Unit.Default;
            });
            DeleteFare.Subscribe(_ => SelectedFare = null);
            DeleteFare.InvokeCommand(FilterFares);
            DeleteFare.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected fare. Please contact the system administrator.", e));

            #endregion

            #region Add/edit fare commands

            AddFare = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditFareViewModel(HostScreen, _fareService)));
            EditFare = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditFareViewModel(HostScreen, _fareService, SelectedFare)));

            #endregion

            #region Updating the list of fares upon navigating back

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && FareReactiveFilter.IsValid)
                .InvokeCommand(FilterFares);

            #endregion
        }

        /// <summary>
        ///     The <see cref="FareAttributeDto" /> currently selected by the user.
        /// </summary>
        public FareAttributeDto SelectedFare
        {
            get { return _selectedFare; }
            set { this.RaiseAndSetIfChanged(ref _selectedFare, value); }
        }

        /// <summary>
        ///     <see cref="DataTransfer.FareReactiveFilter" /> object used to send query data to the service layer.
        /// </summary>
        public FareReactiveFilter FareReactiveFilter
        {
            get { return _fareReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _fareReactiveFilter, value); }
        }

        /// <summary>
        ///     The list of <see cref="FareAttributes" /> objects currently displayed by the user.
        /// </summary>
        public ReactiveList<FareAttributeDto> FareAttributes { get; protected set; }

        /// <summary>
        ///     Fetches <see cref="FareAttribute" /> objects from the database, using the <see cref="FareReactiveFilter" /> object as a query
        ///     parameter.
        /// </summary>
        public ReactiveCommand<FareAttributeDto[]> FilterFares { get; protected set; }

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
