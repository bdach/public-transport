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
    ///     View model responsible for filtering streets.
    /// </summary>
    public class FilterStreetViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     <see cref="StreetService" /> used to fetch data from database.
        /// </summary>
        private readonly StreetService _streetService;

        /// <summary>
        ///     <see cref="Street" /> object currently selected in the view.
        /// </summary>
        private Street _selectedStreet;

        /// <summary>
        ///     <see cref="DataTransfer.StreetFilter" /> object containing the query parameters.
        /// </summary>
        private StreetFilter _streetFilter;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display view model on.</param>
        public FilterStreetViewModel(IScreen screen)
        {
            #region Field/property initialization

            HostScreen = screen;
            _streetService = new StreetService();
            _streetFilter = new StreetFilter();
            Streets = new ReactiveList<Street>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedStreet).Select(c => c != null);

            #region Street filtering command

            FilterStreets = ReactiveCommand.CreateAsyncTask(
                async _ => { return await Task.Run(() => _streetService.FilterStreets(StreetFilter)); });
            FilterStreets.Subscribe(result =>
            {
                Streets.Clear();
                Streets.AddRange(result);
            });
            FilterStreets.ThrownExceptions.Subscribe(
                e => UserError.Throw("Cannot fetch street data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered streets upon filter string change

            this.WhenAnyValue(vm => vm.StreetFilter.StreetNameFilter, vm => vm.StreetFilter.CityNameFilter)
                .Where(_ => StreetFilter.IsValid)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterStreets);

            #endregion

            #region Delete street command

            // TODO: Maybe prompt for confirmation?
            DeleteStreet = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await Task.Run(() => _streetService.Delete(SelectedStreet));
                return Unit.Default;
            });
            DeleteStreet.Subscribe(_ => SelectedStreet = null);
            DeleteStreet.InvokeCommand(FilterStreets);
            DeleteStreet.ThrownExceptions.Subscribe(e => UserError.Throw("Cannot delete the selected city. Please contact the system administrator.", e));

            #endregion

            #region Add/edit commands

            AddStreet =
                ReactiveCommand.CreateAsyncObservable(
                    _ => HostScreen.Router.Navigate.ExecuteAsync(new EditStreetViewModel(screen)));
            EditStreet = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem,
                _ => HostScreen.Router.Navigate.ExecuteAsync(new EditStreetViewModel(screen, SelectedStreet)));

            #endregion

            #region Updating the list of streets upon navigating back to this view model

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this && StreetFilter.IsValid)
                .InvokeCommand(FilterStreets);

            #endregion
        }

        /// <summary>
        ///     Reactive list containing the filtered <see cref="Street" /> objects.
        /// </summary>
        public ReactiveList<Street> Streets { get; protected set; }

        /// <summary>
        ///     Command responsible for filtering out streets in accordance with the <see cref="StreetNameFilter" />.
        /// </summary>
        public ReactiveCommand<List<Street>> FilterStreets { get; protected set; }

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
        ///     <see cref="DataTransfer.StreetFilter" /> object containing the query parameters.
        /// </summary>
        public StreetFilter StreetFilter
        {
            get { return _streetFilter; }
            set { this.RaiseAndSetIfChanged(ref _streetFilter, value); }
        }

        /// <summary>
        ///     Property exposing the currently selected <see cref="Street" />.
        /// </summary>
        public Street SelectedStreet
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