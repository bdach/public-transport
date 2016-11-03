using System;
using PublicTransport.Client.Models;
using PublicTransport.Client.ViewModels;
using ReactiveUI;

namespace PublicTransport.Client.Providers
{
    public class RoutableViewModelFactory
    {
        public IRoutableViewModel GetViewModel(IScreen screen, MenuOption option)
        {
            switch (option)
            {
                case MenuOption.City:
                    return new CityViewModel(screen);
                case MenuOption.Street:
                    return new StreetViewModel(screen);
                default:
                    throw new InvalidOperationException("Could not locate view model for this option");
            }
        }
    }
}
