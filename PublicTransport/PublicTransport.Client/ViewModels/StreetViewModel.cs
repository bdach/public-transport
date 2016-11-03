using System;
using PublicTransport.Client.Interfaces;
using PublicTransport.Client.Models;
using ReactiveUI;

namespace PublicTransport.Client.ViewModels
{
    public class StreetViewModel : ReactiveObject, IDetailViewModel
    {
        public string UrlPathSegment => AssociatedMenuOption.ToString();
        public IScreen HostScreen { get; }
        public ReactiveCommand<object> DisplayCityView { get; protected set; }

        public StreetViewModel(IScreen screen)
        {
            HostScreen = screen;

            DisplayCityView = ReactiveCommand.Create();
            DisplayCityView.Subscribe(_ =>
            {
                HostScreen.Router.Navigate.Execute(new CityViewModel(screen));
            });
        }

        public MenuOption AssociatedMenuOption => MenuOption.Street;
    }
}