using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class FilterCityViewModel : ReactiveObject, IDetailViewModel
    {
        private readonly CityService _cityService;
        private string _nameFilter;

        public FilterCityViewModel(IScreen screen)
        {
            HostScreen = screen;
            _cityService = new CityService();
            Cities = new ReactiveList<City>();

            FilterCities = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                return await Task.Run(() => _cityService.GetCitiesContainingString(NameFilter));
            });
            FilterCities.Subscribe(result =>
            {
                Cities.Clear();
                Cities.AddRange(result);
            });

            this.WhenAnyValue(vm => vm.NameFilter)
                .Where(s => !string.IsNullOrEmpty(s))
                .Throttle(TimeSpan.FromSeconds(0.5))
                .InvokeCommand(this, vm => vm.FilterCities);
        }

        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public MenuOption AssociatedMenuOption => MenuOption.City;
        public ReactiveList<City> Cities { get; set; }
        public ReactiveCommand<List<City>> FilterCities { get; protected set; }
        public ReactiveCommand<object> EditCity { get; protected set; }

        public string NameFilter
        {
            get { return _nameFilter; }
            set { this.RaiseAndSetIfChanged(ref _nameFilter, value); }
        }
    }
}