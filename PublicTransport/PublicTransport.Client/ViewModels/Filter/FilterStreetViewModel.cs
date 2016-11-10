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
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering streets.
    /// </summary>
    public class FilterStreetViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Unit of work used in the view model to access the database.
        /// </summary>
        private readonly StreetUnitOfWork _streetUnitOfWork;

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
            _streetUnitOfWork = new StreetUnitOfWork();
            _streetFilter = new StreetFilter();
            Streets = new ReactiveList<Street>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedStreet).Select(s => s != null);

            #region Street filtering command

            FilterStreets = ReactiveCommand.CreateAsyncTask(async _ => { return await Task.Run(() => _streetUnitOfWork.FilterStreets(StreetFilter)); });
            FilterStreets.Subscribe(result =>
            {
                Streets.Clear();
                Streets.AddRange(result);
            });
            FilterStreets.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch street data from the database. Please contact the system administrator.", e));

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
                await Task.Run(() => _streetUnitOfWork.DeleteStreet(SelectedStreet));
                return Unit.Default;
            });
            DeleteStreet.Subscribe(_ => SelectedStreet = null);
            DeleteStreet.InvokeCommand(FilterStreets);
            DeleteStreet.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected city. Please contact the system administrator.", e));

            #endregion

            #region Add/edit street commands

            AddStreet = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditStreetViewModel(screen, _streetUnitOfWork)));
            EditStreet = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditStreetViewModel(screen, _streetUnitOfWork, SelectedStreet)));

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
        ///     Command responsible for filtering out streets in accordance with the <see cref="DataTransfer.StreetFilter" />.
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