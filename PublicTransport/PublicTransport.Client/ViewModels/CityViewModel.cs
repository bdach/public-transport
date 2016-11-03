using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using PublicTransport.Domain.Entities;
using PublicTransport.Services;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class CityViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "City";
        public IScreen HostScreen { get; }

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

        private readonly CityService _cityService;
        private City _city;
        private string _status;
        public ReactiveCommand<City> AddCity { get; }

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

            this.WhenAnyValue(vm => vm.Status)
                .Throttle(TimeSpan.FromSeconds(3), RxApp.MainThreadScheduler)
                .Subscribe(s => Status = "");
        }
    }
}