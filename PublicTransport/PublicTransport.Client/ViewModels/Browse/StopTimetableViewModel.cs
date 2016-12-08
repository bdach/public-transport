using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using PublicTransport.Client.Services.Stops;
using PublicTransport.Services.DataTransfer;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels.Browse
{
    /// <summary>
    ///     Displays a timetable for a given stop.
    /// </summary>
    public class StopTimetableViewModel : ReactiveObject, IDetailViewModel
    {
        /// <summary>
        ///     Instance of <see cref="IStopService"/> used to fetch timetable data.
        /// </summary>
        private readonly IStopService _stopService;

        /// <summary>
        ///     The route currently selected by the user.
        /// </summary>
        private RouteDto _selectedRoute;

        /// <summary>
        ///     The stop for which the timetable is displayed.
        /// </summary>
        private StopDto _stop;

        /// <summary>
        ///     Dictionary containing timetable data.
        /// </summary>
        private Dictionary<RouteDto, StopTimeDto[]> _timetable;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="screen">Screen to display on.</param>
        /// <param name="stopService">Instance of <see cref="IStopService"/> used to fetch timetable data.</param>
        /// <param name="stop">The stop for which the timetable is displayed.</param>
        public StopTimetableViewModel(IScreen screen, IStopService stopService, StopDto stop)
        {
            #region Property/field initialization

            HostScreen = screen;
            Stop = stop;
            _stopService = stopService;
            Routes = new ReactiveList<RouteDto>();
            StopTimes = new ReactiveList<StopTimeDto>();

            #endregion

            #region Getting routes and times

            GetTimetable = ReactiveCommand.CreateAsyncTask(async _ => await _stopService.GetStopTimetableAsync(Stop.Id));
            GetTimetable.Subscribe(results =>
            {
                Timetable = results;
                Routes.Clear();
                Routes.AddRange(results.Keys);
            });
            GetTimetable.ThrownExceptions.Subscribe(
                ex => UserError.Throw("Cannot connect to server. Please try again later."));

            #endregion

            #region Showing stop times for routes

            this.WhenAnyValue(vm => vm.SelectedRoute)
                .Where(r => r != null)
                .Subscribe(r =>
                {
                    StopTimes.Clear();
                    StopTimes.AddRange(Timetable?[r]);
                });

            #endregion

            #region Closing the view

            Close = ReactiveCommand.CreateAsyncObservable(_ => HostScreen.Router.NavigateBack.ExecuteAsync());

            #endregion
        }

        /// <summary>
        ///     Command responsible for closing the view.
        /// </summary>
        public ReactiveCommand<Unit> Close { get; set; }

        /// <summary>
        ///     Fetches the timetable contents, using the provided service object.
        /// </summary>
        public ReactiveCommand<Dictionary<RouteDto, StopTimeDto[]>> GetTimetable { get; set; }

        /// <summary>
        ///     Dictionary containing timetable data.
        /// </summary>
        public Dictionary<RouteDto, StopTimeDto[]> Timetable
        {
            get { return _timetable; }
            set { this.RaiseAndSetIfChanged(ref _timetable, value); }
        }

        /// <summary>
        ///     List of routes passing through the currently viewed stop.
        /// </summary>
        public ReactiveList<RouteDto> Routes { get; protected set; }

        /// <summary>
        ///     List of stop times for the selected route and the currently viewed stop.
        /// </summary>
        public ReactiveList<StopTimeDto> StopTimes { get; protected set; }

        /// <summary>
        ///     The stop for which the timetable is displayed.
        /// </summary>
        public StopDto Stop
        {
            get { return _stop; }
            set { this.RaiseAndSetIfChanged(ref _stop, value); }
        }

        /// <summary>
        ///     The route currently selected by the user.
        /// </summary>
        public RouteDto SelectedRoute
        {
            get { return _selectedRoute; }
            set { this.RaiseAndSetIfChanged(ref _selectedRoute, value); }
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
        public MenuOption AssociatedMenuOption => MenuOption.Stop;
    }
}