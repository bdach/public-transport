using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.DataTransfer;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Streets;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;
using Splat;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering streets.
    /// </summary>
    public class FilterStreetViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Service used in the view model to access the database.
        /// </summary>
        private readonly IStreetService _streetService;

        /// <summary>
        ///     <see cref="Street" /> object currently selected in the view.
        /// </summary>
        private StreetDto _selectedStreet;

        /// <summary>
        ///     <see cref="StreetReactiveFilter" /> object containing the query parameters.
        /// </summary>
        private StreetReactiveFilter _streetReactiveFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display view model on.</param>
        /// <param name="streetService">Service used in the view model to access the database.</param>
        public FilterStreetViewModel(IScreen screen, IStreetService streetService = null)
        {
            #region Field/property initialization

            HostScreen = screen;
            _streetService = streetService ?? Locator.Current.GetService<IStreetService>();
            _streetReactiveFilter = new StreetReactiveFilter();
            Streets = new ReactiveList<StreetDto>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedStreet).Select(s => s != null);

            #region Street filtering command

            FilterStreets = ReactiveCommand.CreateAsyncTask(async _ => await _streetService.FilterStreetsAsync(StreetReactiveFilter.Convert()));
            FilterStreets.Subscribe(result =>
            {
                Streets.Clear();
                Streets.AddRange(result);
            });
            FilterStreets.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch street data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered streets upon filter string change

            this.WhenAnyValue(vm => vm.StreetReactiveFilter.StreetNameFilter, vm => vm.StreetReactiveFilter.CityNameFilter)
                .Where(_ => StreetReactiveFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterStreets);

            #endregion

            #region Delete street command

            // TODO: Maybe prompt for confirmation?
            DeleteStreet = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await Task.Run(() => _streetService.DeleteStreetAsync(SelectedStreet));
                return Unit.Default;
            });
            DeleteStreet.Subscribe(_ => SelectedStreet = null);
            DeleteStreet.InvokeCommand(FilterStreets);
            DeleteStreet.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected city. Please contact the system administrator.", e));

            #endregion

            #region Add/edit street commands

            AddStreet = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditStreetViewModel(screen, _streetService)));
            EditStreet = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditStreetViewModel(screen, _streetService, SelectedStreet)));

            #endregion

            #region Updating the list of streets upon navigating back to this view model

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && StreetReactiveFilter.IsValid)
                .InvokeCommand(FilterStreets);

            #endregion
        }

        /// <summary>
        ///     Reactive list containing the filtered <see cref="Street" /> objects.
        /// </summary>
        public ReactiveList<StreetDto> Streets { get; protected set; }

        /// <summary>
        ///     Command responsible for filtering out streets in accordance with the <see cref="StreetReactiveFilter" />.
        /// </summary>
        public ReactiveCommand<StreetDto[]> FilterStreets { get; protected set; }

        /// <summary>
        ///     Command responsible for launching the street adding view.
        /// </summary>
        public ReactiveCommand<object> AddStreet { get; protected set; }

        /// <summary>
        ///     Command responsible for launching the street editing view.
        /// </summary>
        public ReactiveCommand<object> EditStreet { get; protected set; }

        /// <summary>
        ///     Command responsible for deleting the street.
        /// </summary>
        public ReactiveCommand<Unit> DeleteStreet { get; protected set; }

        /// <summary>
        ///     <see cref="StreetReactiveFilter" /> object containing the query parameters.
        /// </summary>
        public StreetReactiveFilter StreetReactiveFilter
        {
            get { return _streetReactiveFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetReactiveFilter, value); }
        }

        /// <summary>
        ///     Property exposing the currently selected <see cref="Street" />.
        /// </summary>
        public StreetDto SelectedStreet
        {
            get { return _selectedStreet; }
            set { this.RaiseAndSetIfChanged(ref _selectedStreet, value); }
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