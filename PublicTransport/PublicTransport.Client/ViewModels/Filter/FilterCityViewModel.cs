﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels.Edit;
using PublicTransport.Domain.Entities;
using PublicTransport.Services.UnitsOfWork;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Filter
{
    /// <summary>
    ///     View model responsible for filtering cities.
    /// </summary>
    public class FilterCityViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Unit of work used in the view model to access the database.
        /// </summary>
        private readonly CityUnitOfWork _cityUnitOfWork;

        /// <summary>
        ///     String containing the city name filter.
        /// </summary>
        private string _nameFilter;

        /// <summary>
        ///     <see cref="City" /> object currently selected in the view.
        /// </summary>
        private City _selectedCity;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="screen">Screen to display view model on.</param>
        public FilterCityViewModel(IScreen screen)
        {
            #region Field/property initialization

            HostScreen = screen;
            _cityUnitOfWork = new CityUnitOfWork();
            Cities = new ReactiveList<City>();

            #endregion

            var canExecuteOnSelectedItem = this.WhenAnyValue(vm => vm.SelectedCity).Select(c => c != null);

            #region City filtering command

            FilterCities = ReactiveCommand.CreateAsyncTask(async _ => { return await Task.Run(() => _cityUnitOfWork.FilterCities(NameFilter)); });
            FilterCities.Subscribe(result =>
            {
                Cities.Clear();
                Cities.AddRange(result);
            });
            FilterCities.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot fetch city data from the database. Please contact the system administrator.", e));

            #endregion

            #region Updating the list of filtered cities upon filter string change

            this.WhenAnyValue(vm => vm.NameFilter)
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterCities);

            #endregion

            #region Delete city command

            // TODO: Maybe prompt for confirmation?
            DeleteCity = ReactiveCommand.CreateAsyncTask(canExecuteOnSelectedItem, async _ =>
            {
                await Task.Run(() => _cityUnitOfWork.DeleteCity(SelectedCity));
                return Unit.Default;
            });
            DeleteCity.Subscribe(_ => SelectedCity = null);
            DeleteCity.InvokeCommand(FilterCities);
            DeleteCity.ThrownExceptions.Subscribe(e =>
                UserError.Throw("Cannot delete the selected city. Please contact the system administrator.", e));

            #endregion

            #region Add/edit city commands

            AddCity = ReactiveCommand.CreateAsyncObservable(_ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditCityViewModel(screen, _cityUnitOfWork)));
            EditCity = ReactiveCommand.CreateAsyncObservable(canExecuteOnSelectedItem, _ =>
                HostScreen.Router.Navigate.ExecuteAsync(new EditCityViewModel(screen, _cityUnitOfWork, SelectedCity)));

            #endregion

            #region Updating the list of cities upon navigating back to this view model

            HostScreen.Router.NavigateBack
                .Where(_ => HostScreen.Router.NavigationStack.Last() == this)
                .InvokeCommand(FilterCities);

            #endregion
        }

        /// <summary>
        ///     Reactive list containing the filtered <see cref="City" /> objects.
        /// </summary>
        public ReactiveList<City> Cities { get; protected set; }

        /// <summary>
        ///     Command responsible for filtering out cities in accordance with the <see cref="NameFilter" />.
        /// </summary>
        public ReactiveCommand<List<City>> FilterCities { get; protected set; }

        /// <summary>
        ///     Command responsible for launching the city editing view.
        /// </summary>
        public ReactiveCommand<object> EditCity { get; protected set; }

        /// <summary>
        ///     Command responsible for deleting the city.
        /// </summary>
        public ReactiveCommand<Unit> DeleteCity { get; protected set; }

        /// <summary>
        ///     Command responsible for launching the city adding view.
        /// </summary>
        public ReactiveCommand<object> AddCity { get; protected set; }

        /// <summary>
        ///     Property containing the name filter.
        /// </summary>
        public string NameFilter
        {
            get { return _nameFilter; }
            set { this.RaiseAndSetIfChanged(ref _nameFilter, value); }
        }

        /// <summary>
        ///     Property exposing the currently selected <see cref="City" />.
        /// </summary>
        public City SelectedCity
        {
            get { return _selectedCity; }
            set { this.RaiseAndSetIfChanged(ref _selectedCity, value); }
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
        public MenuOption AssociatedMenuOption => MenuOption.City;
    }
}