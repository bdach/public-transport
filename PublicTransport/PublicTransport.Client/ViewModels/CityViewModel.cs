using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class CityViewModel : ReactiveObject, IDetailViewModel
    {
        private readonly CityService _cityService;
        private City _city;
        private string _status;

        public CityViewModel(IScreen screen)
        {
            HostScreen = screen;
            City = new City();
            _cityService = new CityService();

            AddCity = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var city = await Task.Run(() => _cityService.Create(City));
                return city;
            });

            AddCity.Subscribe(city => Status = $"City {city.Name} saved to database.");
            AddCity.ThrownExceptions.Subscribe(ex => UserError.Throw("Cannot connect to database", ex));

            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            this.WhenAnyValue(vm => vm.Status)
                .Throttle(TimeSpan.FromSeconds(3), RxApp.MainThreadScheduler)
                .Subscribe(s => Status = "");
        }

        public City City
        {
            get { return _city; }
            set { this.RaiseAndSetIfChanged(ref _city, value); }
        }

        public string Status
        {
            get { return _status; }
            set { this.RaiseAndSetIfChanged(ref _status, value); }
        }

        public ReactiveCommand<City> AddCity { get; }
        public ReactiveCommand<Unit> Close { get; }
        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public MenuOption AssociatedMenuOption => MenuOption.City;
    }
}